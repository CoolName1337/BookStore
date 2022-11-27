using BookStore.DAL.Context;
using BookStore.DAL.Interfaces;
using BookStore.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DAL.Repositories
{
    public class RepositoryAuthor : IRepositoryAuthor
    {
        private readonly ApplicationContext _db = new();
        public DbSet<Author> Authors { get => _db.Authors; }
        public async Task Create(Author author)
        {
            _db.Add(author);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Author author)
        {
            _db.Remove(author);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Author author)
        {
            _db.Update(author);
            await _db.SaveChangesAsync();
        }

        public Author GetAuthor(int authorId)
        {
            return Authors
                .Include(author => author.Books)
                    .ThenInclude(book=>book.Ratings)
                .Include(author => author.Reviews)
                    .ThenInclude(review => review.Rates)
                .First(author => author.Id == authorId);

        }
    }
}
