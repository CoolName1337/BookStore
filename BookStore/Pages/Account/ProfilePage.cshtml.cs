using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account;

public class ProfilePageModel : PageModel
{
    public IServiceUser _serviceUser { get; init; }
    public IServiceBook _serviceBook { get; init; }
    public IServiceRating _serviceRating { get; init; }
    public User ProfileUser { get; init; }
    public List<Book> AvailableBooks { get; init; } = new();
    public List<Book> FavoriteBooks { get; init; } = new();
    public ProfilePageModel(IHttpContextAccessor httpContextAccessor, IServiceUser serviceUser, 
        IServiceBook serviceBook, IServiceRating serviceRating)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
        _serviceRating = serviceRating;

        ProfileUser = _serviceUser.GetUser(httpContextAccessor.HttpContext.User);
        AvailableBooks = ProfileUser.AvailableBooks;

        foreach (var bookId in ProfileUser.Favorites.Select(favBook => favBook.BookId)) FavoriteBooks.Add(_serviceBook[bookId]);
        FavoriteBooks.Remove(null);
    }

    public IActionResult OnGetMyBooks()
    {
        return new PartialViewResult()
        {
            ViewName = "_MyBooks",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnGetMyFavorites()
    {
        return new PartialViewResult()
        {
            ViewName = "_MyFavorites",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnGetMyReviews()
    {
        return new PartialViewResult()
        {
            ViewName = "_MyReviews",
            ViewData = this.ViewData
        };
    }
}
