using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>().AddScoped<ApplicationContext>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationContext>();

// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(ops =>
    {
        ops.Conventions.AuthorizeFolder("/Admin", "admin");
        ops.Conventions.AuthorizeFolder("/wwwroot/files", "admin");
    });
builder.Services.AddAuthorization(ops =>
{
    ops.AddPolicy("admin", policy => policy.RequireRole("admin", "creator"));
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.Context.Request.Path.StartsWithSegments("/files"))
        {
            ctx.Context.Response.Headers.Add("Cache-Control", "no-store");

            if (!ctx.Context.User.Identity.IsAuthenticated)
            {
                ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                ctx.Context.Response.ContentLength = 0;
                ctx.Context.Response.Body = Stream.Null;
            }
            else
            {
                using (ApplicationContext db = new())
                {
                    User user = db.Users.Include(user => user.AvailableBooks).First(user => user.Id == ctx.Context.User.Claims.First().Value);
                    string fileName = ctx.Context.Request.Path.Value;
                    bool res = user.AvailableBooks.Any(book => book.SourceFile == fileName);
                    if (!res)
                    {
                        ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        ctx.Context.Response.ContentLength = 0;
                        ctx.Context.Response.Body = Stream.Null;
                    }
                }
            }
        }
    }
});



app.MapRazorPages();



app.Run();

