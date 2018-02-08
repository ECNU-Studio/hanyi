using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    [Table("admin")]
   public class admin
   {
       [Key]
       ///<summary>
       ///主键id
       ///</summary>
       public int id { get; set; }
       public string username { get; set; }
       public string name { get; set; }
       public string password { get; set; }
       public string phone { get; set; }
    }
}
