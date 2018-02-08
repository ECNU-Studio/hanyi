using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class ClassRankView
    {
        public int id { get; set; }
        public string username { get; set; }
        public string head { get; set; }
        public double score { get; set; }
        //public string wenjuan { get; set; }
        //public string ceshi { get; set; }
        //public string gouxuan { get; set; }
        //public string dafen { get; set; }
        //public string pingjia { get; set; }

        public List<classScore> list { get; set; }

        public class classScore{
            public int id { get; set; }
            public string score { get; set; }
            public bool finish { get; set; }
        }
    }
}
