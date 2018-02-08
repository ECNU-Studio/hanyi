using HanYi.UploadFile;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace HanYi.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult GetQRSpecail(string url, bool changeColor = false, string strColorDf = "#00714b27", int width = 240, int height = 240)
        {
            try
            {
                EncodingOptions options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    Width = 240,
                    Height = 240,
                    ErrorCorrection = ErrorCorrectionLevel.H
                };
                BarcodeWriter writer = new BarcodeWriter();
                if (changeColor)
                {
                    options.Width = width;
                    options.Height = height;
                    string strColor = strColorDf;
                    Color col = ColorTranslator.FromHtml(strColor);
                    writer.Renderer = new ZXing.Rendering.BitmapRenderer { Foreground = col, Background = Color.White };
                }
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options = options;
                Bitmap bitmap = writer.Write(url);
                bitmap.MakeTransparent(Color.White);
                string pathbase = "/OriginalPicture/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                string uploadpath = Server.MapPath(pathbase);
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string guidstr = Guid.NewGuid().ToString();
                string filename = guidstr + ".png";
                bitmap.Save(uploadpath + filename, ImageFormat.Png);
                bitmap.Dispose();
                string extname = Path.GetExtension(filename);
                string thumfilename = guidstr + "_thum" + extname;
                ImageUtil.MakeThumbnail(uploadpath + filename, uploadpath + thumfilename, 300, 300, "HW");
                string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
                return Content("{'url':'" + publishurl + pathbase + filename + "','width':'" + options.Width + "','height':'" + options.Height + "','state':'SUCCESS'}");  //向浏览器返回数据json数据
            }
            catch (Exception ex)
            {
                return Content("{'url':'','width':'','height':'','state':'" + ex.Message + "'}");
            }
        }



        /// <summary>
        /// 图片上传接口
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageUpload()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 30;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upFile(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','width':'" + info["width"] + "','height':'" + info["height"] + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }

        /// <summary>
        /// 图片上传接口
        /// </summary>
        /// <returns></returns>
        public ActionResult FileUpload()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 30;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp", ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".txt", ".excel", ".xls", ".xlsx", ".mp3", ".mp4", ".avi", ".rmvb", ".3gp", ".flash", ".wma" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upOAFile(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','width':'" + info["width"] + "','height':'" + info["height"] + "','original':'" + oriName + "','state':'" + info["state"] + "','filesize':'" + info["filesize"] + "'}");  //向浏览器返回数据json数据
        }

        /// <summary>
        /// 图片上传接口
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageUploadYF()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 0.3;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp", ".pdf", ".doc", ".docx" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upFileYoufang(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','width':'" + info["width"] + "','height':'" + info["height"] + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }

        /// <summary>
        /// 图片上传接口
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageUploadH5()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 0.3;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp", ".pdf", ".doc", ".docx" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upFileH5(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','width':'" + info["width"] + "','height':'" + info["height"] + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }

        /// <summary>
        /// 手机上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageUploadByPhone()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 10;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp", ".pdf", ".doc", ".docx" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upFileByPhone(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','width':'" + info["width"] + "','height':'" + info["height"] + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }

        /// <summary>
        /// 裁剪头像图片
        /// </summary>
        /// <param name="picurl"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="cw"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CutPic()
        {
            try
            {
                if (String.IsNullOrEmpty(Request.Form["x1"]) || String.IsNullOrEmpty(Request.Form["y1"]) || String.IsNullOrEmpty(Request.Form["cw"])
                    || String.IsNullOrEmpty(Request.Form["ch"]))
                {
                    return Json(new { src = "", success = false });
                }
                int intx1 = Convert.ToInt32(Request.Form["x1"]);
                int inty1 = Convert.ToInt32(Request.Form["y1"]);
                int intcw = Convert.ToInt32(Request.Form["cw"]);
                int intch = Convert.ToInt32(Request.Form["ch"]);

                //网络地址转成本地地址
                //http://upload.3wzc.com//OriginalPicture/20141117/6ed9059e-42b2-4886-bd2b-bc84be5996db.jpg
                //http://image.ltchina.com/OriginalPicture/20150610/b30143bd-cc04-46c5-acad-5beb706fb785.jpg
                string pathbase = Request.Form["picurl"].ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["PublishUrl"], "");

                string uploadpath = Server.MapPath(pathbase);//获取文件上传路径

                Stream imgStream = GetLocalStream(uploadpath);
                pathbase = "/OriginalPicture/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid() + ".jpeg";
                Cut(imgStream, Server.MapPath(pathbase), intx1, inty1, intcw, intch, 100);
                string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
                publishurl += pathbase;
                return Json(new { src = publishurl, success = true });
            }
            catch
            {
                return Json(new { src = "", success = false });
            }
        }



        [HttpGet]
        public ActionResult GetPic()
        {
            try
            {

                int intx1 = Convert.ToInt32(Request.Params["x1"]);
                int inty1 = Convert.ToInt32(Request.Params["y1"]);
                int intcw = Convert.ToInt32(Request.Params["cw"]);
                int intch = Convert.ToInt32(Request.Params["ch"]);

                //网络地址转成本地地址
                //http://upload.3wzc.com//OriginalPicture/20141117/6ed9059e-42b2-4886-bd2b-bc84be5996db.jpg
                string pathbase = Request.Params["picurl"].ToString().Replace(@"http://upload.3wzc.com", "");
                string uploadpath = Server.MapPath(pathbase + ".jpg");//获取文件上传路径

                Stream imgStream = GetLocalStream(uploadpath);
                pathbase = "/OriginalPicture/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid() + ".jpeg";
                System.Drawing.Image resimge = GetImage(imgStream, Server.MapPath(pathbase), intx1, inty1, intcw, intch, 100);
                ImageResult result = new ImageResult(resimge, System.Drawing.Imaging.ImageFormat.Jpeg);
                return result;
            }
            catch
            {
                return Json(new { src = "", success = false }, JsonRequestBehavior.AllowGet);
            }
        }



        #region 图片操作

        /// <summary>
        /// 将一个文件读成字符流
        /// </summary>
        /// <param name="InFilePath"></param>
        /// <returns></returns>
        public static Stream GetLocalStream(string InFilePath)
        {
            return new MemoryStream(ReadFileReturnBytes(InFilePath));
        }

        /// <summary>从文件中读取二进制数据</summary>
        /// <param name="filePath">文件路径</param>
        /// <returns> byte[]二进制数据</returns>
        public static byte[] ReadFileReturnBytes(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] buff = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return buff;
        }

        #region 裁剪操作
        public static void Cut(System.IO.Stream fromFile, string fileSaveUrl, int xPosition, int yPosition, int width, int height, int quality)
        {
            //创建目录
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);
            //原始图片的宽、高
            int initWidth = initImage.Width;
            int initHeight = initImage.Height;
            if (xPosition + width > initWidth)
                width = initWidth - xPosition;
            if (yPosition + height > initHeight)
                height = initHeight - yPosition;
            //与原图相等直接保存
            if ((width >= initWidth && height >= initHeight) || (width < 1 && height < 1))
            {
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                System.Drawing.Image pickedImage = null;
                System.Drawing.Graphics pickedG = null;
                //对象实例化
                pickedImage = new System.Drawing.Bitmap(width, height);
                pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                //设置质量
                pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //定位
                System.Drawing.Rectangle fromR = new System.Drawing.Rectangle(xPosition, yPosition, width, height);
                System.Drawing.Rectangle toR = new System.Drawing.Rectangle(0, 0, width, height);
                //画图
                pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                System.Drawing.Imaging.ImageCodecInfo[] icis = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                System.Drawing.Imaging.ImageCodecInfo ici = null;
                foreach (System.Drawing.Imaging.ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters(1);
                ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
                //保存缩略图
                pickedImage.Save(fileSaveUrl, ici, ep);
                //释放关键质量控制所用资源
                ep.Dispose();
                //释放截图资源
                pickedG.Dispose();
                pickedImage.Dispose();
                //释放原始图片资源
                initImage.Dispose();
            }
        }



        public System.Drawing.Image GetImage(System.IO.Stream fromFile, string fileSaveUrl, int xPosition, int yPosition, int width, int height, int quality)
        {
            //创建目录
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);
            //原始图片的宽、高
            int initWidth = initImage.Width;
            int initHeight = initImage.Height;
            if (xPosition + width > initWidth)
                width = initWidth - xPosition;
            if (yPosition + height > initHeight)
                height = initHeight - yPosition;
            //与原图相等直接保存
            if ((width >= initWidth && height >= initHeight) || (width < 1 && height < 1))
            {

                return initImage;
            }
            else
            {
                System.Drawing.Image pickedImage = null;
                System.Drawing.Graphics pickedG = null;
                //对象实例化
                pickedImage = new System.Drawing.Bitmap(width, height);
                pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                //设置质量
                pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //定位
                System.Drawing.Rectangle fromR = new System.Drawing.Rectangle(xPosition, yPosition, width, height);
                System.Drawing.Rectangle toR = new System.Drawing.Rectangle(0, 0, width, height);
                //画图
                pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                //释放原始图片资源
                initImage.Dispose();
                return pickedImage;


                //ep.Dispose();
                ////释放截图资源
                //pickedG.Dispose();
                //pickedImage.Dispose();


            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult GetQrcode(string url, bool changeColor = false, int width = 200, int height = 200)
        {
            try
            {
                EncodingOptions options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    Width = 200,
                    Height = 200,
                    ErrorCorrection = ErrorCorrectionLevel.H
                };
                BarcodeWriter writer = new BarcodeWriter();
                if (changeColor)
                {
                    writer.Renderer = new ZXing.Rendering.BitmapRenderer { Foreground = Color.FromArgb(255, 89, 105), Background = Color.Empty };
                }
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options = options;
                Bitmap bitmap = writer.Write(url);
                string pathbase = "/OriginalPicture/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                //string pathbase = "/Qrcode/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                string uploadpath = Server.MapPath(pathbase);//获取文件保存路径
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string guidstr = Guid.NewGuid().ToString();
                string filename = guidstr + ".jpeg";
                bitmap.Save(uploadpath + filename, ImageFormat.Jpeg);
                bitmap.Dispose();
                string extname = Path.GetExtension(filename);
                string thumfilename = guidstr + "_thum" + extname;
                ImageUtil.MakeThumbnail(uploadpath + filename, uploadpath + thumfilename, 300, 300, "HW");
                string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
                return Content("{'url':'" + publishurl + pathbase + filename + "','width':'" + options.Width + "','height':'" + options.Height + "','state':'SUCCESS'}");  //向浏览器返回数据json数据
            }
            catch (Exception ex)
            {
                return Content("{'url':'','width':'','height':'','state':'" + ex.Message + "'}");  //向浏览器返回数据json数据
            }
        }

        public ActionResult GetQrcodeHasBG(string url, int widthD = 280, int heightD = 280, int towidthD = 40, int toheightD = 40
            , int towidthDD = 200, int toheightDD = 200
            , int R = 255, int G = 89, int B = 105)
        {
            try
            {
                string waterfilepath = System.Web.HttpContext.Current.Server.MapPath(url);

                Image originalImage = Image.FromFile(waterfilepath);

                int width = widthD;
                int height = heightD;

                int towidth = towidthDD;
                int toheight = toheightDD;

                int fromwidth = towidthD;
                int fromheight = toheightD;

                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;

                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    oh = originalImage.Height;
                    ow = originalImage.Height * towidth / toheight;
                    y = 0;
                    x = (originalImage.Width - ow) / 2;
                }
                else
                {
                    ow = originalImage.Width;
                    oh = originalImage.Width * toheight / towidth;
                    x = 0;
                    y = (originalImage.Height - oh) / 2;
                }
                //新建一个bmp图片
                Image bitmap = new System.Drawing.Bitmap(width, height);

                //新建一个画板
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);

                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.FromArgb(R, G, B));

                //g.FillRectangle(Brushes.White, 0, 0, towidth, toheight); 

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(fromwidth, fromheight, towidth, toheight),
                    new Rectangle(x + 15, y + 15, ow - 30, oh - 30),
                    GraphicsUnit.Pixel);

                string pathbase = "/OriginalPicture/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                string uploadpath = System.Web.HttpContext.Current.Server.MapPath(pathbase);
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string guidstr = Guid.NewGuid().ToString();
                string filename = guidstr + ".jpeg";
                try
                {
                    //以jpg格式保存缩略图
                    bitmap.Save(uploadpath + filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }

                string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
                return Content("{'url':'" + publishurl + pathbase + filename + "','width':'" + towidth + "','height':'" + toheight + "','state':'SUCCESS'}");  //向浏览器返回数据json数据
            }
            catch (Exception ex)
            {
                return Content("{'url':'','width':'','height':'','state':'" + ex.Message + "'}");  //向浏览器返回数据json数据
            }
        }

        public ActionResult GetQrcodeBG(string url, int towidthD = 466, int toheightD = 549
            , int widthD = 240, int heidthD = 240, int widthDP = 105, int heightDP = 280, int widthDSP = 0, int heightDSP = 0)
        {
            try
            {
                string originalImagePath = System.Web.HttpContext.Current.Server.MapPath("~/img/QRMonkey.png");
                string waterfilepath = System.Web.HttpContext.Current.Server.MapPath(url);

                Image originalImage = Image.FromFile(originalImagePath);

                Image originalImage_1 = Image.FromFile(waterfilepath);

                int OI1_width = widthD;
                int OI1_height = heidthD;
                int OI1_PW = widthDP;
                int OI1_PH = heightDP;
                int IO1_SPW = widthDSP;
                int IO1_SPH = heightDSP;

                int towidth = towidthD;
                int toheight = toheightD;

                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;

                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    oh = originalImage.Height;
                    ow = originalImage.Height * towidth / toheight;
                    y = 0;
                    x = (originalImage.Width - ow) / 2;
                }
                else
                {
                    ow = originalImage.Width;
                    oh = originalImage.Width * toheight / towidth;
                    x = 0;
                    y = (originalImage.Height - oh) / 2;
                }

                //新建一个bmp图片
                Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

                //新建一个画板
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);

                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                    new Rectangle(x, y, ow, oh),
                    GraphicsUnit.Pixel);

                g.DrawImage(originalImage_1, new Rectangle(OI1_PW, OI1_PH, OI1_width, OI1_height),
                    new Rectangle(IO1_SPW, IO1_SPH, OI1_width, OI1_height),
                    GraphicsUnit.Pixel);

                string pathbase = "/OriginalPicture/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                string uploadpath = System.Web.HttpContext.Current.Server.MapPath(pathbase);
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string guidstr = Guid.NewGuid().ToString();
                string filename = guidstr + ".png";
                try
                {
                    //以jpg格式保存缩略图
                    bitmap.Save(uploadpath + filename, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }

                string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
                return Content("{'url':'" + publishurl + pathbase + filename + "','width':'" + towidth + "','height':'" + toheight + "','state':'SUCCESS'}");  //向浏览器返回数据json数据
            }
            catch (Exception ex)
            {
                return Content("{'url':'','width':'','height':'','state':'" + ex.Message + "'}");  //向浏览器返回数据json数据
            }
        }

        /// <summary>
        /// 媒体文件上传接口
        /// </summary>
        /// <returns></returns>
        public ActionResult MediaUpload()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 30;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".mp3", ".mp4", ".avi", ".rmvb", ".3gp", ".flash", ".wma" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upMediaFile(HttpContext, "/OriginalPicture/Media", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取媒体描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }


        /// <summary>
        /// 图片上传接口
        /// </summary>
        /// <returns></returns>
        public ActionResult LargeMediaUpload()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 40;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".mpg", ".avi", ".wma", ".mp4", ".ogg", ".webm" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upMediaFile(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','width':'" + info["width"] + "','height':'" + info["height"] + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }
        /// <summary>
        /// 宝库中国专用上传接口
        /// </summary>
        /// (jingch)
        /// <returns></returns>
        public ActionResult BaoKuUpload()
        {
            /* 可选格式为图片文件 视频文件 音频文件 文档文件*/
            /*视频格式支持：.mpg", ".avi", ".wma", ".mp4", ".ogg", ".webm",".3gp",".avi"*/
            /*音频格式支持：.mp3", ".wav", ".ava"*/
            /*图片格式支持：.gif;.png;.jpg;.jpeg;.bmp*/
            /*文档格式支持：".doc",.docx", ".pdf", ".ppt", ".txt",".excel", ".zip", ".rar",".xls",".xlsx"*/
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 50;           //文件大小限制,单位MB  
            //文件大小限制，单位MB
            string[] filetype = { ".mpg", ".avi", ".3gp", ".wma", ".mp4", ".avi", ".ogg", ".webm", ".mp3", ".wav", ".ava", ".gif", ".png", ".jpg", ".jpeg", ".bmp", ".doc", ".docx", ".pdf", ".ppt", ".txt", ".excel", ".zip", ".rar", ".xls", ".xlsx", ".7z" };         //文件允许格式
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upMediaFile(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','width':'" + info["width"] + "','height':'" + info["height"] + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }
        /// <summary>
        /// 图片上传接口
        /// </summary>
        /// <returns></returns>
        public ActionResult LargePicUpload()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 2;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upFile(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','width':'" + info["width"] + "','height':'" + info["height"] + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }



        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult MakeQrcode(string url, string textNum, long taskmainid, long productid)
        {
            try
            {
                url = System.Web.HttpUtility.UrlDecode(url);
                string file = taskmainid + "-" + productid;
                EncodingOptions options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    Width = 200,
                    Height = 200,
                    ErrorCorrection = ErrorCorrectionLevel.H
                };
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options = options;
                Bitmap bitmap = writer.Write(url);
                string pathbase = "/OriginalPicture/Qrcode/" + file + "/";
                string uploadpath = System.Web.HttpContext.Current.Server.MapPath(pathbase);//获取文件保存路径
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string filename = file + "-" + textNum + "-" + DateTime.Now.Ticks + ".jpeg";
                if (!System.IO.File.Exists(uploadpath + filename))
                {
                    bitmap.Save(uploadpath + filename, ImageFormat.Jpeg);
                }
                string waterfilepath = System.Web.HttpContext.Current.Server.MapPath("~/img/code.png");
                string filepath = uploadpath + filename;
                //加logo
                ImageWaterMarkPic(bitmap, filepath, waterfilepath, textNum, 5, 100, 10, "Microsoft YaHei", 16);

                bitmap.Dispose();

                string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
                return Content("{'url':'" + publishurl + pathbase + filename + "','width':'" + options.Width + "','height':'" + options.Height + "','state':'SUCCESS'}");  //向浏览器返回数据json数据
            }
            catch (Exception ex)
            {
                return Content("{'url':'','width':'','height':'','state':'" + ex.Message + "'}");  //向浏览器返回数据json数据
            }
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult MakeQrcodeV2(string url, string textNum, long taskid, string tasktype)
        {
            try
            {
                url = System.Web.HttpUtility.UrlDecode(url);
                string file = taskid + "-" + tasktype;
                EncodingOptions options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    Width = 200,
                    Height = 200,
                    ErrorCorrection = ErrorCorrectionLevel.H
                };
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                writer.Options = options;
                Bitmap bitmap = writer.Write(url);
                string pathbase = "/OriginalPicture/QrcodeV2/" + file + "/";
                string uploadpath = System.Web.HttpContext.Current.Server.MapPath(pathbase);//获取文件保存路径
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string filename = file + "-" + textNum + "-" + DateTime.Now.Ticks + ".jpeg";
                if (!System.IO.File.Exists(uploadpath + filename))
                {
                    bitmap.Save(uploadpath + filename, ImageFormat.Jpeg);
                }
                string waterfilepath = System.Web.HttpContext.Current.Server.MapPath("~/img/code.png");
                string filepath = uploadpath + filename;
                //加logo
                ImageWaterMarkPic(bitmap, filepath, waterfilepath, textNum, 5, 100, 10, "Microsoft YaHei", 16);

                bitmap.Dispose();

                string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
                return Content("{'url':'" + publishurl + pathbase + filename + "','width':'" + options.Width + "','height':'" + options.Height + "','state':'SUCCESS'}");  //向浏览器返回数据json数据
            }
            catch (Exception ex)
            {
                return Content("{'url':'','width':'','height':'','state':'" + ex.Message + "'}");  //向浏览器返回数据json数据
            }
        }

        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="img">要加水印的原图?(System.Drawing)</param>
        /// <param name="filename">文件名</param>
        /// <param name="watermarkFilename">水印文件名</param>
        /// <param name="watermarkStatus">图片水印位置1=左上 2=中上 3=右上 4=左中  5=中中 6=右中 7=左下 8=右中 9=右下</param>
        /// <param name="quality">加水印后的质量0~100,数字越大质量越高</param>
        /// <param name="watermarkTransparency">水印图片的透明度1~10,数字越小越透明,10为不透明</param>
        public static void ImageWaterMarkPic(Image img, string filename, string watermarkFilename, string watermarkText, int watermarkStatus, int quality, int watermarkTransparency, string fontname, int fontsize)
        {
            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Image watermark = new Bitmap(watermarkFilename);

            if (watermark.Height > img.Height / 5 || watermark.Width > img.Width / 5)
            {
                watermark = MakeThumbnail(watermark, img.Width / 5, img.Height / 5, "HW");
            }

            int xpos = 0;
            int ypos = 0;

            float xxpos = 0;
            float yypos = 0;


            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            SizeF crSize;
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            crSize = g.MeasureString(watermarkText, drawFont);
            switch (watermarkStatus)
            {
                case 1:
                    xxpos = (float)img.Width * (float).01;
                    yypos = (float)img.Height * (float).01;
                    break;
                case 2:
                    xxpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    yypos = (float)img.Height * (float).01;
                    break;
                case 3:
                    xxpos = ((float)img.Width * (float).99) - crSize.Width;
                    yypos = (float)img.Height * (float).01;
                    break;
                case 4:
                    xxpos = (float)img.Width * (float).01;
                    yypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 5:
                    xxpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    yypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 6:
                    xxpos = ((float)img.Width * (float).99) - crSize.Width;
                    yypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 7:
                    xxpos = (float)img.Width * (float).01;
                    yypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 8:
                    xxpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    yypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 9:
                    xxpos = ((float)img.Width * (float).99) - crSize.Width;
                    yypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
            }
            ImageAttributes imageAttributes = new ImageAttributes();

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);


            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xxpos, yypos);


            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
                img.Save(filename, ici, encoderParams);
            else
                img.Save(filename);

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }


        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 返回缩略图
        /// </summary>
        /// <param name="originalImagePath">源图</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static Image MakeThumbnail(Image originalImage, int width, int height, string mode)
        {
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            originalImage.Dispose();
            g.Dispose();

            return bitmap;

        }


        /// <summary>
        /// PDF\PPT上传接口
        /// </summary>
        /// <returns></returns>
        public ActionResult LargeFileUpload()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["fetch"]))
            {
                return Content(String.Format("updateSavePath([{0}]);", "\"OriginalPicture\""), "text/javascript;charset=utf-8");
            }

            Response.ContentType = "text/plain";

            //上传配置
            double size = 40;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".pdf", ".ppt" };         //文件允许格式


            //上传图片
            Hashtable info = new Hashtable();
            Uploader up = new Uploader();

            string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
            //获取上传状态
            info = up.upMediaFile(HttpContext, "/OriginalPicture", filetype, size, User.Identity.Name);
            string title = up.getOtherInfo(HttpContext, "pictitle");                   //获取图片描述
            string oriName = up.getOtherInfo(HttpContext, "fileName");                //获取原始文件名
            return Content("{'url':'" + publishurl + info["url"] + "','title':'" + title + "','original':'" + oriName + "','state':'" + info["state"] + "'}");  //向浏览器返回数据json数据
        }

        /// <summary>
        /// 生成缩略图接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MakeThumbnailInterface()
        {
            try
            {
                int w = Convert.ToInt32(Request.Form["w"] == null ? "300" : Request.Form["w"]);
                int h = Convert.ToInt32(Request.Form["h"] == null ? "300" : Request.Form["h"]);
                string mode = Request.Form["mode"];

                //网络地址转成本地地址
                //http://upload.3wzc.com//OriginalPicture/20141117/6ed9059e-42b2-4886-bd2b-bc84be5996db.jpg
                //http://image.ltchina.com/OriginalPicture/20150610/b30143bd-cc04-46c5-acad-5beb706fb785.jpg
                string pathbase = Request.Form["picurl"].ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["PublishUrl"], "");

                string uploadpath = Server.MapPath(pathbase);//获取文件上传路径
                string originalImagePath = "";
                string thumbnailPath = "";

                //Stream imgStream = GetLocalStream(uploadpath);
                //pathbase = "/OriginalPicture/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid() + ".jpeg";
                //Cut(imgStream, Server.MapPath(pathbase), intx1, inty1, intcw, intch, 100);
                //string publishurl = System.Configuration.ConfigurationManager.AppSettings["PublishUrl"];
                //publishurl += pathbase;

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { src = "", success = false });
            }
        }
	}
}