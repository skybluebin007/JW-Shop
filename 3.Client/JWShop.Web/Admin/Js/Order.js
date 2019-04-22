//删除1个订单
function deleteOrder(orderID) {
    if (window.confirm("确定删除订单？")) {
        tempOrderID = orderID;
        var url = "Ajax.aspx?Action=DeleteOrder&OrderID=" + orderID;
        Ajax.requestURL(url, dealDeleteOrder);
    }
}
//批量删除订单
function deleteOrderList() {
    if (window.confirm("确定删除订单？")) {
        var tempOrderID = "";
        $("input[name='check_OrderID']:checked").each(function () {
            tempOrderID += $(this).val() + ",";
        });
        var url = "Ajax.aspx?Action=DeleteOrderList&OrderID=" + tempOrderID;
        Ajax.requestURL(url, dealDeleteOrder);
    }
}

function dealDeleteOrder(content) {
    if (content != "ok") {
        alertMessage(content);
    }
    else {
        alertMessage("删除成功");
        location.reload();
    }
}
//恢复1个已删除的订单
function recoverOrder(orderId) {
    if (window.confirm("确定恢复订单？")) {
        //tempOrderID = orderID;
        //var url = "Ajax.aspx?Action=DeleteOrder&OrderID=" + orderID;
        //Ajax.requestURL(url, dealDeleteOrder);
        $.ajax({
            type: 'get',
            url: "Ajax.aspx",
            data: { Action: "RecoverOrder", OrderID: orderId },
            cache: false,
            //dataType: 'json',
            success: function (content) {
                if (content != "ok" ) {
                    alertMessage(content);
                }
                else {
                    alertMessage("恢复成功");
                    location.reload();
                }

            },
            error: function () {
                alertMessage("系统忙，请稍后重试");
            }
        });
    }
}