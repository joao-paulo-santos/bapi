using System.ComponentModel.DataAnnotations;

namespace bapi.Dtos
{
    public class BookDto
    {
        public required int Id { get; set; }

        [StringLength(255, ErrorMessage = "Must be between 1 and 255 characters", MinimumLength = 1)]
        public required string Name { get; set; }
        [StringLength(255, ErrorMessage = "Must be between 1 and 255 characters", MinimumLength = 1)]
        public required string Description { get; set; }
    }
}
