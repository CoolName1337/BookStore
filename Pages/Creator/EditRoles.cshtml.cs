using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BookStore.Pages.Creator
{
    public class UserRoleModel
    {
        public User currentUser { get; set; }
        public List<string> userRoles { get; set; }

    }

    [Authorize(Roles = "creator")]
    public class EditRolesModel : PageModel
    {
        public string SearchReq { get; set; }
        public List<UserRoleModel> Users { get; set; } = new();
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        public EditRolesModel(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet(string searchReq)
        {
            using (ApplicationContext db = new())
            {
                foreach (User user in db.Users.ToList())
                {
                    if (user.Id == User.Claims.First().Value) continue;
                    var identityRoles = await _userManager.GetRolesAsync(user);
                    Users.Add(new UserRoleModel() { currentUser = user, userRoles = identityRoles.ToList() });
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(string searchReq,bool wantToClear, string interactBtn, string id)
        {
            if (interactBtn == null && !string.IsNullOrEmpty(searchReq))
            {
                SearchReq = searchReq;
                using (ApplicationContext db = new())
                {
                    var users = db.Users.Where(user =>user.UserName.Contains(searchReq) || user.Email.Contains(searchReq));
                    foreach (var user in users)
                    {
                        if (user.Id == User.Claims.First().Value) continue;
                        var identityRoles = await _userManager.GetRolesAsync(user);
                        Users.Add(new UserRoleModel() { currentUser = user, userRoles = identityRoles.ToList() });
                    }
                }
                return Page();
            }
            else
            {
                User user = await _userManager.FindByIdAsync(id);
                switch (interactBtn)
                {
                    case "delUser":
                        user.Dispose();
                        await _userManager.DeleteAsync(user);
                        break;
                    case "setAdmin":
                        await _userManager.AddToRoleAsync(user, "admin");
                        break;
                    case "remAdmin":
                        await _userManager.RemoveFromRoleAsync(user, "admin");
                        break;
                }
            }
            return RedirectToPage("/Creator/EditRoles");

        }
    }
}
