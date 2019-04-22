<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left.aspx.cs" Inherits="JWShop.Web.Admin.Left" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>左侧菜单</title>
    <link rel="stylesheet" href="/Admin/static/css/main.min.css" />
</head>
<body>
    <div class="menubar ease" id="menubar">
        <div class="main">
            <%foreach (MenuInfo menu in menuList){ %>
            <h2 class="topclass"><%=menu.MenuName %></h2>
            <dl class="menu">
                <%foreach (MenuInfo tempMenu in MenuBLL.ReadMenuChildList(menu.ID)){ %>                
                    <dd class="ease ">
                        <a href="javascript:goUrl('<%=tempMenu.URL %>')" title="<%=tempMenu.MenuName%>">
                            <span><%=tempMenu.MenuName%></span>
                        </a>
                    </dd>
                <%} %>
            </dl>
            <%} %>
        </div>
        <div class="menu-pointer" id="menu-pointer"></div>
    </div>

    <script type="text/javascript">
        function goUrl(url) {
            top.window.o("RightFrame").src = url;
        }
    </script>
</body>
</html>
