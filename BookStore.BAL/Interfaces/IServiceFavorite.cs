using BookStore.DAL.Models;

namespace BookStore.BAL.Interfaces;

public interface IServiceFavorite
{
    Task<bool> TryLike(User user, Book book);
}
