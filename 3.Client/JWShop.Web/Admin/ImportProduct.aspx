<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ImportProduct.aspx.cs" Inherits="JWShop.Web.Admin.ImportProduct" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">   
        <style>
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
    <div class="container ease" id="container">
        <div class="path-title">
        </div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">
                    分类：</div>
                <SkyCES:SingleUnlimitControl ID="ProductClass" runat="server"></SkyCES:SingleUnlimitControl>
            </div>            
            <div class="form-row">
                <div class="head">
                    数据文件：</div>
                <asp:DropDownList ID="dropFiles" runat="server" CssClass="select" />
            </div>
            
            <div class="form-row">
                <div class="head">
                    是否上架：</div>
                <asp:RadioButtonList ID="IsSale" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                    <asp:ListItem Value="0">否</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="form-row">
                <div class="head">
                    上传数据文件：</div>
                <div id="imgUpLoad">
                <asp:FileUpload runat="server" ID="fileUploader" CssClass="forminputx"/>
                        &nbsp;&nbsp;
                        <asp:Button runat="server" ID="btnUpload" CssClass="copycss" Text="上传" OnClick="btnUpload_Click" /><br />
                        <span>上传数据包须小于50M，否则可能上传失败，<br/>
                            您还可以使用FTP工具先将数据包上传到网站的/Upload/taobao目录以后，再重新打开此页面操作。</span>
                </div>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton"
                Text=" 确 定 " runat="server" OnClick="ImportProducts" />
        </div>
    </div>
   
</asp:Content>
