using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BAL.Interfaces;

public interface IServiceAuthor
{
    DbSet<Author> Authors { get; }
    Author GetAuthor(int id);
    Task Add(Author author, IFormFile authorPic);
    Task Remove(Author author);
}
