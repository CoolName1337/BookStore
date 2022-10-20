using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        public LogoutModel(SignInManager<User> signInManager) => _signInManager = signInManager;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(bool IsLogout)
        {
            if (IsLogout) await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }

    }
}
