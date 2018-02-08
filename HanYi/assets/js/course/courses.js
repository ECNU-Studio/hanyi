var pagejs = function () {
    //获取目录
    var GetCatalog = function () {
        res = { "id": $("#coursesid").val() };
        showloading("加载中。。。");
        $ajax("/Courses/GetCatalog", "model=" + JSON.stringify(res), function (data) {
            $("#res_main").html(data);
            hideloading();
        }, "html");

    }
    //编辑课程
    var editCourses = function () {
        var res = $validate({ "select": "#div_01" });
        if (!res.isOK) {
            return;
        }
        res.id = res.coursesid;
        res.abstractFile = res.coverurl_jj;
        res.abstractFilesize = res.filesize_jj;
        res.abstractFilename = res.name_jj;
        showloading("操作中。。。");
        $ajax("/Courses/ModelPostData", JSON.stringify({ "model": res }), function (res) {
            hideloading();
            if (res.success) {
                $("#open_panel").hide();
                showok("保存成功!", function () { local.reload(); });
            } else {
                showerror(res.message);
            }
        });
    }
    //弹出面板
    var showPlane = function () {
        $("#open_panel input[type!='hidden']").val("");
        $("#open_panel").show();
    }

    var showPlaneLink = function () {
        $("#open_panel_link input[type!='hidden']").val("");
        $("#open_panel_link").show();
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
    //勋章上传初始化
    var initupload_xz = function (fileupload) {
        if ($('#' + fileupload).length > 0) {

            $(document).on('click', "#picList_xz .close", function () {
                var $this = $(this), $parent = $this.closest("#picList_xz");
                $("#honor").val("");
                $parent.find("img").attr("src", "");
                $("#picList_xz").hide();
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
                            $("#picList_xz img").attr("src", "");
                            $("#picList_xz").hide();
                            $("#honor").val("");
                            $("#picList_xz img").attr("src", info.url);
                            $("#picList_xz").show();
                            $("#honor").val(info.url);

                        }
                    }
                });
            }, 10);
        }
    }
    //资料上传初始化
    var initupload_zl = function (fileupload) {
        if ($('#' + fileupload).length > 0) {

            $(document).on('click', "#picList_zl .close", function () {
                var $this = $(this), $parent = $this.closest("#picList_zl");
                $("#cover_zl").val("");
                $("#coverurl_zl").val("");
                $parent.find("img").attr("src", "");
                $("#picList_zl").hide();
            });

            setTimeout(function () {
                $('#' + fileupload).uploadify({
                    'swf': UploadCss.swf,
                    'uploader': UploadUrl,
                    'cancelImg': UploadCss.cancelImg,
                    'fileSizeLimit': '20MB',//文件的极限大小，(B, KB, MB, or GB)
                    'fileTypeExts': '*.pdf;*.doc;*.docx;*.excel;*.xls;*.xlsx;*.mp3;*.mp4;*.avi;*.rmvb;*.3gp;*.wma;*.ppt;*.pptx',
                    'fileTypeDesc': '请选择pdf,doc,excel,mp3,mp4,avi,rmvb,3gp,wma,ppt',
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

                        if (info.state == "SUCCESS") {//info.original
                            $("#picList_zl div").html("");
                            $("#picList_zl").hide();
                            $("#cover_zl").val("");
                            $("#coverurl_zl").val("");
                            $("#filesize_zl").val("");
                            $("#picList_zl div").html(info.original);
                            $("#picList_zl").show();
                            $("#cover_zl").val(info.original);
                            $("#coverurl_zl").val(info.url);
                            $("#filesize_zl").val(info.filesize);
                            var pos = info.original.lastIndexOf(".");
                            var name = info.original.substring(0, pos);
                            $("#name_zl").val(name);
                        }
                    }
                });
            }, 10);
        }
    }
    //资料添加处对象
    var $this_ZL = null;
    var $this_ZL_link = null;
    //资料信息dom添加
    var setHtml = function (res) {
        //alert($this_ZL.html());
        $this_ZL.append(res);
        $("#open_panel").hide();
    }
    var setHtml_link = function (res) {
        $this_ZL_link.append(res);
        $("#open_panel_link").hide();
    }
    //生成资料dom
    var createZL = function (title, name, url, filesize) {
        var html = '';
        html += '<div class="dml fl" data-size="' + filesize + '"><div class="dt"><a href="' + url + '" target="_blank">' + name + '</a></div><div class="fr"> ';
        html += '<a class="del_btn" href="javascript:void(0)"></a></div>';
        html += '<input type="hidden" value="' + url + '"/>';
        html += '</div>';
        return html;
    }
    var createZL_link = function (title, name, url, filesize) {
        var html = '';
        html += '<div class="dml fl" data-type="link" data-size="' + filesize + '"><div class="dt"><a href="' + url + '" target="_blank">' + name + '</a></div><div class="fr"> ';
        html += '<a class="del_btn" href="javascript:void(0)"></a></div>';
        html += '<input type="hidden" value="' + url + '"/>';
        html += '</div>';
        return html;
    }
    //生成主目录dom
    var createMain = function () {
        var html = '';
        html += '<div class="graph_cont clearfix"><label><input type="checkbox" name="graph_cont"></label>'
        html += '<div class="fr graph_detail"><div class="clearfix"><input class="gtxt vali-Required" placeholder="添加本章标题" type="text" autocomplete="off" />';
        html += '<div class="gtxt_small_btn"><a class="btn_show" href="javascript:;" data-name="gtxt">章节</a></div></div>';
        html += '<div data-name="gtxt_samll_wrap"><div class="clearfix" data-name="gtxt_small"><input class="gtxt_small vali-Required" type="text" placeholder="添加本节标题" autocomplete="off" />';
        html += '<div class="green_btn" style="display:inline-block;"><a href="javascript:void(0)">资料</a></div>';
        html += '<div class="green_btn_link" style="display:inline-block;"><a href="javascript:void(0)">外链</a></div>';
        html += '<div class="green_btn_del"><a class="btn_show" href="javascript:void">删除</a></div>';
        html += '</div><div class="d_m clearfix"></div>';
        html += '</div></div></div>';
        $("#res_main").append(html);
    }
    //生成子目录dom
    var createSub = function ($this) {
        var html = ''
        html += '<div data-name="gtxt_samll_wrap"><div class="clearfix" data-name="gtxt_small"><input class="gtxt_small vali-Required" type="text" autocomplete="off" />';
        html += '<div class="green_btn" style="display:inline-block;"><a href="javascript:void(0)">资料</a></div>';
        html += '<div class="green_btn_link" style="display:inline-block;"><a href="javascript:void(0)">外链</a></div>';
        html += '<div class="green_btn_del"><a class="btn_show" href="javascript:void">删除</a></div>';
        html += '</div><div class="d_m clearfix"></div></div>';
        $this.closest(".graph_detail").append(html);
    }

    //生成主目录对象
    var createCatalog = function (id, name, courseid, subcatalog) {
        var obj = new Object();
        obj.id = id;
        obj.courseid = courseid;
        obj.name = name;
        obj.subcatalog = subcatalog;
        obj.state = 1;
        obj.get = function () {
            return obj.id + "," + obj.courseid + "," + obj.name + "," + obj.subcatalog + "," + obj.state + ";";
        }
        return obj;
    }
    //生成子目录对象
    var createSubcatalog = function (id, name, catalogid, subcatalogattachment) {
        var obj = new Object();
        obj.id = id;
        obj.catalogid = catalogid;
        obj.name = name;
        obj.subcatalogattachment = subcatalogattachment;
        obj.state = 1;
        obj.get = function () {
            return obj.id + "," + obj.catalogid + "," + obj.name + "," + obj.subcatalogattachment + "," + obj.state + ";";
        }
        return obj;
    }
    //生成资料对象
    var createAttch = function (id, name, path, size, subcatalogid, type) {
        var obj = new Object();
        obj.id = id;
        obj.subcatalogid = subcatalogid;
        obj.name = name;
        obj.path = path;
        obj.size = size;
        obj.state = 1;
        obj.type = type;
        obj.get = function () {
            return obj.id + "," + obj.subcatalogid + "," + obj.name + "," + obj.path + "," + obj.size + "," + obj.type + "," + obj.state + ";";
        }
        return obj;
    }

    //保存目录
    var resultArr = [];
    var subcatalogArr = [];
    var subcatalogattachmentArr = [];
    var isSave = false;
    var saveZJ = function () {
        if (isSave) {
            return;
        }
        isSave = true;
        var res = $validate({ "select": "#res_main" });
        if (!res.isOK) {
            return;
        }
        resultArr = [];
        $(".graph_cont").each(function () {
            subcatalogArr = [];
            var $this = $(this), $main = $this.find("input.gtxt");
            $subs = $this.find("[data-name='gtxt_samll_wrap']");
            $subs.each(function () {
                var $thissub = $(this), $itemsub = $thissub.find(".d_m");
                subcatalogattachmentArr = [];
                $itemsub.find(".dml").each(function () {
                    var $thisattach = $(this)
                    , name = $thisattach.find("a").html()
                    , path = $thisattach.find("input").val()
                    , id = $thisattach.attr("data-id") || 0
                    , size = $thisattach.attr("data-size") || 0
                    , subcatalogid = $thisattach.attr("data-subcatalogid") || 0
                    , type = $thisattach.attr("data-type") || "";

                    subcatalogattachmentArr.push(createAttch(id, name, path, size, subcatalogid, type));
                });

                var name = $thissub.find("input.gtxt_small").val()
                    , id = $thissub.attr("data-id") || 0
                    , catalogid = $thissub.attr("data-catalogid") || 0;
                subcatalogArr.push(createSubcatalog(id, name, catalogid, subcatalogattachmentArr))
            });
            var name = $main.val()
            , coursesid = $("#coursesid").val()
            , id = $main.attr("data-id") || 0;
            resultArr.push(createCatalog(id, name, coursesid, subcatalogArr));
        });
        if (resultArr.length == 0) {
            return;
        }
        //console.log(resultArr); return;
        var paras = {
            ids: resultArr
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(paras),
            url: "/Courses/CoursesCatalogAdd",
            dataType: "json",
            contentType: 'application/json;charset=gb2312;'
        }).success(function (res) {
            if (res.success) {
                showok("操作成功！", function () {
                    GetCatalog();
                    //location.reload();
                });
            } else {
                showerror(res.message);
            }
            isSave = false;
            hideloading();
        }).error(function (xhr, status) {
            isSave = false;
            //alert(status);
        });
    }
    //删除目录
    var delMain = function (obj) {
        var $this = $(obj), $inputs = $("[name='graph_cont']:checked");
        if ($inputs.length == 0) {
            showerror("至少选择一项");
            return;
        }
        $this.customMessage({
            isDel: true,
            title: "",
            content: "是否确认删除？",
            clickSure: function () {
                var ids = [];
                $inputs.each(function () {
                    var $this = $(this), id = $this.attr("data-id") | 0;
                    if (id == 0) {
                        $this.closest(".graph_cont").remove();
                    } else {
                        ids.push(id);
                    }
                });
                if (ids.length > 0) {
                    showloading("提交中...");
                    var paras = {
                        ids: ids
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(paras),
                        url: "/Courses/DelMain",
                        dataType: "json",
                        contentType: 'application/json;charset=gb2312;'
                    }).success(function (res) {
                        if (res.success) {
                            showok("操作成功！", function () {
                                GetCatalog();//location.reload();
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

    //简介附件上传初始化
    var initupload_jj = function (fileupload) {
        if ($('#' + fileupload).length > 0) {


            $(document).on("click", ".del_btn_jj", function () {
                var $this = $(this), $item = $this.closest("div");
                $this.customMessage({
                    isDel: true,
                    title: "",
                    content: "是否确认删除？",
                    clickSure: function () {
                        $("#picList_jj em").html("");
                        $("#picList_jj").hide();
                        $("#name_jj").val("");
                        $("#coverurl_jj").val("");
                        $("#filesize_jj").val("");
                    }
                });
            });

            setTimeout(function () {
                $('#' + fileupload).uploadify({
                    'swf': UploadCss.swf,
                    'uploader': UploadUrl,
                    'cancelImg': UploadCss.cancelImg,
                    'fileSizeLimit': '20MB',//文件的极限大小，(B, KB, MB, or GB)
                    'fileTypeExts': '*.pdf;*.doc;*.docx;*.excel;*.xls;*.xlsx;*.mp3;*.mp4;*.avi;*.rmvb;*.3gp;*.wma;*.ppt;*.pptx',
                    'fileTypeDesc': '请选择pdf,doc,excel,mp3,mp4,avi,rmvb,3gp,wma,ppt',
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

                        if (info.state == "SUCCESS") {//info.original
                            $("#picList_jj em").html("");
                            $("#picList_jj").hide();
                            $("#cover_jj").val("");
                            $("#coverurl_jj").val("");
                            $("#filesize_jj").val("");
                            $("#picList_jj em").html(info.original);
                            $("#picList_jj").show();
                            $("#cover_jj").val(info.original);
                            $("#coverurl_jj").val(info.url);
                            $("#filesize_jj").val(info.filesize);
                            var pos = info.original.lastIndexOf(".");
                            var name = info.original.substring(0, pos);
                            $("#name_jj").val(name);
                        }
                    }
                });
            }, 10);
        }
    }

    var handle = function () {

        $(document).on("click", "[data-toggle='tab']", function () {
            var $this = $(this), classname = $this.attr("class"), htmlVal = $this.html(), hrefid = $this.attr("data-href");
            if (classname == "active") {
                return;
            } else {
                $("[data-toggle='tab']").closest("li").removeClass("active");
                $this.closest("li").addClass("active");
                $(".tab-content .tab-pane").removeClass("active");
                $("" + hrefid).addClass("active");
                switch (htmlVal) {
                    case "基本信息":
                        break;
                    case "目录与下载":
                        GetCatalog();
                        break;
                }
            }
        });

        initupload_jj("fileupload_jj");
        initupload("fileupload");
        initupload_zl("fileupload_zl");
        initupload_xz("fileupload_xz");


        $("#addMain").click(function () {
            createMain();
        });

        $(document).on("click", "[data-name='gtxt']", function () {
            var $this = $(this);
            createSub($this);
        });

        $(document).on("click", ".green_btn_del", function () {
            var $this = $(this).closest("[data-name='gtxt_samll_wrap']");
            var html = $this.html();
            $this.remove();
        });

        $(document).on("click", ".green_btn", function () {
            $("#name_zl").val("");
            $("#picList_zl div").html("");
            $("#picList_zl").hide();
            $("#cover_zl").val("");
            $("#coverurl_zl").val("");
            $this_ZL = $(this).closest("[data-name='gtxt_small']").next(".d_m");
            //console.log($this_ZL);
            //$("#savebtn_zl").attr("data-obj", $this);
            showPlane();
        });

        $(document).on("click", ".green_btn_link", function () {
            $("#cover_zl_link").val("");
            $this_ZL_link = $(this).closest("[data-name='gtxt_small']").next(".d_m");
            showPlaneLink();
        });


        $(document).on("click", "#savebtn_zl", function () {
            var res = $validate({ "select": "#open_panel" });
            if (!res.isOK) {
                return;
            }
            var resHtml = createZL(res.name_zl, res.cover_zl, res.coverurl_zl, res.filesize_zl);
            setHtml(resHtml);
        });

        $(document).on("click", "#savebtn_zl_link", function () {
            var res = $validate({ "select": "#open_panel_link" });
            if (!res.isOK) {
                return;
            }
            var resHtml = createZL_link("", res.name_zl_link, res.cover_zl_link, 0);
            setHtml_link(resHtml);
        });
        
        $(document).on("click", ".del_btn", function () {
            var $this = $(this), $item = $this.closest(".dml");
            $this.customMessage({
                isDel: true,
                title: "",
                content: "是否确认删除？",
                clickSure: function () {
                    $item.remove();
                }
            });
        });

        $(document).on("click", "#delMain", function () {
            delMain(this);
        });

        $("#savebtn_zj").click(function () {
            saveZJ();
        });

        $("#showAdd").click(function () {
            showPlane();
        });

        $("#savebtn").click(function () {
            editCourses();
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
            window.GetCatalog = GetCatalog;
        }
    };
}();


