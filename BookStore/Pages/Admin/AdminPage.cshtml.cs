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

    public AdminPageModel(IServiceBook serviceBook, IServiceGenre serviceGenre)
    {
        _serviceBook = serviceBook;
        _serviceGenre = serviceGenre;
    }

    public IActionResult OnGet()
    {
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(string postType)
    {
        IActionResult actRes = Page();
        switch (postType)
        {
            case "createBook":
                actRes = await TryCreateBook(Request);
                break;
        }

        return actRes;
    }

    public IActionResult OnGetSelectAll()
    {
        return new JsonResult(_serviceGenre.GetAll());
    }

    public async Task<IActionResult> OnPostCreate(string genreName)
    {
        Genre genre = await _serviceGenre.Create(genreName);
        if (genre != null)
        {
            return new JsonResult(genre);
        }
        return new ConflictResult();
    }

    public async Task<IActionResult> OnDeleteDelete(string genreName)
    {
        await _serviceGenre.Delete(genreName);
        return new JsonResult(genreName);
    }

    private async Task<IActionResult> TryCreateBook(HttpRequest Request)
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