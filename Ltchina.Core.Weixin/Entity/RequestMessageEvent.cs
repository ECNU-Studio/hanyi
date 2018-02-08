
namespace Ltchina.Core.Weixin.Entity
{
    /// <summary>
    /// 事件之订阅
    /// </summary>
    public class RequestMessageEvent_Subscribe : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.subscribe; }
        }

        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值（如果不是扫描场景二维码，此参数为空）
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片（如果不是扫描场景二维码，此参数为空）
        /// </summary>
        public string Ticket { get; set; }
    }

    /// <summary>
    /// 事件之取消订阅
    /// </summary>
    public class RequestMessageEvent_Unsubscribe : RequestMessageEventBase, IRequestMessageEventBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.unsubscribe; }
        }
    }

    /// <summary>
    /// 事件之二维码扫描（关注微信）
    /// </summary>
    public class RequestMessageEvent_Scan : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.scan; }
        }

        public string Ticket { get; set; }

        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }
    }

    /// <summary>
    /// 获取用户地理位置（高级接口下才能用）
    /// 获取用户地理位置的方式有两种，一种是仅在进入会话时上报一次，一种是进入会话后每隔5秒上报一次。公众号可以在公众平台网站中设置。
    /// 用户同意上报地理位置后，每次进入公众号会话时，都会在进入时上报地理位置，或在进入会话后每5秒上报一次地理位置，上报地理位置以推送XML数据包到开发者填写的URL来实现。
    /// </summary>
    public class RequestMessageEvent_Location : RequestMessageEventBase, IRequestMessageEventBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.LOCATION; }
        }

        /// <summary>
        /// 地理位置维度，事件类型为LOCATION的时存在
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 地理位置经度，事件类型为LOCATION的时存在
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 地理位置精度，事件类型为LOCATION的时存在
        /// </summary>
        public double Precision { get; set; }
    }

    /// <summary>
    /// 事件之菜单点击
    /// </summary>
    public class RequestMessageEvent_Click : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.CLICK; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
    }

    /// <summary>
    /// 事件之URL跳转视图（View）
    /// </summary>
    public class RequestMessageEvent_View : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.VIEW; }
        }

        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }
    }

    /// <summary>
    /// 事件之扫码推事件(scancode_push)
    /// </summary>
    public class RequestMessageEvent_Scancode_Push : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.scancode_push; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 扫描信息
        /// </summary>
        public ScanCodeInfo ScanCodeInfo { get; set; }
    }

    // <summary>
    /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
    /// </summary>
    public class RequestMessageEvent_Scancode_Waitmsg : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.scancode_waitmsg; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 扫描信息
        /// </summary>
        public ScanCodeInfo ScanCodeInfo { get; set; }
    }

    /// <summary>
    /// 事件之弹出拍照或者相册发图（pic_photo_or_album）
    /// </summary>
    public class RequestMessageEvent_Pic_Photo_Or_Album : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.pic_photo_or_album; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 发送的图片信息
        /// </summary>
        public SendPicsInfo SendPicsInfo { get; set; }
    }

    /// <summary>
    /// 事件之弹出系统拍照发图(pic_sysphoto)
    /// </summary>
    public class RequestMessageEvent_Pic_Sysphoto : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.pic_sysphoto; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 发送的图片信息
        /// </summary>
        public SendPicsInfo SendPicsInfo { get; set; }
    }

    /// <summary>
    /// 事件之弹出微信相册发图器(pic_weixin)
    /// </summary>
    public class RequestMessageEvent_Pic_Weixin : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.pic_weixin; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 发送的图片信息
        /// </summary>
        public SendPicsInfo SendPicsInfo { get; set; }
    }

    /// <summary>
    /// 事件之弹出地理位置选择器（location_select）
    /// </summary>
    public class RequestMessageEvent_Location_Select : RequestMessageEventBase, IRequestMessageEventBase, IRequestMessageEventKey
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.location_select; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 发送的位置信息
        /// </summary>
        public SendLocationInfo SendLocationInfo { get; set; }
    }

    /// <summary>
    /// 事件推送群发结果。
    /// 
    /// 由于群发任务提交后，群发任务可能在一定时间后才完成，因此，群发接口调用时，仅会给出群发任务是否提交成功的提示，若群发任务提交成功，则在群发任务结束时，会向开发者在公众平台填写的开发者URL（callback URL）推送事件。
    /// </summary>
    public class RequestMessageEvent_MassSendJobFinish : RequestMessageEventBase, IRequestMessageEventBase
    {
        public override Event Event
        {
            get { return Event.MASSSENDJOBFINISH; }
        }

        /// <summary>
        /// 群发的结构，为“send success”或“send fail”或“err(num)”。当send success时，也有可能因用户拒收公众号的消息、系统错误等原因造成少量用户接收失败。err(num)是审核失败的具体原因，可能的情况如下：
        /// err(10001), //涉嫌广告 err(20001), //涉嫌政治 err(20004), //涉嫌社会 err(20002), //涉嫌色情 err(20006), //涉嫌违法犯罪 err(20008), //涉嫌欺诈 err(20013), //涉嫌版权 err(22000), //涉嫌互推(互相宣传) err(21000), //涉嫌其他
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// group_id下粉丝数；或者openid_list中的粉丝数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 过滤（过滤是指，有些用户在微信设置不接收该公众号的消息）后，准备发送的粉丝数，原则上，FilterCount = SentCount + ErrorCount
        /// </summary>
        public int FilterCount { get; set; }

        /// <summary>
        /// 发送成功的粉丝数
        /// </summary>
        public int SentCount { get; set; }

        /// <summary>
        /// 发送失败的粉丝数
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 群发消息id，注意ID大小写，与接收消息不同
        /// </summary>
        public long MsgID { get; set; }
    }

    /// <summary>
    /// 事件推送模板消息发送结果。
    /// 在模版消息发送任务完成后，微信服务器会将是否送达成功作为通知，发送到开发者中心中填写的服务器配置地址中。
    /// </summary>
    public class RequestMessageEvent_TemplateSendJobFinish : RequestMessageEventBase, IRequestMessageEventBase
    {
        public override Event Event
        {
            get { return Event.TEMPLATESENDJOBFINISH; }
        }

        /// <summary>
        /// 群发的结构，为“success”（送达成功）或“failed:user block”（送达由于用户拒收（用户设置拒绝接收公众号消息））或“failed: system failed”（送达由于其他原因失败）。
        /// </summary>
        public string Status { get; set; }
    }
}