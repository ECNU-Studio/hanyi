var pagejs = function () {
    var uploader = null;
    var r1 = 40; // radius of inner mask
    var r2 = 60;// radius of circle boundary
    var r3 = 80; // radius of annotation circle
    var ox = 140;
    var oy = 155;
    var ft_size = 12;

    var get_pt = function (deg, r, ox, oy) {
        var x = ox + r * Math.cos(deg * Math.PI / 180);
        var y = oy + r * Math.sin(deg * Math.PI / 180);
        return [x, y];
    }
    var get_circ_pt = function (deg) {
        return get_pt(deg, r2, ox, oy);
    }
    var get_anno_pt = function (deg) {
        return get_pt(deg, r3, ox, oy);
    }

    var get_anno_pt2 = function (deg) {
        var p = get_pt(deg, r3 + ft_size * 3, ox, oy);
        var p1 = p[1];
        if (p[1] > oy) {
            p1 = p[1] - ft_size * 2;
        } else if (p[1] == oy) {
            p1 = p[1] - ft_size * 0.5;
        }
        var p0 = p[0];
        if (p0 > ox) {
            if (Math.abs(p[1] - oy) < oy / 2) {
                p0 = 140 + ft_size * 6 + 2;
            } else {
                p0 = 140 + ft_size;
            }
        } else {
            p0 = p0 - (ft_size * 3);
        }
        return [p0, p1];
    }

    var draw = function (parts_spec) {


        var total = 0;
        for (var i in parts_spec) {
            total += parts_spec[i].share;
        }


        var result = [];
        var agg_share = 0;
        for (var i in parts_spec) {
            var start_share = agg_share;
            var end_share = start_share + parts_spec[i].share;
            var a = get_circ_pt(start_share * 360 / total - 90);
            if (parts_spec > 1) { var b = get_circ_pt(end_share * 360 / total - 90); }
            else { var b = get_circ_pt(end_share * 360 / total - 0.1 - 90); }
            var c = get_anno_pt(((start_share + end_share) / 2) * 360 / total - 90);
            var c2 = get_anno_pt2(((start_share + end_share) / 2) * 360 / total - 90);
            agg_share = end_share;
            var s = '<path d="M' + ' ' + ox + ' ' + oy
                    + ' L ' + a[0] + ' ' + a[1]
                    + ' A ' + r2 + ' ' + r2 + ' 0 ' + ((parts_spec[i].share / total) > 0.5 ? 1 : 0) + ' 1 ' + b[0] + ' ' + b[1]
                    + ' Z'
                    + '" fill="' + parts_spec[i].color + '"/>';
            result.push(s);
            s = '<path d="M' + ' ' + ox + ' ' + oy
                    + ' L ' + c[0] + ' ' + c[1]
                    + '" stroke="' + parts_spec[i].color + '" stroke-width="1"/>';
            result.push(s);
            var descTotal = parts_spec[i].desc;
            var descText = "";
            var descPer = "";
            if (descTotal.split(':').length == 2) {
                descText = descTotal.split(':')[0];
                descPer = descTotal.split(':')[1];
            }

            s = '<foreignObject x="' + c2[0] + '" y="' + c2[1] + '" fill="' + parts_spec[i].color + '" width="' + (ft_size * 6 + 4) + '" height="' + (ft_size * 2.5 + 4) + '">' +
                '<div style="width:' + (ft_size * 6 + 2) + 'px;height:' + (ft_size + 2) + 'px;font-size:' + ft_size + 'px;text-align:center;color:' + parts_spec[i].color + ';">' +
                '<div style="overflow:hidden;text-overflow:ellipsis;white-space:nowrap;width:100%;">' + descText + '</div>' +
                '<div style="margin-top:5px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;width:100%;">' + descPer + '</div>' +
                '</div>' +
                '</foreignObject>';
            result.push(s);
        }
        s = '<circle cx="' + ox + '" cy="' + oy + '" r="' + r1 + '" fill="white"/>';
        result.push(s);
        // add separator
        var agg_share = 0;
        if (parts_spec.length > 1) {
            for (var i in parts_spec) {
                var a = get_circ_pt(agg_share * 360 / total - 90 - 1);
                var b = get_circ_pt(agg_share * 360 / total - 90 + 1);
                var s = '<path d="M' + ' ' + ox + ' ' + oy
                        + ' L ' + a[0] + ' ' + a[1]
                        + ' A ' + r2 + ' ' + r2 + ' 0 0 1 ' + b[0] + ' ' + b[1]
                        + ' Z'
                        + '" fill="white"/>';
                result.push(s);
                agg_share += parts_spec[i].share;
            }
        }
        // return
        //return result.join("\n");
        $("#annulus").html(result.join("\n"));
    }







    var gallery = null;
    var initPhotoSwipeFromDOM = function (gallerySelector) {

        var parseThumbnailElements = function (el) {
            var items = [];

            $(el).children("dd").each(function (index, obj) {
                var str = $(obj).children("img").attr('data-size').split('X');
                var w = 400;
                var h = 600;
                if (str.length == 2) {
                    w = str[0];
                    h = str[1];
                }
                // create slide object
                item = {
                    src: $(obj).children("img").attr('src'),
                    w: w,
                    h: h,
                    author: "",
                    el: obj
                };
                // "medium-sized" image
                item.m = {
                    src: $(obj).children("img").attr('src'),
                    w: w,
                    h: h
                };

                // original image
                item.o = {
                    src: $(obj).children("img").attr('src'),
                    w: w,
                    h: h
                };

                items.push(item);
            });

            return items;
        };

        var onThumbnailsClick = function (e) {
            e = e || window.event;
            e.preventDefault ? e.preventDefault() : e.returnValue = false;

            var eTarget = e.target || e.srcElement;

            var clickedListItem = eTarget;

            if (!clickedListItem) {
                return;
            }

            var clickedGallery = $(clickedListItem).parent().parent();

            var childNodes = $(clickedGallery).find("img"),

                numChildNodes = childNodes.length,
                nodeIndex = 0,
                index;

            $(childNodes).each(function (curindex, obj) {
                if (obj === clickedListItem) {
                    index = curindex;
                }
            });

            if (index >= 0) {
                openPhotoSwipe(index, clickedGallery);
            }
            return false;
        };

        var openPhotoSwipe = function (index, galleryElement, disableAnimation, fromURL) {
            var pswpElement = document.querySelectorAll('.pswp')[0],
               //gallery,
                options,
                items;

            items = parseThumbnailElements(galleryElement);

            // define options (if needed)
            options = {

                galleryUID: $(galleryElement).attr('data-pswp-uid'),

                getThumbBoundsFn: function (index) {
                    // See Options->getThumbBoundsFn section of docs for more info
                    var thumbnail = items[index].el.children[0],
                        pageYScroll = window.pageYOffset || document.documentElement.scrollTop,
                        rect = thumbnail.getBoundingClientRect();

                    return { x: rect.left, y: rect.top + pageYScroll, w: rect.width };
                },

                addCaptionHTMLFn: function (item, captionEl, isFake) {
                    if (!item.title) {
                        captionEl.children[0].innerText = '';
                        return false;
                    }
                    captionEl.children[0].innerHTML = item.title;
                    return true;
                }

            };


            if (fromURL) {
                if (options.galleryPIDs) {
                    // parse real index when custom PIDs are used
                    // http://photoswipe.com/documentation/faq.html#custom-pid-in-url
                    for (var j = 0; j < items.length; j++) {
                        if (items[j].pid == index) {
                            options.index = j;
                            break;
                        }
                    }
                } else {
                    options.index = parseInt(index, 10) - 1;
                }
            } else {
                options.index = parseInt(index, 10);
            }

            // exit if index not found
            if (isNaN(options.index)) {
                return;
            }

            options.mainClass = 'pswp--minimal--dark';
            options.barsSize = { top: 0, bottom: 0 };
            options.captionEl = false;
            options.fullscreenEl = false;
            options.shareEl = false;
            options.bgOpacity = 0.85;
            options.tapToClose = true;
            options.tapToToggleControls = false;

            if (disableAnimation) {
                options.showAnimationDuration = 0;
            }

            // Pass data to PhotoSwipe and initialize it
            gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, items, options);

            // see: http://photoswipe.com/documentation/responsive-images.html
            var realViewportWidth,
                useLargeImages = false,
                firstResize = true,
                imageSrcWillChange;

            gallery.listen('beforeResize', function () {

                var dpiRatio = window.devicePixelRatio ? window.devicePixelRatio : 1;
                dpiRatio = Math.min(dpiRatio, 2.5);
                realViewportWidth = gallery.viewportSize.x * dpiRatio;


                if (realViewportWidth >= 1200 || (!gallery.likelyTouchDevice && realViewportWidth > 400) || screen.width > 1200) {
                    if (!useLargeImages) {
                        useLargeImages = true;
                        imageSrcWillChange = true;
                    }

                } else {
                    if (useLargeImages) {
                        useLargeImages = false;
                        imageSrcWillChange = true;
                    }
                }

                if (imageSrcWillChange && !firstResize) {
                    gallery.invalidateCurrItems();
                }

                if (firstResize) {
                    firstResize = false;
                }

                imageSrcWillChange = false;

            });

            gallery.listen('gettingData', function (index, item) {
                if (useLargeImages) {
                    item.src = item.o.src;
                    item.w = item.o.w;
                    item.h = item.o.h;
                } else {
                    item.src = item.m.src;
                    item.w = item.m.w;
                    item.h = item.m.h;
                }
            });

            gallery.listen('close', function () {
                if (gallery != null) {
                    gallery.close();
                    gallery = null;
                }
            });
            gallery.init();

        };

        $(gallerySelector).click(function () {
            onThumbnailsClick();
        });

    };

    var MarkOrder = function () {
        $ajax("/MobileUser/MarkOrder", "id=" + $("#id").val() , function (data) {
            $("#div_04").html(data);
            hideloading();
        }, "html");
    }
    
    var HuDongQA = function()
    {
         
        $("#hudong_qa").html("");
        $ajax("/MobileUser/HuDongQA", "id=" + $("#id").val(), function (data) {

            $("#hudong_qa").html(data);
            initPhotoSwipeFromDOM('.PhotoSwipe_QA');
            $("#btn_tiwen").unbind("click");
            $("#btn_tiwen").click(function () {
                $("#itemid").val("");
                //$("#txtdetail").css("width", "60%");
                $("#txtdetail").attr("placeholder", "发表下评论");
                $("#txtdetail").focus();
                $("#btn_send").html("提问");
                $("#Comment").show();
                $("#btn_add").show();
                initupfile();
            });

            $(".mainlayer").unbind("click");
            $(".mainlayer").click(function () {
                var $this = $(this);
                setTimeout(function () {
                    if ($this.hasClass("activeRep")) {
                        $("#txtdetail").focus();
                    }
                }, 200);
            });
        }, "html");
    }

    var HuDongQAKeyWord = function (isEmpty) {
        var content = $("#searchword").val();
        if (content == undefined || isEmpty)
            content = "";
        $("#listqa").html("<div class='small_loading'></div>");
        $ajax("/MobileUser/HuDongQAKeyWord", "id=" + $("#id").val() + "&content=" + content, function (data) {
            $("#listqa").html(data);
            initPhotoSwipeFromDOM('.PhotoSwipe_QA');
        }, "html");
    }

    var HuDongComment = function () {
        $("#hudong_comment").html("");
        $ajax("/MobileUser/HuDongComment", "id=" + $("#id").val(), function (data) {
            $("#hudong_comment").html(data);
        }, "html");
    }

    var HuDongCommentList = function () {
        $("#commentList").html("<div class='small_loading'></div>");
        $ajax("/MobileUser/HuDongCommentList", "id=" + $("#id").val(), function (data) {
            $("#commentList").html(data);
        }, "html");
    }

    var HuDongAlbum = function (yes) {
        var yes = "" || yes;
        if (yes != "") {
            $("#hudong_album").html("<div class='small_loading'></div>");
        }
        else {
            $("#hudong_album").html("");
        }
        $ajax("/MobileUser/HuDongAlbum", "id=" + $("#id").val() + "&yes=" + yes, function (data) {
            $("#hudong_album").html(data);
            initPhotoSwipeFromDOM('.PhotoSwipe_Album');
        }, "html");
    }

    var removeThisElements = function (obj) {
        if (!confirm("确定删除？")) {
            return;
        }
        var $Item = $(obj).closest(".showimg");
        $Item.remove();

        $("#hideImgURLList").val($("#hideImgURLList").val().replace("[||]" + $Item.attr("data-value"), ""));
    }

    var parent_item_move = true;
    var intihudong = function () {

        $(document).on("touchend", "#btn_tiwen", function () {
            $("#itemid").val("");
            $("#div_03 li,#div_03 dl,#div_03 .mainlayer").removeClass("activeRep");
            //$("#txtdetail").css("width", "72%");
            $("#txtdetail").attr("placeholder", "发表下评论");
            $("#btn_send").html("提问");
            $("#Comment").show();
            $("#btn_add").show();
            initupfile();
            $("#txtdetail").focus();
            $(".activeRep").removeClass("activeRep");
        });

        $(document).on("touchend", "#searchQA", function () {
            HuDongQAKeyWord();
        });

        //$(document).on("touchend", "#wenda_head", function () {
        $("#wenda_head").click(function () {
            if ($("#wenda_content").is(":hidden")) {
                $("#wenda_content").show();
                $("#itemid").val("");
                //$("#txtdetail").css("width","72%");
                $("#txtdetail").attr("placeholder", "写下你的问题");
                $("#btn_add").show();
                $("#type").val("wenda");
                $("#wenda_head").find(".item-arr").removeClass("down-arr").addClass("up-arr");
                $("#pingjia_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#pingjia_new").show();
                $("#xiangce_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#xiangce_new").show();
                $("#wenda_new").hide();

                $("#btn_send").html("提问");
                $("#Comment").show();
                initupfile();
            }
            else {
                $("#wenda_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#wenda_content").hide();
                $("#Comment").hide();
                $("#wenda_new").show();
            }
            $("#pingjia_content").hide();
            $("#xiangce_content").hide();
            $('html, body').animate({
                scrollTop: $("#wenda_head").offset().top
            }, 200);
            $(".activeRep").removeClass("activeRep");
        });

        //$(document).on("touchend", "#pingjia_head", function () {
        $("#pingjia_head").click(function () {
            if ($("#pingjia_content").is(":hidden")) {
                $("#pingjia_content").show();             
                $("#itemid").val("");
                //$("#txtdetail").css("width","72%");
                $("#txtdetail").attr("placeholder", "请选择相应学员进行回复");
                $("#btn_add").hide();
                $("#type").val("pingjia");
                //$("#Comment").show();
                $("#pingjia_head").find(".item-arr").removeClass("down-arr").addClass("up-arr");
                $("#wenda_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#wenda_new").show();
                $("#xiangce_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#xiangce_new").show();
                $("#pingjia_new").hide(); 
            }
            else {
                $("#pingjia_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#pingjia_content").hide();
                $("#Comment").hide();
                $("#pingjia_new").show();
            }
            $("#xiangce_content").hide();
            $("#wenda_content").hide();
            $('html, body').animate({
                scrollTop: $("#pingjia_head").offset().top
            }, 200);
            $(".activeRep").removeClass("activeRep");
        });

        //$(document).on("touchend", "#xiangce_head", function () {
        $("#xiangce_head").click(function () {
            if ($("#xiangce_content").is(":hidden")) {
                $("#xiangce_content").show();
                $("#itemid").val("");
                //$("#txtdetail").css("width","72%");
                $("#txtdetail").attr("placeholder", "写下你的图片描述");
                $("#btn_add").show();
                $("#type").val("xiangce");
                $("#btn_send").html("发送");
                $("#Comment").show();
                initupfile();
                $("#xiangce_head").find(".item-arr").removeClass("down-arr").addClass("up-arr");
                $("#wenda_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#wenda_new").show();
                $("#pingjia_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#pingjia_new").show();

                $("#xiangce_new").hide();
            }
            else {
                $("#xiangce_head").find(".item-arr").removeClass("up-arr").addClass("down-arr");
                $("#xiangce_content").hide();
                $("#Comment").hide();
                $("#xiangce_new").show();
            }
            $("#wenda_content").hide();
            $("#pingjia_content").hide();
            $('html, body').animate({
                scrollTop: $("#xiangce_head").offset().top
            }, 200);
            $(".activeRep").removeClass("activeRep");
        });
 
        $(document).on("touchend", ".show_all", function () {
            if ($(this).html() == "查看更多")
            {
                $(this).parent("ul:first").find("li").each(function () {
                    $(this).show();
                });
                $(this).html("收起");
            } else
            {
                var index = 0;
                $(this).parent("ul:first").find("li").each(function () {
                    if(index<3)
                        $(this).show();
                    else
                        $(this).hide();
                    index++;
                });
                $(this).html("查看更多");
            }
            
        });
        
        $(document).on("touchmove", ".parent-item", function () {
            parent_item_move = false;
            $(document).on("touchend", ".parent-item", function () {
                parent_item_move = true;
            });
        });

        $(document).on("touchend", ".parent-item", function (e) {
            if (parent_item_move) {
                var $this = $(this), $li = $this.closest("li"), $dl = $this.closest("dl")
                , $main = $this.closest(".mainlayer"), name = $li.attr("data-name");
                if ($dl.hasClass("activeRep")) {
                    return;
                } else if ($main.hasClass("activeRep")) {
                    return;
                }//else if ($li.hasClass("activeRep") && $li.hasClass("baseinfo")) {
                //    return;
                //}
                
                //$(".wenda li").removeClass("activeRep");
                $("#div_03 li,#div_03 dl,#div_03 .mainlayer").removeClass("activeRep");
                if ($dl.hasClass("answerlist")) {
                    $dl.addClass("activeRep");
                    name = $dl.attr("data-atname");
                    $("#totype").val($dl.attr("data-attype"));
                    $("#touserid").val($dl.attr("data-atid"));
                } else {
                    //$li.addClass("activeRep");
                    $main.addClass("activeRep");
                    $("#totype").val(-1);
                    $("#touserid").val("");
                   // $("#txtdetail").focus();
                }
                $("#itemid").val($li.attr("data-id"));
                $("#txtdetail").attr("placeholder", '@' + name);
                if ($li.hasClass("wenda")) {
                    $("#btn_send").html("回复");
                }
                $("#Comment").show();
                initupfile();
            }
        });
        //$(document).on("touchend", ".pingjia", function () {
        //    $("#itemid").val($(this).data("id"));
        //    $("#txtdetail").attr("placeholder", '@' + $(this).data("name"));
        //});
    }

    var initupfile = function()
    {
        if (uploader == null) {
            uploader = Qiniu.uploader({
                runtimes: 'html5,flash,html4',    //上传模式,依次退化
                browse_button: 'btn_add',       //上传选择的点选按钮，**必需**
                uptoken: $('#upToken').val(),
                domain: QiNiu_Domain,   //bucket 域名，下载资源时用到，**必需**
                get_new_uptoken: false,  //设置上传文件的时候是否每次都重新获取新的token
                max_file_size: '100mb',           //最大文件体积限制
                unique_names: true,
                max_retries: 3,                   //上传失败最大重试次数
                dragdrop: false,                   //开启可拖曳上传
                chunk_size: '4mb',                //分块上传时，每片的体积
                auto_start: true,     //选择文件后自动上传，若关闭需要自己绑定事件触发上传
                multi_selection: true,//true,可选多个，false每次一个图片
                //filters: {  
                //    mime_types : [ //只允许上传图片  
                //        { title : "Image files", extensions : "jpg,jpeg,gif,png" },  
                //    ],  
                //    prevent_duplicates : false //不允许选取重复文件  
                //},
                init: {
                    'FilesAdded': function (up, files) {
                        
                    },
                    'FileUploaded': function (up, file, info) {
                        var domain = up.getOption('domain');
                        var res = eval('(' + info + ')');
                        var sourceLink = domain + res.key; //获取上传成功后的文件的Url
                        if (file.type.indexOf("image") >= 0) {
                            //获取图片宽高 
                            var img = new o.Image();
                            img.onload = function () {
                                var width = img.width, height = img.height;
                                //console.log("width:" + width);
                                //console.log("height:" + height);
                                $("#hideImgURLList").val($("#hideImgURLList").val() + "[||]" + sourceLink + "?" + width + "X" + height);

                                var html = "";
                                html += "<div class=\"showimg\" data-value='" + sourceLink + "' >";
                                html += "<div class=\"deletebtn\" onclick='removeThisElements(this);'><img style=\"width:65%;height:auto;\" src=\"/assets/img/10.png\" /></div>";
                                html += "<div style=\"width:60px;height:60px;overflow:hidden;border:1px solid #ccc\"><img style=\"width:100%;height:auto;\" src=\"" + sourceLink + "\" /></div>";
                                html += "</div>";

                                $('#markWrapPic').append(html);
                                //$('#markWrap').hide();
                            }
                            img.load(sourceLink);

                            

                            hideloading();

                        } else {
                            showMsg("请上传图片");
                            hideloading();
                        }
                    },
                    'Error': function (up, err, errTip) {
                        alert(errTip);
                        hideloading();
                    },
                    'BeforeUpload': function (up, file) {
                        // 每个文件上传前,处理相关的事情
                        if (file.type.indexOf("image") < 0) {
                            showMsg("请上传图片");
                            uploader.stop();
                            return;
                        }
                        else {
                            showloading("");
                        }
                    },
                }
            });
        }
    }

    var creatNewAblum = function (content, img) {
        var img = "" || img;
        var html = '';
        html += '<div class="ck_select clearfix"><span class="new_ans_name">' + $("#username").val() + '</span><span>' + content + '</span></div>';
        if (img != "") {
            html += '<dl data-name="Photolist" class="photolist_album clearfix PhotoSwipe_Album">';
            var imgArr = img.split("[||]");
            for (var i = 0; i < imgArr.length; i++) {
                var item = imgArr[i];
                if (item != "") {
                    var WH = (item.split('?').Length >= 2 ? item.split('?')[1] : "400X600");
                    html += '<dd style="width: 6rem; "><img data-size="' + WH + '" src="' + item + '" style="width: 6rem;height: 6rem;" /></dd>';
                }
            }
            html += '</dl>';
        }
        return html;
    }

    var handle = function () {

        $('#classdetial').css("min-height",$('body')[0].clientHeight);

        //$("#Comment textarea").on("focus", function () {
        //    $("#Comment").css("padding-bottom", $("#Comment").height());
        //    //$("#Comment").css("bottom", $("#Comment").height());
        //    setTimeout(function () { $(".to-top-action").hide(); }, 300);
        //});

        //$(".headertop,.indextab,.tab-content").on("touchstart", function () {
        //    $("#Comment textarea").blur();
        //    $("#Comment").css("padding-bottom", 0);
        //    $("#Comment").css("bottom", 0);
        //});

        $(document).on("click", "[data-toggle='tab']", function () {
            var $this = $(this), classname = $this.attr("class"), htmlVal = $this.find("h5:first").html(), hrefid = $this.attr("data-href");
            if (classname == "active") {
                return;
            } else {
                $("#Comment").hide();
                $("[data-toggle='tab']").closest("li").removeClass("active");
                $this.closest("li").addClass("active");
                $(".tab-content .tab-pane").removeClass("active");
                $("" + hrefid).addClass("active");
                if (htmlVal == "班级排名") {
                    MarkOrder();
                } else if (htmlVal == "互动社区"){
                    HuDongQA();
                    //HuDongComment();
                    HuDongAlbum("");
                    $(".item-arr").removeClass("up-arr").addClass("down-arr");
                    $("#wenda_new").show();
                    $("#pingjia_new").show();
                    $("#xiangce_new").show();
                }
                else if (htmlVal == "学习旅程") {
                    
                    HuDongComment();
                   
                } 
                
            }
        });
        if ($("#svg_data").length > 0)
        {
            var svg_data = $("#svg_data").val();
            draw(JSON.parse(svg_data));
        }

        $(".indextab [data-toggle='tab']").eq(1).click();

        var type = $("#type").val();
        if (type != 0) {
            $(".indextab [data-toggle='tab']").eq(type).click();
        }

        //$("#btn_add").on("click", function () {

        //    if ($("#zhaopian").is(":hidden")) {
        //        $("#markWrapPic").show();
        //        $("#zhaopian").show();
        //        //console.log(uploader);
               
        //    } else {
        //        $("#zhaopian").hide();
        //        $("#markWrapPic").hide();
        //    }
        //});
        $("#btn_send").click(function () {

            if ($("#txtdetail").val() == "") {
                showMsg("评价内容不能为空");
                return;
            }
            else if ($("#txtdetail").val().length > 201) {
                showMsg("评价内容必须200字以内");
                return;
            }            
            
            else {
                if ($("#type").val() == "xiangce") {
                    if ($("#markWrapPic img").length == 0) {
                        showMsg("请上传至少一张图片");
                        return;
                    }
                }
                showloading("提交中，请稍侯...");
                var paras = {
                    classid: $("#classid").val(),
                    txtdetail: $("#txtdetail").val(),
                    itemid: $("#itemid").val(),
                    type: $("#type").val(),
                    srclist: $("#hideImgURLList").val(),
                    touserid: $("#touserid").val(),
                    totype: $("#totype").val()
                };
                $.ajax({
                    type: "POST",
                    data:JSON.stringify(paras),
                    url: "/MobileUser/PostSaveEvaluate",
                    dataType: "json",
                    contentType: 'application/json'
                }).success(function (res) {
                    if (res.success) {
                        if ($("#type").val() == "wenda") {
                            HuDongQAKeyWord(true);
                            //处理一下最新
                            var content = $("#txtdetail").val();
                            $("#wenda_new").html("最新问答：" + content);
                        } else if ($("#type").val() == "xiangce") {
                            HuDongAlbum("yes");
                            //处理一下最新
                            var content = $("#txtdetail").val();
                            var img = $("#hideImgURLList").val();
                            var html = creatNewAblum(content, img);
                            $("#xiangce_new").html(html);
                        }
                        else {
                            HuDongCommentList();
                        }
                        $("#Comment").hide();
                        $("#hideImgURLList").val("");
                        $("#markWrapPic").html("");
                        $("#zhaopian").hide();
                        $("#txtdetail").val("");
                        $("#itemid").val("");
                        $("#txtdetail").attr("placeholder", "");
                    } else {
                        showMsg(res.message);
                    }
                    hideloading();
                }).error(function (xhr, status) {
                    hideloading();
                });

            }


        });
         
        intihudong();
       
     
    };
    return {
        init: function () {
            handle();
            window.draw = draw;
            window.removeThisElements = removeThisElements;
            window.initPhotoSwipeFromDOM = initPhotoSwipeFromDOM;
        }
    };
}();