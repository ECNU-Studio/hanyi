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
    [Table("examination_examination")]
    public class examination_examination
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        public int is_published { get; set; }
        /// <summary>
        /// 填写问卷人数
        /// </summary>
        public int take_nums { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        public int course_id { get; set; }
        /// <summary>
        /// 是否随机
        /// </summary>
        public int is_random { get; set; }
        /// <summary>
        /// 问题数量
        /// </summary>
        public int question_count { get; set; }
    }
}
