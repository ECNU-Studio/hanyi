/*
    **select控件初始化
    **界面加载后绑定显示/隐藏事件
    **by wulefu@20161027
    */
var $nice_select_ul = $('.nice-select ul');
if ($nice_select_ul.length > 0) {
    var $nice_select_li = $('.nice-select li');
    var $nice_select_input = $('.nice-select .inputselect');
    $nice_select_input.bind('click', function (e) {
        $nice_select_ul.hide();
        var $this = $(this);
        $this.parent().find('ul').show();
        e.stopPropagation();
    });
    $nice_select_li.bind('click', function (e) {
        var $this = $(this);
        $this.siblings().removeClass("active");
        $this.addClass("active");
        var text = $this.text();
        var val = $this.attr("data-value");
        var $input = $this.parent().parent().find('input');
        $input.val(text);
        $input.attr("data-text", text);
        $input.attr("data-value", val);
        $this.parent().hide();
        e.stopPropagation();
    });
    $(document).bind('click', function (e) {
        $nice_select_ul.hide();
    });
}

/*****check组初始化函数******开始******/
//全选按钮，后添加的checkbox也有此点击事件
$(document).on('click', ".js-check-all", function (e) {
    var $thischeck = $(this), name = $thischeck.attr("name"), $parentDIV = $thischeck.closest("div");
    var $list = $("input[name=" + name + "]").not(".js-check-all").not(":disabled");
    var $valInput = $parentDIV.find("input[type='hidden']");
    if (!$valInput.length) {
        var inputStr = "<input type='hidden' id='" + name + "'>";
        $parentDIV.append(inputStr);
        $valInput = $parentDIV.find("input[type='hidden']");
    }
    var value = "";
    var Data_value = "";
    var b = $thischeck.is(":checked");
    if (b) {
        $list.prop("checked", true);
        $list.each(function () {
            var $this = $(this), htmlText = $this.next(".rctext").html(), val = $this.next(".rctext").attr("data-value");
            value += htmlText + ",";
            if (val != undefined) {
                Data_value += val + ",";
            }
        });
    } else {
        $list.prop("checked", false);
    }
    if (value != "") {
        value = value.substring(0, value.length - 1);
        if (Data_value != "") {
            Data_value = Data_value.substring(0, Data_value.length - 1);
        }
    }
    $valInput.val(value);
    $valInput.attr("data-value", Data_value);
});
//反选全选按钮，后添加的checkbox也有此点击事件
$(document).on('click', ".checkbox", function (e) {
    var $thischeck = $(this);
    if ($thischeck.hasClass("js-check-all")) {
        return;//如果当前为全选按钮，则由上面的函数执行即可
    } else {
        var b = $thischeck.is(":checked");
        var name = $thischeck.attr("name");
        if (b)//从测试来看，选中判断会延后，也就是说，从没有选中到选中时，此时还为没有选中
        {
            //如果从选中到没选中
            //console.log($("input[name=" + name + "]:checked").length);
            //console.log(Math.ceil($("input[name=" + name + "]:checked").length / 2) + 1);
            //console.log($("input[name=" + name + "]:not(:disabled)").length);
            //console.log($("input[name=" + name + "]").length);
            var $tableLayout = $(".tableLayout_wrap");
            if ($tableLayout.find("#MyTable_tableColumn").length) {
                //if ((Math.ceil($("input[name=" + name + "]:checked").length / 2) + 1) == Math.ceil($("input[name=" + name + "]:not(:disabled)").length / 2)) {
                if (($("input[name=" + name + "]:checked").length + 1) == $("input[name=" + name + "]:not(:disabled)").length) {
                    $(".js-check-all[name=" + name + "]").prop("checked", true);
                }
            } else {
                if (($("input[name=" + name + "]:checked").length + 1) == $("input[name=" + name + "]:not(:disabled)").length) {
                    $(".js-check-all[name=" + name + "]").prop("checked", true);
                }
            }

        } else {
            //从没选中改为选中时,判断是否只有全选没选中
            $(".js-check-all[name=" + name + "]").prop("checked", false);
        }

        var $parentDIV = $thischeck.closest("div"), $valInput = $parentDIV.find("input[type='hidden']"), $list = $("input[name=" + name + "]:checked").not(".js-check-all");

        if (!$valInput.length) {
            var inputStr = "<input type='hidden' id='" + name + "'>";
            $parentDIV.append(inputStr);
            $valInput = $parentDIV.find("input[type='hidden']");
        }
        var value = "";
        var Data_value = "";
        if ($list.length > 0) {
            $list.each(function () {
                var $this = $(this), htmlText = $this.next(".rctext").html(), val = $this.next(".rctext").attr("data-value");
                value += htmlText + ",";
                if (val != undefined) {
                    Data_value += val + ",";
                }
            });
        }
        if (value != "") {
            value = value.substring(0, value.length - 1);
            if (Data_value != "") {
                Data_value = Data_value.substring(0, Data_value.length - 1);
            }
        }
        $valInput.val(value);
        $valInput.attr("data-value", Data_value);

    }
});
/*check组初始化函数***********结束*/

