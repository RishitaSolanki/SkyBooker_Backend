using SkyBooker.AuthService.Entities;

namespace SkyBooker.AuthService.Repositories
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByUserIdAsync(Guid userId);
        Task<bool> ExistsByEmailAsync(string email);
        Task<List<User>> SearchUsersAsync(string searchTerm);
        Task<List<User>> FindAllByRoleAsync(string role);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChangesAsync();
    }
}