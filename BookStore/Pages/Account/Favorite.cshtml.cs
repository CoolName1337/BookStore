using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
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
    public async Task OnGet()
    {
        CurrentUser = _serviceUser.GetUser(User);
        foreach (int bookId in _serviceUser.GetAllFavoriteBooks(CurrentUser).Select(s=>int.Parse(s)))
        {
            Book book = _serviceBook[bookId];
            if (book != null)
            {
                FavBooks.Add(book);
            }
            else
            {
                await _serviceUser.DeleteFavoriteBook(CurrentUser, bookId.ToString());
            }
        }
    }
}
