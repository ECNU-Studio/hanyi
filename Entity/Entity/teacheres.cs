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
    /// 教师基本信息
    /// </summary>
    [Table("teacheres")]
    public class teacheres
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 教师登陆名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 教师姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string weixin { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string header { get; set; }
        /// <summary>
        /// 简历
        /// </summary>
        public string cv { get; set; }
        /// <summary>
        /// openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 问答通知
        /// </summary>
        public Nullable<bool> notice_wenda { get; set; }
        /// <summary>
        /// 评论通知
        /// </summary>
        public Nullable<bool> notice_pinglun { get; set; }
        /// <summary>
        /// 问答评论发送邮箱
        /// </summary>
        public Nullable<bool> notice_sendmail { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string introduce { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// qa回复
        /// </summary>
        public bool new_ans { get; set; }
        /// <summary>
        /// 教师所上课程
        /// </summary>
        [ForeignKey("teacherid")]
        public virtual List<courses> courses { set; get; }
        /// <summary>
        /// 语言(1中文，2英文)
        /// </summary>
        public int language { get; set; }
    }
}
