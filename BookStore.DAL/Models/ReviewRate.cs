namespace BookStore.DAL.Models;

public class ReviewRate
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public int ReviewId { get; set; }
    public bool Like { get; set; }

}
