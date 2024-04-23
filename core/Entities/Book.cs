using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    [Table("book")]
    public class Book : BaseEntity
    {
        [Key]
        [Column("id")]
        public new int Id
        {
            get;
            set;
        }
        [Column("name")]
        public required string Name
        {
            get;
            set;
        }
        [Column("description")]
        public required string Description
        {
            get;
            set;
        }
        [Column("modified_date")]
        public DateTime ModifiedDate
        {
            get;
            set;
        }
    }
}
