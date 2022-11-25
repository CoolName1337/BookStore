namespace BookStore.DAL.Models;

public class AuthorReview
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public int AuthorId { get; set; }
    public string Text { get; set; } = "";
    public DateTime Created { get; set; }
    public List<ReviewRate> Rates { get; set; } = new();
}
