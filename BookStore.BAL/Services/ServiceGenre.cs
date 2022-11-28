using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;

namespace BookStore.BAL.Services;

public class ServiceGenre : IServiceGenre
{
    private readonly IRepositoryGenre _repositoryGenre;

	public ServiceGenre(IRepositoryGenre repositoryGenre)
	{
		_repositoryGenre = repositoryGenre;
	}
    public async Task<Genre> Create(string genreName)
    {
        if (GetByName(genreName) == null)
        {
            return await _repositoryGenre.Create(new Genre() { Value = genreName });
        }
        else
        {
            return null;
        }
    }
    public async Task Delete(string genreName)
    {
        await _repositoryGenre.Delete(_repositoryGenre.GetByName(genreName));
    }
    public List<Genre> GetAll()
    {
        return _repositoryGenre.GetAll();
    }
    public Genre GetByName(string name)
    {
        return _repositoryGenre.GetByName(name);
    }
    public Genre GetById(int id)
    {
        return _repositoryGenre.GetById(id);
    }
}
