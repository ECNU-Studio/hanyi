var pagejs = function () {

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
            type: "pingjia"
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

    var handle = function () {
            //var $this = $("#itemcomment");           
            //var name = $this.attr("data-name"), classid = $this.attr("data-classid"), id = $this.attr("data-id");
            //$("#txtdetail").val("").attr("placeholder", '@' + name);
            //$("#classid").val(classid);
            //$("#itemid").val(id);
            //$("#Comment").show();
         

        $("#btn_send").click(function () {
            save();
        });
    };
    return {
        init: function () {
            handle();
        }
    };
}();