function checkPoint(id) {
    checkLogon(location.href, function () {
        $.ajax({
            url: '/mobile/PointExchange.aspx?Action=CheckPoint',
            type: 'GET',
            data: { Id: id },
            success: function (result) {
                var arr = result.split('|');
                if (arr[0] == "ok") {
                    window.location.href = "/mobile/pointexchange/" + id + ".html";
                }
                else app.jMsg(arr[1]);
            }
        });
    });
}
function checkLogon(redirect, callback) {
    $.ajax({
        url: '/Ajax.aspx?Action=VerifyLogin',
        type: 'GET',
        dataType: 'JSON',
        success: function (json) {
            if (!json.haslogin) {                
                alert("您尚未登录");
                window.location.href = "/mobile/User/Login.html?RedirectUrl=" + redirect;
            }
            else {
                callback();
            }
        }
    });
}
$('form').validator({
    theme: 'alertMsg',
    focusInvalid: false,
    stopOnError: true,
    fields: {
        num: { rule: '兑换数量 : required; integer[+]' },
        username: { rule: '收货人姓名 :required' },
        mobile: { rule: '手机号码 :required;mobile' },
        address: { rule: '联系地址 :required' }
    },
    //验证成功
    valid: function (form) {
        if (!confirm("确认兑取该商品？")) return;

        var me = this;
        me.holdSubmit();

        $.ajax({
            url: '/mobile/PointExchange.aspx?Action=Submit',
            type: 'POST',
            data: $(form).serialize(),
            success: function (result) {
                me.holdSubmit(false);               
                var arr = result.split('|');
                if (arr[0] == "ok") {
                    app.jMsg("兑换成功", 500);
                    $(form)[0].reset();
                }
                else {
                 
                    app.jMsg(arr[1], 500);
                }
            },
            error: function () {
                me.holdSubmit(false);
            }
        });
    }
});