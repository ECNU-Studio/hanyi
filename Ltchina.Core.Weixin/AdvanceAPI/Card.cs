using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.HttpUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    public class Card
    {
        /**
         * 创建卡券
         * 创建卡券接口是微信卡券的基础接口，用于创建一类新的卡券，获取card_id，创建成功并通过审核后，
         * 商家可以通过文档提供的其他接口将卡券下发给用户，每次成功领取，库存数量相应扣除。
         * 开发者须知
         *  1.需自定义Code码的商家必须在创建卡券时候，设定use_custom_code为true，且在调用投放卡券接口时填入指定的Code码。指定OpenID同理。特别注意：在公众平台创建的卡券均为非自定义Code类型。
         *  2.can_share字段指领取卡券原生页面是否可分享，建议指定Code码、指定OpenID等强限制条件的卡券填写false。
         *  3.特别注意：编码方式仅支持使用UTF-8，否则会报错。
         *  4.创建成功后该卡券会自动提交审核，审核结果将通过事件通知商户。开发者可调用设置白名单接口设置用户白名单，领取未通过审核的卡券，测试整个卡券的使用流程。
         *
         *  注意事项：
         *  1.高级字段为商户额外展示信息字段，非必填,但是填入某些结构体后，须填充完整方可显示：如填入text_image_list结构体
         *  时，须同时传入image_url和text，否则也会报错；
         *  2.填入时间限制字段（time_limit）,只控制显示，不控制实际使用逻辑，不填默认不显示
         *  3.创建卡券时，开发者填入的时间戳须注意时间戳溢出时间，设置的时间戳须早于2038年1月19日
         * **/
        public static CardCreateRet createCard(String accessToken,CardType type,CardInfo cardInfo){
            String url = string.Format("https://api.weixin.qq.com/card/create?access_token={0}",accessToken);
            String cardName = "";
            switch (type){
                case CardType.GROUPON:
                    cardName = "groupon";
                    break;
                case CardType.CASH:
                    cardName = "cash";
                    break;
                case CardType.DISCOUNT:
                    cardName = "discount";
                    break;
                case CardType.GIFT:
                    cardName = "gift";
                    break;
                case CardType.GENERAL_COUPON:
                    cardName = "general_coupon";
                    break;
                case CardType.MEMBER_CARD:
                    cardName = "member_card";
                    break;
            }
            String json = "{\"card\": {\"card_type\": \""+Enum.GetName(typeof(CardType),type)+"\",\""+cardName+"\":"+JsonConvert.SerializeObject(cardInfo)+"}}";
            return HttpHelper.post<CardCreateRet>(url,json);
        }

        /**
         *  设置测试白名单
         *  由于卡券有审核要求，为方便公众号调试，可以设置一些测试帐号，这些帐号可领取未通过审核的卡券，体验整个流程。
         *  开发者注意事项
         *  1.同时支持“openid”、“username”两种字段设置白名单，总数上限为10个。
         *  2.设置测试白名单接口为全量设置，即测试名单发生变化时需调用该接口重新传入所有测试人员的ID.
         *  3.白名单用户领取该卡券时将无视卡券失效状态，请开发者注意。
         *  openid		测试的openid列表。
         *  username	测试的微信号列表。
         * **/
        public static WxJsonResult setTestWhiteList(String accessToken,List<String> openIds,List<String> names){
            String url = string.Format("https://api.weixin.qq.com/card/testwhitelist/set?access_token={0}",accessToken);
            string json = "";
            if(openIds!=null&&openIds.Count()>0)
            {
                var data = new
                {
                    openid = openIds,
                };
                json = JsonConvert.SerializeObject(data);
            }
            if(names!=null&&names.Count()>0)
            {
                var data = new
                {
                    username = names,
                };
                json = JsonConvert.SerializeObject(data);
            }
            return HttpHelper.post<WxJsonResult>(url, json);
        }

        /**
         * 创建二维码接口
         * 开发者可调用该接口生成一张卡券二维码供用户扫码后添加卡券到卡包。
         *  自定义Code码的卡券调用接口时，POST数据中需指定code，非自定义code不需指定，指定openid同理。指定后的二维码只能被用户扫描领取一次。
         *  获取二维码ticket后，开发者可用通过ticket换取二维码接口换取二维码图片详情。
         * expire_seconds	否	unsigned int	60	指定二维码的有效时间，范围是60 ~ 1800秒。不填默认为365天有效
         * **/
        public static QrCodeRet createQrcode(String accessToken,int? expire_seconds,CardQrcodeType type,List<CardQrcodeInfo> list) {
            String url = string.Format("https://api.weixin.qq.com/card/qrcode/create?access_token={0}",accessToken);
            string json = "";
            if(type == CardQrcodeType.QR_CARD){
                if (expire_seconds.HasValue)
                {
                    var data = new
                    {
                        action_name = Enum.GetName(typeof(CardQrcodeType), type),
                        expire_seconds = expire_seconds.Value,
                        action_info = new {
                            card = list[0],
                        },
                    };
                    json = JsonConvert.SerializeObject(data);
                }
                else 
                {
                    var data = new
                    {
                        action_name = Enum.GetName(typeof(CardQrcodeType), type),
                        action_info = new
                        {
                            card = list[0],
                        },
                    };
                    json = JsonConvert.SerializeObject(data);
                }
            }else{
                var data = new
                {
                    action_name = Enum.GetName(typeof(CardQrcodeType), type),
                    action_info = new
                    {
                        multiple_card = new {
                            card_list = list,
                        },
                    },
                };
                json = JsonConvert.SerializeObject(data);
            }
            return HttpHelper.post<QrCodeRet>(url, json);
        }

        /**
         * 创建货架接口
         * 开发者需调用该接口创建货架链接，用于卡券投放。创建货架时需填写投放路径的场景字段。
         * 卡券货架支持开发者通过调用接口生成一个卡券领取H5页面，并获取页面链接，进行卡券投放动作。
         * 目前卡券货架仅支持非自定义code的卡券，自定义code的卡券需先调用导入code接口将code导入才能正常使用。
         * **/
        public static CardLandingPageRet createLandingPage(String accessToken,CardLandingRequest request){
            String url = string.Format("https://api.weixin.qq.com/card/landingpage/create?access_token={0}",accessToken);
            string json = JsonConvert.SerializeObject(request);
            return HttpHelper.post<CardLandingPageRet>(url, json);
        }
    }

    public class CardLandingRequest {
        public String banner{ get; set; }//页面的banner图片链接，须调用，建议尺寸为640*300。
        public String page_title{ get; set; }//页面的title。
        public bool can_share{ get; set; }//页面是否可以分享,填入true/false
        public CardSceneType scene{ get; set; }//投放页面的场景值；
        public List<CardLandingItem> card_list{ get; set; }//卡券列表，每个item有两个字段
    }

    public class CardLandingItem {
        public String card_id{ get; set; }//所要在页面投放的card_id
        public String thumb_url{ get; set; }//缩略图url
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardSceneType {
        SCENE_NEAR_BY,// 附近
        SCENE_MENU,	//自定义菜单
        SCENE_QRCODE,//	二维码
        SCENE_ARTICLE,//	公众号文章
        SCENE_H5,//	h5页面
        SCENE_IVR,//	自动回复
        SCENE_CARD_CUSTOM_CELL,//	卡券自定义cell
    }

    public class CardLandingPageRet : WxJsonResult {
        public String url{ get; set; }//货架链接。
        public int page_id{ get; set; }//货架ID。货架的唯一标识。
    }

    public class QrCodeRet : WxJsonResult{
        public String ticket { get; set; }//获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。
        public long expire_seconds { get; set; }//该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）。
        public String url { get; set; }//二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        public String show_qrcode_url { get; set; }//二维码显示地址，点击后跳转二维码页面;
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardQrcodeType 
    {
        QR_CARD,//扫描二维码领取单张卡券
        QR_MULTIPLE_CARD,//扫描二维码领取多张卡券
    }
 
    public class CardQrcodeInfo {
        public String card_id{ get; set; }//卡券ID。
        public String code{ get; set; }//卡券Code码,use_custom_code字段为true的卡券必须填写，非自定义code不必填写。
        public String openid{ get; set; }//指定领取者的openid，只有该用户能领取。bind_openid字段为true的卡券必须填写，非指定openid不必填写。
        public Boolean is_unique_code{ get; set; }//指定下发二维码，生成的二维码随机分配一个code，领取后不可再次扫描。填写true或false。默认false，注意填写该字段时，卡券须通过审核且库存不为0。
        public long outer_id{ get; set; }//领取场景值，用于领取渠道的数据统计，默认值为0，字段类型为整型，长度限制为60位数字。用户领取卡券后触发的事件推送中会带上此自定义场景值。
        public String outer_str{ get; set; }//outer_id字段升级版本，用户首次领卡时，会通过事件推送给商户；对于会员卡的二维码，用户每次扫码打开会员卡后点击任何url，会将该值拼入url中，方便开发者定位扫码来源
    }

    public class CardCreateRet : WxJsonResult {
        public String card_id{get;set;}//卡券ID。
    }

    public class CardGroupon : CardInfo
    {
        public String deal_detail { get; set; }//团购券专用，团购详情。
    }

    public class MemberCard : CardInfo 
    {
        public string background_pic_url { get; set; }//商家自定义会员卡背景图，须先调用上传图片接口将背景图上传至CDN，否则报错，卡面设计请遵循微信会员卡自定义背景设计规范  ,像素大小控制在1000像素*600像素以下
        public string prerogative  { get; set; }// 是   string（3072）会员卡特权说明。            
        public bool auto_activate { get; set; }//  否   bool   设置为true时用户领取会员卡后系统自动将其激活，无需调用激活接口，详情见自动激活。            
        public bool wx_activate { get; set; }// 否   bool   设置为true时会员卡支持一键开卡，不允许同时传入activate_url字段，否则设置wx_activate失效。填入该字段后仍需调用接口设置开卡项方可生效，详情见一键开卡。            
        public bool supply_bonus { get; set; }//是   bool   显示积分，填写true或false，如填写true，积分相关字段均为必填。            
        public string bonus_url  { get; set; }// 否   string(128)   设置跳转外链查看积分详情。仅适用于积分无法通过激活接口同步的情况下使用该字段。            
        public bool supply_balance  { get; set; }// 是   bool   是否支持储值，填写true或false。如填写true，储值相关字段均为必填。            
        public string balance_url { get; set; }//  否   string(128)   设置跳转外链查看余额详情。仅适用于余额无法通过激活接口同步的情况下使用该字段。            
        public CustomField custom_field1 { get; set; }//  否   JSON结构   自定义会员信息类目，会员卡激活后显示,包含name_type(name)和url字段            
        public CustomField custom_field2 { get; set; }// 否   JSON结构   自定义会员信息类目，会员卡激活后显示，包含name_type(name)和url字段            
        public CustomField custom_field3 { get; set; }//  否   JSON结构   自定义会员信息类目，会员卡激活后显示，包含name_type(name)和url字段      
        public string bonus_cleared { get; set; }//  否   string（128）   积分清零规则。            
        public string bonus_rules { get; set; }//  否   string（128）   积分规则。            
        public string balance_rules { get; set; }//  否   string（128）   储值说明。            
        public string activate_url { get; set; }//  是   string（128）   激活会员卡的url。            
        public CustomCell custom_cell1 { get; set; }//  否   JSON结构   自定义会员信息类目，会员卡激活后显示。            
        public BonusRule bonus_rule { get; set; }// 否   JSON结构      	积分规则。            
        public int discount { get; set; }//否   int   折扣，该会员卡享受的折扣优惠,填10就是九折。     

    }

    public class CustomCell 
    {
        public string name { get; set; }//   是   string（15）   入口名称。            
        public string tips { get; set; }//  是   string（18）   入口右侧提示语，6个汉字内。            
        public string url { get; set; }// 是   string（128）   入口跳转链接。            
    }

    public class CustomField 
    {
        public FieldNameType name_type { get; set; }// 否   string(24)   会员信息类目半自定义名称，当开发者变更这类类目信息的value值时可以选择触发系统模板消息通知用户。       
        public string name { get; set; }//否	string(24) 	会员信息类目自定义名称，当开发者变更这类类目信息的value值时不会触发系统模板消息通知用户
        public string url { get; set; }// 否   string（128）   点击类目跳转外链url     
    }

    public class BonusRule 
    {
        public int cost_money_unit { get; set; }//否 int 消费金额。以分为单位。            
        public int increase_bonus { get; set; }// 否 int 对应增加的积分。            
        public int max_increase_bonus { get; set; }//否	int 用户单次可获取的积分上限。            
        public int init_increase_bonus { get; set; }//否 int 初始设置积分。            
        public int cost_bonus_unit { get; set; }//否 int 每使用5积分。            
        public int reduce_money { get; set; }//否  int  抵扣xx元，（这里以分为单位）            
        public int least_money_to_use_bonus { get; set; }// 否 int 抵扣条件，满xx元（这里以分为单位）可用。            
        public int max_reduce_bonus { get; set; }//否 int 抵扣条件，单笔最多使用xx积分。            

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FieldNameType
    {
        FIELD_NAME_TYPE_LEVEL,              //等级
        FIELD_NAME_TYPE_COUPON,        //优惠券                
        FIELD_NAME_TYPE_STAMP,            //印花
        FIELD_NAME_TYPE_DISCOUNT,      //折扣
        FIELD_NAME_TYPE_ACHIEVEMEN,  //成就
        FIELD_NAME_TYPE_MILEAGE ,         //里程
        FIELD_NAME_TYPE_SET_POINTS ,    //集点
        FIELD_NAME_TYPE_TIMS,                //次数
    }

    public abstract class CardInfo
    {
        public CardBaseInfo base_info { get; set; }
        public CardAdvancedInfo advanced_info { get; set; }
    }
    public class CardBaseInfo
    {
        public String logo_url{get;set;}//卡券的商户logo，建议像素为300*300
        public String brand_name{get;set;}//商户名字,字数上限为12个汉字
        public CardCodeType code_type{get;set;}//码型：
        public String title{get;set;}//卡券名，字数上限为9个汉字。(建议涵盖卡券属性、服务及金额)。
        public String sub_title{get;set;}//券名，字数上限为18个汉字。
        public CardColorType color{get;set;}//券颜色。按色彩规范标注填写Color010-Color100
        public String notice{get;set;}//卡券使用提醒，字数上限为16个汉字。
        public String service_phone{get;set;}//非必填 客服电话。
        public String description{get;set;}//卡券使用说明，字数上限为1024个汉字。
        public CardDateInfo date_info{get;set;}//使用日期，有效期的信息。
        public CardSku sku{get;set;}//商品信息。
        public int get_limit{get;set;}//每人可领券的数量限制,不填写默认为50。
        public bool use_custom_code{get;set;}//非必填 是否自定义Code码。填写true或false，默认为false。通常自有优惠码系统的开发者选择自定义Code码，并在卡券投放时带入
        public bool bind_openid{get;set;}//非必填   是否指定用户领取，填写true或false。默认为false。通常指定特殊用户群体投放卡券或防止刷券时选择指定用户领取。
        public bool can_share{get;set;}//非必填 卡券领取页面是否可分享。
        public bool can_give_friend{get;set;}//非必填 卡券是否可转赠
        public List<int> location_id_list{get;set;}//非必填    门店位置poiid。调用POI门店管理接口获取门店位置poiid。具备线下门店的商户为必填。
        public String center_title{get;set;}//非必填 卡券顶部居中的按钮，仅在卡券状态正常(可以核销)时显示
        public String center_sub_title{get;set;}//非必填   显示在入口下方的提示语，仅在卡券状态正常(可以核销)时显示。
        public String center_url{get;set;}//非必填 顶部居中的url，仅在卡券状态正常(可以核销)时显示。
        public String custom_url_name{get;set;}//非必填 自定义跳转外链的入口名字。
        public String custom_url{get;set;}//非必填 自定义跳转的URL。
        public String custom_url_sub_title{get;set;}//非必填 显示在入口右侧的提示语。
        public String promotion_url_name{get;set;}//非必填 营销场景的自定义入口名称。
        public String promotion_url_sub_title{get;set;}//非必填 显示在营销入口右侧的提示语。
        public String promotion_url{get;set;}//非必填  入口跳转外链的地址链接。
        public String source{get;set;}//非必填 第三方来源名，例如同程旅游、大众点评。
        public bool need_push_on_view { get; set; }//填写true为用户点击进入会员卡时推送事件，默认为false。详情见进入会员卡事件推送
    }

    public class CardAdvancedInfo
    {
        public CardUseCondition use_condition { get; set; }//使用门槛（条件）字段，若不填写使用条件则在券面拼写：无最低消费限制，全场通用，不限品类；并在使用说明显示：可与其他优惠共享
        [JsonProperty(PropertyName = "abstract")]
        public CardAbstract cardAbstract { get; set; }//封面摘要结构体名称
        public List<CardTextImage> text_image_list { get; set; }//图文列表，显示在详情内页，优惠券券开发者须至少传入一组图文列表
        public List<CardTimeLimit> time_limit { get; set; }//使用时段限制
        public List<BusinessServiceType> business_service { get; set; }//商家服务类型：可多选
    }

    public class CardAbstract 
    {
        [JsonProperty(PropertyName = "abstract")]
        public String cardAbstract { get; set; }//封面摘要简介。
        public List<String> icon_url_list { get; set; }//封面图片列表，仅支持填入一个封面图片链接，上传图片接口上传获取图片获得链接，填写非CDN链接会报错，并在此填入。建议图片尺寸像素850*350
    }

    public class CardUseCondition
    {
        public String accept_category { get; set; }//指定可用的商品类目，仅用于代金券类型，填入后将在券面拼写适用于xxx
        public String reject_category { get; set; }//指定不可用的商品类目，仅用于代金券类型，填入后将在券面拼写不适用于xxxx
        public bool can_use_with_other_discount { get; set; }//不可以与其他类型共享门槛，填写false时系统将在使用须知里拼写“不可与其他优惠共享”，填写true时系统将在使用须知里拼写“可与其他优惠共享”，默认为true
    }

    public class CardTextImage
    {
        public String image_url{ get; set; }//图片链接，必须调用上传图片接口上传图片获得链接，并在此填入，否则报错
        public String text{ get; set; }//图文描述
    }

    public class CardTimeLimit
    {
        public CardTimeLimitType type { get; set; }//
        public int begin_hour { get; set; }//当前type类型下的起始时间（小时），如当前结构体内填写了MONDAY，此处填写了10，则此处表示周一 10:00可用
        public int end_hour { get; set; }//当前type类型下的结束时间（小时），如当前结构体内填写了MONDAY，此处填写了20，则此处表示周一 10:00-20:00可用
        public int begin_minute { get; set; }//当前type类型下的起始时间（分钟），如当前结构体内填写了MONDAY，begin_hour填写10，此处填写了59，则此处表示周一 10:59可用
        public int end_minute { get; set; }//当前type类型下的结束时间（分钟），如当前结构体内填写了MONDAY，begin_hour填写10，此处填写了59，则此处表示周一 10:59-00:59可用
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BusinessServiceType
    {
        BIZ_SERVICE_DELIVER,// 外卖服务
        BIZ_SERVICE_FREE_PARK,//停车位
        BIZ_SERVICE_WITH_PET,//可带宠物
        BIZ_SERVICE_FREE_WIFI,// 免费wifi
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardTimeLimitType
    {
        MONDAY,// 周一
        TUESDAY,// 周二
        WEDNESDAY,//周三
        THURSDAY,// 周四
        FRIDAY,// 周五
        SATURDAY,// 周六
        SUNDAY,// 周日
        HOLIDAY,//节假日
    }

    public class CardSku
    {
        public long quantity { get; set; }
    }

    public class CardDateInfo
    {
        public CardDateType type{get;set;}//DATE_TYPE_FIX_TIME_RANGE表示固定日期区间，DATE_TYPE_FIX_TERM表示固定时长（自领取后按天算
        public long begin_timestamp{get;set;}//ype为DATE_TYPE_FIX_TIME_RANGE时专用，表示起用时间。从1970年1月1日00:00:00至起用时间的秒数，最终需转换为字符串形态传入。（东八区时间，单位为秒）
        public long end_timestamp{get;set;}//表示结束时间，建议设置为截止日期的23:59:59过期。（东八区时间，单位为秒）
        public int fixed_term{get;set;}//type为DATE_TYPE_FIX_TERM时专用，表示自领取后多少天内有效，不支持填写0。
        public int fixed_begin_term{get;set;}//type为DATE_TYPE_FIX_TERM时专用，表示自领取后多少天开始生效，领取后当天生效填写0。（单位为天）
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardDateType
    {
        DATE_TYPE_FIX_TIME_RANGE,
        DATE_TYPE_FIX_TERM,
        DATE_TYPE_PERMANENT,//永久有效
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardType
    {
        GROUPON,//团购券
        CASH,//代金券
        DISCOUNT,//折扣券
        GIFT,//兑换券
        GENERAL_COUPON,//优惠券
        MEMBER_CARD,//会员卡
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardColorType
    {
        Color010,//	#63b359
        Color020,//	#2c9f67
        Color030,//	#509fc9
        Color040,//	#5885cf
        Color050,//	#9062c0
        Color060,//	#d09a45
        Color070,//	#e4b138
        Color080,//	#ee903c
        Color081,//	#f08500
        Color082,//	#a9d92d
        Color090,//	#dd6549
        Color100,//	#cc463d
        Color101,//	#cf3e36
        Color102,//	#5E6671
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardCodeType
    {
        CODE_TYPE_TEXT,//文本；
        CODE_TYPE_BARCODE,//一维码
        CODE_TYPE_QRCODE,//二维码
        CODE_TYPE_ONLY_QRCODE,//二维码无code显示；
        CODE_TYPE_ONLY_BARCODE,//一维码无code显示;
        CODE_TYPE_NONE,//不显示code和条形码类型
    }
}
