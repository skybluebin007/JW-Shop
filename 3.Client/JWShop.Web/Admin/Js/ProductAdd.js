
    readAttribute();

///属性操作
if (o(globalIDPrefix + "AttributeClassID") != null) {
    readAttributeWith(o(globalIDPrefix + "AttributeClassID").value);
}
function readAttributeWith(value) {
    if (value == "0") {
        o("Attribute-Ajax").innerHTML = "";
    }
    else {
        loading("Attribute-Ajax", "商品属性");
        var url = "AttributeRecordAjax.aspx?AttributeClassID=" + value + "&ProductID=" + productID;
        Ajax.requestURL(url, dealReadAttribute);
    }
}

function getProductClass() {
    var selectClassID = 0;

    if ($("#UnlimitClass3").length > 0) {
        if ($("#UnlimitClass3").val() > 0)
            selectClassID = $("#UnlimitClass3").val();
        else
            selectClassID = $("#UnlimitClass2").val();
    } else {
        if ($("#UnlimitClass2").length > 0) {
            if ($("#UnlimitClass2").val() > 0)
                selectClassID = $("#UnlimitClass2").val();
            else
                selectClassID = $("#UnlimitClass1").val();
        } else {
            selectClassID = $("#UnlimitClass1").val();
        }
    }

    return selectClassID;
}
function readAttribute() {
    //var selectClassID = getProductClass();
    var selectClassID = $("#proClassID").val();
    loading("Attribute-Ajax", "商品属性");

    var url = "AttributeRecordAjax.aspx?Action=newLoad&AttributeClassID=" + selectClassID + "&ProductID=" + productID;
    Ajax.requestURL(url, dealReadAttribute);

}

//function readAttribute(oid) {
//    alert("aaa");
//    loading("Attribute-Ajax", "商品属性");

//    var url = "AttributeRecordAjax.aspx?Action=newLoad&AttributeClassID=" + oid + "&ProductID=" + productID;
//    Ajax.requestURL(url, dealReadAttribute);
//}
function dealReadAttribute(content) {
    //o("Attribute-Ajax").innerHTML = content;
    $("#Attribute-Ajax").html(content);
    readStandard();
    //readBrand();
}
function selectKeyword(attributeID, keyword) {
    var attributeIDObj = o(attributeID + "Value");
    if (("," + attributeIDObj.value + ",").indexOf("," + keyword + ",") > -1) {
        alertMessage("已存在该关键字");
    }
    else {
        if (attributeIDObj.value == "") {
            attributeIDObj.value = keyword;
        }
        else {
            attributeIDObj.value += "," + keyword;
        }
    }
}

//读取规格
function readStandard() {
    //var selectClassID = getProductClass();
    var selectClassID = $("#proClassID").val();
    loading("Standard-Ajax", "商品规格");
    var url = "StandardAjax.aspx?AttributeClassID=" + selectClassID + "&ProductID=" + productID;
    var triger = $(".trigerstandard");
    //如果修改了分类则清除原有规格
    if (isUpdate == 1) {
        clearStandard(triger, 1);
        $("#standardbox").html("");
    }

    Ajax.requestURL(url, dealReadStandard);
}
if ($("#proClassID").val() != "0") {
    readStandardWith($("#proClassID").val());
}
function readStandardWith(oid) {
    loading("Standard-Ajax", "商品规格");
    var url = "StandardAjax.aspx?AttributeClassID=" + oid + "&ProductID=" + productID;
    Ajax.requestURL(url, dealReadStandard);
}
function dealReadStandard(content) {
    //o("Standard-Ajax").innerHTML = content;
    $("#Standard-Ajax").html(content);
}

function readBrand() {
    var selectClassID = getProductClass();
    //alert(selectClassID); 
    loading("ctl00_ContentPlaceHolder_BrandID", "商品品牌");
    var url = "BrandAjax.aspx?AttributeClassID=" + selectClassID + "&ProductID=" + productID;
    Ajax.requestURL(url, dealReadBrand);
}
function dealReadBrand(content) {
    o(globalIDPrefix + "BrandID").innerHTML = content;
    o(globalIDPrefix + "RelationBrandID").innerHTML = content;
    o(globalIDPrefix + "AccessoryBrandID").innerHTML = content;
}

if (o(globalIDPrefix + "StandardType") != null) {
    readStandard(o(globalIDPrefix + "StandardType").value);
}

