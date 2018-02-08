using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.CommonAPI;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    /// <summary>
    /// 发送模板消息结果
    /// </summary>
    public class SendTemplateMessageResult : WxJsonResult
    {
        /// <summary>
        /// msgid
        /// </summary>
        public int msgid { get; set; }
    }

    /// <summary>
    /// 模板消息的数据项类型
    /// </summary>
    public class TemplateDataItem
    {
        /// <summary>
        /// 项目值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 16进制颜色代码，如：#FF0000
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">value</param>
        /// <param name="c">color</param>
        public TemplateDataItem(string v, string c = "#173177")
        {
            value = v;
            color = c;
        }
    }

    public class TempleteModel
    {
        /// <summary>
        /// 目标用户OpenId
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 模板消息顶部颜色（16进制），默认为#FF0000
        /// </summary>
        public string topcolor { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        public string url { get; set; }


        public TempleteModel()
        {
            topcolor = "#FF0000";
        }
    }

    /// <summary>
    /// 获取模板ID结果
    /// </summary>
    public class GetTemplateIDResult : WxJsonResult
    {
        /// <summary>
        /// template_id
        /// </summary>
        public string template_id { get; set; }
    }

    public static class TemplateMessage
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="templateId"></param>
        /// <param name="topcolor"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SendTemplateMessageResult SendTemplateMessage<T>(string accessToken, string openId, string templateId, string topcolor, string url, T data)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            var msgData = new TempleteModel()
            {
                touser = openId,
                template_id = templateId,
                topcolor = topcolor,
                url = url,
                data = data
            };
            return CommonJsonSend.Send<SendTemplateMessageResult>(accessToken, urlFormat, msgData);
        }

        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="industryid1"></param>
        /// <param name="industryid2"></param>
        /// <returns></returns>
        public static WxJsonResult SetIndustry(string accessToken, IndustryCode industryid1, IndustryCode industryid2) 
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}";
            var msgData = new { industry_id1 = industryid1, industry_id2 = industryid2 };
            return CommonJsonSend.Send<WxJsonResult>(accessToken, urlFormat, msgData);
        }

        /// <summary>
        /// 获得模板ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="templateidshort"></param>
        /// <returns></returns>
        public static GetTemplateIDResult GetTemplateId(string accessToken, string templateidshort)
        {
            const string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";
            var msgData = new { template_id_short = templateidshort};
            return CommonJsonSend.Send<GetTemplateIDResult>(accessToken, urlFormat, msgData);
        }
    }
}