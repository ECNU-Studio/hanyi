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
    /// 班级分数
    /// </summary>
    [Table("scores")]
    public class scores
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
        /// <summary>
        /// 分数
        /// </summary>
        public Nullable<float> score { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        [ForeignKey("userid")]
        public virtual users user { get; set; }
    }
}
