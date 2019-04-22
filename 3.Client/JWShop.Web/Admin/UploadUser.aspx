<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadUser.aspx.cs" Inherits="JWShop.Web.Admin.UploadUser" %>

<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>附件上传</title>
    <style type="text/css">
        .form-upload { float: left; width: 80px; height: 30px; border-radius: 3px; background: #ddd; cursor: pointer; position: relative; overflow: hidden; margin-right: 10px; }
        .form-upload iframe { width: 280px; height: 30px; opacity: 0; filter: Alpha(opacity=0); cursor: pointer; position: relative; left: -90px; z-index: 2; }
        .form-upload:before { position: absolute; left: 0; top: 0; z-index: 0; width: 100%; height: 30px; line-height: 30px; text-align: center; }
        .form-upload:hover { background: #e64652; color: #FFF; }
        .form-file { height: 30px; cursor: pointer; }
    </style>

    <script type="text/javascript">
        function ShowImage(path) {
            document.getElementById("UploadButton").click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload ID="UploadFile" CssClass="uploadFile form-file" onchange="ShowImage(this.value)" runat="server" />
            &nbsp;
            <asp:Button CssClass="button" ID="UploadButton" Text=" 上 传 " Style="display: none;" runat="server" OnClick="Upload" />
        </div>
    </form>
</body>
</html>