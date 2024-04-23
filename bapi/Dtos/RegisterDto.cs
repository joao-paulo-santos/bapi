using System.ComponentModel.DataAnnotations;

namespace bapi.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Must be between 3 and 20 characters", MinimumLength = 3)]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{5,}$", ErrorMessage = "Password must be at least 5 characters and include at least one lowercase letter and one uppercase letter.")]
        public required string Password { get; set; }
    }
}
