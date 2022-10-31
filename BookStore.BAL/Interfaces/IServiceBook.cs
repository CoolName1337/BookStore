using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace BookStore.BAL.Interfaces;

public interface IServiceBook
{
    Task<ActionResult<Book>> Verificate(string title, string writer, string descr, string price, IFormFile sourceFile, IFormFile sourceImage);
    Task<ActionResult<Book>> Add(string title, string writer, string descr, string price, IFormFile sourceFile, IFormFile sourceImage);
    Task<ActionResult<Book>> Edit(int id, string title, string writer, string descr, string price, IFormFile sourceFile, IFormFile sourceImage);
    string GetCorrectPath(Book book);
    void Delete(Book book);
    IEnumerable<Book> GetBooks();
    IEnumerable<Book> GetBooks(Func<Book, bool> predicate);
    Task Update(Book book);
    Book? this[int Id] { get; }
}
