using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Pages;

public class AuthorPageModel : PageModel
{
    public IServiceUser _serviceUser { get; init; }
    public IServiceBook _serviceBook { get; init; }
    public IServiceAuthor _serviceAuthor { get; init; }
    public IServiceRating _serviceRating { get; init; }
    public Author Author { get; set; }

    public AuthorPageModel(IServiceBook serviceBook, IServiceAuthor serviceAuthor, IServiceUser serviceUser, IServiceRating serviceRating)
    {
        _serviceUser = serviceUser;
        _serviceBook = serviceBook;
        _serviceAuthor = serviceAuthor;
        _serviceRating = serviceRating;
    }
    public void OnGet(int authorId)
    {
        Author = _serviceAuthor.GetAuthor(authorId);
    }

    public IActionResult OnGetBooks(int authorId)
    {
        Author = _serviceAuthor.GetAuthor(authorId);
        return new PartialViewResult()
        {
            ViewName = "_BooksView",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnGetDescription(int authorId)
    {
        Author = _serviceAuthor.GetAuthor(authorId);
        return new PartialViewResult()
        {
            ViewName = "_DescriptionView",
            ViewData = this.ViewData
        };
    }
    public IActionResult OnGetReviews(int authorId)
    {
        Author = _serviceAuthor.GetAuthor(authorId);
        return new PartialViewResult()
        {
            ViewName = "_ReviewsView",
            ViewData = this.ViewData
        };
    }
}
