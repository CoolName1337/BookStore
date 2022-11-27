using System.ComponentModel.DataAnnotations;

namespace BookStore.DAL.Models;

public class Book
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Введите название")]
    public string Title { get; set; } = "";
    [Range(0, int.MaxValue, ErrorMessage = "Введите цену (больше или равна нулю)")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Введите описание")]
    public string Description { get; set; } = "";
    [Required(ErrorMessage = "Укажите картинку")]
    public string SourceImage { get; set; } = "";
    [Required(ErrorMessage = "Укажите файл")]
    public string SourceFile { get; set; } = "";
    public List<Author> Authors { get; set; } = new();
    public DateTime AddingDate { get; set; } = DateTime.Now;
    public DateTime DateOfCreation { get; set; }
    public List<User> Users { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
    public List<Favorite> Favorites { get; set; } = new();
    public List<Genre> Genres { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public int Bought { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Введите количество страниц (больше или равно нулю)")]
    public int Pages { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Введите возрастное ограничение (больше или равно нулю)")]
    public int AgeLimit { get; set; }
}
