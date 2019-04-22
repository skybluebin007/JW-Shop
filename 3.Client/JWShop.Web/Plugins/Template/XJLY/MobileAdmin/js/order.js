//审核订单
$(document.body).on('click', '.js-checkorder',function(){
//$(".js-checkorder").bind('click', function () {
    if (confirm("确定审核该订单?")) {
        var orderid = $(this).attr("_orderid");
        if (orderid != undefined && orderid != null && parseInt(orderid) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'operateorder', param: 'check', orderid: orderid },
                type: 'Post',
                dataType: 'Json',
                cache: false,
                async: false,
                success: function (res) {
                    if (res.ok) {
                        Msg("操作成功");
                        location.reload();
                    }
                    else {
                        Msg(res.msg);
                    }
                },
                error: function () {
                    Msg("系统忙，请稍后重试");
                }
            })
        }
        else {
            Msg("请求参数错误");
        }
    }
})
//取消订单
$(document.body).on('click', '.js-cancelorder',function(){
//$(".js-cancelorder").bind('click', function () {
    if (confirm("确定取消该订单?")) {
        var orderid = $(this).attr("_orderid");
        if (orderid != undefined && orderid != null && parseInt(orderid) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'operateorder', param: 'cancel', orderid: orderid },
                type: 'Post',
                dataType: 'Json',
                cache: false,
                async: false,
                success: function (res) {
                    if (res.ok) {
                        Msg("操作成功");
                        location.reload();
                    }
                    else {
                        Msg(res.msg);
                    }
                },
                error: function () {
                    Msg("系统忙，请稍后重试");
                }
            })
        }
        else {
            Msg("请求参数错误");
        }
    }
})
//确认收货
$(document.body).on('click', '.js-receiveorder', function () {
//$(".js-receiveorder").bind('click', function () {
    if (confirm("确定收货?")) {
        var orderid = $(this).attr("_orderid");
        if (orderid != undefined && orderid != null && parseInt(orderid) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'operateorder', param: 'receive', orderid: orderid },
                type: 'Post',
                dataType: 'Json',
                cache: false,
                async: false,
                success: function (res) {
                    if (res.ok) {
                        Msg("操作成功");
                        location.reload();
                    }
                    else {
                        Msg(res.msg);
                    }
                },
                error: function () {
                    Msg("系统忙，请稍后重试");
                }
            })
        }
        else {
            Msg("请求参数错误");
        }
    }
})

//删除订单
$(document.body).on('click', '.js-delorder', function () {
//$(".js-delorder").bind('click', function () {
    if (confirm("确定删除该订单?")) {
        var orderid = $(this).attr("_orderid");
        if (orderid != undefined && orderid != null && parseInt(orderid) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'operateorder', param: 'delete', orderid: orderid },
                type: 'Post',
                dataType: 'Json',
                cache: false,
                async: false,
                success: function (res) {
                    if (res.ok) {
                        Msg("操作成功");
                        location.reload();
                    }
                    else {
                        Msg(res.msg);
                    }
                },
                error: function () {
                    Msg("系统忙，请稍后重试");
                }
            })
        }
        else {
            Msg("请求参数错误");
        }
    }
})

//撤销操作
$(document.body).on('click', '.js-back', function () {
//$(".js-back").bind('click', function () {
    if (confirm("确定进行撤销操作?")) {
        var orderid = $(this).attr("_orderid");
        if (orderid != undefined && orderid != null && parseInt(orderid) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'operateorder', param: 'back', orderid: orderid },
                type: 'Post',
                dataType: 'Json',
                cache: false,
                async: false,
                success: function (res) {
                    if (res.ok) {
                        Msg("操作成功");
                        location.reload();
                    }
                    else {
                        Msg(res.msg);
                    }
                },
                error: function () {
                    Msg("系统忙，请稍后重试");
                }
            })
        }
        else {
            Msg("请求参数错误");
        }
    }
})