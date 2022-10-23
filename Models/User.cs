using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class User : IdentityUser
    {
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public List<Book> AvailableBooks { get; set; } = new();
        public string FavoriteBooks { get; set; } = "";
        public void AddFavoriteBook(string bookId) => FavoriteBooks += $"{bookId} ";
        public void DeleteFavoriteBook(string bookId)
        {
            HashSet<string> favs = FavoriteBooks.Split(" ").ToHashSet();
            favs.Remove(bookId);
            FavoriteBooks = string.Join(' ', favs);
        }
        public string FindFav(string bookId)
        {
            HashSet<string> favs = FavoriteBooks.Split(" ").ToHashSet();
            return favs.FirstOrDefault(id => bookId == id);
        }

        public void BuyBook(Book book)
        {
            AvailableBooks.Add(book);
        }
    }
}
