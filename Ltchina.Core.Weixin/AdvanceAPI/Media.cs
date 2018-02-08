using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ltchina.Core.Weixin.Entity;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    /// <summary>
    /// 上传媒体文件返回结果
    /// </summary>
    public class UploadResultJson : WxJsonResult
    {
        public UploadMediaFileType type { get; set; }
        public string media_id { get; set; }
        public string thumb_media_id { get; set; } // 上传缩略图返回的meidia_id参数.
        public long created_at { get; set; }
    }
    /// <summary>
    /// 上传媒体文件返回结果
    /// </summary>
    public class UploadMediaFileResult : WxJsonResult
    {
        public UploadMediaFileType type { get; set; }
        public string media_id { get; set; }
        public long created_at { get; set; }
    }

    public class UploadImageResult : WxJsonResult
    {
        public string url { get; set; }
    }
    /// <summary>
    /// 公众号在使用接口时，对多媒体文件、多媒体消息的获取和调用等操作，是通过media_id来进行的。
    /// 通过本接口，公众号可以上传或下载多媒体文件。但请注意，每个多媒体文件（media_id）会在上传、
    /// 用户发送到微信服务器3天后自动删除，以节省服务器资源。
    /// </summary>
    public static class Media
    {
        /// <summary>
        /// 上传媒体文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static UploadResultJson Upload(string accessToken, UploadMediaFileType type, string file)
        {
            var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken, type.ToString());
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = file;
            return HttpUtility.Post.PostFileGetJson<UploadResultJson>(url, null, fileDictionary, null);
        }

        /// <summary>
        /// 下载媒体文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="stream"></param>
        public static void Get(string accessToken, string mediaId, Stream stream)
        {
            var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}",
                accessToken, mediaId);
            HttpUtility.Get.Download(url, stream);
        }

        /// <summary>
        /// 上传图片接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static UploadImageResult UploadImg(string accessToken, string file)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}", accessToken);
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["buffer"] = file;
            return HttpUtility.Post.PostFileGetJson<UploadImageResult>(url, null, fileDictionary, null);
        }
    }
}