using Ltchina.Core.Weixin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Ltchina.Core.Weixin.Helper
{
    public static class EntityHelper
    {
        /// <summary>
        /// 根据XML信息填充实实体
        /// </summary>
        /// <typeparam name="T">MessageBase为基类的类型，Response和Request都可以</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="doc">XML</param>
        public static void FillEntityWithXml<T>(this T entity, XDocument doc) where T : /*MessageBase*/ class, new()
        {
            entity = entity ?? new T();
            var root = doc.Root;

            var props = entity.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                if (root.Element(propName) != null)
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "DateTime":
                            prop.SetValue(entity, DateTimeHelper.GetDateTimeFromXml(root.Element(propName).Value), null);
                            break;
                        case "Boolean":
                            if (propName == "FuncFlag")
                            {
                                prop.SetValue(entity, root.Element(propName).Value == "1", null);
                            }
                            else
                            {
                                goto default;
                            }
                            break;
                        case "Int32":
                            prop.SetValue(entity, int.Parse(root.Element(propName).Value), null);
                            break;
                        case "Int64":
                            prop.SetValue(entity, long.Parse(root.Element(propName).Value), null);
                            break;
                        case "Double":
                            prop.SetValue(entity, double.Parse(root.Element(propName).Value), null);
                            break;
                        //以下为枚举类型
                        case "RequestMsgType":
                            //已设为只读
                            //prop.SetValue(entity, MsgTypeHelper.GetRequestMsgType(root.Element(propName).Value), null);
                            break;
                        case "ResponseMsgType"://Response适用
                            //已设为只读
                            //prop.SetValue(entity, MsgTypeHelper.GetResponseMsgType(root.Element(propName).Value), null);
                            break;
                        case "Event":
                            //已设为只读
                            //prop.SetValue(entity, EventHelper.GetEventType(root.Element(propName).Value), null);
                            break;
                        default:
                            prop.SetValue(entity, root.Element(propName).Value, null);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 将实体转为XML
        /// </summary>
        /// <typeparam name="T">RequestMessage或ResponseMessage</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static XDocument ConvertEntityToXml<T>(this T entity) where T : class , new()
        {
            entity = entity ?? new T();
            var doc = new XDocument();
            doc.Add(new XElement("xml"));
            var root = doc.Root;

            /* 注意！
             * 经过测试，微信对字段排序有严格要求，这里对排序进行强制约束
            */
            var propNameOrder = new List<string>() { "ToUserName", "FromUserName", "CreateTime", "MsgType" };
            if (entity is ResponseMessageNews)
            {
                propNameOrder.AddRange(new[] { "ArticleCount", "Articles", "FuncFlag",/*以下是Atricle属性*/ "Title ", "Description ", "PicUrl", "Url" });
            }
            else if (entity is ResponseMessageTransfer_Customer_Service)
            {
                propNameOrder.AddRange(new[] { "TransInfo", "KfAccount", "FuncFlag" });
            }
            else if (entity is ResponseMessageMusic)
            {
                propNameOrder.AddRange(new[] { "Music", "FuncFlag", /*以下是Music属性*/ "Title ", "Description ", "MusicUrl", "HQMusicUrl", "ThumbMediaId" });
            }
            else if (entity is ResponseMessageImage)
            {
                propNameOrder.AddRange(new[] { "Image",/*以下是Image属性*/ "MediaId " });
            }
            else if (entity is ResponseMessageVoice)
            {
                propNameOrder.AddRange(new[] { "Voice",/*以下是Voice属性*/ "MediaId " });
            }
            else if (entity is ResponseMessageVideo)
            {
                propNameOrder.AddRange(new[] { "Video",/*以下是Video属性*/ "MediaId ", "Title", "Description" });
            }
            else
            {
                propNameOrder.AddRange(new[] { "Content", "FuncFlag" });
            }
            Func<string, int> orderByPropName = propNameOrder.IndexOf;

            var props = entity.GetType().GetProperties().OrderBy(p => orderByPropName(p.Name)).ToList();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                if (propName == "Articles")
                {
                    //文章列表
                    var atriclesElement = new XElement("Articles");
                    var articales = prop.GetValue(entity, null) as List<Article>;
                    foreach (var articale in articales)
                    {
                        var subNodes = ConvertEntityToXml(articale).Root.Elements();
                        atriclesElement.Add(new XElement("item", subNodes));
                    }
                    root.Add(atriclesElement);
                }
                else if (propName == "TransInfo")
                {
                    var transInfoElement = new XElement("TransInfo");
                    var transInfo = prop.GetValue(entity, null) as List<CustomerServiceAccount>;
                    foreach (var account in transInfo)
                    {
                        var trans = ConvertEntityToXml(account).Root.Elements();
                        transInfoElement.Add(trans);
                    }

                    root.Add(transInfoElement);
                }
                else if(propName == "Music" || propName == "Image" || propName == "Video" || propName == "Voice")
                {
                    //音乐、图片、视频、语音格式
                    var musicElement = new XElement(propName);
                    var media = prop.GetValue(entity, null);// as Music;
                    var subNodes = ConvertEntityToXml(media).Root.Elements();
                    musicElement.Add(subNodes);
                    root.Add(musicElement);
                }
                else if (propName == "KfAccount")
                {
                    root.Add(new XElement(propName, prop.GetValue(entity, null).ToString().ToLower()));
                }
                else
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "String":
                            root.Add(new XElement(propName,
                                                  new XCData(prop.GetValue(entity, null) as string ?? "")));
                            break;
                        case "DateTime":
                            root.Add(new XElement(propName, DateTimeHelper.GetWeixinDateTime((DateTime)prop.GetValue(entity, null))));
                            break;
                        case "Boolean":
                            if (propName == "FuncFlag")
                            {
                                root.Add(new XElement(propName, (bool)prop.GetValue(entity, null) ? "1" : "0"));
                            }
                            else
                            {
                                goto default;
                            }
                            break;
                        case "ResponseMsgType":
                            root.Add(new XElement(propName, new XCData(prop.GetValue(entity, null).ToString().ToLower())));
                            break;
                        case "Article":
                            root.Add(new XElement(propName, prop.GetValue(entity, null).ToString().ToLower()));
                            break;
                        case "TransInfo":
                            root.Add(new XElement(propName, prop.GetValue(entity, null).ToString().ToLower()));
                            break;
                        default:
                            root.Add(new XElement(propName, prop.GetValue(entity, null)));
                            break;
                    }
                }
            }
            return doc;
        }

        // <summary>
        /// ResponseMessageBase.CreateFromRequestMessage<T>(requestMessage)的扩展方法
        /// </summary>
        /// <typeparam name="T">需要生成的ResponseMessage类型</typeparam>
        /// <param name="requestMessage">IRequestMessageBase接口下的接收信息类型</param>
        /// <returns></returns>
        public static T CreateResponseMessage<T>(this IRequestMessageBase requestMessage) where T : ResponseMessageBase
        {
            return ResponseMessageBase.CreateFromRequestMessage<T>(requestMessage);
        }
    }
}