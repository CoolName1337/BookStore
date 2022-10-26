using Microsoft.AspNetCore.Identity;
using BookStore.DAL.Models;

namespace BookStore.DAL.Repositories;

public class RepositoryUser
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public RepositoryUser(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInResult> Login(string username, string password, bool remember)
    {
        return await _signInManager.PasswordSignInAsync(username, password, remember, false);
    }
    public async void Logout()
    {
        await _signInManager.SignOutAsync();
    }
    public async Task<IdentityResult> Create(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result;
    }

    public async void Delete(User user)
    {
        await _userManager.DeleteAsync(user);
    }

    public async void Update(User user)
    {
        await _userManager.UpdateAsync(user);
    }

    public User this[string Id]
    {
        get => _userManager.FindByIdAsync(Id).Result;
    }
}
