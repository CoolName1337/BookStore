using BookStore.DAL.Models;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryReview
{
    Task Create(Review review);
    Task Delete(Review review);

}
