function fatherUnlimitClassChange(i, prefix,functionName) {   
    var fatherID = o(prefix + "UnlimitClass" + i).value;
    var gradeCount = o(prefix + "UnlimitClassGradeCount").value;
    for (var k = i + 1; k <= gradeCount; k++) {
    try{
        var obj = removeSelf(o(prefix + "UnlimitClass" + k));}catch(e){}
    }
    var childData =new Array();
    if(fatherID!="0"){            
        for(var k=0;k<unlimitClassData.length;k++){
            if(unlimitClassData[k][2]==fatherID && unlimitClassData[k][3]==prefix){
                childData.push(unlimitClassData[k]);
            }
        }
    }
    if(childData.length>0){      
        i++;
        var newSelectObj = document.createElement("select");
        newSelectObj.id = prefix + "UnlimitClass" + i;
        newSelectObj.name = prefix + "UnlimitClass" + i;        
        addEvent(newSelectObj,"change",function() { fatherUnlimitClassChange(i, prefix,functionName) });    
        newSelectObj.options.add(new Option("请选择", 0));
        for (var k = 0; k < childData.length; k++) {
            newSelectObj.options.add(new Option(childData[k][1], childData[k][0]));
        }        
        var div = o(prefix + "FatherUnlimitClass");
        div.appendChild(newSelectObj);              
    } 
    o(prefix + "UnlimitClassGradeCount").value=i;   
    if(functionName!=""){
        eval(functionName);
    }     
}
//读取搜索分类ID
function readSearchClassID(prefix) {
    var classCount = o(prefix + "UnlimitClassGradeCount").value;
    var result = "";
    for (var i = 1; i <= classCount; i++) {
        var temp = o(prefix + "UnlimitClass" + i).value;
        if (temp != "0") {
            if (result == "") {
                result = "|" + temp + "|";
            }
            else {
                result += temp + "|";
            }
        }
    }
    return result;
}
 //添加类别
function addUnlimitClass(prefix) {
    var productClassValueObj = o(prefix+"UnlimitClassValue");
    var productClassGradeCountObj = o(prefix+"UnlimitClassGradeCount");
    var text = "";
    var idList = "";
    for (var i = 1; i <= productClassGradeCountObj.value; i++) {
        var obj = o(prefix+"UnlimitClass" + i);
        if (obj.value != "0") {
            text += obj.options[obj.selectedIndex].text + ">";
            idList += obj.value + "|";
        }
    }
    if (text != "" && idList != "") {
        var productClassValue = o(prefix+"UnlimitClassValue").value;
        var fatherExist = false;
        if (productClassValue != "") {
            var productClassValueArray = productClassValue.split("||");
            for (var k = 0; k < productClassValueArray.length; k++) {
                var tempValue = productClassValueArray[k];

                if (tempValue.substr(0, 1) != "|") {
                    tempValue = "|" + tempValue;
                }
                if (tempValue.substr(tempValue.length - 1, 1) != "|") {
                    tempValue = tempValue + "|";
                }
                if (("|" + idList).indexOf(tempValue) > -1) {
                    fatherExist = true;
                    break;
                }
            }
        }
        if (fatherExist) {
            alertMessage("不能选择该分类,该类或者其父类已存在",500);
        }
        else {
            if (productClassValue.indexOf("|" + idList) > -1) {
                alertMessage("不能选择该分类，该类的子类已存在",500);
            }
            else {
                text = text.substr(0, text.length - 1);
                var content = text + "&nbsp;<a href=\"javascript:deleteUnlimitClass('|" + idList + "','" + prefix + "')\" title=\"删除\"><img src=\"images/delete.gif\" /></a>";
                var pElemen = document.createElement("p");
                //with (pElemen) {
                    if (prefix) {
                        pElemen.id = prefix + "SelectUnlimitClass|" + idList;
                    }
                    else {
                        pElemen.id = "SelectUnlimitClass|" + idList;
                    }
                //}
                pElemen.innerHTML = content;
                var pareObj = o(prefix+"SelectUnlimitClass");
                pareObj.appendChild(pElemen);
                if (productClassValue != "") {
                    o(prefix+"UnlimitClassValue").value="|" + idList + productClassValue;
                }
                else {
                    o(prefix+"UnlimitClassValue").value="|" + idList;
                }
            }
        }
    }
}
//删除类别
function deleteUnlimitClass(id, prefix) {
    var obj = o(prefix + "SelectUnlimitClass" + id);
    removeSelf(obj);
    if(o(prefix+"UnlimitClassValue").value == id) {
        o(prefix+"UnlimitClassValue").value="";
    }
    else {
        o(prefix+"UnlimitClassValue").value=o(prefix+"UnlimitClassValue").value.replace(id, "");
    }
}
//检查是否选择分类
function checkSelectClass() {
    if (o(prefix+"UnlimitClassValue").value != "") {
        return true;
    }
    else {
        alertMessage("请选择该商品分类",500);
        return false;
    }
}