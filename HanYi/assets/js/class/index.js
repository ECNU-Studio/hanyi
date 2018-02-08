var pagejs = function () {
    var oldindex = 1;
    var Search = function (indexpage, back) {
        oldindex = indexpage;
        var cookiename = "ClassList";
        var res = $validate();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/ClassList", "model=" + JSON.stringify(res), function (data) {

            $("#res").html(data);
            if ($(".tableLayout_wrap").width() < $("#MyTable").width()) {
                FixTableNew("MyTable", 1, $(".tableLayout_wrap").width());
            }

            hideloading();
        }, "html");

    }

    

    var add = function () {
        var res = $validate({ "select": "#open_panel" });
        var userids = [];
        $("[name='addgroup']:checked").not(".js-check-all").each(function () {
            var $this = $(this), id = $this.attr("data-value");
            userids.push(id);
        });
        res.userids = userids;
        if (parseFloat(res.hour) > 6) {
            ShowErrorTipObj($("#hour"), "请填写0-6之间的数字");
            $("#hour").val(0);
            res.isOK = false;
        }
        if (!res.isOK) {
            return;
        }
        showloading("操作中。。。");
        $ajax("/Class/ModelPostData", JSON.stringify({ "model": res,"ids":userids }), function (res) {
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
                    url: "/Class/DelClasses",
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
 


    var matchCourse = function () {
        var value = "";
        var res;
        var loadingHtml = "<li data-item='no' class='droploading'>加载中。。。</li>";
        res = JSON.parse('{"coursename":"' + $("#coursesid_input").val() + '"}');
        $("#CourseDropRes").html(loadingHtml).show();
        //清空
        $("#coursesid").val("");
        $ajax("/Class/matchCourse", JSON.stringify({ "model": JSON.stringify(res) }), function (res) {
            if (res.success) {
                var html = res.message;
                if (html == "") {
                    return;
                }
                $("#CourseDropRes").html(html).show();
                $("#CourseDropRes li").not("[data-item='no']").click(function () {
                    var $this = $(this);
                    var value = $this.html();
                    var id = $this.attr("data-id");
                    $("#coursesid_input").val(value);
                    $("#coursesid").val(id);
                  
                    $("#CourseDropRes").hide().html();
                });
            }
        });
    }

  

    var matchCompany = function () {
        var value = "";
        var res;
        var loadingHtml = "<li data-item='no' class='droploading'>加载中。。。</li>";
        res = JSON.parse('{"companyname":"' + $("#companyid_input").val() + '"}');
        $("#CompanyDropRes").html(loadingHtml).show();
        //清空
        $("#companyid").val("");
        $ajax("/Class/matchCompany", JSON.stringify({ "model": JSON.stringify(res) }), function (res) {
            if (res.success) {
                var html = res.message;
                if (html == "") {
                    return;
                }
                $("#CompanyDropRes").html(html).show();
                $("#CompanyDropRes li").not("[data-item='no']").click(function () {
                    var $this = $(this);
                    var value = $this.html();
                    var id = $this.attr("data-id");
                    $("#companyid_input").val(value);
                    $("#companyid").val(id);
                    CompanyUsersAll(id);
                    $("#CompanyDropRes").hide().html();
                });
            }
        });

    }
    //根据企业id获取该企业下的用户
    var CompanyUsersAll = function (id) {

        $ajax("/Class/CompanyUsers", "companyid=" + id, function (data) {
            $("#user_panel").html(data);

        }, "html");
    }

    //根据企业id获取该企业下未选中的用户
    var CompanyUsers = function (id) {
       
        $ajax("/Class/CompanyUsers", "companyid=" + id + "&type=1", function (data) {
            $("#user_panel").html(data);
           
        }, "html");
       
    }
    var showJSGL = function (obj, title) {
        showerror(title);
    }

    var showPlane = function () {
        $("#open_panel input").val("");
        $("#user_panel").html("");
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

        $("#delbtn").click(function () {
            del(this);
        });
        $("#savebtn").click(function () {
            add();
        });
        $(document).on("keyup focus", "#companyid_input", function (e) {
            if (e.type == "keyup") {
                $("#companyid").val("");
            }
            matchCompany();
        });
        $(document).on("keyup focus", "#coursesid_input", function (e) {
            if (e.type == "keyup") {
                $("#courseid").val("");
            }
            matchCourse();
        });

        $(document).click(function (e) {
            e.stopPropagation();
            if (e.target.id != "companyid_input") {
                $("#CompanyDropRes").hide();
            }
            if (e.target.id != "coursesid_input") {
                $("#CourseDropRes").hide();
            }
        });

        $("#schooltime").datetimepicker();
    }

    return {
        init: function () {
            handle();
            window.Search = Search;            
            window.showJSGL = showJSGL;
        }
    };
}();