var $radiobox = $(".regular-radio");
if ($radiobox.length > 0) {
    $radiobox.bind('click', function (e) {
        var $thisradio = $(this), name = $thisradio.attr("name"), data_value = $thisradio.next(".rctext").attr("data-value"), $parentDIV = $thisradio.closest("div"), $list = $("input[name=" + name + "]");
        var $valInput = $parentDIV.find("input[type='hidden']");
        if (!$valInput.length) {
            var inputStr = "<input type='hidden' id='" + name + "'>";
            $parentDIV.append(inputStr);
            $valInput = $parentDIV.find("input[type='hidden']");
        }
        var value = $thisradio.nextAll('.rctext').first().html();
        $valInput.val(value);
        $valInput.attr("data-value", data_value);
    });
}
/*
    以下为自定义控件实现
*/


var Prototype = {
    Version: '1.4.0',
    ScriptFragment: '(?:<script.*?>)((\n|\r|.)*?)(?:<\/script>)',

    emptyFunction: function () { },
    K: function (x) { return x }
}

var Class = {
    create: function () {
        return function () {
            this.initialize.apply(this, arguments);
        }
    }
}

/*---------------------------------------checkboxgroup---------------------------------------*/
var checkboxgroup = Class.create();
checkboxgroup.prototype = {
    /*初始化*/
    initialize: function (opt) {
        opt = opt || {};

        //参数
        this.MAIN_CLASSNAME = 'checkboxgroup';
        this.ITEM_CLASSNAME_TRUE = 'item_true';
        this.ITEM_CLASSNAME_FALSE = 'item_false';

        //主框架对象
        if (document.getElementById(opt.id) == null) {
            window.alert("id 参数有误！\n元素id为 " + opt.id + " 的元素不存在！");
            return;
        }

        //待选择的元素
        if (opt.content != "" && opt.content.split(',').length % 2 != 0) {
            window.alert("content 参数有误！\n应保证格式为[显示文字,值,显示文字,值, ....]");
            return;
        }

        this.id = opt.id;
        this.obj = document.getElementById(this.id);
        this.content = opt.content;
        this.defaulttext = opt.defaulttext;
        this.selectall = opt.selectall;

        this.renderCheckboxGroup();
    },
    renderCheckboxGroup: function () {
        //当前选中项的值
        this.obj.innerHTML = "";
        this.input = document.createElement('input');
        this.input.type = 'hidden';
        this.input.value = '';
        this.obj.appendChild(this.input);
        this.obj.className = this.MAIN_CLASSNAME;
        
        //多选项点击事件
        var contents = this.content.split(',');
        var root = this.obj;
        var input = this.input;
        var class_false = this.ITEM_CLASSNAME_FALSE;
        var class_true = this.ITEM_CLASSNAME_TRUE;
        var selectall=this.selectall;

        //添加一个‘全选’选项
        if (this.selectall) {
            this.sa = document.createElement('div');
            this.sa.text = "sa";
            this.sa.value = "sa";
            this.sa.innerHTML = "全选";
            this.sa.className = this.ITEM_CLASSNAME_FALSE;

            this.sa.onclick = function () {
                if (this.className == class_false) {
                    input.value = "";
                    for (var i = 0; i < contents.length; i = i + 2) {
                        input.value += contents[i] + ',' + contents[i+1] + ';';
                    }
                    var divs = root.getElementsByTagName('div');
                    for (var i = 0; i < divs.length; i++) {
                        divs[i].className = class_true;
                    }
                }
                else {
                    input.value = ",;";
                    var divs = root.getElementsByTagName('div');
                    for (var i = 0; i < divs.length; i++) {
                        divs[i].className = class_false;
                    }
                }
            }

            this.obj.appendChild(this.sa);
        }

        //添加其它选项
        for (var i = 0; i < contents.length; i = i + 2) {
            this.item = document.createElement('div');
            this.item.text = contents[i];
            this.item.value = contents[i + 1];
            this.item.innerHTML = this.item.text;

            if (this.defaulttext.indexOf(contents[i]) >= 0) {
                //alert(this.defaulttext + '\n' + contents[i] + '\n' + this.defaulttext.indexOf(contents[i]))
                this.item.className = this.ITEM_CLASSNAME_TRUE;
                this.input.value += contents[i] + ',' + contents[i + 1] + ';';
            }
            else {
                this.item.className = this.ITEM_CLASSNAME_FALSE;
            }

            this.item.onclick = function () {
                var divs = root.getElementsByTagName('div');

                if (this.className == class_false) {
                    input.value += this.text + ',' + this.value + ';';
                    this.className = class_true;

                    if (selectall) {//有全选按钮
                        divs[0].className = class_true;
                        for (var i = 1; i < divs.length; i++) {
                            if (divs[i].className == class_false) {
                                divs[0].className = class_false;
                                break;
                            }
                        }
                    }
                }
                else {
                    input.value = input.value.replace(this.text + ',' + this.value + ';', '');
                    this.className = class_false;

                    if (selectall) {//有全选按钮
                        divs[0].className = class_false;
                    }
                }

                
            }

            this.obj.appendChild(this.item);
        }
        //如果默认全部都选中，则【全部】也需选中
        var len = $("div.item_false").length;
        if (len == 1) {
            $("div.item_false").removeClass("item_false").addClass("item_true");
        }
    },

    getText: function () {
        var returnhtml = '';
        //去掉this.input.value最后一个分号
        var items = this.input.value.substring(0, this.input.value.length-1).split(';');
        if (items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                returnhtml += items[i].split(',')[0] + ',';
            }
        }
        return returnhtml.substring(0, returnhtml.length - 1);
    },

    getValue: function () {
        var returnhtml = '';
        //去掉this.input.value最后一个分号
        if (this.input.value.length > 0) {
            var items = this.input.value.substring(0, this.input.value.length - 1).split(';');
            if (items.length > 0) {
                for (var i = 0; i < items.length; i++) {
                    if (items[i] != ",") {
                        returnhtml += items[i].split(',')[1] + ',';
                    }
                }
            }
            return returnhtml.substring(0, returnhtml.length - 1);
        } else {
            return "";
        }
       
    },

    setText: function (t) {
        var vs = t.split(',');
        this.input.value = '';

        if (vs.length > 0) {
            var divs = this.obj.getElementsByTagName('div');
            for (var i = 0; i < divs.length; i++) {
                divs[i].className = this.ITEM_CLASSNAME_FALSE;
                for (var j = 0; j < vs.length; j++) {
                    if (vs[j] == divs[i].text) {
                        divs[i].className = this.ITEM_CLASSNAME_TRUE;
                        this.input.value += divs[i].text + ',' + divs[i].value + ';';
                        break;
                    }
                }
            }
        }
    },

    setValue: function (v) {
        var vs = v.split(',');
        this.input.value = '';

        if (vs.length > 0) {
            var divs = this.obj.getElementsByTagName('div');
            for (var i = 0; i < divs.length; i++) {
                divs[i].className = this.ITEM_CLASSNAME_FALSE;
                for (var j = 0; j < vs.length; j++) {
                    if (vs[j] == divs[i].value) {
                        divs[i].className = this.ITEM_CLASSNAME_TRUE;
                        this.input.value += divs[i].text + ',' + divs[i].value + ';';
                        break;
                    }
                }
            }
        }
    }
}
/*---------------------------------------checkboxgroup:end---------------------------------------*/

