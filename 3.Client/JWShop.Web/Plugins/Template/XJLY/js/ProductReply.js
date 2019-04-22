//==初始
$(function () {
    readProductReply();
});
//=====读取评论回复
function readProductReply() {
    loading("ProductReplyAjax", "产品回复");
    var url = "/ProductReplyAjax.html?CommentID=" + commentID;
    Ajax.requestURL(url, dealSearchProductReply);
}
function dealSearchProductReply(content) {
    o("ProductReplyAjax").innerHTML = content
}
//=====提交回复
function postReply() {
    var content = o("content").value;
    if (content == "") {
        alertMessage("请输入内容");
        return false;
    }
    var url = "/ProductReplyAjax.html?Action=Add&ProductID=" + productID + "&CommentID=" + commentID + "&Content=" + content;
    Ajax.requestURL(url, dealPostReply);
}
function dealPostReply(content) {
    if (content == "ok") {
        alertMessage("回复成功");
        tempPage = 1;
        readProductReply();
    }
    else {
        alertMessage(content);
    }
}