using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class IndexModel : PageModel
{
    public readonly IServiceUser _serviceUser;
    public readonly IServiceBook _serviceBook;

    public IEnumerable<Book> Books { get; set; }
    public IndexModel(IServiceBook serviceBook, IServiceUser serviceUser)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
    }
    public void OnGet()
    {
        Books = _serviceBook.GetBooks();
    }

}