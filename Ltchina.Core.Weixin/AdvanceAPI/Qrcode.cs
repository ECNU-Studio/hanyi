using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.CommonAPI;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    public static class Qrcode
    {
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="expireSeconds">该二维码有效时间，以秒为单位。 最大不超过1800。0时为永久二维码</param>
        /// <param name="sceneId">场景值ID，临时二维码时为32位整型，永久二维码时最大值为1000</param>
        /// <returns></returns>
        public static CreateQrCodeResult Create(string accessToken, int expireSeconds, int sceneId)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            object data = null;
            if (expireSeconds > 0)
            {
                data = new
                {
                    expire_seconds = expireSeconds,
                    action_name = "QR_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = sceneId
                        }
                    }
                };
            }
            else
            {
                data = new
                {
                    action_name = "QR_LIMIT_SCENE",
                    action_info = new
                    {
                        scene = new
                        {
                            scene_id = sceneId
                        }
                    }
                };
            }
            return CommonJsonSend.Send<CreateQrCodeResult>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 获取下载二维码的地址
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static string GetShowQrCodeUrl(string ticket)
        {
            var urlFormat = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
            return string.Format(urlFormat, ticket);
        }

        /// <summary>
        /// 获取二维码（不需要AccessToken）
        /// 错误情况下（如ticket非法）返回HTTP错误码404。
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="stream"></param>
        public static void ShowQrCode(string ticket, Stream stream)
        {
            var url = GetShowQrCodeUrl(ticket);
            HttpUtility.Get.Download(url, stream);
        }
    }

    /// <summary>
    /// 二维码创建返回结果
    /// </summary>
    public class CreateQrCodeResult : WxJsonResult
    {
        public string ticket { get; set; }
        public int expire_seconds { get; set; }

        public string url { get; set; }
    }
}