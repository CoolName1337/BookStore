using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Creator;

public class InitializeRolesModel : PageModel
{
    private readonly ServiceUser _serviceUser;
    private readonly ServiceRole _serviceRole;
    public InitializeRolesModel(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _serviceUser = new(userManager, signInManager);
        _serviceRole = new(roleManager);
    }
    public async Task<IActionResult> OnGet()
    {
        if (User.Identity?.IsAuthenticated ?? false && _serviceUser.GetUsers().Count() > 0)
        {
            await _serviceRole.Create("admin");
            await _serviceRole.Create("creator");
            await _serviceUser.AddRole(_serviceUser.GetUser(User), "creator");
        }
        return RedirectToPage("/Index");
    }




}
