var objID = "Point";
var page = 1;
readUserAccountRecord(objID, page);
function readUserAccountRecord(id, page) {
    loading("UserAccountRecordAjax", "积分");
    
    var url = "/Mobile/User/UserAccountRecordAjax.html?Action=" + id + "&Page=" + page;
    Ajax.requestURL(url, dealReadUserAccountRecord);
}
function dealReadUserAccountRecord(content) {
    o("UserAccountRecordAjax").innerHTML = content;
}
function goPage(page) {
    readUserAccountRecord(objID, page);
} 