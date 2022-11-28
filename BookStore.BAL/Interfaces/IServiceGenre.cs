using BookStore.DAL.Models;

namespace BookStore.BAL.Interfaces;

public interface IServiceGenre
{
    Task<Genre> Create(string genreName);
    Task Delete(string genreName);
    List<Genre> GetAll();
    Genre GetByName(string name);
    Genre GetById(int id);
}
