namespace Ecommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty; 
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        
    }
}