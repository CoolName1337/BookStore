using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class IndexModel : PageModel
{
    public readonly IServiceUser _serviceUser;
    public readonly IServiceBook _serviceBook;
    public readonly IServiceGenre _serviceGenre;
    public IEnumerable<Book> Books { get; set; }

    public IndexModel(IServiceBook serviceBook, IServiceUser serviceUser, IServiceGenre serviceGenre)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
        _serviceGenre = serviceGenre;
    }
    public void OnGet()
    {
        Books = _serviceBook.Books;
    }
    public IActionResult OnGetSelectAllGenres()
    {
        return new JsonResult(_serviceGenre.GetAll());
    }
}