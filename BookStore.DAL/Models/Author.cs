namespace BookStore.DAL.Models;

public class Author
{
    public int Id { get; set; }
    public string ProfilePicture { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<Book> Books { get; set; } = new();
    public List<AuthorReview> Reviews { get; set; } = new();
}
