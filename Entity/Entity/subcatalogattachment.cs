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
    /// 子目录附近表
    /// </summary>
    [Table("subcatalogattachment")]
    public class subcatalogattachment
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }

        /// <summary>
        /// 子目录id
        /// </summary>
        public int subcatalogid { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public Nullable<float> size { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }

    }
}
