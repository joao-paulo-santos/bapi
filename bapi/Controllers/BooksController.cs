using bapi.Dtos;
using bapi.Mappers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bapi.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [AllowAnonymous]
        [Route("getBookById")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BookDto>>> GetBookById(int id)
        {
            Book? book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound("Book not found.");
            return Ok(BookMapper.BookToDto(book));
        }

        [AllowAnonymous]
        [Route("getBookDetailsById")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Book>>> GetBookDetailsById(int id)
        {
            Book? book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound("Book not found.");
            return Ok(book);

        }
        [Authorize(Roles = "Manager, Admin")]
        [Route("Add")]
        [HttpPost]
        public async Task<ActionResult<BookDto>> AddBook(string name, string description)
        {
            Book? book = new Book
            {
                Name = name,
                Description = description,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            book = await _bookService.AddBookAsync(book);
            if (book == null) return Problem("Could not add the new book");

            return BookMapper.BookToDto(book);
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("DeleteBook")]
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBook([FromBody] int id)
        {
            Book? book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound("Book not found");


            bool result = await _bookService.DeleteBookAsync(book);
            if (!result) return Problem("Internal Error, Could not delete the book");

            return Ok(true);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getBooksPaged")]
        public async Task<ActionResult<IReadOnlyList<BookDto>>> GetBooksPaged(int pageIndex = 0, int pageSize = 10)
        {
            var list = await _bookService.GetPagedListOfBooksAsync(pageIndex, pageSize);
            List<BookDto> books = list.Select(x => BookMapper.BookToDto(x)).ToList();
            return Ok(books);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getBooksByNamePaged")]
        public async Task<ActionResult<IReadOnlyList<BookDto>>> GetBooksByNamePaged(string name, int pageIndex = 0, int pageSize = 10)
        {
            var list = await _bookService.QueryPagedBooksByNameAsync(name, pageIndex, pageSize);
            List<BookDto> books = list.Select(x => BookMapper.BookToDto(x)).ToList();
            return Ok(books);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getBooksByDescriptionPaged")]
        public async Task<ActionResult<IReadOnlyList<BookDto>>> GetBooksByDescriptionPaged(string description, int pageIndex = 0, int pageSize = 10)
        {
            var list = await _bookService.QueryPagedBooksByDescriptionAsync(description, pageIndex, pageSize);
            List<BookDto> books = list.Select(x => BookMapper.BookToDto(x)).ToList();
            return Ok(books);
        }

        [Authorize(Roles = "Manager, Admin")]
        [Route("UpdateBook")]
        [HttpPatch]
        public async Task<ActionResult<BookDto>> UpdateBook([FromBody] BookDto updatedBook)
        {
            Book? book = await _bookService.GetBookByIdAsync(updatedBook.Id);
            if (book == null) return NotFound("Book not found");

            book = await _bookService.UpdateBookAsync(BookMapper.DtoToBook(updatedBook));
            if (book == null) return Problem("Internal Error, Could not update the book");

            return BookMapper.BookToDto(book);
        }
    }
}
