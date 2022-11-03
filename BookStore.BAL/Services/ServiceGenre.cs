using BookStore.DAL.Models;
using BookStore.DAL.Repositories;

namespace BookStore.BAL.Services;

internal class ServiceGenre
{
    private readonly RepositoryGenre _repositoryGenre;

	public ServiceGenre(RepositoryGenre repositoryGenre)
	{
		_repositoryGenre = repositoryGenre;
	}
    public async Task Create(string genreName)
    {
        await _repositoryGenre.Create(new Genre() { Value = genreName });
    }
    public async Task Delete(string genreName)
    {
        await _repositoryGenre.Delete(_repositoryGenre.Get(genreName));
    }
    public List<Genre> GetAll()
    {
        return _repositoryGenre.GetAll();
    }
}
