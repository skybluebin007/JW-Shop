var tempPage = 1;
//搜索产品
function prepareContion(sType, Svalue) {
    if (checkInSearch(sType)) {
        replaceSearch(sType, Svalue);
    } else {
        searchContion += "&" + sType + "=" + Svalue;
    }
    if (sType != "ClassID") {
        $("#" + sType + Svalue + "").siblings().removeClass("hover");
        $("#" + sType + Svalue + "").addClass("hover");
    } else {
        $("#" + sType + Svalue + "").siblings().removeClass("hover1").removeClass("hover2").removeClass("hover3").removeClass("hover4").removeClass("hover5").removeClass("hover6");
        $("#" + sType + Svalue + "").addClass("hover" + Svalue + "");
    }
    searchProduct();
}
function checkInSearch(sType) {
    if (searchContion.indexOf(sType) >= 0) {
        return true;
    } else {
        return false;
    }
}
function replaceSearch(sType, sValue) {
    arrSearch = searchContion.split("&");
    i = 0;
    while (i < arrSearch.length) {
        subArr = arrSearch[i].split("=");
        if (subArr[0] == sType) {
            if (searchContion.indexOf("&" + arrSearch[i] + "&") > -1)
                searchContion = searchContion.replace("&" + arrSearch[i], "&" + sType + "=" + sValue);
            else if (searchContion.indexOf("&" + arrSearch[i]) > -1)
                searchContion = searchContion.replace("&" + arrSearch[i], "&" + sType + "=" + sValue);

            break;
        }
        i++;
    }
}

function deleteAll() {
    if (searchContion.indexOf("&") > 0) {
        searchContion = searchContion.substring(0, searchContion.indexOf("&"));
    }
    searchProduct();
}
function delContion(sType) {
    arrSearch = searchContion.split("&");
    i = 0;
    while (i < arrSearch.length) {
        subArr = arrSearch[i].split("=");
        if (subArr[0] == sType) {
            if (searchContion.indexOf("&" + arrSearch[i] + "&") > -1)
                searchContion = searchContion.replace("&" + arrSearch[i], "");
            else if (searchContion.indexOf("&" + arrSearch[i]) > -1)
                searchContion = searchContion.replace("&" + arrSearch[i], "");

            break;
        }
        i++;
    }

    //$(event.currentTarget).parent().children("a").removeClass("cur");
    //$(event.currentTarget).addClass("cur");
    searchProduct();
}

function searchProduct() {
    loading("listContainer", "产品");
    //alert(searchContion)
    var url = "/ProductAjax.html?" + searchContion + "&Page=" + tempPage;
    Ajax.requestURL(url, dealSearchProduct);
}
function dealSearchProduct(content) {
    o("listContainer").innerHTML = content.substr(0, content.lastIndexOf("#"));
    var elem = $("#listContainer"), a = elem.find(".item"), b = elem.find(".g img");

    // 商品列表
    a.hover(
		function () {
		    $(this).addClass("hover");
		},
		function () {
		    $(this).removeClass("hover");
		}
	);
    b.hover(
		function () {
		    $(this).addClass("css3Scale");
		},
		function () {
		    $(this).removeClass("css3Scale");
		}
	).parent().attr({ "title": "全网正品 支持货到付款" });
    $("#listContainer .ls img").hover(
		function () {
		    $(this).parents("dd").addClass("cur").siblings("dd").removeClass("cur").end().parents(".item").find(".g img").attr({ "src": $(this).attr("bigimg") });
		},
		function () {
		    //
		}
	);
}

$(function () {    
    $(".tod > .ug >a").click(function () {
        if ($(this).attr("class") != "hover") {
            $(this).attr("class", "hover");
            var name = $(this).attr("name");
            prepareContion(name, 1);
        } else {
            $(this).attr("class", "");
            var name = $(this).attr("name");
            prepareContion(name, 0);
        }
        searchProduct();
    });
    $(".tid > .d >a").click(function () {
        $(".tid > .d >a").attr("class", "");
        $(this).attr("class", "cur");
        var name = $(this).attr("value");
        changeProductOrderType(name);
    })
})
//改变排序方式
function changeProductOrderType(productOrderType) {
    tempPage = 1;
    addCookie("ProductOrderType", productOrderType, 0);
    searchProduct();
}
//选择产品展现方式
function selectShowWay(way) {
    tempPage = 1;
    var oldWay = getCookie("ProductShowWay");
    if (oldWay != way) {
        addCookie("ProductShowWay", way, 0);
        searchProduct();
    }
    changeShowDisplay();
}
//页面初始化
function pageInit() {
    var productOrderType = getCookie("ProductOrderType");
    if (productOrderType == "" || productOrderType == "undefined") {
        productOrderType = "ID";
    }
    //o("ProductOrderType").value = productOrderType
    changeShowDisplay()
    searchProduct();
}
//显示的方式的展现
function changeShowDisplay() {
    var productShowWay = getCookie("ProductShowWay");
    var content = "";
    if (productShowWay == "2") {
        content = "<img src=\"" + templatePath + "Style/Images/pictureOff.png\" title=\"图片方式\" onclick=\"selectShowWay(1)\"  /> <img src=\"" + templatePath + "Style/Images/listOn.png\" title=\"列表方式\" onclick=\"selectShowWay(2)\" />";
    }
    else {
        content = "<img src=\"" + templatePath + "Style/Images/pictureOn.png\" title=\"图片方式\" onclick=\"selectShowWay(1)\"  /> <img src=\"" + templatePath + "Style/Images/listOff.png\" title=\"列表方式\" onclick=\"selectShowWay(2)\" />";
    }
    //o("showWayDiv").innerHTML = content;
}
//分页
function goPage(page) {
    tempPage = page;
    searchProduct();
}
pageInit();
//收藏产品
function collectProduct(productID) {
    var url = "/Ajax.html?Action=Collect&ProductID=" + productID; ;
    Ajax.requestURL(url, dealCollectProduct);
}
function dealCollectProduct(content) {
    if (content != "") {
        alertMessage(content);
    }
}
//添加到购物车
function addToCart(productID, productName, productStandardType, CurrentMemberPrice, obj) {

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
        if (attributeName == "") {
            var oname = $(obj).attr("oname");
            attributeName = oname;
        }
    }

    if (check) {
        var buyCount = 1;
        if (Validate.isInt(buyCount) && parseInt(buyCount) > 0) {
            if (attributeName != "") {
                productName = productName + "(" + attributeName + ")";
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
function dealAddToCart(content) {
    alert(content);
    if (content == "ok") {

        alertMessage("添加成功");
        var buyCount = 1;
        o("ProductBuyCount").innerHTML = parseInt(o("ProductBuyCount").innerHTML) + parseInt(buyCount);
        //o("ProductTotalPrice").innerHTML = parseFloat(o("ProductTotalPrice").innerHTML) + parseInt(buyCount) * parseFloat(currentMemberPrice);

    }
    else {
        redirect = false;
        alertMessage(content);
    }
}
