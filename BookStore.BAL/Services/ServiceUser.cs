using Microsoft.AspNetCore.Identity;
using BookStore.DAL.Models;
using BookStore.DAL.Repositories;
using System.Security.Claims;

namespace BookStore.BAL.Services;

public class ServiceUser
{
    public readonly RepositoryUser _repository;

    public ServiceUser(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _repository = new(userManager, signInManager);
    }

    public async Task<SignInResult> Login(string username, string password, bool remember)
    {
        return await _repository.Login(username, password, remember);
    }
    public void Logout()
    {
        _repository.Logout();
    }
    public async Task<IdentityResult> Register(string username, string email, string passwordConfirm, string password)
    {
        if (password != passwordConfirm) 
        {
            return IdentityResult.Failed(new IdentityError() { Description = "Пароли не совпадают" });
        }

        var result = await _repository.Create(new User() { UserName = username, Email = email}, password);

        if (result.Succeeded)
        {
            await Login(username, password, false);
        }
        return result;
    }

    public void DeleteUser(User user) => _repository.Delete(user);
    public void DeleteUser(string id) => DeleteUser(_repository[id]);

    public void UpdateUser(User user) => _repository.Update(user);
    public void UpdateUser(string id) => UpdateUser(_repository[id]);


    public void AddFavoriteBook(User user, string bookId)
    {
        user._favoriteBooks += $"{bookId} ";
        UpdateUser(user);
    }
    public void DeleteFavoriteBook(User user, string bookId)
    {
        HashSet<string> favs = user._favoriteBooks.Split(" ").ToHashSet();
        favs.Remove(bookId);
        user._favoriteBooks = string.Join(' ', favs);
        UpdateUser(user);
    }
    public HashSet<string> GetAllFavoriteBooks(User user)
    {
        return user._favoriteBooks.Split(" ").ToHashSet();
    }
    public string FindFav(User user, string bookId)
    {
        return GetAllFavoriteBooks(user).FirstOrDefault(id => bookId == id);
    }


    public User this[string Id]
    {
        get => _repository[Id];
    }

}
