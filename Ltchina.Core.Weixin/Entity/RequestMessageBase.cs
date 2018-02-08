
namespace Ltchina.Core.Weixin.Entity
{
    public interface IRequestMessageBase : IMessageBase
    {
        RequestMsgType MsgType { get; }
        long MsgId { get; set; }

        string Encrypt { get; set; }
    }

    /// <summary>
    /// 接收到请求的消息
    /// </summary>
    public class RequestMessageBase : MessageBase, IRequestMessageBase
    {
        public RequestMessageBase()
        {

        }

        public virtual RequestMsgType MsgType { get; set; }

        public long MsgId { get; set; }

        public string Encrypt { get; set; }
    }
}