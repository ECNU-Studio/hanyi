var pagejs = function () {
    var oldindex = 1;
    var Search = function (indexpage, back) {
        oldindex = indexpage;
        var cookiename = "CoursesList";
        var res = $validate();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Courses/CoursesList", "model=" + JSON.stringify(res), function (data) {

            $("#res").html(data);

            hideloading();
        }, "html");

    }
    var add = function () {
        var res = $validate({ "select": "#open_panel" });
        if (!res.isOK) {
            return;
        }
        showloading("操作中。。。");
        $ajax("/Courses/ModelPostData", JSON.stringify({ "model": res }), function (res) {
            hideloading();
            if (res.success) {
                $("#open_panel").hide();
                showok("保存成功!", function () { Search(oldindex, 0); });
            } else {
                showerror(res.message);
            }
        });
    }


    var showJSGL = function (obj, title) {
        showerror(title);
    }

    var showPlane = function () {
        $("#open_panel input").val("");
        $("#open_panel").show();
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
                    url: "/Courses/DelCourses",
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

    var matchTeacher = function () {
        var value = "";
        var res;
        var loadingHtml = "<li data-item='no' class='droploading'>加载中。。。</li>";
        res = JSON.parse('{"teachername":"' + $("#teacherid_input").val() + '"}');
        $("#DropRes").html(loadingHtml).show();
        //清空
        $("#teacherid").val("");
        $ajax("/Courses/MatchTeacher", JSON.stringify({ "model": JSON.stringify(res) }), function (res) {
            if (res.success) {
                var html = res.message;
                if (html == "") {
                    return;
                }
                $("#DropRes").html(html).show();
                $("#DropRes li").not("[data-item='no']").click(function () {
                    var $this = $(this);
                    var value = $this.html();
                    var id = $this.attr("data-id");
                    $("#teacherid_input").val(value);
                    $("#teacherid").val(id);

                    $("#DropRes").hide().html();
                });
            }
        });

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

        $(document).on("keyup focus", "#teacherid_input", function (e) {
            if (e.type == "keyup") {
                $("#teacherid").val("");
            }
            matchTeacher();
        });

        $(document).click(function (e) {
            e.stopPropagation();
            if (e.target.id != "teacherid_input") {
                $("#DropRes").hide();
            }
        });
    }

    return {
        init: function () {
            handle();
            window.Search = Search;
        }
    };
}();


