var bool = true;

function checkUserName() {
    var userNameObj = o("UserName");   
    var checkUserNameObj = o("CheckUserName");
    if (userNameObj.value != "") {  
        var url = "/Ajax.aspx?UserName=" + encodeURIComponent(userNameObj.value) + "&Action=CheckUserName";
        Ajax.requestURL(url, dealCheckUserName);
    }    
}
function dealCheckUserName(data) {
    //1-不存在该用户名，2-用户名填写正确,3-有非法字符  
    var checkUserNameObj = o("CheckUserName");
    if(data=="1"){
        app.jMsg("不存在该用户名");
        checkUserNameObj.value = "0";   
        bool = false;
        return;
    }
    else{
        checkUserNameObj.value = "1";     
        bool = true;
    }
}
function checkEmail() {
    if ($("input[name='checkType']:checked").val() == "1") {//邮箱验证
        var emailObj = o("Email");     
        var checkEmailObj = o("CheckEmail");
        if (emailObj.value != "") {
            if (!Validate.isEmail(emailObj.value)) {
                app.jMsg("请输写正确的Email地址");
                checkEmailObj.value = "0";             
                bool = false;
                return;
            }      
            var url = "/Ajax.aspx?Email=" + emailObj.value + "&Action=CheckEmail";
            Ajax.requestURL(url, dealCheckEmail);
        }
    }
}
function dealCheckEmail(data) {  
    var checkEmailObj = o("CheckEmail");
    if(data=="2"){
        app.jMsg("不存在该Email");
        checkEmailObj.value = "0";    
        bool = false;
        return;
    }
    else{
        checkEmailObj.value = "1";     
        bool = true;
    }  
}

function checkMobile() {
    if ($("input[name='checkType']:checked").val() == "0") {//手机验证

        var mobileObj = o("Mobile");    
        var checkMobileObj = o("CheckMobile");
        if (mobileObj.value != "") {
            if (!Validate.isMobile(mobileObj.value)) {
                app.jMsg("请输写正确的手机号");
                checkMobileObj.value = "0";            
                bool = false;
                return;
            }      
            $.ajax({
                url: '/Ajax.aspx?Action=CheckLoginNameAndMobile&Mobile=' + encodeURIComponent($("#Mobile").val()) + "&UserName=" + encodeURIComponent($("#UserName").val()),
                type: 'GET',
                success: function (result) {
                    if (result == "1") {
                        app.jMsg("账户名与手机号不匹配");
                        $("#CheckMobile").val("0");                  
                        bool = false;
                        return;
                    }
                    else {
                        $("#CheckMobile").val("1");                    
                        bool = true;
                    }
                }
            });
        }

    }
}

$(function () {
    //切换验证方式
    $('input:radio[name="checkType"]').change(function () {
     
        var checkValue = $("input[name='checkType']:checked").val();
        if (checkValue == "0") {
            $("#liMobile").show();
            $("#liMobileCode").show();
            $("#liEmail").hide();
        }
        else {
            $("#liMobile").hide();
            $("#liMobileCode").hide();
            $("#liEmail").show();
        }
    })
  
   
    $("#btn_submit").click(function () {
        var user = $("#UserName"), email = $("#Email"), mobile = $("#Mobile"), code = $("#SafeCode"), mobileCode = $("#phoneVer");
        if (!$("#btn_submit").length) { return false; }
        var checkValue = $("input[name='checkType']:checked").val();
        if (user.val() == "") {
            app.jMsg("请输入用户名");
            bool = false;
        }
        if (checkValue == "0") {//手机验证
            if (mobile.val() == "" || !/^1[3-9]\d{9}$/.test(mobile.val())) {
                app.jMsg("请输入正确的手机号");
                bool = false;
            }
            if (mobileCode.val() == "") {               
                app.jMsg("请输入短信校验码");
                bool = false;
            }
        }
        else {//邮箱验证
            if (email.val() == "" || !/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email.val())) {
                app.jMsg("请输入正确的邮箱");
                bool = false;
            }
        }
        if (code.val() == "") {
            app.jMsg("请输入验证码");
            bool = false;
        }
        else {         
            bool = true;
        }
        if (bool) {
            //验证通过
            $.ajax({
                url: '/mobile/User/FindPassword.html?Action=Post',
                type: 'POST',
                data: $("form").serialize(),
                success: function (result) {
                    var arr = result.split('|');
                    if (arr[0] == "ok") {
                        app.jMsg("修改成功", 500);
                        window.location.href = arr[1];
                       
                    }
                    else {
                        app.jMsg(arr[1], 500);
                    }
                }
            });
        }
        else {
            //console.log(bool);
        }
    });
    $("#getPw").find(".txt, .txt2").focus(function () {
        $(this).parents(".row").addClass("focus").addClass("blueLine").removeClass("err");
    }).blur(function () {
        if ($(this).val() == "") {
            $(this).parents(".row").removeClass("focus").removeClass("blueLine");
        } else {
            $(this).parents(".row").removeClass("blueLine");
        }
    });

})