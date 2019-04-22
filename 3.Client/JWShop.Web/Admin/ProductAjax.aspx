<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductAjax.aspx.cs" Inherits="JWShop.Web.Admin.ProductAjax" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
 <select name='<%=NamePrefix %><%=controlName %>' id='<%=IDPrefix%><%=controlName %>' <%=cssContent %> <%=dobuleClickContent %>>
 <%if ("SearchProductAccessory,SearchRelationProduct,SearchProductByName".IndexOf(action) > -1)
   {
        foreach (ProductInfo product in productList){ %>
            <option value="<%=product.Id %>"><%=product.Name %></option>
        <%}
    }
   else if (action == "SearchRelationArticle")
   {
       foreach (ArticleInfo article in articleList){ %>
            <option value="<%=article.Id %>"><%=article.Title%></option>
      <%}
   } 
    else if (action == "SearchUser")
   {
       foreach (UserInfo user in userList){ %>
            <option value="<%=user.Id %>|<%=user.UserName%>"><%=user.UserName%></option>
      <%}
   }
%>
</select>