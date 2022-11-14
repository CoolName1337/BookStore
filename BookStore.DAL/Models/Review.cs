namespace BookStore.DAL.Models;

public class Review
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public int BookId { get; set; }
    public string Text { get; set; } = "";
    public DateTime Created { get; set; }
}
