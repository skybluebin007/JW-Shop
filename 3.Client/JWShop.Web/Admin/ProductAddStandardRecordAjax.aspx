<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="ProductAddStandardRecordAjax.aspx.cs" Inherits="JWShop.Web.Admin.ProductAddStandardRecordAjax" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<style type="text/css">#tbodyStandard select{ width : 70px;}</style>

<%if(standardList.Count > 0) {%>
    <table class="listTable" style="width:600px">
        <thead>
            <tr class="listTableHead">
                <%foreach (var standard in standardList) {%>
                    <td style="width: 10%"><%=standard.Name %></td>
                <%} %>
                <td style="width:10%">市场价</td>
                <td style="width:10%">售价</td>
                <td style="width:10%">库存</td>
                <td style="width:10%">条码</td>
                <td style="width:5%">操作</td>
            </tr>
        </thead>
        <tbody id="tbodyStandard">
            <% if(standardRecordList.Count > 0) {%>
                <% foreach(var item in standardRecordList) {%>
                    <% var _index = standardRecordList.IndexOf(item); %>
                    <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')" rowNum="<%=_index %>">
                        <%foreach (var standard in standardList) {%>
                            <% 
                                int _selectStdIndex = 0;
                                string _selectStdValue = "";
                                try
                                {
                                    if (!string.IsNullOrEmpty(item.StandardIdList) && !string.IsNullOrEmpty(item.ValueList))
                                    {
                                        var _selectStdIndexs = item.StandardIdList.Split(';');
                                        var _selectStdValues = item.ValueList.Split(';');
                                        if (_selectStdIndexs.Length == _selectStdValues.Length)
                                        {
                                            _selectStdIndex = _selectStdIndexs.ToList().IndexOf(standard.Id.ToString());
                                            _selectStdValue = _selectStdValues[_selectStdIndex];
                                        }
                                    }
                                }
                                catch { }
                            %>
                            <td style="width: 10%">
                                <select name="std<%=standard.Id %>" id="std<%=standard.Id %><%=_index %>">
                                    <option value="">请选择</option>
                                    <%foreach(var value in standard.ValueList.Split(';')) {%>
                                        <option value="<%=value %>" <% if(value == _selectStdValue) {%>selected="selected"<%} %>><%=value %></option>
                                    <%} %>
                                </select>
                            </td>
                        <%} %>
                        <td><input type="text" name="std_market_price" id="std_market_price<%=_index %>" style="width: 95%" value="<%=item.SalePrice %>" /></td>
                        <td><input type="text" name="std_sale_price" id="std_sale_price<%=_index %>" style="width: 95%" value="<%=item.SalePrice %>"/></td>
                        <td><input type="text" name="std_storage" id="std_storage<%=_index %>" style="width: 95%" value="<%=item.Storage %>"/></td>
                        <td><input type="text" name="std_product_code" id="std_product_code<%=_index %>" style="width: 95%" value="<%=item.ProductCode %>"/></td>
                        <td>
                            <a href="javascript:void(0)" onclick="delRow(this)"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>
                        </td>
                    </tr>
                <%} %>
            <%} else {%>
                <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')" rowNum="0">
                    <%foreach (var standard in standardList) {%>
                        <td style="width: 10%">
                            <select name="std<%=standard.Id %>" id="std<%=standard.Id %>0">
                                <option value="">请选择</option>
                                <%foreach(var value in standard.ValueList.Split(';')) {%>
                                    <option value="<%=value %>"><%=value %></option>
                                <%} %>
                            </select>
                        </td>
                    <%} %>
                    <td><input type="text" name="std_market_price" id="std_market_price0" style="width: 95%" /></td>
                    <td><input type="text" name="std_sale_price" id="std_sale_price0" style="width: 95%" /></td>
                    <td><input type="text" name="std_storage" id="std_storage0" style="width: 95%" value="0" /></td>
                    <td><input type="text" name="std_product_code" id="std_product_code0" style="width: 95%" /></td>
                    <td>
                        <a href="javascript:void(0)" onclick="delRow(this)"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>
                    </td>
                </tr>
            <%} %>
        </tbody>
    </table>
    <input type="button" class="button" value="复制最后行" id="btnCopy" style="margin-top:5px; width: 80px;" onclick="copyStandardLastRow();" />
    <input type="button" class="button" value="添加新行" id="btnStandard" style="margin-top:5px;" onclick="addStandardRow();" />

    <!--standard templates start-->
    <script id="tmplStd" type="text/x-jquery-tmpl">
        <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')" rowNum="${row}">
            <%foreach (var standard in standardList) {%>
                <td style="width: 10%">
                    <select name="std<%=standard.Id %>" id="std<%=standard.Id %>${row}">
                        <option value="">请选择</option>
                        <%foreach(var value in standard.ValueList.Split(';')) {%>
                            <option value="<%=value %>"><%=value %></option>
                        <%} %>
                    </select>
                </td>
            <%} %>
            <td><input type="text" name="std_market_price" id="std_market_price${row}" style="width: 95%" /></td>
            <td><input type="text" name="std_sale_price" id="std_sale_price${row}" style="width: 95%" /></td>
            <td><input type="text" name="std_storage" id="std_storage${row}" style="width: 95%" value="0" /></td>
            <td><input type="text" name="std_product_code" id="std_product_code${row}" style="width: 95%" /></td>
            <td>
                <a href="javascript:void(0)" onclick="delRow(this)"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>
            </td>
        </tr>
    </script>
    <!--standard templates end-->
<%} %>