using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;

namespace BookStore.BAL.Services;

public class ServiceFavorite : IServiceFavorite
{
    private readonly IRepositoryFavorite _repositoryFavorite;

    public ServiceFavorite(IRepositoryFavorite repositoryFavorite)
    {
        _repositoryFavorite = repositoryFavorite;
    }
    public async Task TryLike(User user, Book book)
    {
        Favorite favorite = user.Favorites.FirstOrDefault(fav => fav.BookId == book.Id);
        if (favorite == null)
        {
            favorite = new Favorite() { BookId = book.Id, UserId = user.Id };
            await _repositoryFavorite.Create(favorite);
            user.Favorites.Add(favorite);
        }
        else
        {
            await _repositoryFavorite.Delete(favorite);
            user.Favorites.Remove(favorite);
        }
    }
}
