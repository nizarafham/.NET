using Ecommerce.Models;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Services
{
    public class DummyDataService
    {
        private static List<Category> _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Laptop", ImageUrl = "/images/laptop.png" },
            new Category { Id = 2, Name = "Handphone", ImageUrl = "/images/handphone.jpg" },
            new Category { Id = 3, Name = "Headset", ImageUrl = "/images/headset.jpg" },
            new Category { Id = 4, Name = "Keyboard", ImageUrl = "/images/keyboard.jpg" },
            new Category { Id = 5, Name = "Mouse", ImageUrl = "/images/mouse.jpg" },
            new Category { Id = 6, Name = "Charger", ImageUrl = "/images/charger.jpg" }
        };

        private static List<Brand> _brands = new List<Brand>
        {
            new Brand { Id = 1, Name = "Asus", LogoUrl = "/images/asus_logo.png" },
            new Brand { Id = 2, Name = "Samsung", LogoUrl = "/images/samsung_logo.jpg" },
            new Brand { Id = 3, Name = "Logitech", LogoUrl = "/images/logitech_logo.png" },
            new Brand { Id = 4, Name = "Apple", LogoUrl = "/images/apple_logo.svg" },
            new Brand { Id = 5, Name = "Xiaomi", LogoUrl = "/images/xiaomi_logo.png" }
        };

        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop ROG Strix", Description = "Laptop gaming terbaik", Price = 25000000, CategoryId = 1, BrandId = 1, ImageUrl = "/images/rog_strix.jpg", CategoryName = "Laptop", BrandName = "Asus" },
            new Product { Id = 2, Name = "iPhone 15 Pro", Description = "Handphone canggih", Price = 20000000, CategoryId = 2, BrandId = 4, ImageUrl = "/images/iphone_15_pro.png", CategoryName = "Handphone", BrandName = "Apple" },
            new Product { Id = 3, Name = "Logitech G Pro X", Description = "Headset gaming pro", Price = 1500000, CategoryId = 3, BrandId = 3, ImageUrl = "/images/logitech_g_pro_x.jpeg", CategoryName = "Headset", BrandName = "Logitech" },
            new Product { Id = 4, Name = "Keyboard Mechanical K2", Description = "Keyboard mekanikal", Price = 800000, CategoryId = 4, BrandId = 3, ImageUrl = "/images/keyboard_k2.gif", CategoryName = "Keyboard", BrandName = "Logitech" },
            new Product { Id = 5, Name = "Samsung Galaxy S23", Description = "Flagship Samsung", Price = 15000000, CategoryId = 2, BrandId = 2, ImageUrl = "/images/samsung_s23.png", CategoryName = "Handphone", BrandName = "Samsung" },
            new Product { Id = 6, Name = "Mouse Gaming G502", Description = "Mouse presisi tinggi", Price = 750000, CategoryId = 5, BrandId = 3, ImageUrl = "/images/logitech_g502.bmp", CategoryName = "Mouse", BrandName = "Logitech" },
            new Product { Id = 7, Name = "Charger Anker 65W", Description = "Charger cepat", Price = 300000, CategoryId = 6, BrandId = 5, ImageUrl = "/images/anker_charger.webp", CategoryName = "Charger", BrandName = "Xiaomi" }, //Asumsikan Anker di bawah Xiaomi untuk demo
            new Product { Id = 8, Name = "Macbook Air M2", Description = "Laptop tipis dan bertenaga", Price = 18000000, CategoryId = 1, BrandId = 4, ImageUrl = "/images/macbook_air_m2.jpg", CategoryName = "Laptop", BrandName = "Apple" }
        };

        private static List<BannerItem> _bannerItems = new List<BannerItem>
        {
            new BannerItem { Id = 1, ImageUrl = "/images/laptop.png", AltText = "Diskon Besar", LinkUrl = "/Product?searchQuery=Diskon" },
            new BannerItem { Id = 2, ImageUrl = "/images/mouse.jpg", AltText = "Produk Terbaru", LinkUrl = "/Product?categoryId=1" },
            new BannerItem { Id = 3, ImageUrl = "/images/keyboard.jpg", AltText = "Gratis Ongkir", LinkUrl = "#" }
        };

        public List<Product> GetAllProducts() => _products;
        public List<Category> GetAllCategories() => _categories;
        public List<Brand> GetAllBrands() => _brands;
        public List<BannerItem> GetBannerItems() => _bannerItems;

        public Product? GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public List<Product> GetProducts(int? categoryId, int? brandId, string? searchQuery)
        {
            var query = _products.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()) ||
                                         (p.Description != null && p.Description.ToLower().Contains(searchQuery.ToLower())));
            }
            return query.ToList();
        }
    }
}