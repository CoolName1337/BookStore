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
    ServiceBook _serviceBook = new();
    public ActionResult ActionResult = new();
    public Book CreatedBook = new();

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
        if (ActionResult.Succeed)
        {
            return RedirectToPage("/BookPage", new { Id = CreatedBook.Id });
        }
        CreatedBook = ActionResult.Value ?? new();
        return Page();
    }
}
