using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data; // Sesuaikan dengan namespace DbContext Anda
using Ecommerce.Models; // Sesuaikan dengan namespace Model Anda
using Microsoft.AspNetCore.Authorization; // Untuk otorisasi (opsional, tapi disarankan untuk admin)

// Pastikan Anda memiliki area 'Admin' atau sesuaikan rute jika tidak
// [Area("Admin")] // Jika Anda menggunakan Area "Admin"
// [Authorize(Roles = "Admin")] // Contoh: hanya user dengan role "Admin" yang bisa akses
public class AdminProductsController : Controller
{
    private readonly ApplicationDbContext _context; // Menggunakan DbContext

    public AdminProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: AdminProducts
    // Menampilkan daftar semua produk untuk admin
    public async Task<IActionResult> Index()
    {
        // Include Category dan Brand jika Anda ingin menampilkan nama mereka di daftar
        var applicationDbContext = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: AdminProducts/Details/5
    // Menampilkan detail produk tertentu
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

        return View(product);
    }

    // GET: AdminProducts/Create
    // Menampilkan form untuk membuat produk baru
    public IActionResult Create()
    {
        // Siapkan data Category dan Brand untuk dropdown di form
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
        return View();
    }

    // POST: AdminProducts/Create
    // Memproses data dari form untuk membuat produk baru
    [HttpPost]
    [ValidateAntiForgeryToken] // Penting untuk keamanan
    public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,CategoryId,BrandId,ImageUrl")] Product product)
    {
        // Catatan: Penanganan upload file untuk ImageUrl perlu ditambahkan di sini
        // Saat ini hanya menerima string URL

        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Kembali ke daftar produk setelah berhasil
        }

        // Jika model tidak valid, tampilkan kembali form dengan pesan error
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
        return View(product);
    }

    // GET: AdminProducts/Edit/5
    // Menampilkan form untuk mengedit produk yang sudah ada
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Siapkan data Category dan Brand untuk dropdown di form
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
        return View(product);
    }

    // POST: AdminProducts/Edit/5
    // Memproses data dari form untuk mengupdate produk
    [HttpPost]
    [ValidateAntiForgeryToken] // Penting untuk keamanan
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId,BrandId,ImageUrl")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        // Catatan: Penanganan upload file untuk ImageUrl saat edit juga perlu ditambahkan

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index)); // Kembali ke daftar setelah berhasil
        }

        // Jika model tidak valid, tampilkan kembali form dengan pesan error
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
        return View(product);
    }

    // GET: AdminProducts/Delete/5
    // Menampilkan halaman konfirmasi sebelum menghapus produk
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Ambil produk untuk ditampilkan di halaman konfirmasi
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: AdminProducts/Delete/5
    // Memproses permintaan penghapusan produk
    [HttpPost, ActionName("Delete")] // Nama action method adalah Delete, tapi rute menggunakan /Delete/5 (POST)
    [ValidateAntiForgeryToken] // Penting untuk keamanan
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); // Kembali ke daftar setelah berhasil dihapus
    }

    // Helper method untuk mengecek keberadaan produk
    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
