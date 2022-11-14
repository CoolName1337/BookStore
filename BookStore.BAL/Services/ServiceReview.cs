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
}
