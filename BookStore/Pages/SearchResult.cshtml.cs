using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Pages;

public class SearchResultModel : PageModel
{
    private Func<Book, bool> searchPredicate;
    private IServiceBook _serviceBook;

    public IEnumerable<Book> ResultBooks;
    public IEnumerable<int> IncludingGenres;
    public IEnumerable<int> ExcludingGenres;

    public string? RequestString;

    public SearchResultModel(IServiceBook serviceBook)
    {
        _serviceBook = serviceBook;
        
        searchPredicate = (book) =>
        {
            return
            // Search by name, writer and description
            (book.Title.ToUpper().Contains(RequestString.ToUpper()) ||
            book.Description.ToUpper().Contains(RequestString.ToUpper())) &&
            // Search by including and excluding genres
            (!IncludingGenres.Except(book.Genres.Select(genre => genre.Id)).Any() &&
            !(book.Genres.Select(genre => genre.Id).Except(ExcludingGenres).Count() < book.Genres.Count()));
        };
    }

    public IActionResult OnPost()
    {
        RequestString = Request.Form["search"].ToString();
        IncludingGenres = Request.Form["IncludingGenres"].Select(str=>int.Parse(str));
        ExcludingGenres = Request.Form["ExcludingGenres"].Select(str=>int.Parse(str));

        ResultBooks = _serviceBook.Books
            .Include(book => book.Genres)
            .Where(searchPredicate);

        return Page();
    }
}