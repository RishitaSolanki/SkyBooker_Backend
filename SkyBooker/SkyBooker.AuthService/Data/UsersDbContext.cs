using Microsoft.EntityFrameworkCore;
using SkyBooker.AuthService.Entities;

namespace SkyBooker.AuthService.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}