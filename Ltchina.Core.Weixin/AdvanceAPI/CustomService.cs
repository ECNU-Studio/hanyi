using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.Helper;
using Ltchina.Core.Weixin.CommonAPI;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    public class CustomInfoJson : WxJsonResult
    {
        /// <summary>
        /// 客服列表
        /// </summary>
        public List<CustomInfo_Json> kf_list { get; set; }
    }

    public class CustomInfo_Json
    {
        /// <summary>
        /// 客服账号
        /// </summary>
        public string kf_account { get; set; }

        /// <summary>
        /// 客服昵称
        /// </summary>
        public string kf_nick { get; set; }

        /// <summary>
        /// 客服工号
        /// </summary>
        public int kf_id { get; set; }
    }

    public class CustomOnlineJson : WxJsonResult
    {
        /// <summary>
        /// 在线客服列表
        /// </summary>
        public List<CustomOnline_Json> kf_online_list { get; set; }
    }

    public class CustomOnline_Json
    {
        /// <summary>
        /// 客服账号
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 客服在线状态 1：pc在线，2：手机在线 若pc和手机同时在线则为 1+2=3
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 客服工号
        /// </summary>
        public int kf_id { get; set; }

        /// <summary>
        /// 客服设置的最大自动接入数
        /// </summary>
        public int auto_accept { get; set; }

        /// <summary>
        /// 客服当前正在接待的会话数
        /// </summary>
        public int accepted_case { get; set; }
    }

    /// <summary>
    /// 聊天记录结果
    /// </summary>
    public class GetRecordResult : WxJsonResult
    {
        public List<RecordJson> recordlist { get; set; }
    }

    /// <summary>
    /// 客服记录消息
    /// </summary>
    public class RecordJson
    {
        /// <summary>
        /// 客服账号
        /// </summary>
        public string worker { get; set; }
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 操作ID（会话状态）
        /// </summary>
        public Opercode opercode { get; set; }
        /// <summary>
        /// 操作时间，UNIX时间戳
        /// </summary>
        public long time { get; set; }
        /// <summary>
        /// 聊天记录
        /// </summary>
        public string text { get; set; }
    }

    /// <summary>
    /// 操作ID(会化状态）定义
    /// </summary>
    public enum Opercode
    {
        创建未接入会话 = 1000,
        接入会话 = 1001,
        主动发起会话 = 1002,
        关闭会话 = 1004,
        抢接会话 = 1005,
        公众号收到消息 = 2001,
        客服发送消息 = 2002,
        客服收到消息 = 2003
    }

    /// <summary>
    /// 多客服接口
    /// </summary>
    public static class CustomService
    {
        /// <summary>
        /// 获取用户聊天记录
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="startTime">查询开始时间，会自动转为UNIX时间戳</param>
        /// <param name="endTime">查询结束时间，会自动转为UNIX时间戳，每次查询不能跨日查询</param>
        /// <param name="openId">（非必须）普通用户的标识，对当前公众号唯一</param>
        /// <param name="pageSize">每页大小，每页最多拉取1000条</param>
        /// <param name="pageIndex">查询第几页，从1开始</param>
        public static GetRecordResult GetRecord(string accessToken, DateTime startTime, DateTime endTime, string openId = null, int pageSize = 10, int pageIndex = 1)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/customservice/getrecord?access_token={0}";

            //规范页码
            if (pageSize <= 0)
            {
                pageSize = 1;
            }
            else if (pageSize > 1000)
            {
                pageSize = 1000;
            }


            //组装发送消息
            var data = new
            {
                starttime = DateTimeHelper.GetWeixinDateTime(startTime),
                endtime = DateTimeHelper.GetWeixinDateTime(endTime),
                openId = openId,
                pagesize = pageSize,
                pageIndex = pageIndex
            };

            return CommonJsonSend.Send<GetRecordResult>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 获取在线客服接待信息
        /// 官方API：http://dkf.qq.com/document-3_2.html
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <returns></returns>
        public static CustomOnlineJson GetCustomOnlineInfo(string accessToken)
        {
            var urlFormat = string.Format("https://api.weixin.qq.com/cgi-bin/customservice/getonlinekflist?access_token={0}", accessToken);
            return GetCustomInfoResult<CustomOnlineJson>(urlFormat);
        }

        /// <summary>
        /// 获取客服基本信息
        /// 官方API：http://dkf.qq.com/document-3_1.html
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <returns></returns>
        public static CustomInfoJson GetCustomBasicInfo(string accessToken)
        {
            var urlFormat = string.Format("https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token={0}", accessToken);
            return GetCustomInfoResult<CustomInfoJson>(urlFormat);
        }

        private static T GetCustomInfoResult<T>(string urlFormat)
        {
            var jsonString = HttpUtility.RequestUtility.HttpGet(urlFormat, Encoding.UTF8);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(jsonString);
        }
    }
}