using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace HanYi.UploadFile
{

    /// <summary>
    /// 上传结果
    /// </summary>
    public class UpPicRet
    {
        public string state { get; set; }
        public List<PicInfo> piclist { get; set; }
    }

    public class PicInfo
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class UploaderBatch
    {
        string uploadpath = null;

        /**
      * 上传文件的主处理方法
      * @param HttpContext
      * @param string
      * @param  string[]
      *@param int
      * @return Hashtable
      */
        public UpPicRet upFile(HttpContextBase cxt, string pathbase, string[] filetype, double size, string username)
        {
            pathbase = pathbase + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            UpPicRet ret = new UpPicRet
            {
                state = "SUCCESS",
                piclist = null,
            };
            try
            {
                for (int i = 0; i < cxt.Request.Files.Count; i++)
                {
                    HttpPostedFileBase uploadFile = cxt.Request.Files[i];
                    //格式验证
                    if (checkType(uploadFile, filetype))
                    {
                        //不允许的文件类型
                        ret.state = "\u4e0d\u5141\u8bb8\u7684\u6587\u4ef6\u7c7b\u578b";
                        return ret;
                    }
                    //大小验证
                    if (checkSize(uploadFile, size))
                    {
                        //文件大小超出网站限制
                        ret.state = "\u6587\u4ef6\u5927\u5c0f\u8d85\u51fa\u7f51\u7ad9\u9650\u5236";
                        return ret;
                    }
                }

                //目录创建
                createFolder();
                ret.piclist = new List<PicInfo>();
                for (int i = 0; i < cxt.Request.Files.Count; i++)
                {
                    HttpPostedFileBase uploadFile = cxt.Request.Files[i];
                    string originalName = uploadFile.FileName;
                    System.Drawing.Image image = System.Drawing.Image.FromStream(uploadFile.InputStream);
                    int width = image.Width;
                    int height = image.Height;

                    //保存图片
                    string filename = NameFormater.Format(cxt.Request["fileNameFormat"], originalName);
                    string guidstr = Guid.NewGuid().ToString();
                    string extname = Path.GetExtension(filename);
                    filename = guidstr + extname;

                    uploadFile.SaveAs(uploadpath + filename);
                    string thumfilename = guidstr + "_thum" + extname;
                    ImageUtil.MakeThumbnail(uploadpath + filename, uploadpath + thumfilename, 300, 300, "H");
                    string url = pathbase + filename;
                    ret.piclist.Add(new PicInfo()
                    {
                        url = url,
                        width = width,
                        height = height,
                    });
                }
                return ret;
            }
            catch (Exception ex)
            {
                // 未知错误
                ret.state = ex.Message;
                return ret;
            }
        }

        /**
         * 文件类型检测
         * @return bool
         */
        private bool checkType(HttpPostedFileBase uploadFile, string[] filetype)
        {
            string currentType = getFileExt(uploadFile);
            return Array.IndexOf(filetype, currentType) == -1;
        }

        /**
         * 文件大小检测
         * @param int
         * @return bool
         */
        private bool checkSize(HttpPostedFileBase uploadFile, double size)
        {
            return uploadFile.ContentLength >= (size * 1024 * 1024);
        }

        /**
         * 获取文件扩展名
         * @return string
         */
        private string getFileExt(HttpPostedFileBase uploadFile)
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
}