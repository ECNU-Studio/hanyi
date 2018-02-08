using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    /// <summary>
    /// 学生班级关系表
    /// </summary>
    [Table("classstudent")]
    public class classstudent
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 班级id
        /// </summary>
        public int classid { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userid { get; set; }

       
        
        [ForeignKey("userid")]
        public virtual users users { get; set; }

        [ForeignKey("classid")]
        public virtual classes classes { get; set; }
    }
}
