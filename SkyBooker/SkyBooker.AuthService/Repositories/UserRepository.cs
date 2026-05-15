using Microsoft.EntityFrameworkCore;
using SkyBooker.AuthService.Data;
using SkyBooker.AuthService.Entities;

namespace SkyBooker.AuthService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;

        public UserRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> FindByUserIdAsync(Guid userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<List<User>> SearchUsersAsync(string searchTerm)
        {
            return await _context.Users
                .Where(u => u.FullName.Contains(searchTerm) || u.Email.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<List<User>> FindAllByRoleAsync(string role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            Console.WriteLine($"[REPOSITORY] Adding user to context: {user.UserId}");
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            Console.WriteLine($"[REPOSITORY] Updating user: {user.UserId}");
            _context.Users.Update(user);
        }

        public async Task DeleteAsync(User user)
        {
            Console.WriteLine($"[REPOSITORY] Deleting user: {user.UserId}");
            _context.Users.Remove(user);
        }

        public async Task SaveChangesAsync()
        {
            Console.WriteLine($"[REPOSITORY] Attempting to save changes to database");
            var result = await _context.SaveChangesAsync();
            Console.WriteLine($"[REPOSITORY] SaveChangesAsync completed. Rows affected: {result}");
        }
    }
}