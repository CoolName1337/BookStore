using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class BookPageModel : PageModel
{
    private readonly IServiceBook _serviceBook;
    public readonly IServiceRating _serviceRating;
    public readonly IServiceUser _serviceUser;
    public readonly IServiceFavorite _serviceFavorite;
    public Book TakedBook { get; set; }
    public User CurrentUser { get; set; }

    public BookPageModel(IServiceBook serviceBook, IServiceUser serviceUser, IServiceRating serviceRating, IServiceFavorite serviceFavorite)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
        _serviceRating = serviceRating;
        _serviceFavorite = serviceFavorite;
    }
    public IActionResult OnGet(int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            CurrentUser = _serviceUser.GetUser(User);
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

        CurrentUser = _serviceUser.GetUser(User);
        TakedBook = _serviceBook[int.Parse(Request.Form["id"])];
        switch (interact_btn)
        {
            case "dwn":
                return Redirect(_serviceBook.GetCorrectPath(TakedBook));
            case "buy":
                await _serviceUser.TryBuyBook(CurrentUser, TakedBook);
                break;
            default:
                if (int.TryParse(interact_btn, out int rating) && rating > 0 && rating <= 5)
                {
                    await _serviceRating.TryRateBook(CurrentUser, TakedBook, rating);
                }
                break;
        }
        return RedirectToPage("/BookPage", new { Id = Request.Form["Id"] });
    }

    public async Task<IActionResult> OnPostRateBook(int id, int stars)
    {
        CurrentUser = _serviceUser.GetUser(User);
        TakedBook = _serviceBook[id]; 
        await _serviceRating.TryRateBook(CurrentUser, TakedBook, stars);
        double rating = _serviceRating.GetRating(TakedBook);
        double count = _serviceRating.GetRatingCount(TakedBook);
        return new JsonResult(new { stars, rating, count });
    }


    public async Task<IActionResult> OnPostFavorite(int id)
    {
        CurrentUser = _serviceUser.GetUser(User);
        TakedBook = _serviceBook[id];
        return new JsonResult(await _serviceFavorite.TryLike(CurrentUser, TakedBook));
    }
}