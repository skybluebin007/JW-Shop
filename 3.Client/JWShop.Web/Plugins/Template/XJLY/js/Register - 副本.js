﻿//=========初始
//$(function () {
//    var msg = $("#errorMessageMsg").val();
//    if (msg != "")
//       // alertMessage(msg);
//});

function checkUserName(minLength, maxLength) {
    var userNameObj = o("UserName");
    var messageObj = o("UserNameWarningMessage");
    var checkUserNameObj = o("CheckUserName");
    if (userNameObj.value != "") {
        var length = getStringLength(userNameObj.value);
        var reg = /^([a-zA-Z0-9_\u4E00-\u9FA5])+$/;
        if (length < minLength || length > maxLength) {
            messageObj.innerHTML = " * 长度" + minLength + "- " + maxLength + "个字符！";
            checkUserNameObj.value="0";
            return false;
        }
        else if(!reg.test(userNameObj.value)){
            messageObj.innerHTML = " * 用户名只能包含字母、数字、下划线、中文";
            checkUserNameObj.value="0";
            return false;
        }
        else {
            messageObj.innerHTML = "";
        }
        var url = "/Shop/Ajax.html?UserName=" + encodeURIComponent(userNameObj.value) + "&Action=CheckUserName";
        Ajax.requestURL(url, dealCheckUserName);
    }
    else {
        messageObj.innerHTML = " * 用户名不能为空！";
        checkUserNameObj.value="0";
    } 
}
function dealCheckUserName(data) {
    //1-成功，2-注册过,3-有非法字符
    var messageObj = o("UserNameWarningMessage");
    var checkUserNameObj = o("CheckUserName");
    switch (data) {
        case "1":
            messageObj.innerHTML = "<span style=\"color:#008000\">可以注册</span>";
            checkUserNameObj.value="1";
            break;
        case "2":
            messageObj.innerHTML = " * 用户名已经存在！";
            checkUserNameObj.value="0";
            break;
        case "3":
            messageObj.innerHTML = " * 含有非法字符！";
            checkUserNameObj.value="0";
            break;
        default:
            break;
    }
}
function checkPhone() {
    var phoneObj = o("Phone");
    var warningObj = o("PhoneWarningMessage");
    var checkPhone = o("CheckPhone");
    if (phoneObj.value != "") {
        if (!Validate.isMobile(phoneObj.value)) {
            warningObj.innerHTML = " * 请输写正确的手机号码！";
            checkPhone.value = "0";
            return;
        }
        else {
            warningObj.innerHTML = " ";
            checkPhone.value = "";
        }
    }
    else {
        warningObj.innerHTML = " * 手机号码不能为空！";
        checkPhone.value = "0";
    }
}

function checkEmail() {
    var emailObj = o("Email");
    var warningObj = o("EmailWarningMessage");
    var checkEmailObj = o("CheckEmail");
    if (emailObj.value != "") {
        if (!Validate.isEmail(emailObj.value)) {
            warningObj.innerHTML = " * 请输写正确的Email地址！";
            checkEmailObj.value="0";
            return;
        }
        else{
            warningObj.innerHTML = " ";
            checkEmailObj.value="";
        }
    }
    else{
        warningObj.innerHTML = " * Email地址不能为空！";
        checkEmailObj.value="0";
    }
}
function checkUserPass(minLength, maxLength) {
    var userPassObj = o("UserPassword1");
    var warningObj = o("PasswordWarningMessage1");
    var checkUserPasswordObj1 = o("CheckUserPassword1");
    if (userPassObj.value != "") {
        var length = getStringLength(userPassObj.value);
        if (length < minLength || length > maxLength) {
            warningObj.innerHTML = " * 密码长度为" + minLength + "- " + maxLength + "个字符！";
            checkUserPasswordObj1.value="0";
        }
        else {
            warningObj.innerHTML = "";
            checkUserPasswordObj1.value="1";
        }
    }
    else {
        warningObj.innerHTML = " * 密码不能为空！";
        checkUserPasswordObj1.value="0";
    }
}
function checkUserPass2(minLength, maxLength) {
    var userPassObj = o("UserPassword2");
    var warningObj = o("PasswordWarningMessage2");
    var checkUserPasswordObj2 = o("CheckUserPassword2");
    if (userPassObj.value != "") {
        var length = getStringLength(userPassObj.value);
        if (length < minLength || length > maxLength) {
            warningObj.innerHTML = " * 密码长度为" + minLength + "- " + maxLength + "个字符！";
            checkUserPasswordObj2.value="0";
        }
        else {
            if(userPassObj.value!=o("UserPassword1").value){
                 warningObj.innerHTML = " * 两次密码不一样，请重新输入！";
                 checkUserPasswordObj2.value="0";
            }
            else{
                warningObj.innerHTML = "";
                checkUserPasswordObj2.value="1";
            }
        }
    }
    else {
        warningObj.innerHTML = " * 密码不能为空！";
        checkUserPasswordObj2.value="0";
    }
}
function checkRegister() {
    if(o("CheckUserName").value=="0"){
        alertMessage("用户名有错误");
        return false;
    }
    if(o("CheckUserPassword1").value=="0"){
        alertMessage("密码有错误");
        return false;
    }
    if(o("CheckUserPassword2").value=="0"){
        alertMessage("确认密码有错误");
        return false;
    }
    if (o("CheckPhone").value == "0") {
        alertmessage("email有错误");
        return false;
    }
    if(o("SafeCode").value==""){
        alertMessage("验证码不能为空");
        return false;
    }
    if (!o("AgreeProtocol").checked) {
        alertMessage("请选择遵守条款");
        return false;
    }
//    return true

    $.ajax({
        url: '/Shop/User/Register.aspx?Action=Register',
        type: 'POST',
        data: $("form").serialize(),
        success: function (result) {
            var arr = result.split('|');
            if (arr[0] == "ok") {
                window.location.href = arr[1];
            }
            else {


                alertMessage(arr[1], 500);
            }
        }
    });
}