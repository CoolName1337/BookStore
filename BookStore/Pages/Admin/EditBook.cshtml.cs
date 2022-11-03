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

        ActionResult = await _serviceBook.Edit(
            id,
            Request.Form["title"],
            Request.Form["writer"],
            Request.Form["descr"],
            Request.Form["price"],
            Request.Form.Files["file"],
            Request.Form.Files["img"]
            );

        ChangedBook = ActionResult.Value;
        if (ActionResult.Succeed)
        {
            return RedirectToPage("/BookPage", new { Id = ChangedBook.Id });
        }
        return Page();
    }
}