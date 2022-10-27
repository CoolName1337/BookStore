using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DAL.Models;

public class User : IdentityUser
{
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public string _favoriteBooks { get; set; } = "";
    public string _ratingBooks { get; set; } = "";
    public List<Book> AvailableBooks { get; set; } = new();

    [NotMapped]
    public Dictionary<int, int> RatingBooks
    {
        set
        {
            _ratingBooks = "";
            foreach (int bookId in value.Keys)
            {
                _ratingBooks += $"{bookId}:{value[bookId]} ";
            } 
        }
        get
        {
            if (_ratingBooks.Length == 0) return new Dictionary<int, int>();

            var books_rating = _ratingBooks.Trim().Split(' ');
            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (string book_rating in books_rating)
            {
                result.Add(int.Parse(book_rating.Split(":")[0]), int.Parse(book_rating.Split(":")[1]));
            }
            return result;
        }
    }

    public void RateBook(Book book, int rating)
    {
        var RatingDict = RatingBooks;
        if (RatingDict.Keys.Contains(book.Id))
        {
            book.DeleteRating(RatingDict[book.Id]);
            if (RatingDict[book.Id] == rating)
            {
                RatingDict.Remove(book.Id);
            }
            else
            {
                book.AddRating(rating);
                RatingDict[book.Id] = rating;
            }
        }
        else
        {
            book.AddRating(rating);
            RatingDict[book.Id] = rating;
        }
        RatingBooks = RatingDict;
    }
    
    public int GetRatingForBook(Book book)
    {
        if (RatingBooks.TryGetValue(book.Id, out int rating))
        {
            return rating;
        }
        return 0;
    }
}
