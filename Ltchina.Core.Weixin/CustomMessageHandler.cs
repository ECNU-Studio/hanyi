using Ltchina.Core.Weixin.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ltchina.Core.Weixin
{
    public class DemoCustomMessageHandler : MessageHandler
    {
        public DemoCustomMessageHandler(Stream inputStream, PostModel postModel = null)
            : base(inputStream, postModel)
        {
        }

        /// <summary>
        /// 处理文本请求（(以返回发送文本消息示例)）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            var result = new StringBuilder();
            result.AppendFormat("您刚才发送了文字信息：{0}\r\n\r\n", requestMessage.Content);
            result.AppendLine("您还可以发送【位置】【图片】【语音】【视频】等类型的信息（注意是这几种类型，不是这几个文字），查看不同格式的回复。");
            responseMessage.Content = result.ToString();
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求(以发送图文消息示例)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = "您刚才发送了图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = requestMessage.PicUrl,
                Url = "http://www.baidu.com"
            });
            responseMessage.Articles.Add(new Article()
            {
                Title = "第二条",
                Description = "第二条带连接的内容",
                PicUrl = requestMessage.PicUrl,
                Url = "http://www.qq.com"
            });
            return responseMessage;
        }

        /// <summary>
        /// 处理语音请求(以发送音乐消息示例)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            responseMessage.Music.Title = "这里是一条音乐消息";
            responseMessage.Music.Description = "来自Jeffrey Su的美妙歌声~~";
            responseMessage.Music.ThumbMediaId = "CIX0pyM_WcToQCC5wUUMrev3k4y0bePOni5omEb1v-C5ckfGZFi1u-qL2UmigXtr";
            return responseMessage;
        }

        /// <summary>
        /// 处理视频请求(以返回文本信息为例)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了一条视频信息，ID：" + requestMessage.MediaId;
            return responseMessage;
        }

        // <summary>
        /// 处理位置请求(以发送图文消息示例)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了一条地理位置信息消息，X：" + requestMessage.Location_X;
            return responseMessage;
        }

        /// <summary>
        /// 处理链接消息请求(以返回文本信息为例)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            //            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            //            responseMessage.Content = string.Format(@"您发送了一条连接信息：
            //            Title：{0}
            //            Description:{1}
            //            Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            //            return responseMessage;
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageImage>(requestMessage);
            responseMessage.Image = new Image { MediaId = "CIX0pyM_WcToQCC5wUUMrev3k4y0bePOni5omEb1v-C5ckfGZFi1u-qL2UmigXtr" };
            return responseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */

            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = string.Format("您好{0},欢迎关注码农之心", requestMessage.FromUserName);
            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }

        /// <summary>
        /// 二维码扫描（回复文本消息）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            //通过扫描关注
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            if (requestMessage.EventKey.Contains("qrscene_"))
            {
                responseMessage.Content = string.Format("您已经关注了我们");
            }
            else
            {
                responseMessage.Content = string.Format("通过扫描关注。EventKey:{0},Ticket:{1}", requestMessage.EventKey, requestMessage.Ticket);
            }
            return responseMessage;
        }

        /// <summary>
        /// 上报地理位置
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("您当前的纬度：{0}，经度：{1},地图精度：{2}", requestMessage.Latitude, requestMessage.Longitude, requestMessage.Precision);//生产应用时可记录到数据库
            return responseMessage;//这里也可以返回null（需要注意写日志时候null的问题）
        }

        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
                default:
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                    break;
            }

            return reponseMessage;
        }

        /// <summary>
        /// 菜单之页面跳转
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
            return responseMessage;
        }

        /// <summary>
        /// 事件之扫码推事件(scancode_push)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("事件之扫码推事件,扫描结果为：{0}", requestMessage.ScanCodeInfo.ScanResult);
            return responseMessage;
        }

        /// <summary>
        /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("事件之扫码推事件且弹出“消息接收中”提示框,扫描结果：{0}", requestMessage.ScanCodeInfo.ScanResult);
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出拍照或者相册发图（pic_photo_or_album）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("事件之弹出拍照或者相册发图,发送图片数量为：{0}", requestMessage.SendPicsInfo.Count);
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出系统拍照发图(pic_sysphoto)
        /// 实际测试时发现微信并没有推送RequestMessageEvent_Pic_Sysphoto消息，只能接收到用户在微信中发送的图片消息。
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicSysphotoRequest(RequestMessageEvent_Pic_Sysphoto requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("事件之弹出系统拍照发图,发送图片数量为：{0}", requestMessage.SendPicsInfo.Count);
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出微信相册发图器(pic_weixin)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicWeixinRequest(RequestMessageEvent_Pic_Weixin requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("事件之弹出微信相册发图器,发送图片数量为：{0}", requestMessage.SendPicsInfo.Count);
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出地理位置选择器（location_select）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("事件之弹出地理位置选择器,位置为{0}", requestMessage.SendLocationInfo.Label);
            return responseMessage;
        }

        /// <summary>
        /// 事件之推送群发结果
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_MassSendJobFinishRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "接收到了群发完成的信息。";
            return responseMessage;
        }
    }
}
