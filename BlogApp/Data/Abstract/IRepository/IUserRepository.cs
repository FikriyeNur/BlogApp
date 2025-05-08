using BlogApp.Entity;

namespace BlogApp.Data.Abstract.IRepository
{
    public interface IUserRepository
    {
        IQueryable<User> Users();
        Task AddUserAsync(User entity);
        Task UpdateUserAsync(User entity);
        Task DeleteUserAsync(int userId);
    }
}