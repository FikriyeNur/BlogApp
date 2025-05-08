using BlogApp.Data.Abstract.IRepository;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _context;

        public PostRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts()
        {
            return _context.Posts;
        }

        public async Task AddPostAsync(Post entity)
        {
            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post entity)
        {
            _context.Posts.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeletePostTagAsync(int postId)
        {
            var postTags = await _context.Posts.Include(x => x.Tags).Where(x => x.PostId == postId).ToListAsync();
            if (postTags != null && postTags.Count > 0)
            {
                postTags.ForEach(x => x.Tags.Clear());
                await _context.SaveChangesAsync();
                return;
            }
        }
    }
}