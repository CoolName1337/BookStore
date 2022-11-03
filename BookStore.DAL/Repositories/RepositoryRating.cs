using BookStore.DAL.Context;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;

namespace BookStore.DAL.Repositories;

public class RepositoryRating : IRepositoryRating
{
    private readonly ApplicationContext _db = new();

    public async Task Crete(Rating rating)
    {
        _db.Add(rating);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(Rating rating)
    {
        _db.Remove(rating);
        await _db.SaveChangesAsync();
    }
}