/*---------------------------------------radiobuttongroup---------------------------------------*/
var radiobuttongroup = Class.create();
radiobuttongroup.prototype = {
    /*初始化*/
    initialize: function (opt) {
        opt = opt || {};
        
        //参数
        this.MAIN_CLASSNAME = 'RadiobtnGroup';
        this.ITEM_CLASSNAME_TRUE = 'item_true';
        this.ITEM_CLASSNAME_FALSE = 'item_false';

        //主框架对象
        if (document.getElementById(opt.id) == null) {
            window.alert("id 参数有误！\n元素id为 " + opt.id + " 的元素不存在！");
            return;
        }

        //待选择的元素
        if (opt.content != "" && opt.content.split(',').length % 2 != 0) {
            window.alert("content 参数有误！\n应保证格式为[显示文字,值,显示文字,值, ....]");
            return;
        }

        this.id = opt.id;
        this.obj = document.getElementById(this.id);        
        this.content = opt.content;
        this.defaulttext = opt.defaulttext; 
        
        this.renderGroup();
    },

    renderGroup: function () {
        //当前选中项的值
        this.input = document.createElement('input');
        this.input.type = 'hidden';
        this.input.value = '';
        this.obj.appendChild(this.input);

        var contents = this.content.split(',');

        this.obj.className = this.MAIN_CLASSNAME;
        this.obj.innerHTML = "";

        for (var i = 0; i < contents.length; i = i + 2) {
            this.item = document.createElement('div');
            this.item.text = contents[i];
            this.item.value = contents[i + 1];
            this.item.innerHTML = this.item.text;

            if (this.defaulttext == contents[i]) {
                this.item.className = this.ITEM_CLASSNAME_TRUE;
                this.input.value = contents[i] + ',' + contents[i + 1];
            }
            else {
                this.item.className = this.ITEM_CLASSNAME_FALSE;
            }

            var root = this.obj;
            var input = this.input;
            var class_false = this.ITEM_CLASSNAME_FALSE;
            var class_true = this.ITEM_CLASSNAME_TRUE;
            this.item.onclick = function () {
                if (this.className == class_false) {
                    input.value = this.text + ',' + this.value;

                    var divs = root.getElementsByTagName('div');
                    if (divs.length > 0) {
                        for (var i = 0; i < divs.length; i++) {
                            divs[i].className = class_false;
                        }
                    }
                    this.className = class_true;
                }
            }

            this.obj.appendChild(this.item);
        }
    },

    getText: function () {
        return this.input.value.split(',')[0];
    },

    getValue: function () {
        return this.input.value.split(',')[1];
    },

    setText: function (t) {
        var divs = this.obj.getElementsByTagName('div');
        for (var i = 0; i < divs.length; i++) {
            if (t == divs[i].text) {
                for (var j = 0; j < divs.length; j++) {
                    divs[j].className = this.ITEM_CLASSNAME_FALSE;
                }
                divs[i].className = this.ITEM_CLASSNAME_TRUE;
                this.input.value = divs[i].text + ',' + divs[i].value;
                break;
            }
        }
    },

    setValue: function (v) {
        var divs = this.obj.getElementsByTagName('div'); 
        for (var i = 0; i < divs.length; i++) {
            if (v == divs[i].value) {
                for (var j = 0; j < divs.length; j++) {
                    divs[j].className = this.ITEM_CLASSNAME_FALSE;
                }
                divs[i].className = this.ITEM_CLASSNAME_TRUE;
                this.input.value = divs[i].text + ',' + divs[i].value;
                break;
            }
        }
    }
};

