using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class ClassesDAL : BaseDAL
    {
        /// <summary>
        /// 用户列表列表
        /// </summary>
        /// <param name="companyid">企业id</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<classes>, int> getCLasses(int companyid, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classes.Include("classstudent.users").Include("courses.teacher").Include("companys").AsNoTracking()
                            where s.state == true && s.companyid == companyid
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<classes> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<classes>, int> res = new Tuple<List<classes>, int>(list, total);
                return res;
            }
        }
        /// <summary>
        /// 用户列表列表
        /// </summary>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<classes>, int> getCLasses(int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classes.Include("classstudent").Include("courses.teacher").Include("companys").AsNoTracking()
                            where s.state == true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<classes> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<classes>, int> res = new Tuple<List<classes>, int>(list, total);
                return res;
            }
        }
        /// <summary>
        /// 删除班级 以及 班级关联的学生
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool delClasses(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.classes.Include("classstudent")
                                where ids.Contains(s.id)
                                orderby s.id descending
                                select s;
                    var classes = query.ToList();
                    foreach(var item in classes)
                    {
                        db.classstudent.RemoveRange(item.classstudent);
                    }
                    db.classes.RemoveRange(classes);
                    db.SaveChanges();
                    return true;
                }
            }
            else
            {

                return false;
            }
        }
        /// <summary>
        /// 班级详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static classes getClassById(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classes.Include("classstudent").Include("companys").Include("courses").Include("courses.teacher").Include("classmodels").Include("courses.catalog").Include("courses.catalog.subcatalog").Include("courses.catalog.subcatalog.subcatalogattachment")
                            where  s.id==id 
                            select s;
                 return  query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 问卷详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static questionnaire_questionnaire getquestionnaire_questionnaireByClassId(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var coursesid = from s in db.classes where s.id == id select s;
                var courses_id = coursesid.FirstOrDefault().coursesid;
                var query = from s in db.questionnaire_questionnaire
                            where s.course_id == courses_id
                            select s;
                var questionnaire_Questionnaire =  query.FirstOrDefault();
                return questionnaire_Questionnaire;
            }
        }

        /// <summary>
        /// 是否回答过问卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static questionnaire_runinfo getquestionnaire_runinfo(int id,int userid)
        {
            var questionnaire_id = getquestionnaire_questionnaireByClassId(id);
            if (questionnaire_id == null)
            {
                return null;
            }
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.questionnaire_runinfo
                            where s.questionnaire_id == questionnaire_id.id && s.user_id == userid
                            select s;
                var questionnaire_Runinfo = query.FirstOrDefault();
                return questionnaire_Runinfo;
            }
        }
        ///<summary>
        ///获取examinationid
        ///</summary>
        ///<param name="classesid"></param>
        ///<return></return>
        public static examination_examination GetExaminationID(int classesid)
        {
            var classes_info = getClassById(classesid);
            var coures_id = classes_info.coursesid;
            using (HanYiContext db = new HanYiContext())
            {
                var examinationid = from s in db.examination_examination where s.course_id == coures_id select s;
                var examination = examinationid.FirstOrDefault();
                return examination;
            }
        }
        /// <summary>
        /// 是否填写测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static examination_takeinfo GetExamination(int classesid, int userid)
        {
            var examination = GetExaminationID(classesid);
            if (examination == null)
            {
                return null;
            }
            var examination_id = examination.id;
            using (HanYiContext db = new HanYiContext())
            {
                
                var examinationtakeinfo = from s in db.examination_takeinfo where s.examination_id == examination_id && s.user_id == userid select s;
                var examination_takeinfo = examinationtakeinfo.FirstOrDefault();
                return examination_takeinfo;
            }
        }
        /// <summary>
        /// 根据培训师id获取培训师 课程
        /// </summary>
        /// <param name="companyid">教师id</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<classes>, int> getTeacherCLasses(int teacherid, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classes.Include("classstudent").Include("courses").Include("comment").Include("companys").AsNoTracking()
                            where s.state == true && s.courses.teacherid == teacherid
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<classes> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<classes>, int> res = new Tuple<List<classes>, int>(list, total);
                return res;
            }
        }
    }
}
