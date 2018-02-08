var pagejs = function () {
    var Login = function () {

        var phone = $("#phone").val();
        if (phone == "") {
            showMsg("手机号码不能为空");
            return;
        }

        var pwd = $("#password").val();
        if (pwd == "") {
            showMsg("密码不能为空");
            return;
        }
        showloading("");
        var paras = {
            phone: phone,
            password: pwd
        };
        $.ajax({
            type: "POST",
            data: JSON.stringify(paras),
            url: "/Account/PostLoginUser",
            dataType: "json",
            contentType: 'application/json'
        }).success(function (res) {
            if (res.success) {
                location.href = res.url;
            } else {
                $("#errorMsg").show();
            }
            hideloading();
        }).error(function (xhr, status) {
            hideloading();
            //showMsg(status);
        });
    }

    var handle = function () {


        $(".login_btn").click(function () {
            Login();
        });
        document.getElementById("password").addEventListener("keypress", function (evt) {
            if (evt.keyCode == 13) {
                $(".login_btn").click();
            }
        }, false);
    };
    return {
        init: function () {

            $("#errorMsg").hide();
            handle();
            window.Login = Login;
        }
    };
}();