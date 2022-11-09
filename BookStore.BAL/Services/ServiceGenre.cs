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
        return await _repositoryGenre.Create(new Genre() { Value = genreName });
    }
    public async Task Delete(string genreName)
    {
        await _repositoryGenre.Delete(_repositoryGenre.Get(genreName));
    }
    public List<Genre> GetAll()
    {
        return _repositoryGenre.GetAll();
    }
    public Genre Get(string name)
    {
        return _repositoryGenre.Get(name);
    }
}
