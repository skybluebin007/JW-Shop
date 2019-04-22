<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadImage.aspx.cs" Inherits="JWShop.Web.Admin.UploadImage" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>附件上传</title>
    <link href="/Admin/Style/style.css" type="text/css" rel="stylesheet" media="all" /> 
    <style type="text/css">
        .upfile a{display:inline-block; width:110px; height:28px; background-image:url(/Plugins/Template/Default/images/btn_uploadphoto.jpg); position:relative; overflow:hidden; float:left}
        .upfile input{position:absolute; right:0; top:0; font-size:100px; opacity:0; filter:alpha(opacity=0);}
    </style>

    <script type="text/javascript">
        function ShowImage(path) {
           document.getElementById("UploadButton").click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="upfile">
             <a href="#">
                <asp:FileUpload ID="UploadFile" onchange="ShowImage(this.value)" runat="server" />&nbsp;<asp:Button CssClass="button" style="display:none" ID="UploadButton" Text=" 上 传 "  runat="server"  OnClick="Upload"/>
            </a>
        </div>
    </form>
</body>
</html>