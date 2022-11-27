using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ActionResult = BookStore.BAL.Services.ActionResult<BookStore.DAL.Models.Book>;

namespace BookStore.Pages.Admin;

[Authorize(Roles = "creator,admin")]
public class AdminPageModel : PageModel
{
    public readonly IServiceBook _serviceBook;
    public readonly IServiceGenre _serviceGenre;
    public readonly IServiceAuthor _serviceAuthor;

    public ActionResult ActionResult { get; set; } = new();
    public Book CreatedBook { get; set; } = new();
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
    public IActionResult OnPostCreateBook(Book CreatedBook)
    {
        //_serviceBook.AddGenres(
        //        CreatedBook,
        //        _serviceGenre.GetAll().Where(genre => Request.Form["genres"].Contains(genre.Id.ToString())).ToArray()
        //        );
        //_serviceBook.AddAuthors(
        //    CreatedBook,
        //    Request.Form["authors"].Select(authId => _serviceAuthor.Authors.Find(int.Parse(authId))).ToList()
        //    );
        return GetPartialBooks();
    }
    public async Task<IActionResult> OnDeleteDeleteBook(string genreName)
    {
        await _serviceGenre.Delete(genreName);
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