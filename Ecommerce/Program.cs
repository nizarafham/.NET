// Program.cs
using Ecommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // Pastikan ini ada jika menggunakan cookie auth

var builder = WebApplication.CreateBuilder(args);

// using Ecommerce.Services; // Tambahkan ini di atas
// Hapus atau komen baris ini jika Anda sudah menggunakan data dari database
// builder.Services.AddSingleton<Ecommerce.Services.DummyDataService>();

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
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // Gunakan skema default
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => // Gunakan skema default
    {
        options.Cookie.Name = "Ecommerce.AuthCookie";
        options.LoginPath = "/Account/Login"; // Halaman login
        options.AccessDeniedPath = "/Account/AccessDenied";
         options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Waktu expired cookie, sesuaikan dengan session timeout
         options.SlidingExpiration = true; // Perpanjang waktu expired jika user aktif
    });

// Tambahkan layanan otorisasi
builder.Services.AddAuthorization();


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

// Urutan middleware ini PENTING
app.UseAuthentication(); // Harus sebelum UseAuthorization
app.UseAuthorization();  // Harus setelah UseAuthentication
app.UseSession();        // Biasanya setelah Auth/Auth jika Session digunakan untuk data setelah login

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
