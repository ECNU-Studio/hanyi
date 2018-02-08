using System.Collections.Generic;

namespace Ltchina.Core.Weixin.Entity
{
    /// <summary>
    /// 回复文本消息实体
    /// </summary>
    public class ResponseMessageText : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Text; }
        }

        public string Content { get; set; }
    }
    /// <summary>
    /// 回复图文消息实体
    /// </summary>
    public class ResponseMessageNews : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.News; }
        }

        public int ArticleCount
        {
            get
            {
                return Articles == null ? 0 : Articles.Count;
            }
            set
            {
                //这里开放set只为了逆向从Response的Xml转成实体的操作一致性，没有实际意义。
            }
        }

        /// <summary>
        /// 文章列表，微信客户端只能输出前10条（可能未来数字会有变化，出于视觉效果考虑，建议控制在8条以内）
        /// </summary>
        public List<Article> Articles { get; set; }

        public ResponseMessageNews()
        {
            Articles = new List<Article>();
        }
    }

    /// <summary>
    /// 回复音乐消息实体
    /// </summary>
    public class ResponseMessageMusic : ResponseMessageBase, IResponseMessageBase
    {
        public override ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Music; }
        }

        public Music Music { get; set; }

        public ResponseMessageMusic()
        {
            Music = new Music();
        }
    }
    /// <summary>
    /// 回复图片实体
    /// </summary>
    public class ResponseMessageImage : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Image; }
        }

        public Image Image { get; set; }

        public ResponseMessageImage()
        {
            Image = new Image();
        }
    }

    /// <summary>
    /// 回复视频实体
    /// </summary>
    public class ResponseMessageVideo : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Video; }
        }

        public Video Video { get; set; }

        public ResponseMessageVideo()
        {
            Video = new Video();
        }
    }

    /// <summary>
    /// 回复语音实体
    /// </summary>
    public class ResponseMessageVoice : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Voice; }
        }

        public Voice Voice { get; set; }

        public ResponseMessageVoice()
        {
            Voice = new Voice();
        }
    }

    /// <summary>
    /// 回复多客服消息
    /// </summary>
    public class ResponseMessageTransfer_Customer_Service : ResponseMessageBase, IResponseMessageBase
    {
        public ResponseMessageTransfer_Customer_Service()
        {
            TransInfo = new List<CustomerServiceAccount>();
        }

        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Transfer_Customer_Service; }
        }

        public List<CustomerServiceAccount> TransInfo { get; set; }
    }

    public class CustomerServiceAccount
    {
        public string KfAccount { get; set; }
    }
}