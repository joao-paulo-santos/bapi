using Core.Entities;
using Core.Interfaces;

namespace Application.Services
{
    public class BookService : IBookService
    {
        const int maxPageSize = 50;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Book?> AddBookAsync(Book newBook)
        {
            return await _unitOfWork.BookRepository.AddBookAsync(newBook);
        }

        public async Task<bool> DeleteBookAsync(Book book)
        {
            return await _unitOfWork.BookRepository.DeleteBookAsync(book);
        }

        public async Task<Book?> GetBookByIdAsync(int Id)
        {
            return await _unitOfWork.BookRepository.GetBookByIdAsync(Id);
        }

        public async Task<IReadOnlyList<Book>> GetPagedListOfBooksAsync(int pageIndex, int pageSize)
        {
            int size = pageSize > maxPageSize ? maxPageSize : pageSize;
            size = size > 0 ? size : 5;
            return await _unitOfWork.BookRepository.GetPagedListOfBooksAsync(pageIndex, size);
        }

        public async Task<IReadOnlyList<Book>> QueryPagedBooksByDescriptionAsync(string decription, int pageIndex, int pageSize)
        {
            int size = pageSize > maxPageSize ? maxPageSize : pageSize;
            size = size > 0 ? size : 5;
            return await _unitOfWork.BookRepository.QueryPagedBooksByDescriptionAsync(decription, pageIndex, size);
        }

        public async Task<IReadOnlyList<Book>> QueryPagedBooksByNameAsync(string name, int pageIndex, int pageSize)
        {
            int size = pageSize > maxPageSize ? maxPageSize : pageSize;
            size = size > 0 ? size : 5;
            return await _unitOfWork.BookRepository.QueryPagedBooksByNameAsync(name, pageIndex, size);
        }

        public async Task<Book?> UpdateBookAsync(Book newBook)
        {
            return await _unitOfWork.BookRepository.UpdateBookAsync(newBook);
        }
    }
}
