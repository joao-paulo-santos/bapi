using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly PostgressDbContext _context;
        public BookRepository(PostgressDbContext context)
        {
            _context = context;
        }
        public async Task<Book?> AddBookAsync(Book newBook)
        {
            _context.Books.Add(newBook);
            int changes = await _context.SaveChangesAsync();
            return changes > 0 ? newBook : null;
        }

        public async Task<bool> DeleteBookAsync(Book book)
        {
            _context.Books.Remove(book);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Book?> GetBookByIdAsync(int Id)
        {
            Book? book = await _context.Books.FirstOrDefaultAsync(u => u.Id == Id);
            return book;
        }

        public async Task<IReadOnlyList<Book>> GetPagedListOfBooksAsync(int pageIndex, int pageSize)
        {
            return await _context.Books.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IReadOnlyList<Book>> QueryPagedBooksByDescriptionAsync(string decription, int pageIndex, int pageSize)
        {
            return await _context.Books.Where(x => x.Description.Contains(decription, StringComparison.CurrentCultureIgnoreCase))
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IReadOnlyList<Book>> QueryPagedBooksByNameAsync(string name, int pageIndex, int pageSize)
        {
            return await _context.Books.Where(x => x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Book?> UpdateBookAsync(Book newBook)
        {
            Book? book = await _context.Books.FirstOrDefaultAsync(u => u.Id == newBook.Id);
            if (book == null) return null;
            book.Name = newBook.Name;
            book.Description = newBook.Description;
            book.ModifiedDate = DateTime.UtcNow;
            int changes = await _context.SaveChangesAsync();
            return changes > 0 ? book : null;
        }
    }
}