function setStandardList(sType, pType) {
   
    $("#StandardType").val(pType);
    var oSalePrice = $("#" + globalIDPrefix + "SalePrice").val();
    var oMarketPrice = $("#" + globalIDPrefix + "MarketPrice").val();
    var oGroupPrice = $("#" + globalIDPrefix + "SalePrice").val();
    if ($("#hstandid").length > 0 && $("#hstandid").val().length>0) {
        //生成选中规格    
        var standIDList = $("#hstandid").val();
        var standNameList = $("#hstandname").val();
        var strHead = "<thead><tr>"
        var strBody = "<tbody><tr>";
        var sSelectCount1 = sSelectCount2 = sSelectCount3 = 0;
        standIDList = standIDList.substring(0, standIDList.length - 1);
        standArr = standIDList.split(";");
        nameArr = standNameList.substring(0, standNameList.length - 1).split(";");
        var hasChecked = 0; //选中数量
        var checkedIDs = "";
        var chk_value1 = []; var chk_value2 = []; var chk_value3 = [];
        for (i = 0; i < standArr.length; i++) {
            var nowCount;
            if (sType == 1) {
                nowCount = $("#sbox" + standArr[i] + " input:checked[name ="+ standArr[i]+"Value]").length;
            } else {
                nowCount = $("#sbox" + standArr[i] + " input").length;
            }
            if (nowCount > 0) {
                if (sType == 1) {
                    if (hasChecked == 0) {
                        chk_value1 = $("#sbox" + standArr[i] + " input:checked[name =" + standArr[i] + "Value]").each(function () {
                            chk_value1.push($(this).val());
                        });
                        sSelectCount1 = nowCount;
                    }
                    if (hasChecked == 1) {
                        chk_value2 = $("#sbox" + standArr[i] + " input:checked[name =" + standArr[i] + "Value]").each(function () {
                            chk_value2.push($(this).val());
                        });
                        sSelectCount2 = nowCount;
                    }
                    if (hasChecked == 2) {
                        chk_value3 = $("#sbox" + standArr[i] + " input:checked[name =" + standArr[i] + "Value]").each(function () {
                            chk_value3.push($(this).val());
                        });
                        sSelectCount3 = nowCount;
                    }
                } else {                  
                    if (hasChecked == 0) {
                        chk_value1 = $("#sbox" + standArr[i] + " input").each(function () {
                            chk_value1.push($(this).val());
                        });
                        sSelectCount1 = nowCount;
                    }
                    if (hasChecked == 1) {
                        chk_value2 = $("#sbox" + standArr[i] + " input").each(function () {
                            chk_value2.push($(this).val());
                        });
                        sSelectCount2 = nowCount;
                    }
                    if (hasChecked == 2) {
                        chk_value3 = $("#sbox" + standArr[i] + " input").each(function () {
                            chk_value3.push($(this).val());
                        });
                        sSelectCount3 = nowCount;
                    }
                }
                strHead += "<td >" + nameArr[i] + "</td>";
                checkedIDs += standArr[i] + ";";
                hasChecked++;
            }
        }

        if (pType == 1) {         
            strHead += "<td width=\"10%\"><input type=\"hidden\" name=\"StandardIDList\" id=\"StandardIDList\" value=\"" + checkedIDs + "\"> <span class=\"red\">*</span>本站价</td><td width=\"10%\"><span class=\"red\">*</span>市场价</td><td width=\"10%\"><span class=\"red\">*</span>团购价</td><td width=\"10%\"><span class=\"red\">*</span>库存</td><td width=\"15%\" >货号</td><td>图片</td></tr></thead>";
        } else {
            strHead += "<td width=\"55%\"><input type=\"hidden\" name=\"StandardIDList\" id=\"StandardIDList\" value=\"" + checkedIDs + "\"> 关联产品</td></tr></thead>";
        }

        var recordCount = 0;
        if (hasChecked == 3) {
            for (uc1 = 0; uc1 < sSelectCount1; uc1++) {
                for (uc2 = 0; uc2 < sSelectCount2; uc2++) {
                    for (uc3 = 0; uc3 < sSelectCount3; uc3++) {
                        recordCount++;
                        if (pType == 1) {
                            strBody += "<td >" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "|" + chk_value2[uc2].value + "|" + chk_value3[uc3].value + "\"></td><td >" + chk_value2[uc2].value + "</td><td >" + chk_value3[uc3].value + "</td><td ><input type=\"text\" name=\"sSalePrice\" value=\"" + oSalePrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsSalePrice()\" maxlength=\"8\"></td><td ><input type=\"text\" name=\"sMarketPrice\" value=\"" + oMarketPrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsMarketPrice()\"  maxlength=\"8\"></td><td ><input type=\"text\" name=\"sGroupPrice\" value=\"" + oGroupPrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsGroupPrice()\"  maxlength=\"8\"></td><td ><input type=\"text\" name=\"sStorage\" onkeyup=\"value=value.replace(/[^\\d]/g,'')\" onafterpaste=\"value=value.replace(/[^\\d]/g,'')\" onblur=\"setTotalStorage(this)\" value=\"0\"  maxlength=\"8\"></td><td  height=\"30\" ><input type=\"text\" name=\"sProductNumber\" value=\"\" ></td><td><img id=\"img_sPhoto" + recordCount + "\" src=\"/Admin/Images/nopic.gif\" width=\"50\" height=\"50\" class=\"standPhoto\"><input type=\"hidden\" name=\"sPhoto\" id=\"ctl00_ContentPlaceHolder_sPhoto" + recordCount + "\" value=\"\" ><div class=\"form-upload\"><iframe src=\"UploadStandardPhoto.aspx?Control=sPhoto" + recordCount + "&TableID=1&FilePath=ProductCoverPhoto/Original\" width=\"80px\" height=\"30px\" frameborder=\"0\" allowTransparency=\"true\" scrolling=\"no\" id=\"uploadIFrame\"></iframe></div></td></tr><tr>";
                        } else {
                            if (recordCount > 1) {
                                strBody += "<td >" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "|" + chk_value2[uc2].value + "|" + chk_value3[uc3].value + "\"></td><td >" + chk_value2[uc2].value + "</td><td >" + chk_value3[uc3].value + "</td><td ><input type=\"hidden\" id=\"Product" + recordCount + "\" name=\"Product\" value=\"0\" /><span id=\"ProductName" + recordCount + "\">当前产品</span> | <a href=\"javascript:loadProducts(" + recordCount + ");\">修改</a> | <a href=\"javascript:void(0)\" onclick=\"deleteStandard(this)\">删除</a></td></tr><tr>";
                            }
                            else {//产品组规格第一个规格的关联产品必须是自己，不能修改
                                strBody += "<td >" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "|" + chk_value2[uc2].value + "|" + chk_value3[uc3].value + "\"></td><td >" + chk_value2[uc2].value + "</td><td >" + chk_value3[uc3].value + "</td><td ><input type=\"hidden\" id=\"Product" + recordCount + "\" name=\"Product\" value=\"0\" /><span id=\"ProductName" + recordCount + "\">当前产品</span> </td></tr><tr>";
                            }
                        }
                    }
                }
            }
        }
        else if (hasChecked == 2) {
            for (uc1 = 0; uc1 < sSelectCount1; uc1++) {
                for (uc2 = 0; uc2 < sSelectCount2; uc2++) {
                    recordCount++;
                    if (pType == 1) {
                        strBody += "<td >" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "|" + chk_value2[uc2].value + "\"></td><td >" + chk_value2[uc2].value + "</td><td ><input type=\"text\" name=\"sSalePrice\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsSalePrice()\" value=\"" + oSalePrice + "\"  maxlength=\"8\"></td><td ><input type=\"text\" name=\"sMarketPrice\" value=\"" + oMarketPrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsMarketPrice()\" maxlength=\"8\"></td><td ><input type=\"text\" name=\"sGroupPrice\" value=\"" + oGroupPrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsGroupPrice()\"  maxlength=\"8\"></td><td ><input type=\"text\" name=\"sStorage\" value=\"0\" onkeyup=\"value=value.replace(/[^\\d]/g,'')\" onafterpaste=\"value=value.replace(/[^\\d]/g,'')\" onblur=\"setTotalStorage(this)\"  maxlength=\"8\"></td><td  height=\"30\" ><input type=\"text\" name=\"sProductNumber\" value=\"\" ></td><td><img id=\"img_sPhoto" + recordCount + "\" src=\"/Admin/Images/nopic.gif\" width=\"50\" height=\"50\" class=\"standPhoto\"><input type=\"hidden\" name=\"sPhoto\" id=\"ctl00_ContentPlaceHolder_sPhoto" + recordCount + "\" value=\"\" ><div class=\"form-upload\"><iframe src=\"UploadStandardPhoto.aspx?Control=sPhoto" + recordCount + "&TableID=1&FilePath=ProductCoverPhoto/Original\" width=\"80px\" height=\"30px\" frameborder=\"0\" allowTransparency=\"true\" scrolling=\"no\" id=\"uploadIFrame\"></iframe></div></td></tr><tr>";
                    } else {
                        if (recordCount > 1) {
                            strBody += "<td >" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "|" + chk_value2[uc2].value + "\"></td><td >" + chk_value2[uc2].value + "</td><td ><input type=\"hidden\" id=\"Product" + recordCount + "\" name=\"Product\" value=\"0\" /><span id=\"ProductName" + recordCount + "\">当前产品</span> | <a href=\"javascript:loadProducts(" + recordCount + ");\">修改</a> | <a href=\"javascript:void(0)\" onclick=\"deleteStandard(this)\">删除</a></td></tr><tr>";
                        }
                        else {//产品组规格第一个规格的关联产品必须是自己，不能修改
                            strBody += "<td >" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "|" + chk_value2[uc2].value + "\"></td><td >" + chk_value2[uc2].value + "</td><td ><input type=\"hidden\" id=\"Product" + recordCount + "\" name=\"Product\" value=\"0\" /><span id=\"ProductName" + recordCount + "\">当前产品</span></td></tr><tr>";
                        }
                    }
                }
            }
        }
        else if (hasChecked == 1) {
            for (uc1 = 0; uc1 < sSelectCount1; uc1++) {
                recordCount++;
                if (pType == 1) {
                    strBody += "<td  height=\"30\">" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "\"></td><td ><input type=\"text\" name=\"sSalePrice\" value=\"" + oSalePrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsSalePrice()\"  maxlength=\"8\"></td><td ><input type=\"text\" name=\"sMarketPrice\" value=\"" + oMarketPrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsMarketPrice()\"  maxlength=\"8\"></td><td ><input type=\"text\" name=\"sGroupPrice\" value=\"" + oGroupPrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsGroupPrice()\"  maxlength=\"8\"></td><td ><input type=\"text\" name=\"sStorage\" value=\"0\" onkeyup=\"value=value.replace(/[^\\d]/g,'')\" onafterpaste=\"value=value.replace(/[^\\d]/g,'')\" onblur=\"setTotalStorage(this)\"  maxlength=\"8\"></td><td  height=\"30\" ><input type=\"text\" name=\"sProductNumber\" value=\"\" ></td><td><img id=\"img_sPhoto" + recordCount + "\" src=\"/Admin/Images/nopic.gif\" width=\"20\" height=\"20\" class=\"standPhoto\"><input type=\"hidden\" name=\"sPhoto\" id=\"ctl00_ContentPlaceHolder_sPhoto" + recordCount + "\" value=\"\" ><div class=\"form-upload\"><iframe src=\"UploadStandardPhoto.aspx?Control=sPhoto" + recordCount + "&TableID=1&FilePath=ProductCoverPhoto/Original\" width=\"80px\" height=\"30px\" frameborder=\"0\" allowTransparency=\"true\" scrolling=\"no\" id=\"uploadIFrame\"></iframe></div></td></tr><tr>";
                } else {
                    if (recordCount > 1) {
                        strBody += "<td  height=\"30\">" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "\"></td><td ><input type=\"hidden\" id=\"Product" + recordCount + "\" name=\"Product\" value=\"0\" /><span id=\"ProductName" + recordCount + "\">当前产品</span> | <a href=\"javascript:loadProducts(" + recordCount + ");\">修改</a> | <a href=\"javascript:void(0)\" onclick=\"deleteStandard(this)\">删除</a></td></tr><tr>";
                    }
                    else {
                        //产品组规格第一个规格的关联产品必须是自己，不能修改
                        strBody += "<td  height=\"30\">" + chk_value1[uc1].value + "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + chk_value1[uc1].value + "\"></td><td ><input type=\"hidden\" id=\"Product" + recordCount + "\" name=\"Product\" value=\"0\" /><span id=\"ProductName" + recordCount + "\">当前产品</span></td></tr><tr>";
                    }
                }
            }
        } else {
            //alert("请至少在上方选择一个规格生成");
            $("#standardbox").html("");
            $("#standardbox").hide();
            //隐藏批量填充模块
            $("#batchSet").hide();
            //没有选中任何项则关闭规格
            $("#StandardType").val(0);
            return false;
        }
        if (pType == 1) {
            //如果是单产品规格则显示批量填充模块
            $("#batchSet").show();
        }
        else {
            //如果是单产品规格则显示批量填充模块
            $("#batchSet").hide();
        }

            strBody += "</tr></tbody>";
            $("#standardbox").html(strHead + strBody);
            $("#standardbox").show();
       
    } else {
        alert("没有选择分类或者该分类对应的产品类型下无规格");
    }
}
function clearStandard(o, sType) {
    if (sType == 1) {
        $("#standardAllBox").hide();
        $("#isOpenStandard").val(0);
        $("#StandardType").val(0);        
        $(o).attr("onclick", "clearStandard(this,2);")
        $(o).text("开启规格")
    } else {
        $("#standardAllBox").show();
        $("#isOpenStandard").val(1);
        $(o).attr("onclick", "clearStandard(this,1);")
        $(o).text("关闭规格")
    }
}

