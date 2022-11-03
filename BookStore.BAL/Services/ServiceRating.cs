using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;

namespace BookStore.BAL.Services;

public class ServiceRating : IServiceRating
{
    private readonly IRepositoryRating _repositoryRating;

    public ServiceRating(IRepositoryRating repositoryRating)
    {
        _repositoryRating = repositoryRating;
    }

    public async Task TryRateBook(User user, Book book, int stars)
    {
        int _r = GetUserRating(user, book);
        if (_r > 0)
        {
            await UnrateBook(user, book);
            if (_r == stars) return;
        }
        await RateBook(user, book, stars);
    }

    public async Task RateBook(User user, Book book, int stars)
    {
        Rating rating = new Rating() { BookId = book.Id, UserId = user.Id, Stars = stars };
        await _repositoryRating.Crete(rating);
        book.Ratings.Add(rating);
    }
    public async Task UnrateBook(User user, Book book)
    {
        Rating rating = book.Ratings.Find(rating => rating.UserId == user.Id);
        await _repositoryRating.Delete(rating);
        book.Ratings.Remove(rating);
    }

    public double GetRating(Book book)
    {
        if (GetRatingCount(book) == 0) return 0;
        return book.Ratings.Average(rating => rating.Stars);
    }

    public int GetRatingCount(Book book)
    {
        return book.Ratings.Count;
    }

    public int GetUserRating(User user, Book book)
    {
        return book?.Ratings.FirstOrDefault(rating => rating.UserId == user.Id)?.Stars ?? 0;
    }
}
