using System.ComponentModel.DataAnnotations;

namespace BookStore.Models;

public enum Genres
{
    Porn,
    WomansShit,
    MansShit,
}
public class Book
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Source { get; set; }
    public string? Writer { get; set; }
    [Range(0, 5)]
    public decimal Rate { get; set; }
    public List<User> Users { get; set; }

}
