//=============初始化
$(function () {
    selectProductFavor();
    readUserAddress();
    //读取商品优惠礼品列表
    selectGifts();

    /*使用积分*/
    $(".wrapper").on("click", '#M_UsePoint', function () {
        $("#usePoint .M-point-hide").toggleClass("hidden");
        $("#usePoint .M-piont-msg").addClass("hidden");
        $("#M_pointTotalDischarge").html("0.00");
        $("#M_pointToUse").val("0");
        $("#M_pointToUse").attr('_old', 0);
        $("#point").val('0');
        $("#pointMoney").val('0');
        showMoneyDetail();
    });

    $(".wrapper").on("keyup", '#M_pointToUse', function () {
        var _cost = $(this).val();
        if (_cost == "") {
            $("#usePoint .M-piont-msg").addClass("hidden");
            $("#M_pointTotalDischarge").html("0.00");
            $("#point").val('0');
            $(this).attr('_old', 0);
            showMoneyDetail();

            return;
        }
        if (Validate.isInt(_cost)) {
            _cost = parseInt(_cost);
            var _maxUse = $("#pointLeft").val();
            var _rate = $("#pointRate").val();
            if (_cost > _maxUse) {
                $("#u-point-msg").html("您本次最多可使用" + _maxUse + "积分");
                $("#usePoint .M-piont-msg").removeClass("hidden");
                $(this).val($(this).attr('_old'));
            } else {
                $("#usePoint .M-piont-msg").addClass("hidden");
                var _pointMoney = Math.Round(_cost * _rate / 100, 2, true);
                $("#M_pointTotalDischarge").html("-" + _pointMoney);
                $("#pointMoney").val(_pointMoney);
                $("#point").val(_cost);
                $(this).val(_cost);
                $(this).attr('_old', _cost);
                showMoneyDetail();
            }
        } else {
            $("#u-point-msg").html("积分数必须为大于等于0的整数");
            $("#usePoint .M-piont-msg").removeClass("hidden");
            $(this).val($(this).attr('_old'));
        }
    });
});
//=============读取用户地址
function readUserAddress() {
    loading("CheckOutAddressAjax", "用户地址");
    var id = 0;
    if ($("input[name='UserAddress']:checked").length>0) {
        id = $("input[name='UserAddress']:checked").val();
    }
    if (id == 0) id = $("input[name='UserAddress']:first").val();
    var url = "CheckOutAddressAjax.html?ID=" + id;
    Ajax.requestURL(url, dealReadUserAddress);
}
function dealReadUserAddress(data) {
    var reg = /<script(.|\n)*?>((.|\n|\r\n)*)?<\/script>/im;
    var match = data.match(reg);
    var myScript = "";
    if (match != null) {
        myScript = match[2];
        var script = document.createElement("script");
        script.text = myScript;
        document.getElementsByTagName("head")[0].appendChild(script);
    }
    var html = data.replace(reg, "");
    o("CheckOutAddressAjax").innerHTML = html;
    readShippingList();
}
//============读取配送方式
function readShippingList() {
    loading("ShippingListAjax", "配送方式");
    var count = parseInt(o("UnlimitClassGradeCount").value);
    var regionID = readSearchClassID("");
    var url = "CheckOutShippingAjax.aspx?RegionID=" + regionID;
    Ajax.requestURL(url, dealReadShippingList);
}
function dealReadShippingList(data) {
    var arr = data.split('||');
    $('#ShippingListAjax').html(arr[0]);

    //默认第一个配送方式赋值给checkout页面
    var value = $("#ShippingListAjax").find('.checked').find('label').text();
    $(".order-logistics > span").text(value);

    //优惠活动列表展示
    $('#FavorList').html(arr[1]);
    var shippingId = $("input[name='ShippingID']:checked").val();
    if (shippingId == "" || typeof (shippingId) == "undefined") {
        $('#FavorList').hide();
    }
    selectShipping();
}
//===========选择配送方式
function selectShipping() {
    var shippingID = getRadioValue(os("name", "ShippingID"));
    if (shippingID > 0) { $('#FavorList').show(); }
    var regionID = readSearchClassID("");
    $("#RegionID").val(regionID);//纪录地域选择，用于页面提交
    var favorId = $("input[name='FavorableActivity']:checked").val();
    var url = "/mobile/CheckOutShippingAjax.aspx?Action=SelectShipping&RegionID=" + regionID + "&ShippingID=" + shippingID + "&favorId="+favorId;
    Ajax.requestURL(url, dealSelectShipping);
}
function dealSelectShipping(data) {
    var dataArray = data.split("|");  
    if (dataArray[0] != 'ok') {
        app.jMsg(dataArray[1]);
    }
    else {
        o("ShippingMoney").value = dataArray[1];
        o("orderfavorableMoney").value = dataArray[2];
        var _newFavorMoney = parseFloat($('#orderfavorableMoney').val()) + parseFloat($('#productfavorableMoney').val());
        $("#FavorableMoney").val(_newFavorMoney.toFixed(2));
        showMoneyDetail();
    }
}
//==================显示金额
function showMoneyDetail() {
    var ProductBuyCount =$("#hiProductBuyCount").val();
    var productMoney = parseFloat(o("ProductMoney").value);
    var shippingMoney = parseFloat($('#ShippingMoney').val());
    if (isNaN(shippingMoney)) {
        shippingMoney = 0;
    }
    var pointMoney = parseFloat($("#pointMoney").val());
    var couponMoney = parseFloat($("#CouponMoney").val());
    if ($("input[name='UserCoupon']:checked").length > 0) {
        var userCoupon = $("input[name='UserCoupon']:checked").val();
        var arr = userCoupon.split("|");
        couponMoney = parseFloat(arr[1]);
    }
    var _newFavorMoney = parseFloat($('#orderfavorableMoney').val()) + parseFloat($('#productfavorableMoney').val());
    var favorableMoney = _newFavorMoney.toFixed(2);
    if (productMoney + shippingMoney - pointMoney - couponMoney - favorableMoney>= 0) {
        var content = "<dd><span>￥" + productMoney + "</span><label>" + ProductBuyCount + "</lable> 件商品小计：</dd>";
        content += "<dd><span>+￥" + shippingMoney + "</span><label>物流费用：<label></dd>";
        content += "<dd><span>-￥" + favorableMoney + "</span><label>优惠金额：<label></dd>";
        content += "<dd><span>-￥" + couponMoney + "</span><label>优惠券：<label></dd>";
        content += "<dd><span>-￥" + pointMoney + "</span><label>积分抵扣：<label></dd>";
        content += "<dt><a href=\"/Mobile/Cart.html\"><i class=\"ico_gon\"></i>继续购物</a> </dt>";
        o("MoneyDetail").innerHTML = content;
        o("totalmoney").innerHTML = "<b class=\"red\">￥" + parseFloat(productMoney + shippingMoney - pointMoney - couponMoney - favorableMoney).toFixed(2) + "";
    }
    else {
        app.jMsg("金额有错误", 500);
    }

}
//=============金额检查
function checkMoney(moneyLeft, fillMoney, obj) {
    if (!Validate.isNumber(fillMoney) || fillMoney < 0 || fillMoney > moneyLeft) {
        app.jMsg("请填写正确金额", 500);
        obj.value = 0;
    }
    else {
        o("Balance").value = obj.value;
        showMoneyDetail();
    }
}
//索要发票
function needInvoice(checked) {
    if (checked) {
        o("InvoiceDiv").style.display = "";
    }
    else {
        o("InvoiceDiv").style.display = "none";
    }
}
//填写优惠券
function fillUserCoupon(checked) {
    if (checked) {
        o("UserCouponDiv").style.display = "";
    }
    else {
        o("UserCouponDiv").style.display = "none";
    }
}
//添加优惠券
function addUserCoupon() {
    var number =$("#Number").val();
    var password = $("#Password").val();
    if (number != "" && password != "") {
        var url = "/Ajax.html?Action=CheckUserCoupon&Number=" + number + "&Password=" + password + "&totalProductMoney=" + $("#ProductMoney").val();
        Ajax.requestURL(url, dealAddUserCoupon);
    }
    else {
        app.jMsg("优惠券编号和密码不能为空", 500);
    }
}
function dealAddUserCoupon(data) {
    if (data.indexOf("|") == -1) {
        app.jMsg(data, 500);
    }
    else {
        var dataArray = data.split("|");
        var number = $("#Number").val();
        var _value = dataArray[0] + "|" + dataArray[1];
        //o("UserCoupon").options.add(new Option("编号：" + number + "（" + dataArray[1] + " 元）", dataArray[0] + "|" + dataArray[1]));
        //o("UserCoupon").value = dataArray[0] + "|" + dataArray[1];
        var _html = "<dd><label><input type='radio' name='UserCoupon' value='"+_value+"' onclick='selectUserCoupon()'>编号：" + number + "（" + dataArray[1]+"元）</label></dd>"
        $("#couponRadio .radio").append(_html);
        $('#couponRadio .radio dd').bind('click', function () {
            if ($(this).find('input').prop('checked')) {
                $(this).addClass('checked').siblings().removeClass('checked');
            }
        })
        //设置选中当前添加的项
        //$("input[name='UserCoupon'][value='"+_value+"']").attr("checked", true);
        //selectUserCoupon();
    }
}
//选择优惠券
function selectUserCoupon() {
    var userCoupon = $("input[name='UserCoupon']:checked").val();
    o("CouponMoney").value = userCoupon.split("|")[1];
    showMoneyDetail();
}
/*计算商品优惠金额*/
function selectProductFavor() {
    var _prodcutfavorid = $("input[name='ProductFavorableActivity']:checked").val();
    $.ajax({
        url: '/CheckOut.aspx?Action=SelectProductFavor',
        type: 'GET',
        data: { favorId: _prodcutfavorid },
        success: function (result) {
            var arr = result.split('|');
            if (arr[0] == 'ok') {
                //计算优惠金额总额
                $('#productfavorableMoney').val(arr[1]);
                var _newFavorMoney = parseFloat($('#orderfavorableMoney').val()) + parseFloat($('#productfavorableMoney').val());

                $("#FavorableMoney").val(_newFavorMoney.toFixed(2));
                showMoneyDetail();
                selectGifts();
            }
        }
    });
}
//读取商品优惠礼品列表
function selectGifts() {
    var _prodcutfavorid = $("input[name='ProductFavorableActivity']:checked").val();
    $.ajax({
        url: '/CheckOut.aspx?Action=ReadingGifts',
        type: 'GET',
        dataType: 'Json',
        data: { favorId: _prodcutfavorid },
        success: function (result) {
            if (result.count <= 0) {
                $("#giftlist .giftList").html("");
                $("#giftlist").hide();
            }
            else {
                var _html = "";
                for (var i in result.dataList) {
                    var gift = result.dataList[i];
                    if (i == 0) { _html += "<ul class='giftInfo'><li class='photo'><img src='" + gift.Photo + "' alt='" + gift.Name + "'/></li><li class='name'>" + gift.Name + "</li><li><input type='radio' name='GiftID' value='" + gift.Id + "' checked='checked'/></li></ul>"; }
                    else {
                        _html += "<ul class='giftInfo'><li class='photo'><img src='" + gift.Photo + "' alt='" + gift.Name + "'/></li><li class='name'>" + gift.Name + "</li><li><input type='radio' name='GiftID' value='" + gift.Id + "'/></li></ul>";
                    }
                }
                $("#giftlist .giftList").html(_html);
                $("#giftlist").show();
            }
        },
        error: function () {

        }
    });
}
//=============提交检查
function checkSubmit() {
    //var consignee = o("Consignee").value;
    //if (consignee == "") {
    //    o("Consignee").focus();
    //    app.jMsg("收货人姓名不能为空", 500);        
    //    return false;
    //}    
    //var tel = o("Tel").value;
    //var mobile = o("Mobile").value;
    //if (tel == "" && mobile == "") {
    //    app.jMsg("固定电话，手机必须得填写一个", 500);
    //    return false;
    //} else {
    //    if (tel != "") {
    //        if (!Validate.isTel(tel)) {
    //            app.jMsg("请输入正确的固定电话", 500);
    //            return false;
    //        }
    //    } else {
    //        if (!Validate.isMobile(mobile)) {
    //            app.jMsg("请输入正确的手机号码", 500);
    //            return false;
    //        }
    //    }
    //}
    //var address = o("Address").value;
    //if (address == "") {
    //    app.jMsg("地址不能为空", 500);
    //    return false;
    //}
    var userAddressID = $("#defaultAddrId").val();
    if (userAddressID <=0 || typeof (userAddressID) == "undefined") {
        app.jMsg("请选择收货地址", 500);
        return false;
    }
    
    var shippingID =$("input[name='ShippingID']:checked").val();
    if (shippingID == "" || typeof(shippingID) == "undefined") {
        app.jMsg("请选择配送方式", 500);
        return false;
    }
    
    var payType = $("input[name='Pay']:checked").val();
    if (payType == "" || typeof(payType) == "undefined") {
        app.jMsg("请选择付款方式", 500);
        return false;
    }
    console.log(payType);
    var productMoney = parseFloat(o("ProductMoney").value);
    var shippingMoney = parseFloat($('#ShippingMoney').val());
    if (isNaN(shippingMoney)) {
        shippingMoney = 0;
    }  
    var pointMoney = parseFloat($("#pointMoney").val());
    var couponMoney = parseFloat($("#CouponMoney").val());
    if ($("input[name='UserCoupon']:checked").length > 0) {
        var userCoupon = $("input[name='UserCoupon']:checked").val();
        var arr = userCoupon.split("|");
        couponMoney = parseFloat(arr[1]);
    }

    var _newFavorMoney = parseFloat($('#orderfavorableMoney').val()) + parseFloat($('#productfavorableMoney').val());
    var favorableMoney = _newFavorMoney.toFixed(2);
    if (productMoney + shippingMoney - pointMoney - couponMoney - favorableMoney<0) {  
        app.jMsg("金额有错误", 500);
        return false;
    }
   // alert("run here");
    return true;
}