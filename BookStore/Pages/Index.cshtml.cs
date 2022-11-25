using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Pages;

public class IndexModel : PageModel
{
    public readonly IServiceUser _serviceUser;
    public readonly IServiceBook _serviceBook;
    public readonly IServiceGenre _serviceGenre;
    public List<Book> Books { get; set; }
    public int PageSize { get; init; } = 30;

    public IndexModel(IServiceBook serviceBook, IServiceUser serviceUser, IServiceGenre serviceGenre)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
        _serviceGenre = serviceGenre;
    }
    public IActionResult OnGet()
    {
        Books = _serviceBook.Books.Take(PageSize).ToList();
        return Page();
    }
    public IActionResult OnGetBooksPage(int? id)
    {
        int page = id ?? 0;
        Books = _serviceBook.Books.Skip(page * PageSize).Take(PageSize).ToList();
        return new PartialViewResult()
        {
            ViewName = "_BooksView",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnGetSelectAllGenres()
    {
        return new JsonResult(_serviceGenre.GetAll());
    }
}