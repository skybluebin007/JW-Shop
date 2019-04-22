var preID = "Introduce";
//记录浏览的产品
function recordProduct() {
    var historyProduct = getCookie("HistoryProduct");
    if (("," + historyProduct + ",").indexOf("," + productID + ",") == -1) {
        if (historyProduct == "") {
            historyProduct = productID;
        }
        else {
            historyProduct = productID + "," + historyProduct;
        }
        if (historyProduct.toString().indexOf(",") > -1) {
            if (historyProduct.split(",").length > 8) {
                historyProduct = historyProduct.substring(0, historyProduct.lastIndexOf(","));
            }
        }
        addCookie("HistoryProduct", historyProduct, 0);
    }
}
var tempPage = 1;
//读取产品评论
function readProductComment() {
    loading("ProductCommentAjax", "用户评价");
    var url = "/Mobile/ProductCommentAjax.aspx?ProductID=" + productID + "&Page=" + tempPage;
    Ajax.requestURL(url, dealSearchProductComment);
}
function dealSearchProductComment(content) {
    o("ProductCommentAjax").innerHTML = content
}
//分页
function goPage(page) {
    tempPage = page;
    readProductComment();
}
var tempPage2 = 1;
//读取购买记录
function readProductSale() {
    loading("ProductSaleAjax", "购买记录");
    var url = "/ProductSaleAjax.aspx?ProductID=" + productID + "&Page=" + tempPage2;
    Ajax.requestURL(url, dealSearchProductSale);
}
function dealSearchProductSale(content) {
    o("ProductSaleAjax").innerHTML = content
}
//分页
function goPage2(page) {
    tempPage2 = page;
    readProductSale();
}
//页面初始化
function pageInit(allowComment) {
    recordProduct();
    if (allowComment == 1) {
        readProductComment();
    }
    //readProductSale();
    //productPhotoScroll();
}
//收藏产品
function collectProduct(productID) {
    var url = "/Ajax.aspx?Action=Collect&ProductID=" + productID; ;
    Ajax.requestURL(url, dealCollectProduct);
}
function dealCollectProduct(content) {
    if (content != "") {
        app.jMsg(content);
    }
}
$(function () {
    //选择单一规格
    $(".StandardType1 .standard a").on("click", function () {
        var value = $(this).attr("_v");
        var id = $(this).attr("_i");
        o("Standard_" + id).innerHTML = value;
        o("StandardValue_" + id).value = value;
        $(this).siblings("a").removeClass("current");
        $(this).addClass("current");
        var standardValue = getTextValue(os("name", "StandardValue"));
        var check = true;
        for (var i = 0; i < standardValue.length; i++) {
            if (standardValue[i] == "") {
                check = false;
                break;
            }
        }
        if (check) {
            var standardRecordValueList = o("StandardRecordValueList").value;
            var attributeName = "";
            for (var i = 0; i < standardValue.length; i++) {
                attributeName += standardValue[i] + ",";
            }
            attributeName = attributeName.substr(0, attributeName.length - 1)
            if (standardRecordValueList.indexOf("|" + productID + "," + attributeName + "|") == -1) {
                o("Standard_" + id).innerHTML = "请选择";
                o("StandardValue_" + id).value = "";
                $(this).siblings("a").removeClass("current");
                app.jMsg("没有该规格的产品");
            }
        }
    });

    //选择产品组规格
    $(".StandardType2 .standard a").on("click", function () {
        var value = $(this).attr("_v");
        var id = $(this).attr("_i");
        o("Standard_" + id).innerHTML = value;
        o("StandardValue_" + id).value = value;
        var standardValue = getTextValue(os("name", "StandardValue"));
        var check = true;
        for (var i = 0; i < standardValue.length; i++) {
            if (standardValue[i] == "") {
                check = false;
                break;
            }
        }
        if (check) {
            var standardRecordValueList = o("StandardRecordValueList").value;
            var attributeName = "";
            for (var i = 0; i < standardValue.length; i++) {
                attributeName += standardValue[i] + ",";
            }
            attributeName = attributeName.substr(0, attributeName.length - 1)
            standardRecordValueList = standardRecordValueList.substr(1, standardRecordValueList.length - 2);
            var valueList = standardRecordValueList.split('|');
            for (var i = 0; i < valueList.length; i++) {
                var pid = valueList[i].substr(0, valueList[i].indexOf(','));
                if (valueList[i] == pid + "," + attributeName) {
                    productID = pid;
                    break;
                }
            }

            window.location.href = "/Mobile/ProductDetail-I" + productID + ".html";
        }

    });

    $(".number_panel a").on("click", function () {
        var n = $("#BuyCount").val();
        var l = $("#leftStorageCount").val();
        if ($(this).attr("_a") == "plus") {
            n++;
        } else {
            n--;
        }
        if (n > 0 && n <= parseInt(l)) {
            countPrice(n, l);
        }
    });
});

