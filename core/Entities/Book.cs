using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
