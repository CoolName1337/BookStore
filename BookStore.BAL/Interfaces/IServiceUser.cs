using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.BAL.Interfaces;

public interface IServiceUser
{
    public IQueryable<User> Users { get; }
    Task TryBuyBook(User user, Book book);
    Task AddRole(User user, string roleName);
    IList<string> GetRoles(User user);
    Task RemoveRole(User user, string roleName);
    Task<SignInResult> Login(string username, string password, bool remember);
    Task Logout();
    Task<IdentityResult> Register(string username, string email, string passwordConfirm, string password);
    Task UpdateProfilePicture(User user, IFormFile img);
    Task Delete(User user);
    Task Delete(string id);
    Task Update(User user);
    Task Update(string id);
    User GetUser(string id);
    User GetUser(ClaimsPrincipal claimsPrincipal);
}
