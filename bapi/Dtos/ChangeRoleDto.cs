using Core.Entities;

namespace bapi.Dtos
{
    public class ChangeRoleDto
    {
        public string Username { get; set; }
        public string NewRole { get; set; }
    }
}
