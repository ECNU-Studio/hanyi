using System;
using System.Collections.Generic;
using System.Linq;
using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.CommonAPI;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    /// <summary>
    /// 获取用户分组ID返回结果
    /// </summary>
    public class GetGroupIdResult : WxJsonResult
    {
        public int groupid { get; set; }
    }

    /// <summary>
    /// 创建分组返回结果
    /// </summary>
    public class CreateGroupResult : WxJsonResult
    {
        public GroupsJson_Group group { get; set; }
    }

    public class GroupsJson : WxJsonResult
    {
        public List<GroupsJson_Group> groups { get; set; }
    }

    public class GroupsJson_Group
    {
        public int id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 此属性在CreateGroupResult的Json数据中，创建结果中始终为0
        /// </summary>
        public int count { get; set; }
    }
    public static class Group
    {
        /// <summary>
        /// 创建分组
        /// </summary>
        /// <returns></returns>
        public static CreateGroupResult Create(string accessToken, string name)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}";
            var data = new
            {
                group = new
                {
                    name = name
                }
            };
            return CommonJsonSend.Send<CreateGroupResult>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 获取所有分组
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static GroupsJson Get(string accessToken)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}";
            var url = string.Format(urlFormat, accessToken);
            return Ltchina.Core.Weixin.HttpUtility.Get.GetJson<GroupsJson>(url);
        }

        /// <summary>
        /// 获取用户分组
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static GetGroupIdResult GetId(string accessToken, string openId)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token={0}";
            var data = new { openid = openId };
            return CommonJsonSend.Send<GetGroupIdResult>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 修改分组名
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="id"></param>
        /// <param name="name">分组名字（30个字符以内）</param>
        /// <returns></returns>
        public static WxJsonResult Update(string accessToken, int id, string name)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}";
            var data = new
            {
                group = new
                {
                    id = id,
                    name = name
                }
            };
            return CommonJsonSend.Send(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="toGroupId"></param>
        /// <returns></returns>
        public static WxJsonResult MemberUpdate(string accessToken, string openId, int toGroupId)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}";
            var data = new
            {
                openid = openId,
                to_groupid = toGroupId
            };
            return CommonJsonSend.Send(accessToken, urlFormat, data);
        }
    }
}