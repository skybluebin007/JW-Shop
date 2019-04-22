function checkUserName(minLength, maxLength) {
    var userNameObj = $("#UserName");
    var checkUserNameObj = $("#CheckUserName");
    if (userNameObj.val() != "") {
        var length = getStringLength(userNameObj.val());
        var reg = /^([a-zA-Z0-9_\u4E00-\u9FA5])+$/;
        if (length < minLength || length > maxLength) {
            checkUserNameObj.val("0");
            userNameObj.addClass("focus");
            return false;
        }
        else if (!reg.test(userNameObj.val())) {
            checkUserNameObj.val("0");
            userNameObj.addClass("focus");
            return false;
        }
        else {
            checkUserNameObj.val("1");
            userNameObj.removeClass("focus");
        }
        var url = "/Ajax.aspx?UserName=" + encodeURIComponent(userNameObj.val()) + "&Action=CheckUserName";
        Ajax.requestURL(url, dealCheckUserName);
    }
    else {
        checkUserNameObj.val("0");
        userNameObj.addClass("focus");
    }
}
function dealCheckUserName(data) {
    //1-成功，2-注册过,3-有非法字符
    var checkUserNameObj = $("#CheckUserName");
    switch (data) {
        case "1":
            checkUserNameObj.val("1");
            $("#UserName").removeClass("focus");
            break;
        case "2":
            checkUserNameObj.val("0");
            $("#UserName").addClass("focus");
            break;
        case "3":
            checkUserNameObj.val("0");
            $("#UserName").addClass("focus");
            break;
        default:
            break;
    }
}
function checkPhone() {
    var phoneObj = o("Phone");
    var checkPhone = o("CheckPhone");
    if (phoneObj.value != "") {
        if (!Validate.isMobile(phoneObj.value)) {
            checkPhone.value = "0";
            phoneObj.focus();
            return;
        }
        else {
            //checkPhone.value = "1";
            $.ajax({
                type: 'post',
                url: "/Ajax.html?Action=CheckMobile&Mobile=" + encodeURIComponent(phoneObj.value) + "&checkUserId=0",
                data: {},
                cache: false,
                //dataType: 'json',
                success: function (data) {
                    if (data == "2") {
                        checkPhone.value = "1";       
                    }
                    else {
                        checkPhone.value = "2";      
                    }
                },
                error: function () { }
            });
        }
    }
    else {
        checkPhone.value = "0";
        phoneObj.focus();
    }
}

function checkEmail() {
    var emailObj = $("#Email");
    var checkEmailObj = $("#CheckEmail");
    if (emailObj.val() != "") {
        if (!Validate.isEmail(emailObj.val())) {
            checkEmailObj.val("0");
            emailObj.addClass("focus");
            return;
        }
        else {
            checkEmailObj.val("1");
            emailObj.removeClass("focus");
        }
    }
    else {
        checkEmailObj.val("0");
        emailObj.addClass("focus");
    }
}
function checkUserPass(minLength, maxLength) {
    var userPassObj = $("#UserPassword1");
    var checkUserPasswordObj1 = $("#CheckUserPassword1");
    if (userPassObj.val() != "") {
        var length = getStringLength(userPassObj.val());
        if (length < minLength || length > maxLength) {
            checkUserPasswordObj1.val("0");
            userPassObj.addClass("focus");
        }
        else {
            checkUserPasswordObj1.val("1");
            userPassObj.removeClass("focus");
        }
    }
    else {
        checkUserPasswordObj1.val("0");
        userPassObj.addClass("focus");
    }
}
function checkUserPass2(minLength, maxLength) {
    var userPassObj = $("#UserPassword2");
    var checkUserPasswordObj2 = $("#CheckUserPassword2");
    if (userPassObj.val() != "") {
        var length = getStringLength(userPassObj.val());
        if (length < minLength || length > maxLength) {
            checkUserPasswordObj2.val("0");
            userPassObj.addClass("focus");
        }
        else {
            if (userPassObj.val() != $("#UserPassword1").val()) {
                checkUserPasswordObj2.val("0");
                userPassObj.addClass("focus");
            }
            else {
                checkUserPasswordObj2.val("1");
                userPassObj.removeClass("focus");
            }
        }
    }
    else {
        checkUserPasswordObj2.val("0");
        userPassObj.addClass("focus");
    }
}


function checkRegister() {
    if ($("#CheckUserName").val() == "0") {
        app.jMsg("用户名已存在或者用户名长度不正确");
        $("#UserName").focus();
        return false;
    }
    if ($("#CheckUserPassword1").val() == "0") {
        app.jMsg("密码长度不正确");
        $("#UserPassword1").focus();
        return false;
    }
    if ($("#CheckUserPassword2").val() == "0") {
        app.jMsg("密码长度不正确或者两次密码不一致");
        $("#UserPassword2").focus();
        return false;
    }
    if ($("#registerType").val() == "1") {
        //手机短信验证
        if ($("#CheckPhone").val() == "0") {
            app.jMsg("手机号码未输入或输入错误");
            $("#Phone").focus();
            return false;
        }
        if (o("CheckPhone").value == "2") {
            app.jMsg("手机号码已被注册");           
            $("#Phone").focus();           
            return false;
        }
        if ($("#PhoneCode").val() == "") {
            app.jMsg("验证码不能为空");
            $("#PhoneCode").focus();
            return false;
        }
    }
    else {//邮件验证码验证
        if ($("#CheckEmail").val() == "0") {
            app.jMsg("邮箱不正确");
            return false;
        }
        if ($("#SafeCode").val() == "") {
            app.jMsg("验证码不能为空");
            $("#SafeCode").focus();
            return false;
        }
    }
    this.form.submit();
}
