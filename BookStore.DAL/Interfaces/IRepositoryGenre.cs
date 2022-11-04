using BookStore.DAL.Models;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryGenre
{
    Task Create(Genre genre);
    Task Delete(Genre genre);

    List<Genre> GetAll();

    Genre Get(string name);
}
