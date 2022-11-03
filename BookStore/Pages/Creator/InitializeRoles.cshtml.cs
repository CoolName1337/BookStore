using BookStore.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages.Creator;

public class InitializeRolesModel : PageModel
{
    private readonly IServiceUser _serviceUser;
    private readonly IServiceRole _serviceRole;
    public InitializeRolesModel(IServiceUser serviceUser, IServiceRole serviceRole)
    {
        _serviceUser = serviceUser;
        _serviceRole = serviceRole;
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