using Microsoft.AspNetCore.Identity;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryRole
{
    Task Create(IdentityRole role);
}
