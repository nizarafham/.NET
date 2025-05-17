namespace Ecommerce.Models
{
    public class BannerItem
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        public string? LinkUrl { get; set; } // Opsional
    }
}