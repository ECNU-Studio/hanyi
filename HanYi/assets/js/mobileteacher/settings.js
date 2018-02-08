var pagejs = function () {
    var isclick = false;
    var change = function (obj) {
        if (isclick) { return; } isclick = true;
        var $this = $(obj), type = $this.attr("data-type"), val = $this.prop("checked");
        var paras = {
            type: type,
            val: val
        };
        showloading("");
        $.ajax({
            type: "POST",
            data: JSON.stringify(paras),
            url: "/MobileTeacher/SettingPost",
            dataType: "json",
            contentType: 'application/json'
        }).success(function (res) {
            if (res.success) {
                // window.location.href = window.location.href;
            } else {
                alert(res.message);
            }
            isclick = false;
            hideloading();
        }).error(function (xhr, status) {
            hideloading();
        });
    }

    var handle = function () {

        $(".mui-switch").click(function () {
            var type = $(this).attr("data-type");
            if (type == 3) {
                $(this).prop("checked", "checked");
                window.location.href = "http://en2.nengli8.com/MobileTeacher/Settings";
            } else {
                change(this);
            }
        });
    };
    return {
        init: function () {
            handle();

        }
    };
}();