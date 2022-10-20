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
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

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

        private async Task<IActionResult> Login()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToPage("/Index");    
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            return await Login();
        }
    }
}
