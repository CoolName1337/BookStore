using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Pages.Account
{
    public class FavoriteModel : PageModel
    {
        public List<Book> FavBooks { get; set; } = new();
        public void OnGet()
        {
            using (ApplicationContext db = new())
            {
                string Id = User.Claims.FirstOrDefault().Value;
                User user = db.Users.Find(Id);
                foreach (string bookId in user.FavoriteBooks.Split(" "))
                {
                    if (int.TryParse(bookId, out int id))
                    {
                        Book book = db.Books.Find(id);
                        if (book != null)
                        {
                            FavBooks.Add(book);
                        }
                        else
                        {
                            user.DeleteFavoriteBook(bookId);
                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
