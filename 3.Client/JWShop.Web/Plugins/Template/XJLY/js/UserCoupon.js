var objID = "NotUse";
var page = 1;
readUserCoupon(objID, page);
function readUserCoupon(id, page) {
    loading("UserCouponAjax", "优惠券");
    o(objID).className = "item";
    objID = id;
    var obj = o(objID);
    obj.className = "item cur";
    var url = "UserCouponAjax.html?Action=" + id + "&Page=" + page;
    Ajax.requestURL(url, dealReadUserCoupon);
}
function dealReadUserCoupon(content) {
    o("UserCouponAjax").innerHTML = content;
}
function goPage(page) {
    readUserCoupon(objID, page);
} 
//添加优惠劵
function addUserCoupon() {
    var number = o("Number").value;
    var password = o("Password").value;
    if (number != "" && number!= "") {
        var url = "UserCouponAjax.html?Action=AddUserCoupon&Number=" + number + "&Password=" + encodeURI(password);
        Ajax.requestURL(url, dealAddUserCoupon);
    }
    else {
        alertMessage("请填写卡号和密码");
    }
}
function dealAddUserCoupon(content){
    if (content != "") {
        alertMessage(content);
    }
    else {
        alertMessage("添加成功");
        o("Number").value = "";
        o("Password").value = "";
    }
}