using BookStore.BAL.DTO;
using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookStore.Pages.Admin;

[Authorize(Roles = "creator,admin")]
public class EditBookModel : PageModel
{
    private readonly IServiceBook _serviceBook;
    public readonly IServiceGenre _serviceGenre;
    public readonly IServiceAuthor _serviceAuthor;
    public int BookId;
    public BookDTO ChangedBook { get; set; }
    public EditBookModel(IServiceBook serviceBook, IServiceGenre serviceGenre, IServiceAuthor serviceAuthor)
    {
        _serviceBook = serviceBook;
        _serviceGenre = serviceGenre;
        _serviceAuthor = serviceAuthor;
    }

    public void OnGet(int id)
    {
        BookId = id;
        ChangedBook = BookDTO.FromBook(_serviceBook.GetBook(id));
    }
    public async Task<JsonResult> OnPostUpdateBook(int id, BookDTO ChangedBook)
    {
        var book = await _serviceBook.Edit(id, ChangedBook);

        await _serviceBook.RemoveGenres(book, book.Genres);
        if (ChangedBook.Genres != null)
        {
            await _serviceBook.AddGenres(
            book,
            ChangedBook.Genres.Split(";").ToList().Where(str => int.TryParse(str, out int res))
            .Select(genreId => _serviceGenre.GetById(int.Parse(genreId)))
            );
        }

        await _serviceBook.RemoveAuthors(book, book.Authors);
        if (ChangedBook.Authors != null)
        {
            await _serviceBook.AddAuthors(
            book,
            ChangedBook.Authors.Split(";").ToList().Where(str => int.TryParse(str, out int res))
            .Select(authorId => _serviceAuthor.Authors.Find(int.Parse(authorId)))
            );
        }
        return new JsonResult(id);
    }

    public IActionResult OnPostDeleteBook(int id)
    {
        _serviceBook.Delete(_serviceBook.GetBook(id));
        return RedirectToPage("/Index");
    }
}