using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class TagModel
    {
        public int TagId { get; set; }

        [Display(Name = "Etiket Adı")]
        public string? Text { get; set; }

        [Display(Name = "Etiket Url")]
        public string? Url { get; set; }

        public List<PostModel> Posts { get; set; } = new List<PostModel>();
    }
}