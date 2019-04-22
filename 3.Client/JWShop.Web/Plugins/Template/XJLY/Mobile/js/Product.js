var tempPage = 1;

var hasContent = true;
var canScroll = true;
function scrollProduct() {
    if (hasContent) {
        var paras = window.location.search;
        paras = paras.substr(1, paras.length - 1);
        if (paras.toLowerCase().indexOf("&page=") >= 0) {
            var paraArr = window.location.search.split("&");
            for (i = 0; i < paraArr.length; i++) {
                var atvalueArr = paraArr[i].split("=");
                if (atvalueArr[0].toLowerCase() == "page") paras = paras.replace(paraArr[i], "");
            }
        }
        var url = "/Mobile/ProductAjax.html?" + paras + "&Page=" + tempPage;;
        Ajax.requestURL(url, dealScrollProduct);
    }
}
function dealScrollProduct(content) {
    if (parseInt(content.substr(content.lastIndexOf("#") + 1)) > 0) {
        o("ProductAjax").innerHTML += content.substr(0, content.lastIndexOf("#"));
    } else {
        hasContent = false;
        if (o("ProductAjax").innerHTML.indexOf("没有更多") < 0) {
            o("ProductAjax").innerHTML += "<div ig-load>没有更多了哦。</div>";
        }
    }
    canScroll = true;
}

$(function () {
    $(window).scroll(function () {
        if (canScroll) {
            if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
                tempPage++;
                canScroll = false;
                scrollProduct();
            }
        }
    });
});
