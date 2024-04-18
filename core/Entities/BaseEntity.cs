using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        public int Id
        {
            get;
            set;
        }
        [Column("created_date")]
        public DateTime CreatedDate
        {
            get;
            set;
        }
    }
}
