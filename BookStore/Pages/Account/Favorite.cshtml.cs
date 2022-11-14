using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account;

public class FavoriteModel : PageModel
{
    public readonly IServiceUser _serviceUser;
    private readonly IServiceBook _serviceBook;
    public User CurrentUser;
    public List<Book> FavBooks { get; set; } = new();

    public FavoriteModel(IServiceUser serviceUser, IServiceBook serviceBook)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
    }

    public void OnGet()
    {
        CurrentUser = _serviceUser.GetUser(User);
        foreach (var bookId in CurrentUser.Favorites.Select(favBook => favBook.BookId))
        {
            Book book = _serviceBook[bookId];
            if(book != null) FavBooks.Add(book);
        } 
    }
}
