using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BookStore.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Book> Books { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=usersstoredb;Trusted_Connection=True;");
        }
    }
}
