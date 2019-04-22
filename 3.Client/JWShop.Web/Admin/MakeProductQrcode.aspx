<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPage.Master"  CodeBehind="MakeProductQrcode.aspx.cs" Inherits="JWShop.Web.Admin.MakeProductQrcode" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <div class="container ease" id="container">
        <div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <span id="" style="color:red;float:right;font-size: 18px;">重新生成本站所有产品二维码,请谨慎操作!</span> 
                </div>
            <div class="form-row">
             <asp:Button ID="submit" runat="server" Text="确 定" style="height: 30px;background: #e64652;color: #fff;border-radius: 1px;border: 0;font-size: 16px;cursor: pointer;padding:0 5px;" OnClick="submit_Click" />   
                     
                </div>

        </div>
    </div>

</asp:Content>
