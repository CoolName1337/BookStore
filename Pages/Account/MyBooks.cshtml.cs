using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account;

public class MyBooksModel : PageModel
{
    private readonly ServiceUser _serviceUser;

    public MyBooksModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _serviceUser = new(userManager, signInManager);
    }

    public List<Book> MyBooks { get; set; } = new();
    public void OnGet()
    {
        MyBooks = _serviceUser[User.Claims.First().Value].AvailableBooks;
    }
}
