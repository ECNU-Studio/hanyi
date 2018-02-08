var QiNiu_Domain = "http://oxr4xprx6.bkt.clouddn.com/";
var UploadUrl = "http://www.nengli8.com/Service/FileUpload";
var UploadUrlImage = "http://www.nengli8.com/Service/ImageUpload";

var UploadCss = {
    swf: '/assets/plugins/uploadify/uploadify.swf',
    cancelImg: '/assets/plugins/uploadify/uploadify-cancel.png'
};

/**
 * [isMobile 判断平台]
 * @param test: 0:iPhone    1:Android
 */
function ismobile(test) {
    var u = navigator.userAgent, app = navigator.appVersion;
    if (/AppleWebKit.*Mobile/i.test(navigator.userAgent) || (/MIDP|SymbianOS|NOKIA|SAMSUNG|LG|NEC|TCL|Alcatel|BIRD|DBTEL|Dopod|PHILIPS|HAIER|LENOVO|MOT-|Nokia|SonyEricsson|SIE-|Amoi|ZTE/.test(navigator.userAgent))) {
        if (window.location.href.indexOf("?mobile") < 0) {
            try {
                if (/iPhone|mac|iPod|iPad/i.test(navigator.userAgent)) {
                    return '0';
                } else {
                    return '1';
                }
            } catch (e) { }
        }
    } else if (u.indexOf('iPad') > -1) {
        return '0';
    } else {
        return '1';
    }
};

/**
* 设置cookies
* @param {String} key 
* @param {String} value 
*/
var SetCookie = function (key, value) {
    var argv = SetCookie.arguments;
    var argc = SetCookie.arguments.length;
    var expires = (argc > 2) ? argv[2] : null;
    if (expires != null)  {
        var LargeExpDate = new Date();
        LargeExpDate.setTime(LargeExpDate.getTime() + (expires * 1000 * 3600 * 24));
    }
    document.cookie = key + "=" + encodeURIComponent(value) + ((expires == null) ? "" : ("; expires=" + LargeExpDate.toGMTString()));
};


/**
* 获取Cookie对应值
* @param {String} key
* @return {String}
*/
var GetCookie = function (key) {
    var arr = document.cookie.match(new RegExp("(^| )" + key + "=([^;]*)(;|$)"));
    if (arr != null) {
        return decodeURIComponent(arr[2]);
    } else {
        return null;
    }
};

/*判断是否是手机号码*/
var validatemobile = function (mobile) {
    if (mobile.length == 0) {
        return false;
    }
    if (mobile.length != 11) {
        return false;
    }

    var myreg = /^((13[0-9]|14[5|7]|15[0-9]|18[0-9]|17[0-9])[0-9]{8})$/;
    if (!myreg.test(mobile)) {
        return false;
    }
    return true;
}

var validatePhone = function (phone) {
    if (phone.length == 0) {
        return false;
    }
    var myreg = /^\d{3,4}-?\d{6,8}$/;
    if (!myreg.test(phone)) {
        return false;
    }
    return true;
}

/*判断邮箱地址*/
var isEmail = function (str) {
    var reg = /^([a-zA-Z0-9_-])+([.][a-zA-Z0-9_-]+)*@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
    if (!reg.test(str)) {
        return false;
    }
    return true;
}

var showloading = function (text) {
    if (typeof (text) == "undefined" || text == null) {
        text = "保存中...";
    }
    $("#beforebox-loading div").text(text);
    $("#beforebox-overlay").show();
}

var hideloading = function () {
    $("#beforebox-overlay").hide();
}

