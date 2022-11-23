using BookStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DAL.Interfaces;

public interface IRepositoryBook
{
    DbSet<Book> Books { get; }
    Task Create(Book book);
    Task Delete(Book book);
    Task Update(Book book); 
    Book GetBook(int bookId);
}
