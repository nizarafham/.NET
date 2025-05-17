using Microsoft.EntityFrameworkCore;
using Ecommerce.Models; // Pastikan namespace ini benar
using System.Collections.Generic; // Perlu untuk List

namespace Ecommerce.Data // Sesuaikan dengan namespace DbContext Anda
{
    public class ApplicationDbContext : DbContext // Sesuaikan dengan nama DbContext Anda
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Definisikan DbSet untuk setiap model Anda
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BannerItem> BannerItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptop", ImageUrl = "/images/laptop.png" },
                new Category { Id = 2, Name = "Handphone", ImageUrl = "/images/handphone.jpg" },
                new Category { Id = 3, Name = "Headset", ImageUrl = "/images/headset.jpg" },
                new Category { Id = 4, Name = "Keyboard", ImageUrl = "/images/keyboard.jpg" },
                new Category { Id = 5, Name = "Mouse", ImageUrl = "/images/mouse.jpg" },
                new Category { Id = 6, Name = "Charger", ImageUrl = "/images/charger.jpg" }
            );

            // Seed Brands
            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Asus", LogoUrl = "/images/asus_logo.png" },
                new Brand { Id = 2, Name = "Samsung", LogoUrl = "/images/samsung_logo.jpg" },
                new Brand { Id = 3, Name = "Logitech", LogoUrl = "/images/logitech_logo.png" },
                new Brand { Id = 4, Name = "Apple", LogoUrl = "/images/apple_logo.svg" },
                new Brand { Id = 5, Name = "Xiaomi", LogoUrl = "/images/xiaomi_logo.png" }
            );

             // Seed Products
             // Pastikan Id di sini tidak berkonflik jika ada data lain
             // Juga perhatikan relasi dengan CategoryId dan BrandId
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop ROG Strix", Description = "Laptop gaming terbaik", Price = 25000000, CategoryId = 1, BrandId = 1, ImageUrl = "/images/rog_strix.jpg" /*, CategoryName = "Laptop", BrandName = "Asus"*/ }, // CategoryName dan BrandName tidak perlu di sini jika ada relasi navigasi
                new Product { Id = 2, Name = "iPhone 15 Pro", Description = "Handphone canggih", Price = 20000000, CategoryId = 2, BrandId = 4, ImageUrl = "/images/iphone_15_pro.png" /*, CategoryName = "Handphone", BrandName = "Apple"*/ },
                new Product { Id = 3, Name = "Logitech G Pro X", Description = "Headset gaming pro", Price = 1500000, CategoryId = 3, BrandId = 3, ImageUrl = "/images/logitech_g_pro_x.jpeg" /*, CategoryName = "Headset", BrandName = "Logitech"*/ },
                new Product { Id = 4, Name = "Keyboard Mechanical K2", Description = "Keyboard mekanikal", Price = 800000, CategoryId = 4, BrandId = 3, ImageUrl = "/images/keyboard_k2.gif" /*, CategoryName = "Keyboard", BrandName = "Logitech"*/ },
                new Product { Id = 5, Name = "Samsung Galaxy S23", Description = "Flagship Samsung", Price = 15000000, CategoryId = 2, BrandId = 2, ImageUrl = "/images/samsung_s23.png" /*, CategoryName = "Handphone", BrandName = "Samsung"*/ },
                new Product { Id = 6, Name = "Mouse Gaming G502", Description = "Mouse presisi tinggi", Price = 750000, CategoryId = 5, BrandId = 3, ImageUrl = "/images/logitech_g502.bmp" /*, CategoryName = "Mouse", BrandName = "Logitech"*/ },
                new Product { Id = 7, Name = "Charger Anker 65W", Description = "Charger cepat", Price = 300000, CategoryId = 6, BrandId = 5, ImageUrl = "/images/anker_charger.webp" /*, CategoryName = "Charger", BrandName = "Xiaomi"*/ },
                new Product { Id = 8, Name = "Macbook Air M2", Description = "Laptop tipis dan bertenaga", Price = 18000000, CategoryId = 1, BrandId = 4, ImageUrl = "/images/macbook_air_m2.jpg" /*, CategoryName = "Laptop", BrandName = "Apple"*/ }
            );

            // Seed Banner Items
            modelBuilder.Entity<BannerItem>().HasData(
                 new BannerItem { Id = 1, ImageUrl = "/images/banner1.jng", AltText = "Diskon Besar", LinkUrl = "/Product?searchQuery=Diskon" },
                 new BannerItem { Id = 2, ImageUrl = "/images/banner2.jpg", AltText = "Produk Terbaru", LinkUrl = "/Product?categoryId=1" },
                 new BannerItem { Id = 3, ImageUrl = "/images/banner3.jpg", AltText = "Gratis Ongkir", LinkUrl = "#" }
            );

             // Konfigurasi lain jika ada (misalnya relasi) bisa ditambahkan di sini
             // Contoh: modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId);

        }
    }
}