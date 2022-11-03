using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.BAL.Services;

public class ServiceUser : IServiceUser
{
    public readonly IRepositoryUser _repository;

    public ServiceUser(IRepositoryUser repository)
    {
        _repository = repository;
    }
    
    
    public async Task TryBuyBook(User user, Book book)
    {
        book.Bought++;
        user.AvailableBooks.Add(book);
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

    public User GetUser(ClaimsPrincipal claimsPrincipal)
    {
        return this[claimsPrincipal.Claims.First().Value];
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

    public async Task DeleteUser(User user)
    {
        await _repository.Delete(user);
    }
    public async Task DeleteUser(string id) => await DeleteUser(_repository[id]);

    public async Task UpdateUser(User user) => await _repository.Update(user);
    public async Task UpdateUser(string id) => await UpdateUser(_repository[id]);

    public IEnumerable<User> GetUsers()
    {
        return _repository.GetUsers();
    }

    public IEnumerable<User> GetUsers(Func<User, bool> predicate)
    {
        return _repository.GetUsers(predicate);
    }
    public User this[string Id]
    {
        get => _repository[Id];
    }
}
