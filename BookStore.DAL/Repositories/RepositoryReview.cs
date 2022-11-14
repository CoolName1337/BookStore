using BookStore.DAL.Context;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;

namespace BookStore.DAL.Repositories;

public class RepositoryReview : IRepositoryReview
{
    private readonly ApplicationContext _db = new();

    public async Task Create(Review review)
    {
        review.Created = DateTime.Now;
        _db.Add(review);
        await _db.SaveChangesAsync();
    }
    public async Task Delete(Review review)
    {
        _db.Remove(review);
        await _db.SaveChangesAsync();
    }
}
