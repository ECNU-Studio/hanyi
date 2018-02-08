var pagejs = function () {
    var oldindexUser = 1;   
    var oldindexscore = 1;
    var oldindexstate = 1;
    var oldindexrank = 1;
    var oldindexaddress = 1;
    var SaveBaseInfo = function(){
        var res = $validate({ "select": "#div_00" });
        if (!res.isOK) {
            return;
        }
        if (parseFloat(res.hour) > 6) {
            ShowErrorTipObj($("#hour"), "请填写0-6之间的数字");
            $("#hour").val(0)
            return;
        }
        showloading("操作中。。。");
        $ajax("/Class/UpdatePostData", JSON.stringify({ "model": res }), function (res) {
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
        var cookiename = "UserList";
        var res = $validate({ "select": "#div_01" });
        res.classid = $("#classid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/UserList", "model=" + JSON.stringify(res), function (data) {
            $("#res_user").html(data);
            hideloading();
        }, "html");

    }
    var SearchAddress = function (indexpage, back) {
        oldindexaddress = indexpage;
        var cookiename = "AddressList";
        var res = $validate({ "select": "#div_06" });
        res.classid = $("#classid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/AddressList", "model=" + JSON.stringify(res), function (data) {
            $("#res_address").html(data);
            hideloading();

            $(".edit-address").on("click", function () {
                showAddressPlane();
                $("#addressid").val($(this).attr("data-id"));
                $("#period").val($(this).attr("data-period"));
                $("#address").val($(this).attr("data-address"));
                $("#datebegin").val($(this).attr("data-datebegin"));

            })

        }, "html");

    }

    var addAddress = function () {
        var res = $validate({ "select": "#address_panel" });
        res.classid = $("#classid").val();
        res.id = res.addressid;
        if (!res.isOK) {
            return;
        }
        showloading("操作中。。。");
        $ajax("/Class/addAddress", JSON.stringify(res), function (res) {
            hideloading();
            if (res.success) {
                $("#address_panel").hide();
                showok("保存成功!", function () { SearchAddress(oldindexaddress, 0); });
            } else {
                showerror(res.message);
            }
        });
    }
    var delAddress = function (obj) {
        var $this = $(obj);
        var delArr = [];
        $("[name='groupaddress']:checked").not(".js-check-all").each(function () {
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
                    ids: delArr,
                    classid: $("#classid").val()
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(paras),
                    url: "/Class/delAddress",
                    dataType: "json",
                    contentType: 'application/json;charset=gb2312;'
                }).success(function (res) {
                    if (res.success) {
                        showok("操作成功！", function () { SearchAddress(oldindexaddress, 0); });
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
    var addUser = function () {
        var res = $validate({ "select": "#open_panel" });
        var userids = [];
        $("[name='addgroup']:checked").not(".js-check-all").each(function () {
            var $this = $(this), id = $this.attr("data-value");
            userids.push(id);
        });
        res.ids = userids;
        res.classid = $("#classid").val();
        if (!res.isOK) {
            return;
        }
        showloading("操作中。。。");
        $ajax("/Class/addUser", JSON.stringify(  res  ), function (res) {
            hideloading();
            if (res.success) {
                $("#open_panel").hide();
                showok("保存成功!", function () { SearchUser(oldindexUser, 0); });
            } else {
                showerror(res.message);
            }
        });
    }
    //根据企业id获取该企业下的用户
    var CompanyUsers = function (id) {

        $ajax("/Class/CompanyUsers", "companyid=" + id + "&type=1&classid=" + $("#classid").val(), function (data) {
            $("#user_panel").html(data);

        }, "html");

    }

    var showJSGL = function (obj, title) {
        showerror(title);
    }

    var showPlane = function () {
        $("#open_panel input[type!='hidden']").val("");
        $("#open_panel").show();
        CompanyUsers($("#companyid").val());
    }

    var showAddressPlane = function () {
        $("#address_panel input[type!='hidden']").val("");
        $("#addressid").val("");
        $("#address_panel").show();
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
                    ids: delArr,
                    classid: $("#classid").val()
                }
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(paras),
                    url: "/Class/delUser",
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

    var SearchClassModels = function () {
        var cookiename = "SearchClassModels";
           
        showloading("搜索中。。。");
        $ajax("/Class/ClassModelsList", "classid=" + $("#classid").val(), function (data) {
            $("#res_classmodels").html(data);
            hideloading();
        }, "html");
    }

    var addRow = function () {
        var $table = $('#module_table');
        var length = $table.find("tr").length
        var tr = '  <tr class="row">'+
                                    '<td>'+
                                        '<div class="ck_select clearfix">'+
                                         '   <label><input type="checkbox" data-value="' + length + '" name="module" class="checkbox" /></label>' +
                                       ' </div>'+
                                   ' </td>' +
                                   ' <td><input placeholder="标题" class="Text_lt " type="text" value="" /></td>' +
                                   ' <td>    <select style="border-radius:4px;" >' +                                             
                                                '<option value="问卷"  >问卷</option>' +
                                                '<option value="测试" >测试</option>' +                                             
                                                '<option value="勾选"  >勾选</option>' +
                                                '<option value="打分" >打分</option>' +
                                                '</select> </td>' +
                                   ' <td><input placeholder="模块" class="Text_lt " type="text" value="" /></td>'+
                                    '<td><input placeholder="链接" class="Text_lt " type="text" value="" /></td>'+
                                   ' <td><input style="width:40px" placeholder="占比" class="Text_lt  vali-Number" type="text" value="" />%</td>' +
                               ' </tr>';
        $table.append(tr);
    }

    var delRow = function (obj)
    {
        var $this = $(obj);        
        var delArr = [];
        $("[name='module']:checked").not(".js-check-all").each(function () {
            var $this = $(this), id = $this.attr("data-value");
            delArr.push(id); 
            $(this).parents("tr").remove();
        });
        if (delArr.length == 0) {
            showerror("至少选择一项");
            return;
        }
       
    }

    var isTitle = true;
    var addModel  =function()
    {
        isTitle = true;
        var res = $validate({ "select": "#module_table" });
        if (!res.isOK) {
            return;

       }
        var param =  new Array() 
        $('#module_table').find("tr").not(".fixThead").each(function () {
            res = {};
            res.type = $(this).find("select:first").find("option:selected").text();
            $(this).find("input").each(function () {
                res.classesid = $("#classid").val();
                if ($(this).attr('placeholder') == "模块")
                    res.name = $(this).val();
                else if ($(this).attr('placeholder') == "链接")
                    res.path = $(this).val();
                else if ($(this).attr('placeholder') == "占比")
                    res.percent = $(this).val();
                else if ($(this).attr('placeholder') == "标题") {
                    res.title = $(this).val();
                    if (res.title.length > 114) {//控制字数，模块图会出问题，一般为最多4个字
                        isTitle = false;
                    }
                }
                    
               
            });
            res.id = $(this).attr("data-id");
            if (res != {})
            param.push(res);
        });
         
        if (!isTitle) {
            showerror("标题长度最长为114");
            return;
        }
        showloading("操作中。。。");
        
        $.ajax({
            type: "POST",
            data: JSON.stringify({ "model": param.reverse(), "classid": $("#classid").val() }),
            url: "/Class/addClassModels",
            dataType: "json",
            contentType: 'application/json;charset=gb2312;'
        }).success(function (res) {
            if (res.success) {
                showok("操作成功！", function () { SearchClassModels(); });
            } else {
                showerror(res.message);
            }
            hideloading();
        }).error(function (xhr, status) {
            //alert(status);
        });
    }

    var SearchScore = function (indexpage, back)
    {
        oldindexscore = indexpage;
        var cookiename = "SearchScore";
        var res = $validate({ "select": "#div_06" });
        res.classid = $("#classid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/SearchScore", "model=" + JSON.stringify(res), function (data) {
            $("#res_score").html(data);
            hideloading();
        }, "html");
    }

    var saveScore = function () {
        var res = $validate({ "select": "#score_table" });
        if (!res.isOK) {
            return;

        }
        var param = new Array()
        $('#score_table').find("tr").not(".fixThead").each(function () {
            res = {};
            $(this).find("input").each(function () {
                res.classid = $("#classid").val();
                res.userid = $(this).attr("data-value");
                res.score = $(this).val();
            });
            if (res != {})
                param.push(res);
        });

        showloading("操作中。。。");

        $.ajax({
            type: "POST",
            data: JSON.stringify({ "model": param.reverse(), "classid": $("#classid").val() }),
            url: "/Class/SaveScore",
            dataType: "json",
            contentType: 'application/json;charset=gb2312;'
        }).success(function (res) {
            if (res.success) {
                showok("操作成功！", function () { SearchScore(); });
            } else {
                showerror(res.message);
            }
            hideloading();
        }).error(function (xhr, status) {
            //alert(status);
        });
    }

    var SearchState = function (indexpage, back) {
        oldindexstate= indexpage;
        var cookiename = "SearchState";
        var res = $validate({ "select": "#div_08" });
        res.classid = $("#classid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/SearchState", "model=" + JSON.stringify(res), function (data) {
            $("#res_state").html(data);
            hideloading();
        }, "html");
    }

    var saveState = function () {
          
        var param = new Array()
        $("[name='studystate']").not(".js-check-all").each(function () {
            var res = {};
            res.classid = $("#classid").val();
            res.userid = $(this).attr("data-value");
            res.modelid = $(this).attr("data-mid");
             
            if ($(this).prop('checked') == true) {
                res.isfinish = true;
                res.score = 100;
            }
            else {
                res.isfinish = false;
                res.score = 0;
            }
             if (res != {})
                param.push(res);
        });


        $("[name='studyinput']").each(function () {
            var res = {};
            res.classid = $("#classid").val();
            res.userid = $(this).attr("data-value");
            res.modelid = $(this).attr("data-mid");
            res.score = $(this).val();
            if (res.score !="")
                res.isfinish = true;
            else
                res.isfinish = false;
            if (res != {})
                param.push(res);
        });

        showloading("操作中。。。");

        $.ajax({
            type: "POST",
            data: JSON.stringify({ "model": param.reverse(), "classid": $("#classid").val() }),
            url: "/Class/SaveState",
            dataType: "json",
            contentType: 'application/json;charset=gb2312;'
        }).success(function (res) {
            if (res.success) {
                showok("操作成功！", function () { SearchState(oldindexstate,0); });
            } else {
                showerror(res.message);
            }
            hideloading();
        }).error(function (xhr, status) {
            //alert(status);
        });
    }

    var oldindexQA = 1;
    var SearchQA = function (indexpage, back) {
        oldindexQA = indexpage;
        var cookiename = "SearchQA";
        var res = $validate({ "select": "#div_07" });
        res.classid = $("#classid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/SearchQA", "model=" + JSON.stringify(res), function (data) {
            $("#QA_res").html(data);
            hideloading();
        }, "html");
    }

    var oldindexComment = 1;
    var SearchComment = function (indexpage, back) {
        oldindexComment = indexpage;
        var cookiename = "SearchComment";
        var res = $validate({ "select": "#div_07" });
        res.classid = $("#classid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/SearchComment", "model=" + JSON.stringify(res), function (data) {
            $("#Comment_res").html(data);
            hideloading();
        }, "html");
    }

    var oldindexAlbum = 1;
    var SearchAlbum = function (indexpage, back) {
        oldindexAlbum = indexpage;
        var cookiename = "SearchAlbum";
        var res = $validate({ "select": "#div_07" });
        res.classid = $("#classid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/SearchAlbum", "model=" + JSON.stringify(res), function (data) {
            $("#Album_res").html(data);
            hideloading();
        }, "html");
    }


    var AllDel = function (obj, parent) {
        var $this = $(obj), $inputs = $(parent).find("input:checked");
        if ($inputs.length == 0) {
            showerror("至少选择一项");
            return;
        }
        $this.customMessage({
            isDel: true,
            title: "",
            content: "是否确认删除？",
            clickSure: function () {
                var ids_parent = [];
                var ids = [];
                $inputs.each(function () {
                    var $this = $(this), id = $this.attr("data-id") || 0, parent_id = $this.attr("data-parentid") || 0;
                    if (id != 0) { ids.push(id);}
                    if (parent_id != 0) { ids_parent.push(parent_id); }
                });
                if (ids.length > 0 || ids_parent.length > 0) {
                    showloading("提交中...");
                    var type = "";
                    var url = "";
                    switch (parent) {
                        case "#div_03":
                            type = "QA";
                            url = "/Class/Del";
                            break;
                        case "#div_04":
                            type = "Comment";
                            url = "/Class/Del";
                            break;
                        case "#div_05":
                            type = "Ablum";
                            url = "/Class/DelAblum";
                            break;
                    }
                    var paras = {
                        ids: ids,
                        ids_parent: ids_parent,
                        type: type
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(paras),
                        url: url,
                        dataType: "json",
                        contentType: 'application/json;charset=gb2312;'
                    }).success(function (res) {
                        if (res.success) {
                            showok("操作成功！", function () {
                                switch (parent) {
                                    case "#div_03":
                                        SearchQA(oldindexQA, 0);
                                        break;
                                    case "#div_04":
                                        SearchComment(oldindexComment, 0);
                                        break;
                                    case "#div_05":
                                        SearchAlbum(oldindexAlbum, 0);
                                        break;
                                }
                            });
                        } else {
                            showerror(res.message);
                        }
                        hideloading();
                    }).error(function (xhr, status) {
                        //alert(status);
                    });
                }
            }
        });
    }




    var SearchClassRank = function (indexpage, back)
    {
        oldindexrank = indexpage;
        var cookiename = "SearchState";
        var res = $validate({ "select": "#div_07" });
        res.classid = $("#classid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        res = SetSearchCondition(back, res, cookiename);

        showloading("搜索中。。。");
        $ajax("/Class/SearchClassRank", "model=" + JSON.stringify(res), function (data) {
            $("#res_rank").html(data);
            hideloading();
        }, "html");
    }

    var handle = function () {

        $("#schooltime").datetimepicker();

        $(document).on("click", "[data-toggle='tab']", function () {
            var $this = $(this), classname = $this.attr("class"), htmlVal = $this.html(), hrefid = $this.attr("data-href");
            if (classname == "active") {
                return;
            } else {
                $("[data-toggle='tab']").closest("li").removeClass("active");
                $this.closest("li").addClass("active");
                $(".tab-content .tab-pane").removeClass("active");
                $("" + hrefid).addClass("active");
                if (htmlVal == "学员") {
                    SearchUser(1, 0);
                } else if (htmlVal == "课程模块") {
                    SearchClassModels(1, 0);
                }
                else if (htmlVal == "成绩") {
                    SearchScore(1, 0);
                }
                else if (htmlVal == "进度") {
                    SearchState(1, 0);
                } else if (htmlVal == "问答") {
                    SearchQA(1, 0);
                } else if (htmlVal == "评价") {
                    SearchComment(1, 0);
                } else if (htmlVal == "相册") {
                    SearchAlbum(1, 0);
                } else if (htmlVal == "排名") {
                    SearchClassRank(1, 0);
                } else if (htmlVal == "上课地址")
                {
                    SearchAddress(1, 0);
                }
                
            }
        });


        //$("[data-toggle='tab']").eq(2).click();

        $(document).on("click", "li i", function () {
            var $this = $(this)
                , name = $this.html()
            , $list = $this.closest(".ck_select").next("ul");
            //, $answerlist = $this.closest(".ck_select").next("[data-name='answerlist']");
            if ($list.length == 0) {
                $list = $this.closest(".ck_select").next("dl");
                if ($list.length == 0) {
                    $list = $this.closest(".ck_select").next(".tablewrap");
                }
            }
            if (name == "展开") {
                $this.removeClass("rot-r90").addClass("rot-r270").html("收起");
                $list.show();
                //$answerlist.show();
            }else{
                $this.removeClass("rot-r270").addClass("rot-r90").html("展开");
                $list.hide();
               // $answerlist.hide();
            }
        });

        //SearchUser(1, $("#back").val());

        $("#savebtn_base").click(function () {
            SaveBaseInfo();
        });

        $("#add_address").click(function () {
            showAddressPlane();
        });

        $("#savebtn_address").click(function () {
            addAddress();
        });
        
        $("#del_address").click(function () {
            delAddress(this);
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

        $("#savebtn_user").click(function () {
            addUser();
        });
        
        $("#add_row").click(function () {
            addRow();
        });
        $("#del_row").click(function () {
            delRow(this);
        });
        
        $("#del_user").click(function () {
            del(this);
        });
        $("#btn_SearchScore").click(function () {
            SearchScore(1, 0);
        });
        $("#btn_SearchState").click(function () {
            SearchState(1, 0);
        });
        $("#savebtn_model").click(function () {
            addModel();
        });
        $("#savebtn_score").click(function () {
            saveScore();
        });
        $("#savebtn_state").click(function () {
            saveState();
        });

        //全选与删除
        $("[name='AllSelect']").click(function () {
            var $this = $(this),parent = $this.attr("data-href");
            if ($this.attr("data-all") == "yes") {
                $(parent + " input").prop("checked", "");
                $this.attr("data-all", "no");
            }else{
                $(parent + " input").prop("checked", "checked");
                $this.attr("data-all", "yes");
            }
        });

        $("[name='AllDel']").click(function () {
            var $this = $(this), parent = $this.attr("data-href");
            AllDel(this, parent);
        });
        $("#datebegin").datetimepicker();

    }

    return {
        init: function () {
            handle();
            window.SearchUser = SearchUser;
            
        }
    };
}();


