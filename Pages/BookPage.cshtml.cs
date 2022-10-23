using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System;

namespace BookStore.Pages
{
    public class BookPageModel : PageModel
    {
        public Book TakedBook;
        public User user;
        public void OnGet(int id)
        {
            using (ApplicationContext db = new())
            {

                if (User.Identity.IsAuthenticated)
                {
                    user = Admin.Admin.GetUser(User);
                }

                TakedBook = db.Books.Find(id);
            }
        }

        public IActionResult OnPost(string interact_btn)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToPage("/Account/Login");

            using (ApplicationContext db = new())
            {
                string id = User.Claims.First().Value;
                user = db.Users.Find(id);
                switch (interact_btn)
                {
                    case "dwn":
                        string file = db.Books.Find(int.Parse(Request.Form["Id"])).SourceFile;
                        return Redirect(file);
                        break;
                    case "buy":
                        Book book = db.Books.Find(int.Parse(Request.Form["Id"]));
                        user.BuyBook(book);
                        break;
                    case "fav":
                        user.DeleteFavoriteBook(Request.Form["Id"]);
                        break;
                    case "unf":
                        user.AddFavoriteBook(Request.Form["Id"]);
                        break;
                    default:
                        break;
                }
                db.SaveChanges();
            }
            
            return RedirectToPage("/BookPage", new { Id = Request.Form["Id"] });
        }

    }
}
