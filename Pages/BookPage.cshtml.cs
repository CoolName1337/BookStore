using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class BookPageModel : PageModel
{
    public readonly ServiceUser serviceUser;
    private readonly ServiceBook _serviceBook = new();
    public Book TakedBook { get; set; }
    public User CurrentUser { get; set; }

    public BookPageModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        serviceUser = new(userManager, signInManager);
    }
    public IActionResult OnGet(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            CurrentUser = serviceUser.GetUser(User);
        }
        TakedBook = _serviceBook[id];
        if (TakedBook == null)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPost(string interact_btn)
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("/Account/Login");

        CurrentUser = serviceUser.GetUser(User);
        TakedBook = _serviceBook[int.Parse(Request.Form["Id"])];
        switch (interact_btn)
        {
            case "dwn":
                return Redirect(_serviceBook.GetCorrectPath(TakedBook));
            case "buy":
                await serviceUser.TryBuyBook(CurrentUser, TakedBook);
                break;
            case "fav":
                await serviceUser.DeleteFavoriteBook(CurrentUser, TakedBook.Id.ToString());
                break;
            case "unf":
                await serviceUser.AddFavoriteBook(CurrentUser, TakedBook.Id.ToString());
                break;
            default:
                if (int.TryParse(interact_btn, out int rating) && rating > 0 && rating <= 5)
                {
                    CurrentUser.RateBook(TakedBook, rating);
                }
                break;
        }
        await _serviceBook.Update(TakedBook);
        await serviceUser.UpdateUser(CurrentUser);

        return RedirectToPage("/BookPage", new { Id = Request.Form["Id"] });
    }

}
