﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadAdd.aspx.cs" Inherits="JWShop.Web.Admin.UploadAdd" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>附件上传</title>
<link rel="stylesheet" href="/Admin/static/css/main.min.css" />
<script src="Js/jquery-1.7.2.min.js"></script>
<script type="text/javascript">
function ShowImage(path) {
$("#UploadButton").click();
}
</script>
</head>
<body>
<form id="form1" runat="server">

        <asp:FileUpload ID="UploadFile" CssClass="uploadFile form-file" onchange="ShowImage(this.value)" runat="server" />
        &nbsp;
        <asp:Button CssClass="button" ID="UploadButton" Text=" 上 传 " style="display:none;"  runat="server"  OnClick="UploadImage"/>

</form>
</body>
</html>
