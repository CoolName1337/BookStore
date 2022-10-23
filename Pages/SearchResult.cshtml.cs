using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages
{
    public class SearchResultModel : PageModel
    {
        public Book[] ResultBooks;
        public string? RequestString;
        public void OnGet(string search)
        {
            if (string.IsNullOrEmpty(search) || search == " ") return;
            RequestString = search;
            using (ApplicationContext db = new())
            {
                ResultBooks = db.Books.Where((book) =>
                book.Title.ToUpper().Contains(search.ToUpper()) ||
                book.Writer.ToUpper().Contains(search.ToUpper()) ||
                book.Description.ToUpper().Contains(search.ToUpper())).ToArray();
            }
        }

    }
}
