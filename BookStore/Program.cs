using BookStore.DAL.Context;
using BookStore.DAL.Models;
using BookStore.Middlewares;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>().AddScoped<ApplicationContext>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

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

builder.Services.AddUserService();
builder.Services.AddBookService();
builder.Services.AddRoleService();
builder.Services.AddFavoriteService();
builder.Services.AddRatingService();
builder.Services.AddGenreService();
builder.Services.AddReviewService();
builder.Services.AddAuthorService();

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

app.UseMiddleware<CheckMiddleware>();

app.UseStaticFiles();

app.MapRazorPages();

app.Run();

