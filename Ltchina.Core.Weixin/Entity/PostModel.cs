﻿
namespace Ltchina.Core.Weixin.Entity
{
    /// <summary>
    /// 微信公众服务器Post过来的加密参数集合（不包括PostData）
    /// </summary>
    public class PostModel
    {
        public string Signature { get; set; }
        public string Msg_Signature { get; set; }
        public string Timestamp { get; set; }
        public string Nonce { get; set; }

        //以下信息不会出现在微信发过来的信息中，都是企业号后台需要设置（获取的）的信息，用于扩展传参使用
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
        public string AppId { get; set; }

        public long UserId { get; set; }
    }
}