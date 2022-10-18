using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookStore.Models;

namespace BookStore.Pages
{
    public class BookPageModel : PageModel
    {
        public Book TakedBook = null;

        public void OnGet()
        {
            string id = Request.Query["id"];
            using (ApplicationContext db = new())
            {
                foreach (Book book in db.Books.ToArray())
                {
                    if (book.Id == int.Parse(id))
                    {
                        TakedBook = book;
                        return;
                    }
                }
            }
        }

    }
}
