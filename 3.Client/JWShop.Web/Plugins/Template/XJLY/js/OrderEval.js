//============初始化以及变量
$(function () {
    loadstar();
});

//============星级选择
function loadstar() {
    // 选择星级
    var starEt = $(".starEt");
    starEt.children("span").hover(
		function () {
		    $(this).addClass("hover").siblings("span").removeClass("hover");
		},
		function () {
		    starEt.children("span").removeClass("hover");
		}
	).click(function () {
	    $(this).addClass("cur").siblings("span").removeClass("cur");
	    $(this).siblings(".startValue").val($(this).index() + 1);
	    starEt.find("input[type='hidden']").val($(this).index() + 1);
	});
}
//================提交评论
function submitCom() {
    var value = "";
    $("#PLList").find("dd").each(function () {
        var pid = $(this).attr("pid");
        if ($("#content_" + pid).val().indexOf('×') > 0 || $("#content_" + pid).val().indexOf('√') > 0) {
            alertMessage('评价内容不能包含特殊符号~');
            return;
        }
        if ($("#content_" + pid).val() == "") {
            alertMessage('评价内容不能为空~');
            return;
        }
        value += pid + "×" + $("#hdstar_" + pid).val() + "×" + $("#content_" + pid).val() + "√";
    });
    var url = "/User/OrderAjax.html?Action=SubmitCom&Value=" + value;
    Ajax.requestURL(url, submitOK);
}
function submitOK(data) {
    if (data == "ok") {
        alertMessage('评价成功~');
        setTimeout(function () {
            window.location.href = "/User/UserProductComment.html";
        }, 1000);
    }
    else {
        alertMessage(data);
    }
}