using System.ComponentModel.DataAnnotations;

namespace BookStore.Models;

public enum Genres
{
    Porn,
    WomansShit,
    MansShit,
    Science,
}

internal class Rating
{
    public decimal Get() => stars.Values.Sum() / stars.Count();
    Dictionary<int, int> stars = new();
    public void AddStar(int id, int s) => stars.Add(id, s);
    public void RemoveStar(int id) => stars.Remove(id);
}

public class Book
{
    [Range(0, 1000)]
    public decimal Price { get; set; }
    [Required]
    public string Name { get; set; } = "Undefined";
    [Required]
    public string Data { get; set; }
    public string Description { get; set; }

    private bool isBought = false;

    [Range(0, 5)]
    private decimal Rating;
}