//计算价格
function countPrice(count, leftStorageCount) {
    if (Validate.isInt(count) && parseInt(count) > 0) {
        if (count <= leftStorageCount) {
            var currentMemberPrice = o("CurrentMemberPrice").value;
            o("BuyCount").value = count;
            o("currentTotalMemberPrice").value = (parseInt(count) * parseFloat(currentMemberPrice)).toFixed(2);
        }
        else {
            app.jMsg("当前库存不能满足您的购买数量");
            o("BuyCount").value = "";
            o("BuyCount").focus();
        }
    }
    else {
        app.jMsg("数量填写有错误");
    }
}
//添加到购物车
function addToCart(productID, productName, productStandardType, obj) {
    var check = true;
    var attributeName = "";
    var standardValueList = "";
    if (productStandardType == "1") {
        var standardValue = getTextValue(os("name", "StandardValue"));
        standardValueList = standardValue.join(";");
        for (var i = 0; i < standardValue.length; i++) {
            attributeName += standardValue[i] + ";";
            if (standardValue[i] == "") {
                check = false;
                break;
            }
        }
    }    
    if (check) {
        var buyCount = o("BuyCount").value;
        if (Validate.isInt(buyCount) && parseInt(buyCount) > 0) {
            if (attributeName != "") {
                productName = productName + "(" + attributeName.substr(0, attributeName.length - 1) + ")";
            }
            var currentMemberPrice = $("#CurrentMemberPrice").val();
            var url = "/Ajax.aspx?Action=AddToCart&ProductID=" + productID + "&ProductName=" + encodeURIComponent(productName) + "&BuyCount=" + buyCount + "&standardValueList=" + encodeURI(standardValueList) + "&CurrentMemberPrice=" + currentMemberPrice;
            Ajax.requestURL(url, dealAddToCart);
        }
        else {
            app.jMsg("数量填写有错误");
        }
    }
    else {
        app.jMsg("请选择规格");
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
            window.location.href = "/Mobile/Cart.html";
        }
        else {//添加到购物车
            app.jMsg('恭喜，已加入到购物车');
            var buyCount = o("BuyCount").value;
            if ($('[ig-carnum]').length) $('[ig-carnum]').text(parseInt(o("ProductBuyCount").innerHTML) + parseInt(buyCount));
            //o("ProductTotalPrice").innerHTML = parseFloat(o("ProductTotalPrice").innerHTML) + parseInt(buyCount) * parseFloat(currentMemberPrice);
        }
    }
    else {
        redirect = false;
        app.jMsg(content);
    }
}

//缺货登记
function bookingProduct(productID, productName, productStandardType) {
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
        if (attributeName != "") {
            productName = productName + "(" + attributeName.substr(0, attributeName.length - 1) + ")";
        }
        var url = "/Mobile/ProductBookingAdd.html?ProductID=" + productID + "&ProductName=" + encodeURIComponent(productName);
        window.location.href = url;
    }
    else {
        app.jMsg("请选择规格");
    }
}


