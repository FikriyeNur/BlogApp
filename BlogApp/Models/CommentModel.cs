using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class CommentModel
    {
        public int CommentId { get; set; }

        [Display(Name = "Yorum")]
        public string? Text { get; set; }

        public DateTime PublishedDate { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        public PostModel Post { get; set; } = null!;
        public UserModel User { get; set; } = null!;
    }
}