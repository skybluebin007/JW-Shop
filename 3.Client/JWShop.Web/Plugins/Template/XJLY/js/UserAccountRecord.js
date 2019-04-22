var objID = "Point";
var page = 1;
readUserAccountRecord(objID, page);
function readUserAccountRecord(id, page) {
    loading("UserAccountRecordAjax", "优惠券");
    o(objID).className = "item";
    objID = id;
    var obj = o(objID);
    obj.className = "item cur";
    var url = "UserAccountRecordAjax.html?Action=" + id + "&Page=" + page;
    Ajax.requestURL(url, dealReadUserAccountRecord);
}
function dealReadUserAccountRecord(content) {
    o("UserAccountRecordAjax").innerHTML = content;
}
function goPage(page) {
    readUserAccountRecord(objID, page);
} 