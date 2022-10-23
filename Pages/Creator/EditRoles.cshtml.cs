using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BookStore.Pages.Creator
{
    [Authorize(Roles = "creator")]
    public class EditRolesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
