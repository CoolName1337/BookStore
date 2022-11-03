using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class SearchResultModel : PageModel
{
    private IServiceBook _serviceBook;
    public IEnumerable<Book> ResultBooks;
    public string? RequestString;

    public SearchResultModel(IServiceBook serviceBook)
    {
        _serviceBook = serviceBook;
    }

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