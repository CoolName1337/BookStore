using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class User : IdentityUser
    {
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public decimal Balance { get; set; }
        public List<Book> AvailableBooks { get; set; } = new();

    }
}
