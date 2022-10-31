using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly IServiceUser _serviceUser;

    public LogoutModel(IServiceUser serviceUser)
    {
        _serviceUser = serviceUser;
    }
    public void OnGet() { }

    public async Task<IActionResult> OnPost(bool IsLogout)
    {
        if (IsLogout) await _serviceUser.Logout();
        return RedirectToPage("/Index");
    }

}
