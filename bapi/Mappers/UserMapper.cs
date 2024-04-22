using bapi.Dtos;
using Core.Entities;

namespace bapi.Mappers
{
    public static class UserMapper
    {
        public static UserClaimsDto UserToUserClaimsDto(User user, string token)
        {
            return new UserClaimsDto
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        public static UserDto UserToDto(User user)
        {
            return new UserDto
            {
                Username = user.Username,
                Role = user.Role.ToString()
            };
        }
    }
}
