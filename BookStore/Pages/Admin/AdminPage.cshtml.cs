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

    public IActionResult OnGet() {
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
            case "editGenres":
                if (Request.Form["editGenreBtn"].ToString().StartsWith("del"))
                {
                    actRes = await TryDeleteGenre(Request);
                }
                else
                {
                    actRes = await TryAddGenre(Request);
                }
                break;
        }

        return actRes;
    }

    private async Task<IActionResult> TryAddGenre(HttpRequest Request)
    {
        await _serviceGenre.Create(Request.Form["genreName"]);
        return Page();
    }

    private async Task<IActionResult> TryDeleteGenre(HttpRequest Request)
    {
        await _serviceGenre.Delete(Request.Form["editGenreBtn"].ToString().Split("_")[1]);
        return Page();
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