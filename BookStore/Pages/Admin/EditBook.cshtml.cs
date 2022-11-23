using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Admin;

[Authorize(Roles = "creator,admin")]
public class EditBookModel : PageModel
{
    private readonly IServiceBook _serviceBook;
    public readonly IServiceGenre _serviceGenre;
    public Book ChangedBook { get; set; }
    public BAL.Services.ActionResult<Book> ActionResult = new();

    public IFormFile BookFormFile { get; set; }
    public IFormFile ImageFormFile { get; set; }


    public EditBookModel(IServiceBook serviceBook, IServiceGenre serviceGenre)
    {
        _serviceBook = serviceBook;
        _serviceGenre = serviceGenre;
    }

    public void OnGet(int id)
    {
        ChangedBook = _serviceBook.GetBook(id);
    }

    public async Task<IActionResult> OnPostAsync(int id, bool WantToDelete)
    {
        if (WantToDelete)
        {
            _serviceBook.Delete(_serviceBook.GetBook(id));
            return RedirectToPage("/Index");
        }
        BookFormFile = Request.Form.Files["file"];
        ImageFormFile = Request.Form.Files["img"];
        DateTime.TryParse(Request.Form["creatingDate"], out DateTime res);

        Book book = new()
        {
            Title = Request.Form["title"],
            Writer = Request.Form["writer"],
            Description = Request.Form["descr"],
            DateOfCreation = res
        };

        if (!decimal.TryParse(Request.Form["price"], out decimal numPrice) || numPrice< 0) book.Price = -1;
        else book.Price = numPrice;
        
        if (!int.TryParse(Request.Form["ageLimit"], out int ageLimit) || ageLimit< 0) book.AgeLimit = -1;
        else book.AgeLimit = ageLimit;
        
        if (!int.TryParse(Request.Form["pagesCount"], out int pagesCount) || pagesCount< 0) book.Pages = -1;
        else book.Pages = pagesCount;

        ActionResult = await _serviceBook.Edit(id, book, BookFormFile, ImageFormFile);

        ChangedBook = ActionResult.Value;
        if (ActionResult.Succeed)
        {
            _serviceBook.RemoveGenres(ChangedBook, ChangedBook.Genres);
            _serviceBook.AddGenres(
                ChangedBook,
                _serviceGenre.GetAll().Where(genre => Request.Form["genres"].Contains(genre.Id.ToString())).ToArray()
                );
            _serviceBook.Update(ChangedBook);
            return RedirectToPage("/BookPage", new { Id = ActionResult.Value.Id });
        }
        ChangedBook.Genres = _serviceGenre.GetAll().Where(genre => Request.Form["genres"].Contains(genre.Id.ToString())).ToList();
        return Page();
    }
}