function IsURL(str_url) {
    var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
    + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //ftp的user@
    + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184
    + "|" // 允许IP和DOMAIN（域名）
    + "([0-9a-z_!~*'()-]+\.)*" // 域名- www.
    + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名
    + "[a-z]{2,6})" // first level domain- .com or .museum
    + "(:[0-9]{1,4})?" // 端口- :80
    + "((/?)|" // a slash isn't required if there is no file name
    + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
    var re = new RegExp(strRegex);
    //re.test()
    if (re.test(str_url)) {
        return (true);
    } else {
        return (false);
    }
}
/*验证是否为带小数点的数字*/
function IsDoubleNum(str) {
    var strRegex = /^[0-9]\d*[.]?\d*$/;
    if (strRegex.test(str)) {
        return (true);
    } else {
        return (false);
    }
}
/*验证是否为整数*/
function IsInt(str) {
    var strRegex = /^[1-9]\d*$|^0$/;
    if (strRegex.test(str)) {
        return (true);
    } else {
        return (false);
    }
}

/*验证日期*/
function IsDate(str) {
    var strRegex = /^[1-2][0-9]{3}-[0-1][1-9]-[0-3][0-9]$/;
    //var re = new RegExp(strRegex);
    if (strRegex.test(str)) {
        return (true);
    } else {
        return (false);
    }
}
/*验证时间*/
function IsTime(str) {
    var strRegex = "/^[0-5]{0,1}[0-9]:[0-5]{0,1}[0-9]$/";
    if (strRegextest(str)) {
        return (true);
    } else {
        return (false);
    }
}
/*判断字符串是否为字母或数字*/
function checknum(value) {
    var Regx = /^[A-Za-z0-9]*$/;
    if (Regx.test(value)) {
        return true;
    }
    else {
        return false;
    }
}

function RepalceSpecialChar(s) {
    var pattern = new RegExp("[`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）——|{}【】‘；：”“'。，、？]")
    var rs = "";
    for (var i = 0; i < s.length; i++) {
        rs = rs + s.substr(i, 1).replace(pattern, '');
    }
    return rs;
}

/*导出表格*/
function ExportToExcel(tableid, tablename) {
    var table = document.getElementById(tableid);
    if (!table) {
        return;
    }

    var data = "<table border=\"1\">" + table.innerHTML + "</table>";

    var objBody = document.getElementsByTagName("body").item(0);

    //var objExpTemp = $('objExpTemp');
    var objExpTemp = document.getElementById("objExpTemp");

    if (!objExpTemp) {

        objExpTemp = document.createElement("iframe");
        objExpTemp.setAttribute('id', 'objExpTemp');
        objExpTemp.style.display = 'none';
        objExpTemp.src = 'about:blank';
        objBody.appendChild(objExpTemp);
    }

    var ExcelForm = objExpTemp.contentWindow.document.forms['ExcelForm'];
    if (!ExcelForm) {
        objExpTemp.contentWindow.document.write('<div style="display:none"><form name=ExcelForm><input id="expContent" name="content" type="text" /><input id="expfileName" name="fileName" type="text" /></form></div>');
    }
    var ExcelForm = objExpTemp.contentWindow.document.forms['ExcelForm'];
    var txtData = objExpTemp.contentWindow.document.getElementById('expContent');
    var txtFilename = objExpTemp.contentWindow.document.getElementById('expfileName');
    txtData.value = data;
    txtFilename.value = escape(tablename);
    ExcelForm.action = '../../ExcelTransfer/index';
    ExcelForm.method = 'POST';

    ExcelForm.submit();

    return;
}
//js时间比较(yyyy-mm-dd hh:mi:ss),前面比后面的时间大返回false,后面比前面时间大则返回true
function compareTime(startDate, endDate) {
    if (startDate.length > 0 && endDate.length > 0) {
        var startDateTemp = startDate.split(" ");
        var endDateTemp = endDate.split(" ");

        var arrStartDate = startDateTemp[0].split("-");
        var arrEndDate = endDateTemp[0].split("-");

        var arrStartTime = startDateTemp[1].split(":");
        var arrEndTime = endDateTemp[1].split(":");

        var allStartDate = new Date(arrStartDate[0], arrStartDate[1], arrStartDate[2], arrStartTime[0], arrStartTime[1], arrStartTime[2]);
        var allEndDate = new Date(arrEndDate[0], arrEndDate[1], arrEndDate[2], arrEndTime[0], arrEndTime[1], arrEndTime[2]);

        if (allStartDate.getTime() > allEndDate.getTime()) {
            return false;
        } else {
            return true;
        }
    } else {
        alert("时间不能为空");
        return false;
    }
}
//获取当前的日期时间 格式“yyyy-MM-dd HH:MM:SS”
function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + date.getHours() + seperator2 + date.getMinutes()
            + seperator2 + date.getSeconds();
    return currentdate;
}
//js时间相差(yyyy-mm-dd)
function compareDate(startDate, endDate) {
    if (startDate.length > 0 && endDate.length > 0) {
        var startDateTemp = startDate.split(" ");
        var endDateTemp = endDate.split(" ");

        var arrStartDate = startDateTemp[0].split("-");
        var arrEndDate = endDateTemp[0].split("-");

        var day1 = (new Date).setFullYear(arrStartDate[0], arrStartDate[1], arrStartDate[2]);
        var day2 = (new Date).setFullYear(arrEndDate[0], arrEndDate[1], arrEndDate[2]);
        var number_of_days = (day2 - day1) / 86400000;

        return number_of_days;

    } else {
        alert("时间不能为空");
        return false;
    }
}

