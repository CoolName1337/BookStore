using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class SearchResultModel : PageModel
{
    public IEnumerable<Book> ResultBooks;
    public string? RequestString;
    private ServiceBook _serviceBook = new();

    public void OnGet(string search)
    {
        if (string.IsNullOrEmpty(search) || search == " ") return;
        RequestString = search;
        ResultBooks = _serviceBook.GetBooks((book) =>
            book.Title.ToUpper().Contains(search.ToUpper()) ||
            book.Writer.ToUpper().Contains(search.ToUpper()) ||
            book.Description.ToUpper().Contains(search.ToUpper())
            );
    }

}
