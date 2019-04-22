function checkEmail() {
    var emailObj = o("Email");
    var checkEmailObj = o("CheckEmail");
    if (emailObj.value != "") {
        if (!Validate.isEmail(emailObj.value)) {//格式不对
            checkEmailObj.value = "1";
            return;
        }
        else {

            $.ajax({
                type: 'post',
                url: "/Ajax.html?Email=" + encodeURIComponent(emailObj.value) + "&Action=CheckEmail2",
                //data: {},
                cache: false,
                //dataType: 'json',
                success: function (data) {
                    if (data == "2") {
                        checkEmailObj.value = "0";
                    }
                    else {//被占用
                        checkEmailObj.value = "2";
                    }
                },
                error: function () { }
            });

        }
    }

}

$(function () {
    //个人信息submit
    $("#submit_info").click(function () {
        //checkEmail();
       //if ($("#Email").val() == "" || !Validate.isEmail($("#Email").val())) {
       //     alertMessage("Email输入错误");
       //     $("#Email").focus();
       //     return false;
       // }
        //if ($("#CheckEmail").val() == "2") {
        //    alertMessage("Email已被其他会员注册");
        //    $("#Email").focus();
        //    return false;
        //}
        if ($("#QQ").val() != "" && !Validate.isQQ($("#QQ").val())) {
            alertMessage("QQ号输入错误");
            $("#QQ").focus();
            return false;
        }
        if ($("#Tel").val() != "" && !Validate.isTel($("#Tel").val())) {
            alertMessage("固定电话输入错误");
            $("#Tel").focus();
            return false;
        }

        return true;
        $("input[name='file_code']").val(0);

    })
    //头像submit
    $("#submit_file").click(function () {
        $("input[name='file_code']").val(1);
    })
    //隐藏区域“中国”选项
    if ($("#UnlimitClass1").length > 0) { $("#UnlimitClass1").hide(); }
})