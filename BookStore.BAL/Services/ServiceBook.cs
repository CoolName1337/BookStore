using BookStore.BAL.DTO;
using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BAL.Services;

public class ServiceBook : IServiceBook
{
    private IRepositoryBook _repositoryBook;

    public DbSet<Book> Books => _repositoryBook.Books;

    public ServiceBook(IRepositoryBook repositoryBook)
    {
        _repositoryBook = repositoryBook;
    }

    public async Task<Book> Add(BookDTO bookDTO)
    {
        var book = await bookDTO.ToBook();
        await _repositoryBook.Create(book);
        return book;
    }
    public async Task<Book> Edit(int bookId, BookDTO bookDTO)
    {
        var book = GetBook(bookId);
        await bookDTO.ToBook(book);
        await Update(book);
        return book;
    }

    public string GetCorrectPath(Book book)
    {
        if (book == null) return "";
        string file = book.SourceFile;
        return "files/" + System.Net.WebUtility.UrlEncode(file.Replace("/files/", "")).Replace("+", " ").Replace(" ", "%20");
    }

    public async Task AddAuthors(Book book, IEnumerable<Author> authors)
    {
        book.Authors.AddRange(authors);
        book.Authors = book.Authors.Distinct().ToList();
        await Update(book);
    }

    public async Task RemoveAuthors(Book book, IEnumerable<Author> authors)
    {
        book.Authors = book.Authors.Where(author => !authors.Contains(author)).ToList();
        await Update(book);
    }

    public async Task AddGenres(Book book, IEnumerable<Genre> genres)
    {
        book.Genres.AddRange(genres);
        book.Genres = book.Genres.Distinct().ToList();
        await Update(book);
    }
    public async Task RemoveGenres(Book book, IEnumerable<Genre> genres)
    {
        book.Genres = book.Genres.Where(genre => !genres.Contains(genre)).ToList();
        await Update(book);
    }
    public void Delete(Book book)
    {
        FileService.Delete(book.SourceFile);
        FileService.Delete(book.SourceImage);
        _repositoryBook.Delete(book);
    }

    public Task Update(Book book) => _repositoryBook.Update(book);
    public Book GetBook(int bookId) => _repositoryBook.GetBook(bookId);

}
