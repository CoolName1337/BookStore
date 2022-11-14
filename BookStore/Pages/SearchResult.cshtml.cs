using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
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

    public IActionResult OnPost()
    {
        RequestString = Request.Form["search"];
        var IncludingGenres = Request.Form["IncludingGenres"].Select(str=>int.Parse(str));
        var ExcludingGenres = Request.Form["ExcludingGenres"].Select(str=>int.Parse(str));

        ResultBooks = _serviceBook.GetBooks((book) =>
        {
            bool res =
            // Search by name, writer and description
            (book.Title.ToUpper().Contains(RequestString.ToUpper()) ||
            book.Writer.ToUpper().Contains(RequestString.ToUpper()) ||
            book.Description.ToUpper().Contains(RequestString.ToUpper())) &&
            // Search by including and excluding genres
            (!IncludingGenres.Except(book.Genres.Select(genre => genre.Id)).Any() &&
            !(book.Genres.Select(genre => genre.Id).Except(ExcludingGenres).Count() < book.Genres.Count()));
            return res;
        });
        return Page();
    }
}