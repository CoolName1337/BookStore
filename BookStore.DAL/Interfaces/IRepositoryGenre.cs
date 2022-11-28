using BookStore.DAL.Models;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryGenre
{
    Task<Genre> Create(Genre genre);
    Task Delete(Genre genre);

    List<Genre> GetAll();

    Genre GetByName(string name);
    Genre GetById(int id);
}
