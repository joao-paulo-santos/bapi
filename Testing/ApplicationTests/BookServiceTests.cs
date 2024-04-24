using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.ApplicationTests
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _bookService = new BookService(_mockUnitOfWork.Object);
        }

        #region Helpers
        private Book GetDefaultBook()
        {
            return new Book { Name = "testbook", Description = "testdec" };
        }
        #endregion

        #region Add Tests
        [Fact]
        public async Task AddBookAsync_ValidBook_AddsBookAndSavesChanges()
        {
            // Arrange
            var newBook = GetDefaultBook();
            _mockUnitOfWork.Setup(uow => uow.BookRepository.AddBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(newBook);

            // Act
            var addedBook = await _bookService.AddBookAsync(newBook);

            // Assert
            Assert.NotNull(addedBook);
            Assert.Equal(newBook.Name, addedBook.Name);
            Assert.Equal(newBook.Description, addedBook.Description);
        }

        #endregion

        #region Delete Tests
        [Fact]
        public async Task DeleteBookAsync_ValidBook_DeletesBookAndSavesChanges()
        {
            // Arrange
            var book = new Book { Name = "Test Book", Description = "A test description" };
            _mockUnitOfWork.Setup(uow => uow.BookRepository.DeleteBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(true);

            // Act
            var isDeleted = await _bookService.DeleteBookAsync(book);

            // Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteBookAsync_InvalidBook_DeletesBookAndSavesChanges()
        {
            // Arrange
            var book = new Book { Name = "Test Book", Description = "A test description" };
            _mockUnitOfWork.Setup(uow => uow.BookRepository.DeleteBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(false);

            // Act
            var isDeleted = await _bookService.DeleteBookAsync(book);

            // Assert
            Assert.False(isDeleted);
        }
        #endregion

        #region Get By Id Tests
        [Fact]
        public async Task GetBookByIdAsync_ExistingBook_ReturnsBook()
        {
            // Arrange
            var bookId = 1;
            var mockBook = new Book { Id = bookId, Name = "Test Book", Description = "A test description" };
            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetBookByIdAsync(bookId))
                .ReturnsAsync(mockBook);

            // Act
            var retrievedBook = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.NotNull(retrievedBook);
            Assert.Equal(bookId, retrievedBook.Id);
            Assert.Equal("Test Book", retrievedBook.Name);
            Assert.Equal("A test description", retrievedBook.Description);
        }

        [Fact]
        public async Task GetBookByIdAsync_NonexistentBook_ReturnsNull()
        {
            // Arrange
            var nonExistentId = 10; // Assuming this ID doesn't exist
            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetBookByIdAsync(nonExistentId))
                .ReturnsAsync((Book?)null);

            // Act
            var retrievedBook = await _bookService.GetBookByIdAsync(nonExistentId);

            // Assert
            Assert.Null(retrievedBook);
        }
        #endregion

        #region Get Paged Tests

        [Fact]
        public async Task GetPagedListOfBooksAsync_ValidInput_ReturnsPagedBooks()
        {
            // Arrange
            var pageIndex = 0;
            var pageSize = 10;
            var mockBooks = new List<Book>() { GetDefaultBook(), GetDefaultBook() };
            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetPagedListOfBooksAsync(pageIndex, pageSize))
                .ReturnsAsync(mockBooks);

            // Act
            var pagedBooks = await _bookService.GetPagedListOfBooksAsync(pageIndex, pageSize);

            // Assert
            Assert.NotNull(pagedBooks);
            Assert.Equal(2, pagedBooks.Count);
        }

        [Fact]
        public async Task GetPagedListOfBooksAsync_PageSizeGreaterThanMax_ReturnsMaxPageSize()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 55; // Greater than maxPageSize (50)
            var expectedSize = 50;
            var mockBooks = new List<Book>();
            for (int i = 0; i < expectedSize; i++)
            {
                mockBooks.Add(GetDefaultBook());
            }
            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetPagedListOfBooksAsync(pageIndex, expectedSize))
                .ReturnsAsync(mockBooks);
            // Act
            var pagedBooks = await _bookService.GetPagedListOfBooksAsync(pageIndex, pageSize);
         

            // Assert
            Assert.NotNull(pagedBooks);
            Assert.Equal(expectedSize, pagedBooks.Count);
        }

        [Fact]
        public async Task GetPagedListOfBooksAsync_ZeroPageSize_ReturnsDefaultPageSize()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 0; // lower than minimum
            var expectedSize = 5;
            var mockBooks = new List<Book>();
            for (int i = 0; i < expectedSize; i++)
            {
                mockBooks.Add(GetDefaultBook());
            }
            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetPagedListOfBooksAsync(pageIndex, expectedSize))
                .ReturnsAsync(mockBooks);
            // Act
            var pagedBooks = await _bookService.GetPagedListOfBooksAsync(pageIndex, pageSize);


            // Assert
            Assert.NotNull(pagedBooks);
            Assert.Equal(expectedSize, pagedBooks.Count);
        }
        #endregion
        #region Query by Description paged Tests
        [Fact]
        public async Task QueryPagedBooksByDescriptionAsync_ValidInput_ReturnsPagedBooks()
        {
            // Arrange
            var description = "test desc";
            var pageIndex = 0;
            var pageSize = 10;
            var mockBooks = new List<Book>() { GetDefaultBook(), GetDefaultBook() };
            _mockUnitOfWork.Setup(uow => uow.BookRepository.QueryPagedBooksByDescriptionAsync(description, pageIndex, pageSize))
                .ReturnsAsync(mockBooks);

            // Act
            var pagedBooks = await _bookService.QueryPagedBooksByDescriptionAsync(description, pageIndex, pageSize);

            // Assert
            Assert.NotNull(pagedBooks);
            Assert.Equal(2, pagedBooks.Count);
        }

        [Fact]
        public async Task QueryPagedBooksByDescriptionAsync_PageSizeGreaterThanMax_ReturnsMaxPageSize()
        {
            // Arrange
            var description = "test desc";
            var pageIndex = 1;
            var pageSize = 55; // Greater than maxPageSize (50)
            var expectedSize = 50;
            var mockBooks = new List<Book>();
            for (int i = 0; i < expectedSize; i++)
            {
                mockBooks.Add(GetDefaultBook());
            }
            _mockUnitOfWork.Setup(uow => uow.BookRepository.QueryPagedBooksByDescriptionAsync(description, pageIndex, expectedSize))
                .ReturnsAsync(mockBooks);
            // Act
            var pagedBooks = await _bookService.QueryPagedBooksByDescriptionAsync(description, pageIndex, pageSize);


            // Assert
            Assert.NotNull(pagedBooks);
            Assert.Equal(expectedSize, pagedBooks.Count);
        }

        [Fact]
        public async Task QueryPagedBooksByDescriptionAsync_ZeroPageSize_ReturnsDefaultPageSize()
        {
            // Arrange
            var description = "test desc";
            var pageIndex = 1;
            var pageSize = 0; // lower than minimum
            var expectedSize = 5;
            var mockBooks = new List<Book>();
            for (int i = 0; i < expectedSize; i++)
            {
                mockBooks.Add(GetDefaultBook());
            }
            _mockUnitOfWork.Setup(uow => uow.BookRepository.QueryPagedBooksByDescriptionAsync(description, pageIndex, expectedSize))
                .ReturnsAsync(mockBooks);
            // Act
            var pagedBooks = await _bookService.QueryPagedBooksByDescriptionAsync(description, pageIndex, pageSize);


            // Assert
            Assert.NotNull(pagedBooks);
            Assert.Equal(expectedSize, pagedBooks.Count);
        }

        [Fact]
        public async Task UpdateBookAsync_ValidBook_UpdatesBookAndSavesChanges()
        {
            // Arrange
            var existingBook = new Book { Id = 1, Name = "Old Name", Description = "Old Description" };
            var newBook = new Book { Id = existingBook.Id, Name = "New Name", Description = "New Description" };
            _mockUnitOfWork.Setup(uow => uow.BookRepository.UpdateBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(newBook);

            // Act
            var updatedBook = await _bookService.UpdateBookAsync(newBook);

            // Assert
            Assert.NotNull(updatedBook);
            Assert.Equal(newBook.Id, updatedBook.Id);
            Assert.Equal(newBook.Name, updatedBook.Name);
            Assert.Equal(newBook.Description, updatedBook.Description);
        }
        #endregion
    }
}