/* 新版页面信息弹出提示  2s自动消失 */
var showMsg = function (text, timeout) {

    if (typeof (text) == "undefined" || text == null) {
        text = "暂未进行任何操作";
    }
    $("#load_shows p").text(text);
    $("#load_shows").show();
    var divWidth = $(".load_loading").width();
    var winWidth = window.innerWidth;
    var marginleft = (parseInt(winWidth) - parseInt(divWidth + 40)) / 2;
    $("#load_shows .load_loading").css("margin-left", marginleft);
    var time = 1500;
    if (timeout != undefined && !isNaN(timeout)) {
        time = timeout;
    }
    $("#load_shows").fadeOut(time, "swing");
};

function checkUrl(url) {
    var strRegex = "^((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?$";
    var re = new RegExp(strRegex);
    //re.test()
    if (re.test(url)) {
        return (true);
    } else {
        return (false);
    }

}
/*objid为需要提示的文本框ID，tip为提示的内容*/
function ShowErrorTip(objid, tip, tag) {
    if ($("#" + objid).length <= 0) {
        return;
    }
    if (tip == "") {
        tip = "请输入";
    }
    var html = '<div class="error_box" id="' + objid + '_error">';
    html += '<div class="error_box_left"></div>';
    html += '<div class="error_text"><span>' + tip + '</span><a href="javascript:void(0);" onclick="$(this).parent().parent().remove();"></a></div>';
    html += ' </div>';

    $("#header").after(html);
    var error_box = $('#' + objid + '_error');
    var position = $("#" + objid).offset();
    if (tag) {
        position.left += 3;
        position.top = $("#" + objid).closest("li").find(".title").offset().top;
    }

    error_box.width($('#' + objid + '_error .error_box_left').width() + $('#' + objid + '_error .error_text').width() + 40);
    error_box.css({ "left": (position.left + $("#" + objid).width() + 20) + "px", "top": position.top + "px" });
    return;
}
/*obj为需要提示的文本框，tip为提示的内容*/
function ShowErrorTipObj(obj, tip, tag) {
    var $obj = obj;
    if ($obj.length <= 0) {
        return;
    }
    if (tip == "") {
        tip = "请输入";
    }

    var objid = $obj.attr("id");
    if (objid == undefined || objid == "") {
        for (var i = 0; i < 100; i++) {
            objid = "random_" + (Math.random() * 100).toFixed(0);
            if (!$("#" + objid).length) {
                $obj.attr("id", objid);
                break;
            }
        }
    }

    var html = '<div class="error_box" id="' + objid + '_error">';
    html += '<div class="error_box_left"></div>';
    html += '<div class="error_text"><span>' + tip + '</span><a href="javascript:void(0);" onclick="$(this).parent().parent().remove();"></a></div>';
    html += ' </div>';

    $("#header").after(html);
    var error_box = $('#' + objid + '_error');
    var position = $("#" + objid).offset();
    if (tag) {
        position.left += 3;
        position.top = $("#" + objid).closest("li").find(".title").offset().top;
    }

    error_box.width($('#' + objid + '_error .error_box_left').width() + $('#' + objid + '_error .error_text').width() + 40);
    var widthO = 20;
    if ($("#" + objid).parent(".nice-select").length) {
        widthO = 40;
    }
    error_box.css({ "left": (position.left + $("#" + objid).width() + widthO) + "px", "top": position.top + "px" });
    return;
}
function showerrorwarn() {
    var html = '<div class="add-top-error"><span>相关信息没有填写完整，请您按照提示完善相关内容！</span></div>';
    $("#erroe-area").html(html);
    $(".add-top-error").fadeIn(1000);
    $(".add-top-error").fadeOut(500);
}
/*关闭所有错误提示*/
function CloseAllErrorTip() {
    $(".error_box").remove();
}
//验证身份证号
var validateCard = function (idcard) {
    var Errors = new Array("ok", "身份证号码位数不对!", "身份证号码出生日期超出范围或含有非法字符!", "身份证号码校验错误!", "身份证地区非法!");
    var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "xinjiang", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
    var idcard, Y, JYM;
    var S, M;
    var idcard_array = new Array();
    idcard_array = idcard.toUpperCase().split("");
    if (area[parseInt(idcard.substr(0, 2))] == null) return Errors[4];
    switch (idcard.length) {
        case 15:
            if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/;//测试出生日期的合法性 
            }
            else {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/;//测试出生日期的合法性 
            }
            if (ereg.test(idcard))
                return Errors[0];
            else
                return Errors[2];
            break;
        case 18:
            if (parseInt(idcard.substr(6, 4)) % 4 == 0 || (parseInt(idcard.substr(6, 4)) % 100 == 0 && parseInt(idcard.substr(6, 4)) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/;//闰年出生日期的合法性正则表达式 
            }
            else {
                ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/;//平年出生日期的合法性正则表达式 
            }
            if (ereg.test(idcard)) {
                S = (parseInt(idcard_array[0]) + parseInt(idcard_array[10])) * 7 + (parseInt(idcard_array[1]) + parseInt(idcard_array[11])) * 9 + (parseInt(idcard_array[2]) + parseInt(idcard_array[12])) * 10 + (parseInt(idcard_array[3]) + parseInt(idcard_array[13])) * 5 + (parseInt(idcard_array[4]) + parseInt(idcard_array[14])) * 8 + (parseInt(idcard_array[5]) + parseInt(idcard_array[15])) * 4 + (parseInt(idcard_array[6]) + parseInt(idcard_array[16])) * 2 + parseInt(idcard_array[7]) * 1 + parseInt(idcard_array[8]) * 6 + parseInt(idcard_array[9]) * 3;
                Y = S % 11;
                M = "F";
                JYM = "10X98765432";
                M = JYM.substr(Y, 1);
                if (M == idcard_array[17])
                    return Errors[0];
                else
                    return Errors[3];
            }
            else
                return Errors[2];
            break;
        default:
            return Errors[1];
            break;
    }
}
//对电子邮件的验证
var validateEmail = function (email) {
    var reg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
    if (!reg.test(email)) {
        return false;
    }
    return true;
}
//判断银行卡号
function luhmCheck(bankno) {
    var lastNum = bankno.substr(bankno.length - 1, 1);//取出最后一位（与luhm进行比较）

    var first15Num = bankno.substr(0, bankno.length - 1);//前15或18位
    var newArr = new Array();
    for (var i = first15Num.length - 1; i > -1; i--) {    //前15或18位倒序存进数组
        newArr.push(first15Num.substr(i, 1));
    }
    var arrJiShu = new Array();  //奇数位*2的积 <9
    var arrJiShu2 = new Array(); //奇数位*2的积 >9

    var arrOuShu = new Array();  //偶数位数组
    for (var j = 0; j < newArr.length; j++) {
        if ((j + 1) % 2 == 1) {//奇数位
            if (parseInt(newArr[j]) * 2 < 9)
                arrJiShu.push(parseInt(newArr[j]) * 2);
            else
                arrJiShu2.push(parseInt(newArr[j]) * 2);
        }
        else //偶数位
            arrOuShu.push(newArr[j]);
    }

    var jishu_child1 = new Array();//奇数位*2 >9 的分割之后的数组个位数
    var jishu_child2 = new Array();//奇数位*2 >9 的分割之后的数组十位数
    for (var h = 0; h < arrJiShu2.length; h++) {
        jishu_child1.push(parseInt(arrJiShu2[h]) % 10);
        jishu_child2.push(parseInt(arrJiShu2[h]) / 10);
    }

    var sumJiShu = 0; //奇数位*2 < 9 的数组之和
    var sumOuShu = 0; //偶数位数组之和
    var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
    var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
    var sumTotal = 0;
    for (var m = 0; m < arrJiShu.length; m++) {
        sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
    }

    for (var n = 0; n < arrOuShu.length; n++) {
        sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
    }

    for (var p = 0; p < jishu_child1.length; p++) {
        sumJiShuChild1 = sumJiShuChild1 + parseInt(jishu_child1[p]);
        sumJiShuChild2 = sumJiShuChild2 + parseInt(jishu_child2[p]);
    }
    //计算总和
    sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

    //计算Luhm值
    var k = parseInt(sumTotal) % 10 == 0 ? 10 : parseInt(sumTotal) % 10;
    var luhm = 10 - k;

    if (lastNum == luhm) {
        $("#banknoInfo").html("Luhm验证通过");
        return true;
    }
    else {
        $("#banknoInfo").html("银行卡号必须符合Luhm校验");
        return false;
    }
}

