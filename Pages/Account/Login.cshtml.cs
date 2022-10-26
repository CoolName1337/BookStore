using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
namespace BookStore.Pages.Account
{
    public class LoginModel : PageModel
    {
        public string Username = "";
        private readonly ServiceUser _serviceUser;

        public LoginModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _serviceUser = new(userManager, signInManager);
        }

        public async Task<IActionResult> OnPostAsync(bool remember)
        {
            var result = await _serviceUser.Login(Request.Form["username"], Request.Form["password"], remember);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError("", "");
            return Page();
        }

        public void OnGet() { }
    }

}
