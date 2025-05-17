using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // Logika otentikasi dummy
                // Di aplikasi nyata, Anda akan memvalidasi ke database
                if (model.Username == "user" && model.Password == "password")
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim("FullName", "Pengguna Demo"), // Contoh claim tambahan
                        new Claim(ClaimTypes.Role, "User"), // Contoh role
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, "CookieAuth"); // Nama skema otentikasi harus sama

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <true|false>,
                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10), // Session persistence
                        IsPersistent = true, // Cookie akan persist antar sesi browser jika true
                        //IssuedUtc = <DateTimeOffset>,
                        //RedirectUri = <string>
                    };

                    await HttpContext.SignInAsync(
                        "CookieAuth", // Nama skema otentikasi
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    _logger?.LogInformation("User {Name} logged in at {Time}.", model.Username, DateTime.UtcNow); // Jika ada logger

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Username atau password salah.");
            }
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            _logger?.LogInformation("User logged out."); // Jika ada logger
            return RedirectToAction("Index", "Home");
        }


        [Authorize] // Hanya user yang login bisa akses
        public IActionResult Profile()
        {
            // Ambil informasi user dari ClaimsPrincipal
            var userName = User.FindFirstValue(ClaimTypes.Name); // atau User.Identity.Name
            var fullName = User.FindFirstValue("FullName");

            ViewBag.UserName = userName;
            ViewBag.FullName = fullName;
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // Opsional: Logger
        private readonly ILogger<AccountController>? _logger;
        public AccountController(ILogger<AccountController>? logger = null) // Logger bisa null jika tidak di-inject
        {
            _logger = logger;
        }
    }
}