/*字符长度*/
function strlen(str) {
    var len = 0;
    for (var i = 0; i < str.length; i++) {
        var c = str.charCodeAt(i);
        //单字节加1 
        if ((c >= 0x0001 && c <= 0x007e) || (0xff60 <= c && c <= 0xff9f)) {
            len++;
        }
        else {
            len += 2;
        }
    }
    return len;
}

function leftToRight() {
    if ($("#left_menu").length > 0) {
        setTimeout(function () {
            var height = window.innerHeight - 72;
            var rheight = $("#web_content").height();
            var lheight = $("#left_menu").height();
            if (rheight < height) {
                $("#web_content").height(height);
                $("#left_menu").height(height - 45);
            } else if (lheight < rheight) {
                $("#left_menu").height(rheight - 45);
            }
        }, 200);
    }
}

$(function () {

    $(".js-menu,.js-menu-arrow").bind('click', function (e) {
        e.stopPropagation();
        var $this = $(this).closest("li"), item = $this.attr("data-item"), $ul = $("#left_menu");
        if ($this.find("a.js-menu-arrow").hasClass("arrow")) {
            $this.find("a.js-menu-arrow").removeClass("arrow").addClass("arrow_t");
            $ul.find("[data-item=\"" + item + "_sub\"]").slideUp(300);
        }
        else {
            $ul.find("a.js-menu-arrow").removeClass("arrow").addClass("arrow_t");
            $ul.find(".sub").slideUp(300);
            $this.find("a.js-menu-arrow").removeClass("arrow_t").addClass("arrow");
            $ul.find("[data-item=\"" + item + "_sub\"]").slideDown(300);
        }
    });


    $("input.vali-IntN").blur(function () {
        if (!IsInt($(this).val())) {
            $(this).val("");
            return;
        }
    });
    $("input.vali-DoubleN").blur(function () {
        if (!IsDoubleNum($(this).val())) {
            $(this).val("");
            return;
        }
    });
    $("input.vali-DoubleEmpty").blur(function () {
        if ($(this).val() != "") {
            if (!IsDoubleNum($(this).val())) {
                $(this).val("");
                return;
            }
        }
    });
});


