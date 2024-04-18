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
        public int Id
        {
            get;
            set;
        }
        [Column("username")]
        public string Username
        {
            get;
            set;
        }
        [Column("password")]
        public string Password
        {
            get;
            set;
        }
        [Column("user_role")]
        public Role Role
        {
            get;
            set;
        }
    }
}
