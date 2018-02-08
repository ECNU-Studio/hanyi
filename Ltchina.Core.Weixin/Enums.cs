using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ltchina.Core.Weixin
{
    /// <summary>
    /// 当RequestMsgType类型为Event时，Event属性的类型
    /// </summary>
    public enum Event
    {
        /// <summary>
        /// 订阅
        /// </summary>
        subscribe,

        /// <summary>
        /// 取消订阅
        /// </summary>
        unsubscribe,

        /// <summary>
        /// 二维码扫描
        /// </summary>
        scan,
        /// <summary>
        /// 上报地理位置
        /// </summary>
        LOCATION,
        /// <summary>
        /// 自定义菜单点击事件
        /// </summary>
        CLICK,
        /// <summary>
        /// URL跳转
        /// </summary>
        VIEW,
        /// <summary>
        /// 扫码推事件
        /// </summary>
        scancode_push,

        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        scancode_waitmsg,

        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        pic_sysphoto,

        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        pic_photo_or_album,

        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        pic_weixin,

        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        location_select,
        /// <summary>
        /// 事件推送群发结果
        /// </summary>
        MASSSENDJOBFINISH,
        /// <summary>
        /// 事件推送模板消息结果
        /// </summary>
        TEMPLATESENDJOBFINISH,
    }

    /// <summary>
    /// 接收消息类型
    /// </summary>
    public enum RequestMsgType
    {
        Text, //文本
        Location, //地理位置
        Image, //图片
        Voice, //语音
        Video, //视频
        Link, //连接信息
        Event, //事件推送
    }

    /// <summary>
    /// 发送消息类型
    /// </summary>
    public enum ResponseMsgType
    {
        Text,//文本
        News,//图文
        Music,//音乐
        Image,//图片
        Voice,//语音
        Video,//视频
        Transfer_Customer_Service,
        //transfer_customer_service
    }

    /// <summary>
    /// 语言
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// 中文简体
        /// </summary>
        zh_CN,
        /// <summary>
        /// 中文繁体
        /// </summary>
        zh_TW,
        /// <summary>
        /// 英文
        /// </summary>
        en
    }

    /// <summary>
    /// 菜单按钮类型
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 点击
        /// </summary>
        click,
        /// <summary>
        /// Url
        /// </summary>
        view,
        /// <summary>
        /// 扫码推事件
        /// </summary>
        scancode_push,
        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        scancode_waitmsg,
        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        pic_sysphoto,
        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        pic_photo_or_album,
        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        pic_weixin,
        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        location_select
    }

    /// <summary>
    /// 上传媒体文件类型
    /// </summary>
    public enum UploadMediaFileType
    {
        /// <summary>
        /// 图片: 1M，支持JPG格式
        /// </summary>
        image,
        /// <summary>
        /// 语音：2M，播放长度不超过60s，支持AMR\MP3格式
        /// </summary>
        voice,
        /// <summary>
        /// 视频：10MB，支持MP4格式
        /// </summary>
        video,
        /// <summary>
        /// thumb：64KB，支持JPG格式
        /// </summary>
        thumb,
        /// <summary>
        /// 图文消息
        /// </summary>
        news
    }

    public enum GroupMessageType
    {
        /// <summary>
        /// 图文消息
        /// </summary>
        mpnews = 0,
        /// <summary>
        /// 文本
        /// </summary>
        text = 1,
        /// <summary>
        /// 语音
        /// </summary>
        voice = 2,
        /// <summary>
        /// 图片
        /// </summary>
        image = 3,
        /// <summary>
        /// 视频
        /// </summary>
        video = 4
    }

    /// <summary>
    /// 返回码（JSON）
    /// </summary>
    public enum ReturnCode
    {
        系统繁忙 = -1,
        请求成功 = 0,
        验证失败 = 40001,
        不合法的凭证类型 = 40002,
        不合法的OpenID = 40003,
        不合法的媒体文件类型 = 40004,
        不合法的文件类型 = 40005,
        不合法的文件大小 = 40006,
        不合法的媒体文件id = 40007,
        不合法的消息类型 = 40008,
        不合法的图片文件大小 = 40009,
        不合法的语音文件大小 = 40010,
        不合法的视频文件大小 = 40011,
        不合法的缩略图文件大小 = 40012,
        不合法的APPID = 40013,
        不合法的access_token = 40014,
        不合法的菜单类型 = 40015,
        不合法的按钮个数1 = 40016,
        不合法的按钮个数2 = 40017,
        不合法的按钮名字长度 = 40018,
        不合法的按钮KEY长度 = 40019,
        不合法的按钮URL长度 = 40020,
        不合法的菜单版本号 = 40021,
        不合法的子菜单级数 = 40022,
        不合法的子菜单按钮个数 = 40023,
        不合法的子菜单按钮类型 = 40024,
        不合法的子菜单按钮名字长度 = 40025,
        不合法的子菜单按钮KEY长度 = 40026,
        不合法的子菜单按钮URL长度 = 40027,
        不合法的自定义菜单使用用户 = 40028,
        不合法的oauth_code = 40029,
        不合法的refresh_token = 40030,
        缺少access_token参数 = 41001,
        缺少appid参数 = 41002,
        缺少refresh_token参数 = 41003,
        缺少secret参数 = 41004,
        缺少多媒体文件数据 = 41005,
        缺少media_id参数 = 41006,
        缺少子菜单数据 = 41007,
        access_token超时 = 42001,
        需要GET请求 = 43001,
        需要POST请求 = 43002,
        需要HTTPS请求 = 43003,
        多媒体文件为空 = 44001,
        POST的数据包为空 = 44002,
        图文消息内容为空 = 44003,
        多媒体文件大小超过限制 = 45001,
        消息内容超过限制 = 45002,
        标题字段超过限制 = 45003,
        描述字段超过限制 = 45004,
        链接字段超过限制 = 45005,
        图片链接字段超过限制 = 45006,
        语音播放时间超过限制 = 45007,
        图文消息超过限制 = 45008,
        接口调用超过限制 = 45009,
        创建菜单个数超过限制 = 45010,
        不存在媒体数据 = 46001,
        不存在的菜单版本 = 46002,
        不存在的菜单数据 = 46003,
        解析JSON_XML内容错误 = 47001,
        api功能未授权 = 48001,
        用户未授权该api = 50001,
    }

    public enum IndustryCode
    {
        IT科技_互联网_电子商务 = 1,
        IT科技_IT软件与服务 = 2,
        IT科技_IT硬件与设备 = 3,
        IT科技_电子技术 = 4,
        IT科技_通信与运营商 = 5,
        IT科技_网络游戏 = 6,
        金融业_银行 = 7,
        金融业_基金_理财_信托 = 8,
        金融业_保险 = 9,
        餐饮_餐饮 = 10,
        酒店旅游_酒店 = 11,
        酒店旅游_旅游 = 12,
        运输与仓储_快递 = 13,
        运输与仓储_物流 = 14,
        运输与仓储_仓储 = 15,
        教育_培训 = 16,
        教育_院校 = 17,
        政府与公共事业_学术科研 = 18,
        政府与公共事业_交警 = 19,
        政府与公共事业_博物馆 = 20,
        政府与公共事业_公共事业_非盈利机构 = 21,
        医药护理_医药医疗 = 22,
        医药护理_护理美容 = 23,
        医药护理_保健与卫生 = 24,
        交通工具_汽车相关 = 25,
        交通工具_摩托车相关 = 26,
        交通工具_火车相关 = 27,
        交通工具_飞机相关 = 28,
        房地产_建筑 = 29,
        房地产_物业 = 30,
        消费品_消费品 = 31,
        商业服务_法律 = 32,
        商业服务_会展 = 33,
        商业服务_中介服务 = 34,
        商业服务_认证 = 35,
        商业服务_审计 = 36,
        文体娱乐_传媒 = 37,
        文体娱乐_体育 = 38,
        文体娱乐_娱乐休闲 = 39,
        印刷_印刷 = 40,
        其它_其它 = 41,
    }
}
