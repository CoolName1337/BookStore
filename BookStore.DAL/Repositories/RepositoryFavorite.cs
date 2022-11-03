using BookStore.DAL.Context;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;

namespace BookStore.DAL.Repositories;

public class RepositoryFavorite : IRepositoryFavorite
{
    private readonly ApplicationContext _db = new();
    
    public async Task Create(Favorite favorite)
    {
        _db.Add(favorite);
        await _db.SaveChangesAsync();
    }
    public async Task Delete(Favorite favorite)
    {
        _db.Remove(favorite);
        await _db.SaveChangesAsync();
    }
}
