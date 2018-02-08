using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{

    [Table("keyvalue")]
    public class keyvalue
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// key值
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 要缓存的字符串
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 写入时间
        /// </summary>
        public Nullable<DateTime> dateintput { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public Nullable<DateTime> dateexpiry { get; set; }
    }
}
