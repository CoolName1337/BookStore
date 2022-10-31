using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BookStore.BAL.Services;

public class ServiceRole : IServiceRole
{
    private IRepositoryRole _repositoryRole;
    public ServiceRole(IRepositoryRole repositoryRole)
    {
        _repositoryRole = repositoryRole;
    }

    public async Task Create(string roleName)
    {
        await _repositoryRole.Create(new IdentityRole(roleName));
    }
}
