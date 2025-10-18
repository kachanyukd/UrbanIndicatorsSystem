using UrbanIndicatorsSystem.Services;
using UrbanIndicatorsSystem.Data; // Додаємо
using UrbanIndicatorsSystem.Models; // Додаємо
using Microsoft.EntityFrameworkCore; // Додаємо
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1. Додаємо Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. Додаємо DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// 3. Додаємо Identity з нашими налаштуваннями
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Налаштування для Завдання 2c
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;

})
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 4. Додаємо підтримку Razor Pages
builder.Services.AddRazorPages();

// --- Твої існуючі сервіси (залишаємо) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITrafficService, TrafficService>();
// ----------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Це вже є

// 5. Критично: додаємо UseAuthentication() ПЕРЕД UseAuthorization()
app.UseAuthentication();
app.UseAuthorization(); // Це вже є

app.MapControllers(); // Це вже є

// 6. Мапимо Razor Pages (для логіна, реєстрації і т.д.)
app.MapRazorPages();

// 7. Міняємо твій root-ендпоінт, щоб він вів на Welcome Page (яку ми створимо)
app.MapGet("/", () => Results.Redirect("/Index"));
// app.MapGet("/", () => "Urban Indicators System API is running."); // Старий код

app.Run();