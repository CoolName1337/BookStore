using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace BookStore.Models
{
    public class User : IdentityUser
    {
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public List<Book> AvailableBooks { get; set; } = new();
        public string FavoriteBooks { get; set; } = "";
        public string RaitingBooks { get; set; } = "";


        public void AddFavoriteBook(string bookId) => FavoriteBooks += $"{bookId} ";
        public void DeleteFavoriteBook(string bookId)
        {
            HashSet<string> favs = FavoriteBooks.Trim().Split(" ").ToHashSet();
            favs.Remove(bookId);
            FavoriteBooks = string.Join(' ', favs);
        }
        public string FindFav(string bookId)
        {
            HashSet<string> favs = FavoriteBooks.Trim().Split(" ").ToHashSet();
            return favs.FirstOrDefault(id => bookId == id);
        }

        public void BuyBook(Book book)
        {
            AvailableBooks.Add(book);
        }

        public void RateBook(Book book, int rating)
        {
            var ratingsArray = RaitingBooks.Trim().Split(" ");
            string book_rate = ratingsArray.FirstOrDefault(book_rate => book_rate.StartsWith($"{book.Id}:"));
            if (!string.IsNullOrEmpty(book_rate))
            {
                RaitingBooks = RaitingBooks.Replace(book_rate + " ", "");
                book.DeleteRating(book_rate.Split(":")[1]);
                if (rating.ToString() == book_rate.Split(":")[1]) return;
            }

            book.RaitingString += $"{rating} ";
            this.RaitingBooks += $"{book.Id}:{rating} ";
        }
        public int GetRaitingForBook(Book book)
        {
            if (!string.IsNullOrEmpty(RaitingBooks))
            {
                var ratingArray = RaitingBooks.Trim().Split(" ");
                var existRatedBook = ratingArray.Where(str => str.Split(":")[0] == book.Id.ToString()).FirstOrDefault();
                var rating = existRatedBook?.Split(":")[1];
                if (int.TryParse(rating, out int intRating)) return intRating;
            }
            return 0;
        }
    }
}
