using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Pages
{
    public class BookPageModel : PageModel
    {
        public Book TakedBook;
        public User CurrentUser;

        UserManager<User> _userManager;
        public BookPageModel(UserManager<User> userManager) => _userManager = userManager;
        public void OnGet(int id)
        {
            using (ApplicationContext db = new())
            {

                if (User.Identity.IsAuthenticated)
                {
                    CurrentUser = Admin.Admin.GetUser(User);
                }

                TakedBook = db.Books.Find(id);
            }
        }

        public async Task<IActionResult> OnPost(string interact_btn)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToPage("/Account/Login");

            using (ApplicationContext db = new())
            {
                User user = await _userManager.FindByIdAsync(User.Claims.First().Value);
                Book book = db.Books.Find(int.Parse(Request.Form["Id"]));
                switch (interact_btn)
                {
                    case "dwn":
                        string file = book.SourceFile;
                        return Redirect("files/" + System.Net.WebUtility.UrlEncode(file.Replace("/files/", "")).Replace("+", " "));
                    case "buy":
                        user.BuyBook(book);
                        break;
                    case "fav":
                        user.DeleteFavoriteBook(book.Id.ToString());
                        break;
                    case "unf":
                        user.AddFavoriteBook(book.Id.ToString());
                        break;
                    default:
                        if (int.TryParse(interact_btn, out int rating) && rating > 0 && rating <= 5)
                        {
                            user.RateBook(book, rating);
                        }
                        break;
                }
                db.Update(user);
                db.SaveChanges();
            }

            return RedirectToPage("/BookPage", new { Id = Request.Form["Id"] });
        }

    }
}
