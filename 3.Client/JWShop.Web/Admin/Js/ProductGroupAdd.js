//增加一条商品组
function addProductGroup(photo, link, photoMobile, linkMobile, idList, nameList, photoList, themeActivityID) {
    var countObj = o("ProductGroupCount");
    var parentObj = o("ProductGroup");
    var span = document.createElement("span");
    with (span) {
        id = "ProductGroup" + countObj.value;
        className = "themeActivityBlock";
    }
    parentObj.appendChild(span);
    span.innerHTML = readContent(photo, link, photoMobile, linkMobile, idList, nameList, photoList, countObj.value, themeActivityID);
    o("ProductGroupCount").value = 1 + parseInt(countObj.value);
}
//修改一条商品组
function updateProductGroup(photo, link, photoMobile, linkMobile, idList, nameList, photoList, id, themeActivityID) {
    var span = o("ProductGroup" + id);
    span.className = "themeActivityBlock";
    span.innerHTML = readContent(photo, link, photoMobile, linkMobile, idList, nameList, photoList, id, themeActivityID);
}
//删除一条商品组
function deleteProductGroup(id) {
    var obj = o("ProductGroup" + id);
    obj.parentNode.removeChild(obj);
}
//提交检查
function checkSubmit() {
    checkProductHandler(globalIDPrefix + "Product", "RelationProductID");
}
//读取内容
function readContent(photo, link, photoMobile, linkMobile, idList, nameList, photoList, id, themeActivityID) {
    var result = "<div class=\"form-row\"><input type=\"button\" class=\"button updateThemeAdd\" value=\"修改产品组\" onclick=\"updateThemeAdd(" + id + ")\"  style=\"width:80px\" /> ";
    result += "<input type=\"button\" class=\"button\" onclick=\"deleteProductGroup(" + id + ")\" value=\"删除产品组\"  style=\"width:80px\" />";
    result += "<input name=\"ProductGroupValue" + id + "\" id=\"ProductGroupValue" + id + "\"  type=\"hidden\"  value=\"" + photo + "|" + link + "|" + photoMobile + "|" + linkMobile + "|" + idList + "\"/></div>";
    result += "<div class=\"form-row\">";
    result += "<div class=\"head\">PC端图片：</div>";
    if (photo != "") {
        result += "<img src=\"" + photo + "\"  height=\"60\"/>";
    }
    result += "</div>";
    result += "<div class=\"form-row\">";
    result += "<div class=\"head\">移动端图片：</div>";
    result += "";
    if (photoMobile != "") {
        result += "<img src=\"" + photoMobile + "\"  height=\"60\"/>";
    }
    result += "</div>";
    result += "<div class=\"form-row\">";
    result += "<div class=\"head\">PC端更多地址：</div>";
    result += "" + link + "";
    result += "</div>";
    result += "<div class=\"form-row\">";
    result += "<div class=\"head\">移动端更多地址：</div>";
    result += "" + linkMobile + "";
    result += "</div>";
    result += "<div class=\"form-row\">";
    result += "<div class=\"head\">商品：</div>";
    result += "";
    if (idList != "") {
        for (var i = 0; i < idList.split(",").length; i++) {
            result += "<div class=\"themeActivityPhoto\"><img  src=\"" + photoList.split(",")[i] + "\" alt=\"\" onload=\"photoLoad(this,60,60)\" title=\"" + nameList.split(",")[i] + "\" /><br />" + nameList.split(",")[i] + "</div>";
        }
    }
    result += "</div>";
    result += "<input name=\"ProductGroupNameValue" + id + "\" id=\"ProductGroupNameValue" + id + "\"  type=\"hidden\"  value=\"" + nameList + "\"/>";
    return result;
}

