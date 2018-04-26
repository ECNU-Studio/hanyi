using Entity.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HanYi.Entity
{
    public class HanYiContext: DbContext
    {
        public HanYiContext()
            : base("name=hanyi")
        {
            //this.Configuration.UseDatabaseNullSemantics = true;
        }
        public virtual DbSet<album> album { get; set; }
        public virtual DbSet<albumsub> albumsub { get; set; }
        public virtual DbSet<catalog> catalog { get; set; }
        public virtual DbSet<classes> classes { get; set; }
        public virtual DbSet<classmodels> classmodels { get; set; }
        public virtual DbSet<classstudent> classstudent { get; set; }
        public virtual DbSet<companys> companys { get; set; }
        public virtual DbSet<courses> courses { get; set; }
        public virtual DbSet<qa> qa { get; set; }
        public virtual DbSet<qasub> qasub { get; set; }
        public virtual DbSet<scores> scores { get; set; }
        public virtual DbSet<studystate> studystate { get; set; }
        public virtual DbSet<subcatalog> subcatalog { get; set; }
        public virtual DbSet<subcatalogattachment> subcatalogattachment { get; set; }
        public virtual DbSet<teacheres> teacheres { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<comment> comment { get; set; }
        public virtual DbSet<commentsub> commentsub { get; set; }
        public virtual DbSet<medal> medal { get; set; }
        public virtual DbSet<medaluser> medaluser { get; set; }
        public virtual DbSet<admin> admin { get; set; }
        public virtual DbSet<classaddress> classaddress { get; set; }
        public virtual DbSet<keyvalue> keyvalue { get; set; }
        public virtual DbSet<questionnaire_questionnaire> questionnaire_questionnaire { get; set; }
        public virtual DbSet<questionnaire_runinfo> questionnaire_runinfo { get; set; }
        public virtual DbSet<examination_examination> examination_examination { get; set; }
        public virtual DbSet<examination_takeinfo> examination_takeinfo { get; set; }
    }        
}