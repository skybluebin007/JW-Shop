//填写配送信息
function send(){
    if (!Validate.isDate(o(globalIDPrefix + "ShippingDate").value)) {
        $(".tab-title").find('span').eq(5).click();
        o(globalIDPrefix + "ShippingDate").focus();
        layer.msg('配送日期格式错误');
        return false;
    }
    if (o(globalIDPrefix + "ShippingNumber").value == "") {
        o(globalIDPrefix + "ShippingNumber").focus();
        layer.msg('配送单号不能为空');
        return false;
    }
    return true;
}
//收到货物检查
function received(){
    if (sendPoint != "0" && isActivity == "0") {
        return confirm("确认收货？该用户将得到" + sendPoint + "个积分");
    }
    return true;        
}
//撤销操作检查
function back(){
    if (sendPoint != "0" && orderStatus == "6" && isActivity == "0") {
        return confirm("确认撤销？该用户将减少" + sendPoint + "个积分");
    }
    return true;  
}
//取消订单
function cancel() {
    return confirm("确认取消这个订单？");
}
//显示订单中的商品
function showProduct(orderID){
    var index = layer.load(0);
    $.ajax({
        url: 'OrderAjax.aspx',
        type: 'GET',
        data: { 'Id': orderID},
        success: function (data) {
            $('#OrderAjax').html(data);
            layer.close(index);
        }
    });
}

//删除产品
function deleteOrderProduct(strOrderDetailID, strProductID, oldCount) {
    layer.confirm('确定删除？', { icon: 3, title: '提示' }, function (index) {
        layer.close(index);

        var orderID = getQueryString("Id");
        $.ajax({
            url: 'OrderAjax.aspx?Action=DeleteOrderProduct',
            type: 'GET',
            data: { 'StrOrderDetailID': strOrderDetailID, 'StrProductID': strProductID, 'OldCount': orderID, 'OrderID': orderID },
            success: function (result) {
                if (result != "") {
                    layer.msg(result, { icon: 2 });
                }
                else {
                    var orderID = getQueryString("Id");
                    showProduct(orderID);
                }
            }
        });
    });
}
//改变数量
function changeOrderProductBuyCount(strOrderDetailID, buyObj, strProductID, oldCount) {
    var buyCount = $(buyObj).val();
    layer.confirm('确定更改数量？', { icon: 3, title: '提示' }, function (index) {
        layer.close(index);

        var orderID = getQueryString("Id");
        $.ajax({
            url: 'OrderAjax.aspx?Action=ChangeOrderProductBuyCount',
            type: 'GET',
            data: { 'StrOrderDetailID': strOrderDetailID, 'BuyCount': buyCount, 'StrProductID': strProductID, 'OldCount': oldCount, 'OrderID': orderID },
            success: function (result) {
                if (result != "") {
                    layer.msg(result, { icon: 2 });
                }
                else {
                    $(buyObj).attr('org-value', buyCount);
                    var orderID = getQueryString("Id");
                    showProduct(orderID);
                }
            }
        });
    }, function () {
        $(buyObj).val($(buyObj).attr('org-value'));
    });
}
