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
    /// 班级基本信息
    /// </summary>
    [Table("classes")]
    public class classes
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        public int companyid { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        public int coursesid { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public Nullable<DateTime> schooltime { get; set; }
        /// <summary>
        /// 上课地点
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 周期
        /// </summary>
        public string period { get; set; }
        /// <summary>
        /// 学时
        /// </summary>
        public Nullable<int> hour { get; set; }
        /// <summary>
        /// 上次课程的学生
        /// </summary>
        [ForeignKey("classid")]
        public virtual List<classstudent> classstudent { get; set; }
        /// <summary>
        /// 此班级要上的课程
        /// </summary>
        [ForeignKey("coursesid")]
        public virtual courses courses { get; set; }
        /// <summary>
        /// 上此课程的公司
        /// </summary>

        [ForeignKey("companyid")]
        public virtual companys companys { get; set; }
        /// <summary>
        /// 课程模块
        /// </summary>
        [ForeignKey("classesid")]
        public virtual List<classmodels> classmodels { get; set; }

        [ForeignKey("classid")]
        public virtual List<comment> comment { get; set; }
        [ForeignKey("classid")]
        public virtual List<classaddress> classaddress { get; set; }

    }
}
