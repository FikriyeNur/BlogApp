
using BlogApp.Entity;

namespace BlogApp.Data.Abstract.IRepository
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments();
        Task AddCommentAsync(Comment entity);
        Task UpdateCommentAsync(Comment entity);
        Task DeleteCommentAsync(int commentId);
    }
}