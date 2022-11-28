using System.ComponentModel.DataAnnotations;

namespace BookStore.DAL.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string SourceImage { get; set; }
    public string SourceFile { get; set; }
    public List<Author> Authors { get; set; } = new();
    public DateTime AddingDate { get; set; }
    public DateTime DateOfCreation { get; set; }
    public List<User> Users { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
    public List<Favorite> Favorites { get; set; } = new();
    public List<Genre> Genres { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public int Bought { get; set; }
    public int Pages { get; set; }
    public int AgeLimit { get; set; }
}
