using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("member")]
    public class User : BaseEntity
    {
        [Key]
        [Column("id")]
        public new int Id
        {
            get;
            set;
        }
        [Column("username")]
        public required string Username
        {
            get;
            set;
        }
        [Column("password")]
        public required string Password
        {
            get;
            set;
        }
        [Column("user_role")]
        public required Role Role
        {
            get;
            set;
        }
    }
}