//==============================================
$(function () {
    GetProductFavor();
    ShowChooseText();
});
//显示已选择文本
function ShowChooseText() {
    //显示已选择
    var standardValue = getTextValue(os("name", "StandardValue"));
    var attributeName = "";
    for (var i = 0; i < standardValue.length; i++) {
        attributeName += standardValue[i] + ",";
    }
    $("#buyPro").find(".rowSelected").html("\"" + attributeName.substr(0, attributeName.length - 1) + "\"");
    getProductPriceAndStore();
    //读取商品优惠
    GetProductFavor();
}
function GetProductID() {
    var productID = "";
    var standardRecordValueList = o("StandardRecordValueList").value;
    var standardValue = getTextValue(os("name", "StandardValue"));
    var attributeName = "";
    for (var i = 0; i < standardValue.length; i++) {
        attributeName += standardValue[i] + ";";
    }
    attributeName = attributeName.substr(0, attributeName.length - 1)
    standardRecordValueList = standardRecordValueList.substr(1, standardRecordValueList.length - 2);
    var valueList = standardRecordValueList.split('|');
    var productID = "";
    for (var i = 0; i < valueList.length; i++) {
        var pid = valueList[i].substr(0, valueList[i].indexOf(';'));
        if (valueList[i] == pid + ";" + attributeName) {
            productID = pid;
            break;
        }
    }
    return productID;
}
//产品组合规格点击
function selectMultiStandard(value, id) {
    var oldValue = $("#StandardValue_" + id).val();
    $("#StandardValue_" + id).val(value);
    var productID = GetProductID();
    if (productID == "") {
        alert('该产品未上市,不能查看');
        $("#StandardValue_" + id).val(oldValue);
        return;
    }
    window.location.href = "/Mobile/ProductDetail-I" + productID + ".html";
}
//产品单规格点击
function selectSingleStandard(value, id, obj) {
    var oldValue = $("#StandardValue_" + id).val();
    $("#StandardValue_" + id).val(value);
    var productID = GetProductID();
    if (productID == "") {
        alert('该产品未上市,不能查看');
        $("#StandardValue_" + id).val(oldValue);
        return;
    } else {
        getProductPriceAndStore();
    }
    $(obj).addClass("cur").siblings("dd").removeClass("cur");
    ShowChooseText();
}

function getProductPriceAndStore() {
    var productID = GetProductID();
    var standardValue = getTextValue(os("name", "StandardValue"));
    var attributeName = "";
    for (var i = 0; i < standardValue.length; i++) {
        attributeName += standardValue[i] + ";";
    }
    attributeName = attributeName.substr(0, attributeName.length - 1)
    $.get("/Ajax.aspx", { action: "GetProductPriceAndStore", productID: productID, valueList: attributeName, rnd: Math.random() }, function (data) {
        if (data != "no") {
            var valueArr = data.split("|");
            $("#salePrice").html(valueArr[0]);
            $("#CurrentMemberPrice").val(valueArr[0]);
            $("#marketPrice").html(valueArr[1]);
            $("#productStore").html(valueArr[2]);
            $("#BuyCount").val(1);
            if (parseInt(valueArr[2]) > 0) {
                $("[buybox]").show();
                $("[checkinbox]").hide();
                
            } else {
                $("[buybox]").hide();
                $("[checkinbox]").show();                
            }
        } else {

        }
    })
}

function CheckStore(buyCount) {
    var backStr = false;
    var productID = GetProductID();
    var standardValue = getTextValue(os("name", "StandardValue"));
    var attributeName = "";
    var standardType = $("#standardType").val();
    for (var i = 0; i < standardValue.length; i++) {
        attributeName += standardValue[i] + ";";
    }
    attributeName = attributeName.substr(0, attributeName.length - 1)

    //alert(productID + "_" + buyCount + "_" + attributeName);
    $.ajax({
        type: "get",
        url: "/Ajax.aspx",
        data: { action: "CheckStore", productID: productID, buyCount: buyCount, standardType: standardType, valueList: attributeName, rnd: Math.random() },
        dataType: "json",
        async: false,//其他函数并须要等待AJAX返回数据后再往下执行
        success: function (data) {
            if (data == 0) {
                backStr = false;
            } else {
                backStr = true;
            }
        }
    });

    return backStr;
}
//读取该商品享受的商品优惠活动列表
function GetProductFavor() {
    var _price = $("#CurrentMemberPrice").val();
    var _classId = $("#pcid").val();
    $.ajax({
        url: '/ajax.aspx?Action=ReadProductFavorList',
        type: 'GET',
        dataType: 'Json',
        data: { classId: _classId, price: _price },
        success: function (result) {
            if (result.count <= 0) {
                $("#productfavor").html("");
                $("#productfavor").hide();
            }
            else {
                var _html = "<span class='fee' style='width:100%;display:block;'>可享受以下优惠：</span>";          
                for (var i in result.dataList) {
                    var favor = result.dataList[i];

                    _html += "<div class='feeli'>" + favor.Name + ":" + favor.Content + "</div>";
                
                }
                $("#productfavor").html(_html);
                $("#productfavor").show();
            }
        },
        error: function () {

        }
    })
}
