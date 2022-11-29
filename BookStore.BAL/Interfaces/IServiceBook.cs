using BookStore.BAL.DTO;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BAL.Interfaces;

public interface IServiceBook
{
    DbSet<Book> Books { get; }
    Task<Book> Add(BookDTO bookDTO);
    Task<Book> Edit(int bookId, BookDTO bookDTO);
    string GetCorrectPath(Book book);
    void Delete(Book book);
    Task AddGenres(Book book, IEnumerable<Genre> genres);
    Task RemoveGenres(Book book, IEnumerable<Genre> genres);
    Task AddAuthors(Book book, IEnumerable<Author> authors);
    Task RemoveAuthors(Book book, IEnumerable<Author> authors);
    Task Update(Book book);
    Book GetBook(int id);
}
