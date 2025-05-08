using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.Models
{
    public class CommentViewModel
    {
        public int PostId { get; set; }
        public int? CommentId { get; set; }

        [Display(Name = "Yorum")]
        public string? Text { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public UserModel? User { get; set; }
    }
}