//如果有两个日期控件，则会【有】日期控制方面的限制。如果只有一个日期控件，则第二个参数传【null】，则【没有】日期控制方面的限制
function InitDate(dateid_start, dateid_end) {
    if (dateid_start != null && $("#" + dateid_start).length > 0 && dateid_end != null && $("#" + dateid_end).length > 0) {
        //如果两个ID都存在

        $("#" + dateid_start).datepicker({
            dateFormat: "yy-mm-dd",
            onSelect: function (dateText, inst) {
                var arys = new Array();
                var arys = dateText.split('-');
                $("#" + dateid_end).datepicker('option', 'minDate', new Date(arys[0], arys[1] - 1, arys[2]));
            },
            onClose: function (dateText, inst) {
                if (dateText == "") {
                    $("#" + dateid_end).datepicker('option', 'minDate', null);
                }
            }
        });
        $("#" + dateid_end).datepicker({
            dateFormat: "yy-mm-dd",
            onSelect: function (dateText, inst) {
                var arys = new Array();
                var arys = dateText.split('-');
                $("#" + dateid_start).datepicker('option', 'maxDate', new Date(arys[0], arys[1] - 1, arys[2]));
            },
            onClose: function (dateText, inst) {
                if (dateText == "") {
                    $("#" + dateid_start).datepicker('option', 'maxDate', null);
                }
            }
        });
    }
    else if (dateid_start != null && $("#" + dateid_start).length > 0) {
        $("#" + dateid_start).datepicker({
        });
    }
    else if (dateid_end != null && $("#" + dateid_end).length > 0) {
        $("#" + dateid_end).datepicker({
        });
    }
}

