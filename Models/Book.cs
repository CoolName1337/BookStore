using Microsoft.AspNetCore.Mvc.RazorPages;
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

    public string GenresString { get; set; } = "";
    public string RaitingString { get; set; } = "";
    public List<User> Users { get; set; } = new List<User>();

    [NotMapped]
    public double Rating
    {

        get
        {
            if (RaitingString == "") return 0;
            var res = RaitingString.Trim().Split(" ").Average(str => int.Parse(str));
            return Math.Round(res, 1, MidpointRounding.ToEven);
        }
    }
    public int GetRaitingCount(int star = 0) => star == 0 ? RaitingString.Trim().Split(" ").Length : RaitingString.Trim().Split(" ").Where(s => s == star.ToString()).Count();

    public void DeleteRating(string rating)
    {
        var rateArray = RaitingString.Trim().Split(" ").ToList();
        rateArray.Remove(rating);
        RaitingString = "";
        foreach (var el in rateArray) RaitingString += el + " ";
    }

}
