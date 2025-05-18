using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key ke Product
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        // Properti Navigasi ke Product
        public Product Product { get; set; } = null!; // Menggunakan null-forgiving operator

        // Foreign Key ke AppUser (user yang memiliki item keranjang ini)
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        // Properti Navigasi ke AppUser
        public User User { get; set; } = null!; // Menggunakan null-forgiving operator

        [Range(1, int.MaxValue, ErrorMessage = "Jumlah harus minimal 1.")]
        public int Quantity { get; set; }
    }
}
