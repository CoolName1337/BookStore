using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.BAL.Interfaces;

public interface IServiceUser
{
    Task TryBuyBook(User user, Book book);
    Task AddRole(User user, string roleName);

    IList<string> GetRoles(User user);
    Task RemoveRole(User user, string roleName);
    User GetUser(ClaimsPrincipal claimsPrincipal);

    Task<SignInResult> Login(string username, string password, bool remember);
    Task Logout();
    Task<IdentityResult> Register(string username, string email, string passwordConfirm, string password);
    Task UpdateProfilePicture(User user, IFormFile img);
    Task DeleteUser(User user);
    Task DeleteUser(string id);
    Task UpdateUser(User user);
    Task UpdateUser(string id);

    IEnumerable<User> GetUsers();

    IEnumerable<User> GetUsers(Func<User, bool> predicate);
    User GetUserByName(string userName);
    User this[string Id] { get; }
}
