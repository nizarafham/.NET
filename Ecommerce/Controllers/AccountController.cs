using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies; // Diperlukan untuk CookieAuthenticationDefaults.AuthenticationScheme

namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController>? _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                                        .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null)
                {
                    // GANTI INI dengan implementasi HASHING DAN VERIFIKASI yang AMAN!
                    bool isPasswordValid = VerifyPasswordHash(model.Password, user.HashedPassword);

                    if (isPasswordValid)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim("FullName", user.Username), // Ganti jika ada properti FullName
                            new Claim(ClaimTypes.Role, user.Role),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); // <-- Gunakan skema default

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme, // <-- Gunakan skema default
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        _logger?.LogInformation("User {Name} logged in at {Time}.", user.Username, DateTime.UtcNow);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            if (user.Role == "Admin")
                            {
                                return RedirectToAction("Index", "AdminProducts", new { area = "Admin" });
                            }
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Username atau password salah.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
             ViewData["ReturnUrl"] = returnUrl;
             return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username sudah digunakan.");
                    return View(model);
                }

                var newUser = new User
                {
                    Username = model.Username,
                    // GANTI INI dengan implementasi HASHING yang AMAN!
                    HashedPassword = HashPassword(model.Password),
                    Role = "User"
                };

                _context.Add(newUser);
                await _context.SaveChangesAsync();

                _logger?.LogInformation("User {Name} created a new account at {Time}.", newUser.Username, DateTime.UtcNow);

                 if (Url.IsLocalUrl(returnUrl))
                 {
                     return Redirect(returnUrl);
                 }
                 else
                 {
                     return RedirectToAction(nameof(Login));
                 }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // <-- Gunakan skema default
            _logger?.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        public IActionResult Profile()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            ViewBag.UserName = userName;
            ViewBag.UserId = userId;
            ViewBag.UserRole = userRole;
            ViewBag.FullName = User.FindFirstValue("FullName");
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // Metode placeholder untuk Hashing dan Verifikasi Password
        // GANTI INI dengan implementasi HASHING DAN VERIFIKASI yang AMAN!
        private string HashPassword(string password)
        {
            // Gunakan library hashing yang aman di sini
            return password; // Contoh TIDAK AMAN
        }

         private bool VerifyPasswordHash(string password, string hashedPassword)
        {
            // Gunakan library hashing yang aman di sini
            return password == hashedPassword; // Contoh TIDAK AMAN
        }
    }
}
