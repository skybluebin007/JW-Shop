//修改显示
$("table.product-list-img .penIcon").click(function () {
	$(".tc").hide();
    var _pid = $(this).attr("_pid");
    console.log("pid:"+_pid);
    var _modifyType = $(this).attr("_modifyType");
    //修改价格
    if (_modifyType == "price") {
        var _standType = $("#updatePrice" + _pid).attr("_standType");
        var _salePrice = $("#updatePrice" + _pid).attr("_salePrice");
        $("#updatePrice" + _pid).html("");
        var _html = "<li class=\"first\"><label>一口价：</label><input type=\"text\" value='" + _salePrice + "' id='pprice" + _pid + "'  onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this)\"/></li>";
        if (_standType == 1) {
            $.ajax({
                url: "Ajax.aspx",
                type: "get",
                data: { Action: "GetProductStandardRecord", productId: _pid, standardType: _standType },
                dataType: "json",
                success: function (data) {
                    //console.log(data.count);
                    if (data.count > 0) {
                        for (var i in data.dataList) {
                            var item = data.dataList[i];
                            _html += "<li><label>" + item.ValueList + "：</label><input type='text' value='" + item.SalePrice + "' name='standardPrice" + _pid + "' _valueList='" + item.ValueList + "' _standIdList='" + item.StandardIdList + "' onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this)\"/></li>";
                        }
                        if (data.count > 5) {
                            _html += "<li><label><a href='ProductAdd.aspx?ID=" + _pid + " '>修改更多价格：</a></label><span class='pbtn' onclick=\"pbtnClick(this)\">保存</span></li>";
                        }
                        else {
                            _html += "<li><label></label><span class='pbtn' onclick=\"pbtnClick(this)\">保存</span></li>";
                        }
                    }
                    else {
                        console.log(data.count);
                        _html += "<li><span class='pbtn' onclick=\"pbtnClick(this)\">保存</span></li>";
                        //console.log(_html);
                    }
                    $("#updatePrice" + _pid).append(_html);
                },
                error: function () { alertMessage("系统忙，请稍后重试"); }
            })

        }
        else {
            _html += "<li><label></label><span class='pbtn' onclick=\"pbtnClick(this)\">保存</span></li>";
            $("#updatePrice" + _pid).append(_html);
        }
    }
    //修改库存
    if (_modifyType == "storage") {
        console.log("#updateStorage" + _pid);
        var _standType = $("#updateStorage" + _pid).attr("_standType");
        var _totalStorage = $("#updateStorage" + _pid).attr("_totalStorage");
        console.log("_totalStorage:" + _totalStorage);
        $("#updateStorage" + _pid).html("");
        var _html = "";
        if (_standType == 1) {
            _html = "<li class=\"first\"><label>总库存：</label><input type=\"text\" value='" + _totalStorage + "' id='pstorage" + _pid + "' disabled/></li>";
            $.ajax({
                url: "Ajax.aspx",
                type: "get",
                data: { Action: "GetProductStandardRecord", productId: _pid, standardType: _standType },
                dataType: "json",
                success: function (data) {
                    //console.log(data.count);
                    if (data.count > 0) {
                        for (var i in data.dataList) {
                            var item = data.dataList[i];
                            _html += "<li><label>" + item.ValueList + "：</label><input type='text' value='" + item.Storage + "' name='standardStorage" + _pid + "' _valueList='" + item.ValueList + "' _standIdList='" + item.StandardIdList + "' onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this)\"/></li>";
                        }
                        if (data.count > 5) {
                            _html += "<li><label><a href='ProductAdd.aspx?ID=" + _pid + " '>修改更多库存：</a></label><span class='pbtn' onclick=\"pbtnClick(this)\">保存</span></li>";
                        }
                        else {
                            _html += "<li><label></label><span class='pbtn' onclick=\"pbtnClick(this)\">保存</span></li>";
                        }
                    }
                    else {
                        console.log(data.count);
                        _html += "<li><span class='pbtn' onclick=\"pbtnClick(this)\">保存</span></li>";
                        //console.log(_html);
                    }
                    $("#updateStorage" + _pid).append(_html);
                },
                error: function () { alertMessage("系统忙，请稍后重试"); }
            })

        }
        else {
            _html = "<li class=\"first\"><label>总库存：</label><input type=\"text\" value='" + _totalStorage + "' id='pstorage" + _pid + "'  onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this)\"/></li>";
            _html += "<li><label></label><span class='pbtn' onclick=\"pbtnClick(this)\">保存</span></li>";
            $("#updateStorage" + _pid).append(_html);
        }
    }

    // 修改区域显示
    if ($(this).attr("_standType") == 1) { $(this).siblings(".tc").removeClass("tc-d"); }
    else { $(this).siblings(".tc").addClass("tc-d"); }	
    $(this).siblings(".tc").show();
});
$(".tc_close").click(function () {
    $(this).parent().parent().find(".tc").hide();
});
$(".pbtn").click(function () {
    var htt = $(this).parent().parent().find(".first").find("input").val();
    $(this).parent().parent().parent().parent().find(".changeval").html(htt);
    $(this).parent().parent().parent().parent().find(".tc").hide();
});
//价格、库存修改后保存
function pbtnClick(obj) {
    var _modifyType = $(obj).parent().parent().parent().parent().find(".penIcon").attr("_modifyType");
    //console.log(_modifyType);
    //修改库存
    if (_modifyType == "storage") {
        var _standType = $(obj).parent().parent().attr("_standType");
        var _pid = $(obj).parent().parent().attr("_pid");
        console.log(_pid);
        if (isNaN($("#pstorage" + _pid).val()) || parseFloat($("#pstorage" + _pid).val()) < 0 || $("#pstorage" + _pid).val() == "") {
            $("#pstorage" + _pid).focus();
            alertMessage("总库存填写不规范");
            return false;
        }
        var _totalStorage = $("#pstorage" + _pid).val();
        var _standIdList = "", _valueList = "", _storageList = "",_ts=0;
        var checkstandprice = true;
        $("input[name='standardStorage" + _pid + "']").each(function () {
            console.log($(this).val());
            if ($(this).val() == "" || isNaN($(this).val()) || parseFloat($(this).val()) < 0) {
                $(this).focus();
                alertMessage("规格库存填写不规范");
                checkstandprice = false;
            }
            else {
                _standIdList += $(this).attr("_standIdList") + ",";
                _valueList += encodeURIComponent($(this).attr("_valueList")) + ",";
                _storageList += $(this).val() + ",";
                _ts += parseInt($(this).val());
            }
        })
        //console.log(checkstandprice);
        if (checkstandprice == true) {
            //console.log("_standIdList:" + _standIdList);
            //console.log("_valueList:" + _valueList);
            //console.log("_storageList:" + _storageList);
            if (_standType==1) $("#pstorage" + _pid).val(_ts);
            $.ajax({
                url: "Product.aspx?ActionProduct=ModifyStorage",
                type: "get",
                data: { productId: _pid, totalStorage: _totalStorage, valueList: _valueList, standIdList: _standIdList, storageList: _storageList },
                dataType: "json",
                success: function (data) {
                    if (data.flag == true) {
                        alertMessage("修改成功");                     
                        $("#pstorage" + _pid).val(data.msg);                    
                        var htt = $(obj).parent().parent().find(".first").find("input").val();
                        $("#updateStorage" + _pid).attr("_totalStorage", htt);
                        //剩余库存
                        //console.log(parseInt($("#orderCount" + _pid).html()));
                        var _leftStorage = parseInt(htt) - parseInt($("#orderCount" + _pid).html());
                       
                     $(obj).parent().parent().parent().parent().find(".changeval").html(_leftStorage);
                        $(obj).parent().parent().parent().parent().find(".tc").hide();
                    }
                    else {
                        alertMessage(data.msg);
                        return false;
                    }
                },
                error: function () { alertMessage("系统忙，请稍后重试"); }
            })

        }

    }
    //修改价格
    if (_modifyType == "price") {
        var _standType = $(obj).parent().parent().attr("_standType");
        var _pid = $(obj).parent().parent().attr("_pid");
        console.log(_pid);
        if (isNaN($("#pprice" + _pid).val()) || parseFloat($("#pprice" + _pid).val()) < 0 || $("#pprice" + _pid).val() == "") {
            $("#pprice" + _pid).focus();
            alertMessage("一口价填写不规范");
            return false;
        }
        var _salePrice = $("#pprice" + _pid).val();
        var _standIdList = "", _valueList = "", _priceList = "";
        var checkstandprice = true;
        $("input[name='standardPrice" + _pid + "']").each(function () {
            console.log($(this).val());
            if ($(this).val() == "" || isNaN($(this).val()) || parseFloat($(this).val()) < 0) {
                $(this).focus();
                alertMessage("价格填写不规范");
                checkstandprice = false;
            }
            else {
                _standIdList += $(this).attr("_standIdList") + ",";
                _valueList += encodeURIComponent($(this).attr("_valueList")) + ",";
                _priceList += $(this).val() + ",";
            }
        })
        //console.log(checkstandprice);
        if (checkstandprice == true) {
            //console.log("_standIdList:" + _standIdList);
            //console.log("_valueList:" + _valueList);
            //console.log("_priceList:" + _priceList);
            $.ajax({
                url: "Product.aspx?ActionProduct=ModifyPrice",
                type: "get",
                data: { productId: _pid, salePrice: _salePrice, valueList: _valueList, standIdList: _standIdList, priceList: _priceList },
                dataType: "json",
                success: function (data) {
                    if (data.flag == true) {
                        alertMessage("修改成功");
                        $("#pprice" + _pid).val(data.msg);                       
                        var htt = $(obj).parent().parent().find(".first").find("input").val();
                        $("#updatePrice" + _pid).attr("_salePrice", htt);
                        $(obj).parent().parent().parent().parent().find(".changeval").html(htt);
                        $(obj).parent().parent().parent().parent().find(".tc").hide();
                    }
                    else {
                        alertMessage(data.msg);
                        return false;
                    }
                },
                error: function () { alertMessage("系统忙，请稍后重试"); }
            })

        }
    }
}
//修改商品名称
function UpdateProductName(obj) {
    var _pid = $(obj).attr("_pid");
    var _pname = $("#pname" + _pid).val();
    if (_pname == "") {
        alertMessage("请输入商品名称");
        $("#pname" + _pid).focus();
        return false;
    }
    $.ajax({
        url: "Ajax.aspx",
        type: "get",
        data: { Action: "UpdateProductName", ProductId: _pid, ProductName: encodeURIComponent(_pname) },
        dataType: "text",
        success: function (data) {
            if (data == "ok") {
                alertMessage("更新成功");
                UpdateSuccess(obj);
            }
            else {
                if (data.indexOf("|") > 0) {
                    alertMessage(data.split("|")[1]);
                }
                else {
                    alertMessage("系统忙，请稍后重试");
                }
                return false;
            }
        },
        error: function () {
            alertMessage("系统忙，请稍后重试");
            return false;
        }

    })
}
//修改成功后的调用方法
function UpdateSuccess(obj) {
    $(obj).parent(".tc").siblings(".penIcon").show();
    $(obj).parent(".tc").siblings("a,span").show();
    $(obj).parent(".tc").hide();
    var txt = $(obj).siblings("input,textarea").val();
    $(obj).parent(".tc").siblings("a,span").text(txt);
}

