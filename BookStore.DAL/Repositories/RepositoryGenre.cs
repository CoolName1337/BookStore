using BookStore.DAL.Context;
using BookStore.DAL.Models;

namespace BookStore.DAL.Repositories;

public class RepositoryGenre
{
    private readonly ApplicationContext _db = new();

    public async Task Create(Genre genre)
    {
        _db.Add(genre);
        await _db.SaveChangesAsync();
    }
    public async Task Delete(Genre genre)
    {
        _db.Remove(genre);
        await _db.SaveChangesAsync();
    }

    public List<Genre> GetAll()
    {
        return _db.Genres.ToList();
    }

    public Genre Get(string name)
    {
        return _db.Genres.FirstOrDefault(genre => genre.Value == name);
    }
}
