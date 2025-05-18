using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username wajib diisi")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password wajib diisi")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Ingat Saya")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}