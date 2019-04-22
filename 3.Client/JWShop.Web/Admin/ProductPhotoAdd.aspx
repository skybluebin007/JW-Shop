<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductPhotoAdd.aspx.cs" Inherits="JWShop.Web.Admin.ProductPhotoAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>产品图片上传</title>
    <link rel="stylesheet" href="/Admin/static/css/main.min.css" />
    <script src="Js/jquery-1.7.2.min.js"></script>
<script type="text/javascript">
    function ShowImage(path) {
        $("#SubmitButton").click();
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <label for="UploadFile" class="ig-multigraph-button">
    <asp:FileUpload ID="UploadFile" runat="server"  onchange="ShowImage(this.value)"/>
        &nbsp;
        <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="100px" style="display:none" />
        &nbsp;
        <asp:Button CssClass="button" style="display:none;" ID="SubmitButton" Text=" 上 传 " runat="server"  OnClick="SubmitButton_Click" />
            </label>
    </form>
</body>
</html>
