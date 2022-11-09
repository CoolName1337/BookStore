namespace BookStore.DAL.Models;

public class Genre
{
    public int Id { get; set; }
    public string Value { get; set; } = "";
    public List<Book> Books { get; set; } = new();
}
