using BookStore.DAL.Context;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DAL.Repositories;

public class RepositoryBook : IRepositoryBook
{
    private readonly ApplicationContext _db = new();
    public DbSet<Book> Books { get => _db.Books; }
    public async Task Create(Book book)
    {
        book.AddingDate = DateTime.Now;
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

    public Book GetBook(int bookId)
    {
        return Books
            .Include(book => book.Authors)
            .Include(book => book.Users)
            .Include(book => book.Ratings)
            .Include(book => book.Genres)
            .Include(book => book.Reviews)
                .ThenInclude(review=>review.Rates)
            .First(book => book.Id == bookId);
            
    }
}
