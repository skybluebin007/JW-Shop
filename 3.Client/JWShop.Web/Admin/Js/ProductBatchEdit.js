/**逐个编辑**/
function saveSingleEdit(productID) {
    var productNumber = o("ProductNumber" + productID);
    var weight = o("Weight" + productID);
    var marketPrice = o("MarketPrice" + productID);
    var salePrice = o("SalePrice" + productID);
    var sendPoint = o("SendPoint" + productID);
    var totalStorageCount = o("TotalStorageCount" + productID);
    var lowerCount = o("LowerCount" + productID);
    var upperCount = o("UpperCount" + productID);
    if (!Validate.isNumber(weight.value)) {
        alertMessage("重量必须是货币类型");
        return;
    }
    if (!Validate.isNumber(marketPrice.value)) {
        alertMessage("市场价必须是货币类型");
        return;
    }
    if (!Validate.isNumber(salePrice.value)) {
        alertMessage("本站价必须是货币类型");
        return;
    }    
    if (!Validate.isInt(sendPoint.value)) {
        alertMessage("赠送积分必须是数字类型");
        return;
    }
    if (!Validate.isInt(totalStorageCount.value)) {
        alertMessage("库存数量必须是数字类型");
        return;
    }
    if (!Validate.isInt(lowerCount.value)) {
        alertMessage("库存下限必须是数字类型");
        return;
    }
    if (!Validate.isInt(upperCount.value)) {
        alertMessage("库存上限必须是数字类型");
        return;
    }   
    var url = "ProductSingleEdit.aspx?Action=SingleEdit&ProductID=" + productID;
    url += "&ProductNumber=" + productNumber.value;
    url += "&Weight=" + weight.value;
    url += "&MarketPrice=" + marketPrice.value;
    url += "&SalePrice=" + salePrice.value;
    url += "&SendPoint=" + sendPoint.value;
    url += "&TotalStorageCount=" + totalStorageCount.value;
    url += "&LowerCount=" + lowerCount.value;
    url += "&UpperCount=" + upperCount.value;
    Ajax.requestURL(url, dealSaveEdit);
}
function dealSaveEdit(data) {
    alertMessage(data);
}

/**统一编辑**/
function deleteProduct(productID){
    removeSelf(o("Product"+productID));
}
function saveUnionEdit(){
    var productIDList=getCheckboxValue(os("name","ProductID"));
    if(productIDList!=""){
        var isEdit=false;
        var weight = o("Weight");
        var marketPrice = o("MarketPrice");
        var salePrice = o("SalePrice");
        var sendPoint = o("SendPoint");
        var totalStorageCount = o("TotalStorageCount");
        var lowerCount = o("LowerCount" );
        var upperCount = o("UpperCount");
        if(weight.value!=""){
            if (!Validate.isNumber(weight.value)) {
                alertMessage("重量必须是货币类型");
                return;
            }
            isEdit=true;
        }
        if(marketPrice.value!=""){
            if (!Validate.isNumber(marketPrice.value)) {
                alertMessage("市场价必须是货币类型");
                return;
            }
            isEdit=true;
        }
        if (salePrice.value != "") {
            if (!Validate.isNumber(salePrice.value)) {
                alertMessage("本站价必须是货币类型");
                return;
            }
            isEdit = true;
        }        
        if(sendPoint.value!=""){
            if (!Validate.isInt(sendPoint.value)) {
                alertMessage("赠送积分必须是数字类型");
                return;
            }
            isEdit=true;
        }
        if(totalStorageCount.value!=""){
            if (!Validate.isInt(totalStorageCount.value)) {
                alertMessage("库存数量必须是数字类型");
                return;
            }
            isEdit=true;
        }
        if(lowerCount.value!=""){
            if (!Validate.isInt(lowerCount.value)) {
                alertMessage("库存下限必须是数字类型");
                return;
            }
            isEdit=true;
        }
        if(upperCount.value!=""){
            if (!Validate.isInt(upperCount.value)) {
                alertMessage("库存上限必须是数字类型");
                return;
            } 
            isEdit=true;  
        }   
        if(isEdit){
            var url = "ProductUnionEdit.aspx?Action=UnionEdit&ProductIDList=" + productIDList;
            url += "&Weight=" + weight.value;
            url += "&MarketPrice=" + marketPrice.value;
            url += "&SalePrice=" + salePrice;
            url += "&SendPoint=" + sendPoint.value;
            url += "&TotalStorageCount=" + totalStorageCount.value;
            url += "&LowerCount=" + lowerCount.value;
            url += "&UpperCount=" + upperCount.value;
            Ajax.requestURL(url, dealSaveUnionEdit);
        }
        else{
            alertMessage("请先选择要编辑的项");
        }
    }
    else{
        alertMessage("请先选择商品");
    }
}
function dealSaveUnionEdit(data) {
    alertMessage(data);
}