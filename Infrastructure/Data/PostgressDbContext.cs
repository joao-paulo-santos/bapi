using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class PostgressDbContext : DbContext
    {
        public PostgressDbContext(DbContextOptions<PostgressDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}
