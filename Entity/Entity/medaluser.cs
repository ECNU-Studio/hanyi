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
    /// 用户活得的勋章
    /// </summary>
    [Table("medaluser")]
    public class medaluser
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 勋章id
        /// </summary>
        public Nullable<int> medalid { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        public Nullable<int> classid { get; set; }
        /// <summary>
        /// 类型（1勋章 2课程）
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 活动日期
        /// </summary>
        public Nullable<DateTime> date { get; set; }

        [ForeignKey("medalid")]
        public virtual medal medal { get; set; }

        [ForeignKey("classid")]
        public virtual classes classes { get; set; }
    }
}
