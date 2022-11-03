using BookStore.DAL.Models;

namespace BookStore.BAL.Interfaces;

public interface IServiceFavorite
{
    Task TryLike(User user, Book book);
}
