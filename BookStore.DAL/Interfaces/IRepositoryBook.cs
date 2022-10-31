using BookStore.DAL.Models;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryBook
{
    Task<int> Create(Book book);

    void Delete(Book book);

    Task Update(Book book);
    IEnumerable<Book> GetBooks();
    IEnumerable<Book> GetBooks(Func<Book, bool> predicate);
    Book? this[int Id] { get; }
}
