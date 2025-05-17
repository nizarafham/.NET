using Microsoft.AspNetCore.Mvc;
using Ecommerce.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class HomeController : Controller 
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index() // Atau public async Task OnGetAsync()
    {
        ViewBag.Banners = await _context.BannerItems.ToListAsync();
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Brands = await _context.Brands.ToListAsync();
        ViewBag.RecommendedProducts = await _context.Products.ToListAsync();

        return View(); // Atau Page()
    }
}