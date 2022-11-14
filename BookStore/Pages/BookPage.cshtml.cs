using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;

namespace BookStore.Pages;

public class BookPageModel : PageModel
{
    private readonly IServiceBook _serviceBook;
    public readonly IServiceRating _serviceRating;
    public readonly IServiceUser _serviceUser;
    public readonly IServiceFavorite _serviceFavorite;
    public readonly IServiceReview _serviceReview;
    public Book TakedBook { get; set; }
    public User CurrentUser { get; set; }

    public BookPageModel(
        IServiceBook serviceBook, IServiceUser serviceUser, IServiceRating serviceRating,
        IServiceFavorite serviceFavorite, IServiceReview serviceReview)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
        _serviceRating = serviceRating;
        _serviceFavorite = serviceFavorite;
        _serviceReview = serviceReview;
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

    private void PostInitialize(int bookId)
    {
        CurrentUser = _serviceUser.GetUser(User);
        TakedBook = _serviceBook[bookId];
    }
    public async Task<IActionResult> OnPostRateBook(int bookId, int stars)
    {
        PostInitialize(bookId);
        await _serviceRating.TryRateBook(CurrentUser, TakedBook, stars);
        double rating = _serviceRating.GetRating(TakedBook);
        double count = _serviceRating.GetRatingCount(TakedBook);
        double userRating = _serviceRating.GetUserRating(CurrentUser, TakedBook);
        return new JsonResult(new { userRating, rating, count });
    }


    public async Task<IActionResult> OnPostFavorite(int bookId)
    {
        CurrentUser = _serviceUser.GetUser(User);
        TakedBook = _serviceBook[bookId];
        bool res = await _serviceFavorite.TryLike(CurrentUser, TakedBook);
        return new JsonResult(res);
    }

    public async Task OnPostReview(int bookId, string text)
    {
        PostInitialize(bookId);
        await _serviceReview.AddReview(TakedBook, CurrentUser, text);
    }
    public async Task<IActionResult> OnDeleteReview(int reviewId, string userId)
    {
        CurrentUser = _serviceUser.GetUser(User);
        var userReview = CurrentUser.Reviews.Find(review => review.Id==reviewId);
        if (User.IsInRole("admin") || User.IsInRole("creator"))
        {
            userReview = _serviceUser[userId].Reviews.Find(review => review.Id == reviewId);
        }
        await _serviceReview.DeleteReview(userReview);
        return new JsonResult("kaka");
    }
}