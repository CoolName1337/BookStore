using Microsoft.AspNetCore.Identity;
using BookStore.DAL.Models;
using Microsoft.EntityFrameworkCore;
using BookStore.DAL.Interfaces;

namespace BookStore.DAL.Repositories;

public class RepositoryUser : IRepositoryUser
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public RepositoryUser(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public async Task AddRole(User user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }
    public IList<string> GetRoles(User user)
    {
        return _userManager.GetRolesAsync(user).Result;
    }
    public async Task RemoveRole(User user, string role)
    {
        await _userManager.RemoveFromRoleAsync(user, role);
    }

    public async Task<SignInResult> Login(string username, string password, bool remember)
    {
        return await _signInManager.PasswordSignInAsync(username, password, remember, false);
    }
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
    public async Task<IdentityResult> Create(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result;
    }

    public async Task Delete(User user)
    {
        await _userManager.DeleteAsync(user);
    }

    public async Task Update(User user)
    {
        await _userManager.UpdateAsync(user);
    }

    public IEnumerable<User> GetUsers()
    {
        return _userManager.Users
            .Include(user => user.AvailableBooks)
            .Include(user => user.Ratings)
            .Include(user => user.Favorites)
            .Include(book => book.Reviews)
                .ThenInclude(review=>review.Rates)
            .ToList();
    }
    public IEnumerable<User> GetUsers(Func<User,bool> predicate)
    {
        return _userManager.Users
            .Include(user => user.AvailableBooks)
            .Include(user => user.Ratings)
            .Include(user => user.Favorites)
            .Include(book => book.Reviews)
                .ThenInclude(review => review.Rates)
            .Where(predicate);
    }
    public User this[string Id]
    {
        get => _userManager.Users
            .Include(user=>user.AvailableBooks)
            .Include(user=>user.Ratings)
            .Include(user => user.Favorites)
            .Include(book => book.Reviews)
                .ThenInclude(review => review.Rates)
            .First(user=>user.Id == Id);
    }
}
