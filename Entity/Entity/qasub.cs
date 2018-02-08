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
    /// 问答回复子表
    /// </summary>
    [Table("qasub")]
    public class qasub
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 发表用户id
        /// </summary>
        public Nullable<int> userid { get; set; }
        /// <summary>
        /// 回复用户id
        /// </summary>
        public Nullable<int> touserid { get; set; }
        /// <summary>
        /// 发表的教师id
        /// </summary>
        public Nullable<int> teacherid { get; set; }
        /// <summary>
        /// 回复的的教师id
        /// </summary>
        public Nullable<int> toteacherid { get; set; }        
        /// <summary>
        /// 问答id
        /// </summary>
        public int qaid { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 输入日期
        /// </summary>
        public Nullable<DateTime> date { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }
        public string pic1 { get; set; }
        public string pic2 { get; set; }
        public string pic3 { get; set; }
        public string pic4 { get; set; }
        public string pic5 { get; set; }
        public string pic6 { get; set; }
        /// <summary>
        /// 回复的用户
        /// </summary>
        [ForeignKey("userid")]
        public virtual users user { get; set; }
        /// <summary>
        /// 回复的的用户
        /// </summary>
        [ForeignKey("touserid")]
        public virtual users touser { get; set; }

        [ForeignKey("teacherid")]
        public virtual teacheres teacher { get; set; }

        [ForeignKey("toteacherid")]
        public virtual teacheres toteacher { get; set; }
    }
}
