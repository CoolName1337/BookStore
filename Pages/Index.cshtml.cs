using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class IndexModel : PageModel
{
    public ServiceBook _serviceBook = new();
    public readonly ServiceUser _serviceUser;
    public IEnumerable<Book> Books { get; set; }

    public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _serviceUser = new(userManager, signInManager);
    }
    public void OnGet()
    {
        Books = _serviceBook.GetBooks();
    }

}