using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username wajib diisi.")]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password wajib diisi.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Konfirmasi Password")]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi password tidak cocok.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
