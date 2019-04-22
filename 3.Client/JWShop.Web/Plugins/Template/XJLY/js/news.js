$(function () {

    readNews($("#classId").val());
});
var _id = $("#classId").val();
var tempPage = 1;
//读取购物车
function readNews(id) {
    _id = id;
    loading("NewsList", "新闻中心");
    Ajax.requestURL("NewsAjax.html?ID=" + _id + "&Page=" + tempPage, dealReadNews)
}
function dealReadNews(data) {
    o("NewsList").innerHTML = data;
}
//分页
function goPage(page) {
    tempPage = page;
    readNews(_id);
}
