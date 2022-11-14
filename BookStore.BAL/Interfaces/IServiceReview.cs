using BookStore.DAL.Models;

namespace BookStore.BAL.Interfaces;

public interface IServiceReview
{
    Task AddReview(Book book, User user, string reviewText);
    Task DeleteReview(Review review);
}