var $ajax = function (url, data, successfn, dataType, type, errorfn) {
    var textStr = "参数有误",
        _url = (url == null || url == "" || typeof (url) == "undefined") ? "" : url,
        _type = (type == null || type == "" || typeof (type) == "undefined") ? "post" : type,
    _dataType = (dataType == null || dataType == "" || typeof (dataType) == "undefined") ? "json" : dataType,
    _data = (data == null || data == "" || typeof (data) == "undefined") ? { "date": new Date().getTime() } : data,
    _contentType = "application/json";

    if (_url == "") {
        alert("url" + textStr);
        return;
    }

    if (_dataType.toLowerCase() == "html") {
        _contentType = "application/x-www-form-urlencoded; charset=utf-8";
    }

    var isjson = (typeof (_data) == "object" && Object.prototype.toString.call(_data).toLowerCase() == "[object object]" && !_data.length);
    if (isjson) {
        _data = JSON.stringify(_data);
    }

    $.ajax({
        type: _type,
        data: _data,
        url: _url,
        dataType: _dataType,
        contentType: _contentType,
        success: function (d) {
            if (Object.prototype.toString.call(successfn) === '[object Function]') {
                successfn(d);
            }
        },
        error: function (e) {
            if (Object.prototype.toString.call(errorfn) === '[object Function]') {
                errorfn(e);
            } else {
                hideloading();
            }
        }
    });

}

