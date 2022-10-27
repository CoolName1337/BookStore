using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace BookStore.Pages.Creator;

public class UserRoleModel
{
    public User currentUser { get; set; }
    public List<string> userRoles { get; set; }

}

[Authorize(Roles = "creator")]
public class EditRolesModel : PageModel
{
    private readonly ServiceUser _serviceUser;
    public string SearchReq { get; set; } = "";
    public List<UserRoleModel> Users { get; set; } = new();

    public EditRolesModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _serviceUser = new(userManager, signInManager);
    }
    public IActionResult OnGet(string searchReq)
    {
        SearchReq = searchReq;
        foreach (User user in _serviceUser.GetUsers())
        {
            if (user == _serviceUser.GetUser(User)) continue;
            Users.Add(new UserRoleModel() { currentUser = user, userRoles = _serviceUser.GetRoles(user).ToList() });
        }
        return Page();
    }

    public async Task<IActionResult> OnPost(string searchReq, string interactBtn, string id)
    {
        if (interactBtn == null && !string.IsNullOrEmpty(searchReq))
        {
            SearchReq = searchReq;
            foreach (User user in _serviceUser.GetUsers(user => user.UserName.Contains(searchReq) || user.Email.Contains(searchReq)))
            {
                if (user == _serviceUser.GetUser(User)) continue;
                Users.Add(new UserRoleModel() { currentUser = user, userRoles = _serviceUser.GetRoles(user).ToList() });
            }
            return Page();
        }
        else
        {
            User user = _serviceUser[id];
            switch (interactBtn)
            {
                case "delUser":
                    var ratingBooks = user.RatingBooks;
                    ServiceBook _serviceBook = new ServiceBook();
                    foreach (int bookId in ratingBooks.Keys)
                    {
                        Book book = _serviceBook[bookId];
                        if (book == null) continue;
                        book.DeleteRating(ratingBooks[bookId]);
                        await _serviceBook.Update(book);
                    }
                    await _serviceUser.DeleteUser(user);
                    break;
                case "setAdmin":
                    await _serviceUser.AddRole(user, "admin");
                    break;
                case "remAdmin":
                    await _serviceUser.RemoveRole(user, "admin");
                    break;
            }
        }
        return RedirectToPage("/Creator/EditRoles");

    }
}
