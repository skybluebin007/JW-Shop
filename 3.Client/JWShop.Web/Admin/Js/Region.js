var regionID = 0;
//读取地区
function readRegion(id) {    
    var url = "RegionAjax.aspx?Action=Read&ID=" + id;   
    regionID = id;
    Ajax.requestURL(url, dealReadRegion);
}
function dealReadRegion(data) {    
    o("Region-Ajax").innerHTML = data;    
}
//删除地区
function deleteRegion(id) {    
    var url = "RegionAjax.aspx?Action=Delete&ID=" + id;
    Ajax.requestURL(url, dealDeleteRegion);
}
function dealDeleteRegion(data) {
    if (data == 'error') {
        alertMessage('不允许删除该类别');
    }
    else{
        alertMessage('成功删除');
        readRegion(regionID);
    }
}
//添加新地区
function addRegion() {
    var regionNameValue = o(globalIDPrefix+"RegionName").value;
    if (regionNameValue.length < 1) {
        alertMessage("请输入地区名称");
        return;
    }
    var url = "RegionAjax.aspx?Action=Add&RegionName=" + encodeURI(regionNameValue) + "&FatherID=" + regionID;    
    Ajax.requestURL(url,dealAddRegion);
}
function dealAddRegion() {
    alertMessage('增加成功');
    readRegion(regionID);
}
//修改地区
function updateRegion(obj, newValue, oldValue, id) {
    if (newValue.length > 0 && newValue!=oldValue) {
        var url = "RegionAjax.aspx?Action=Update&ID=" + id + "&Name=" + encodeURI(newValue);         
         obj.innerHTML =newValue;
        Ajax.requestURL(url,dealUpdateRegion);       
    }
    else {
        obj.innerHTML = oldValue;
    }
}
function dealUpdateRegion() {
    alertMessage('更新成功');
}

