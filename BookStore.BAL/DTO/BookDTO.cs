using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookStore.BAL.DTO;

public class BookDTO
{
    [Required(ErrorMessage = "Введите название")]
    public string Title { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Введите цену (больше или равна нулю)")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Введите описание")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Укажите картинку")]
    public IFormFile SourceImage { get; set; }
    [Required(ErrorMessage = "Укажите файл")]
    public IFormFile SourceFile { get; set; }
    public string Genres { get; set; }
    public string Authors { get; set; }
    [Required(ErrorMessage = "Укажите дату написания")]
    public string DateOfCreation { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Введите количество страниц (больше или равно нулю)")]
    public int Pages { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Введите возрастное ограничение (больше или равно нулю)")]
    public int AgeLimit { get; set; }

    /// <summary>
    /// Returns new Book instance with adding book file and image file to server directory
    /// </summary>
    /// <returns>Book instance</returns>
    public async Task<Book> ToBook() => new Book()
    {
        Title = this.Title,
        Description = this.Description,
        DateOfCreation = DateTime.Parse(this.DateOfCreation),
        AddingDate = DateTime.Now,
        Price = this.Price,
        AgeLimit = this.AgeLimit,
        Pages = this.Pages,
        SourceFile = await FileService.SaveBookFile(this.SourceFile, this.Title + this.DateOfCreation + DateTime.Now.Ticks.ToString()),
        SourceImage = await FileService.SaveBookFile(this.SourceImage, this.Title + this.DateOfCreation + DateTime.Now.Ticks.ToString())
    };
    /// <summary>
    /// Fills the Book instance properties with property values of this BookDTO instance
    /// </summary>
    /// <param name="book">
    /// Instance to fill
    /// </param>
    /// <returns>Rhe same Book instance with filled properties</returns>
    public async Task<Book> ToBook(Book book)
    {
        if (this.SourceFile != null)
        {
            FileService.Delete(book.SourceFile);
            book.SourceFile = await FileService.SaveBookFile(this.SourceFile, this.Title + this.DateOfCreation + DateTime.Now.Ticks.ToString());
        }
        if(this.SourceImage != null)
        {
            FileService.Delete(book.SourceImage);
            book.SourceImage = await FileService.SaveBookFile(this.SourceImage, this.Title + this.DateOfCreation + DateTime.Now.Ticks.ToString());
        }

        book.Title = this.Title;
        book.Description = this.Description;
        book.DateOfCreation = DateTime.Parse(this.DateOfCreation);
        book.Price = this.Price;
        book.AgeLimit = this.AgeLimit;
        book.Pages = this.Pages;

        return book;
    }

    public static BookDTO FromBook(Book book)
    {
        var bookDTO = new BookDTO()
        {
            Title = book.Title,
            Description = book.Description,
            DateOfCreation = book.DateOfCreation.ToString("yyyy-MM-dd"),
            Price = book.Price,
            AgeLimit = book.AgeLimit,
            Pages = book.Pages,
            Genres = String.Join(";", book.Genres.Select(genre => genre.Id)) + ";",
            Authors = String.Join(";", book.Authors.Select(author => author.Id)) + ";",
        };
        return bookDTO;
    }
}
