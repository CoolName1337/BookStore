using BookStore.DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BookStore.BAL.Services;

public class ServiceRole
{
    private RepositoryRole _repositoryRole;
    public ServiceRole(RoleManager<IdentityRole> roleManager)
    {
        _repositoryRole = new(roleManager);
    }

    public async Task Create(string roleName)
    {
        await _repositoryRole.Create(new IdentityRole(roleName));
    }
}
