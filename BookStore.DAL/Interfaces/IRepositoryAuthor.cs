using BookStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryAuthor
{
    public DbSet<Author> Authors { get; }
    Task Create(Author author);
    Task Delete(Author author);
    Task Update(Author author);
    Author GetAuthor(int authorId);
}
