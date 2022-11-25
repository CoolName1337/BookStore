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
    private readonly IServiceBook _serviceBook;
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


    public async Task<IActionResult> OnPostCreateBook()
    {
        DateTime.TryParse(Request.Form["creatingDate"], out DateTime res);
        CreatedBook = new()
        {
            Title = Request.Form["title"],
            Description = Request.Form["descr"],
            DateOfCreation = res
        };

        if (!decimal.TryParse(Request.Form["price"], out decimal numPrice) || numPrice < 0) CreatedBook.Price = -1;
        else CreatedBook.Price = numPrice;

        if (!int.TryParse(Request.Form["ageLimit"], out int ageLimit) || ageLimit < 0) CreatedBook.AgeLimit = -1;
        else CreatedBook.AgeLimit = ageLimit;

        if (!int.TryParse(Request.Form["pagesCount"], out int pagesCount) || pagesCount < 0) CreatedBook.Pages = -1;
        else CreatedBook.Pages = pagesCount;

        BookFormFile = Request.Form.Files["file"];
        ImageFormFile = Request.Form.Files["img"];

        ActionResult = await _serviceBook.Add(CreatedBook, BookFormFile, ImageFormFile);
        if (ActionResult.Succeed)
        {
            _serviceBook.AddGenres(
                CreatedBook,
                _serviceGenre.GetAll().Where(genre => Request.Form["genres"].Contains(genre.Id.ToString())).ToArray()
                );
            _serviceBook.AddAuthors(
                CreatedBook,
                Request.Form["authors"].Select(authId => _serviceAuthor.Authors.Find(int.Parse(authId))).ToList()
                );
            await _serviceBook.Update(CreatedBook);
            return RedirectToPage("/BookPage", new { Id = ActionResult.Value.Id });
        }

        CreatedBook.Genres = _serviceGenre.GetAll()
            .Where(genre => Request.Form["genres"]
            .Contains(genre.Id.ToString())).ToList();

        return Page();
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