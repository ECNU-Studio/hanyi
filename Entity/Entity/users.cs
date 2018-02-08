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
    /// 用户基本信息
    /// </summary>
    [Table("users")]
    public class users
    {
        [Key]
        ///<summary>
        ///主键id
        ///</summary>
        public int id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string photo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        public int companyid { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// qq
        /// </summary>
        public string qq { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
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
        /// 累计学时
        /// </summary>
        public Nullable<int> total_hours { get; set; }
        /// <summary>
        /// 学习课程
        /// </summary>
        public Nullable<int> total_class { get; set; }
        /// <summary>
        /// 连续天数
        /// </summary>
        public Nullable<int> total_day { get; set; }
        /// <summary>
        /// 上次时间，以便计算是否连续
        /// </summary>
        public Nullable<DateTime> daybefor { get; set; }

        /// <summary>
        /// 连续登录，第一天登录
        /// </summary>
        public Nullable<DateTime> dayfirst { get; set; }
        /// <summary>
        /// 总成绩
        /// </summary>
        public Nullable<float> total_score { get; set; }
        /// <summary>
        /// 已完成课程
        /// </summary>
        public Nullable<int> class_finish { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 是否有新的回复
        /// </summary>
        public bool new_ans { get; set; }
       
        [ForeignKey("userid")]
        public List<classstudent> classstudent { get; set; }
        [ForeignKey("companyid")]
        public companys company { get; set; }
        /// <summary>
        /// 语言(1中文，2英文)
        /// </summary>
        public  int language { get; set; }
        
    }
}
