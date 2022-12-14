using BookStore.DAL.Models;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryReview
{
    Task Create(Review review);
    Task Delete(Review review);

    Task CreateRate(ReviewRate reviewRate);
    Task DeleteRate(ReviewRate reviewRate);
}
