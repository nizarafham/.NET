using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BannerItems",
                columns: new[] { "Id", "AltText", "ImageUrl", "LinkUrl" },
                values: new object[,]
                {
                    { 1, "Diskon Besar", "/images/banner1.jng", "/Product?searchQuery=Diskon" },
                    { 2, "Produk Terbaru", "/images/banner2.jpg", "/Product?categoryId=1" },
                    { 3, "Gratis Ongkir", "/images/banner3.jpg", "#" }
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { 1, "/images/asus_logo.png", "Asus" },
                    { 2, "/images/samsung_logo.jpg", "Samsung" },
                    { 3, "/images/logitech_logo.png", "Logitech" },
                    { 4, "/images/apple_logo.svg", "Apple" },
                    { 5, "/images/xiaomi_logo.png", "Xiaomi" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "/images/laptop.png", "Laptop" },
                    { 2, "/images/handphone.jpg", "Handphone" },
                    { 3, "/images/headset.jpg", "Headset" },
                    { 4, "/images/keyboard.jpg", "Keyboard" },
                    { 5, "/images/mouse.jpg", "Mouse" },
                    { 6, "/images/charger.jpg", "Charger" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "BrandName", "CategoryId", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "", 1, "", "Laptop gaming terbaik", "/images/rog_strix.jpg", "Laptop ROG Strix", 25000000m },
                    { 2, 4, "", 2, "", "Handphone canggih", "/images/iphone_15_pro.png", "iPhone 15 Pro", 20000000m },
                    { 3, 3, "", 3, "", "Headset gaming pro", "/images/logitech_g_pro_x.jpeg", "Logitech G Pro X", 1500000m },
                    { 4, 3, "", 4, "", "Keyboard mekanikal", "/images/keyboard_k2.gif", "Keyboard Mechanical K2", 800000m },
                    { 5, 2, "", 2, "", "Flagship Samsung", "/images/samsung_s23.png", "Samsung Galaxy S23", 15000000m },
                    { 6, 3, "", 5, "", "Mouse presisi tinggi", "/images/logitech_g502.bmp", "Mouse Gaming G502", 750000m },
                    { 7, 5, "", 6, "", "Charger cepat", "/images/anker_charger.webp", "Charger Anker 65W", 300000m },
                    { 8, 4, "", 1, "", "Laptop tipis dan bertenaga", "/images/macbook_air_m2.jpg", "Macbook Air M2", 18000000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BannerItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BannerItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BannerItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
