using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HanYi.UploadFile
{
    /// <summary>
    /// Excel上传类
    /// </summary>
    public class UploaderExcel
    {
        string state = "SUCCESS";

        string URL = null;
        string currentType = null;
        string uploadpath = null;//文件上传目录
        string filename = null;
        string originalName = null;
        string filesavepath = null; //文件最终保存路径
        HttpPostedFileBase uploadFile = null;

        /**
      * 上传文件的主处理方法
      * @param HttpContext
      * @param string
      * @param  string[]
      *@param int
      * @return Hashtable
      */
        public Hashtable upFile(HttpContextBase cxt, string pathbase, string[] filetype, int size, string username)
        {
            pathbase = pathbase + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            try
            {
                uploadFile = cxt.Request.Files[0];
                originalName = uploadFile.FileName;

                //目录创建
                createFolder();

                //格式验证
                if (checkType(filetype))
                {
                    //不允许的文件类型
                    state = "不允许的文件类型";
                }
                //大小验证
                if (checkSize(size))
                {
                    //文件大小超出网站限制
                    state = "文件大小超出网站限制";
                }
                //保存图片
                if (state == "SUCCESS")
                {
                    filename = NameFormater.Format(cxt.Request["fileNameFormat"], originalName);
                    var testname = filename.Replace(" ", "");
                    var ai = 1;
                    while (File.Exists(uploadpath + testname))
                    {
                        testname = Path.GetFileNameWithoutExtension(filename) + "_" + ai++ + Path.GetExtension(filename);
                    }
                    filesavepath = uploadpath + testname;
                    uploadFile.SaveAs(filesavepath);
                }
            }
            catch (Exception ex)
            {
                state = ex.Message;
            }

            return getUploadInfo();
        }

        /**
         * 获取上传信息
         * @return Hashtable
         */
        private Hashtable getUploadInfo()
        {
            Hashtable infoList = new Hashtable();

            infoList.Add("state", state);
            infoList.Add("filesavepath", filesavepath);
            if (originalName != null)
                infoList.Add("originalName", originalName);
            return infoList;
        }

        //该方法实现从Excel中导出数据到DataSet中，其中filepath为Excel文件的绝对路径， sheetname为excel文件中的表名
        public DataSet ExcelDataSource(string filepath, string ext)
        {
            string strConn;
            strConn = getConnString(filepath, ext);
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "TABLE" });
            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [" + sheetNames.Rows[0]["TABLE_NAME"].ToString() + "]", strConn);
            DataSet ds = new DataSet();
            oada.Fill(ds);
            conn.Close();
            return ds;
        }

        public DataSet ReadExcel(string filepath, string ext)
        {
            FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = null;
            switch (ext)
            {
                case ".xls":
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    break;
                case ".xlsx":
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    break;
                default:
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    break;
            }
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet ds = excelReader.AsDataSet(true);
            stream.Close();
            excelReader.Close();
            return ds;
        }

        //根据后缀名取得链接字符串
        public string getConnString(string filepath, string ext)
        {
            string strConn = "";
            switch (ext)
            {
                case ".xls":
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";
                    break;
                case ".xlsx":
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=Excel 12.0";
                    break;
                default:
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";
                    break;
            }
            return strConn;
        }
        /**
         * 文件类型检测
         * @return bool
         */
        private bool checkType(string[] filetype)
        {
            currentType = getFileExt();
            return Array.IndexOf(filetype, currentType) == -1;
        }

        /**
         * 文件大小检测
         * @param int
         * @return bool
         */
        private bool checkSize(int size)
        {
            return uploadFile.ContentLength >= (size * 1024 * 1024);
        }

        /**
         * 获取文件扩展名
         * @return string
         */
        private string getFileExt()
        {
            string[] temp = uploadFile.FileName.Split('.');
            return "." + temp[temp.Length - 1].ToLower();
        }

        /**
         * 按照日期自动创建存储文件夹
         */
        private void createFolder()
        {
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }
        }
    }


    public static class NameFormater
    {
        public static string Format(string format, string filename)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                format = "{filename}{rand:6}";
            }
            string ext = Path.GetExtension(filename);
            filename = Path.GetFileNameWithoutExtension(filename);
            format = format.Replace("{filename}", filename);
            format = new Regex(@"\{rand(\:?)(\d+)\}", RegexOptions.Compiled).Replace(format, new MatchEvaluator(delegate(Match match)
            {
                var digit = 6;
                if (match.Groups.Count > 2)
                {
                    digit = Convert.ToInt32(match.Groups[2].Value);
                }
                var rand = new Random();
                return rand.Next((int)Math.Pow(10, digit), (int)Math.Pow(10, digit + 1)).ToString();
            }));
            format = format.Replace("{time}", DateTime.Now.Ticks.ToString());
            format = format.Replace("{yyyy}", DateTime.Now.Year.ToString());
            format = format.Replace("{yy}", (DateTime.Now.Year % 100).ToString("D2"));
            format = format.Replace("{mm}", DateTime.Now.Month.ToString("D2"));
            format = format.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            format = format.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            format = format.Replace("{ii}", DateTime.Now.Minute.ToString("D2"));
            format = format.Replace("{ss}", DateTime.Now.Second.ToString("D2"));
            var invalidPattern = new Regex(@"[\\\/\:\*\?\042\<\>\|]");
            format = invalidPattern.Replace(format, "");
            return format + ext;
        }
    }
}