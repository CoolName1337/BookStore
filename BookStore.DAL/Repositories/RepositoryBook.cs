using BookStore.DAL.Models;

namespace BookStore.DAL.Repositories;

public class RepositoryBook
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

    public void Update(Book book)
    {
        _db.Update(book);
        _db.SaveChangesAsync();
    }
    public Book? this[int Id]
    {
        get => _db.Find<Book>(Id);
    }
}
