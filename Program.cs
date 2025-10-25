using UrbanIndicatorsSystem.Services;
using UrbanIndicatorsSystem.Data;
using UrbanIndicatorsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

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
builder.Services.AddDbContext<TrafficDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

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

// ========================
// DISCOVERY + CONTROLLERS
// ========================
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// BUSINESS LOGIC
builder.Services.AddScoped<ITrafficService, TrafficService>(); // now will use TrafficDbContext

var app = builder.Build();

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