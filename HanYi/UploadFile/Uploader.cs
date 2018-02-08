using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HanYi.UploadFile
{
    public class Uploader
    {
        string state = "SUCCESS";
        int size = 0;
        string URL = null;
        string currentType = null;
        string uploadpath = null;
        string filename = null;
        string originalName = null;
        int width = 0;
        int height = 0;
        HttpPostedFileBase uploadFile = null;
        long filesize = 0;
        /**
      * 上传文件的主处理方法
      * @param HttpContext
      * @param string
      * @param  string[]
      *@param int
      * @return Hashtable
      */
        public Hashtable upFile(HttpContextBase cxt, string pathbase, string[] filetype, double size, string username)
        {
            pathbase = pathbase + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            //uploadpath = pathbase;

            try
            {
                uploadFile = cxt.Request.Files[0];
                originalName = uploadFile.FileName;
                System.Drawing.Image image = System.Drawing.Image.FromStream(uploadFile.InputStream);
                width = image.Width;
                height = image.Height;

                //目录创建
                createFolder();

                //格式验证
                if (checkType(filetype))
                {
                    //不允许的文件类型
                    state = "1";// "\u4e0d\u5141\u8bb8\u7684\u6587\u4ef6\u7c7b\u578b";
                }
                //大小验证
                if (checkSize(size))
                {
                    //文件大小超出网站限制
                    state = "2";// "\u6587\u4ef6\u5927\u5c0f\u8d85\u51fa\u7f51\u7ad9\u9650\u5236";
                }
                //保存图片
                if (state == "SUCCESS")
                {
                    filename = NameFormater.Format(cxt.Request["fileNameFormat"], originalName);
                    string guidstr = Guid.NewGuid().ToString();
                    string extname = Path.GetExtension(filename);
                    filename = guidstr + extname;
                    //var testname = filename.Replace(" ", "");
                    //var ai = 1;
                    //while (File.Exists(uploadpath + testname))
                    //{
                    //    testname = Path.GetFileNameWithoutExtension(filename) + "_" + ai++ + Path.GetExtension(filename);
                    //}
                    //uploadFile.SaveAs(uploadpath + testname);
                    //URL = pathbase + testname;

                    uploadFile.SaveAs(uploadpath + filename);
                    string thumfilename = guidstr + "_thum" + extname;
                    ImageUtil.MakeThumbnail(uploadpath + filename, uploadpath + thumfilename, 300, 300, "HW");
                    URL = pathbase + filename;
                }
            }
            catch (Exception ex)
            {
                // 未知错误
                state = "3";// "\u672a\u77e5\u9519\u8bef";
                URL = ex.Message;
            }
            return getUploadInfo();
        }

        /**
      * 上传文件的主处理方法
      * @param HttpContext
      * @param string
      * @param  string[]
      *@param int
      * @return Hashtable
      */
        public Hashtable upFileYoufang(HttpContextBase cxt, string pathbase, string[] filetype, double size, string username)
        {
            pathbase = pathbase + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            //uploadpath = pathbase;

            try
            {
                uploadFile = cxt.Request.Files[0];
                originalName = uploadFile.FileName;
                System.Drawing.Image image = System.Drawing.Image.FromStream(uploadFile.InputStream);
                width = image.Width;
                height = image.Height;

                //目录创建
                createFolder();

                //格式验证
                if (checkType(filetype))
                {
                    //不允许的文件类型
                    state = "\u4e0d\u5141\u8bb8\u7684\u6587\u4ef6\u7c7b\u578b";
                }
                //大小验证
                if (checkSize(size))
                {
                    //文件大小超出网站限制
                    state = "\u6587\u4ef6\u5927\u5c0f\u8d85\u51fa\u7f51\u7ad9\u9650\u5236";
                }
                //保存图片
                if (state == "SUCCESS")
                {
                    filename = NameFormater.Format(cxt.Request["fileNameFormat"], originalName);
                    string guidstr = Guid.NewGuid().ToString();
                    string extname = Path.GetExtension(filename);
                    filename = guidstr + extname;
                    //var testname = filename.Replace(" ", "");
                    //var ai = 1;
                    //while (File.Exists(uploadpath + testname))
                    //{
                    //    testname = Path.GetFileNameWithoutExtension(filename) + "_" + ai++ + Path.GetExtension(filename);
                    //}
                    //uploadFile.SaveAs(uploadpath + testname);
                    //URL = pathbase + testname;

                    uploadFile.SaveAs(uploadpath + filename);
                    string thumfilename = guidstr + "_thum" + extname;
                    ImageUtil.MakeThumbnail(uploadpath + filename, uploadpath + thumfilename, 414, 276, "HW");
                    URL = pathbase + filename;
                }
            }
            catch (Exception ex)
            {
                // 未知错误
                state = "\u672a\u77e5\u9519\u8bef";
                URL = ex.Message;
            }
            return getUploadInfo();
        }

        /// <summary>
        /// H5上传接口
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="pathbase"></param>
        /// <param name="filetype"></param>
        /// <param name="size"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Hashtable upFileH5(HttpContextBase cxt, string pathbase, string[] filetype, double size, string username)
        {
            pathbase = pathbase + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(cxt.Request.InputStream);
                if (image == null || image.Width == 0)
                {
                    // 未知错误
                    state = "\u672a\u77e5\u9519\u8bef";
                    URL = "上传文件出错";
                }
                width = image.Width;
                height = image.Height;

                //目录创建
                createFolder();

                //格式验证
                if (checkType(filetype))
                {
                    //不允许的文件类型
                    state = "\u4e0d\u5141\u8bb8\u7684\u6587\u4ef6\u7c7b\u578b";
                }
                //大小验证
                //if (checkSize(size))
                //{
                //文件大小超出网站限制
                //state = "\u6587\u4ef6\u5927\u5c0f\u8d85\u51fa\u7f51\u7ad9\u9650\u5236";
                //}
                //保存图片
                if (state == "SUCCESS")
                {
                    filename = NameFormater.Format(cxt.Request["fileNameFormat"], originalName) + ".jpg";
                    string guidstr = Guid.NewGuid().ToString();
                    string extname = Path.GetExtension(filename);
                    filename = guidstr + extname;
                    image.Save(uploadpath + filename);
                    string thumfilename = guidstr + "_thum" + extname;
                    ImageUtil.MakeThumbnail(uploadpath + filename, uploadpath + thumfilename, 300, 300, "HW");
                    URL = pathbase + filename;
                }
            }
            catch (Exception ex)
            {
                // 未知错误
                state = "\u672a\u77e5\u9519\u8bef";
                URL = ex.Message;
            }
            return getUploadInfo();
        }

        /// <summary>
        /// 手机上传图片
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="pathbase"></param>
        /// <param name="filetype"></param>
        /// <param name="size"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Hashtable upFileByPhone(HttpContextBase cxt, string pathbase, string[] filetype, double size, string username)
        {
            pathbase = pathbase + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(cxt.Request.InputStream);

                if (image == null || image.Width == 0)
                {
                    // 未知错误
                    state = "\u672a\u77e5\u9519\u8bef";
                    URL = "上传文件出错";
                }
                width = image.Width;
                height = image.Height;

                ////格式验证
                if (checkType(filetype))
                {
                    //不允许的文件类型
                    state = "\u4e0d\u5141\u8bb8\u7684\u6587\u4ef6\u7c7b\u578b";
                }

                //目录创建
                createFolder();

                //保存图片
                if (state == "SUCCESS")
                {
                    filename = NameFormater.Format(cxt.Request["fileNameFormat"], originalName) + ".jpg";
                    string guidstr = Guid.NewGuid().ToString();
                    string extname = Path.GetExtension(filename);
                    filename = guidstr + "_orgin" + extname;
                    image.Save(uploadpath + filename);
                    string newfilename = guidstr + extname;
                    ImageUtil.MakePhonePic(uploadpath + filename, uploadpath + newfilename, 1000, 1000, "W");
                    string thumfilename = guidstr + "_thum" + extname;
                    ImageUtil.MakeThumbnail(uploadpath + filename, uploadpath + thumfilename, 100, 100, "HW");
                    URL = pathbase + filename;
                }
            }
            catch (Exception ex)
            {
                // 未知错误
                state = "\u672a\u77e5\u9519\u8bef";
                URL = ex.Message;
            }
            return getUploadInfo();
        }

        /// <summary>
        /// 上传媒体文件
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="pathbase"></param>
        /// <param name="filetype"></param>
        /// <param name="size"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Hashtable upMediaFile(HttpContextBase cxt, string pathbase, string[] filetype, double size, string username)
        {
            pathbase = pathbase + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
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
                    state = "\u4e0d\u5141\u8bb8\u7684\u6587\u4ef6\u7c7b\u578b";
                }
                //大小验证
                if (checkSize(size))
                {
                    //文件大小超出网站限制
                    state = "\u6587\u4ef6\u5927\u5c0f\u8d85\u51fa\u7f51\u7ad9\u9650\u5236";
                }
                //保存图片
                if (state == "SUCCESS")
                {
                    filename = NameFormater.Format(cxt.Request["fileNameFormat"], originalName);
                    string guidstr = Guid.NewGuid().ToString();
                    string extname = Path.GetExtension(filename);
                    filename = guidstr + extname;
                    uploadFile.SaveAs(uploadpath + filename);
                    string thumfilename = guidstr + "_thum" + extname;
                    ImageUtil.MakeThumbnail(uploadpath + filename, uploadpath + thumfilename, 300, 300, "HW");
                    URL = pathbase + filename;
                }
            }
            catch (Exception ex)
            {
                // 未知错误
                state = "\u672a\u77e5\u9519\u8bef";
                URL = ex.Message;
            }
            return getUploadInfo();
        }

        /// <summary>
        /// 上传媒体文件
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="pathbase"></param>
        /// <param name="filetype"></param>
        /// <param name="size"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Hashtable upOAFile(HttpContextBase cxt, string pathbase, string[] filetype, double size, string username)
        {
            pathbase = pathbase + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            try
            {
                uploadFile = cxt.Request.Files[0];
                originalName =DateTime.Now.Ticks + uploadFile.FileName;
                filesize = uploadFile.ContentLength;
                //目录创建
                createFolder();

                //格式验证
                if (checkType(filetype))
                {
                    //不允许的文件类型
                    state = "\u4e0d\u5141\u8bb8\u7684\u6587\u4ef6\u7c7b\u578b";
                }
                //大小验证
                //if (checkSize(size))
                //{
                //    //文件大小超出网站限制
                //    state = "\u6587\u4ef6\u5927\u5c0f\u8d85\u51fa\u7f51\u7ad9\u9650\u5236";
                //}
                //保存文件
                if (state == "SUCCESS")
                {
                    uploadFile.SaveAs(uploadpath + originalName);
                    URL = pathbase + originalName;
                }
            }
            catch (Exception ex)
            {
                // 未知错误
                state = "\u672a\u77e5\u9519\u8bef";
                URL = ex.Message;
            }
            return getUploadInfo();
        }

        /**
     * 上传涂鸦的主处理方法
      * @param HttpContext
      * @param string
      * @param  string[]
      *@param string
      * @return Hashtable
     */
        public Hashtable upScrawl(HttpContextBase cxt, string pathbase, string tmppath, string base64Data)
        {
            pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            FileStream fs = null;
            try
            {
                //创建目录
                createFolder();
                //生成图片
                filename = System.Guid.NewGuid() + ".png";
                fs = File.Create(uploadpath + filename);
                byte[] bytes = Convert.FromBase64String(base64Data);
                fs.Write(bytes, 0, bytes.Length);

                URL = pathbase + filename;
            }
            catch (Exception e)
            {
                state = "未知错误";
                URL = "";
            }
            finally
            {
                fs.Close();
                deleteFolder(cxt.Server.MapPath(tmppath));
            }
            return getUploadInfo();
        }

        /**
    * 获取文件信息
    * @param context
    * @param string
    * @return string
    */
        public string getOtherInfo(HttpContextBase cxt, string field)
        {
            string info = null;
            if (cxt.Request.Form[field] != null && !String.IsNullOrEmpty(cxt.Request.Form[field]))
            {
                if (field == "fileName")
                {
                    string[] str = cxt.Request.Form[field].Split(',');
                    if (str != null)
                    {
                        if (str.Length > 1) info = cxt.Request.Form[field].Split(',')[1];
                        else info = cxt.Request.Form[field].Split(',')[0];
                    }
                }
                else
                {
                    info = cxt.Request.Form[field];
                }
            }
            return info;
        }

        /**
         * 获取上传信息
         * @return Hashtable
         */
        private Hashtable getUploadInfo()
        {
            Hashtable infoList = new Hashtable();

            infoList.Add("state", state);
            infoList.Add("url", URL);
            infoList.Add("width", width);
            infoList.Add("height", height);
            infoList.Add("size", size);
            infoList.Add("filesize", filesize);

            if (currentType != null)
                infoList.Add("currentType", currentType);
            if (originalName != null)
                infoList.Add("originalName", originalName);
            return infoList;
        }

        /**
         * 重命名文件
         * @return string
         */
        private string reName()
        {
            return System.Guid.NewGuid() + getFileExt();
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
        private bool checkSize(double size)
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

        /**
         * 删除存储文件夹
         * @param string
         */
        public void deleteFolder(string path)
        {
            //if (Directory.Exists(path))
            //{
            //    Directory.Delete(path, true);
            //}
        }
    }
}