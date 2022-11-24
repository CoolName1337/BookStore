using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        TakedBook = _serviceBook.GetBook(id);
        if (TakedBook == null)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }

    public IActionResult OnGetDwnBook(int bookId)
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("/Account/Login");
        return new JsonResult(_serviceBook.GetCorrectPath(_serviceBook.Books.Find(bookId)));
    }

    public async Task<IActionResult> OnPostBuyBook(int bookId)
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("/Account/Login");
        var user = _serviceUser.Users.First(user => user.Id == User.Claims.First().Value);
        var book = _serviceBook.Books.Find(bookId);
        await _serviceUser.TryBuyBook(user, book);
        return new JsonResult("Success");
    }

    private void PostInitialize(int bookId)
    {
        CurrentUser = _serviceUser.GetUser(User);
        TakedBook = _serviceBook.GetBook(bookId);
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
        TakedBook = _serviceBook.GetBook(bookId);
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
        var userReview = CurrentUser.Reviews.Find(review => review.Id == reviewId);
        if (User.IsInRole("admin") || User.IsInRole("creator"))
        {
            userReview = _serviceUser.GetUser(userId)?.Reviews.Find(review => review.Id == reviewId);
        }
        await _serviceReview.DeleteReview(userReview);
        return new JsonResult("kaka");
    }
    public async Task<IActionResult> OnPostReviewRate(int reviewId, string userId, bool like)
    {
        CurrentUser = _serviceUser.GetUser(User);
        var review = _serviceUser.GetUser(userId)?.Reviews.Find(review => review.Id == reviewId);
        await _serviceReview.TryRateReview(CurrentUser, review, like);
        return new JsonResult(
            new
            {
                likes = review.Rates.Count(rate => rate.Like),
                dislikes = review.Rates.Count(rate => !rate.Like)
            }
            );
    }

    public IActionResult OnGetReviewSort(int bookId, string req)
    {
        PostInitialize(bookId);
        ViewData["req"] = req;
        return new PartialViewResult()
        {
            ViewName = "_ReviewsForBookPage",
            ViewData = this.ViewData
        };
    }

}