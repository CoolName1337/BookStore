using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace BookStore.Pages
{
    public class FilterBooksPageModel : PageModel
    {
        public readonly IServiceUser _serviceUser;
        public readonly IServiceBook _serviceBook;
        public readonly IServiceRating _serviceRating;
        public string FilterString { get; set; } = "";
        public Func<int, IEnumerable<Book>> FilterFunc;

        public FilterBooksPageModel(IServiceBook serviceBook, IServiceUser serviceUser, IServiceRating serviceRating)
        {
            _serviceUser = serviceUser;
            _serviceBook = serviceBook;
            _serviceRating = serviceRating;
        }
        
        public IActionResult OnGet(string filterString)
        {
            filterString = filterString ?? string.Empty;
            switch (filterString.ToUpper())
            {
                case "DATE":
                    FilterString = "Новое";
                    FilterFunc = _serviceBook.GetBooks().OrderBy(book => book.Bought).Take;
                    break;
                case "POPULARITY":
                    FilterString = "Популярное";
                    FilterFunc = _serviceBook.GetBooks().OrderBy(book => book.AddingDate).Take;
                    break;
                case "RATING":
                    FilterString = "Топ по оценкам";
                    FilterFunc = _serviceBook.GetBooks().OrderBy(book => _serviceRating.GetRating(book)).Take;
                    break;
                default:
                    return BadRequest();
            }
            return Page();
        }
    }
}
