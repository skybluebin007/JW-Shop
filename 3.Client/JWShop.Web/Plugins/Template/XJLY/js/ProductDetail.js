var preID = "Introduce";
var redirect = false;
//===========添加好友
function addFriend(userID) {
    jConfirm("添加好友", "确认是否添加为好友？", function (res) {
        if (res) {
            var url = "/Ajax.html?Action=AddFriend&UserID=" + userID; ;
            Ajax.requestURL(url, dealAddFriend);
        }
    });
}
function dealAddFriend(content) {
    if (content != "") {
        alertMessage(content);
    }
}
//=================评论
var tempCommentID = 0;
//反对评论
function against(commentID) {
    tempCommentID = commentID;
    var url = "/ProductCommentAjax.html?Action=Against&CommentID=" + commentID;
    Ajax.requestURL(url, dealAgainst);
}
function dealAgainst(content) {
    if (content == "ok") {
        alertMessage("反对成功");
        o("Against" + tempCommentID).innerHTML = parseFloat(o("Against" + tempCommentID).innerHTML) + 1;
    }
    else {
        alertMessage(content);
    }
}
//支持评论
function support(commentID) {
    tempCommentID = commentID;
    var url = "/ProductCommentAjax.html?Action=Support&CommentID=" + commentID;
    Ajax.requestURL(url, dealSupport);
}
function dealSupport(content) {
    if (content == "ok") {
        alertMessage("支持成功");
        o("Support" + tempCommentID).innerHTML = parseFloat(o("Support" + tempCommentID).innerHTML) + 1;
    }
    else {
        alertMessage(content);
    }
}
//切换页签
function show(id) {
    if (id != preID) {
        o("title" + preID).className = "productOff";
        o("product" + preID).style.display = "none";
        o("title" + id).className = "productOn";
        o("product" + id).style.display = "";
        preID = id;
    }
}
//记录浏览的产品
function recordProduct() {
    var historyProduct = getCookie("HistoryProduct");
    if (("," + historyProduct + ",").indexOf("," + productID + ",") == -1) {
        if (historyProduct == "") {
            historyProduct = productID;
        }
        else {
          if((","+historyProduct.toString()+",").indexOf(","+productID+",")<0)
            historyProduct = productID + "," + historyProduct;
        }
        //if (historyProduct.toString().indexOf(",") > -1) {
        //    if (historyProduct.split(",").length > 8) {
        //        historyProduct = historyProduct.substring(0, historyProduct.lastIndexOf(","));
        //    }
        //}
        addCookie("HistoryProduct", historyProduct, 0);
    }
}
var tempPage = 1;
//读取产品评论
function readProductComment() {
    loading("ProductCommentAjax", "用户评价");
    var url = "/ProductCommentAjax.html?ProductID=" + productID + "&Page=" + tempPage + "&commStyle=" + commStyle;
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
    var url = "/ProductSaleAjax.html?ProductID=" + productID + "&Page=" + tempPage2;
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
    readProductComment();

    //readProductSale();
    //productPhotoScroll();
}
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

//计算价格
function countPrice(count, leftStorageCount) {
    if (Validate.isInt(count) && parseInt(count) > 0) {
        if (count <= leftStorageCount) {
            var currentMemberPrice = o("CurrentMemberPrice").value;
            o("BuyCount").value = count;
            o("currentTotalMemberPrice").value = (parseInt(count) * parseFloat(currentMemberPrice)).toFixed(2);
        }
        else {
            alertMessage("当前库存不能满足您的购买数量");
            o("BuyCount").value = "";
            o("BuyCount").focus();
        }
    }
    else {
        alertMessage("数量填写有错误");
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
            var url = "/Ajax.html?Action=AddToCart&ProductID=" + productID + "&ProductName=" + encodeURIComponent(productName) + "&BuyCount=" + buyCount + "&standardValueList=" + encodeURIComponent(standardValueList) + "&CurrentMemberPrice=" + currentMemberPrice;
            $.ajax({
                type: 'get',
                url: url,
                //data: {},
                cache: false,
                //dataType: 'json',
                success: function (content) {
                    if (content == "ok") {
                        if (redirect) {//立即购买 
                            redirect = false;
                            window.location.href = "/Cart.html";
                        }
                        else {//添加到购物车
                            alertMessage("添加成功");
                            var buyCount = o("BuyCount").value;
                            var currentMemberPrice = $("#CurrentMemberPrice").val();                          
                            if ($("#ProductBuyCount").length > 0) $("#ProductBuyCount").text(parseInt($("#ProductBuyCount").text()) + parseInt(buyCount));
                            if ($("#rightProductBuyCount").length > 0) $("#rightProductBuyCount").text($("#ProductBuyCount").text());
                            //显示购物车下拉框（ajax加载最新加入的4件商品）
                            $(".carbox").addClass("carhover");
                            LoadNewCartProducts();
                        }
                    }
                    else {
                        redirect = false;
                        alertMessage(content);
                    }
                },
                error: function () { }
            });
           
            //Ajax.requestURL(url, dealAddToCart);
        }
        else {
            alertMessage("数量填写有错误");
            return;
        }
    }
    else {
        alertMessage("请选择规格");
        return;
    }
}
//立即购买

