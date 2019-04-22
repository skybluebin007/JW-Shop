function changeShippingWay() {
    var objs = os("name", "ShippingWay");
    var regionObj = o("ShippingRegionDiv");
    var value = getRadioValue(objs);
    if (value == "0") {
        regionObj.style.display = "none";
    }
    else {
        regionObj.style.display = "";
    }
}
function changeReduceWay() {
    var objs = os("name", "ReduceWay");
    var reduceMoneyObj = o("ReduceMoneyDiv");
    var reduceDiscountObj = o("ReduceDiscountDiv");
    var value = getRadioValue(objs);
    if (value == "0") {
        reduceMoneyObj.style.display = "none";
        reduceDiscountObj.style.display = "none";
    }
    else if (value == "1") {
        reduceMoneyObj.style.display = "";
        reduceDiscountObj.style.display = "none";
    }
    else {
        reduceMoneyObj.style.display = "none";
        reduceDiscountObj.style.display = "";
    }
}
function changeGiftWay() {
    var objs = os("name", "GiftWay");
    var giftObj = o("GiftDiv");
    var value = getRadioValue(objs);
    if (value == "0") {
        giftObj.style.display = "none";
    }
    else {
        giftObj.style.display = "";
    }
}

function searchGift() {
    var name = $("#GiftName").val();
    var index = layer.load(0, { time: 10 * 1000 });
    $.ajax({
        url: '/Admin/Ajax.aspx?Action=SearchGift&Name=' + name,
        type: 'GET',
        success: function (data) {
            layer.close(index);
            var result = "";
            if (data != "") {
                var giftArray = data.split("#");
                for (var i = 0; i < giftArray.length; i++) {
                    var tempArray = giftArray[i].split("|");
                    result += "<span><input type=\"checkbox\" value=\"" + giftArray[i] + "\" onclick=\"selectGift(this)\"> " + substring(tempArray[1], 10) + " </span>";
                }
            }
            $('#SearchGiftList').html(result);
        }
    });
}

function selectGift(obj) {
    var giftArray = obj.value.split("|");
    if (obj.checked) {
        if (o("Gift" + giftArray[0]) == null) {
            var content = "<span id=\"Gift" + giftArray[0] + "\">" + substring(giftArray[1], 8) + "<span onclick=\"deleteGift(" + giftArray[0] + ")\" style=\"cursor:pointer;display: inline-block;vertical-align: middle;line-height: 10px;\"><img src=\"static/images/ico-delete.png\" title=\"删除\" /></span><input class=\"input\" name=\"GiftList\" type=\"hidden\" value=\"" + giftArray[0] + "\"/></span>";
            $('#SelectGiftList').append(content);
        }
        else {
            layer.msg("已存在该礼品");
        }
    }
    else {
        try {
            deleteGift(giftArray[0]);
        } catch (e) { }
    }
}
function deleteGift(id) {
    var obj = o("Gift" + id);
    obj.parentNode.removeChild(obj);
}