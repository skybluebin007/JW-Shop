//审核通过
$(document.body).on('click', '.js-approverefund', function () {
    if (confirm("确定审核通过该退款申请?")) {
        var id = $(this).attr("_id");
        if (id != undefined && id != null && parseInt(id) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'orderrefund', param: 'approve', id: id },
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

//审核拒绝
$(document.body).on('click', '.js-rejectrefund', function () {
    if (confirm("确定拒绝该退款申请?")) {
        var id = $(this).attr("_id");
        if (id != undefined && id != null && parseInt(id) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'orderrefund', param: 'reject', id: id },
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

//确认退款 js-finishrefund
$(document.body).on('click', '.js-finishrefund', function () {
    if (confirm("确定退款?")) {
        var id = $(this).attr("_id");
        if (id != undefined && id != null && parseInt(id) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'orderrefund', param: 'finish', id: id },
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
//取消退款 js-cancelrefund
$(document.body).on('click', '.js-cancelrefund', function () {
    if (confirm("确定取消退款?")) {
        var id = $(this).attr("_id");
        if (id != undefined && id != null && parseInt(id) > 0) {
            $.ajax({
                url: '/mobileadmin/ajax.html',
                data: { action: 'orderrefund', param: 'cancel', id: id },
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