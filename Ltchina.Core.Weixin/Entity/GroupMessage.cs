
namespace Ltchina.Core.Weixin.Entity
{
     public class BaseGroupMessageDataByGroupId
    {
        public GroupMessageByGroupId_GroupId filter { get; set; }
        public string msgtype { get; set; }
    }

    public class GroupMessageByGroupId_GroupId
    {
        public bool is_to_all { get; set; }
        public string group_id { get; set; }
    }

    public class GroupMessageByGroupId_MediaId
    {
        public string media_id { get; set; }
    }

    public class GroupMessageByGroupId_Content
    {
        public string content { get; set; }
    }

    public class GroupMessageByGroupId_VoiceData : BaseGroupMessageDataByGroupId
    {
        public GroupMessageByGroupId_MediaId voice { get; set; }  
    }

    public class GroupMessageByGroupId_ImageData : BaseGroupMessageDataByGroupId
    {
        public GroupMessageByGroupId_MediaId image { get; set; }
    }

    public class GroupMessageByGroupId_TextData : BaseGroupMessageDataByGroupId
    {
        public GroupMessageByGroupId_Content text { get; set; }
    }

    public class GroupMessageByGroupId_MpNewsData : BaseGroupMessageDataByGroupId
    {
        public GroupMessageByGroupId_MediaId mpnews { get; set; }
    }

    public class GroupMessageByGroupId_MpVideoData : BaseGroupMessageDataByGroupId
    {
        public GroupMessageByGroupId_MediaId mpvideo { get; set; }
    }
    public class BaseGroupMessageDataByOpenId
    {
        public string[] touser { get; set; }
        public string msgtype { get; set; }
    }

    public class GroupMessageByOpenId_MediaId
    {
        public string media_id { get; set; }
    }

    public class GroupMessageByOpenId_Content
    {
        public string content { get; set; }
    }

    public class GroupMessageByOpenId_Video
    {
        public string title { get; set; }
        public string media_id { get; set; }
        public string description { get; set; }
    }

    public class GroupMessageByOpenId_VoiceData : BaseGroupMessageDataByOpenId
    {
        public GroupMessageByOpenId_MediaId voice { get; set; }
    }

    public class GroupMessageByOpenId_ImageData : BaseGroupMessageDataByOpenId
    {
        public GroupMessageByOpenId_MediaId image { get; set; }
    }

    public class GroupMessageByOpenId_TextData : BaseGroupMessageDataByOpenId
    {
        public GroupMessageByOpenId_Content text { get; set; }
    }

    public class GroupMessageByOpenId_MpNewsData : BaseGroupMessageDataByOpenId
    {
        public GroupMessageByOpenId_MediaId mpnews { get; set; }
    }

    public class GroupMessageByOpenId_MpVideoData : BaseGroupMessageDataByOpenId
    {
        public GroupMessageByOpenId_Video video { get; set; }
    }
}