//按销量排序
function OrderByOrderCount() {
    if (_queryOrderType == "" || _queryOrderType == "Asc" || _productOrderType != "OrderCount") { _orderType = "Desc"; }
    if (_queryOrderType == "Desc") { _orderType = "Asc"; }

    _productOrderType = "OrderCount";

    if (_url.indexOf("?") < 0) { window.location.href = _url + "?Page=1&ProductOrderType=" + _productOrderType + "&OrderType=" + _orderType; }
    else {
        var _url1 = _url.split("?");
        var _tmp = _url1[1];
        var _urlParams = _url1[1].split("&");
        for (var i = 0; i < _urlParams.length; i++) {
            if (_urlParams[i].indexOf("Page=") >= 0) _tmp = _tmp.replace("&" + _urlParams[i], "").replace(_urlParams[i], "");
            if (_urlParams[i].indexOf("ProductOrderType=") >= 0) _tmp = _tmp.replace("&" + _urlParams[i], "").replace(_urlParams[i], "");
            if (_urlParams[i].indexOf("OrderType=") >= 0) _tmp = _tmp.replace("&" + _urlParams[i], "").replace(_urlParams[i], "");

        }
        _gotourl = _url1[0] + "?" + _tmp + "&Page=1&ProductOrderType=" + _productOrderType + "&OrderType=" + _orderType;
        window.location.href = _gotourl;

    }
}
//按发布时间排序
function OrderByAddDate() {
    if (_queryOrderType == "" || _queryOrderType == "Asc" || _productOrderType != "AddDate") { _orderType = "Desc"; }
    if (_queryOrderType == "Desc") { _orderType = "Asc"; }
    _productOrderType = "AddDate";

    if (_url.indexOf("?") < 0) { window.location.href = _url + "?Page=1&ProductOrderType=" + _productOrderType + "&OrderType=" + _orderType; }
    else {
        var _url1 = _url.split("?");
        var _tmp = _url1[1];
        var _urlParams = _url1[1].split("&");
        for (var i = 0; i < _urlParams.length; i++) {
            if (_urlParams[i].indexOf("Page=") >= 0) _tmp = _tmp.replace("&" + _urlParams[i], "").replace(_urlParams[i], "");
            if (_urlParams[i].indexOf("ProductOrderType=") >= 0) _tmp = _tmp.replace("&" + _urlParams[i], "").replace(_urlParams[i], "");
            if (_urlParams[i].indexOf("OrderType=") >= 0) _tmp = _tmp.replace("&" + _urlParams[i], "").replace(_urlParams[i], "");

        }
        _gotourl = _url1[0] + "?" + _tmp + "&Page=1&ProductOrderType=" + _productOrderType + "&OrderType=" + _orderType;
        window.location.href = _gotourl;
    }
}

//价格只能输入正数及两位小数    
function clearNoNum(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符   
    obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的   
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数   
    if (obj.value.indexOf(".") < 0 && obj.value != "") {//以上已经过滤，此处控制的是如果没有小数点，首位不能为类似于 01、02的金额  
        obj.value = parseFloat(obj.value);
    }
}