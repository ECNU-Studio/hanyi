using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class StudystateView
    {
        public int id { get; set; }
        /// <summary>
        /// 班级id
        /// </summary>
        public int classid { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 模块id
        /// </summary>
        public int modelid { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public Nullable<bool> isfinish { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public Nullable<double> score { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
      
        public virtual users user { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public virtual classmodels classmodel { get; set; }
    }
}