function buyNow(productID, productName, productStandardType) {
    redirect = true;
    addToCart(productID, productName, productStandardType);   
    //window.location.href = "/Cart.html";
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
            var currentMemberPrice = $("#CurrentMemberPrice").val();
            //o("ProductBuyCount").innerHTML = parseInt(o("ProductBuyCount").innerHTML) + parseInt(buyCount);
            //o("ProductTotalPrice").innerHTML = parseFloat(o("ProductTotalPrice").innerHTML) + parseInt(buyCount) * parseFloat(currentMemberPrice);
            if ($("#ProductBuyCount").length > 0) $("#ProductBuyCount").text(parseInt($("#ProductBuyCount").text()) + parseInt(buyCount));
            if ($("#rightProductBuyCount").length > 0) $("#rightProductBuyCount").text($("#ProductBuyCount").text());
            //显示购物车下拉框（ajax加载最新加入的4件商品）
            $(".carbox").addClass("carhover");           
            LoadNewCartProducts();
        }
    }
    else {
        redirect = false;
        alertMessage(content);
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
        var url = "/ProductBookingAdd.html?ProductID=" + productID + "&ProductName=" + encodeURIComponent(productName);
        window.location.href = url;
    }
    else {
        alertMessage("请选择规格");
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
    if (valueList.length > 1) {
        for (var i = 0; i < valueList.length; i++) {
            var pid = valueList[i].substr(0, valueList[i].indexOf(';'));
            if (valueList[i] == pid + ";" + attributeName) {
                productID = pid;
                break;
            }
        }
    } else {
        productID = $("#hProductID").val();
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
    window.location.href = "/ProductDetail-I" + productID + ".html";
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
    $(obj).addClass("hover").siblings("li").removeClass("hover");
    $(obj).parent().parent().find("li").removeClass("hover");
    $(obj).parent().addClass("hover");
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
    if (standardValue != "") {//如果有规格
        $.get("/Ajax.html", { action: "GetProductPriceAndStore", productID: productID, valueList: attributeName, rnd: Math.random() }, function (data) {
            if (data != "no") {
                var valueArr = data.split("|");
                $("#salePrice").html(valueArr[0]);
                $("#CurrentMemberPrice").val(valueArr[0]);
                $("#marketPrice").html(valueArr[1]);
                $("#productStore").html(valueArr[2]);
                $("#BuyCount").val(1);
                if (typeof (valueArr[3]) != "undefined" && valueArr[3] != "") {
                    $("#bigImg").attr("src", valueArr[3]);
                    $("#bigImg").attr("jqimg", valueArr[3]);
                } else {
                    var orgimg = $("#bigImg").attr("orgimg");
                    $("#bigImg").attr("src", orgimg);
                    $("#bigImg").attr("jqimg", orgimg);
                }
                if (parseInt(valueArr[2]) > 0) {                    
                    $("#buybox").show();
                    $("#checkinbox").hide();
                    //$("#addCar3").unbind().html("<s class=\"icon\"></s>加入购物车").attr("onclick", $("#addCartBTN").attr("onclick"));
                    $("#addCar3").show();
                    $("#booking1").hide();
                } else {
                    $("#buybox").hide();
                    $("#checkinbox").show();
                    //$("#addCar3").unbind().html("缺货登记").attr("onclick", $("#booking").attr("onclick"));
                    $("#addCar3").hide();
                    $("#booking1").show();
                }                
            } else {

            }
        });
    } else {
        var leftCount = $("#productStore").text();
        if (parseInt(leftCount) > 0) {
            $("#buybox").show();
            $("#checkinbox").hide();
            //$("#addCar3").unbind().html("<s class=\"icon\"></s>加入购物车").attr("onclick", $("#addCartBTN").attr("onclick"));
            $("#addCar3").show();
            $("#booking1").hide();
        } else {
            $("#buybox").hide();
            $("#checkinbox").show();
            //$("#addCar3").unbind().html("缺货登记").attr("onclick", $("#booking").attr("onclick"));
            $("#addCar3").hide();
            $("#booking1").show();
        }
    }
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


    $.ajax({
        type: "get",
        url: "/Ajax.html",
        data: { action: "CheckStore", productID: productID, buyCount: buyCount, standardType: standardType, valueList: attributeName, rnd: Math.random() },
        dataType: "json",
        async: false, //其他函数并须要等待AJAX返回数据后再往下执行
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
        dataType:'Json',
        data: { classId: _classId,price:_price },
        success: function (result) { 
            if (result.count <= 0) {
                $("#productfavor").html("");
                $("#productfavor").hide();
            }
            else {
                var _html="<span class='title fl' style='width:100%;'>可享受以下优惠：</span>";
                for (var i in result.dataList) {
                    var favor = result.dataList[i];
                
                    _html += "<span class='title2 fl' style='width:100%;'>"+favor.Name+":"+favor.Content+"</span>";
                   
                }
                $("#productfavor").html(_html);
                $("#productfavor").show();
            }
        },
        error: function () {
           
        }
    })
}

