using bapi.Dtos;
using Core.Entities;

namespace bapi.Mappers
{
    public static class BookMapper
    {
        public static BookDto BookToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description
            };
        }
        public static Book DtoToBook(BookDto book)
        {
            return new Book
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description
            };
        }
    }
}
