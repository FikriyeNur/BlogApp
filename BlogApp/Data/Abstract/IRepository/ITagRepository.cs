using BlogApp.Entity;

namespace BlogApp.Data.Abstract.IRepository
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags();
        Task AddTagAsync(Tag entity);
        Task UpdateTagAsync(Tag entity);
        Task DeleteTagAsync(int tagId);
    }
}