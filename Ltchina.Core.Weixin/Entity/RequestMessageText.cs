
namespace Ltchina.Core.Weixin.Entity
{
    /// <summary>
    /// 文本请求实体
    /// </summary>
    public class RequestMessageText : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Text; }
        }
        public string Content { get; set; }
    }

    /// <summary>
    /// 图片请求实体
    /// </summary>
    public class RequestMessageImage : RequestMessageBase, IRequestMessageBase 
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Image; }
        }

        public string MediaId { get; set; }
        public string PicUrl { get; set; }
    }
    /// <summary>
    /// 语音请求实体
    /// </summary>
    public class RequestMessageVoice : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Voice; }
        }

        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音格式：amr
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 语音识别结果，UTF8编码
        /// 开通语音识别功能，用户每次发送语音给公众号时，微信会在推送的语音消息XML数据包中，增加一个Recongnition字段。
        /// 注：由于客户端缓存，开发者开启或者关闭语音识别功能，对新关注者立刻生效，对已关注用户需要24小时生效。开发者可以重新关注此帐号进行测试。
        /// </summary>
        public string Recognition { get; set; }
    }
    /// <summary>
    /// 视频消息请求实体
    /// </summary>
    public class RequestMessageVideo : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Video; }
        }

        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }
    }

    /// <summary>
    /// 地理位置请求实体
    /// </summary>
    public class RequestMessageLocation : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Location; }
        }

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public double Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y { get; set; }
        public int Scale { get; set; }
        public string Label { get; set; }
    }

    /// <summary>
    /// 链接消息请求实体
    /// </summary>
    public class RequestMessageLink : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Link; }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}