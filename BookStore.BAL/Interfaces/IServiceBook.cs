using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace BookStore.BAL.Interfaces;

public interface IServiceBook
{
    Task<ActionResult<Book>> Verificate(Book book, IFormFile bookFile, IFormFile imageFile);
    Task<ActionResult<Book>> Add(Book book, IFormFile bookFile, IFormFile imageFile);
    Task<ActionResult<Book>> Edit(int id, Book book, IFormFile bookFile, IFormFile imageFile);
    string GetCorrectPath(Book book);
    void Delete(Book book);
    IEnumerable<Book> GetBooks();
    IEnumerable<Book> GetBooks(Func<Book, bool> predicate);
    Task Update(Book book);
    Book this[int Id] { get; }
}
