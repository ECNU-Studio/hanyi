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
    [Table("questionnaire_runinfo")]
    public class questionnaire_runinfo
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 问卷回答时间
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 问卷id
        /// </summary>
        public int questionnaire_id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }
    }
}
