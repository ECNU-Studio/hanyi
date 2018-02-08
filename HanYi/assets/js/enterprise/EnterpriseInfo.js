var pagejs = function () {
    var oldindexUser = 1;
    var oldindexCourse = 1;

    var SaveCompany = function () {
        var res = $validate({ "select": "#div_01" });
        if (!res.isOK) {
            return;
        }
        showloading("操作中。。。");
        $ajax("/Enterprise/ModelPostData", JSON.stringify({ "model": res }), function (res) {
            hideloading();
            if (res.success) {
                $("#open_panel").hide();
                showok("保存成功!", function () { Search(oldindex, 0); });
            } else {
                showerror(res.message);
            }
        });
    }

    var SearchUser = function (indexpage, back) {
        oldindexUser = indexpage;
        var cookiename = "EnterpriseUserList";
        var res = $validate({ "select": "#div_02" });
        res.pageIndex = indexpage;
        res.pageSize = 15;
        res.companyid = $("#companyid").val();
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Enterprise/EnterpriseUserList", "model=" + JSON.stringify(res), function (data) {

            $("#res_user").html(data);

            hideloading();
            $(".update-user").bind("click", function () {                
                //$("#open_panel input[id='userid'] ").val($(this).attr("data-value"));
                //$("#open_panel input[id='name'] ").val($(this).attr("data-name"));
                //$("#open_panel input[id='username'] ").val($(this).attr("data-username"));
                //$("#open_panel  [id='password-li'] ").hide();
                //$("#open_panel  input[id='password'] ").val("abc123");
                //$("#open_panel input[id='email'] ").val($(this).attr("data-email"));
                //$("#open_panel input[id='department'] ").val($(this).attr("data-department"));
                //$("#open_panel input[id='position'] ").val($(this).attr("data-position"));
                //$("#open_panel input[id='tel'] ").val($(this).attr("data-tel"));
                //$("#open_panel").show();
            });

        }, "html");

    }

    var SearchCourse = function (indexpage, back) {
        oldindexCourse = indexpage;
        var cookiename = "EnterpriseCourseList";
        var res = $validate({ "select": "#div_03" });
        res.companyid = $("#companyid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Enterprise/EnterpriseCourseList", "model=" + JSON.stringify(res), function (data) {

            $("#res_course").html(data);

            hideloading();
        }, "html");

    }
    var addUser = function () {
        var res = $validate({ "select": "#open_panel" });
        if (!res.isOK) {
            return;
        }
        res.id = res.userid;

        showloading("操作中。。。");
        $ajax("/Enterprise/ModelPostData_User", JSON.stringify({ "model": res }), function (res) {
            hideloading();
            if (res.success) {
                $("#open_panel").hide();
                showok("保存成功!", function () { SearchUser(oldindexUser, 0); });
            } else {
                showerror(res.message);
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
                    url: "/Enterprise/PWSet",
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
    var showJSGL = function (obj, title) {
        showerror(title);
    }

    var showPlane = function () {
        $("#open_panel input[type!='hidden']").val("");
        $("#open_panel input[id='userid'] ").val("");
        $("#open_panel [id='password-li'] ").show();
        $("#open_panel").show();
    }

    var del = function (obj) {
        var $this = $(obj);
        var delArr = [];
        $("[name='user']:checked").not(".js-check-all").each(function () {
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
                    url: "/Enterprise/DelEnterpriseUser",
                    dataType: "json",
                    contentType: 'application/json;charset=gb2312;'
                }).success(function (res) {
                    if (res.success) {
                        showok("操作成功！", function () { SearchUser(oldindexUser); });
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

    //封面上传初始化
    var initupload = function (fileupload) {
        if ($('#' + fileupload).length > 0) {

            $(document).on('click', "#picList .close", function () {
                var $this = $(this), $parent = $this.closest("#picList");
                $("#cover").val("");
                $parent.find("img").attr("src", "");
                $("#picList").hide();
            });

            setTimeout(function () {
                $('#' + fileupload).uploadify({
                    'swf': UploadCss.swf,
                    'uploader': UploadUrlImage,
                    'cancelImg': UploadCss.cancelImg,
                    'fileSizeLimit': '5MB',//文件的极限大小，(B, KB, MB, or GB)
                    'fileTypeExts': '*.png;*.jpg;*.jpeg;*.bmp;',
                    'fileTypeDesc': '请选择jpg,png,jpeg,bmp',
                    'buttonText': '上传',
                    'width': 72,
                    'height': 28,
                    // Put your options here
                    'auto': true,
                    'multi': false,
                    'removeCompleted': true,
                    'onFallback': function () {
                        alert("上传控件初始化失败，请安装flash插件（地址：https://get.adobe.com/cn/flashplayer/?no_redirect）。");
                    },
                    'onInit': function (instance) {
                        $("#" + fileupload).css({ "marginTop": "-24px" });
                    },
                    //上传到服务器，服务器返回相应信息到data里
                    'onUploadSuccess': function (file, data, response) {
                        var info = eval("(" + data + ")");

                        if (info.state == "SUCCESS") {
                            $("#picList img").attr("src", "");
                            $("#picList").hide();
                            $("#cover").val("");
                            $("#picList img").attr("src", info.url);
                            $("#picList").show();
                            $("#cover").val(info.url);

                        }
                    }
                });
            }, 10);
        }
    }

    var handle = function () {

        $(document).on("click", "[data-toggle='tab']", function () {
            var $this = $(this),classname = $this.attr("class"),htmlVal = $this.html(),hrefid= $this.attr("data-href");
            if (classname == "active") {
                return;
            } else {
                $("[data-toggle='tab']").closest("li").removeClass("active");
                $this.closest("li").addClass("active");
                $(".tab-content .tab-pane").removeClass("active");
                $("" + hrefid).addClass("active");
                if (htmlVal == "所有用户") {
                    SearchUser(1, 0);
                } else {
                    SearchCourse(1, 0);
                }
            }
        });

        initupload("fileupload");

       // SearchUser(1, $("#back").val());
        $("#savebtn_company").click(function () {
            SaveCompany();
        });
        $("#btn_SearchUser").click(function () {
            SearchUser(1, 0);
        });

        $("#btn_SearchCourse").click(function () {
            SearchCourse(1, 0);
        });

        $("#showAdd").click(function () {
            showPlane();
        });

        $("#savebtn").click(function () {
            addUser();
        });

        $("#del_user").click(function () {
            del(this);
        });
    }

    return {
        init: function () {
            handle();
            window.SearchUser = SearchUser;
            window.SearchCourse = SearchCourse;
            window.PWSet = PWSet;
        }
    };
}();