/*---------------------------------------radiobuttongroup结束---------------------------------------*/

/*---------------------------------------单选列表---------------------------------------*/
var DropDownList = Class.create();
DropDownList.prototype = {
    /*初始化*/
    initialize: function (opt) {
        opt = opt || {};
        //主框架对象
        if (document.getElementById(opt.ItemID) == null) {
            window.alert("containerId 参数有误！\n元素ID为 " + opt.container + " 的元素不存在！");
            return;
        }
        this.ItemID = opt.ItemID;
        this.obj = document.getElementById(this.ItemID);
        this.obj.innerHTML = "";

        //待选择的元素
        if (opt.Value != "" && opt.Value.split(',').length % 2 != 0) {
            window.alert("id为" + this.ItemID + "的value值设置有误！\n应保证格式为[显示文字,值,显示文字,值, ....]");
            return;
        }
        this.Value = opt.Value;

        //显示方向：靠左显示、靠右显示
        if (opt.ShowSide != 'left' && opt.ShowSide != 'right') {
            window.alert("id为" + this.ItemID + "ShowSide值设置有误！\n取值为  1 'left'靠左显示  2  'right'靠右显示！");
            return;
        }
        this.ShowSide = opt.ShowSide;

        //待选择元素显示列数
        if (isNaN(opt.ColumnCount) == true || opt.ColumnCount < 0) {
            this.ColumnCount = 1;
        }
        this.ColumnCount = parseInt(opt.ColumnCount);

        //元素宽度
        this.Width = parseInt(opt.Width) || 127;
        if (opt.ColumnCount != 1) {
            //this.olWidth = 103;
            if (opt.WidthStyle == "big") {
                this.olWidth = 150;
            }
            else {
                if (opt.olWidth != undefined) {
                    this.olWidth = opt.olWidth ? (this.olWidth = opt.olWidth - 12) : this.Width - 12;
                } else {
                    this.olWidth = 103;
                }
            }
        } else {
            this.olWidth = opt.olWidth ? (this.olWidth = opt.olWidth - 12) : this.Width - 12;
        }

        this.classShowName = "select_mid";
        if (opt.classShowName != null && opt.classShowName != "") {
            this.classShowName = opt.classShowName;
        }

        //图层位置
        this.ZIndex = opt.ZIndex;

        this.RenderSingle();
    },

    RenderSingle: function () {
        //把待选择内容转到数组里
        var items = this.Value.split(',');

        this.obj.style.cursor = "pointer";
        this.obj.style.width = this.Width + "px";
        this.obj.className = this.classShowName;

        this.divBoxL = document.createElement('div');
        this.divBoxL.className = 'divBoxL';
        this.obj.appendChild(this.divBoxL);
        this.divBoxR = document.createElement('div');
        this.divBoxR.className = 'divBoxR';
        $(this.divBoxR).width(this.Width - 5);
        this.obj.appendChild(this.divBoxR);

        //选择框主元素
        this.divRoot = document.createElement('div');
        this.RenderCSS_3(this.divRoot, this.ZIndex);
        this.divBoxR.appendChild(this.divRoot);

        //显示文字元素
        this.h6 = document.createElement('h6');
        this.RenderCSS_4(this.h6, this.Width);
        this.h6.innerHTML = ''; //默认显示空
        this.obj.value = '';
        this.divRoot.value = '';
        this.divRoot.appendChild(this.h6);


        //浮动面板元素
        this.ol = document.createElement('ol');
        this.RenderCSS_5(this.ol, this.ColumnCount, this.ShowSide, this.Width);
        this.divRoot.appendChild(this.ol);

        var ol = this.ol;
        var h6 = this.h6;
        var divRoot = this.divRoot;


        //待选择元素
        for (var i = 0; i < items.length; i = i + 2) {
            this.li = document.createElement('li');
            //this.li.title = items[i];
            this.li.innerHTML = items[i];
            this.li.title = items[i];
            this.li.className = items[i + 1];
            this.RenderCSS_6(this.li);

            this.li.onmouseover = function () {
                this.style.color = "#fff";
                this.style.background = "#66a8fb";
            }

            this.li.onmouseout = function () {
                this.style.color = "#333";
                this.style.background = "#fff";
            }

            this.li.onclick = function () {
                h6.innerHTML = this.innerHTML;
                divRoot.value = this.className;
                ol.style.display = "none";
            }

            this.ol.appendChild(this.li);
        }

        var divBoxL = this.divBoxL;
        var divBoxR = this.divBoxR;

        var hoverClassName = (this.obj.className);
        //添加事件
        $(this.obj).hover(
            function () {
                $(ol).show();
                $(this).addClass(hoverClassName + '_focus');
            },
            function () {
                $(ol).hide();
                $(this).removeClass(hoverClassName + '_focus');
            }
        )

        //赋个默认值
        h6.innerHTML = items.length > 0 ? items[0] : "";
        divRoot.value = items.length > 1 ? items[1] : "";
    },

    //添加样式函数
    RenderCSS_1: function (e) {
        e.style.background = "url(../../Images/dz.webcontrols/dropdown.png) no-repeat 0 0";
        e.style.width = "5px";
        e.style.height = "36px";
        e.style.cssFloat = "left";
        e.style.styleFloat = "left";
    },

    RenderCSS_2: function (e, width) {
        e.style.background = "url(../../images/dz.webcontrols/dropdown.png) no-repeat right 0";
        e.style.width = (parseInt(width) - 3) + "px";
        e.style.height = "36px";
        e.style.cssFloat = "left";
        e.style.styleFloat = "left";
    },

    RenderCSS_3: function (e, ZIndex) {
        e.style.position = "relative";
        e.style.textAlign = "left";
        e.style.cssFloat = "left";
        e.style.styleFloat = "left";
        e.style.margin = "0";
        e.style.display = "inline";
        e.style.cursor = "pointer";
        e.style.fontFamily = "Microsoft YaHei";
        e.style.zIndex = ZIndex;
    },

    RenderCSS_4: function (e, width) {
        e.style.cssFloat = "left";
        e.style.styleFloat = "left";
        e.style.color = "#333";
        e.style.fontSize = "12px";
        e.style.fontWeight = "normal";
        e.style.fontFamily = "Microsoft YaHei";
        e.style.textDecoration = "none";
        e.style.height = "28px";
        e.style.lineHeight = "28px";
        e.style.paddingLeft = "11px";
        e.style.paddingRight = "2px";
        e.style.width = (parseInt(width) - 40) + "px";
        e.style.overflow = "hidden";
        e.style.textOverflow = "ellipsis";
        e.style.whiteSpace = "nowrap";
    },

    RenderCSS_5: function (e, ColumnCount, ShowSide, Width) {
        e.style.display = "none";
        e.style.margin = "0";
        e.style.padding = "5px";
        e.style.position = "absolute";

        if (ShowSide == 'right') {
            e.style.left = -(parseInt(ColumnCount) * this.olWidth - parseInt(Width) - 15) + 'px';
        }
        else {
            e.style.left = "0px";
        }
        e.style.top = "27px";
        e.style.width = parseInt(ColumnCount) * this.olWidth + "px";
        e.style.background = "#fff";
        e.style.border = "1px solid #66a8fb";
    },

    RenderCSS_6: function (e, width) {
        e.style.cssFloat = "left";
        e.style.styleFloat = "left";
        e.style.height = "23px";
        e.style.lineHeight = "23px";
        e.style.cursor = "pointer";
        e.style.color = "#333";
        e.style.padding = "0 10px";
        e.style.paddingLeft = "5px";
        e.style.width = (this.olWidth - 15) + 'px';
        e.style.overflow = "hidden";
        e.style.textOverflow = "ellipsis";
        e.style.whiteSpace = "nowrap";
        e.style.fontSize = "12px";
        e.style.marginBottom = "5px";
        e.style.textAlign = "center";
    },

    //取值函数
    GetValue: function () {
        return this.divRoot.value;
    },

    GetText: function () {
        return (this.h6 != null) ? this.h6.innerHTML : "";
    },

    //赋值函数
    //通过显示文字赋值
    SetText: function (text) {
        if ('' == text) {
            return;
        }

        this.h6.innerHTML = "";
        this.divRoot.value = "";

        var temp = this.Value.split(',');

        for (var i = 0; i < temp.length; i = i + 2) {
            if (temp[i] == text) {
                this.h6.innerHTML = text;
                this.divRoot.value = temp[i + 1];
                return;
            }
        }
    },


    //通过显示文字赋值
    SetValue: function (value) {
        if ('' == value) {
            //window.alert("SetCurrentByText()方法不能接收空值参数！");
            return;
        }

        this.h6.innerHTML = "";
        this.divRoot.value = "";

        var temp = this.Value.split(',');

        for (var i = 0; i < temp.length; i = i + 2) {
            if (temp[i + 1] == value) {
                this.h6.innerHTML = temp[i];
                this.divRoot.value = value;
                return;
            }
        }
    },

    SetEmpty: function () {
        this.h6.innerHTML = "";
        this.divRoot.value = "";

        var objInputElementList = this.divRoot.getElementsByTagName('input');
        for (var i = 0; i < objInputElementList.length; i++) {
            var objChildElement = objInputElementList.item(i);
            objChildElement.checked = false;
        }
    }
}

