using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DAL.Models;
using BookStore.DAL.Repositories;

namespace BookStore.BAL.Services;

public class ServiceBook
{
    private RepositoryBook _repositoryBook = new();

    public async Task<ActionResult<Book>> Verificate(string title, string writer, string descr, string price, IFormFile sourceFile, IFormFile sourceImage)
    {
        //sorry...

        ActionResult<Book> actionResult = new(); 
        Book book = new Book()
        {
            Title = title,
            Description = descr,
            Writer = writer,
            SourceFile = await FileService.Save(sourceFile),
            SourceImage = await FileService.Save(sourceImage)
        };
        if (string.IsNullOrWhiteSpace(book.Title)) actionResult.Errors.Add("Название не указано");
        if (string.IsNullOrWhiteSpace(book.Description)) actionResult.Errors.Add("Описание не указано");
        if (string.IsNullOrWhiteSpace(book.SourceImage)) actionResult.Errors.Add("Картинка не добавлена");
        if (string.IsNullOrWhiteSpace(book.SourceFile)) actionResult.Errors.Add("Файл книги не добавлен");
        if (string.IsNullOrWhiteSpace(book.Writer)) actionResult.Errors.Add("Писатель не указан"); 
        if (!decimal.TryParse(price, out decimal numPrice) || numPrice < 0)
        {
            actionResult.Errors.Add("Цена должна быть числом и неменьше нуля");
        }
        else
        {
            book.Price = numPrice;
        }
        actionResult.Value = book;
        return actionResult;
    }
    public async Task<ActionResult<Book>> Add(string title, string writer, string descr, string price, IFormFile sourceFile, IFormFile sourceImage)
    {
        var res = await Verificate(title, writer, descr, price, sourceFile, sourceImage);
        
        if (res.Succeed)
        {
            await _repositoryBook.Create(res.Value);
        }
        else
        {
            FileService.Delete(res.Value.SourceFile);
            FileService.Delete(res.Value.SourceImage);
        }
        return res;
    }
    public async Task<ActionResult<Book>> Edit(int id, string title, string writer, string descr, string price, IFormFile sourceFile, IFormFile sourceImage)
    {
        var res = await Verificate(title, writer, descr, price, sourceFile, sourceImage);
        
        if (res.Succeed)
        {
            var book = _repositoryBook[id];

            book.Title = res.Value.Title;
            book.Writer = res.Value.Writer;
            book.Description = res.Value.Description;
            book.Price = res.Value.Price;
            book.SourceImage = res.Value.SourceImage;
            book.SourceFile = res.Value.SourceFile;

            _repositoryBook.Update(book);
        }
        else
        {
            FileService.Delete(res.Value.SourceFile);
            FileService.Delete(res.Value.SourceImage);
        }
        return res;
    }

    public string GetCorrectPath(Book book)
    {
        string file = book.SourceFile;
        return "files/" + System.Net.WebUtility.UrlEncode(file.Replace("/files/", "")).Replace("+", " ").Replace(" ", "%20");
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

    public Book? this[int Id]
    {
        get => _repositoryBook[Id];
    }
}
