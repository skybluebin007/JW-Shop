<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrandAjax.aspx.cs" Inherits="JWShop.Web.Admin.BrandAjax" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

    <option value="0">请选择</option>
    <%        
        foreach (ProductBrandInfo productBrand in productBrandList)
      { %>
    <option value="<%=productBrand.Id%>"><%=productBrand.Name%></option>
    <%         
    } %>
