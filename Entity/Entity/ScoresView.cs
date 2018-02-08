using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class ScoresView
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
        /// 分数
        /// </summary>
        public Nullable<float> score { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>        
        public virtual users user { get; set; }
    }
}
