using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.Helper;
using System;
using System.Xml.Linq;

namespace Ltchina.Core.Weixin
{
    public static class RequestMessageFactory
    {
        /// <summary>
        /// 获取请求实体属性
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IRequestMessageBase GetRequestEntity(XDocument doc)
        {
            RequestMessageBase requestMessage = null;
            RequestMsgType msgType;

            try
            {
                msgType = MsgTypeHelper.GetRequestMsgType(doc);
                switch (msgType)
                {
                    case RequestMsgType.Text:
                        requestMessage = new RequestMessageText();
                        break;
                    case RequestMsgType.Image:
                        requestMessage = new RequestMessageImage();
                        break;
                    case RequestMsgType.Voice:
                        requestMessage = new RequestMessageVoice();
                        break;
                    case RequestMsgType.Video:
                        requestMessage = new RequestMessageVideo();
                        break;
                    case RequestMsgType.Location:
                        requestMessage = new RequestMessageLocation();
                        break;
                    case RequestMsgType.Link:
                        requestMessage = new RequestMessageLink();
                        break;
                    case RequestMsgType.Event:
                        //判断Event类型
                        switch (doc.Root.Element("Event").Value.ToUpper())
                        {
                            case "SUBSCRIBE"://订阅（关注）
                                requestMessage = new RequestMessageEvent_Subscribe();
                                break;
                            case "UNSUBSCRIBE"://取消订阅（关注）
                                requestMessage = new RequestMessageEvent_Unsubscribe();
                                break;
                            case "SCAN"://二维码扫描
                                requestMessage = new RequestMessageEvent_Scan();
                                break;
                            case "LOCATION"://上报地理位置
                                requestMessage = new RequestMessageEvent_Location();
                                break;
                            case "CLICK"://菜单点击
                                requestMessage = new RequestMessageEvent_Click();
                                break;
                            case "VIEW"://菜单URL跳转
                                requestMessage = new RequestMessageEvent_View();
                                break;
                            case "SCANCODE_PUSH"://扫码推事件(scancode_push)
                                requestMessage = new RequestMessageEvent_Scancode_Push();
                                break;
                            case "SCANCODE_WAITMSG"://扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
                                requestMessage = new RequestMessageEvent_Scancode_Waitmsg();
                                break;
                            case "PIC_SYSPHOTO"://弹出系统拍照发图(pic_sysphoto)
                                requestMessage = new RequestMessageEvent_Pic_Sysphoto();
                                break;
                            case "PIC_PHOTO_OR_ALBUM"://弹出拍照或者相册发图（pic_photo_or_album）
                                requestMessage = new RequestMessageEvent_Pic_Photo_Or_Album();
                                break;
                            case "PIC_WEIXIN"://弹出微信相册发图器(pic_weixin)
                                requestMessage = new RequestMessageEvent_Pic_Weixin();
                                break;
                            case "LOCATION_SELECT"://弹出地理位置选择器（location_select）
                                requestMessage = new RequestMessageEvent_Location_Select();
                                break;
                            case "MASSSENDJOBFINISH":
                                requestMessage = new RequestMessageEvent_MassSendJobFinish();
                                break;
                            case "TEMPLATESENDJOBFINISH":
                                requestMessage = new RequestMessageEvent_TemplateSendJobFinish();
                                break;
                            default://其他意外类型（也可以选择抛出异常）
                                requestMessage = new RequestMessageEventBase();
                                break;
                        }
                        break;
                    default:
                        throw new Exception(string.Format("MsgType：{0} 在RequestMessageFactory中没有对应的处理程序！", msgType), new ArgumentOutOfRangeException());//为了能够对类型变动最大程度容错（如微信目前还可以对公众账号suscribe等未知类型，但API没有开放），建议在使用的时候catch这个异常
                }
                EntityHelper.FillEntityWithXml(requestMessage, doc);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(string.Format("RequestMessage转换出错！可能是MsgType不存在！，XML：{0}", doc.ToString()), ex);
            }
            return requestMessage;
        }

    }
}