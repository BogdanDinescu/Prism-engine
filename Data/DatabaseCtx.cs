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
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<NewsSource>().ToTable("NewsSources");
            modelBuilder.Entity<NewsArticle>().ToTable("NewsArticles");
            modelBuilder.Entity<UserPreference>().ToTable("UserPreferences");
            modelBuilder.Entity<Post>().ToTable("Posts");
        }
    }
}