using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryUser
{
    public IQueryable<User> Users { get; }
    Task AddRole(User user, string role);
    IList<string> GetRoles(User user);
    Task RemoveRole(User user, string role);
    Task<SignInResult> Login(string username, string password, bool remember);
    Task Logout();
    Task<IdentityResult> Create(User user, string password);
    Task Delete(User user);
    Task Update(User user);
}
