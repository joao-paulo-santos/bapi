using Core.Entities;

namespace Core.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(int Id);
        Task<IReadOnlyList<Book>> QueryPagedBooksByNameAsync(string name, int pageIndex, int pageSize);
        Task<IReadOnlyList<Book>> QueryPagedBooksByDescriptionAsync(string decription, int pageIndex, int pageSize);
        Task<IReadOnlyList<Book>> GetPagedListOfBooksAsync(int pageIndex, int pageSize);
        Task<Book?> AddBookAsync(Book newBook);
        Task<Book?> UpdateBookAsync(Book newBook);
        Task<bool> DeleteBookAsync(Book book);
    }
}
