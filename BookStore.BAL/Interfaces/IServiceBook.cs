using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BAL.Interfaces;

public interface IServiceBook
{
    DbSet<Book> Books { get; }
    Task<ActionResult<Book>> Verificate(Book book, IFormFile bookFile, IFormFile imageFile);
    Task<ActionResult<Book>> Add(Book book, IFormFile bookFile, IFormFile imageFile);
    Task<ActionResult<Book>> Edit(int id, Book book, IFormFile bookFile, IFormFile imageFile);
    string GetCorrectPath(Book book);
    void Delete(Book book);
    void AddGenres(Book book, IEnumerable<Genre> genres);
    void RemoveGenres(Book book, IEnumerable<Genre> genres);
    Task Update(Book book);
    Book GetBook(int id);
}
