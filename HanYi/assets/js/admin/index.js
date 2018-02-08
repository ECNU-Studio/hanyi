var pagejs = function () {
    var oldindex = 1;
    var Search = function (indexpage, back) {
        oldindex = indexpage;
        var cookiename = "AdminList";
        var res = $validate();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Admin/AdminList", "model=" + JSON.stringify(res), function (data) {

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
        if (!res.isOK) {
            return;
        }
        showloading("操作中。。。");
        $ajax("/Admin/ModelPostData", JSON.stringify({ "model": res }), function (res) {
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
                    url: "/Admin/DelAdmin",
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
                    url: "/Admin/PWSet",
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

        
    }

    return {
        init: function () {
            handle();
            window.Search = Search;
            window.showJSGL = showJSGL;
            window.PWSet = PWSet;
        }
    };
}();


