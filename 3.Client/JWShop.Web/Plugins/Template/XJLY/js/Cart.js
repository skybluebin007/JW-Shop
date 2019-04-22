//==============初始化以及变量
var CarList;
$(function () {
    CarList = $("#cartList");
    readCart();
});
//==================全局方法
//计算显示总价格
function countAllMoney() {
    var result = 0;
    CarList.find(".cartList_body input[type='checkbox']").each(function () {
        if ($(this).prop("checked")) {
//            alert($(o("CartProductPrice" + $(this).val())).text());
//            alert($(o("CartProductPrice" + $(this).val())).text().substr(1));

            result = parseFloat(result) + parseFloat($(o("CartProductPrice" + $(this).val())).text());
        }
    });
    o("ProductTotalPrice").innerHTML = "￥" + result.toFixed(2);
    return result.toFixed(2);
}
function countAllBuyCount() {
    var count = 0;
    CarList.find(".cartList_body input[type='checkbox']").each(function () {

        count = parseInt(count) + parseInt($("#BuyCount" + $(this).val()).val());

    });    
    $("#rightProductBuyCount").html(count);
    $("#ProductBuyCount").html(count);
}
//==================读取购物车数据
function readCart() {
    loading("cartList", "购物车");
    Ajax.requestURL("/CartAjax.html?Action=Read", dealReadCart)
}
function dealReadCart(data) {
    CarList.html(data);
    //$("#rightProductBuyCount").html($("#cartProductCount").val());
}
//=============以下删除购物车
function deleteOrderProduct(strCartID, price,obj) {
    jConfirm("删除商品", "确定从购物车中删除此商品？", function (res) {
        if (res) {
            $($(obj).parent().parent()).fadeOut(500, function () {
                $(this).remove();                
            });
            var oldCount = o("BuyCount" + strCartID).value;
            Ajax.requestURL("/CartAjax.html?Action=DeleteCart&StrCartID=" + strCartID + "&OldCount=" + oldCount + "&Price=" + price + "", dealDeleteCart);
        }
    })
}
function dealDeleteCart(data) {
    try {       
        setTimeout(countAllMoney, 500);
        setTimeout(countAllBuyCount, 500);
    }
    catch (e) { alertMessage("删除失败"); }
}
//===============以下清空购物车
function clearCart() {
    jConfirm("清空购物车", "确定要清空购物车中的商品？", function (res) {
        if (res) {
            Ajax.requestURL("/CartAjax.html?Action=ClearCart", dealClearCart)
        }
    })
}
function dealClearCart(data) {
    CarList.find(".cartList_body").html("<tr><td height='100' align='center' colspan='7'>暂无商品...</td><tr>");
    o("ProductTotalPrice").innerHTML = "￥0";
}
//==============以下是选择商品
$(function () {
    CarList.find(".checkAll").change(function () {
        CarList.find(".checkbox").prop({ "checked": this.checked });
    });
    CarList.find(".checkbox").change(function () {
        if (CarList.find(".checkbox").length == cartList.find(".checkbox:checked").length) {
            CarList.find(".checkAll").prop({ "checked": true });
        } else {
            CarList.find(".checkAll").prop({ "checked": false });
        }
    });
});
//===============以下是加减
function Jian(strCartID, price, productCount, leftStorageCount, productWeight) {
    var objMe = $("#Jian" + strCartID);
    if (objMe.siblings(".txt").val() >= 2) {
        objMe.removeClass("noDrop").siblings(".add").removeClass("noDrop");
    } else {
        objMe.addClass("noDrop");
    }
    if (parseInt(o("BuyCountval" + strCartID).value) > 1) {
        o("BuyCountval" + strCartID).value = parseInt(o("BuyCountval" + strCartID).value) - 1;
        changeOrderProductBuyCount(strCartID, o("BuyCountval" + strCartID), price, productCount, leftStorageCount, productWeight);
    }
}
function Jia(strCartID, price, productCount, leftStorageCount, productWeight) {
    var objMe = $("#Jia" + strCartID);
    if (objMe.siblings(".txt").val() <= 999) {
        objMe.removeClass("noDrop").siblings(".minus").removeClass("noDrop");
    } else {
        objMe.addClass("noDrop");
    }
    o("BuyCountval" + strCartID).value = parseInt(o("BuyCountval" + strCartID).value) + 1;
    changeOrderProductBuyCount(strCartID, o("BuyCountval" + strCartID), price, productCount, leftStorageCount, productWeight);
}
//改变数量
function changeOrderProductBuyCount(strCartID, buyCountObj, price, productCount, leftStorageCount, productWeight) {
    var buyCount = buyCountObj.value;
    var oldCount = o("BuyCount" + strCartID).value;
    if (buyCount != oldCount) {
        if (Validate.isInt(buyCount) && parseInt(buyCount) > 0) {
            if (parseInt(buyCount) <= leftStorageCount) {
                o("BuyCount" + strCartID).value = buyCount;
                Ajax.requestURL("/CartAjax.html?Action=ChangeBuyCount&StrCartID=" + strCartID + "&BuyCount=" + buyCount + "&OldCount=" + oldCount + "&Price=" + price + "&ProductCount=" + productCount + "&ProductWeight=" + productWeight, dealChangeCart);
            }
            else {
                o("BuyCountval" + strCartID).value = oldCount;
                alertMessage("当前库存不能满足您的购买数量");
            }
        }
        else {
            o("BuyCountval" + strCartID).value = oldCount;
            alertMessage("数量填写有错误");
        }
    }
}
function dealChangeCart(data) {
    try {
        var dataArray = data.split("|");
        o("CartProductPrice" + dataArray[0]).innerHTML = parseFloat(dataArray[2]).toFixed(2);
        countAllMoney();
        countAllBuyCount();
    }
    catch (e) { alertMessage("修改失败"); }
}
//================以下是选中事件
function chooseAll(obj) {
    $("#cartList .cartList_body input[type='checkbox']").each(function () {
        $(this).prop("checked", $(obj).prop("checked"));
        chooseCB($(this));
    });
    $(".checkAll").prop("checked", $(obj).prop("checked"));
}
//计算所有选中商品的价格
function chooseCB(obj) {
    var strCartID = $(obj).val();
    var Price = $(obj).attr("pr");
    var productWeight = $(obj).attr("we");
    var opType = "Add";
    if ($(obj).prop("checked")) {
        opType = "Add";
    } else {
        opType = "Remove";
    }
    var buyCount = o("BuyCountval" + strCartID).value;
    var checkstr = "";
    CarList.find(".cartList_body input[type='checkbox']").each(function () {
        if ($(this).prop("checked")) {
            checkstr += $(this).val() + ",";
        }
    });
    countAllMoney();
    countAllBuyCount();
  
   
    //Ajax.requestURL("/CartAjax.html?Action=ChangeBuyNumber&StrCartID=" + strCartID + "&BuyCount=" + buyCount + "&checkProduct=" + checkstr + "&opType=" + opType + "&Price=" + Price + "&ProductWeight=" + productWeight, dealChangeCartNumber);
}
function dealChangeCartNumber(data) {
    try {
        o("ProductTotalPrice").innerHTML = "￥" + countAllMoney();
    }
    catch (e) { alertMessage("加载失败"); }
}
//==============以下 删除选中
function delChoose() {
    jConfirm("删除选中商品", "确定要删除选中的商品？", function (res) {
        if (res) {
            if (CarList.find(".cartList_body input[type='checkbox']:checked").length == 0) {
                alertMessage("请选择要删除的商品");
                return;
            }
            CarList.find(".cartList_body input[type='checkbox']:checked").each(function () {
                var StrCartID = $(this).val();
                Ajax.requestURL("/CartAjax.html?Action=DeleteCart&StrCartID=" + StrCartID + "&OldCount=" + o("BuyCount" + StrCartID).value + "&Price=" + o("price" + StrCartID).value + "&ProductCount=1&ProductWeight=" + o("ProductWeight" + StrCartID).value, removeTr);
                $(this).parents("ul").remove();
            });
            countAllMoney();
            countAllBuyCount();
        }
    })
}
function removeTr() {
    CarList.find(".cartList_body input[type='checkbox']:checked").parents("tr").fadeOut(500, function () {
        $(this).remove();
        countAllMoney();
        countAllBuyCount();
    });
}
//==============去结算
function goToPay() {
    if (CarList.find(".cartList_body input[type='checkbox']:checked").length == 0) {
        alertMessage("请选择要购买的商品");
        return;
    }
    var strIds = '';
    $(".cartList_body input[type='checkbox']:checked").each(function () {        
            strIds += ',' + $(this).val();        
    });
    if (strIds == '') {
        alertMessage('请选择需要购买的商品!');
        return;
    }
    strIds = strIds + ',';
    addCookie("CheckCart", strIds, 0);
    //alert(strIds);
    window.location.href = '/CheckOut.html'
}
