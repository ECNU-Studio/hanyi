using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    [Table("commentsub")]
    public class commentsub
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
          /// <summary>
          /// 评论id
          /// </summary>
        public int commentid { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Nullable< int> userid { get; set; }
        /// <summary>
        /// 教师id
        /// </summary>
        public Nullable< int> teacherid { get; set; }
        /// <summary>
        /// 输入日期
        /// </summary>
        public Nullable<DateTime> date { get; set; } 
        /// <summary>
        /// 有效
        /// </summary>
        public bool state { get; set; }

        /// <summary>
        /// 评论的用户
        /// </summary>
        [ForeignKey("userid")]
        public virtual users user { get; set; }
        /// <summary>
        /// 教师
        /// </summary>

        [ForeignKey("teacherid")]
        public virtual teacheres teacher { get; set; }
    }
}
