using BookStore.BAL.Interfaces;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;

namespace BookStore.BAL.Services;

public class ServiceReview : IServiceReview
{
    private readonly IRepositoryReview _repositoryReview;
    public ServiceReview(IRepositoryReview repositoryReview)
    {
        _repositoryReview = repositoryReview;
    }

    public async Task AddReview(Book book, User user, string reviewText)
    {
        Review review = new() { BookId = book.Id, UserId = user.Id, Text = reviewText };
        await _repositoryReview.Create(review);
        book.Reviews.Add(review);
        user.Reviews.Add(review);
    }

    public async Task DeleteReview(Review review)
    {
        await _repositoryReview.Delete(review);
    }
    public async Task TryRateReview(User user, Review review, bool like)
    {
        var rate = review.Rates.Find(rate => rate.UserId == user.Id);
        if (rate != null)
        {
            await _repositoryReview.DeleteRate(rate);
            review.Rates.Remove(rate);
            if (like == rate.Like) return;
        }
        rate = new ReviewRate() { ReviewId = review.Id, UserId = user.Id, Like = like };
        await _repositoryReview.CreateRate(rate);
        review.Rates.Add(rate);
    }

}
