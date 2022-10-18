using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages
{
    public class SearchResultModel : PageModel
    {
        public Book[] ResultBooks = new Book[0];
        public string? RequestString;

        private bool CheckBook(Book book, string req)
        {
            bool _ = 
                (book.Title?.ToUpper().Contains(req.ToUpper()) ?? false) || 
                (book.Writer?.ToUpper().Contains(req.ToUpper()) ?? false) || 
                (book.Description?.ToUpper().Contains(req.ToUpper()) ?? false);
            return _;
        }

        public void OnGet()
        {

        }

        public void OnPost(string? search)
        {
            if (search == "" || search == " ") return;
            RequestString = search;
            using (ApplicationContext db = new()){
                Book[] allBooks = db.Books.ToArray();
                ResultBooks = (from book in allBooks where CheckBook(book, search ?? "") select book).ToArray();
            }

        }
    }
}
