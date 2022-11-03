namespace BookStore.DAL.Models;

public class Rating
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public int BookId { get; set; }
    public int Stars { get; set; }
}
