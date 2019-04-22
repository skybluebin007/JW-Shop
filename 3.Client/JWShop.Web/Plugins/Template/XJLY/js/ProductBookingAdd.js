//提交检查
function checkSubmit(){
    var relationUser = o("RelationUser").value;
    if (relationUser == "") {
        o("RelationUser").focus();
        alertMessage("联系人不能为空", 500);
        return false;
    }
    var email = o("Email").value;
    if (email == "") {
       o("Email").focus();
        alertMessage("Email不能为空", 500);
        return false;
    }
    if (!Validate.isEmail(email)) {
        o("Email").focus();
        alertMessage("Email错误");

        return false;
    }

    var tel = o("Tel").value;
    if (tel == "") {
        o("Tel").focus();
        alertMessage("联系电话不能为空", 500);
        return false;
    }
    if (!Validate.isTel(tel) && !Validate.isMobile(tel)) {
        $("#Tel").focus();
        alertMessage("联系电话错误");

        return false;
    }
    return true;
}