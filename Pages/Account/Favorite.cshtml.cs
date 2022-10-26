using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account;

public class FavoriteModel : PageModel
{
    public readonly ServiceUser _serviceUser;
    public readonly ServiceBook _serviceBook = new();
    public User CurrentUser;

    public List<Book> FavBooks { get; set; }


    public FavoriteModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _serviceUser = new(userManager, signInManager);
    }
    public void OnGet()
    {
        string Id = User.Claims.FirstOrDefault().Value;
        CurrentUser = _serviceUser[Id];
        foreach (int bookId in _serviceUser.GetAllFavoriteBooks(CurrentUser).Select(s=>int.Parse(s)))
        {
            Book book = _serviceBook[bookId];
            if (book != null)
            {
                FavBooks.Add(book);
            }
            else
            {
                _serviceUser.DeleteFavoriteBook(CurrentUser, bookId.ToString());
            }
        }
    }
}
