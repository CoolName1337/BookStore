namespace BookStore.DAL.Models;

public class Favorite
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public int BookId { get; set; }
}
