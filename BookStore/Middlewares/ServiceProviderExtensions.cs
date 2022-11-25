using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Repositories;

namespace BookStore.Middlewares;

public static class ServiceProviderExtensions
{
    public static void AddUserService(this IServiceCollection services)
    {
        services.AddTransient<IServiceUser, ServiceUser>();
        services.AddTransient<IRepositoryUser, RepositoryUser>();
    }
    public static void AddBookService(this IServiceCollection services)
    {
        services.AddTransient<IServiceBook, ServiceBook>();
        services.AddTransient<IRepositoryBook, RepositoryBook>();
    }
    public static void AddRoleService(this IServiceCollection services)
    {
        services.AddTransient<IServiceRole, ServiceRole>();
        services.AddTransient<IRepositoryRole, RepositoryRole>();
    }
    public static void AddRatingService(this IServiceCollection services)
    {
        services.AddTransient<IServiceRating, ServiceRating>();
        services.AddTransient<IRepositoryRating, RepositoryRating>();
    }
    public static void AddFavoriteService(this IServiceCollection services)
    {
        services.AddTransient<IServiceFavorite, ServiceFavorite>();
        services.AddTransient<IRepositoryFavorite, RepositoryFavorite>();
    }
    public static void AddGenreService(this IServiceCollection services)
    {
        services.AddTransient<IServiceGenre, ServiceGenre>();
        services.AddTransient<IRepositoryGenre, RepositoryGenre>();
    }
    public static void AddReviewService(this IServiceCollection services)
    {
        services.AddTransient<IServiceReview, ServiceReview>();
        services.AddTransient<IRepositoryReview, RepositoryReview>();
    }
    public static void AddAuthorService(this IServiceCollection services)
    {
        services.AddTransient<IServiceAuthor, ServiceAuthor>();
        services.AddTransient<IRepositoryAuthor, RepositoryAuthor>();
    }
}
