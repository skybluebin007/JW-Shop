function checkUserName(minLength, maxLength) {
    var userNameObj = o("UserName");
    var checkUserNameObj = o("CheckUserName");
    if (userNameObj.value != "") {
        var length = getStringLength(userNameObj.value);
        var reg = /^([a-zA-Z0-9_\u4E00-\u9FA5])+$/;
        if (length < minLength || length > maxLength) {
            checkUserNameObj.value = "0";
            return false;
        }
        else if (!reg.test(userNameObj.value)) {
            checkUserNameObj.value = "0";
            return false;
        }
        else {
        }
        var url = "/Ajax.aspx?UserName=" + encodeURIComponent(userNameObj.value) + "&Action=CheckUserName";
        Ajax.requestURL(url, dealCheckUserName);
    }
    else {
        checkUserNameObj.value = "0";
    }
}
function dealCheckUserName(data) {
    //1-成功，2-注册过,3-有非法字符
    var checkUserNameObj = o("CheckUserName");
    switch (data) {
        case "1":
            checkUserNameObj.value = "1";
            break;
        case "2":
            checkUserNameObj.value = "0";
            break;
        case "3":
            checkUserNameObj.value = "0";
            break;
        default:
            break;
    }
}
function checkEmail() {
    var emailObj = o("Email");
    var checkEmailObj = o("CheckEmail");
    if (emailObj.value != "") {
        if (!Validate.isEmail(emailObj.value)) {
            checkEmailObj.value = "0";
            return;
        }
        else {
            checkEmailObj.value = "";
        }
    }
    else {
        checkEmailObj.value = "0";
    }
}
function checkUserPass(minLength, maxLength) {
    var userPassObj = o("UserPassword1");
    var checkUserPasswordObj1 = o("CheckUserPassword1");
    if (userPassObj.value != "") {
        var length = getStringLength(userPassObj.value);
        if (length < minLength || length > maxLength) {
            checkUserPasswordObj1.value = "0";
        }
        else {
            checkUserPasswordObj1.value = "1";
        }
    }
    else {
        checkUserPasswordObj1.value = "0";
    }
}
function checkUserPass2(minLength, maxLength) {
    var userPassObj = o("UserPassword2");
    var checkUserPasswordObj2 = o("CheckUserPassword2");
    if (userPassObj.value != "") {
        var length = getStringLength(userPassObj.value);
        if (length < minLength || length > maxLength) {
            checkUserPasswordObj2.value = "0";
        }
        else {
            if (userPassObj.value != o("UserPassword1").value) {
                checkUserPasswordObj2.value = "0";
            }
            else {
                checkUserPasswordObj2.value = "1";
            }
        }
    }
    else {
        checkUserPasswordObj2.value = "0";
    }
}


function checkRegister() {
    if (o("CheckUserName").value == "0") {
        alertMessage("用户名已存在或者用户名长度不正确");
        return false;
    }
    if (o("CheckUserPassword1").value == "0") {
        alertMessage("密码长度不正确");
        return false;
    }
    if (o("CheckUserPassword2").value == "0") {
        alertMessage("密码长度不正确或者两次密码不一致");
        return false;
    }
    if (o("SafeCode").value == "") {
        alertMessage("验证码不能为空");
        return false;
    }
    if (o("AgreeProtocol") != null) {
        if (!o("AgreeProtocol").checked) {
            alertMessage("请选择遵守条款");
            return false;
        }
    }

    this.form.submit();
}
