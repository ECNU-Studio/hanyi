var pagejs = function () {
    var oldindex = 1;
    var Search = function (indexpage, back) {
        oldindex = indexpage;
        var cookiename = "TeacherList";
        var res = $validate();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Teacher/TeacherList", "model=" + JSON.stringify(res), function (data) {

            $("#res").html(data);
            if ($(".tableLayout_wrap").width() < $("#MyTable").width()) {
                FixTableNew("MyTable", 1, $(".tableLayout_wrap").width());
            }

            hideloading();
        }, "html");

    }

    var ChangeStatus = function (obj, title, id, status) {
        $(obj).customMessage({
            title: "",
            content: title,
            clickSure: function () {
                showloading("提交中...");
                var paras = {
                    id: id,
                    status: status
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(paras),
                    url: "/Account/ChangeStatus",
                    dataType: "json",
                    contentType: 'application/json;charset=gb2312;'
                }).success(function (res) {
                    if (res.success) {
                        showok("操作成功！", function () { Search(oldindex, 0); });
                    } else {
                        showerror(res.message);
                    }
                    hideloading();
                }).error(function (xhr, status) {
                    //alert(status);
                });
            }
        });
    }

    var PWSet = function (obj, title, id) {
        $(obj).customMessage({
            title: "",
            content: title,
            clickSure: function () {
                showloading("提交中...");
                var paras = {
                    id: id
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(paras),
                    url: "/Teacher/PWSet",
                    dataType: "json",
                    contentType: 'application/json;charset=gb2312;'
                }).success(function (res) {
                    if (res.success) {
                        showok(res.message);
                    } else {
                        showerror(res.message);
                    }
                    hideloading();
                }).error(function (xhr, status) {
                    //alert(status);
                });
            }
        });
    }

    var add = function () {
        var res = $validate({ "select": "#open_panel" });
        if (!res.isOK) {
            return;
        }
        showloading("操作中。。。");
        $ajax("/Teacher/ModelPostData", JSON.stringify({ "model": res }), function (res) {
            hideloading();
            if (res.success) {
                $("#open_panel").hide();
                showok("保存成功!", function () { Search(oldindex, 0); });
            } else {
                showerror(res.message);
            }
        });
    }
    var del = function (obj) {
        var $this = $(obj);
        var delArr = [];
        $("[name='group']:checked").not(".js-check-all").each(function () {
            var $this = $(this), id = $this.attr("data-value");
            delArr.push(id);
        });
        if (delArr.length == 0) {
            showerror("至少选择一项");
            return;
        }
        $(obj).customMessage({
            isDel: true,
            title: "",
            content: "是否确认删除？",
            clickSure: function () {
                showloading("提交中...");
                var paras = {
                    ids: delArr
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(paras),
                    url: "/Teacher/DelTeachers",
                    dataType: "json",
                    contentType: 'application/json;charset=gb2312;'
                }).success(function (res) {
                    if (res.success) {
                        showok("操作成功！", function () { location.reload(); });
                    } else {
                        showerror(res.message);
                    }
                    hideloading();
                }).error(function (xhr, status) {
                    //alert(status);
                });
            }
        });
    }
    var initupload = function () {
        if ($('#fileupload').length > 0) {

           

            setTimeout(function () {
                $('#fileupload').uploadify({
                    'swf': UploadCss.swf,
                    'uploader': UploadUrl,
                    'cancelImg': UploadCss.cancelImg,
                    'fileSizeLimit': '5MB',//文件的极限大小，(B, KB, MB, or GB)
                    'fileTypeExts': '*.png;*.jpg;*.jpeg;*.bmp;*.pdf;*.doc;*.docx;',
                    'fileTypeDesc': '请选择jpg,png,jpeg,pdf,doc,docx',
                    'buttonText': '',
                    'width': 84,
                    'height': 26,
                    // Put your options here
                    'auto': true,
                    'multi': false,
                    'removeCompleted': true,
                    'onFallback': function () {
                        alert("上传控件初始化失败，请安装flash插件（地址：https://get.adobe.com/cn/flashplayer/?no_redirect）。");
                    },
                    'onInit': function (instance) {
                        $("#fileupload").css({ "marginTop": "-26px" });
                    },
                    //上传到服务器，服务器返回相应信息到data里
                    'onUploadSuccess': function (file, data, response) {
                        var info = eval("(" + data + ")");
                         
                        if (info.state == "SUCCESS") {
                            $("#picList").html("");
                            $("#picurl").val("");
                            //var picHtml = createfilemodel(info.original, info.url);
                            //$("#picList").html(picHtml);
                            $("#cv").val(info.url);
                            $("#url_cv").html(' <a href="' + info.url + '">' + info.original + '</a>');
                        }
                    }
                });
            }, 10);
        }
    }
    var showJSGL = function (obj, title) {
        showerror(title);
    }

    var showPlane = function () {
        $("#open_panel input").val("");
        $("#open_panel").show();
    }

    var handle = function () {

        Search(1, $("#back").val());
        $("#btn_search").click(function () {
            Search(1, 0);
        });

        $("#showAdd").click(function () {
            showPlane();
        });

        $("#btn_del").click(function () {
            del(this);
        });
        $("#savebtn").click(function () {
            add();
        });
        initupload();
    }

    return {
        init: function () {
            handle();
            window.Search = Search;
            window.PWSet = PWSet;
            window.ChangeStatus = ChangeStatus;
            window.showJSGL = showJSGL;
        }
    };
}();


