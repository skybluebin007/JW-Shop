function addAttribute() {    
    var name = $("#ctl00_ContentPlaceHolder_AttrName").val();
    var inputType = 5;
    if ($("#InputType").attr("checked")) inputType = 6;
    var inputValue = $("#ctl00_ContentPlaceHolder_InputValue").val();
    if (getStringLength(name) <= 0 || getStringLength(inputValue) <= 0) {
        alert("属性有误，请设置名称和属性值")
    } else {
        var adnum = $("#attributeBox").children.length + 1;
        var oldattrid = $("#tr" + $("#hattrid").val() + "").find("input[name='IdList']").val();
        if (typeof (oldattrid) == "undefined") oldattrid = "0";

        if ($("#hattrid").val() != "") {
            $("#tr" + $("#hattrid").val() + "").remove();
            $("#hattrid").val("");
        }
        var msgstr = "<tr id=\"tr" + adnum + "\"><td height=\"60\"><input type=\"text\" name=\"NameList\" value=\"" + name + "\" class=\"ig-txt\" maxlength=\"20\"/></td><td >" + getSectionType(inputType) + "</td><td ><dl class=\"ig-group-del clearfix\">" + getSectionList(inputValue) + "<input type=\"hidden\" name=\"ValueList\" value=\"" + inputValue.replace(/,/g, '|') + "\"></dl></td><td ><input type=\"hidden\" name=\"IdList\" value=\"" + oldattrid + "\" /><a href=\"javascript:editAttr(" + adnum + ",'" + name + "'," + inputType + ",'" + inputValue + "')\" class=\"ig-colink\" title=\"编辑\">编辑</a><a href=\"javascript:void(0)\" onclick=\"deleteAttr(this)\" atid=\"" + oldattrid + "\" class=\"ig-colink\" title=\"删除\">删除</a></td></tr>";

        $("#attributeBox").append(msgstr);
        $("#ctl00_ContentPlaceHolder_AttrName").val("");
        $("#InputType").removeAttr("checked");
        $("#InputType").parent().removeClass("checked");
        $("#ctl00_ContentPlaceHolder_InputValue").val("");
		$(".type-ads1").hide();
		$("#attributeBox").parent("table").show();
    }
}
function getSectionType(sType) {
    switch (sType) {
        case 5:
            return "单选<input type=\"hidden\" name=\"TypeList\" value=\"5\">";
            break;
        case 6:
            return "多选<input type=\"hidden\" name=\"TypeList\" value=\"6\">";
            break;
        default:
            return "单选<input type=\"hidden\" name=\"TypeList\" value=\"5\">";
            break;
    }
}
function editAttr(id, name, stype, svalue) {
	
    $("#hattrid").val(id);
    $("#ctl00_ContentPlaceHolder_AttrName").val(name);
    if (stype == 6) {
        $("#InputType").attr("checked", "checked");
        $("#InputType").parent().addClass("checked");
    } else {
        $("#InputType").removeAttr("checked");
        $("#InputType").parent().removeClass("checked");
    }
    $("#ctl00_ContentPlaceHolder_InputValue").val(svalue);   
    $(".type-ads1").show();
    $(".type-ads1 .title").text("修改属性");
}

function deleteAttr(o) {	
    $(o).parent().parent().remove();
    if(typeof($(o).attr("atid"))!="undefined") {
        var atid = $(o).attr("atid");
        if (atid > 0) {
            $.get("/Admin/Ajax.aspx", { action: "DeleteAttribute", atid: atid }, function (data) {
            });
        }
    }

    if (typeof ($(o).attr("skuid")) != "undefined") {
        var skuid = $(o).attr("skuid");
        if (skuid > 0) {
            $.get("/Admin/Ajax.aspx", { action: "DeleteStandard", skuid: skuid }, function (data) {
            });
        }
    }

}
function getSectionList(listStr) {
    var sectoinStr="";
    sectionArr = listStr.split(";");
    for (i = 0; i < sectionArr.length; i++) {
        if (sectionArr[i] != "")
            sectoinStr += "<dd class=\"item\">" + sectionArr[i] + "</dd>";
    }
    return sectoinStr;
}




