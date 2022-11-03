using BookStore.DAL.Models;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryRating
{
    Task Crete(Rating rating);
    Task Delete(Rating rating);
}
