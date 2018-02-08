﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    /// <summary>
    /// 相册基本信息
    /// </summary>
    [Table("album")]
    public class album
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 文字描述
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 班级id
        /// </summary>
        public int classid { get; set; }
        /// <summary>
        /// 发表用户id
        /// </summary>
        public Nullable<int> userid { get; set; }
        public Nullable< int> teacherid { get; set; }
        public string pic1 { get; set; }
        public string pic2 { get; set; }
        public string pic3 { get; set; }
        public string pic4 { get; set; }
        public string pic5 { get; set; }
        public string pic6 { get; set; }
        /// <summary>
        /// 输入日期
        /// </summary>
        public Nullable<DateTime> date { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 回复的用户
        /// </summary>
        [ForeignKey("albumid")]
        public virtual List<albumsub> albumsub { get; set; }
        /// <summary>
        /// 回复的用户
        /// </summary>
        [ForeignKey("userid")]
        public virtual users user { get; set; }

        [ForeignKey("teacherid")]
        public virtual teacheres teacher { get; set; }
    }
}
