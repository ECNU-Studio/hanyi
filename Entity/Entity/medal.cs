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
    /// 勋章类
    /// </summary>
    [Table("medal")]
    public class medal
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 勋章名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 勋章数据
        /// </summary>
        public int data { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public medaltype type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string des { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 勋章显示图片（获得）
        /// </summary>
        public string pichave { get; set; }
        /// <summary>
        /// 勋章图片（未获得）
        /// </summary>
        public string picno { get; set; }
    }

    public enum medaltype
    {
        XUESHI = 1,
        KECHENG = 2,
        TIANSHU = 3,
        CHENGJI = 4
    }
}
