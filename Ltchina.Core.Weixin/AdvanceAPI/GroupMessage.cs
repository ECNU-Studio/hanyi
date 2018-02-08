using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.CommonAPI;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    public static class GroupMessage
    {
        /// <summary>
        /// 上传图文消息素材
        /// </summary>
        /// <param name="accessToken">Token</param>
        /// <param name="news">图文消息组</param>
        /// <returns></returns>
        public static UploadMediaFileResult UploadNews(string accessToken, params NewsModel[] news)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}";
            var data = new
            {
                articles = news
            };
            return CommonJsonSend.Send<UploadMediaFileResult>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 根据分组进行群发
        /// 
        /// 请注意：
        /// 1、该接口暂时仅提供给已微信认证的服务号
        /// 2、虽然开发者使用高级群发接口的每日调用限制为100次，但是用户每月只能接收4条，请小心测试
        /// 3、无论在公众平台网站上，还是使用接口群发，用户每月只能接收4条群发消息，多于4条的群发将对该用户发送失败。
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SendResult SendGroupMessageByGroupId(string accessToken, string groupId, string mediaId, GroupMessageType type)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";

            BaseGroupMessageDataByGroupId baseData = null;
            bool flag = false;
            switch (type)
            {
                case GroupMessageType.image:
                    baseData = new GroupMessageByGroupId_ImageData()
                    {
                        filter = new GroupMessageByGroupId_GroupId()
                        {
                            group_id = groupId
                        },
                        image = new GroupMessageByGroupId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "image"
                    };
                    break;
                case GroupMessageType.voice:
                    baseData = new GroupMessageByGroupId_VoiceData()
                    {
                        filter = new GroupMessageByGroupId_GroupId()
                        {
                            group_id = groupId
                        },
                        voice = new GroupMessageByGroupId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "voice"
                    };
                    break;
                case GroupMessageType.mpnews:
                    baseData = new GroupMessageByGroupId_MpNewsData()
                    {
                        filter = new GroupMessageByGroupId_GroupId()
                        {
                            group_id = groupId
                        },
                        mpnews = new GroupMessageByGroupId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "mpnews"
                    };
                    break;
                case GroupMessageType.video:
                    baseData = new GroupMessageByGroupId_MpVideoData()
                    {
                        filter = new GroupMessageByGroupId_GroupId()
                        {
                            group_id = groupId
                        },
                        mpvideo = new GroupMessageByGroupId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "mpvideo"
                    };
                    break;
                case GroupMessageType.text:
                    flag = true;
                    //throw new Exception("发送文本信息请使用SendTextGroupMessageByGroupId方法。");
                    break;
                default:
                    flag = true;
                    //throw new Exception("参数错误。");
                    break;
            }
            if (flag) throw new Exception("参数错误。");
            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, baseData);
        }

        /// <summary>
        /// 根据GroupId进行群发文本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="content">用于群发文本消息的content</param>
        /// <returns></returns>
        public static SendResult SendTextGroupMessageByGroupId(string accessToken, string groupId, string content)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";

            BaseGroupMessageDataByGroupId baseData = new GroupMessageByGroupId_TextData()
            {
                filter = new GroupMessageByGroupId_GroupId()
                {
                    group_id = groupId
                },
                text = new GroupMessageByGroupId_Content()
                {
                    content = content
                },
                msgtype = "text"
            };

            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, baseData);
        }

        /// <summary>
        /// 给所有用户群发
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SendResult SendGroupMessageToAll(string accessToken, string mediaId, GroupMessageType type)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";

            BaseGroupMessageDataByGroupId baseData = null;
            bool flag = false;
            switch (type)
            {
                case GroupMessageType.image:
                    baseData = new GroupMessageByGroupId_ImageData()
                    {
                        filter = new GroupMessageByGroupId_GroupId()
                        {
                            is_to_all = true
                        },
                        image = new GroupMessageByGroupId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "image"
                    };
                    break;
                case GroupMessageType.voice:
                    baseData = new GroupMessageByGroupId_VoiceData()
                    {
                        filter = new GroupMessageByGroupId_GroupId()
                        {
                            is_to_all = true
                        },
                        voice = new GroupMessageByGroupId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "voice"
                    };
                    break;
                case GroupMessageType.mpnews:
                    baseData = new GroupMessageByGroupId_MpNewsData()
                    {
                        filter = new GroupMessageByGroupId_GroupId()
                        {
                            is_to_all = true
                        },
                        mpnews = new GroupMessageByGroupId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "mpnews"
                    };
                    break;
                case GroupMessageType.video:
                    baseData = new GroupMessageByGroupId_MpVideoData()
                    {
                        filter = new GroupMessageByGroupId_GroupId()
                        {
                            is_to_all = true
                        },
                        mpvideo = new GroupMessageByGroupId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "mpvideo"
                    };
                    break;
                case GroupMessageType.text:
                    flag = true;
                    //throw new Exception("发送文本信息请使用SendTextGroupMessageToAll方法。");
                    break;
                default:
                    flag = true;
                    //throw new Exception("参数错误。");
                    break;
            }
            if (flag) throw new Exception("参数错误。");
            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, baseData);
        }

        /// <summary>
        /// 给所有用户群发文本
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static SendResult SendTextGroupMessageToAll(string accessToken, string content)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";

            BaseGroupMessageDataByGroupId baseData = new GroupMessageByGroupId_TextData()
            {
                filter = new GroupMessageByGroupId_GroupId()
                {
                    is_to_all = true
                },
                text = new GroupMessageByGroupId_Content()
                {
                    content = content
                },
                msgtype = "text"
            };

            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, baseData);
        }

        /// <summary>
        /// 根据OpenId进行群发
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <param name="type"></param>
        /// <param name="openIds">openId字符串数组</param>
        /// 注意mediaId和content不可同时为空
        /// <returns></returns>
        public static SendResult SendGroupMessageByOpenId(string accessToken, GroupMessageType type, string mediaId, params string[] openIds)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";

            BaseGroupMessageDataByOpenId baseData = null;
            bool flag = false;
            switch (type)
            {
                case GroupMessageType.image:
                    baseData = new GroupMessageByOpenId_ImageData()
                    {
                        touser = openIds,
                        image = new GroupMessageByOpenId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "image"
                    };
                    break;
                case GroupMessageType.voice:
                    baseData = new GroupMessageByOpenId_VoiceData()
                    {
                        touser = openIds,
                        voice = new GroupMessageByOpenId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "voice"
                    };
                    break;
                case GroupMessageType.mpnews:
                    baseData = new GroupMessageByOpenId_MpNewsData()
                    {
                        touser = openIds,
                        mpnews = new GroupMessageByOpenId_MediaId()
                        {
                            media_id = mediaId
                        },
                        msgtype = "mpnews"
                    };
                    break;
                case GroupMessageType.video:
                    flag = true;
                    //throw new Exception("发送视频信息请使用SendVideoGroupMessageByOpenId方法。");
                    break;
                case GroupMessageType.text:
                    flag = true;
                    //throw new Exception("发送文本信息请使用SendTextGroupMessageByOpenId方法。");
                    break;
                default:
                    flag = true;
                    //throw new Exception("参数错误。");
                    break;
            }
            if (flag) throw new Exception("参数错误。");
            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, baseData);
        } 

        /// <summary>
        /// 根据OpenId进行群发文本消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="content"></param>
        /// <param name="openIds">openId字符串数组</param>
        /// 注意mediaId和content不可同时为空
        /// <returns></returns>
        public static SendResult SendTextGroupMessageByOpenId(string accessToken, string content, params string[] openIds)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";

            BaseGroupMessageDataByOpenId baseData = new GroupMessageByOpenId_TextData()
            {
                touser = openIds,
                text = new GroupMessageByOpenId_Content()
                {
                    content = content
                },
                msgtype = "text"
            };

            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, baseData);
        }

        /// <summary>
        /// 根据OpenId进行群发视频消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="title"></param>
        /// <param name="mediaId"></param>
        /// <param name="openIds">openId字符串数组</param>
        /// <param name="description"></param>
        /// 注意mediaId和content不可同时为空
        /// <returns></returns>
        public static SendResult SendVideoGroupMessageByOpenId(string accessToken, string title, string description, string mediaId, params string[] openIds)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";

            BaseGroupMessageDataByOpenId baseData = new GroupMessageByOpenId_MpVideoData()
            {
                touser = openIds,
                video = new GroupMessageByOpenId_Video()
                {
                    title = title,
                    description = description,
                    media_id = mediaId
                },
                msgtype = "video"
            };

            return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, baseData);
        }

        /// <summary>
        /// 删除群发消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId">发送出去的消息ID</param>
        /// <returns></returns>
        public static WxJsonResult DeleteSendMessage(string accessToken, string mediaId)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token={0}";

            var data = new
            {
                msg_id = mediaId
            };
            return CommonJsonSend.Send<WxJsonResult>(accessToken, urlFormat, data);
        }
    }

    /// <summary>
    /// 发送信息后的结果
    /// </summary>
    public class SendResult : WxJsonResult
    {
        /// <summary>
        /// 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb），图文消息为news
        /// </summary>
        public UploadMediaFileType type { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string msg_id { get; set; }
    }
}