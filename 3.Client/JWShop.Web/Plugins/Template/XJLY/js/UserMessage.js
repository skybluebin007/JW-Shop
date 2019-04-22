var page = 1;
readUserMessage(page);
function readUserMessage( page) {
    loading("UserMessageAjax", "留言");
    var url = "UserMessageAjax.html?Action=Read&Page=" + page;
    Ajax.requestURL(url, dealReadUserMessage);
}
function dealReadUserMessage(content) {
    o("UserMessageAjax").innerHTML = content;
}
function goPage(page) {
    readUserMessage(page);
} 
//添加留言
function addUserMessage() {
    var messageClass=getRadioValue(os("name","MessageClass"));
    var title = o("Title").value;
    var content = o("Content").value;
    if (title != "" && content != "" && title != "请输入标题" && content != "请输入内容") {
        var url = "UserMessageAjax.html?Action=AddUserMessage&MessageClass="+messageClass+"&Title=" + encodeURI(title) + "&Content=" + encodeURI(content);
        Ajax.requestURL(url, dealAddUserMessage);
    }
    else {
        alertMessage("请填写标题和内容");
    }
}
function dealAddUserMessage(content){
    if (content != "") {
        alertMessage(content);
    }
    else {
        alertMessage("添加成功");
        o("Title").value = "";
        o("Content").value = "";
        readUserMessage(page);
    }
}