/*---------------------------------------单选列表:结束---------------------------------------*/


/*---------------------------------------tab---------------------------------------*/
function setTab(tabid, conid) {
    var lis = document.getElementById(tabid).parentNode.getElementsByTagName('li');
    var cons = document.getElementById(conid).parentNode.getElementsByTagName('div');

    if (lis.length > 0) {
        for (var i = 0; i < lis.length; i++) {
            if (lis[i].id == tabid) {
                lis[i].className = "current";
            }
            else {
                lis[i].className = "";
            }
        }
    }
    
    if (cons.length > 0) {
        for (var i = 0; i < cons.length; i++) {
            if (cons[i].id == conid) {
                cons[i].style.display = "";
            }
            else {
                cons[i].style.display = "none";
            }
        }
    }
}

/*---------------------------------------tab:end---------------------------------------*/

/*表头固定*/
function FixTableNew(TableID, FixColumnNumber, width, className) {
    width = width - 2;
    if ($("#" + TableID + "_tableLayout").length != 0) {
        $("#" + TableID + "_tableLayout").before($("#" + TableID));
        $("#" + TableID + "_tableLayout").empty();
    }
    else {
        className = className || "tableLayout";
        $("#" + TableID).after("<div id='" + TableID + "_tableLayout' class='" + className + "' style='width:" + width + "px;'></div>");
    }
    var oldtable = $("#" + TableID),
        init_w = parseInt(oldtable.width()),
        init_h = parseInt(oldtable.height());
    $('<div id="' + TableID + '_tableColumn"></div>'
        + '<div id="' + TableID + '_tableData"></div>').appendTo("#" + TableID + "_tableLayout");
    var tableColumnClone = oldtable.clone(true);
    $("#" + TableID + "_tableData").append(oldtable);
    // 如果小于指定宽度 就按指定宽度算, 不设定fix表格
    if (!(init_w > width)) {
        $("#" + TableID).width('100%');
        return false;
    } else {
        tableColumnClone.attr("id", TableID + "_tableColumnClone");
        $("#" + TableID + "_tableColumn").append(tableColumnClone);
        var ColumnsWidth = 0;
        // 计算固定td的宽度(width+padding)
        $("#" + TableID + "_tableColumn tr:first th:lt(" + FixColumnNumber + ")").each(function () {
            ColumnsWidth += $(this).outerWidth() + 1;
        });
        $("#" + TableID + "_tableColumn").css({ 'width': ColumnsWidth, "overflow": "hidden", "position": "absolute", 'top': 0, 'left': 0, 'background-color': '#ffffff' });
        $("#" + TableID + "_tableData").css({ "overflow-x": "auto", "overflow-y": "hidden", "height": init_h + 17 });
    }
}
/*表头固定：end*/

