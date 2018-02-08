using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.Helper;
using Ltchina.Core.Weixin.Tencent;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Ltchina.Core.Weixin
{
    public abstract class MessageHandler
    {
        /// <summary>
        /// 用户微信id
        /// </summary>
        public string WeixinOpenId { get; private set; }

        /// <summary>
        /// 原始请求xml
        /// </summary>
        public XDocument RequestDocument { get; set; }

        /// <summary>
        /// 原始的加密请求（如果不加密则为null）
        /// </summary>
        public XDocument EcryptRequestDocument { get; set; }

        public virtual IRequestMessageBase RequestMessage { get; set; }

        public virtual IResponseMessageBase ResponseMessage { get; set; }

        /// <summary>
        /// 是否使用了加密信息
        /// </summary>
        public bool UsingEcryptMessage { get; set; }

        //是否取消执行
        public bool CancelExcute { get; set; }

        private PostModel _postModel;

        public  XDocument ResponseDocument
        {
            get
            {
                if (ResponseMessage == null)
                {
                    return null;
                }
                return EntityHelper.ConvertEntityToXml(ResponseMessage as ResponseMessageBase);
            }
        }

        /// <summary>
        /// 最后返回的ResponseDocument。
        /// 这里是Senparc.Weixin.MP，根据请求消息半段在ResponseDocument基础上是否需要再次进行加密（每次获取重新加密，所以结果会不同）
        /// </summary>
        public  XDocument FinalResponseDocument
        {
            get
            {
                if (ResponseDocument == null)
                {
                    return null;
                }

                if (!UsingEcryptMessage)
                {
                    return ResponseDocument;
                }

                var timeStamp = DateTime.Now.Ticks.ToString();
                var nonce = DateTime.Now.Ticks.ToString();

                WXBizMsgCrypt msgCrype = new WXBizMsgCrypt(_postModel.Token, _postModel.EncodingAESKey, _postModel.AppId);
                string finalResponseXml = null;
                msgCrype.EncryptMsg(ResponseDocument.ToString(), timeStamp, nonce, ref finalResponseXml);//TODO:这里官方的方法已经把EncryptResponseMessage对应的XML输出出来了

                return XDocument.Parse(finalResponseXml);
            }
        }

        /// <summary>
        /// 是否使用了兼容模式加密信息
        /// </summary>
        public bool UsingCompatibilityModelEcryptMessage { get; set; }

        public MessageHandler(Stream inputStream,PostModel postModel = null)
        {
            inputStream.Seek(0, SeekOrigin.Begin);//强制调整指针位置
            using (XmlReader xr = XmlReader.Create(inputStream))
            {
                RequestDocument = XDocument.Load(xr);
            }
            //LogHelper.Info(RequestDocument.ToString());
            //RequestMessage = RequestMessageFactory.GetRequestEntity(RequestDocument);
            Init(RequestDocument, postModel);
        }


        /// <summary>
        /// 处理加密通信
        /// </summary>
        /// <param name="postDataDocument"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public  XDocument Init(XDocument postDataDocument, PostModel postModel = null)
        {
            //进行加密判断并处理
            _postModel = postModel;
            var postDataStr = postDataDocument.ToString();

            XDocument decryptDoc = postDataDocument;

            if (_postModel != null && postDataDocument.Root.Element("Encrypt") != null && !string.IsNullOrEmpty(postDataDocument.Root.Element("Encrypt").Value))
            {
                //使用了加密
                UsingEcryptMessage = true;
                EcryptRequestDocument = postDataDocument;

                WXBizMsgCrypt msgCrype = new WXBizMsgCrypt(_postModel.Token, _postModel.EncodingAESKey, _postModel.AppId);
                string msgXml = null;
                var result = msgCrype.DecryptMsg(_postModel.Msg_Signature, _postModel.Timestamp, _postModel.Nonce, postDataStr, ref msgXml);

                //判断result类型
                if (result != 0)
                {
                    //验证没有通过，取消执行
                    CancelExcute = true;
                    return null;
                }

                if (postDataDocument.Root.Element("FromUserName") != null && !string.IsNullOrEmpty(postDataDocument.Root.Element("FromUserName").Value))
                {
                    //TODO：使用了兼容模式，进行验证即可
                    UsingCompatibilityModelEcryptMessage = true;
                }

                decryptDoc = XDocument.Parse(msgXml);//完成解密
            }

            RequestMessage = RequestMessageFactory.GetRequestEntity(decryptDoc);
            if (UsingEcryptMessage)
            {
                RequestMessage.Encrypt = postDataDocument.Root.Element("Encrypt").Value;
            }

            return decryptDoc;
        }

        /// <summary>
        /// 文字类型请求
        /// </summary>
        public virtual IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 图片类型请求
        /// </summary>
        public virtual IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 语音类型请求
        /// </summary>
        public virtual IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 视频类型请求
        /// </summary>
        public virtual IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 位置类型请求
        /// </summary>
        public virtual IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 位置类型请求
        /// </summary>
        public virtual IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        public abstract IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage);

        public virtual IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            return null;
        }

        // <summary>
        /// 根据当前的RequestMessage创建指定类型的ResponseMessage
        /// </summary>
        /// <typeparam name="TR">基于ResponseMessageBase的响应消息类型</typeparam>
        /// <returns></returns>
        public TR CreateResponseMessage<TR>() where TR : ResponseMessageBase
        {
            if (RequestMessage == null)
            {
                return null;
            }

            return RequestMessage.CreateResponseMessage<TR>();
        }

        /// <summary>
        /// 执行微信请求
        /// </summary>
        public virtual void Execute()
        {
            if (CancelExcute)
            {
                return;
            }

            try
            {
                if (RequestMessage == null)
                {
                    return;
                }

                switch (RequestMessage.MsgType)
                {
                    case RequestMsgType.Text:
                        {
                            var requestMessage = RequestMessage as RequestMessageText;
                            ResponseMessage = OnTextOrEventRequest(requestMessage) ?? OnTextRequest(requestMessage);
                        }
                        break;
                    case RequestMsgType.Image:
                        {
                            var requestMessage = RequestMessage as RequestMessageImage;
                            ResponseMessage = OnImageRequest(requestMessage);
                        }
                        break;
                    case RequestMsgType.Voice: 
                        {
                            var requestMessage = RequestMessage as RequestMessageVoice;
                            ResponseMessage = OnVoiceRequest(requestMessage);
                        }
                        break;
                    case RequestMsgType.Video:
                        {
                            var requestMessage = RequestMessage as RequestMessageVideo;
                            ResponseMessage = OnVideoRequest(requestMessage);
                        }
                        break;
                    case RequestMsgType.Location: 
                        {
                            var requestMessage = RequestMessage as RequestMessageLocation;
                            ResponseMessage = OnLocationRequest(requestMessage);
                        }
                        break;
                    case RequestMsgType.Link:
                        {
                            var requestMessage = RequestMessage as RequestMessageLink;
                            ResponseMessage = OnLinkRequest(requestMessage);
                        }
                        break;
                    case RequestMsgType.Event:
                        {
                            var requestMessageText = (RequestMessage as IRequestMessageEventBase).ConvertToRequestMessageText();
                            ResponseMessage = OnTextOrEventRequest(requestMessageText) ?? OnEventRequest(RequestMessage as IRequestMessageEventBase);
                        }
                        break;
                    default:
                        throw new Exception("未知的MsgType请求类型");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MessageHandler中Execute()过程发生错误：" + ex.Message);
            }
        }

        /// <summary>
        /// Event事件类型请求
        /// </summary>
        public virtual IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var strongRequestMessage = RequestMessage as IRequestMessageEventBase;
            IResponseMessageBase responseMessage = null;
            switch (strongRequestMessage.Event)
            {
                case Event.subscribe://订阅
                    responseMessage = OnEvent_SubscribeRequest(RequestMessage as RequestMessageEvent_Subscribe);
                    break;
                case Event.unsubscribe://退订
                    responseMessage = OnEvent_UnsubscribeRequest(RequestMessage as RequestMessageEvent_Unsubscribe);
                    break;
                case Event.scan://二维码
                    responseMessage = OnEvent_ScanRequest(RequestMessage as RequestMessageEvent_Scan);
                    break;
                case Event.LOCATION://自动发送的用户当前位置
                    responseMessage = OnEvent_LocationRequest(RequestMessage as RequestMessageEvent_Location);
                    break;
                case Event.CLICK://菜单点击
                    responseMessage = OnEvent_ClickRequest(RequestMessage as RequestMessageEvent_Click);
                    break;
                case Event.VIEW://URL跳转（view视图）
                    responseMessage = OnEvent_ViewRequest(RequestMessage as RequestMessageEvent_View);
                    break;
                case Event.pic_photo_or_album://弹出拍照或者相册发图
                    responseMessage = OnEvent_PicPhotoOrAlbumRequest(RequestMessage as RequestMessageEvent_Pic_Photo_Or_Album);
                    break;
                case Event.scancode_push://扫码推事件
                    responseMessage = OnEvent_ScancodePushRequest(RequestMessage as RequestMessageEvent_Scancode_Push);
                    break;
                case Event.scancode_waitmsg://扫码推事件且弹出“消息接收中”提示框
                    responseMessage = OnEvent_ScancodeWaitmsgRequest(RequestMessage as RequestMessageEvent_Scancode_Waitmsg);
                    break;
                case Event.location_select://弹出地理位置选择器
                    responseMessage = OnEvent_LocationSelectRequest(RequestMessage as RequestMessageEvent_Location_Select);
                    break;
                case Event.pic_weixin://弹出微信相册发图器
                    responseMessage = OnEvent_PicWeixinRequest(RequestMessage as RequestMessageEvent_Pic_Weixin);
                    break;
                case Event.pic_sysphoto://弹出系统拍照发图
                    responseMessage = OnEvent_PicSysphotoRequest(RequestMessage as RequestMessageEvent_Pic_Sysphoto);
                    break;
                case Event.MASSSENDJOBFINISH://群发消息成功
                    responseMessage = OnEvent_MassSendJobFinishRequest(RequestMessage as RequestMessageEvent_MassSendJobFinish);
                    break;
                case Event.TEMPLATESENDJOBFINISH://推送模板消息成功
                    responseMessage = OnEvent_TemplateSendJobFinishRequest(RequestMessage as RequestMessageEvent_TemplateSendJobFinish);
                    break;
                default:
                    throw new Exception("未知的Event下属请求信息");
            }
            return responseMessage;
        }

        /// <summary>
        /// Event事件类型请求之subscribe
        /// </summary>
        public virtual IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之unsubscribe
        /// </summary>
        public virtual IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之scan
        /// </summary>
        public virtual IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之LOCATION
        /// </summary>
        public virtual IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之CLICK
        /// </summary>
        public virtual IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之View
        /// </summary>
        public virtual IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 扫码推事件
        /// </summary>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_PicWeixinRequest(RequestMessageEvent_Pic_Weixin requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_PicSysphotoRequest(RequestMessageEvent_Pic_Sysphoto requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 事件推送群发结果
        /// </summary>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_MassSendJobFinishRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 发送模板消息返回结果
        /// </summary>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_TemplateSendJobFinishRequest(RequestMessageEvent_TemplateSendJobFinish requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }
    }
}