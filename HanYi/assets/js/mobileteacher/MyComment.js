var pagejs = function () {

    var getList = function () {
        showloading("");
        var rqData = "";
        rqData += "content=" + $("#content").val();
        $.ajax({
            type: "GET",
            data: rqData,
            url: "/MobileTeacher/MyCommentList",
            contentType: "application/html;chartset=utf-8;",
            datatype: "html"
        }).success(function (data) {
            $("#res").html(data);
            hideloading();
        }).error(function (xhr, status) {
            hideloading();
        });
    }

    var handle = function () {
        $(".input_sear i").click(function () {
            getList();
        });
        getList();
    };
    return {
        init: function () {
            handle();
            window.getList = getList;
        }
    };
}();