//自定义面板内容
(function ($) {
    var Drag = function (id) {
        this.el = document.getElementById(id);
        this.el.me = this;
        this.el.onmousedown = this.dragstart;
    }
    Drag.prototype = {
        constructor: Drag,
        dragstart: function (e, self, el) {
            e = e || window.event;
            self = this.me;//拖动的对象
            el = self.el;//拖动的元素,后面来改变元素的left和top的
            el.offset_x = e.clientX - el.offsetLeft;
            el.offset_y = e.clientY - el.offsetTop;
            document.onmousemove = function (e) {
                self.drag(e, el);
            }
            document.onmouseup = function (e) {
                self.dragend();
            }

        },
        drag: function (e, el) {
            e = e || window.event;
            with (el.style) {
                cursor: 'pointer',
                left = e.clientX - el.offset_x + "px";
                top = e.clientY - el.offset_y + "px";
            }
            !+"\v1" ? document.selection.empty() : window.getSelection().removeAllRanges();//js清除选中的内容，要不然在拖动的过程中会选中其他文本
        },

        dragend: function () {
            document.onmouseup = document.onmousemove = null;
        }
    }

    $.fn.customPlane = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.customPanel');
        }
    };

    var methods = {
        init: function (opts) {
            var options = $.extend({}, $.fn.customPlane.defaults, opts);
            globalOptions = options;

            return this.each(function () {
                var $originalElement = $(this);//父元素jquery对象
                var str = $.fn.customPlane.randomString(10);
                var str_message = $.fn.customMessage.randomString(10);
                var html = '<div class="frame_panel">';
                html += '<div class="frame_box" id="' + str_message + '">';
                html += '<div class="frame_close" onclick="$(this).parent().parent().remove();"></div>';
                if (globalOptions.title != "") {
                    html += '<div class="message_title">' + globalOptions.title + '</div>';
                }
                html += '<div class="message_content">' + globalOptions.content + '</div>';
                html += '<div class="message_btn">';
                if (globalOptions.hasCancelBtn) {
                    html += '<div id="' + str + '" class="message_sure">确定</div>';
                    html += '<div class="message_cancel" onclick="$(this).parent().parent().parent().remove();">取消</div>';
                } else {
                    html += '<div id="' + str + '" class="message_sure" style="margin: 0 auto;float:none;">确定</div>';
                }
                html += '</div>';
                html += '</div>';
                html += '</div>';

                $('body').append(html);

                $("#" + str).click(function () {
                    globalOptions.clickSure();
                    $(this).parent().parent().parent().remove();
                });

                new Drag(str_message);
            });

        }
    };
    $.fn.customPlane.randomString = function (len) {
        len = len || 32;
        var $chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678';    /****默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1****/
        var maxPos = $chars.length;
        var pwd = '';
        for (i = 0; i < len; i++) {
            pwd += $chars.charAt(Math.floor(Math.random() * maxPos));
        }
        return pwd;
    };

    // 默认参数

    $.fn.customPlane.defaults = {
        title: '提示框',               //初始值
        content: '',              //自定义内容
        hasCancelBtn: true,
        clickSure: function () { }     //点击确定时触发
    };

})(jQuery);

