using BlogApp.Entity;

namespace BlogApp.Data.Abstract.IRepository
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts();
        Task AddPostAsync(Post entity);
        Task UpdatePostAsync(Post entity);
        Task DeletePostAsync(int postId);
        Task DeletePostTagAsync(int postId);
    }
}