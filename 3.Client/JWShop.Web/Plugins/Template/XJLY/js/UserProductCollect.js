//添加到购物车
function addToCart(productID, productName, productStandardType, CurrentMemberPrice) {
    var check = true;
    var attributeName = "";
    if (productStandardType == "1") {
        var standardValue = getTextValue(os("name", "StandardValue"));
        for (var i = 0; i < standardValue.length; i++) {
            attributeName += standardValue[i] + ",";
            if (standardValue[i] == "") {
                check = false;
                break;
            }
        }
    }
    if (check) {
        var buyCount = 1;
        if (Validate.isInt(buyCount) && parseInt(buyCount) > 0) {
            if (attributeName != "") {
                productName = productName + "(" + attributeName.substr(0, attributeName.length - 1) + ")";
            }
            var currentMemberPrice = CurrentMemberPrice;
            var url = "/Ajax.html?Action=AddToCart&ProductID=" + productID + "&ProductName=" + encodeURIComponent(productName) + "&BuyCount=" + buyCount + "&CurrentMemberPrice=" + currentMemberPrice;
            Ajax.requestURL(url, dealAddToCart);
        }
        else {
            alertMessage("数量填写有错误");
        }
    }
    else {
        alertMessage("请选择规格");
    }
}
//立即购买
var redirect = false;
function buyNow(productID, productName, productStandardType) {
    addToCart(productID, productName, productStandardType);
    redirect = true;
}
function dealAddToCart(content) {
    if (content == "ok") {
        if (redirect) {//立即购买 
            redirect = false;
            window.location.href = "/Cart.html";
        }
        else {//添加到购物车
            alertMessage("添加成功");
            var buyCount = o("BuyCount").value;
            var currentMemberPrice = o("CurrentMemberPrice").value;
            o("ProductBuyCount").innerHTML = parseInt(o("ProductBuyCount").innerHTML) + parseInt(buyCount);
            //o("ProductTotalPrice").innerHTML = parseFloat(o("ProductTotalPrice").innerHTML) + parseInt(buyCount) * parseFloat(currentMemberPrice);
        }
    }
    else {
        redirect = false;
        alertMessage(content);
    }
}