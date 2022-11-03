using BookStore.DAL.Models;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryFavorite
{
    Task Create(Favorite favorite);
    Task Delete(Favorite favorite);
}
