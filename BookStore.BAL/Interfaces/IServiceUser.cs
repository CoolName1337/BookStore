using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.BAL.Interfaces;

public interface IServiceUser
{
    public Task TryBuyBook(User user, Book book);
    public Task AddRole(User user, string roleName);

    public IList<string> GetRoles(User user);
    public Task RemoveRole(User user, string roleName);
    public User GetUser(ClaimsPrincipal claimsPrincipal);

    public Task<SignInResult> Login(string username, string password, bool remember);
    public Task Logout();
    public Task<IdentityResult> Register(string username, string email, string passwordConfirm, string password);
    public Task DeleteUser(User user);
    public Task DeleteUser(string id);
    public Task UpdateUser(User user);
    public Task UpdateUser(string id);

    public IEnumerable<User> GetUsers();

    public IEnumerable<User> GetUsers(Func<User, bool> predicate);
    public User this[string Id] { get; }
}
