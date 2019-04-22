
// 加入收藏
$(".addFavo").click(function () {
    var title = $("title").html();
    var url = window.location.href;
    try {
        window.external.addFavorite(url, title);
    }
    catch (e) {
        try {
            window.sidebar.addPanel(title, url, "");
        }
        catch (e) {
            alert("抱歉，您所使用的浏览器无法完成此操作。\n\n加入收藏失败，请使用Ctrl+D进行添加");
        }
    }
});

var rediectUrl = getQueryString(document.location.search, 'RedirectUrl');

$('#fmLogin').validator({
    theme: 'alertMessage',
    fields: {
        name: { rule: "用户名 :required;" },
        password: { rule: "密码 :required" }
    },
    //验证成功
    valid: function (form) {
        $('.login_box .fr .con input[type=submit]').attr("disabled", "disabled");
        $('.login_box .fr .con input[type=submit]').css("background-color", "#808080");
        $('.login_box .fr .con input[type=submit]').css("border", "none");
        $('.login_box .fr .con input[type=submit]').css("text-shadow", "none");
        $('.login_box .fr .con input[type=submit]').css("cursor", "default");
        $('.login_box .fr .con .loginbtn').css("border", "none");
        $('.login_box .fr .con .loginbtn').css("background", "none");
        $('.login_box .fr .con input[type=submit]').val("正在登录，请稍候…");

        $.ajax({
            url: '/User/Login.aspx?Action=Submit&RedirectUrl=' + rediectUrl,
            type: 'POST',
            data: $(form).serialize(),
            success: function (result) {
                var arr = result.split('|');
                if (arr[0] == "ok") {
                    window.location.href = arr[2];
                }
                else {
                    $('.login_box .fr .con input[type=submit]').removeAttr("disabled");
                    $('.login_box .fr .con input[type=submit]').removeAttr("style");
                    $('.login_box .fr .con .loginbtn').css("border", "1px solid #CB2A2D");
                    $('.login_box .fr .con .loginbtn').css("background", "#E85356");
                    $('.login_box .fr .con input[type=submit]').val("登  录");

                    alertMessage(arr[1]);
                }
            }
        });
    }
});

$('#fmRegister').validator({
    theme: 'tulou_right',
    invalidClass: "form-input-invalid",
    msgWrapper: "div",
    focusInvalid: false,
    rules: {
        mobileRemote: function (element) {
            return $.get("/User/Register.aspx", { Action: "CheckMobile", value: element.value }, function (data) { });
        }
    },
    fields: {
        name: { rule: "手机号码 :required; mobile; mobileRemote;" },
        password: { rule: "密码 :required" },
        code: { rule: "短信验证码 :required;" },
        protocol: {
            rule: "checked",
            msg: "请接受服务协议",
            msgStyle: "display: block;float: right;margin-top: 6px;padding-left: 10px;right: 30px;",
            msgMaker: function (opt) {
                return '<label style="float:none; margin-top:10px; color: #FF3C3C">' + opt.msg + '</label>';
            }
        }
    },
    msgMaker: function(opt){
        return '<label class="' + opt.type + '">' + opt.msg + '</label>';
    },
    //验证成功
    valid: function (form) {
        $.ajax({
            url: '/User/Register.aspx?Action=Submit&RedirectUrl=' + rediectUrl,
            type: 'POST',
            data: $(form).serialize(),
            success: function (result) {
                var arr = result.split('|');
                if (arr[0] == "ok") {
                    alertMessage(arr[1]);
                    window.location.href = arr[2];
                }
                else {
                    alertMessage(arr[1]);
                }
            }
        });
    }
});

$(function () {
    var util = {
        wait: wait_sec,
        hsTime: function (that) {
            var _this = this;
            if (_this.wait == 0) {
                $(that).removeAttr("disabled").val('获取短信验证码');
                _this.wait = 60;
            } else {
                $(that).attr("disabled", true).val('在' + _this.wait + '秒后点此重发');
                _this.wait--;
                setTimeout(function () {
                    _this.hsTime(that);
                }, 1000)
            }
        }
    }

    $("#get_code").click(function () {
        var mobile = $.trim($('input[name=name]').val());
        if (!(new RegExp("^1[3-9]\\d{9}$")).test(mobile)) {
            alertMessage("请填写有效的手机号");
            return;
        }

        $.ajax({
            url: '/Ajax.aspx?Action=GetVerifyCode&mobile=' + mobile,
            type: 'GET',
            success: function (result) {
                var arr = result.split('|');
                if (arr[0] == "ok") {
                    util.wait = 60;
                    util.hsTime('#get_code');
                }
                else {
                    alertMessage(arr[1]);
                }
            }
        });
    })

    if (wait_sec > 0) util.hsTime('#get_code');
})