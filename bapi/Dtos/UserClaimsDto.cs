namespace bapi.Dtos
{
    public class UserClaimsDto
    {
        public required string Username { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
    }
}
