using Microsoft.EntityFrameworkCore;
using Prism.Models;

namespace Prism.Data
{

    public class DatabaseCtx: DbContext
    {
        public DatabaseCtx(DbContextOptions<DatabaseCtx> options) : base(options)
        {
        }
    
        public DbSet<User> Users { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<NewsSource>().ToTable("NewsSources");
            modelBuilder.Entity<UserPreference>().ToTable("UserPreferences");
        }
    }
}