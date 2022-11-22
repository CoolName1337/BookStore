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
        ProfileUser = _serviceUser.GetUserByName(userName);

        AvailableBooks = ProfileUser.AvailableBooks;

        foreach (var bookId in ProfileUser.Favorites.Select(favBook => favBook.BookId)) FavoriteBooks.Add(_serviceBook[bookId]);
        FavoriteBooks.Remove(null);
    }

    public async Task<IActionResult> OnPostProfilePicture(string userName, IFormFile image)
    {
        if (userName == User.Identity.Name)
        {
            SetUserData(userName);
            if (!image.ContentType.StartsWith("image")) throw new Exception();
            var user = _serviceUser.GetUser(User);
            await _serviceUser.UpdateProfilePicture(user, image);
            return new JsonResult(user.ProfilePicture);
        }
        throw new Exception();
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