$validate = function (options) {
    var opt = options || { "select": "#form" }, isOk = true;
    if (opt.select == "") {
        return;
    }
    var $ele = $(opt.select), $inputGroup = $ele.find("input,textarea").not("[type='checkbox']").not("[type='radio']").not("[data-no='no']");
    CloseAllErrorTip();
    var reslut = "";
    //vali-作为验证class的前缀，与其他的样式区别
    //vali-Required:必填;vali-Number:数字;vali-Double:大于零的数;
    //vali-Int:整数;vali-Numorletter:数字或者字母;vali-Email:邮箱;
    //vali-Phone:手机号码;vali-Url:网址;
    var valiArr = ['vali-Required', 'vali-Number', 'vali-Double', 'vali-Int', 'vali-Numorletter', 'vali-Email', 'vali-Phone'];
    valiArr.push('vali-Url');
    var valiDic = {};
    valiDic['vali-Required'] = "不能为空！";
    valiDic['vali-Number'] = "必须为数字！";
    valiDic['vali-Double'] = "必须为大于零的数！";
    valiDic['vali-Int'] = "必须为整数！";
    valiDic['vali-Numorletter'] = "必须为数字或者字母！";
    valiDic['vali-Email'] = "邮箱格式不正确！";
    valiDic['vali-Phone'] = "手机号码格式不正确！";
    valiDic['vali-Url'] = "请输入正确的网址！";
    $inputGroup.each(function (index, item) {
        var $this = $(this), id = $this.attr("id"), val = $.trim($this.val()), len = $this.attr("data-len"), dvalue = $this.attr("data-value");
        if (val == undefined) {
            return true;
        }
        val = val.replace(/\r\n/g, "{换行}").replace(/\n/g, "{换行}").replace(/\r/g, "{换行}");
        val = val.replace(/\"/g, "'");
        var className = $this.attr("class");
        if (className != undefined && className != "" && className.indexOf("inputselect") > -1) {
            val = val.replace("请选择", "").replace("不限", "");
        }
        var isContinue = true;
        var arrLength = valiArr.length;
        for (var i = 0; i < arrLength; i++) {
            if (className == undefined || className == "" || className.indexOf('vali-') < 0) {
                break;
            }
            var item = valiArr[i];
            if (isContinue == false) {
                break;
            }
            if ($this.hasClass(item)) {
                var strText = "不能为空！";
                if (valiDic[item] != undefined && valiDic[item] != "") {
                    strText = valiDic[item];
                }
                switch (item) {
                    case "vali-Required":
                        if (val == "") {
                            if ($this.attr("type") == "hidden") {
                                var $news = $("#" + $this.attr("data-for"));
                                if ($news != undefined && $news.length > 0) {
                                    ShowErrorTipObj($news, strText);
                                }
                            } else {
                                ShowErrorTipObj($this, strText);
                            }
                            isOk = false;
                            isContinue = false;
                        }
                        break;
                    case "vali-Number":
                        if (val != "" && !Number(val)) {
                            ShowErrorTipObj($this, strText);
                            isOk = false;
                            isContinue = false;
                        }
                        break;
                    case "vali-Double":
                        if (val != "" && (!IsDoubleNum(val) || parseFloat(val) == 0)) {
                            ShowErrorTipObj($this, strText);
                            isOk = false;
                            isContinue = false;
                        }
                        break;
                    case "vali-Int":
                        if (val != "" && (!IsInt(val) || parseFloat(val) == 0)) {
                            ShowErrorTipObj($this, strText);
                            isOk = false;
                            isContinue = false;
                        }
                        break;
                    case "vali-Numorletter":
                        if (val != "" && !checknum(val)) {
                            ShowErrorTipObj($this, strText);
                            isOk = false; isContinue = false;
                        }
                        break;
                    case "vali-Email":
                        if (val != "" && !isEmail(val)) {
                            ShowErrorTipObj($this, strText);
                            isOk = false; isContinue = false;
                        }
                        break;
                    case "vali-Phone":
                        if (val != "" && !validatemobile(val)) {
                            ShowErrorTipObj($this, strText);
                            isOk = false; isContinue = false;
                        }
                        break;
                    case "vali-Url":
                        if (val != "" && !checkUrl(val)) {
                            ShowErrorTipObj($this, strText);
                            isOk = false;
                        }
                        break;

                }
            }
        }

        if (isOk) {
            reslut += '"' + id + '":"' + (val == "" ? '' : val) + '",';
            if (dvalue != undefined) {
                reslut += '"' + id + '_dvalue":"' + (dvalue == "" ? '' : $.trim(dvalue)) + '",';
            }


        }
    });

    reslut += '"isOK":' + isOk + ",";
    if (reslut != "") {
        reslut = reslut.substring(0, reslut.length - 1);
    }
    if (isOk == false) {
        showerrorwarn();
    }
    var str = ("{" + reslut + "}");
    res = JSON.parse(str);
    //定位到第一个报错点
    //$(document).scrollTop($(".error_text").first().position().top);
    return res;
};

/*操作成功面板
调用方式
1、当没有操作时，就是操作成功的提示，则showok();
2、一般来说，操作成功以后会回调函数，
比如刷新当前页面,则showok(null,function(){location.reload();})
,或showok("操作成功!",function(){location.reload();})
3、如果没有回调函数，则直接可以showok("保存成功");
*/
var showok = function (message, callback) {
    var $ok = $("#okBox");
    message = message || '操作成功！';
    callback = callback || function () { };
    if ($ok.length > 0) {
        $ok.remove();
    }
    var html = '';
    html += '<div class="add_frame_panel" id="okBox" style="display:block;z-index:9999;">';
    html += '<div class="add_frame_box" style="width: 300px; left: 40%; top: 25%;">';
    html += '<div class="add_frame_close"></div>';
    html += ' <div class="okbox">' + message + '</div>';
    html += '<div class="btn_frame">';
    html += '<div class="cancel btn"><a href="javascript:void(0)">知道了</a></div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    $("body").append(html);
    $("#okBox").show();
    $("#okBox .add_frame_close,#okBox .cancel").bind('click', function () {
        $('#okBox').hide();
        callback();
    });
}

var showokMessage = function (message, lookcallback, callback,text) {
    var $ok = $("#okBoxMessage");
    message = message || '操作成功！';
    lookcallback = lookcallback || function () { };
    callback = callback || function () { };
    text = text || "查看项目";
    if ($ok.length > 0) {
        $ok.remove();
    }
    var html = '';
    html += '<div class="add_frame_panel" id="okBoxMessage" style="display:block;z-index:9999;">';
    html += '<div class="add_frame_box" style="width: 300px; left: 40%; top: 25%;">';
    html += '<div class="add_frame_close"></div>';
    html += ' <div class="okboxMessage">' + message + '</div>';
    html += '<div class="btn_frame" style="background-color:#fff;">';
    html += '<div class="look btn"><a href="javascript:void(0)">' + text + '</a></div>';
    html += '<div class="cancel btn mg-left-15"><a href="javascript:void(0)">留在此页</a></div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    $("body").append(html);
    $("#okBoxMessage").show();
    $("#okBoxMessage .cancel").bind('click', function () {
        $('#okBoxMessage').hide();
        callback();
    });
    $("#okBoxMessage .add_frame_close").bind('click', function () {
        $('#okBoxMessage').hide();
    });
    $("#okBoxMessage .look").bind('click', function () {
        $('#okBoxMessage').hide();
        lookcallback();
    });
}

/*操作成功面板
调用方式
showerror("操作失败!")
目前z-index最高。对应的errorbox为z-index:10000
*/
var showerror = function (message, callback) {
    var $ok = $("#okBox");
    message = message || '操作失败！';
    callback = callback || function () { };
    if ($ok.length > 0) {
        $ok.remove();
    }
    var html = '';
    html += '<div class="add_frame_panel" id="okBox" style="display:block;z-index:99999;">';
    html += '<div class="add_frame_box" style="width: 300px; left: 40%; top: 25%;">';
    html += '<div class="add_frame_close"></div>';
    html += ' <div class="errorbox">' + message + '</div>';
    html += '<div class="btn_frame">';
    html += '<div class="cancel btn"><a href="javascript:void(0)">知道了</a></div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    $("body").append(html);
    $("#okBox").show();
    $("#okBox .add_frame_close,#okBox .cancel").bind('click', function () {
        $('#okBox').hide();
        callback();
    });
}