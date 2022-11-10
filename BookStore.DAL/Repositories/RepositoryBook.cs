using BookStore.DAL.Context;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DAL.Repositories;

public class RepositoryBook : IRepositoryBook
{

    private readonly ApplicationContext _db = new();
    public async Task Create(Book book)
    {
        _db.Add(book);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(Book book)
    {
        _db.Remove(book);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Book book)
    {
        _db.Update(book);
        await _db.SaveChangesAsync();
    }
    public IEnumerable<Book> GetBooks()
    {
        return _db.Books
            .Include(book => book.Ratings)
            .Include(book => book.Favorites)
            .Include(book => book.Genres)
            .ToList();
    }
    public IEnumerable<Book> GetBooks(Func<Book, bool> predicate)
    {
        return _db.Books
            .Include(book => book.Ratings)
            .Include(book => book.Favorites)
            .Include(book => book.Genres)
            .Where(predicate);
    }
    public Book this[int Id]
    {
        get => _db.Books
            .Include(book => book.Ratings)
            .Include(book => book.Favorites)
            .Include(book => book.Genres)
            .FirstOrDefault(book => book.Id == Id);
    }
}
