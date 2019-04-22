//=======初始化变量
var objID = "Add";
var page = 1;
$(function () {
    readUserApply(objID, page);
});
function readUserApply(id, page) {
    loading("UserApplyAjax", "提现申请");
    o(objID).className = "item";
    objID = id;
    var obj = o(objID);
    obj.className = "item cur";
    var url = "UserApplyAjax.html?Action=" + id + "&Page=" + page;
    Ajax.requestURL(url, dealReadUserApply);
}
function dealReadUserApply(content) {
    o("UserApplyAjax").innerHTML = content;
}
function goPage(page) {
    readUserApply(objID, page);
}
//添加提现申请
function addUserApply() {
    var money = o("Money").value;
    var userNote = o("UserNote").value;
    if (money != "" && userNote != "") {
        var url = "UserApplyAjax.html?Action=AddUserApply&Money=" + money + "&UserNote=" + encodeURI(userNote);
        Ajax.requestURL(url, dealAddUserApply);
    }
    else {
        alertMessage("请填写金额和备注");
    }
}
function dealAddUserApply(content) {
    if (content != "") {
        alertMessage(content);
    }
    else {
        alertMessage("申请提交成功，后台将进行审核！");
        o("Money").value = "";
        o("UserNote").value = "";
    }
}