var content = "输入商品关键字";
//o("keyWord").value = content;
function clearKeyWord() {
    if (o("keyWord").value == content) {
        o("keyWord").value = "";
    }
}
function fillKeyWord() {
    if (o("keyWord").value == "") {
        o("keyWord").value = content;
    }
}
function topSearchProduct() {
    var keyWord = o("searchTxt").value;
    if (keyWord == "竞网科技旗下商城系统！") {
        alert("请输入商品关键字");
        return;
    }
    var url = "/List.html?kw=" + encodeURIComponent(keyWord) + "";
    window.location = url;
}

//全选全不选
function selectAll(obj) {
    var objs = os("name", "SelectID");
    if (objs != null && objs.length > 0) {
        for (var i = 0; i < objs.length; i++) {
            objs[i].checked = obj.checked;
        }
    }
}
//检查是否选择
function checkSelect() {
    var objs = os("name", "SelectID");
    var result = getCheckboxValue(objs);
    if (result.length == 0) {
        alert("请选择要操作的记录");
        return false;
    }
    else {
        if (confirm("注意：请问确定要操作吗")) {
            return true;
        }
    }
    return false;
}
//确认检查
function check() {
    if (confirm("注意：请问确定要操作吗")) {
        return true;
    }
    return false;
}