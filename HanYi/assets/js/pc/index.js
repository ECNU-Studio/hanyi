var pagejs = function () {

    var pageSize = 40;
    var curPageIndex = 1;

    function loadMore() {
        $(".load_more").html('<img src="/assets/img/simpleloading.gif" style="width: 16px; height: 16px;" />');
        getTaskList(curPageIndex + 1);
    }

    function loadMoreEnd() {
        var isEnd = (curPageIndex * pageSize - parseInt($("#total").val())) >= 0 ? 1 : 0;

        if (isEnd == 1) {
            $(".load_more").html('<span>共 ' + $("#total").val() + ' 门课程</span>');
        } else {
            $(".load_more").html('<a href="javascript:void(0)" onclick="loadMore()">加载更多</a>');
        }
        $(".load_more").show();
    }

    var getTaskList = function (pageIndex) {
        showloading("");
        curPageIndex = pageIndex;
        var rqData = "";
        rqData += "&pageIndex=" + pageIndex;
        rqData += "&pageSize=" + pageSize;
        $.ajax({
            type: "GET",
            data: rqData,
            url: "/SSKKPC/CourseList",
            contentType: "application/html;chartset=utf-8;",
            datatype: "html"
        }).success(function (data) {
            if (pageIndex == 1) {
                $("#res").html(data);
            }
            else {
                $("#res").append(data);
            }
            loadMoreEnd();

            if ($("#total").val() > 0 && !$("#res").hasClass("indexlist")) {
                $("#res").addClass("indexlist")
            }

            hideloading();
        }).error(function (xhr, status) {
            hideloading();
        });
    }





    var intervalTag = null;

    var handle = function () {

        getTaskList(1);
    };
    return {
        init: function () {
            handle();

            window.getTaskList = getTaskList;
            window.loadMore = loadMore;
        }
    };
}();