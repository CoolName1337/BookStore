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

        public IActionResult OnPost(string interact_btn, int setRating)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToPage("/Account/Login");
            using (ApplicationContext db = new())
            {
                User user = Admin.Admin.GetUser(User);
                Book book = db.Books.Find(int.Parse(Request.Form["Id"]));
                if ((setRating < 0 || setRating > 5 ? 0 : setRating) != 0)
                {
                    user.RateBook(book, setRating);
                }
                switch (interact_btn)
                {
                    case "dwn":
                        return Redirect("files/" + System.Net.WebUtility.UrlEncode(book.SourceFile.Replace("/files/", "")).Replace("+", " "));
                    case "buy":
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
                db.Update(user);
                db.SaveChanges();
            }
            return RedirectToPage("/BookPage", new { Id = Request.Form["Id"] });
        }

    }
}
