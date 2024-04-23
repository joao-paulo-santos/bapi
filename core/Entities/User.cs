using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
