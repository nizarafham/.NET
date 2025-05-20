using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic; // Untuk List<int>

namespace Ecommerce.Controllers
{
    [Authorize] // Pastikan hanya user yang login yang bisa mengakses Checkout
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CheckoutController>? _logger;

        public CheckoutController(ApplicationDbContext context, ILogger<CheckoutController>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        // POST: /Checkout/ProcessSelectedItems
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessSelectedItems(string selectedCartItemIds)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                _logger?.LogError("User ID not found or invalid in claims for Checkout.");
                TempData["ErrorMessage"] = "Autentikasi diperlukan untuk melanjutkan checkout.";
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(selectedCartItemIds))
            {
                TempData["ErrorMessage"] = "Tidak ada item yang dipilih untuk checkout.";
                return RedirectToAction("Index", "Cart");
            }

            // Parse ID item keranjang dari string yang dipisahkan koma
            var selectedIds = selectedCartItemIds.Split(',')
                                             .Where(id => int.TryParse(id, out _))
                                             .Select(int.Parse)
                                             .ToList();

            if (!selectedIds.Any())
            {
                TempData["ErrorMessage"] = "Tidak ada item yang valid dipilih untuk checkout.";
                return RedirectToAction("Index", "Cart");
            }

            // Ambil item keranjang dari database yang sesuai dengan ID yang dipilih DAN milik user yang sedang login
            var cartItemsToCheckout = await _context.CartItems
                                                    .Where(ci => selectedIds.Contains(ci.Id) && ci.UserId == userId)
                                                    .Include(ci => ci.Product) // Agar bisa mengakses nama produk untuk notifikasi
                                                    .ToListAsync();

            if (!cartItemsToCheckout.Any())
            {
                TempData["ErrorMessage"] = "Item yang dipilih tidak ditemukan di keranjang Anda atau sudah dihapus.";
                return RedirectToAction("Index", "Cart");
            }

            // --- Simulasi Proses Checkout ---
            // Di sini Anda akan menambahkan logika untuk membuat Order dan OrderItem,
            // mengurangi stok produk, memproses pembayaran, dll.
            // Untuk kebutuhan ini, kita akan langsung menghapus item dan memberikan notifikasi.

            // Contoh: Membuat notifikasi berisi nama-nama produk yang di-checkout
            var productNames = cartItemsToCheckout.Select(ci => ci.Product.Name).ToList();
            TempData["SuccessMessage"] = $"Berhasil checkout item: {string.Join(", ", productNames)}. Terima kasih!";

            // Hapus item yang sudah di-checkout dari keranjang
            _context.CartItems.RemoveRange(cartItemsToCheckout);
            await _context.SaveChangesAsync();

            _logger?.LogInformation("User {UserId} successfully checked out {ItemCount} items.", userId, cartItemsToCheckout.Count);

            // Redirect kembali ke halaman keranjang yang sekarang sudah terupdate (item dihapus)
            return RedirectToAction("Index", "Cart");
        }
    }
}
