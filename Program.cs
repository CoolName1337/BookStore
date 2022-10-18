using BookStore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

new ApplicationContext().Database.EnsureDeleted();
using (ApplicationContext db = new())
{

    User[] users = new[] { new User { Name = "Chmo", Balance = 228 }, new User { Name = "Kak", Balance = 14 }, new User { Name = "shk", Balance = 88 } };
    Book[] books = new[] { new Book { Title = "Pisya Popa 1", Price = 69 }, new Book { Title = "Pisya Popa 3", Price = 69 }, new Book { Title = "Pisya Popa 3", Price = 231 } };

    db.Users.AddRange(users);
    db.Books.AddRange(books);
    db.SaveChanges();
}

app.Run();

