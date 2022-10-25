using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    [Required]
    public string SourceImage { get; set; }
    [Required]
    public string SourceFile { get; set; }
    [Required]
    public string Writer { get; set; }
    public DateTime AddingDate { get; set; }
    public DateTime DateOfCreation { get; set; }

    public List<User> Users { get; set; } = new List<User>();
    public string _ratingString { get; set; } = "";

    public void AddRating(int rating)
    {
        _ratingString += rating.ToString();
    }
    public void DeleteRating(int rating)
    {
        var list = _ratingString.ToList();
        list.Remove(rating.ToString()[0]);
        _ratingString = string.Join("", list);
    }

    [NotMapped]
    public double Rating
    {
        get => _ratingString.Length == 0 ? 0 : Math.Round(_ratingString.ToArray().Average(str => int.Parse(str.ToString())),1,MidpointRounding.ToEven);
    }
    [NotMapped]
    public int RatingCount
    {
        get => _ratingString.Length;
    }
    
}
