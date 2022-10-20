using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<Book> Books { get; set; } = new List<Book>();

        public void OnGet()
        {
            using(ApplicationContext db = new())
            {
                Books = db.Books.ToList();
            }
        }


    }
}