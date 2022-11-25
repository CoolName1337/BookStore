using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BAL.Services;

public class ServiceAuthor : IServiceAuthor
{
    private readonly IRepositoryAuthor _repositoryAuthor;
    public DbSet<Author> Authors { get => _repositoryAuthor.Authors; }

    public ServiceAuthor(IRepositoryAuthor repositoryAuthor)
    {
        _repositoryAuthor = repositoryAuthor;
    }

    public async Task Remove(Author author)
    {
        if (author != null)
        {
            FileService.Delete(author.ProfilePicture);
            await _repositoryAuthor.Delete(author);
        }
    }

    public async Task Add(Author author, IFormFile authorPic)
    {
        if (authorPic == null)
        {
            throw new Exception("Не удалось сохранить изображение");
        }
        if (!author.Name.Any() || !author.Description.Any())
        {
            throw new Exception("Имя или описание не указано");
        }
        author.ProfilePicture = await FileService.SaveAuthorPic(authorPic, author.Name);
        await _repositoryAuthor.Create(author);
    }

    public Author GetAuthor(int id)
    {
        return _repositoryAuthor.GetAuthor(id);
    }

}
