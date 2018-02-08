using Ltchina.Core.Weixin.CommonAPI;
using Ltchina.Core.Weixin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    public class Shop
    {
        /// <summary>
        /// 查询门店列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static ShopListRet getPoiList(string accessToken)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token={0}";
            var data = new
            {
                begin = 0,
                limit = 10
            };
            return CommonJsonSend.Send<ShopListRet>(accessToken, urlFormat, data);
        }

        /// <summary>
        /// 获取门店信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="poi_id"></param>
        /// <returns></returns>
        public static ShopInfoRet getPoi(string accessToken, string poi_id)
        {
            var urlFormat = "http://api.weixin.qq.com/cgi-bin/poi/getpoi?access_token={0}";
            var data = new
            {
                poi_id = poi_id,
            };
            return CommonJsonSend.Send<ShopInfoRet>(accessToken, urlFormat, data);
        }
    }

    public class ShopInfoRet : WxJsonResult
    {
        public ShopInfoItem business { get; set; }

    }

    public class ShopInfoItem
    {
        public ShopInfo base_info { get; set; }
    }

    public class ShopListRet : WxJsonResult
    {
        public List<ShopListItem> business_list { get; set; }
        public int total_count { get; set; }//门店总数量
    }
    public class ShopListItem
    {
        public ShopInfo base_info { get; set; }
    }

    public class ShopInfo
    {
        public String sid { get; set; }//商户自己的id，用于后续审核通过收到poi_id 的通知时，做对应关系。请商户自己保证唯一识别性
        public String business_name { get; set; }//门店名称（仅为商户名，如：国美、麦当劳，不应包含地区、地址、分店名等信息，错误示例：北京国美）
        public String branch_name { get; set; }//分店名称（不应包含地区信息，不应与门店名有重复，错误示例：北京王府井店）
        public String province { get; set; }//门店所在的省份（直辖市填城市名,如：北京市）
        public String city { get; set; }//门店所在的城市
        public String district { get; set; }//门店所在地区
        public String address { get; set; }//门店所在的详细街道地址（不要填写省市信息）
        public String telephone { get; set; }//门店的电话（纯数字，区号、分机号均由“-”隔开）
        public List<String> categories { get; set; }//门店的类型（不同级分类用“,”隔开，如：美食，川菜，火锅。详细分类参见附件：微信门店类目表）
        public int offset_type { get; set; }//坐标类型，1 为火星坐标（目前只能选1）
        public double longitude { get; set; }//门店所在地理位置的经度
        public double latitude { get; set; }//门店所在地理位置的纬度（经纬度均为火星坐标，最好选用腾讯地图标记的坐标）
        public List<PhotoItem> photo_list { get; set; }//图片列表，url 形式，可以有多张图片，尺寸为640*340px。必须为上一接口生成的url。图片内容不允许与门店不相关，不允许为二维码、员工合照（或模特肖像）、营业执照、无门店正门的街景、地图截图、公交地铁站牌、菜单截图等
        public String recommend { get; set; }//推荐品，餐厅可为推荐菜；酒店为推荐套房；景点为推荐游玩景点等，针对自己行业的推荐内容
        public String special { get; set; }//特色服务，如免费wifi，免费停车，送货上门等商户能提供的特色功能或服务
        public String introduction { get; set; }//商户简介，主要介绍商户信息等
        public String open_time { get; set; }//营业时间，24 小时制表示，用“-”连接，如 8:00-20:00
        public int avg_price { get; set; }//人均价格，大于0 的整数
        public String poi_id { get; set; }//
        public int available_state { get; set; }//门店是否可用状态。1 表示系统错误、2 表示审核中、3 审核通过、4 审核驳回。当该字段为1、2、4 状态时，poi_id 为空
        public int update_status { get; set; }//扩展字段是否正在更新中。1 表示扩展字段正在更新中，尚未生效，不允许再次更新； 0 表示扩展字段没有在更新中或更新已生效，可以再次更新

    }

    public class PhotoItem
    {
        public String photo_url { get; set; }
    }
}
