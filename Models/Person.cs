

namespace BookStore.Models;

public class Person
{
    List<Book> books = new();
    private decimal cash;
    public decimal Cash
    {
        get => cash;
        set => cash = value < 0 ? 0 : value;
    }
    public void Buy(Book book)
    {
        books.Add(book);
        cash -= book.Price;
    }
}

