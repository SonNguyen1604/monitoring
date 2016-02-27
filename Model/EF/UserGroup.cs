using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    [Table("UserGroup")]
    public class UserGroup
    {
        [Key]
        [StringLength(20)]
        public string ID {get;set;}

        [StringLength(50)]
        public string Name { get; set; }
    }
}
