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
    /// 子目录
    /// </summary>
    [Table("subcatalog")]
    public class subcatalog
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
        /// 目录id
        /// </summary>
        public int catalogid { get; set; }
        /// <summary>
        /// 顺序索引
        /// </summary>
        public int index { get; set; }
        
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }
        [ForeignKey("subcatalogid")]
        public virtual List<subcatalogattachment> subcatalogattachment { get; set; }
    }
}
