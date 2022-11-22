using Microsoft.AspNetCore.Identity;

namespace BookStore.DAL.Models;

public class User : IdentityUser
{
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public List<Book> AvailableBooks { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
    public List<Favorite> Favorites { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
    public string ProfilePicture { get; set; } = "";
}
