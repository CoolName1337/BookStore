using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null;
        public DbSet<Book> Books { get; set; } = null;
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MegaDB.db");
        }
    }
}
