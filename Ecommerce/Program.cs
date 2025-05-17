// Program.cs
using Ecommerce.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// using Ecommerce.Services; // Tambahkan ini di atas
builder.Services.AddSingleton<Ecommerce.Services.DummyDataService>();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // Tambahkan ini

// Tambahkan ini untuk session (diperlukan untuk login)
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=ecommerce.db"));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Atur timeout session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Tambahkan ini untuk Authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "Ecommerce.AuthCookie";
        options.LoginPath = "/Account/Login"; // Halaman login
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Tambahkan ini SEBELUM UseAuthorization
app.UseAuthorization();
app.UseSession(); // Tambahkan ini

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();