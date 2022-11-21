using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace BookStore.BAL.Services;

public class ServiceBook : IServiceBook
{
    private IRepositoryBook _repositoryBook;

    public ServiceBook(IRepositoryBook repositoryBook)
    {
        _repositoryBook = repositoryBook;
    }

    public async Task<ActionResult<Book>> Verificate(Book book, IFormFile bookFile, IFormFile imageFile)
    {
        //sorry...

        ActionResult<Book> actionResult = new(); 

        if (string.IsNullOrWhiteSpace(book.Title)) actionResult.Errors.Add("Название не указано");
        if (string.IsNullOrWhiteSpace(book.Description)) actionResult.Errors.Add("Описание не указано");
        if (string.IsNullOrWhiteSpace(book.Writer)) actionResult.Errors.Add("Писатель не указан");
        if (book.Price < 0) actionResult.Errors.Add("Цена должна быть числом и неменьше нуля");
        if (book.AgeLimit < 0) actionResult.Errors.Add("Возрастно ограничение должно быть числом и неменьше нуля");
        if (book.Pages < 0) actionResult.Errors.Add("Количество страниц должно быть числом и неменьше нуля");
        if (bookFile == null) actionResult.Errors.Add("Файл книги не указан");
        else book.SourceFile = await FileService.Save(bookFile);
        if (imageFile == null) actionResult.Errors.Add("Картинка не указана");
        else book.SourceImage = await FileService.Save(imageFile);

        actionResult.Value = book;
        return actionResult;
    }
    public async Task<ActionResult<Book>> Add(Book book, IFormFile bookFile, IFormFile imageFile)
    {
        var res = await Verificate(book, bookFile, imageFile);
        
        if (res.Succeed)
        {
            await _repositoryBook.Create(book);
        }
        else
        {
            FileService.Delete(book.SourceFile);
            FileService.Delete(book.SourceImage);
        }
        return res;
    }
    public async Task<ActionResult<Book>> Edit(int id, Book book, IFormFile bookFile, IFormFile imageFile)
    {
        var res = await Verificate(book, bookFile, imageFile);
        if (res.Succeed)
        {
            Book currentBook = _repositoryBook[id];

            currentBook.Genres = book.Genres;
            currentBook.Title = book.Title;
            currentBook.Writer = book.Writer;
            currentBook.Price = book.Price;
            currentBook.SourceFile = book.SourceFile;
            currentBook.SourceImage = book.SourceImage;
            currentBook.Description = book.Description;

            await _repositoryBook.Update(book);
        }
        else
        {
            FileService.Delete(book.SourceFile);
            FileService.Delete(book.SourceImage);
        }
        return res;
    }

    public string GetCorrectPath(Book book)
    {
        string file = book.SourceFile;
        return "files/" + System.Net.WebUtility.UrlEncode(file.Replace("/files/", "")).Replace("+", " ").Replace(" ", "%20");
    }

    public void AddGenres(Book book, IEnumerable<Genre> genres)
    {
        book.Genres.AddRange(genres);
        book.Genres = book.Genres.Distinct().ToList();
    }
    public void RemoveGenres(Book book, IEnumerable<Genre> genres)
    {
        book.Genres = book.Genres.Where(genre => !genres.Contains(genre)).ToList();
    }


    public void Delete(Book book)
    {
        FileService.Delete(book.SourceFile);
        FileService.Delete(book.SourceImage);
        _repositoryBook.Delete(book);
    }
    public IEnumerable<Book> GetBooks()
    {
        return _repositoryBook.GetBooks();
    }
    public IEnumerable<Book> GetBooks(Func<Book, bool> predicate)
    {
        return _repositoryBook.GetBooks(predicate);
    }
    public async Task Update(Book book)
    {
        await _repositoryBook.Update(book);
    }
    public Book this[int Id]
    {
        get => _repositoryBook[Id];
    }
}
