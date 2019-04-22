//申请退款
$('form').validator({
    theme: 'tulou_right',
    invalidClass: "form-input-invalid",
    msgWrapper: "div",
    //https://github.com/niceue/nice-validator/issues/81
    //自定义远程验证（remote）返回的结果格式
    dataFilter: function (data) {
        return data.ErrorCodeMsg;
    },
    rules: {
        countRefundRemote: function (element) {
            if (orderDetailId > 0) {
                return $.ajax({
                    url: '/User/OrderRefundApply.aspx?Action=CalcCanRefundMoney',
                    type: 'GET',
                    data: { 'orderId': orderId, 'orderDetailId': orderDetailId, 'num': element.value },
                    success: function (result) {
                        var json = JSON.parse(result);

                        if (json.CanRefund) {
                            $('input[name=refund_money]').val(json.CanRefundMoney);
                            canRefundMoney = json.CanRefundMoney;
                        }
                    }
                });
            }
        },
        moneyRefund: function (element) {
            if (element.value <= 0) return '退款金额必须大于0';

            if (element.value > canRefundMoney) {
                $(element).val(canRefundMoney);
                return '本次最多退款 ' + canRefundMoney + ' 元';
            }
        }
    },
    fields: {
        refund_count: { rule: '退款数量 :required; integer[+]; countRefundRemote;' },
        refund_money: { rule: '退款金额 :required; money; moneyRefund;' }
    },
    msgMaker: function (opt) {
        return '<label class="' + opt.type + '" style="line-height:30px;">' + opt.msg + '</label>';
    },
    //验证成功
    valid: function (form) {
        if (window.confirm('确认申请退款？')) {
            $.ajax({
                url: '/User/OrderRefundApply.aspx?Action=Submit&orderId=' + orderId + '&orderDetailId=' + orderDetailId,
                type: 'POST',
                data: $(form).serialize(),
                success: function (result) {
                    var arr = result.split('|');
                    if (arr[0] == "ok") {
                        alert("退款申请成功");
                        if (window.location.href.toLowerCase().indexOf("/mobile/") > -1)
                            window.location.href = "/mobile/user/orderrefunddetail.html?id=" + arr[1];
                        else
                            window.location.href = "/user/orderrefunddetail.html?id=" + arr[1];
                    }
                    else {
                        alertMessage(arr[1]);
                    }
                }
            });
        }
    }
});

//取消退款
function cancelRefund(id) {
    if (window.confirm('确认取消退款？')) {
        $.ajax({
            url: '/User/OrderRefundDetail.aspx?Action=CancelRefund&id=' + id,
            type: 'GET',
            success: function (result) {
                var arr = result.split('|');
                if (arr[0] == "ok") {
                    alert("取消退款成功");
                    window.location.reload();
                }
                else {
                    alertMessage(arr[1]);
                }
            }
        });
    }
}