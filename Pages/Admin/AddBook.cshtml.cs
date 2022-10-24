using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace BookStore.Pages.Admin
{
    public static class Admin{
        public static async Task<string> SaveFile(IFormFile file)
        {
            bool isImage = file.ContentType.StartsWith("image");
            string type = isImage ? "images" : "files";
            string filePath = Path.Combine($"/{type}/", file.FileName);
            using (var fileStream = new FileStream("wwwroot"+filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filePath;
        }
        public static void DeleteFile(string file)
        {
            if (File.Exists("wwwroot" + file))
                File.Delete("wwwroot" + file);
        }
        public static User GetUser(ClaimsPrincipal claims)
        {
            User user;
            using (ApplicationContext db = new())
            {
                string Id = claims.Claims.First().Value;
                user = db.Users.Include(user => user.AvailableBooks).First(user => user.Id == Id);
                db.Update(user);
                db.SaveChanges();
            }
            return user;
        }

    }
    public class AddBookModel : PageModel
    {
        public Book CreatedBook = new();
        public List<string> exceptionRes { get; set; } = new();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(bool WantToCreate)
        {

            if (!WantToCreate) return RedirectToPage("/Index");
            CreatedBook = new()
            {
                Title = Request.Form["title"],
                Writer = Request.Form["writer"],
                Description = Request.Form["descr"],
                Price = decimal.Parse(Request.Form["price"])
            };
            if (Request.Form.Files["file"] != null)
                CreatedBook.SourceFile = await Admin.SaveFile(Request.Form.Files["file"]);
            if (Request.Form.Files["img"] != null)
                CreatedBook.SourceImage = await Admin.SaveFile(Request.Form.Files["img"]);
            if (decimal.TryParse(Request.Form["price"], out decimal price))
                CreatedBook.Price = price;
            else
                exceptionRes.Add($"Поле: PRICE должно быть только числом\n");

            foreach (string key in Request.Form.Keys.Except(new List<string> { "price", "WantToCreate" }))
                if (string.IsNullOrEmpty(Request.Form[key]))
                    exceptionRes.Add($"Поле: {key.ToUpper()} обязательно\n");
            if (exceptionRes.Count > 0)
            {
                Admin.DeleteFile(CreatedBook.SourceImage);
                Admin.DeleteFile(CreatedBook.SourceFile);
                return Page();
            }

            using (ApplicationContext db = new())
            {
                db.Books.Add(CreatedBook);
                db.SaveChanges();
            }
            return RedirectToPage("/BookPage", new { Id = CreatedBook.Id });
        }
    }
}
