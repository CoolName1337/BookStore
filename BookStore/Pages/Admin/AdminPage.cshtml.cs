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
    public ActionResult ActionResult = new();
    public Book CreatedBook = new();

    public IFormFile BookFormFile { get; set; }
    public IFormFile ImageFormFile { get; set; }

    public AdminPageModel(IServiceBook serviceBook, IServiceGenre serviceGenre)
    {
        _serviceBook = serviceBook;
        _serviceGenre = serviceGenre;
    }

    public IActionResult OnGet() { return Page(); }

    public IActionResult OnGetSelectAll()
    {
        return new JsonResult(_serviceGenre.GetAll());
    }

    public async Task<IActionResult> OnPostCreate(string genreName)
    {
        Genre genre = await _serviceGenre.Create(genreName);
        if (genre != null)
        {
            return new JsonResult(_serviceGenre.GetAll());
        }
        return new ConflictResult();
    }

    public async Task<IActionResult> OnDeleteDelete(string genreName)
    {
        await _serviceGenre.Delete(genreName);
        return new JsonResult(_serviceGenre.GetAll());
    }

    public async Task<IActionResult> OnPostCreateBook()
    {
        DateTime.TryParse(Request.Form["creatingDate"], out DateTime res);
        CreatedBook = new()
        {
            Title = Request.Form["title"],
            Writer = Request.Form["writer"],
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
            _serviceBook.Update(CreatedBook);
            return RedirectToPage("/BookPage", new { Id = ActionResult.Value.Id });
        }
        CreatedBook.Genres = _serviceGenre.GetAll().Where(genre => Request.Form["genres"].Contains(genre.Id.ToString())).ToList();
        return Page();
    }
}