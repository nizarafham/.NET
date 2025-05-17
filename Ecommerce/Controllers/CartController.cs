using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class CartController : Controller
    {
        [Authorize] // Hanya user yang login bisa akses
        public IActionResult Index()
        {
            // Untuk sekarang, hanya tampilkan view kosong
            // Nanti Anda akan mengambil data cart dari session atau database
            ViewBag.CartMessage = "Keranjang belanja Anda masih kosong.";
            return View();
        }

        // Tambahkan method lain seperti AddToCart, RemoveFromCart, etc.
    }
}