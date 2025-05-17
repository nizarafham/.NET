using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly DummyDataService _dataService;

        public ProductController(DummyDataService dataService)
        {
            _dataService = dataService;
        }

        // /Product?categoryId=1 atau /Product?brandId=2 atau /Product?searchQuery=laptop
        public IActionResult Index(int? categoryId, int? brandId, string? searchQuery)
        {
            var products = _dataService.GetProducts(categoryId, brandId, searchQuery);
            ViewBag.FilterTitle = "Hasil Pencarian"; // Default

            if (categoryId.HasValue)
            {
                var category = _dataService.GetAllCategories().FirstOrDefault(c => c.Id == categoryId.Value);
                ViewBag.FilterTitle = $"Produk Kategori: {category?.Name ?? "Tidak Ditemukan"}";
            }
            else if (brandId.HasValue)
            {
                var brand = _dataService.GetAllBrands().FirstOrDefault(b => b.Id == brandId.Value);
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

            return View(products);
        }

        // /Product/Details/1
        public IActionResult Details(int id)
        {
            var product = _dataService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}