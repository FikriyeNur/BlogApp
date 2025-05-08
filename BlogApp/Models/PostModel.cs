using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class PostModel
    {
        [Display(Name = "Gönderi Id")]
        public int PostId { get; set; }

        [Display(Name = "Gönderi Başlığı")]
        public string? Title { get; set; }

        [Display(Name = "Gönderi İçeriği")]
        public string? Content { get; set; }

        [Display(Name = "Gönderi Resmi")]
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        
        public DateTime PublishedDate { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public int UserId { get; set; }
        public string? Url { get; set; }

        public UserModel? User { get; set; } 

        [Display(Name = "Etiketler")]
        public List<TagModel> Tags { get; set; } = new List<TagModel>();
        public string? SelectedTags { get; set; }
        
        public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}