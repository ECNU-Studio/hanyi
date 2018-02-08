var initPhotoSwipeFromDOM = function (gallerySelector) {

    var parseThumbnailElements = function (el) {
        var items = [];

        $(el).children(".pic").each(function (index, obj) {
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

        gallery.init();

    };

    $(gallerySelector).click(function () {
        onThumbnailsClick();
    });

};