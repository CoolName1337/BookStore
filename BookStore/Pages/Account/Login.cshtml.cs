using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account;
public class LoginModel : PageModel
{
    private readonly IServiceUser _serviceUser;
    public string Username { get; set; } = "";

    public LoginModel(IServiceUser serviceUser)
    {
        _serviceUser = serviceUser;
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

