using UrbanIndicatorsSystem.Services;
using UrbanIndicatorsSystem.Data;
using UrbanIndicatorsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using Asp.Versioning;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// 1️⃣  DATABASES (SQLite for Auth, PostgreSQL for Business)
// ======================================================

// --- SQLite (Identity) ---
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

// --- PostgreSQL (Traffic data) ---
if (builder.Environment.EnvironmentName != "Test")
{
    builder.Services.AddDbContext<TrafficDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
}

// ======================================================
// 2️⃣  REDIS (Caching + Sessions)
// ======================================================
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

// ======================================================
// 3️⃣  API VERSIONING
// ======================================================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddMvc();

// ======================================================
// 4️⃣  CORS (для React)
// ======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ======================================================
// 5️⃣  DEPENDENCY INJECTION
// ======================================================
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITrafficService, TrafficService>();
builder.Services.AddScoped<IAreaService, AreaService>();

// Фоновий сервіс (оновлення трафіку)
if (builder.Environment.EnvironmentName != "Test")
{
    builder.Services.AddHostedService<TrafficUpdateService>();
}

// ======================================================
// 6️⃣  BUILD APPLICATION
// ======================================================
var app = builder.Build();

// ======================================================
// 7️⃣  SEED INITIAL DATA
// ======================================================
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

// ======================================================
// 8️⃣  MIDDLEWARE PIPELINE
// ======================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "client", "build")),
    RequestPath = ""
});

app.MapFallbackToFile("index.html");

// Якщо Redis увімкнений — активуємо сесії
if (builder.Configuration.GetSection("Redis").GetValue<bool>("Enabled"))
{
    app.UseSession();
}

app.UseCors("AllowReact");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// Якщо хочеш — можна зробити редірект з кореня
app.MapGet("/", () => Results.Redirect("/Index"));

app.Run();

// ======================================================
// 9️⃣  PARTIAL CLASS FOR TESTING
// ======================================================
public partial class Program { }