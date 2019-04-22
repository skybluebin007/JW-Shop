var bool = true;

function checkUserName() {
    var userNameObj = o("UserName");
    var messageObj = o("UserNameWarningMessage");
    var checkUserNameObj = o("CheckUserName");
    if (userNameObj.value != "") {
        messageObj.innerHTML = "";
        var url = "/Ajax.aspx?UserName=" + encodeURIComponent(userNameObj.value) + "&Action=CheckUserName";
        Ajax.requestURL(url, dealCheckUserName);
    }    
}
function dealCheckUserName(data) {
    //1-不存在该用户名，2-用户名填写正确,3-有非法字符
    var messageObj = o("UserNameWarningMessage");
    var checkUserNameObj = o("CheckUserName");
    if(data=="1"){
        messageObj.innerHTML = " * 不存在该用户名！";
        checkUserNameObj.value = "0";
        $("#UserNameWarningMessage").parents(".row").addClass("err");
        bool = false;
        return;
    }
    else{
        checkUserNameObj.value = "1";
        $("#UserNameWarningMessage").parents(".row").removeClass("err");
        bool = true;
    }
}
function checkEmail() {
    if ($("input[name='checkType']:checked").val() == "1") {//邮箱验证
        var emailObj = o("Email");
        var warningObj = o("EmailWarningMessage");
        var checkEmailObj = o("CheckEmail");
        if (emailObj.value != "") {
            if (!Validate.isEmail(emailObj.value)) {
                warningObj.innerHTML = " * 请输写正确的Email地址！";
                checkEmailObj.value = "0";
                $("#EmailWarningMessage").parents(".row").addClass("err");
                console.log("ckemail0");
                bool = false;
                return;
            }
            warningObj.innerHTML = "";
            var url = "/Ajax.aspx?Email=" + encodeURIComponent(emailObj.value) + "&Action=CheckEmail";
            Ajax.requestURL(url, dealCheckEmail);
        }
    }
}
function dealCheckEmail(data) {
    var warningObj = o("EmailWarningMessage");
    var checkEmailObj = o("CheckEmail");
    if(data=="2"){
        warningObj.innerHTML = " * 不存在该Email！";
        checkEmailObj.value = "0";
        $("#EmailWarningMessage").parents(".row").addClass("err");
        console.log("ckemail00");
        bool = false;
        return;
    }
    else{
        checkEmailObj.value = "1";
        $("#EmailWarningMessage").parents(".row").removeClass("err");
        bool = true;
    }  
}

function checkMobile() {
    if ($("input[name='checkType']:checked").val() == "0") {//手机验证

        var mobileObj = o("Mobile");
        var warningObj = o("MobileWarningMessage");
        var checkMobileObj = o("CheckMobile");
        if (mobileObj.value != "") {
            if (!Validate.isMobile(mobileObj.value)) {
                warningObj.innerHTML = " * 请输写正确的手机号！";
                checkMobileObj.value = "0";
                $("#MobileWarningMessage").parents(".row").addClass("err");
                bool = false;
                return;
            }
            warningObj.innerHTML = "";
            //var url = "/Ajax.aspx?Email=" + emailObj.value + "&Action=CheckEmail";
            //Ajax.requestURL(url, dealCheckEmail);
            $.ajax({
                url: '/Ajax.aspx?Action=CheckLoginNameAndMobile&Mobile=' + encodeURIComponent($("#Mobile").val()) + "&UserName=" + encodeURIComponent($("#UserName").val()),
                type: 'GET',
                success: function (result) {
                    if (result == "1") {
                        $("#MobileWarningMessage").html(" * 手机号错误！");
                        $("#CheckMobile").val("0");
                        $("#MobileWarningMessage").parents(".row").addClass("err");
                        console.log("mobileck");
                        bool = false;
                        return;
                    }
                    else {
                        $("#CheckMobile").val("1");
                        $("#MobileWarningMessage").parents(".row").removeClass("err");
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
        //去掉错误提示
        $("#Mobile").parents(".row").removeClass("err");
        $("#phoneVer").parents(".row").removeClass("err");
        $("#Email").parents(".row").removeClass("err");
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
  
   
    $("#getPw").find(".submit").click(function () {
        //var user = $("#UserName");
        var email = $("#Email"), mobile = $("#Mobile"), code = $("#SafeCode"), mobileCode = $("#phoneVer");
        if (!$("#getPw").length) { return false; }      
        var checkValue = $("input[name='checkType']:checked").val();
        //if (user.val() == "") {
        //    user.parents(".row").addClass("err");
        //    bool = false;
        //}
        if (checkValue == "0") {//手机验证
            if (mobile.val() == "" || !/^1[3-9]\d{9}$/.test(mobile.val())) {
                mobile.parents(".row").addClass("err");
                //console.log("mobile");
                bool = false;
            }
            if (mobileCode.val() == "") {               
                mobileCode.parents(".row").addClass("err");
                //console.log("mobileCode");
                bool = false;
            }
        }
        else {//邮箱验证
            if (email.val() == "" || !/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email.val())) {
                email.parents(".row").addClass("err");
                //console.log("ckemail1");
                bool = false;
            }
        }
        if (code.val() == "") {
            code.parents(".row").addClass("err");
            bool = false;
        }
        else {
            code.parents(".row").removeClass("err");
            bool = true;
        }
        if (bool) {
            //验证通过
            $.ajax({
                url: '/User/FindPassword.html?Action=Post',
                type: 'POST',
                data: $("form").serialize(),
                success: function (result) {
                    var arr = result.split('|');
                    if (arr[0] == "ok") {
                        alertMessage("修改成功", 500);
                        window.location.href = arr[1];
                       
                    }
                    else {
                        alertMessage(arr[1], 500);
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