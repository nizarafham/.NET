using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims; // Diperlukan untuk mendapatkan user ID
using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore; // Untuk operasi database seperti FindAsync, FirstOrDefaultAsync, AddAsync, SaveChangesAsync

namespace Ecommerce.Controllers
{
    // Atribut [Authorize] bisa diletakkan di sini jika SEMUA action butuh login,
    // atau di setiap action secara individual jika hanya beberapa yang butuh.
    // Untuk keranjang user yang login, semua action biasanya butuh login.
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController>? _logger; // Opsional Logger

        public CartController(ApplicationDbContext context, ILogger<CartController>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Cart
        // Menampilkan isi keranjang user yang sedang login
        public async Task<IActionResult> Index()
        {
            // Mendapatkan ID user yang sedang login
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                // Ini seharusnya tidak terjadi jika [Authorize] bekerja,
                // tapi ini adalah fallback yang aman.
                return RedirectToAction("Login", "Account");
            }

            // Mengambil item keranjang user dari database, termasuk data Produk terkait
            var cartItems = await _context.CartItems
                                        .Where(ci => ci.UserId == userId)
                                        .Include(ci => ci.Product) // Memuat data Produk terkait
                                        .ToListAsync();

            return View(cartItems);
        }

        // POST: /Cart/AddToCart
        // Menambahkan produk ke keranjang user yang sedang login
        [HttpPost] // Biasanya AddToCart dipanggil via POST dari form atau AJAX
        [ValidateAntiForgeryToken] // Penting untuk keamanan
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1) // Default quantity 1
        {
            // Mendapatkan ID user yang sedang login
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                // Jika user belum login, redirect ke halaman login
                // Middleware [Authorize] akan menangani sebagian besar kasus ini,
                // tapi ini adalah validasi tambahan.
                 return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("AddToCart", "Cart", new { productId = productId, quantity = quantity }) });
            }

            // Cari item keranjang yang sudah ada untuk user dan produk ini
            var existingCartItem = await _context.CartItems
                                                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

            if (existingCartItem == null)
            {
                // Jika item belum ada, buat item keranjang baru
                var productToAdd = await _context.Products.FindAsync(productId);
                if (productToAdd == null)
                {
                    // Produk tidak ditemukan
                    return NotFound(); // Atau redirect ke halaman error/pesan
                }

                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity > 0 ? quantity : 1 // Pastikan quantity minimal 1
                };

                _context.CartItems.Add(newCartItem);
                 _logger?.LogInformation("User {UserId} added product {ProductId} to cart.", userId, productId);
            }
            else
            {
                // Jika item sudah ada, tambahkan jumlah (quantity)
                existingCartItem.Quantity += (quantity > 0 ? quantity : 1);
                 _logger?.LogInformation("User {UserId} updated quantity for product {ProductId} in cart. New quantity: {Quantity}.", userId, productId, existingCartItem.Quantity);
            }

            // Simpan perubahan ke database
            await _context.SaveChangesAsync();

            // Redirect ke halaman keranjang setelah berhasil
            return RedirectToAction(nameof(Index));

            // Opsional: Redirect kembali ke halaman asal
            // return Redirect(Request.Headers["Referer"].ToString());
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
             var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                 return Unauthorized(); // Atau RedirectToAction("Login", "Account");
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem == null)
            {
                return NotFound(); // Item keranjang tidak ditemukan atau bukan milik user ini
            }

            if (quantity <= 0)
            {
                // Jika quantity 0 atau kurang, hapus item
                _context.CartItems.Remove(cartItem);
                 _logger?.LogInformation("User {UserId} removed item {CartItemId} from cart.", userId, cartItemId);
            }
            else
            {
                // Update quantity
                cartItem.Quantity = quantity;
                 _logger?.LogInformation("User {UserId} updated item {CartItemId} quantity to {Quantity}.", userId, cartItemId, quantity);
            }

            await _context.SaveChangesAsync();

            // Redirect kembali ke halaman keranjang
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/RemoveItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                 return Unauthorized(); // Atau RedirectToAction("Login", "Account");
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem == null)
            {
                return NotFound(); // Item keranjang tidak ditemukan atau bukan milik user ini
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

             _logger?.LogInformation("User {UserId} removed item {CartItemId} from cart.", userId, cartItemId);

            // Redirect kembali ke halaman keranjang
            return RedirectToAction(nameof(Index));
        }
    }
}
