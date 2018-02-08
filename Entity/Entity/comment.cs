using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    [Table("comment")]
    public class comment
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string content { get; set; }
        public int c1 { get; set; }
        public int c2 { get; set; }
        public int c3 { get; set; }
        public int c4 { get; set; }
        public int c5 { get; set; }
        public int c6 { get; set; }
        public int c7 { get; set; }
        public int c8 { get; set; }
        public int c9 { get; set; }
        /// <summary>
        /// 班级id
        /// </summary>
        public int classid { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 输入日期
        /// </summary>
        public Nullable<DateTime> date { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 匿名评论
        /// </summary>
        public bool anonymous { get; set; }
        /// <summary>
        /// 评论的用户
        /// </summary>
        [ForeignKey("userid")]
        public virtual users user { get; set; }
        /// <summary>
        /// 课程
        /// </summary>
        [ForeignKey("classid")]
        public virtual classes classes { get; set; }
        /// <summary>
        /// 评论回复
        /// </summary>
        [ForeignKey("commentid")]
        public virtual List<commentsub> commentsub { get; set; }
    }
}
