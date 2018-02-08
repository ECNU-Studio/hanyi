using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    [Table("classaddress")]
    public class classaddress
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 班级id
        /// </summary>
        public int classid { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 开课时间
        /// </summary>
        public Nullable<DateTime> datebegin { get; set; }
        /// <summary>
        /// 课程
        /// </summary>
        public string period { get; set; }
    
    }
}
