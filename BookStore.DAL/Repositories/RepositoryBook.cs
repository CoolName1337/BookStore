using BookStore.DAL.Context;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;

namespace BookStore.DAL.Repositories;

public class RepositoryBook : IRepositoryBook
{
    
    private readonly ApplicationContext _db = new();
    public async Task<int> Create(Book book)
    {
        Book _book = _db.Add<Book>(book).Entity;
        return await _db.SaveChangesAsync();
    }

    public void Delete(Book book)
    {
        _db.Remove<Book>(book);
        _db.SaveChangesAsync();
    }

    public async Task Update(Book book)
    {
        _db.Update(book);
        await _db.SaveChangesAsync();
    }
    public IEnumerable<Book> GetBooks()
    {
        return _db.Books.ToList();
    }
    public IEnumerable<Book> GetBooks(Func<Book, bool> predicate)
    {
        return _db.Books.Where(predicate);
    }
    public Book? this[int Id]
    {
        get => _db.Find<Book>(Id);
    }
}
