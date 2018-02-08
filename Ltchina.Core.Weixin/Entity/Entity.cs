using System.Collections.Generic;

namespace Ltchina.Core.Weixin.Entity
{
    public class Image
    {
        public string MediaId { get; set; }
    }

    public class Voice
    {
        public string MediaId { get; set; }
    }

    public class Video
    {
        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 扫码事件中的ScanCodeInfo
    /// </summary>
    public class ScanCodeInfo
    {
        public string ScanType { get; set; }
        public string ScanResult { get; set; }
    }

    /// <summary>
    /// 系统拍照发图中的SendPicsInfo
    /// </summary>
    public class SendPicsInfo
    {
        /// <summary>
        /// 发送的图片数量
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 图片列表
        /// </summary>
        public List<PicItem> PicList { get; set; }
    }

    public class PicItem
    {
        public Md5Sum item { get; set; }
    }

    public class Md5Sum
    {
        /// <summary>
        /// 图片的MD5值，开发者若需要，可用于验证接收到图片
        /// </summary>
        public string PicMd5Sum { get; set; }
    }

    /// <summary>
    /// 弹出地理位置选择器的事件推送中的SendLocationInfo
    /// </summary>
    public class SendLocationInfo
    {
        /// <summary>
        /// X坐标信息
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// Y坐标信息
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 精度，可理解为精度或者比例尺、越精细的话 scale越高
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置的字符串信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 朋友圈POI的名字，可能为空
        /// </summary>
        public string Poiname { get; set; }
    }

    /// <summary>
    /// 图文消息模型
    /// </summary>
    public class NewsModel
    {
        /// <summary>
        /// 图文消息缩略图的media_id，可以在基础支持上传多媒体文件接口中获得
        /// </summary>
        public string thumb_media_id { get; set; }

        /// <summary>
        /// 图文消息的作者
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 在图文消息页面点击“阅读原文”后的页面
        /// </summary>
        public string content_source_url { get; set; }

        /// <summary>
        /// 图文消息页面的内容，支持HTML标签
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 图文消息的描述
        /// </summary>
        public string digest { get; set; }

        /// <summary>
        /// 是否显示封面，1为显示，0为不显示
        /// </summary>
        public string show_cover_pic { get; set; }
    }
}