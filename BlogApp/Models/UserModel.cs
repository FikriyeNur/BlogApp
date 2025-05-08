using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [Display(Name = "Şifre Tekrar")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Ad Soyad")]
        public string? FullName { get; set; }

        [Display(Name = "Resim")]
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "E-Posta")]
        public string? Email { get; set; }

        public List<PostModel> Posts { get; set; } = new List<PostModel>();
        public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}