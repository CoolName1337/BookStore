using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BookStore.Pages.Admin
{
    [Authorize(Roles = "creator,admin")]
    public class EditBookModel : PageModel
    {
        public Book ChangedBook { get; set; }
        public List<string> exceptionRes { get; set; } = new();
        public void OnGet(int id)
        {
            using (ApplicationContext db = new()) {
                ChangedBook = db.Books.FirstOrDefault(book => book.Id == id);
            }
        }
        public async Task<IActionResult> OnPostAsync(int id, bool WantToUpdate, bool WantToDelete)
        {
            if (WantToDelete)
            {
                using (ApplicationContext db = new()) {
                    ChangedBook = db.Books.FirstOrDefault(book => book.Id == id);

                    Admin.DeleteFile(ChangedBook.SourceImage);
                    Admin.DeleteFile(ChangedBook.SourceFile);

                    db.Books.Remove(ChangedBook);
                    db.SaveChanges();
                }
                return RedirectToPage("/Index");
            }

            if (!WantToUpdate) return RedirectToPage("/BookPage", new { Id = id });

            using (ApplicationContext db = new()) {
                ChangedBook = db.Books.FirstOrDefault(book => book.Id == id);
            }

            ChangedBook.Title = Request.Form["title"];
            ChangedBook.Description = Request.Form["descr"];
            ChangedBook.Writer = Request.Form["writer"];

            if (Request.Form.Files["file"] != null)
                ChangedBook.SourceFile = await Admin.SaveFile(Request.Form.Files["file"]);
            if (Request.Form.Files["img"] != null)
                ChangedBook.SourceImage = await Admin.SaveFile(Request.Form.Files["img"]);
            if (decimal.TryParse(Request.Form["price"],out decimal price))
                ChangedBook.Price = price;
            else
                exceptionRes.Add($"Поле: PRICE должно быть числом\n");

            foreach (string key in Request.Form.Keys.Except(new List<string> { "file" , "img" , "price", "WantToCreate" }))
                if (string.IsNullOrEmpty(Request.Form[key]))
                    exceptionRes.Add($"Поле: {key.ToUpper()} обязательно\n");
            if (exceptionRes.Count > 0) {
                Admin.DeleteFile(ChangedBook.SourceImage);
                Admin.DeleteFile(ChangedBook.SourceFile);
                return Page();
            }


            using (ApplicationContext db = new())
            {
                db.Books.Update(ChangedBook);
                db.SaveChanges();
            }
            return RedirectToPage("/BookPage", new { Id = ChangedBook.Id });
        }
    }
}
