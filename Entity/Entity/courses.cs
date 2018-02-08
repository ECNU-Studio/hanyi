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
    /// 课程基本信息
    /// </summary>
    [Table("courses")]
    public class courses
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 课程简介
        /// </summary>
        public string coursesabstract { get; set; }
        /// <summary>
        /// 简介附件
        /// </summary>
        public string abstractFile { get; set; }  
        /// <summary>
        /// 简介附件名称
        /// </summary>
        public string abstractFilename { get; set; }
        /// <summary>
        /// 简介附件文件大小
        /// </summary>
        public Nullable<float> abstractFilesize { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string cover { get; set; }
        /// <summary>
        /// 勋章图
        /// </summary>
        public string honor { get; set; }
        /// <summary>
        /// 讲师id
        /// </summary>
        public int teacherid { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 此课程的上课教师
        /// </summary>
        [ForeignKey("teacherid")]
        public virtual teacheres teacher { get; set; }

        /// <summary>
        /// 上次课程的班级
        /// </summary>
         [ForeignKey("coursesid")]
        public virtual List<classes> classes { get; set; }
        /// <summary>
        /// 课程目录
        /// </summary>
        [ForeignKey("courseid")]
         public virtual List<catalog> catalog { get; set; }
    }
}
