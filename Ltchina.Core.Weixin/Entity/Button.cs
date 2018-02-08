using System.Collections.Generic;

namespace Ltchina.Core.Weixin.Entity
{
    public interface IBaseButton
    {
        string name { get; set; }
    }

    /// <summary>
    /// 所有按钮基类
    /// </summary>
    public class BaseButton : IBaseButton
    {
        //public ButtonType type { get; set; }
        /// <summary>
        /// 按钮描述，既按钮名字，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    /// 所有单击按钮的基类（view，click等）
    /// </summary>
    public abstract class SingleButton : BaseButton, IBaseButton
    {
        /// <summary>
        /// 按钮类型（click或view）
        /// </summary>
        public string type { get; set; }

        public SingleButton(string theType)
        {
            type = theType;
        }
    }

    /// <summary>
    /// 点击推事件按钮
    /// </summary>
    public class SingleClickButton : SingleButton
    {
        /// <summary>
        /// 类型为click时必须。
        /// 按钮KEY值，用于消息接口(event类型)推送，不超过128字节
        /// </summary>
        public string key { get; set; }

        public SingleClickButton()
            : base(ButtonType.click.ToString())
        {
        }
    }

    /// <summary>
    /// 跳转URL按钮
    /// </summary>
    public class SingleViewButton : SingleButton
    {
        /// <summary>
        /// 类型为view时必须
        /// 网页链接，用户点击按钮可打开链接，不超过256字节
        /// </summary>
        public string url { get; set; }

        public SingleViewButton()
            : base(ButtonType.view.ToString())
        {
        }
    }

    /// <summary>
    /// 弹出地理位置选择器按钮
    /// </summary>
    public class SingleLocationSelectButton : SingleButton
    {
        /// <summary>
        /// 类型为location_select时必须。
        /// 用户点击按钮后，微信客户端将调起地理位置选择工具，完成选择操作后，将选择的地理位置发送给开发者的服务器，同时收起位置选择工具，随后可能会收到开发者下发的消息。
        /// 仅支持微信iPhone5.4.1以上版本，和Android5.4以上版本的微信用户，旧版本微信用户点击后将没有回应，开发者也不能正常接收到事件推送。
        /// </summary>
        public string key { get; set; }

        public SingleLocationSelectButton()
            : base(ButtonType.location_select.ToString())
        {
        }
    }

    /// <summary>
    /// 弹出拍照或者相册发图按钮
    /// </summary>
    public class SinglePicPhotoOrAlbumButton : SingleButton
    {
        /// <summary>
        /// 类型为pic_photo_or_album时必须。
        /// 用户点击按钮后，微信客户端将弹出选择器供用户选择“拍照”或者“从手机相册选择”。用户选择后即走其他两种流程。
        /// 仅支持微信iPhone5.4.1以上版本，和Android5.4以上版本的微信用户，旧版本微信用户点击后将没有回应，开发者也不能正常接收到事件推送。
        /// </summary>
        public string key { get; set; }

        public SinglePicPhotoOrAlbumButton()
            : base(ButtonType.pic_photo_or_album.ToString())
        {
        }
    }

    /// <summary>
    /// 弹出系统拍照发图按钮
    /// </summary>
    public class SinglePicSysphotoButton : SingleButton
    {
        /// <summary>
        /// 类型为pic_sysphoto时必须。
        /// 用户点击按钮后，微信客户端将调起系统相机，完成拍照操作后，会将拍摄的相片发送给开发者，并推送事件给开发者，同时收起系统相机，随后可能会收到开发者下发的消息。
        /// 仅支持微信iPhone5.4.1以上版本，和Android5.4以上版本的微信用户，旧版本微信用户点击后将没有回应，开发者也不能正常接收到事件推送。
        /// </summary>
        public string key { get; set; }

        public SinglePicSysphotoButton()
            : base(ButtonType.pic_sysphoto.ToString())
        {
        }
    }

    /// <summary>
    /// 弹出微信相册发图器按钮
    /// </summary>
    public class SinglePicWeixinButton : SingleButton
    {
        /// <summary>
        /// 类型为pic_weixin时必须。
        /// 用户点击按钮后，微信客户端将调起微信相册，完成选择操作后，将选择的相片发送给开发者的服务器，并推送事件给开发者，同时收起相册，随后可能会收到开发者下发的消息。
        /// 仅支持微信iPhone5.4.1以上版本，和Android5.4以上版本的微信用户，旧版本微信用户点击后将没有回应，开发者也不能正常接收到事件推送。
        /// </summary>
        public string key { get; set; }

        public SinglePicWeixinButton()
            : base(ButtonType.pic_weixin.ToString())
        {
        }
    }

    /// <summary>
    /// 扫码推事件按钮
    /// </summary>
    public class SingleScancodePushButton : SingleButton
    {
        /// <summary>
        /// 类型为scancode_push时必须。
        /// 用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后显示扫描结果（如果是URL，将进入URL），且会将扫码的结果传给开发者，开发者可以下发消息。
        /// 仅支持微信iPhone5.4.1以上版本，和Android5.4以上版本的微信用户，旧版本微信用户点击后将没有回应，开发者也不能正常接收到事件推送。
        /// </summary>
        public string key { get; set; }

        public SingleScancodePushButton()
            : base(ButtonType.scancode_push.ToString())
        {
        }
    }

    /// <summary>
    /// 扫码推事件且弹出“消息接收中”提示框按钮
    /// </summary>
    public class SingleScancodeWaitmsgButton : SingleButton
    {
        /// <summary>
        /// 类型为scancode_waitmsg时必须。
        /// 用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后，将扫码的结果传给开发者，同时收起扫一扫工具，然后弹出“消息接收中”提示框，随后可能会收到开发者下发的消息。
        /// 仅支持微信iPhone5.4.1以上版本，和Android5.4以上版本的微信用户，旧版本微信用户点击后将没有回应，开发者也不能正常接收到事件推送。
        /// </summary>
        public string key { get; set; }

        public SingleScancodeWaitmsgButton()
            : base(ButtonType.scancode_waitmsg.ToString())
        {
        }
    }

    /// <summary>
    /// 整个按钮设置（可以直接用ButtonGroup实例返回JSON对象）
    /// </summary>
    public class ButtonGroup
    {
        /// <summary>
        /// 按钮数组，按钮个数应为2~3个
        /// </summary>
        public List<BaseButton> button { get; set; }

        public ButtonGroup()
        {
            button = new List<BaseButton>();
        }
    }

    /// <summary>
    /// 子菜单
    /// </summary>
    public class SubButton : BaseButton, IBaseButton
    {
        /// <summary>
        /// 子按钮数组，按钮个数应为2~5个
        /// </summary>
        public List<SingleButton> sub_button { get; set; }

        public SubButton()
        {
            sub_button = new List<SingleButton>();
        }

        public SubButton(string name)
            : this()
        {
            base.name = name;
        }
    }
}