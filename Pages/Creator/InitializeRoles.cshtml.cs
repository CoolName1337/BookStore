using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Xml.Linq;

namespace BookStore.Pages.Creator
{
    public class InitializeRolesModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public InitializeRolesModel(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task OnGet()
        {
            using(ApplicationContext db = new())
            {
                if (User.Identity.IsAuthenticated && db.Users.Count()==1)
                {
                    var adminResult = await _roleManager.CreateAsync(new IdentityRole("admin"));
                    var creatorResult = await _roleManager.CreateAsync(new IdentityRole("creator"));
                    await _userManager.AddToRolesAsync(await _userManager.FindByNameAsync(User.Identity.Name), new List<string>() { "creator" });
                }
                
            }
        }




    }
}
