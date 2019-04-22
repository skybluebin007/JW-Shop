 function searchProduct() {
    var productName = o("ProductName").value;        
    var url = "/Admin/ProductAjax.aspx?ControlName=PrdouctID&Action=SearchProductByName&ProductName=" + encodeURI(productName);
    Ajax.requestURL(url, dealSearchProduct);
    alertMessage("正在搜索...", 1); 
}
function dealSearchProduct(data) {
    closeAlertDiv();
    var obj = o("PrdouctIDBox");
    obj.removeChild(o(globalIDPrefix+"PrdouctID"));
    obj.innerHTML = data;
    var productListObj = o(globalIDPrefix+"PrdouctID");
    if (productListObj.length == 0) {
        productListObj.options.add(new Option("没有符合您搜索的产品", "0"));
    }
}
function checkProductSelect(){
    Page_ClientValidate();
    var productListObj = o(globalIDPrefix+"PrdouctID");
    if(productListObj.value=="0" || productListObj.value==""){
         alertMessage("请选择商品"); 
         return false;
    }
    else{
        return true;
    }
}