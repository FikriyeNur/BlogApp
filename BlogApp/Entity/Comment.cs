
namespace BlogApp.Entity
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string? Text { get; set; }
        public DateTime PublishedDate { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}