function addStandard() {   

    var name = $("#ctl00_ContentPlaceHolder_StandardName").val();
    var inputType = 5;
    if ($("#StandardType").attr("checked")) inputType = 6;
    var inputValue = $("#ctl00_ContentPlaceHolder_StandardValue").val();
    if (getStringLength(name) <= 0 || getStringLength(inputValue) <= 0) {
        alert("规格有误，请设置名称和规格值")
    } else {
        var oldstandardid = $("#trs" + $("#hstandid").val() + "").find("input[name='SIdList']").val();
        if (typeof (oldstandardid) == "undefined") oldstandardid = "0";

        if ($("#hstandid").val() != "") {
            $("#trs" + $("#hstandid").val() + "").remove();//清除现有行
            $("#hstandid").val("");
        }        
        var standCount = $("input[name='SNameList']").length;
        if (standCount <= 2) {
            var adnum = $("#standardBox").children.length+1;            
            var msgstr = "<tr id=\"trs" + adnum + "\"><td height=\"60\"><input type=\"text\" name=\"SNameList\" value=\"" + name + "\" class=\"ig-txt\" maxlength=\"20\" /></td><td style=\"display:none;\">" + getSectionTypeS(inputType) + "</td><td ><dl class=\"ig-group-del clearfix\">" + getSectionList(inputValue) + "<input type=\"hidden\" name=\"SValueList\" value=\"" + inputValue.replace(/,/g, '|') + "\"></dl></td><td ><input type=\"hidden\" name=\"SIdList\" value=\"" + oldstandardid + "\" /><a href=\"javascript:editStand(" + adnum + ",'" + name + "'," + inputType + ",'" + inputValue + "')\" class=\"ig-colink\" title=\"编辑\">编辑</a><a href=\"javascript:void(0)\" onclick=\"deleteAttr(this)\" skuid=\"0\" class=\"ig-colink\" title=\"删除\">删除</a></td></tr>";
            $("#standardBox").append(msgstr);

            $("#ctl00_ContentPlaceHolder_StandardName").val("");
            $("#ctl00_ContentPlaceHolder_StandardValue").val("");
        } else {
            alert("为了您的数据关系明确，最多允许3个规格");
        }
        $(".type-ads2").hide();
        $("#standardBox").parent("table").show();
    }
}

function getSectionTypeS(sType) {
    switch (sType) {
        case 5:
            return "文字<input type=\"hidden\" name=\"STypeList\" value=\"1\">";
            break;
        case 6:
            return "图片<input type=\"hidden\" name=\"STypeList\" value=\"2\">";
            break;
        default:
            return "文字<input type=\"hidden\" name=\"STypeList\" value=\"1\">";
            break;
    }
}
function editStand(id, name, stype, svalue) {
    $("#hstandid").val(id);
    $(".type-ads2 .title").text("修改规格");
    $("#ctl00_ContentPlaceHolder_StandardName").val(name);
    if (stype == 6) {
        $("#StandardType").attr("checked", "checked");
        $("#StandardType").parent().addClass("checked");
    } else {
        $("#StandardType").removeAttr("checked");
        $("#StandardType").parent().removeClass("checked");
    }
    $("#ctl00_ContentPlaceHolder_StandardValue").val(svalue);
    $(".type-ads2").show();
}

//function addAttribute2(productTypeID) {
//    var name = $("#Name").val();
//    var inputType = $("#InputType").val();
//    var inputValue = $("#InputValue").val();
//    var orderID = $("#OrderID").val();
//    $.post("Ajax.aspx", { "Action": "AddProductTypeAttribute", "Name": name, "AttributeClassID": productTypeID, "InputType": inputType, "InputValue": inputValue, "OrderID": orderID }, function (data) {
//        if (data == "ok") {
//            loadAttribute(productTypeID);
//        }
//    })
//}

