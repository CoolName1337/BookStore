using BookStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BookStore.Pages.Account
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel model { get; set; } = new(); 
        private readonly SignInManager<User> _signInManager;
        public LoginModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        public void OnGet()
        {

        }

        private async Task<IActionResult> Login(bool remember)
        {
            model = new LoginViewModel() { Username = Request.Form["username"], Password = Request.Form["password"] };
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, remember, false);
                if (result.Succeeded)
                    return RedirectToPage("/Index");
            }
            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            return Page();
        }

        public async Task<IActionResult> OnPost(bool remember)
        {
            return await Login(remember);
        }
    }
}
