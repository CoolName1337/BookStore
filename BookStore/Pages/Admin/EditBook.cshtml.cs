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
    public Book ChangedBook { get; set; }
    public BAL.Services.ActionResult<Book> ActionResult = new();

    public IFormFile BookFormFile { get; set; }
    public IFormFile ImageFormFile { get; set; }


    public EditBookModel(IServiceBook serviceBook)
    {
        _serviceBook = serviceBook;
    }

    public void OnGet(int id)
    {
        ChangedBook = _serviceBook[id];
    }

    public async Task<IActionResult> OnPostAsync(int id, bool WantToUpdate, bool WantToDelete)
    {
        if (WantToDelete)
        {
            _serviceBook.Delete(_serviceBook[id]);
            return RedirectToPage("/Index");
        }
        if (!WantToUpdate)
        {
            return RedirectToPage("/BookPage", new { Id = id });
        }

        BookFormFile = Request.Form.Files["file"];
        ImageFormFile = Request.Form.Files["img"];

        Book book = new()
        {
            Title = Request.Form["title"],
            Writer = Request.Form["writer"],
            Description = Request.Form["descr"],
        };
        if (!decimal.TryParse(Request.Form["price"], out decimal numPrice) || numPrice < 0)
        {
            book.Price = -1;
        }
        else
        {
            book.Price = numPrice;
        }

        ActionResult = await _serviceBook.Edit(id, book, BookFormFile, ImageFormFile);

        ChangedBook = ActionResult.Value;
        if (ActionResult.Succeed)
        {
            return RedirectToPage("/BookPage", new { Id = ChangedBook.Id });
        }
        return Page();
    }
}