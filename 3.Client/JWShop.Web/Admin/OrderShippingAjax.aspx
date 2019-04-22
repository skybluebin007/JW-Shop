<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderShippingAjax.aspx.cs" Inherits="JWShop.Web.Admin.OrderShippingAjax" %>
<%@ Import Namespace="JWShop.Entity" %>
<select name="ShippingID">
    <%foreach(ShippingInfo shipping in shippingList){%>    
    <option value="<%=shipping.Id%>" <%if(orderShippingId==shipping.Id){ %>selected="selected"<%} %>><%=shipping.Name%></option>
    <%} %>
</select>