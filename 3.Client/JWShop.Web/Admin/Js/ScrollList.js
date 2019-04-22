document.write("<div id=\"notice\" style=\"	height:30px;line-height:30px;overflow:hidden; width:50%; float:left; text-align:left;margin-left:5px\">");
document.write("<a href=\"http://www.yyjing.com\" target=\"_blank\"  style=\"text-decoration:none\">湖南竞网，专业的网站建设，设计与策划。</a><br/>");
document.write("<a href=\"http://xb.yyjing.net/login.asp\" target=\"_blank\"  style=\"text-decoration:none\">网站模板与案例欣赏。</a><br/>");
document.write("<a href=\"#\" target=\"_blank\"  style=\"text-decoration:none\">热烈欢迎湖南竞网科技有限公司投票系统系统上线！</a><br/>");
document.write("</div>");
window.onload = function () {
    var scrollup = new ScrollText("notice");
    scrollup.LineHeight = 30;
    scrollup.Amount = 1;
    scrollup.Start();
}