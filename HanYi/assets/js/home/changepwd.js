var pagejs = function () {
 
    var PWChange = function (obj, title, id) {
        var res = $validate();
        if (!res.isOK) {
            return;
        }
        if ($("#newpass").val() != $("#repass").val())
        {
            showJSGL(this, "新密码不一致");
            return;
        }
        showloading("操作中。。。");
        $ajax("/Admin/PWChange", JSON.stringify({ "model": JSON.stringify(res) }), function (res) {
            hideloading();
            if (res.success) {
                $("#open_panel").hide();
                showok("修改成功!", function () { Search(oldindex, 0); });
            } else {
                showerror(res.message);
            }
        });
    }

  
   
    
    var showJSGL = function (obj, title) {
        showerror(title);
    }

   
    var handle = function () {
        
        $("#savebtn").click(function () {
            PWChange();
        });
       
    }

    return {
        init: function () {
            handle();            
                       
        }
    };
}();


