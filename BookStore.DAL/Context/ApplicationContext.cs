using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DAL.Context;

public class ApplicationContext : IdentityDbContext<User>
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Author> Authors { get; set; }
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder opts)
    {
        opts.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=usersstoredb;Trusted_Connection=True;");
    }
}
