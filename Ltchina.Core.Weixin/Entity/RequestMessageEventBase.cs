
namespace Ltchina.Core.Weixin.Entity
{
    public interface IRequestMessageEventBase : IRequestMessageBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        Event Event { get; }
    }

    /// <summary>
    /// 具有EventKey属性的RequestMessage接口
    /// </summary>
    public interface IRequestMessageEventKey
    {
        string EventKey { get; set; }
    }

    public class RequestMessageEventBase : RequestMessageBase, IRequestMessageEventBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Event; }
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public virtual Event Event
        {
            get { return Event.CLICK; }
        }
    }
}