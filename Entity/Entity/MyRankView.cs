using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class MyRankView
    {
        public int id { get; set; }
        public string classname { get; set; }
        public string cover { get; set; }
        public double score { get; set; }
        //public string wenjuan { get; set; }
        //public string ceshi { get; set; }
        //public string gouxuan { get; set; }
        //public string dafen { get; set; }
        //public string pingjia { get; set; }
        public int order { get; set; }
        public List<classmodels> listcm { get; set; }
        public List<Entity.ClassRankView.classScore> list { get; set; }
    }
}
