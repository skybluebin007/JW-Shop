<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="VoteItemAdd.aspx.cs" Inherits="JWShop.Web.Admin.VoteItemAdd" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
        <style>
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <link rel="stylesheet" href="/Admin/kindeditor/themes/default/default.css" />
    <link rel="stylesheet" href="/Admin/kindeditor/plugins/code/prettify.css" />
    <script type="text/javascript" src="/Admin/kindeditor/kindeditor.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/plugins/code/prettify.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/kindeditor-content.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>

    <script>
        var KE, KE1;
        $(document).ready(function () {
            KindEditor.ready(function (K) {

                KE = K.create('#ctl00_ContentPlaceHolder_Content', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                KE1 = K.create('#ctl00_ContentPlaceHolder_MobileContent', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                prettyPrint();
            });
        });


    </script>
    <div class="container ease" id="container">
        <!--<div class="path-title">
        </div>-->
        <div class="product-container product-container-border product-container-mt70">
            <div class="form-row">
                <div class="head">
                    投票类别：</div>
                <asp:DropDownList ID="VoteType" runat="server" CssClass="select" />
            </div>
            <div class="form-row">
                <div class="head">
                    选项名称：</div>
                <SkyCES:TextBox ID="ItemName" CssClass="txt" MaxLength="12" runat="server" Width="400px" CanBeNull="必填" />
            </div>
            <div class="form-row" style="display:none;">
                <div class="head">
                    解决方案：</div>
                <SkyCES:TextBox ID="Solution" CssClass="txt" runat="server" Width="400px" />
            </div>       
            <div class="form-row">
                <div class="head">
                    排序号：</div>
                <SkyCES:TextBox ID="OrderID" CssClass="txt" runat="server" Width="100px" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />
            </div>
       <div class="form-row">
                <div class="head">
                    是否显示：</div>
                <asp:RadioButtonList ID="IsShow" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                    <asp:ListItem Value="0">否</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="form-row" style="display:none;">
                <div class="head">
                    网站类型：</div>
                <SkyCES:TextBox ID="Point" CssClass="txt" runat="server" Width="400px" />
            </div>
            <div class="form-row" style="display: none;">
                <div class="head">
                    预计工期：</div>
                <SkyCES:TextBox ID="CoverDepartment" CssClass="txt" runat="server" Width="400px" />
            </div>
            <div class="form-row">
                <div class="head">
                    图片：</div>
                 <a <%if (!string.IsNullOrEmpty(Photo.Text)){%>href="<%=Photo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Photo.Text)%>" class="icon"  height="50" id="nailimg"/></a>
                <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
            </div>
            <div class="form-row">
                <div class="head">
                    上传图片：</div>
                <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=ArticleBLL.TableID%>&FilePath=ArticlePhoto/Original&NeedMark=0"
                        width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no">
                    </iframe>
                </div>
             
            </div>
      
            <div class="form-row" style="display:none;">
                <div class="head">
                    关键字：</div>
                <SkyCES:TextBox ID="Keywords" CssClass="txt" runat="server" Width="400px" />
            </div>
            <div class="form-row">
                <div class="head">
                    简介：</div>
                <SkyCES:TextBox ID="Department" CssClass="text" runat="server" Width="690px" TextMode="MultiLine"
                    Height="" />
            </div>
            <div class="form-row" style="float: none;">
                <div class="head">
                    内容：</div>
                <div class="content-box" id="ig-tab">
                    <ul class="tab clearfix">
                        <li class="cur">PC端</li>
                        <li>移动端</li>
                    </ul>
                    <div class="main" style="border: 0;">
                        <div class="row">
                            <textarea class="input" id="Content" name="Content" style="width: 700px; height: 400px"
                                runat="server"></textarea></div>
                        <div class="row hidden">
                            <textarea class="input" id="MobileContent" name="Content" style="width: 640px; height: 400px"
                                runat="server"></textarea></div>
                    </div>
                </div>
            </div>


        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
        </div>
    </div>


</asp:Content>