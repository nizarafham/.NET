using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Diperlukan untuk operasi EF Core
using Ecommerce.Data; // Namespace DbContext Anda
using Ecommerce.Models; // Namespace Model Anda

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context; // Menggunakan DbContext

        // Inject ApplicationDbContext melalui konstruktor
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Product
        // Menampilkan daftar produk, bisa difilter berdasarkan kategori, merek, atau pencarian
        public async Task<IActionResult> Index(int? categoryId, int? brandId, string? searchQuery)
        {
            // Query awal mengambil semua produk dari database
            var productsQuery = _context.Products.AsQueryable();

            // Include properti navigasi Category dan Brand jika Anda menampilkannya di View
            // Ini penting agar Anda bisa mengakses product.Category.Name dan product.Brand.Name
            productsQuery = productsQuery
                .Include(p => p.Category)
                .Include(p => p.Brand);

            // Filter berdasarkan CategoryId jika ada
            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            // Filter berdasarkan BrandId jika ada
            if (brandId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.BrandId == brandId.Value);
            }

            // Filter berdasarkan SearchQuery jika ada
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                // Cari di Nama atau Deskripsi produk (case-insensitive)
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()) ||
                                                         (p.Description != null && p.Description.ToLower().Contains(searchQuery.ToLower())));
            }

            // Ambil data hasil query dari database secara asynchronous
            var products = await productsQuery.ToListAsync();

            // Siapkan data untuk ViewBag.FilterTitle
            // Ambil nama kategori atau merek dari database jika ID filter ada
            if (categoryId.HasValue)
            {
                 var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId.Value);
                 ViewBag.FilterTitle = $"Produk Kategori: {category?.Name ?? "Tidak Ditemukan"}";
            }
            else if (brandId.HasValue)
            {
                 var brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == brandId.Value);
                 ViewBag.FilterTitle = $"Produk Merek: {brand?.Name ?? "Tidak Ditemukan"}";
            }
            else if (!string.IsNullOrEmpty(searchQuery))
            {
                 ViewBag.FilterTitle = $"Hasil Pencarian untuk: \"{searchQuery}\"";
            }
            else
            {
                 ViewBag.FilterTitle = "Semua Produk";
            }


            // Anda bisa mengirimkan data filter ke View jika diperlukan
            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.CurrentBrandId = brandId;
            ViewBag.CurrentSearchQuery = searchQuery;

            return View(products); // Mengirimkan List<Product> ke View
        }

        // GET: /Product/Details/5
        // Menampilkan detail produk berdasarkan ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Ambil produk berdasarkan ID, include Category dan Brand
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product); // Mengirimkan objek Product ke View
        }

        // Anda bisa menambahkan action lain seperti Create, Edit, Delete di sini
        // jika Anda ingin mengelola produk melalui controller ini (bukan AdminProductsController)
        // Namun, karena kita sudah punya AdminProductsController, action CRUD di sini mungkin tidak diperlukan.
    }
}