//自定义弹出信息面板内容
(function ($) {
    var Drag = function (id) {
        this.el = document.getElementById(id);
        this.el.me = this;
        this.el.onmousedown = this.dragstart;
    }
    Drag.prototype = {
        constructor: Drag,
        dragstart: function (e, self, el) {
            e = e || window.event;
            self = this.me;//拖动的对象
            el = self.el;//拖动的元素,后面来改变元素的left和top的
            el.offset_x = e.clientX - el.offsetLeft;
            el.offset_y = e.clientY - el.offsetTop;
            document.onmousemove = function (e) {
                self.drag(e, el);
            }
            document.onmouseup = function (e) {
                self.dragend();
            }

        },
        drag: function (e, el) {
            e = e || window.event;
            with (el.style) {
                cursor: 'pointer',
                left = e.clientX - el.offset_x + "px";
                top = e.clientY - el.offset_y + "px";
            }
            !+"\v1" ? document.selection.empty() : window.getSelection().removeAllRanges();//js清除选中的内容，要不然在拖动的过程中会选中其他文本
        },

        dragend: function () {
            document.onmouseup = document.onmousemove = null;
        }
    }

    $.fn.customMessage = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.customPanel');
        }
    };

    var methods = {
        init: function (opts) {
            var options = $.extend({}, $.fn.customMessage.defaults, opts);
            globalOptions = options;

            return this.each(function () {
                var $originalElement = $(this);//父元素jquery对象
                var str = $.fn.customMessage.randomString(10);
                var str_message = $.fn.customMessage.randomString(10);
                var html = '<div class="message_panel">';
                html += '<div class="message_box" id="' + str_message + '">';
                html += '<div class="messgae_close" onclick="$(this).parent().parent().remove();"></div>';
                if (globalOptions.title != "") {
                    html += '<div class="message_title">' + globalOptions.title + '</div>';
                }
                html += '<div class="message_content">';
                if (globalOptions.isDel) {
                    html += '<i class="warn"></i>';
                    html += globalOptions.content;
                   // html += '<em class="warn">删除后不可恢复！</em>';
                } else {
                    html += globalOptions.content;
                }
                html += '</div>';
                html += '<div class="message_btn">';
                if (globalOptions.hasCancelBtn) {
                    if (globalOptions.hasDel) {
                        html += '<div id="' + str + '" class="message_del">删除</div>';
                    } else {
                        html += '<div id="' + str + '" class="message_sure">确定</div>';
                    }
                    html += '<div class="message_cancel" onclick="$(this).parent().parent().parent().remove();">取消</div>';
                } else {
                    html += '<div id="' + str + '" class="message_sure" style="margin: 0 auto;float:none;">确定</div>';
                }
                html += '</div>';
                html += '</div>';
                html += '</div>';
                $('body').append(html);

                $("#" + str).click(function () {
                    globalOptions.clickSure();
                    $(this).parent().parent().parent().hide();
                });

                new Drag(str_message);
            });

        }
    };
    $.fn.customMessage.randomString = function (len) {
        len = len || 32;
        var $chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678';    /****默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1****/
        var maxPos = $chars.length;
        var pwd = '';
        for (i = 0; i < len; i++) {
            pwd += $chars.charAt(Math.floor(Math.random() * maxPos));
        }
        return pwd;
    };

    // 默认参数

    $.fn.customMessage.defaults = {
        title: '提示框',               //初始值
        content: '',              //自定义内容
        hasCancelBtn: true,
        clickSure: function () { }     //点击确定时触发
    };

})(jQuery);

//由于需要实现子页面返回时带条件搜索
//当前实现的逻辑是把条件存到cookie中，并从cookie中取出，赋值到各文本框中，再搜索.
var SetSearchCondition = function (back, paraVal, cookiename, id) {
    if (paraVal == "") {
        return null;
    }

    //如果是返回的需要进行赋值
    if (back == 1) {
        if (localStorage) {//如果支持localstorage，则用localstorage来存储，避免使用cookie
            paraVal = JSON.parse(localStorage.getItem(cookiename));
        } else {
            paraVal = JSON.parse(GetCookie(cookiename));
        }
        $.each(paraVal, function (name, value) {
            //先赋值input框
            var $inputItem = $("#" + name);
            if (!$inputItem.length) {
                return true;
            }
            //设置iput框value值
            $inputItem.val(value);
            //可能要对特殊的data-value处理

            //检查是否为单选，多选，下拉等特殊情况
            //获取父节点
            var $parentDIV = $inputItem.parent("div");
            if ($parentDIV.hasClass("nice-select")) {//下拉
                if (paraVal[name + "_dvalue"] == "不限") {
                    $inputItem.val(paraVal[name + "_dvalue"]);
                }
                $("li", $parentDIV).removeClass("active");
                $("li", $parentDIV).each(function () {
                    var $this = $(this);
                    if ($this.html() == value || $this.attr("data-value") == value) {
                        $this.addClass("active");
                        if ($this.attr("data-value") != undefined) {
                            $inputItem.attr("data-value", paraVal[name + "_dvalue"]);
                        }
                        return false;
                    }
                });
            } else if ($parentDIV.find("input.regular-radio").length > 0) {//单选
                $parentDIV.find("input.regular-radio").each(function () {
                    var $this = $(this);
                    if ($this.nextAll(".rctext").first().html() == value) {
                        $this.click();
                        return false;
                    }
                });
            } else if ($parentDIV.find("input.customcheckbox").length > 0) {//多选
                var arrV = value.split(',');
                if (arrV.length > 0) {
                    $parentDIV.find("input.customcheckbox").each(function () {
                        var $this = $(this);
                        if ($.inArray($this.nextAll(".rctext").first().html(), arrV) > -1) {
                            $this.click();
                            if ($this.nextAll(".rctext").first().html() == "全选") {
                                return false;
                            }
                        }
                    });
                }
            }
        });
    }
    //存储当前搜索信息

    if (localStorage) {
        localStorage.setItem(cookiename, JSON.stringify(paraVal));
    } else {
        SetCookie(cookiename, JSON.stringify(paraVal));
    }
    var returnParas = paraVal;
    if (back == 1) {
        if (id != undefined && id != "") {
            returnParas = $validate({ "select": "#" + id });
        } else {
            returnParas = $validate();
        }
        returnParas.pageIndex = 1;
        returnParas.pageSize = 15;
    }
    return returnParas;
}

