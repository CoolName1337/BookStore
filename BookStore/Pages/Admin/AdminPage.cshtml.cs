using BookStore.BAL.DTO;
using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Admin;

[Authorize(Roles = "creator,admin")]
public class AdminPageModel : PageModel
{
    public readonly IServiceBook _serviceBook;
    public readonly IServiceGenre _serviceGenre;
    public readonly IServiceAuthor _serviceAuthor;

    public BookDTO CreatedBook { get; set; } = new();
    public IFormFile BookFormFile { get; set; }
    public IFormFile ImageFormFile { get; set; }

    public AdminPageModel(IServiceBook serviceBook, IServiceGenre serviceGenre, IServiceAuthor serviceAuthor)
    {
        _serviceBook = serviceBook;
        _serviceGenre = serviceGenre;
        _serviceAuthor = serviceAuthor;
    }

    public IActionResult OnGet() { return Page(); }
    private IActionResult GetPartialBooks()
    {
        return new PartialViewResult()
        {
            ViewName = "_BooksView",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnPostFindBooks(string req)
    {
        ViewData["req"] = req ?? "";
        return GetPartialBooks();
    }
    public async Task<IActionResult> OnPostCreateBook(BookDTO CreatedBook)
    {
        var book = await _serviceBook.Add(CreatedBook);
        if(CreatedBook.Genres)
        _serviceBook.AddGenres(
            book,
            CreatedBook.Genres.Split(";").ToList().Where(str => int.TryParse(str, out int res))
            .Select(genreId => _serviceGenre.GetById(int.Parse(genreId)))
            );
        _serviceBook.AddAuthors(
            book,
            CreatedBook.Authors?.Split(";").ToList().Where(str => int.TryParse(str, out int res))
            .Select(authorId => _serviceAuthor.Authors.Find(int.Parse(authorId)))
            );
        await _serviceBook.Update(book);
        return GetPartialBooks();
    }
    public IActionResult OnDeleteDeleteBook(int bookId)
    {
        _serviceBook.Delete(_serviceBook.Books.Find(bookId));
        return GetPartialBooks();
    }



    private IActionResult GetPartialGenres()
    {
        return new PartialViewResult()
        {
            ViewName = "_GenresView",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnPostFindGenres(string req, string type)
    {
        ViewData["req"] = req ?? "";
        ViewData["type"] = type ?? "";
        return GetPartialGenres();
    }
    public async Task<IActionResult> OnPostCreateGenre(string genreName)
    {
        Genre genre = await _serviceGenre.Create(genreName);
        return GetPartialGenres();
    }
    public async Task<IActionResult> OnDeleteDeleteGenre(string genreName)
    {
        await _serviceGenre.Delete(genreName);
        return GetPartialGenres();
    }


    private IActionResult GetPartialAuthors()
    {
        return new PartialViewResult()
        {
            ViewName = "_AuthorsView",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnPostFindAuthors(string req, string type)
    {
        ViewData["req"] = req ?? "";
        ViewData["type"] = type ?? "";
        return GetPartialAuthors();
    }
    public async Task<IActionResult> OnPostCreateAuthor(string authorName, string description, IFormFile authorPic)
    {
        var author = new Author() { Name = authorName, Description = description };
        await _serviceAuthor.Add(author, authorPic);
        return GetPartialAuthors();
    }
    public async Task<IActionResult> OnDeleteDeleteAuthor(int authorId)
    {
        await _serviceAuthor.Remove(_serviceAuthor.Authors.Find(authorId));
        return GetPartialAuthors();
    }
}