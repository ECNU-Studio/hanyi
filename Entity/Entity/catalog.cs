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
    /// 课程目录
    /// </summary>
    [Table("catalog")]
    public class catalog
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 目录名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        public int courseid { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 子目录列表
        /// </summary>
        [ForeignKey("catalogid")]
        public virtual List<subcatalog> subcatalog { get; set; }
    }
}
