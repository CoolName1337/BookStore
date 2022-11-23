using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.BAL.Services;

public class ServiceUser : IServiceUser
{
    public readonly IRepositoryUser _repository;
    public IQueryable<User> Users { get => _repository.Users; }

    public ServiceUser(IRepositoryUser repository)
    {
        _repository = repository;
    }
    public async Task TryBuyBook(User user, Book book)
    {
        book.Bought++;
        user.AvailableBooks.Add(book);
        await Update(user);
    }
    public async Task AddRole(User user, string roleName)
    {
        await _repository.AddRole(user, roleName);
    }

    public IList<string> GetRoles(User user)
    {
        return _repository.GetRoles(user);
    }
    public async Task RemoveRole(User user, string roleName)
    {
        await _repository.RemoveRole(user, roleName);
    }

    public async Task<SignInResult> Login(string username, string password, bool remember)
    {
        return await _repository.Login(username, password, remember);
    }
    public async Task Logout()
    {
        await _repository.Logout();
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

    public async Task UpdateProfilePicture(User user, IFormFile img)
    {
        FileService.Delete(user.ProfilePicture);
        user.ProfilePicture = await FileService.SaveProfilePic(img, user.UserName);
        await Update(user);
    }

    public async Task Delete(User user)
    {
        FileService.Delete(user.ProfilePicture);
        await _repository.Delete(user);
    }
    public async Task Delete(string id) => await Delete(Users.First(user=>user.Id == id));

    public async Task Update(User user) => await _repository.Update(user);
    public async Task Update(string id) => await Update(Users.First(user => user.Id == id));

    public User GetUser(ClaimsPrincipal claimsPrincipal)
    {
        return GetUser(claimsPrincipal.Claims.First().Value);
    }
    public User GetUser(string id)
    {
        return Users
            .Include(user => user.AvailableBooks)
            .Include(user => user.Favorites)
            .Include(user => user.Ratings)
            .Include(user => user.Reviews)
                .ThenInclude(review=>review.Rates)
            .FirstOrDefault(user => user.Id == id);
    }
}
