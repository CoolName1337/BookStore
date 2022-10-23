using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account
{
    public class MyBooksModel : PageModel
    {
        public List<Book> MyBooks { get; set; } = new();
        public void OnGet()
        {
            using (ApplicationContext db = new())
            {
                MyBooks = Admin.Admin.GetUser(User).AvailableBooks;
                db.SaveChanges();
            }
        }
    }
}
