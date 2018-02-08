using Ltchina.Core.Weixin.Entity;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Ltchina.Core.Weixin.HttpUtility
{
    public static class Get
    {
        public static T GetJson<T>(string url, Encoding encoding = null)
        {
            string returnText = HttpUtility.RequestUtility.HttpGet(url, encoding);

            JavaScriptSerializer js = new JavaScriptSerializer();

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                WxJsonResult errorResult = js.Deserialize<WxJsonResult>(returnText);
                if (errorResult.errcode != ReturnCode.请求成功)
                {
                    //发生错误
                    throw new Exception(
                        string.Format("微信请求发生错误！错误代码：{0}，说明：{1}",
                                        (int)errorResult.errcode,
                                        errorResult.errmsg));
                }
            }

            T result = js.Deserialize<T>(returnText);

            return result;
        }

        public static void Download(string url, Stream stream)
        {
            WebClient wc = new WebClient();
            var data = wc.DownloadData(url);
            foreach (var b in data)
            {
                stream.WriteByte(b);
            }
        }
    }
}