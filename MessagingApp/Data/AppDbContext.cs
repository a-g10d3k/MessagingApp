using Microsoft.EntityFrameworkCore;
using MessagingApp.Models;

namespace MessagingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<MessageModel> Messages { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;
    }
}