/*checkbox按钮控制器*/
if ($(".gongsi-box").length > 0) {
    $(".gongsi-box").each(function (index, obj) {
        var data = $(obj).attr("data-for");
        if ($("#" + data).length > 0) {
            $("#" + data).attr("readonly", "readonly");
            var position = $("#" + data).offset();
            $(obj).css({ "top": (position.top + 28) + "px", "left": position.left + "px" });
            $("#" + data).focus(function () {
                $("div[data-for=" + data + "]").show();
            });
        }

        //标题全选按钮事件
        $(".box-title :checkbox", $(obj)).click(function () {
            if ($(this)[0].checked == false) {
                $(this).parent().parent().find(":checkbox").each(function (index, obj) {
                    $(obj)[0].checked = false;
                });
            }
            else {
                $(this).parent().parent().find(":checkbox").each(function (index, obj) {
                    $(obj)[0].checked = true;
                });
            }

            var data = $(this).parent().parent().attr("data-for");
            if (data != "" && $("#" + data).length > 0) {
                var res = "";
                $(this).parent().parent().find("li input:checked").each(function (index, obj) {
                    res += $(obj).val() + ",";
                });
                if (res.length > 0) {
                    res = res.substring(0, res.length - 1);
                }
                $("#" + data).val(res);
            }
        });

        //组的全选按钮事件
        $(".group-title :checkbox", $(obj)).click(function () {
            var obj = this;
            var name = $(obj).attr("name");
            if ($(obj)[0].checked == false) {
                $(obj).parent().parent().find("input[name=" + name + "]").each(function (index, obj) {
                    $(obj)[0].checked = false;
                });
                //置全选为未选中
                $(obj).parent().parent().find(".box-title :checkbox").first()[0].checked = false;
            } else {
                $(obj).parent().parent().find("input[name=" + name + "]").each(function (index, obj) {
                    $(obj)[0].checked = true;
                });

                //判断是否所有都被选中
                if ($(obj).parent().parent().find("li input:checked").length == $(obj).parent().parent().find("li input").length) {
                    $(obj).parent().parent().find(".box-title").first().children(":checkbox").first()[0].checked = true;
                }
            }

            var data = $(this).parent().parent().attr("data-for");
            if (data != "" && $("#" + data).length > 0) {
                var res = "";
                $(this).parent().parent().find("li input:checked").each(function (index, obj) {
                    res += $(obj).val() + ",";
                });
                if (res.length > 0) {
                    res = res.substring(0, res.length - 1);
                }
                $("#" + data).val(res);
            }
        });

        //子复选框事件
        $(".group-ul :checkbox", $(obj)).click(function () {
            var obj = this;
            //if ($(obj)[0].checked == false) {
            //    //组全选取消
            //    $(obj).parent().parent().prev(".group-title").children(":checkbox").first()[0].checked = false;
            //    //大全选取消
            //    $(obj).parent().parent().parent().find(".box-title").children(":checkbox").first()[0].checked = false;
            //}
            //else {
            //    //判断是否所有组内元素都被选中
            //    if ($(obj).parent().parent().find("li input:checked").length == $(obj).parent().parent().find("li input").length) {
            //        $(obj).parent().parent().prev(".group-title").children(":checkbox")[0].checked = true;

            //        //判断是否所有都被选中
            //        if ($(obj).parent().parent().parent().find("li input:checked").length == $(obj).parent().parent().parent().find("li input").length) {
            //            $(obj).parent().parent().parent().find(".box-title").first().children(":checkbox")[0].checked = true;
            //        }
            //    }

            //}

            var data = $(this).parent().parent().parent().attr("data-for");
            if (data != "" && $("#" + data).length > 0) {
                var res = "";
                $(this).parent().parent().parent().find("li input:checked").each(function (index, obj) {
                    res += $(obj).val() + ",";
                });
                if (res.length > 0) {
                    res = res.substring(0, res.length - 1);
                }
                $("#" + data).val(res);
            }
        });
    });
}