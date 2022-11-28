using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Admin;

[Authorize(Roles = "creator,admin")]
public class EditBookModel : PageModel
{
    private readonly IServiceBook _serviceBook;
    public readonly IServiceGenre _serviceGenre;
    public readonly IServiceAuthor _serviceAuthor;
    public Book ChangedBook { get; set; }
    public BAL.Services.ActionResult<Book> ActionResult = new();

    public IFormFile BookFormFile { get; set; }
    public IFormFile ImageFormFile { get; set; }


    public EditBookModel(IServiceBook serviceBook, IServiceGenre serviceGenre, IServiceAuthor serviceAuthor)
    {
        _serviceBook = serviceBook;
        _serviceGenre = serviceGenre;
        _serviceAuthor = serviceAuthor;
    }

    public void OnGet(int id)
    {
        ChangedBook = _serviceBook.GetBook(id);
    }

    public IActionResult OnPostAsync(int id, bool WantToDelete)
    {
        _serviceBook.Delete(_serviceBook.Books.Find(id));
        return RedirectToPage("/Index");
    }

    public void OnPostDeleteBook(int id)
    {
        _serviceBook.Delete(_serviceBook.GetBook(id));
    }
}