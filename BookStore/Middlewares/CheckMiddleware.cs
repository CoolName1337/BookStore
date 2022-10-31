using BookStore.BAL.Interfaces;
using BookStore.DAL.Models;
using System.Net;

namespace BookStore.Middlewares
{
    public class CheckMiddleware
    {
        private RequestDelegate _next;
        public CheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IServiceUser serviceUser)
        {
            string filePath = context.Request.Path.Value;
            if (filePath.StartsWith("/files"))
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    User user = serviceUser.GetUser(context.User);
                    bool res = user.AvailableBooks.Any(book => book.SourceFile == filePath);
                    if (!res)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Locked;
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }
}
