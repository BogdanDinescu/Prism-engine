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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}