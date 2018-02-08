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
    /// 企业基本信息
    /// </summary>
    [Table("companys")]
    public class companys
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string  name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 法人
        /// </summary>
        public string legalperson { get; set; }
        /// <summary>
        /// 企业地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 企业封面
        /// </summary>
        public string cover { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }
    }
}
