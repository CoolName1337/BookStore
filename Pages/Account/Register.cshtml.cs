using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Pages.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirm { get; set; }
    }
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel model { get; set; } = new RegisterViewModel();
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        private async Task<IActionResult> Register()
        {
            model = new RegisterViewModel() { Username = Request.Form["username"], Email=Request.Form["email"], 
                Password = Request.Form["password"], PasswordConfirm = Request.Form["passwordConfirm"] };
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Username};

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToPage("/Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("error", error.Description);
                    }
                }
            }
            return Page();

        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            return await Register();
        }
    }
}
