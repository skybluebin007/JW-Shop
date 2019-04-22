<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="StandardAjax.aspx.cs" Inherits="JWShop.Web.Admin.StandardAjax" %>

<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
    <%
        string standardStr = string.Empty;
        string standardName = string.Empty;
        int standardCount = 1;
        #region 获取该商品的规格值ValueList
        string _productStandardValues = string.Empty;
        if (product.Id > 0)
        {
            List<ProductTypeStandardRecordInfo> recordList = ProductTypeStandardRecordBLL.ReadListByProduct(product.Id, product.StandardType);
            foreach (var _standardRecord in recordList)
            {
                if (_productStandardValues == string.Empty) _productStandardValues = _standardRecord.ValueList;
                else _productStandardValues += ";" + _standardRecord.ValueList;
            }
            _productStandardValues = ";" + _productStandardValues + ";";
        }
        #endregion
        foreach (var standard in standardList)
        {           
            %>
    <div class="form-checkbox" id="sbox<%=standard.Id%>">
        <div class="head"><%=standard.Name %>：</div>
        <div class="checkList">
	        <label class="ig-checkbox"><input type="checkbox" name="chooseAll<%=standard.Id%>"" _divId="sbox<%=standard.Id%>" onclick="chooseAll(this)"/><span class="red">全选</span></label>
	        <%            
	            foreach (string inputValue in standard.ValueList.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries))
	            {                
	                %>        
	            <label <%if (_productStandardValues.IndexOf(";" + inputValue + ";") >= 0){%>class="ig-checkbox checked"<%}else{ %>class="ig-checkbox"<%} %>><input name="<%=standard.Id %>Value" type="checkbox"  value="<%=inputValue %>" <%if (_productStandardValues.IndexOf(";" + inputValue + ";") >= 0){%> checked="checked"<%} %> onclick="RecreateStandard(1,1);"/><%=inputValue %>&nbsp;&nbsp;</label>
	        <%} %>
        </div>
    </div>    
    <%
          standardStr += standard.Id + ";";
          standardName += standard.Name + ";";
          standardCount++;
    } %>
    <input type="hidden" name="hstandid" id="hstandid" value="<%=standardStr %>" />
    <input type="hidden" name="hstandname" id="hstandname" value="<%=standardName %>" />
