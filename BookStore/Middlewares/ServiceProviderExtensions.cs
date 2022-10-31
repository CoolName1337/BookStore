using BookStore.BAL.Interfaces;
using BookStore.BAL.Services;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Repositories;

namespace BookStore.Middlewares
{
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
    }
}
