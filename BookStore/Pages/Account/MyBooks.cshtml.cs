using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account;

public class MyBooksModel : PageModel
{
    private readonly IServiceUser _serviceUser;

    public MyBooksModel(IServiceUser serviceUser)
    {
        _serviceUser = serviceUser;
    }

    public List<Book> MyBooks { get; set; } = new();
    public void OnGet()
    {
        MyBooks = _serviceUser[User.Claims.First().Value].AvailableBooks;
    }
}
