using Microsoft.AspNetCore.Identity;

namespace BookStore.DAL.Repositories;

public class RepositoryRole
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RepositoryRole(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task Create(IdentityRole role)
    {
        await _roleManager.CreateAsync(role);
    }

}
