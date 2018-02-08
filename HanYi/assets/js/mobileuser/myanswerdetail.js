var pagejs = function () {
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
                gallery,
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

    var save = function () {
        var id = $("#itemid").val(), content = $("#txtdetail").val(), classid = $("#classid").val();
        if (content == "") {
            showMsg("回复内容不能为空");
            return;
        }
        showloading("");
        var paras = {
            itemid: id,
            txtdetail: content,
            classid: classid,
            type: "wenda",
            srclist: $("#hideImgURLList").val(),
            touserid: $("#touserid").val(),
            totype: $("#totype").val()
        };
        $.ajax({
            type: "POST",
            data: JSON.stringify(paras),
            url: "/MobileUser/PostSaveEvaluate",
            dataType: "json",
            contentType: 'application/json'
        }).success(function (res) {
            if (res.success) {
                window.location.href = window.location.href;
            } else {
                showMsg(res.message);
            }
            hideloading();
        }).error(function (xhr, status) {
            hideloading();
        });
    }
    var uploader = null;

    var removeThisElements = function (obj) {
        if (!confirm("确定删除？")) {
            return;
        }
        var $Item = $(obj).closest(".showimg");
        $Item.remove();

        $("#hideImgURLList").val($("#hideImgURLList").val().replace("[||]" + $Item.attr("data-value"), ""));
    }

    var initPhoto = function () {
        uploader = Qiniu.uploader({
            runtimes: 'html5,flash,html4',    //上传模式,依次退化
            browse_button: 'addbtn',       //上传选择的点选按钮，**必需**
            uptoken: $('#upToken').val(),
            domain: QiNiu_Domain,   //bucket 域名，下载资源时用到，**必需**
            get_new_uptoken: false,  //设置上传文件的时候是否每次都重新获取新的token
            max_file_size: '10mb',           //最大文件体积限制
            unique_names: true,
            max_retries: 3,                   //上传失败最大重试次数
            dragdrop: false,                   //开启可拖曳上传
            chunk_size: '4mb',                //分块上传时，每片的体积
            auto_start: true,     //选择文件后自动上传，若关闭需要自己绑定事件触发上传
            multi_selection: true,//true,可选多个，false每次一个图片
            //filters: {
            //    mime_types: [ //只允许上传图片  
            //        { title: "Image files", extensions: "jpg,jpeg,gif,png" },
            //    ],
            //    prevent_duplicates: false //不允许选取重复文件  
            //},
            init: {
                'FileUploaded': function (up, file, info) {
                    var domain = up.getOption('domain');
                    var res = eval('(' + info + ')');
                    var sourceLink = domain + res.key; //获取上传成功后的文件的Url
                    if (file.type.indexOf("image") >= 0) {
                        if ($('#markWrapPic .showimg').length < 7) {
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
                            }
                            img.load(sourceLink);
                        }
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
                        if ($('#markWrapPic .showimg').length > 7) {
                            showMsg("上传图片数量不能超过6张");
                            uploader.stop();
                            return;
                        } else {
                            showloading("");
                        }
                    }
                },
            }
        });
    }

    var parent_item_move = true;
    var handle = function () {
        //$(".item").click(function () {
        //    var $this = $(this);
        //    if ($this.hasClass("activeRep")) {
        //        return;
        //    }
        //    $(".item").remove("activeRep");
        //    $this.addClass("activeRep");
        //    var name = $this.attr("data-name"), classid = $this.attr("data-classid"), id = $this.attr("data-id");
        //    $("#txtdetail").val("").attr("placeholder", '@' + name);
        //    $("#classid").val(classid);
        //    $("#itemid").val(id);
        //    $("#Comment").show();
        //    if (uploader == null) {
        //        initPhoto();
        //    }
        //});


        $(document).on("touchmove", ".parent-item", function () {
            parent_item_move = false;
            $(document).on("touchend", ".parent-item", function () {
                parent_item_move = true;
            });
        });

        $(document).on("touchend", ".parent-item", function (e) {
            if (parent_item_move) {
                var $this = $(this), $li = $(".item"), $dl = $this.closest("dl")
                , name = $li.attr("data-name");
                if ($dl.hasClass("activeRep")) {
                    return;
                } else if ($li.hasClass("activeRep") && $li.hasClass("baseinfo")) {
                    return;
                }
                $("a,.sublayer,.parent-item,.answerlist").removeClass("activeRep");
                if ($dl.hasClass("answerlist")) {
                    $dl.addClass("activeRep");
                    name = $dl.attr("data-atname");
                    $("#totype").val($dl.attr("data-attype"));
                    $("#touserid").val($dl.attr("data-atid"));
                } else {
                    $li.addClass("activeRep");
                    //$(".sublayer").addClass("activeRep");
                    $("#totype").val(-1);
                    $("#touserid").val("");
                }
                $("#itemid").val($li.attr("data-id"));
                $("#txtdetail").attr("placeholder", '@' + name);
            }
        });

        initPhoto();

        $("#btn_send").click(function () {
            save();
        });

        initPhotoSwipeFromDOM('.PhotoSwipe');
    };
    return {
        init: function () {
            handle();
            window.removeThisElements = removeThisElements;
            window.initPhotoSwipeFromDOM = initPhotoSwipeFromDOM;
        }
    };
}();