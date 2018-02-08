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
    /// 课程模块
    /// </summary>
    [Table("classmodels")]
    public class classmodels
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 标题（供显示）
        /// </summary>
        public string title { get; set; }        
        /// <summary>
        /// 模块类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 占比
        /// </summary>
        public float percent { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        public int classesid { get; set; }
        
    }
}
