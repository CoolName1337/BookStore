using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BookStore.Pages.Admin
{
    public class AddBookModel : PageModel
    {
        [BindProperty]
        public Book CreatedBook { get; set; } = new();
        public void OnGet()
        {
        }
        [BindProperty]
        public IFormFile UploadImg { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            string filePath = Path.Combine("wwwroot/images/", UploadImg.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                CreatedBook.Source = $"/images/{UploadImg.FileName}";
                await UploadImg.CopyToAsync(fileStream);
            }
            using(ApplicationContext db = new())
            {
                db.Books.Add(CreatedBook);
                db.SaveChanges();
            }
            return RedirectToPage("/BookPage", new { Id = CreatedBook.Id });
        }   
    }
}