function deleteStandard(o) {
    $(o).parent().parent().remove();
}


//function getSelectValue(name) {
//    var objs = os("name", name);
//    var result = "";
//    if (objs != null && objs.length > 0) {
//        for (var i = 0; i < objs.length; i++) {
//            if (result == "") {
//                result = objs[i].value;
//            }
//            else {
//                result += "," + objs[i].value;
//            }
//        }
//    }
//    return result;
//}
//搜索关联商品
function searchRelationProduct() {
    var productName = o(globalIDPrefix + "ProductName").value;
    var classID = o(globalIDPrefix + "RelationClassID").value;
    var brandID = o(globalIDPrefix + "RelationBrandID").value;
    var id = getQueryString("ID");
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
//搜索关联配件
function searchProductAccessory(action) {
    var productAccessoryName = o(globalIDPrefix + "AccessoryProductName").value;
    var classID = o(globalIDPrefix + "AccessoryClassID").value;
    var brandID = o(globalIDPrefix + "AccessoryBrandID").value;
    var id = getQueryString("ID");
    var url = "ProductAjax.aspx?ControlName=CandidateAccessory&Action=SearchProductAccessory&ProductName=" + encodeURI(productAccessoryName) + "&ClassID=" + classID + "&BrandID=" + brandID + "&ID=" + id;
    Ajax.requestURL(url, dealSearchProductAccessory);
    alertMessage("正在搜索...", 1);
}
function dealSearchProductAccessory(data) {
    closeAlertDiv();
    var obj = o("CandidateAccessoryBox");
    obj.removeChild(o(globalIDPrefix + "CandidateAccessory"));
    obj.innerHTML = data;
}
//搜索关联文章
function searchRelationArticle() {
    var title = o(globalIDPrefix + "ArticleName").value;
    var classID = o(globalIDPrefix + "ArticleClassID").value;
    var url = "ProductAjax.aspx?ControlName=CandidateArticle&Action=SearchRelationArticle&ArticleTitle=" + encodeURI(title) + "&ClassID=" + classID;
    Ajax.requestURL(url, dealSearchArticle);
    alertMessage("正在搜索...", 1);
}
function dealSearchArticle(data) {
    closeAlertDiv();
    var obj = o("CandidateArticleBox");
    obj.removeChild(o(globalIDPrefix + "CandidateArticle"));
    obj.innerHTML = data;
}
//产品组规格--选择产品后处理方法
function dealSelectProduct(productID, productName, tag) {
    if (productName.length > 22) productName = productName.substring(0, 22)+"..";
    o("ProductName" + tag).innerHTML = productName;
    o("Product" + tag).value = productID;
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


function addProductAccessoryAll(candidateObjName, selectedObjName) {
    var strID = getSelectedID(selectedObjName);
    var candidateObj = o(candidateObjName);
    var selectedObj = o(selectedObjName);
    for (var i = 0; i < candidateObj.length; i++) {
        if (strID.indexOf("|" + candidateObj.options[i].value + "|") == -1) {
            selectedObj.options[selectedObj.length] = new Option(candidateObj.options[i].text, candidateObj.options[i].value);
        }
    }
}
function addProductAccessorySingle(candidateObjName, selectedObjName) {
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
//批量设置规格市场价 本站价 库存  货号
function BatchSetStandard() {
    if ($("#bzj").val() == "") {
        $("#bzj").focus();
        alertMessage("请填写本站价");
        return false;
    }
    if (isNaN($("#bzj").val()) || parseFloat($("#bzj").val()) < 0) {
        $("#bzj").focus();
        alertMessage("本站价填写不规范");
        return false;
    }
    if ($("#scj").val() == "") {
        $("#scj").focus();
        alertMessage("请填写市场价");
        return false;
    }
    if (isNaN($("#scj").val()) ||  parseFloat($("#scj").val()) < 0) {
        $("#scj").focus();
        alertMessage("市场价填写不规范");
        return false;
    }
    if ($("#kc").val() == "") {
        $("#kc").focus();
        alertMessage("请填写库存");
        return false;
    }
    if (isNaN($("#kc").val()) || parseInt($("#kc").val()) < 0) {
        $("#kc").focus();
        alertMessage("库存填写不规范");
        return false;
    }
    //规格 本站价批量赋值
    $("input[name='sSalePrice']").each(function () {     
        $(this).val( parseFloat($("#bzj").val()));         
    });
    //一口价 本站价赋值
    $("#ctl00_ContentPlaceHolder_SalePrice").val($("#bzj").val());
    //规格 市场价批量赋值
    $("input[name='sMarketPrice']").each(function () {
        $(this).val( parseFloat($("#scj").val()));
    });
    //一口价 市场价赋值
    $("#ctl00_ContentPlaceHolder_MarketPrice").val($("#scj").val());
    //规格 团购价批量赋值
    $("input[name='sGroupPrice']").each(function () {
        $(this).val(parseFloat($("#bzj").val()));
    });
    //规格 库存批量赋值
    var _storage = 0;
    $("input[name='sStorage']").each(function () {
        $(this).val(parseInt($("#kc").val()));
        _storage += parseInt($(this).val());
    });
    //一口价 库存赋值
    $("#ctl00_ContentPlaceHolder_TotalStorageCount").val(_storage);   
    $("#ctl00_ContentPlaceHolder_TotalStorageCount").attr("readonly", "readonly");
    $("#ctl00_ContentPlaceHolder_HidTotalStorageCount").val(_storage);

    //规格 货号批量赋值
    var k = 1;
    $("input[name='sProductNumber']").each(function () {
        $(this).val($("#hh").val() + "-" + k);
        k++;
    });
    
}
//根据填写的规格本站价自动填充一口价--本站价
function setsSalePrice() {
    var _tmp = 0;
    if (!isNaN($("input[name='sSalePrice']").eq(0).val())) _tmp = parseFloat($("input[name='sSalePrice']").eq(0).val());
    $("input[name='sSalePrice']").each(function () {
        if (!isNaN($(this).val()) && parseFloat($(this).val()) < _tmp) {
            _tmp = $(this).val();
        }
    });
    if(_tmp>0) $("#ctl00_ContentPlaceHolder_SalePrice").val(_tmp);
}
//根据填写的规格市场价自动填充一口价--市场价
function setsMarketPrice() {
    var _tmp = 0;
    if (!isNaN($("input[name='sMarketPrice']").eq(0).val())) _tmp = parseFloat($("input[name='sMarketPrice']").eq(0).val());
    $("input[name='sMarketPrice']").each(function () {
        if (!isNaN($(this).val()) && parseFloat($(this).val()) < _tmp) {
            _tmp = $(this).val();
        }
    });
    if (_tmp > 0) $("#ctl00_ContentPlaceHolder_MarketPrice").val(_tmp);
}
//根据填写的规格本站价自动填充一口价--团购价
function setsGroupPrice() {
    var _tmp = 0;
    if (!isNaN($("input[name='sGroupPrice']").eq(0).val())) _tmp = parseFloat($("input[name='sGroupPrice']").eq(0).val());
    $("input[name='sGroupPrice']").each(function () {
        if (!isNaN($(this).val()) && parseFloat($(this).val()) < _tmp) {
            _tmp = $(this).val();
        }
    });
    
}
//取一组数的最大值 最小值（用于比较一口价和规格价格范围）
function GetMinAndMax(obj) {
    var _min = 0, _max = 0;
    if (!isNaN(obj.eq(0).val())) _min = _max = parseFloat(obj.eq(0).val());
    obj.each(function () {
        if (!isNaN($(this).val()) && parseFloat($(this).val()) < _min) {
            _min = $(this).val();
        }
        if (!isNaN($(this).val()) && parseFloat($(this).val()) > _max) {
            _max = $(this).val();
        }
    }); 
    return _min + "|" + _max;
}
//检查一口价-本站价是否在规格范围内
function checkSalePrice() {
    var _min=0,_max=0;  
    if ($("input[name='sSalePrice']").length > 0) {
        var result = GetMinAndMax($("input[name='sSalePrice']"));
        if (result.indexOf("|") > 0) {
            _min = result.split("|")[0];
            _max = result.split("|")[1];
        }
        var _salePrice=parseFloat($("#ctl00_ContentPlaceHolder_SalePrice").val());
        if (_salePrice > _max || _salePrice < _min) {
            alertMessage("一口价-本站价必须在"+_min+"-"+_max+"之间");
            $("#ctl00_ContentPlaceHolder_SalePrice").focus();
            return false;
        }
    }   
}
//检查一口价-市场价是否在规格价格范围内
function checkMarketPrice() {
    var _min = 0, _max = 0;
    if ($("input[name='sMarketPrice']").length > 0) {
        var result = GetMinAndMax($("input[name='sMarketPrice']"));
        if (result.indexOf("|") > 0) {
            _min = result.split("|")[0];
            _max = result.split("|")[1];
        }
        var _marketPrice = parseFloat($("#ctl00_ContentPlaceHolder_MarketPrice").val());
        if (_marketPrice > _max || _marketPrice < _min) {
            alertMessage("一口价-市场价必须在" + _min + "-" + _max + "之间");
            $("#ctl00_ContentPlaceHolder_MarketPrice").focus();
            return false;
        }
    }
}
//检查商品名称
function checkProductName() {
    // 标题不为空
    if ($("#ctl00_ContentPlaceHolder_Name").val() == "") {
        $("#NameMsg").text("不能为空!");
        alertMessage("商品标题不能为空");
        $("#ctl00_ContentPlaceHolder_Name").focus();
        return false;
    }
    else {
        $("#NameMsg").text("");
    }
}

//添加商品提交前检查
function checkProduct() {
    // 标题不为空
    // 标题不为空
    if ($("#ctl00_ContentPlaceHolder_Name").val() == "") {
        $("#NameMsg").text("不能为空!");
        alertMessage("商品标题不能为空");
        $("#ctl00_ContentPlaceHolder_Name").focus();
        return false;
    }
    else {
        $("#NameMsg").text("");
    }
    //检查商品主图  
    if ($("#ctl00_ContentPlaceHolder_Photo").val() == "") {
        alertMessage("请上传商品主图");
        return false;
    }
    //检查规格信息填写 start
    var checkstandard = true;
    $("input[name='sSalePrice']").each(function () {
        var saleprice = $(this).val();
        if (saleprice == "") {
            $(this).focus();
            alertMessage("请填写规格对应本站价");
            checkstandard = false;
            return false;
        }
        if (isNaN(saleprice) || saleprice<0) {
            $(this).focus();
            alertMessage("规格对应本站价填写不规范");
            checkstandard = false;
            return false;
        }
    });
    $("input[name='sMarketPrice']").each(function () {
        var sMarketPrice = $(this).val();
        if (sMarketPrice == "") {
            $(this).focus();
            alertMessage("请填写规格对应市场价");
            checkstandard = false;
            return false;
        }
        if (isNaN(sMarketPrice) || sMarketPrice < 0) {
            $(this).focus();
            alertMessage("规格对应市场价填写不规范");
            checkstandard = false;
            return false;
        }
    });
    $("input[name='sGroupPrice']").each(function () {
        var sGroupPrice = $(this).val();
        if (sGroupPrice == "") {
            $(this).focus();
            alertMessage("请填写规格对应市场价");
            checkstandard = false;
            return false;
        }
        if (isNaN(sGroupPrice) || sGroupPrice < 0) {
            $(this).focus();
            alertMessage("规格对应团购价填写不规范");
            checkstandard = false;
            return false;
        }
    });
    $("input[name='sStorage']").each(function () {
        var sStorage = $(this).val();
        if (sStorage == "") {
            $(this).focus();
            alertMessage("请填写规格对应库存");
            checkstandard = false;
            return false;
        }
        if (isNaN(sStorage) || sStorage < 0) {
            $(this).focus();
            alertMessage("规格对应库存填写不规范");
            checkstandard = false;
            return false;
        }
    });
    if (checkstandard == false) { return false;}
    //检查规格信息填写 end
    //检查一口价-本站价是否在规格范围内  
        var _min=0,_max=0;  
        if ($("input[name='sSalePrice']").length > 0) {
            var result = GetMinAndMax($("input[name='sSalePrice']"));
            if (result.indexOf("|") > 0) {
                _min = result.split("|")[0];
                _max = result.split("|")[1];
            }
            var _salePrice=parseFloat($("#ctl00_ContentPlaceHolder_SalePrice").val());
            if (_salePrice > _max || _salePrice < _min) {
                alertMessage("一口价-本站价必须在"+_min+"-"+_max+"之间");
                $("#ctl00_ContentPlaceHolder_SalePrice").focus();
                return false;
            }
        }
    //检查一口价-市场价是否在规格范围内 
        if ($("input[name='sMarketPrice']").length > 0) {
            var result = GetMinAndMax($("input[name='sMarketPrice']"));
            if (result.indexOf("|") > 0) {
                _min = result.split("|")[0];
                _max = result.split("|")[1];
            }
            var _marketPrice = parseFloat($("#ctl00_ContentPlaceHolder_MarketPrice").val());
            if (_marketPrice > _max || _marketPrice < _min) {
                alertMessage("一口价-市场价必须在" + _min + "-" + _max + "之间");
                $("#ctl00_ContentPlaceHolder_MarketPrice").focus();
                return false;
            }
        }
        if (isNaN($("#ctl00_ContentPlaceHolder_OrderID").val()) || parseInt($("#ctl00_ContentPlaceHolder_OrderID").val() < 0)) {
            $("#ctl00_ContentPlaceHolder_OrderID").focus();
            alertMessage("排序号填写不规范");
            return false;
        }
        if (isNaN($("#ctl00_ContentPlaceHolder_TotalStorageCount").val()) || parseInt($("#ctl00_ContentPlaceHolder_TotalStorageCount").val() < 0)) {
            $("#ctl00_ContentPlaceHolder_TotalStorageCount").focus();
            alertMessage("总库存填写不规范");
            return false;
        }
        if (isNaN($("#ctl00_ContentPlaceHolder_LowerCount").val()) || parseInt($("#ctl00_ContentPlaceHolder_LowerCount").val() < 0)) {
            $("#ctl00_ContentPlaceHolder_LowerCount").focus();
            alertMessage("库存下限填写不规范");
            return false;
        }
    //正整数
        var reg2 = /^[0-9]*[1-9][0-9]*$/;
    //开启开团之后判断团购价格、团购人数
        var opengroup = $("input[name='ctl00$ContentPlaceHolder$OpenGroup']:checked").val();
        if (opengroup == 1 && parseFloat($("#ctl00_ContentPlaceHolder_GroupPrice").val())<=0) {
            alert("团购价格必填且必须大于0");
            $("#ctl00_ContentPlaceHolder_GroupPrice").focus();
            return false;
        }
        if (opengroup == 1 && !reg2.test($("#ctl00_ContentPlaceHolder_GroupQuantity").val())) {
            alert("团购人数必填且必须是正整数");
            $("#ctl00_ContentPlaceHolder_GroupQuantity").focus();
            return false;
        }
    //虚拟销量开启了之后判断虚拟销量数是正整数     
        var usevirtualorder = $("input[name='ctl00$ContentPlaceHolder$UseVirtualOrder']:checked").val();
        if (usevirtualorder == 1 && !reg2.test($("#ctl00_ContentPlaceHolder_VirtualOrderCount").val())) {
            alert("虚拟销量数必填且必须是正整数");
            $("#ctl00_ContentPlaceHolder_VirtualOrderCount").focus();
            return false;
        }
        
    checkProductHandler(globalIDPrefix + "Product", "RelationProductID");
    checkProductHandler(globalIDPrefix + "Article", "RelationArticleID");
    checkProductHandler(globalIDPrefix + "Accessory", "RelationAccessoryID");

    return Page_ClientValidate();
}

function checkArticle() {
    checkProductHandler(globalIDPrefix + "Product", "RelationProductID");
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
//添加商品图片
//取图集<li>属性 _orderid最大值
function GetMinAndMaxOrder(obj) {
    var _min = 0, _max = 0;
    if (!isNaN(obj.eq(0).val())) _min = _max = parseFloat(obj.eq(0).val());
    obj.each(function () {
        if (!isNaN($(this).attr("_orderid")) && parseFloat($(this).attr("_orderid")) < _min) {
            _min = $(this).attr("_orderid");
        }
        if (!isNaN($(this).attr("_orderid")) && parseFloat($(this).attr("_orderid")) > _max) {
            _max = $(this).attr("_orderid");
        }
    });
    return _min + "|" + _max;
}
function addProductPhoto(photo, name, proStyle) {
    var productID = getQueryString("ID");  
    if (productID == "" || productID <= 0) {       
        var content = "<li class=\"productPhoto\" id=\"ProductPhoto" + photo + "\" >";
        content += "<img src=\"" + photo + "\" alt=\"\"  title=\"" + photo + "\" onload=\"photoLoad(this,90,90)\" id=\"photo" + productID + "\"/>";
        content += substring(name, 6) + "<div class='opr'>";
        content += "<span class=\"down\" onclick=\"MoveDownProductPhotoLi(this)\" title=\"后移\">后移</span>";
        content += "<span class=\"up\" onclick=\"MoveUpProductPhotoLi(this)\" title=\"前移\">前移</span>";
        content += "<span class='delete' onclick=\"deleteProductPhoto('" + photo + "','" + proStyle + "')\" title='删除'>删除</span>";
        content += "<a class='cut' href=\"javascript:loadCut('" + photo.replace("75-75", "Original") + "','ProductPhoto" + photo + "',0)\" title='裁剪'>裁剪</a></div>";
        content += "<input name=\"ProductPhoto\"  type=\"hidden\" value=\"" + name + "|" + photo + "\"/>";
        content += "</li>";
        //o("ProductPhotoList").innerHTML = o("ProductPhotoList").innerHTML + content;
        $('#ProductPhotoListAdd').before(content);      
    }
    else {
        Ajax.requestURL("Ajax.aspx?Action=AddProductPhoto&ProductID=" + productID + "&Name=" + name + "&Photo=" + photo + "&proStyle=" + proStyle, dealAddProductPhoto);
    }
}
function dealAddProductPhoto(data) {
    if (data != "") {
        var productPhotoArray = data.split("|");
        id = productPhotoArray[0];
        name = productPhotoArray[1];
        photo = productPhotoArray[2];
        proStyle = productPhotoArray[3];
        var content = "<li class=\"productPhoto\" id=\"ProductPhoto" + id + "\">";
        content += "<img src=\"" + photo + "\" alt=\"\"  title=\"" + photo + "\" onload=\"photoLoad(this,90,90)\"  id=\"photo" + id + "\"/>";
        content += substring(name, 6) + "<div class='opr'>";
        content += "<span class=\"down\" onclick=\"MoveDownProductPhoto(" + id + ")\" title=\"后移\">后移</span>";
        content += "<span class=\"up\" onclick=\"MoveUpProductPhoto(" + id + ")\" title=\"前移\">前移</span>";
        content += "<span class='delete' onclick=\"deleteProductPhoto('" + id + "','" + proStyle + "')\" title='删除'>删除</span>";
        content += "<a  class='cut' href=\"javascript:loadCut('" + photo.replace("75-75", "Original") + "','ProductPhoto" + id + "','" + id + "')\" title='裁剪'>裁剪</a></div>";
        content += "</li>";
        // o("ProductPhotoList").innerHTML=o("ProductPhotoList").innerHTML+content;
        $('#ProductPhotoListAdd').before(content);  // date:2015.12.21 by:charlee
    }
}
//添加商品时，图集图片后移
function MoveDownProductPhotoLi(photospan) {  
    $(photospan).parent().parent().next(".productPhoto").insertBefore($(photospan).parent().parent()); 
}
//添加商品时，图集图片前移
function MoveUpProductPhotoLi(photospan) {
    $(photospan).parent().parent().prev(".productPhoto").insertAfter($(photospan).parent().parent());   
}

//后移商品图片
function MoveDownProductPhoto(photoid) {
    //if (confirm("您确定将此图片往后移一位？")) {
    $.ajax({
        type: 'get',
        url: "Ajax.aspx?Action=MoveDownProductPhoto&productPhotoId=" + photoid,
        //data: {},
        cache: false,
        dataType: 'json',
        success: function (content) {
            if (content.flag == true) {
                alertMessage("操作成功", 500);
                $("#ProductPhotoList").find(".productPhoto:not('#firstPhotoLi')").remove();
                var _html = "";
                for (var i in content.dataList) {
                    var item = content.dataList[i];
                    _html += "<li class='productPhoto' id='ProductPhoto" + item.Id + "'><img src='" + item.ImageUrl.replace("Original", "75-75") + "' alt='' title='" + item.Name + "' onload='photoLoad(this,90,90)' id='photo" + item.Id + "' /><div class='opr'><span class='down' onclick='MoveDownProductPhoto(" + item.Id + ")' title='后移'>后移</span><span class='up' onclick='MoveUpProductPhoto(" + item.Id + ")' title='前移'>前移</span><span class='delete' onclick='deleteProductPhoto(" + item.Id + ")' title='删除'>删除</span><a  class='cut' href=\"javascript:loadCut('" + item.ImageUrl.replace("75-75", "Original") + "','ProductPhoto" + item.Id + "'," + item.Id + ")\" title=\"裁剪\">裁剪</a></div></li>";
                }
                $("#ProductPhotoListAdd").before(_html);
            }
            else {
                alert("系统忙，请稍后重试");
            }
        },
        error: function () {

            alert("系统忙，请稍后重试");
        }
    });
    //}
}
//前移商品图片
function MoveUpProductPhoto(photoid) {
    //if (confirm("您确定将此图片往前移一位？")) {
    $.ajax({
        type: 'get',
        url: "Ajax.aspx?Action=MoveUpProductPhoto&productPhotoId=" + photoid,
        //data: {},
        cache: false,
        dataType: 'json',
        success: function (content) {
            if (content.flag == true) {
                alertMessage("操作成功", 500);
                $("#ProductPhotoList").find(".productPhoto:not('#firstPhotoLi')").remove();
                var _html = "";
                for (var i in content.dataList) {
                    var item = content.dataList[i];
                    _html += "<li class='productPhoto' id='ProductPhoto" + item.Id + "'><img src='" + item.ImageUrl.replace("Original", "75-75") + "' alt='' title='" + item.Name + "' onload='photoLoad(this,90,90)' id='photo" + item.Id + "' /><div class='opr'><span class='down' onclick='MoveDownProductPhoto(" + item.Id + ")' title='后移'>后移</span><span class='up' onclick='MoveUpProductPhoto(" + item.Id + ")' title='前移'>前移</span><span class='delete' onclick='deleteProductPhoto(" + item.Id + ")' title='删除'>删除</span><a  class='cut' href=\"javascript:loadCut('" + item.ImageUrl.replace("75-75", "Original") + "','ProductPhoto" + item.Id + "'," + item.Id + ")\" title=\"裁剪\">裁剪</a></div></li>";
                }
                $("#ProductPhotoListAdd").before(_html);
            }
            else {
                alert("系统忙，请稍后重试");
            }
        },
        error: function () {

            alert("系统忙，请稍后重试");
        }
    });
    //}
}
//图集剪切后载入
function editProductPhoto(targetID, delTarget, productPhotoID) {
    var _name="";
  
    if (productPhotoID <= 0 || productPhotoID == "") {//添加商品
        //$("#" + targetID + ":first-child").attr("src", delTarget);
        o(targetID).firstChild.src = delTarget;
        //$("#" + targetID).find("[name='ProductPhoto']").val( _name + "|" + delTarget );
        o(targetID).lastChild.value = _name + "|" + delTarget;
    }
    else {
        //修改商品
        $("#photo"+productPhotoID).attr("src", delTarget);
    }

    //alert($("li[id='" + targetID + "']").length);
    $("li[id='" + targetID + "']").find(".cut").attr("href", "javascript:loadCut('" + delTarget + "','" + targetID + "','" + productPhotoID + "')");

   
}

//删除产品图片
function deleteProductPhoto(id, proStyle) {
    var productID = getQueryString("ID");
    removeSelf(o("ProductPhoto" + id));
    if (productID != "") {
        Ajax.requestURL("Ajax.aspx?Action=DeleteProductPhoto&ProductPhotoID=" + id + "&proStyle=" + proStyle, function () { });
    }
}
//删除产品
function deleteProduct(productID) {
    if (window.confirm("确定要删除该商品？")) {
        var url = "Ajax.aspx?Action=DeleteProduct&ProductID=" + productID;
        Ajax.requestURL(url, dealDeleteProduct);
    }
}
function dealDeleteProduct(data) {
    if (data == "ok") {
        reloadPage();
    }
    else {
        alert("该商品存在相关订单，不能删除。");
    }
}
//下架商品
function offSaleProduct(productID) {
    if (window.confirm("确定要下架该商品？")) {
        $.ajax({
            url: "Ajax.aspx",
            type: "get",
            data: { Action: "OffSaleProduct", ProductID: productID },
            dataType: "text",
            success: function (data) {
                if (data == "ok") {
                    alertMessage("操作成功",500);
                    location.reload();
                }
                else {
                    alertMessage("系统忙，请稍后重试");
                }
            },
            error: function () { alertMessage("系统忙，请稍后重试"); }
        })
    }
}
//上架商品
function onSaleProduct(productID) {
    if (window.confirm("确定要上架该商品？")) {
        $.ajax({
            url: "Ajax.aspx",
            type: "get",
            data: { Action: "OnSaleProduct", ProductID: productID },
            dataType: "text",
            success: function (data) {
                if (data == "ok") {
                    alertMessage("操作成功", 500);
                    location.reload();
                }
                else {
                    alertMessage("系统忙，请稍后重试");
                }
            },
            error: function () { alertMessage("系统忙，请稍后重试"); }
        })
    }
}
function setTotalStorage(o) {
    var tarstorage = $("#ctl00_ContentPlaceHolder_TotalStorageCount");
    tarstorage.attr("readonly", "readonly");
    var allStorage = 0;
    $("input[name='sStorage']").each(function () {
        allStorage = parseInt(allStorage) + parseInt($(this).val());
    });
    tarstorage.val(allStorage);
    $("#ctl00_ContentPlaceHolder_HidTotalStorageCount").val(allStorage);

}
//上传主图立即显示
function loadFirstPhoto() {
    var imgsrc = $("#ctl00_ContentPlaceHolder_Photo").val();
    if (imgsrc != "") {
        $("#firstPhoto").attr("src", imgsrc);
        $("#firstPhotoLi").show();
        $("#firstPhotoUploadLi").hide();
    }
    else {
        $("#firstPhoto").attr("src", "");
        $("#firstPhotoUploadLi").show();
    }
}
//删除商品主图
function deleteProductCoverPhoto() {
    var productPhoto = $("#ctl00_ContentPlaceHolder_Photo").val();
    $.ajax({
        url: "Ajax.aspx?Action=DeleteProductCoverPhoto&ProductPhoto=" + productPhoto,
        type: 'get',       
        cache: false,     
        success: function (data) {
            if (data == "ok") {
                $("#ctl00_ContentPlaceHolder_Photo").val("");
                $("#firstPhoto").attr("src", "");
                $("#firstPhotoLi").hide();
                $("#firstPhotoUploadLi").show();
            }
            else {
                alert("系统忙，请稍后重试1");
            }

        },
        error: function () {
            alert("系统忙，请稍后重试2");
        }
    })
}



//价格只能输入正数及两位小数    
function clearNoNum(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符   
    obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的   
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数   
    if (obj.value.indexOf(".") < 0 && obj.value != "") {//以上已经过滤，此处控制的是如果没有小数点，首位不能为类似于 01、02的金额  
        obj.value = parseFloat(obj.value);
    }
}

    //清空搜索条件
        function ResetSearch(){    
        $("#ctl00_ContentPlaceHolder_Key").val("");
        $("#ctl00_ContentPlaceHolder_ProductNumber").val("");
        $("#ctl00_ContentPlaceHolder_ClassID").val("0");
        $("#ctl00_ContentPlaceHolder_LowerSalePrice").val("");
        $("#ctl00_ContentPlaceHolder_UpperSalePrice").val("");
        $("#ctl00_ContentPlaceHolder_LowerOrderCount").val("");
        $("#ctl00_ContentPlaceHolder_UpperOrderCount").val("");
        $("#ctl00_ContentPlaceHolder_BrandID").val("0");
        $("#ctl00_ContentPlaceHolder_IsTop").val("");
        $("#ctl00_ContentPlaceHolder_IsHot").val("");        
    }


