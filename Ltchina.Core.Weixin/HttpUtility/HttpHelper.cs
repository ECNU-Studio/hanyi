using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ltchina.Core.Weixin.HttpUtility
{
    public class HttpHelper
    {
        /// <summary>
        /// Http POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static T post<T>(string url,string jsonBody)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                byte[] postData = Encoding.UTF8.GetBytes(jsonBody);
                byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
                string srcString = Encoding.UTF8.GetString(responseData);
                return JsonConvert.DeserializeObject<T>(srcString);
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
