var pagejs = function () {
     
    var oldindexstate = 1;  

    var showJSGL = function (obj, title) {
        showerror(title);
    }

    var SearchCourse = function (indexpage, back) {
        
        oldindexscore = indexpage;
        var cookiename = "SearchClassModels";
        var res = $validate({ "select": "#div_02" });
        res.teacherid = $("#teacherid").val();
        res.pageIndex = indexpage;
        res.pageSize = 15;
        if (!res.isOK) {
            return;
        }
        showloading("搜索中。。。");
        $ajax("/Teacher/CourseList", "model=" + JSON.stringify(res), function (data) {
            $("#res_course").html(data);
            hideloading();
        }, "html");
    }

    var SaveCourse = function () {
        var res = $validate({ "select": "#div_01" });
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
    //封面上传初始化
    var initupload = function (fileupload) {
        if ($('#' + fileupload).length > 0) {

            $(document).on('click', ".close", function () {
                var $this = $(this), $parent = $this.closest(".upload_img");
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
                            $("#cv").val(info.url);

                        }
                    }
                });
            }, 10);
        }
    }
    //资料上传初始化
    var initupload_zl = function (fileupload) {
        if ($('#' + fileupload).length > 0) {

            $(document).on('click', ".close", function () {
                var $this = $(this), $parent = $this.closest(".upload_img");
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
                    'fileSizeLimit': '5MB',//文件的极限大小，(B, KB, MB, or GB)
                    'fileTypeExts': '*.pdf;*.doc;*.docx;*.excel;',
                    'fileTypeDesc': '请选择pdf,doc,excel',
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
                            $("#picList_zl div").html(info.original);
                            $("#picList_zl").show();
                            $("#cover_zl").val(info.original);
                            $("#coverurl_zl").val(info.url);
                            $("#cv").val(info.url);
                        }
                    }
                });
            }, 10);
        }
    }
    //资料添加处对象
    var $this_ZL = null;
    //资料信息dom添加
    var setHtml = function (res) {
        //alert($this_ZL.html());
        $this_ZL.append(res);
        $("#open_panel").hide();
    }
    //生成资料dom
    var createZL = function (title, name, url) {
        var html = '';
        html += '<div class="dml fl"><div class="dt"><a href="' + url + '" target="_blank">' + name + '</a></div><div class="fr"> ';
        html += '<a class="del_btn" href="javascript:void(0)"></a></div>';
        html += '<input type="hidden" value="' + url + '"/>';
        html += '</div>';
        return html;
    }
    //生成主目录dom
    var createMain = function () {
        var html = '';
        html += '<div class="graph_cont clearfix"><label><input type="checkbox" name="graph_cont"></label>'
        html += '<div class="fr graph_detail"><div class="clearfix"><input class="gtxt vali-Required" type="text" autocomplete="off">';
        html += '<div class="gtxt_small_btn"><a class="btn_show" href="javascript:;" data-name="gtxt">添加字节</a></div></div>';
        html += '<div data-name="gtxt_samll_wrap"><div class="clearfix" data-name="gtxt_small"><input class="gtxt_small vali-Required" type="text" autocomplete="off">';
        html += '<div class="green_btn" style="display:inline-block;"><a href="javascript:void(0)">资料</a></div><div class="green_btn_del"><a class="btn_show" href="javascript:void">删除</a></div>';
        html += '</div><div class="d_m clearfix"></div>';
        html += '</div></div></div>';
        $("#res_main").append(html);
    }
    //生成子目录dom
    var createSub = function ($this) {
        var html = ''
        html += '<div data-name="gtxt_samll_wrap"><div class="clearfix" data-name="gtxt_small"><input class="gtxt_small vali-Required" type="text" autocomplete="off"><div class="green_btn" style="display:inline-block;"><a href="javascript:void(0)">资料</a></div><div class="green_btn_del"><a class="btn_show" href="javascript:void">删除</a></div>';
        html += '<div class="d_m clearfix"></div></div></div>';
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
    var createAttch = function (id, name, path, subcatalogid) {
        var obj = new Object();
        obj.id = id;
        obj.subcatalogid = subcatalogid;
        obj.name = name;
        obj.path = path;
        obj.state = 1;
        obj.get = function () {
            return obj.id + "," + obj.subcatalogid + "," + obj.name + "," + obj.path + "," + obj.state + ";";
        }
        return obj;
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
                if (htmlVal == "基本信息") {
                    SearchUser(1, 0);
                } else if (htmlVal == "课程") {
                    SearchCourse(1, 0);
                }
                

            }
        });

       // initupload("fileupload");
        initupload_zl("fileupload");
      

        $("#btn_SearchCourse").click(function () {
            SearchCourse(1, 0);
        });
        $("#savebtn").click(function () {
            SaveCourse();
        });
        
    }

    return {
        init: function () {
            handle();
            window.SearchCourse = SearchCourse;

        }
    };
}();


