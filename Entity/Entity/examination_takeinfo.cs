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
    [Table("examination_takeinfo")]
    public class examination_takeinfo
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }
        /// <summary>
        /// 测试id
        /// </summary>
        public int examination_id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }
    }
}
