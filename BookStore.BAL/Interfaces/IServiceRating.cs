using BookStore.DAL.Models;

namespace BookStore.BAL.Interfaces;

public interface IServiceRating
{
    Task TryRateBook(User user, Book book, int stars);
    Task RateBook(User user, Book book, int stars);
    Task UnrateBook(User user, Book book);
    double GetRating(Book book);
    int GetRatingCount(Book book);
    int GetUserRating(User user, Book book);
}
