using Newtonsoft.Json.Linq;
using Qiniu.RS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace HanYi.Util
{
    public class CommonBLL
    {
        public static T GetJsonValue<T>(JObject json, string key)
        {
            try
            {
                dynamic d = json;
                T q = d[key].ToObject<T>();
                return q;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string message)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = Encoding.Default.GetBytes(message);
            byte[] outStr = md5.ComputeHash(result);
            string md5string = BitConverter.ToString(outStr).Replace("-", "");
            return md5string;
        }

        /// <summary>
        /// 源图片地址换成缩略图地址
        /// </summary>
        /// <param name="orginPic"></param>
        /// <returns></returns>
        public static string GetThumPic(string orginPic)
        {
            string re = orginPic;
            if (re.IndexOf(".jpg") >= 0)
            {
                re = re.Replace(".jpg", "_thum.jpg");
            }
            else if (re.IndexOf(".png") >= 0)
            {
                re = re.Replace(".png", "_thum.png");
            }
            else if (re.IndexOf(".jpeg") >= 0)
            {
                re = re.Replace(".jpeg", "_thum.jpeg");
            }
            else if (re.IndexOf(".bmp") >= 0)
            {
                re = re.Replace(".bmp", "_thum.bmp");
            }
            return re;
        }

        public enum FileType
        {
            /// <summary>
            /// NONE
            /// </summary>
            NONE = -1,
            /// <summary>
            /// word
            /// </summary>
            DOC = 0,
            /// <summary>
            /// pdf
            /// </summary>
            PDF = 1,
            /// <summary>
            /// txt
            /// </summary>
            TXT = 2,
            /// <summary>
            /// execl
            /// </summary>
            EXECL = 3,
            /// <summary>
            /// Video
            /// </summary>
            Video = 4,
            /// <summary>
            /// audio
            /// </summary>
            Audio = 5,
            /// <summary>
            /// ppt
            /// </summary>
            PPT = 6
        }

        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <param name="orginPic"></param>
        /// <returns></returns>
        public static FileType GetFileType(string filename)
        {
            FileType fileType = FileType.DOC;
            if (filename == null) return FileType.NONE;
            string re = filename.ToLower();
            if (re.IndexOf(".doc") >= 0 || re.IndexOf(".docx") >= 0)
            {
                fileType = FileType.DOC;
            }
            else if (re.IndexOf(".pdf") >= 0)
            {
                fileType = FileType.PDF;
            }
            else if (re.IndexOf(".txt") >= 0)
            {
                fileType = FileType.TXT;
            }
            else if (re.IndexOf(".excel") >= 0 || re.IndexOf(".xls") >= 0 || re.IndexOf(".xlsx") >= 0)
            {
                fileType = FileType.EXECL;
            }
            else if (re.IndexOf(".mp3") >= 0 || re.IndexOf(".wma") >= 0)
            {
                fileType = FileType.Audio;
            }
            else if (re.IndexOf(".avi") >= 0 || re.IndexOf(".mp4") >= 0|| re.IndexOf(".rmvb") >= 0 || re.IndexOf(".3gp") >= 0)
            {
                fileType = FileType.Video;
            }
            else if (re.IndexOf(".ppt") >= 0 || re.IndexOf(".pptx") >= 0)
            {
                fileType = FileType.PPT;
            }
            return fileType;
        }


        /// <summary>  
        /// 计算文件大小函数(保留两位小数),Size为字节大小  
        /// </summary>  
        /// <param name="Size">初始文件大小</param>  
        /// <returns></returns>  
        public static string CountSize(float Size)
        {
            string m_strSize = "";
            float FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = (FactSize / 1024.00).ToString("##0.#") + " KB";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("##0.#") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("##0.#") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("##0.#") + " GB";
            return m_strSize;
        }  

        /// <summary>
        /// 获取七牛上传凭证
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            //Qiniu.Conf.Config.ACCESS_KEY = "U6hGNMOIBBXCV8gvW-w8qsd8Z3IoinXCgRxN9kRc";
            //Qiniu.Conf.Config.SECRET_KEY = "HmRzauNoO2DcN-MoQH-SQrrA2F9MSsbnsoi2tNJR";
            Qiniu.Conf.Config.ACCESS_KEY = "GVcP89y9mUeRa5qcN0R9ow6nGYStTcVhtEbCeD3L";
            Qiniu.Conf.Config.SECRET_KEY = "1yp0k7hLqEFwMSwMH8LwWSpGbPROAjUyydS_0jdH";

            //设置上传的空间
            String bucket = "hanyihuadong";
            //设置上传的文件的key值
            //String key = "tedhulian";

            //普通上传,只需要设置上传的空间名就可以了,第二个参数可以设定token过期时间
            PutPolicy put = new PutPolicy(bucket, 3600);

            //调用Token()方法生成上传的Token
            string upToken = put.Token();
            return upToken;

        }

        /// <summary>
        /// 返回首页
        /// </summary>
        /// <returns></returns>
        public static void GotoError(int? id = 1, string msg = "")
        {
            string err = "对不起，该页面无法访问！";
            if (id.HasValue)
            {
                switch (id.Value)
                {
                    case 1:
                        err = "你无权访问该页面！";
                        break;
                    case 2:
                        err = "参数错误！";
                        break;
                    case 3:
                        err = "暂未开通该功能，请联系客服人员开通！";
                        break;
                    default:
                        break;
                }
            }

            HttpContext.Current.Response.Redirect(string.Format("/home/error?msg={0}", HttpContext.Current.Server.UrlEncode(!string.IsNullOrEmpty(msg) ? msg : err)), true);
            HttpContext.Current.Response.End();
        }


        #region Menu XML 操作
        /// <summary>
        /// 获取Menu XML元素
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<XElement> GetMenuXElement()
        {
            string menu = "";
            IEnumerable<XElement> elements = null;

            //可以通过增加逻辑来增加各左边的列表
            menu = AppDomain.CurrentDomain.BaseDirectory + "SysMenu\\Menu.xml";
            elements = XElement.Load(menu).Elements("leftMenu").ElementAt(0).Elements();

            return elements;
        }

        /// <summary>
        /// 根据用户类型/权限判断XML节点
        /// </summary>
        /// <param name="permissionNames"></param>
        /// <param name="cmsUserType"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> GetXElement(List<string> permissionNames, IEnumerable<XElement> elements, string nodeName, bool isInnerUser)
        {
            var xElement = (from v in elements
                            where v.Name == nodeName &&
                            (isInnerUser || CheckHasPermission(v, permissionNames))
                            select v);

            return xElement;
        }

        /// <summary>
        /// 判断菜单是否有权限
        /// </summary>
        /// <param name="element"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckHasPermission(XElement element, List<string> permissionNames)
        {
            bool flag = element.Attribute("permissionName") != null && (element.Attribute("permissionName").Value == "" || permissionNames.Contains(element.Attribute("permissionName").Value));

            if (!flag && element.HasElements)
            {
                foreach (var item in element.Elements())
                {
                    if (CheckHasPermission(item, permissionNames))
                    {
                        flag = true;
                        break;
                    }
                }
            }

            return flag;
        }



        /// <summary>
        /// 判断菜单是否选中
        /// </summary>
        /// <param name="element"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckChooseMenu(XElement element, string url)
        {
            bool flag = element.Attribute("url") != null && element.Attribute("url").Value.StartsWith(url, StringComparison.OrdinalIgnoreCase);

            if (!flag && element.HasElements)
            {
                foreach (var item in element.Elements())
                {
                    if (CheckChooseMenu(item, url))
                    {
                        flag = true;
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 判断menuindex是否为选中
        /// </summary>
        /// <param name="menuIndex">url举例：0,1,2</param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static bool CheckMenuIndex(string menuIndex, int i, int? j = null, int? k = null)
        {
            bool flag = false;

            if (!string.IsNullOrWhiteSpace(menuIndex))
            {
                flag = menuIndex.StartsWith(GetMenuIndex(i, j, k));
            }

            return flag;
        }


        /// <summary>
        /// 生成MenuIndex
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static string GetMenuIndex(int? i, int? j = null, int? k = null)
        {
            return string.Format("{0}{1}{2}",
                i.HasValue ? (i.ToString()) : "0",
                j.HasValue ? ("," + j.ToString()) : "",
                k.HasValue ? ("," + k.ToString()) : "");
        }

        #endregion

        #region 正则获取查询字符串

        /// <summary>
        /// 正则字符串
        /// </summary>
        private const string _regexSEQuery = @"(?<=(\&|\?|^)({0})\=).*?(?=\&|$)";


        /// <summary>
        /// 查找关键字
        /// <remarks>更多访问http://www.cnblogs.com/ronli/</remarks>
        /// <param name="querykey">多个QueryKey按优先顺序用'|'分隔</param>
        /// </summary>
        private static string RegexMatch(string input, string querykey, RegexOptions options)
        {
            return Regex.Match(input, string.Format(_regexSEQuery, querykey), options).Value.Trim();
        }


        /// <summary>
        /// 复合查找关键字
        /// <remarks>更多访问http://www.cnblogs.com/ronli/</remarks>
        /// <param name="querykey">多个QueryKey按优先顺序用'&'(连接)或'|'(短路)分隔</param>
        /// </summary>
        public static string RegexMatches(string input, string querykey, RegexOptions options)
        {
            string value = "";
            string[] orKeys = querykey.Split('|');
            string[] andKeys;

            foreach (string or in orKeys)
            {
                andKeys = or.Split('&');
                foreach (string and in andKeys)
                {
                    value += (RegexMatch(input, and, options) + " ");
                }
                if (value != "") break;
            }
            return value.Trim();
        }

        #endregion

        /// <summary>
        /// 将post提交的数据中的on替换为true
        /// </summary>
        /// <param name="postData">提交数据</param>
        /// <returns></returns>
        public static string ReplaceOnValueToTrue(string postData)
        {
            if (!string.IsNullOrEmpty(postData))
            {
                return postData.Replace(":\"on\"", ":\"true\"");
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到周字符串
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static string GetWeekString(DateTime dt1, DateTime dt2)
        {
            int weekOfYear = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt1.ToLocalTime(),
                System.Globalization.CalendarWeekRule.FirstFullWeek,
                DayOfWeek.Monday);

            return string.Format("{1:yyyy}年第{0}周({1:M月d日}—{2:M月d日})", weekOfYear, dt1.ToLocalTime(), dt2.ToLocalTime());
        }

        /// <summary>
        /// 得到月字符串
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static string GetMonthString(DateTime dt1, DateTime dt2)
        {
            return string.Format("{0:yyyy年M月}({0:M月d日}—{1:M月d日})", dt1.ToLocalTime(), dt2.ToLocalTime());
        }


        /// <summary>
        ///在C#后台实现JavaScript的函数escape()的字符串转换
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string escape(string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byteArr = System.Text.Encoding.Unicode.GetBytes(s);

            for (int i = 0; i < byteArr.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(byteArr[i + 1].ToString("X2"));//把字节转换为十六进制的字符串表现形式

                sb.Append(byteArr[i].ToString("X2"));
            }
            return sb.ToString();

        }

        /// <summary>
        //把JavaScript的escape()转换过去的字符串解释回来
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string unescape(string s)
        {

            string str = s.Remove(0, 2);//删除最前面两个＂%u＂
            string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
            byte[] byteArr = new byte[strArr.Length * 2];
            for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
            {
                byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16); //把十六进制形式的字串符串转换为二进制字节
                byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
            }
            str = System.Text.Encoding.Unicode.GetString(byteArr);　//把字节转为unicode编码
            return str;

        }

        #region POST

        /// <summary>
        /// 根据地址和post数据得到返回值
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postString">POST数据</param>
        /// <param name="encoding">字符串编码</param>
        /// <returns></returns>
        public static string GetReponseByURL(string url, string postString, Encoding encoding)
        {
            byte[] postData = encoding.GetBytes(postString);

            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Headers.Add("ContentLength", postData.Length.ToString());

                byte[] responseData = client.UploadData(url, "POST", postData);
                return encoding.GetString(responseData);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 根据地址和post数据得到返回值(UTF-8)
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postString">POST数据</param>
        /// <returns></returns>
        public static string GetReponseByURL(string url, string postString)
        {
            return GetReponseByURL(url, postString, Encoding.GetEncoding("utf-8"));
        }
        #endregion

        /// <summary>
        /// 转换特殊字符
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ConvertMessge(string message)
        {
            string res = "";
            Regex reg1 = new Regex("<", RegexOptions.IgnoreCase);
            Regex reg2 = new Regex(">", RegexOptions.IgnoreCase);
            Regex reg3 = new Regex("\n", RegexOptions.IgnoreCase);
            res = reg1.Replace(message, "&lt;");
            res = reg2.Replace(res, "&gt;");
            res = reg3.Replace(res, "<br/>");
            return res;
        }


        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip) || ip == "unknown") { ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
            if (string.IsNullOrEmpty(ip) || ip == "unknown") { ip = HttpContext.Current.Request.UserHostAddress; }
            if (ip.Contains(",")) { ip = ip.Split(',')[0]; }

            return ip;
        }


        #region 全角半角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 将全角、半角空格换为指定字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static string ReplaceSpace(string input, char rep)
        {
            //char[] c = input.ToCharArray();
            //for (int i = 0; i < c.Length; i++)
            //{
            //    if (c[i] == 12288 || c[i] == 32)
            //    {
            //        c[i] = rep;
            //        continue;
            //    }
            //}
            //return new string(c);
            input = Regex.Replace(input, "[\\s]+", rep.ToString());
            return input;
        }
        #endregion

    }
}