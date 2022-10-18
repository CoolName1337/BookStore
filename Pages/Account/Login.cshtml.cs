using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public  Credential Credential { get; set; }
        public void OnGet()
        {

        }

        public void OnPost()
        {

        }
    }

    public class Credential
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
