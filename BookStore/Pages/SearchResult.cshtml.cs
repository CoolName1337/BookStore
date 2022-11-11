using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using BookStore.Models;
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

    public void OnPost([FromBody]Filter filter) 
    {
        if (string.IsNullOrEmpty(filter.SearchRequest) || filter.SearchRequest == " ") return;
        RequestString = filter.SearchRequest;
        ResultBooks = _serviceBook.GetBooks((book) =>
        {
            bool res = book.Title.ToUpper().Contains(filter.SearchRequest.ToUpper()) ||
            book.Writer.ToUpper().Contains(filter.SearchRequest.ToUpper()) ||
            book.Description.ToUpper().Contains(filter.SearchRequest.ToUpper());
            return res;

        });
    }

}