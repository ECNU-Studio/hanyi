using System;

namespace Ltchina.Core.Weixin.Entity
{
    public interface IResponseMessageBase : IMessageBase
    {

    }

    /// <summary>
    /// 响应回复消息
    /// </summary>
    public class ResponseMessageBase : MessageBase, IResponseMessageBase
    {
        public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Text; }
        }

        /// <summary>
        /// 获取响应类型实例，并初始化
        /// </summary>
        /// <typeparam name="T">需要返回的类型</typeparam>
        /// <param name="requestMessage">请求数据</param>
        /// <returns></returns>
        public static T CreateFromRequestMessage<T>(IRequestMessageBase requestMessage) where T : ResponseMessageBase
        {
            try
            {
                var tType = typeof(T);
                var responseName = tType.Name.Replace("ResponseMessage", ""); //请求名称
                ResponseMsgType msgType = (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), responseName);
                return CreateFromRequestMessage(requestMessage, msgType) as T;
            }
            catch (Exception ex)
            {
                throw new Exception("ResponseMessageBase.CreateFromRequestMessage<T>过程发生异常！"+ex.Message);
            }
        }

        private static ResponseMessageBase CreateFromRequestMessage(IRequestMessageBase requestMessage, ResponseMsgType msgType)
        {
            ResponseMessageBase responseMessage = null;
            try
            {
                switch (msgType)
                {
                    case ResponseMsgType.Text:
                        responseMessage = new ResponseMessageText();
                        break;
                    case ResponseMsgType.News:
                        responseMessage = new ResponseMessageNews();
                        break;
                    case ResponseMsgType.Music:
                        responseMessage = new ResponseMessageMusic();
                        break;
                    case ResponseMsgType.Image:
                         responseMessage = new ResponseMessageImage();
                        break;
                    case ResponseMsgType.Voice:
                        responseMessage = new ResponseMessageVoice();
                        break;
                    case ResponseMsgType.Video:
                        responseMessage = new ResponseMessageVideo();
                        break;
                    case ResponseMsgType.Transfer_Customer_Service:
                        responseMessage = new ResponseMessageTransfer_Customer_Service();
                        break;
                    default:
                        throw new Exception(string.Format("ResponseMsgType没有为 {0} 提供对应处理程序。", msgType), new ArgumentOutOfRangeException());
                }

                responseMessage.ToUserName = requestMessage.FromUserName;
                responseMessage.FromUserName = requestMessage.ToUserName;
                responseMessage.CreateTime = DateTime.Now; //使用当前最新时间

            }
            catch (Exception ex)
            {
                throw new Exception("CreateFromRequestMessage过程发生异常"+ex.Message);
            }

            return responseMessage;
        }
    }
}