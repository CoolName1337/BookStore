using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages.Account;

public class ProfilePageModel : PageModel
{
    public IServiceUser _serviceUser { get; init; }
    public IServiceBook _serviceBook { get; init; }
    public IServiceRating _serviceRating { get; init; }
    public User ProfileUser { get; set; }
    public List<Book> AvailableBooks { get; set; } = new();
    public List<Book> FavoriteBooks { get; set; } = new();
    public ProfilePageModel(IHttpContextAccessor httpContextAccessor, IServiceUser serviceUser,
        IServiceBook serviceBook, IServiceRating serviceRating)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
        _serviceRating = serviceRating;
    }

    public void OnGet(string userName)
    {
        SetUserData(userName);
    }

    private void SetUserData(string userName)
    {
        ProfileUser = _serviceUser.Users
            .Include(user => user.AvailableBooks)
            .Include(user => user.Favorites)
            .Include(user => user.Reviews)
                .ThenInclude(review => review.Rates)
            .First(user => user.UserName == userName);

        AvailableBooks = ProfileUser.AvailableBooks;

        foreach (var bookId in ProfileUser.Favorites.Select(favBook => favBook.BookId)) FavoriteBooks.Add(_serviceBook.GetBook(bookId));
        FavoriteBooks.Remove(null);
    }

    public IActionResult OnGetProfilePicture()
    {
        return new JsonResult(_serviceUser.GetUser(User)?.ProfilePicture);
    }

    public async Task<IActionResult> OnPostProfilePicture(string userName, IFormFile image)
    {
        if (userName != User.Identity.Name)
        {
            throw new Exception("Ошибка доступа");
        }
        if (image == null)
        {
            throw new Exception("Файл не выбран");
        }
        if (!image.ContentType.StartsWith("image"))
        {
            throw new Exception("Файл не подходит");
        }
        SetUserData(userName);
        await _serviceUser.UpdateProfilePicture(ProfileUser, image);
        return new JsonResult(ProfileUser.ProfilePicture);
    }
    public IActionResult OnGetAvailableBooks(string userName)
    {
        SetUserData(userName);
        return new PartialViewResult()
        {
            ViewName = "_MyBooks",
            ViewData = this.ViewData,
        };
    }
    public IActionResult OnGetFavorites(string userName)
    {
        SetUserData(userName);
        return new PartialViewResult()
        {
            ViewName = "_MyFavorites",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnGetReviews(string userName)
    {
        SetUserData(userName);
        return new PartialViewResult()
        {
            ViewName = "_MyReviews",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnGetSettings(string userName)
    {
        SetUserData(userName);
        return new PartialViewResult()
        {
            ViewName = "_ProfileSettings",
            ViewData = this.ViewData
        };
    }
}
