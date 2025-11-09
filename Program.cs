using UrbanIndicatorsSystem.Services;
using UrbanIndicatorsSystem.Data;
using UrbanIndicatorsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// ========================
// 1) IDENTITY + SQLITE
// ========================
var sqliteConnection = builder.Configuration.GetConnectionString("Sqlite") 
    ?? throw new InvalidOperationException("Sqlite connection string not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(sqliteConnection));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// ========================
// 2) BUSINESS DB → POSTGRES
// ========================
if (builder.Environment.EnvironmentName != "Test")
{
    builder.Services.AddDbContext<TrafficDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
}

// ========================
// 3) REDIS (Session + Cache)
// ========================
if (builder.Configuration.GetSection("Redis").GetValue<bool>("Enabled"))
{
    var redisHost = builder.Configuration["Redis:Host"];
    var redisPort = builder.Configuration["Redis:Port"];
    var redis = ConnectionMultiplexer.Connect($"{redisHost}:{redisPort}");
    builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = $"{redisHost}:{redisPort}";
        options.InstanceName = builder.Configuration["Redis:InstanceName"];
    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
}

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddMvc();

// ========================
// DISCOVERY + CONTROLLERS
// ========================
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// BUSINESS LOGIC
builder.Services.AddScoped<ITrafficService, TrafficService>(); // now will use TrafficDbContext

// Реєструємо фоновий сервіс тільки не в тестовому режимі
if (builder.Environment.EnvironmentName != "Test")
{
    builder.Services.AddHostedService<TrafficUpdateService>();
}

var app = builder.Build();

// Seed data only if not running tests
if (app.Environment.EnvironmentName != "Test")
{
    using (var scope = app.Services.CreateScope())
    {
        var trafficContext = scope.ServiceProvider.GetRequiredService<TrafficDbContext>();
        trafficContext.Database.EnsureCreated();
        
        if (!trafficContext.Areas.Any())
        {
            var area = new Area { Name = "Downtown" };
            trafficContext.Areas.Add(area);
            trafficContext.SaveChanges();
            
            trafficContext.TrafficData.AddRange(
            new TrafficData { RoadName = "Main Street", TrafficLevel = "High", AreaId = area.Id, Timestamp = DateTime.Now },
            new TrafficData { RoadName = "Broadway", TrafficLevel = "Medium", AreaId = area.Id, Timestamp = DateTime.Now },
            new TrafficData { RoadName = "5th Avenue", TrafficLevel = "Low", AreaId = area.Id, Timestamp = DateTime.Now }
        );
        trafficContext.SaveChanges();
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

if (builder.Configuration.GetSection("Redis").GetValue<bool>("Enabled"))
{
    app.UseSession();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapGet("/", () => Results.Redirect("/Index"));

app.Run();

public partial class Program { }