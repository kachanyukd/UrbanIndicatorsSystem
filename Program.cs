using UrbanIndicatorsSystem.Services;
using UrbanIndicatorsSystem.Data;
using UrbanIndicatorsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

// ========================
// 2) BUSINESS DB → POSTGRES
// ========================
if (builder.Environment.EnvironmentName != "Test")
{
    var postgresConnection = builder.Configuration.GetConnectionString("Postgres")
        ?? throw new InvalidOperationException("Postgres connection string not found.");

    builder.Services.AddDbContext<TrafficDbContext>(options =>
        options.UseNpgsql(postgresConnection));
}

// ========================
// 3) REDIS (Session + Cache) with error handling
// ========================
var redisEnabled = builder.Configuration.GetSection("Redis").GetValue<bool>("Enabled", true);
if (redisEnabled)
{
    try
    {
        var redisHost = builder.Configuration["Redis:Host"] ?? "localhost";
        var redisPort = builder.Configuration["Redis:Port"] ?? "6379";
        var redisInstanceName = builder.Configuration["Redis:InstanceName"] ?? "UrbanIndicators_";
        var redisConfig = $"{redisHost}:{redisPort},abortConnect=false,connectTimeout=5000,connectRetry=3";
        
        Console.WriteLine($"🔄 Connecting to Redis: {redisHost}:{redisPort}");
        
        var redis = ConnectionMultiplexer.Connect(redisConfig);
        builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig;
            options.InstanceName = redisInstanceName;
        });

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        
        Console.WriteLine("✅ Redis connected successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Redis connection failed: {ex.Message}");
        Console.WriteLine("📌 Running without Redis cache (using memory cache)...");
        builder.Services.AddDistributedMemoryCache();
        redisEnabled = false;
    }
}
else
{
    Console.WriteLine("ℹ️ Redis is disabled, using memory cache");
    builder.Services.AddDistributedMemoryCache();
}

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ========================
// DISCOVERY + CONTROLLERS
// ========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// BUSINESS LOGIC
builder.Services.AddScoped<ITrafficService, TrafficService>();

// Background service (not in tests)
if (builder.Environment.EnvironmentName != "Test")
{
    var trafficUpdateEnabled = builder.Configuration.GetSection("TrafficUpdate").GetValue<bool>("Enabled", true);
    if (trafficUpdateEnabled)
    {
        builder.Services.AddHostedService<TrafficUpdateService>();
    }
}

var app = builder.Build();

// ========================
// DATABASE INITIALIZATION with migrations
// ========================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // 1. Initialize Identity DB (SQLite) with migrations
    try
    {
        Console.WriteLine("🔄 Initializing Identity database...");
        var identityContext = services.GetRequiredService<ApplicationDbContext>();
        await identityContext.Database.MigrateAsync();
        Console.WriteLine("✅ Identity database initialized");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error initializing Identity database: {ex.Message}");
        Console.WriteLine($"   Stack: {ex.StackTrace}");
    }
    
    // 2. Initialize Traffic DB (Postgres) with migrations
    if (app.Environment.EnvironmentName != "Test")
    {
        try
        {
            Console.WriteLine("🔄 Initializing Traffic database...");
            var trafficContext = services.GetRequiredService<TrafficDbContext>();
            await trafficContext.Database.MigrateAsync();
            
            if (!trafficContext.Areas.Any())
            {
                var area = new Area { Name = "Downtown" };
                trafficContext.Areas.Add(area);
                await trafficContext.SaveChangesAsync();
                
                trafficContext.TrafficData.AddRange(
                    new TrafficData { RoadName = "Main Street", TrafficLevel = "High", AreaId = area.Id, Timestamp = DateTime.UtcNow },
                    new TrafficData { RoadName = "Broadway", TrafficLevel = "Medium", AreaId = area.Id, Timestamp = DateTime.UtcNow },
                    new TrafficData { RoadName = "5th Avenue", TrafficLevel = "Low", AreaId = area.Id, Timestamp = DateTime.UtcNow }
                );
                await trafficContext.SaveChangesAsync();
            }
            Console.WriteLine("✅ Traffic database initialized");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error initializing Traffic database: {ex.Message}");
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

if (redisEnabled)
{
    try
    {
        app.UseSession();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Session middleware failed: {ex.Message}");
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/Index"));

app.Run();

public partial class Program { }