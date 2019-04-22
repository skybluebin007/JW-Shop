//=============订单操作start=================
var tempOrderStatus;
var tempOrderID;
function orderOperate(orderID, orderStatus) {
    if (window.confirm("确定操作？")) {
        tempOrderID = orderID;
        tempOrderStatus = orderStatus;
        var url = "/User/OrderAjax.html?Action=OrderOperate&OrderID=" + orderID + "&operate=" + orderStatus;
        Ajax.requestURL(url, dealOrderOperate);
    }
}
function dealOrderOperate(content) {
    if (content != "") {
        alertMessage(content);
    }
    else {
        alertMessage("操作成功");
        //if (tempOrderStatus == "1" || tempOrderStatus == "2") {//未付款或者未审核
        //    o("OrderStatus" + tempOrderID).innerHTML = "无效";
        //}
        //else if (tempOrderStatus == "5") {//已发货
        //    o("OrderStatus" + tempOrderID).innerHTML = "已收货";
        //}
        //else {
        //}
       
        //o("OrderOperate" + tempOrderID).innerHTML = "<a href=\"/User/OrderDetail.html?ID=" + tempOrderID + "\">查看</a>";
        window.location.reload();
    }
}
//=============订单操作End=================

//================扩展=====================
$(function () {
    $("#orderSearch").find(".txt").focus(function () {
        $(this).val("")
        $("#orderSearch").removeClass("redBorder");
        $("#orderSearch").animate({ easing: "", duration: 400 });
    }).blur(function () {
        var _this = $(this);
        if (_this.val() == "" || _this.val() == "搜索订单编号") {
            $("#orderSearch").animate({ easing: "", duration: 400, complete: function () {
                _this.val("搜索订单编号");
            }
            });
        }
    });
});
function submitForm() {
    var key = $("#keywords").val();
    if (key == "搜索订单编号" || key == "") {
        $("#orderSearch").addClass("redBorder");
        $("#keywords").val("搜索订单编号");
        return false;
    }
}

