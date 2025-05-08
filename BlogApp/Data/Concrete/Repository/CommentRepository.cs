using BlogApp.Data.Abstract.IRepository;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogContext _context;
        public CommentRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Comment> Comments()
        {
            return _context.Comments;
        }

        public async Task AddCommentAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment entity)
        {
            _context.Comments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
        }
    }
}