//读取一条产品组数据
function readProductGroup(id, themeActivityID) {
    var productGroupValue = parent.$("#ProductGroupValue" + id).val();
    var productGroupNameValue = parent.$("#ProductGroupNameValue" + id).val();
    var valueArray = productGroupValue.split("|");
    var nameArray = productGroupNameValue.split(",");
    o(globalIDPrefix + "Photo").value = valueArray[0];
    if (valueArray[0] != "") {
        $("#" + globalIDPrefix + "Photo").before(" <a href='" + valueArray[0] + "' target='_blank'><img src='" + valueArray[0] + "' class='icon'  height='50' /></a>");
    }
    else {
        $("#" + globalIDPrefix + "Photo").before(" <img src='/Admin/Images/nopic.gif' class='icon'  height='50' />");
    }
    o(globalIDPrefix + "Link").value = valueArray[1];
    o(globalIDPrefix + "PhotoMobile").value = valueArray[2];
    if (valueArray[2] != "") {
        $("#" + globalIDPrefix + "PhotoMobile").before(" <a href='" + valueArray[2] + "' target='_blank'><img src='" + valueArray[2] + "' class='icon'  height='50' /></a>");
    }
    else {
        $("#" + globalIDPrefix + "PhotoMobile").before(" <img src='/Admin/Images/nopic.gif' class='icon'  height='50' />");
    }
    o(globalIDPrefix + "LinkMobile").value = valueArray[3];
    var productObj = o(globalIDPrefix + "Product");
    for (var i = 0; i < valueArray[4].split(",").length; i++) {
        productObj.options.add(new Option(nameArray[i], valueArray[4].split(",")[i]));
    }
}

function searchRelationProduct() {
    var productName = o(globalIDPrefix + "ProductName").value;
    var classID = o(globalIDPrefix + "RelationClassID").value;
    var brandID = o(globalIDPrefix + "RelationBrandID").value;
    var id = getQueryString("Id");
    var url = "ProductAjax.aspx?ControlName=CandidateProduct&Action=SearchRelationProduct&ProductName=" + encodeURI(productName) + "&ClassID=" + classID + "&BrandID=" + brandID + "&ID=" + id;
    Ajax.requestURL(url, dealSearchRelationProduct);
    alertMessage("正在搜索...", 1);
}

function dealSearchRelationProduct(data) {
    closeAlertDiv();
    var obj = o("CandidateProductBox");
    obj.removeChild(o(globalIDPrefix + "CandidateProduct"));
    obj.innerHTML = data;
}

function addAll(candidateObjName, selectedObjName) {
    var strID = getSelectedID(selectedObjName);
    var candidateObj = o(candidateObjName);
    var selectedObj = o(selectedObjName);
    for (var i = 0; i < candidateObj.length; i++) {
        if (strID.indexOf("|" + candidateObj.options[i].value + "|") == -1) {
            selectedObj.options[selectedObj.length] = new Option(candidateObj.options[i].text, candidateObj.options[i].value);
        }
    }
}

function addSingle(candidateObjName, selectedObjName) {
    var strID = getSelectedID(selectedObjName);
    var candidateObj = o(candidateObjName);
    var selectedObj = o(selectedObjName);
    for (var i = 0; i < candidateObj.length; i++) {
        if (candidateObj.options[i].selected && strID.indexOf("|" + candidateObj.options[i].value + "|") == -1) {
            selectedObj.options[selectedObj.length] = new Option(candidateObj.options[i].text, candidateObj.options[i].value);
        }
    }
}

function getSelectedID(objName) {
    var obj = o(objName);
    var result = "";
    for (var i = 0; i < obj.length; i++) {
        result = result + "|" + obj.options[i].value;
    }
    result = result + "|";
    return result;
}

function checkProductHandler(selectedObjName, operateObjName) {
    try {
        var obj = o(selectedObjName);
        var strID = '';
        for (var i = 0; i < obj.length; i++) {
            if (strID != '') {
                strID += ',' + obj.options[i].value;
            }
            else {
                strID = obj.options[i].value;
            }
        }
        o(operateObjName).value = strID;
    } catch (e) { }
}

function dropSingle(objName) {
    var obj = o(objName);
    if (obj.length < 1) {
        return;
    }
    for (var i = obj.length - 1; i >= 0; i--) {
        if (obj.options[i].selected) {
            obj.remove(i);
        }
    }
}

function dropAll(objName) {
    var obj = o(objName);
    if (obj.length < 1) {
        return;
    }
    for (var i = obj.length - 1; i >= 0; i--) {
        obj.remove(i);
    }
}