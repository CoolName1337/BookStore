using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace BookStore.Pages.Creator;


[Authorize(Roles = "creator")]
public class EditRolesModel : PageModel
{
    public readonly IServiceUser _serviceUser;
    public List<User> Users { get; set; } = new();

    public EditRolesModel(IServiceUser serviceUser)
    {
        _serviceUser = serviceUser;
    }
    public IActionResult OnGet()
    {
        foreach (User user in _serviceUser.Users)
        {
            if (user.UserName == User.Identity.Name) continue;
            Users.Add(user);
        }
        return Page();
    }

    public async Task<IActionResult> OnPostSwitchAdminRole(string id)
    {
        User user = _serviceUser.GetUser(id);
        if (_serviceUser.GetRoles(user).Contains("admin"))
        {
            await _serviceUser.RemoveRole(user, "admin");
        }
        else
        {
            await _serviceUser.AddRole(user, "admin");
        }
        return new JsonResult(_serviceUser.GetRoles(user).Contains("admin"));
    }

    public async Task OnDeleteUser(string id)
    {
        await _serviceUser.Delete(id);
    }

    public IActionResult OnPostFindUsers(string req)
    {
        req = req?.ToUpper() ?? "";
        Users = _serviceUser.Users
            .Where(user =>
            (user.NormalizedUserName.Contains(req) ||
            user.NormalizedEmail.Contains(req)) &&
            user.UserName != User.Identity.Name
            ).ToList();
        return new PartialViewResult()
        {
            ViewName = "_UsersContainer",
            ViewData = this.ViewData
        };
    }
}
