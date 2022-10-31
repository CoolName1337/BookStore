using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ActionResult = BookStore.BAL.Services.ActionResult<BookStore.DAL.Models.Book>;

namespace BookStore.Pages.Admin;

[Authorize(Roles = "creator,admin")]
public class AddBookModel : PageModel
{
    private readonly IServiceBook _serviceBook;
    public ActionResult ActionResult = new();
    public Book CreatedBook = new();

    public AddBookModel(IServiceBook serviceBook)
    {
        _serviceBook = serviceBook;
    }

    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync()
    {
        ActionResult = await _serviceBook.Add(
            Request.Form["title"],
            Request.Form["writer"],
            Request.Form["descr"],
            Request.Form["price"],
            Request.Form.Files["file"],
            Request.Form.Files["img"]
            );
        CreatedBook = ActionResult.Value ?? new();
        if (ActionResult.Succeed)
        {
            return RedirectToPage("/BookPage", new { Id = ActionResult.Value.Id });
        }
        return Page();
    }
}
