using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class LoginModel
    {
        [Display(Name = "E-Posta")]
        public string? Email { get; set; }

        [Display(Name = "Şifre")]
        public string? Password { get; set; }
    }
}