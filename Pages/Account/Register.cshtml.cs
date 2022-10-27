using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly ServiceUser _serviceUser;
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";

    public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _serviceUser = new(userManager, signInManager);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Username = Request.Form["username"];
        Email = Request.Form["email"];

        var result = await _serviceUser.Register(
            Request.Form["username"],
            Request.Form["email"],
            Request.Form["passwordConfirm"],
            Request.Form["password"]
            );

        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }

        ModelState.AddModelError("", result.Errors.First().Description);
        return Page();
    }
    public void OnGet() { }
}

