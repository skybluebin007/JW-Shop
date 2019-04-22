<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ArticleClassAdd.aspx.cs" Inherits="JWShop.Web.Admin.ArticleClassAdd" %>

<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script type="text/javascript" src="/Admin/Js/ProductAdd.js"></script>
    <link rel="stylesheet" href="/Admin/kindeditor/themes/default/default.css" />
    <link rel="stylesheet" href="/Admin/kindeditor/plugins/code/prettify.css" />
    <script type="text/javascript" src="/Admin/kindeditor/kindeditor.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/plugins/code/prettify.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/kindeditor-content.js"></script>
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <script>
        var KE;
        $(document).ready(function () {
            KindEditor.ready(function (K) {

                KE = K.create('#ctl00_ContentPlaceHolder_AddCol2', {
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
        <div class="path-title">
        </div>
        <div class="product-container product-container-border" style="margin-top: 30px;">
            <div class="form-row">
                <div class="head">
                    上级分类：</div>
                <asp:DropDownList ID="FatherID" runat="server" CssClass="select" />
            </div>
            <div class="form-row">
                <div class="head">
                    分类名称：</div>
                <SkyCES:TextBox ID="ClassName" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" />
            </div>
            <div class="form-row"  style="display:none;">
                <div class="head">
                    内容类型：</div>
                <asp:DropDownList ID="ShowType" CssClass="select" runat="server">
                    <asp:ListItem Value="2">文章列表</asp:ListItem>
                    <asp:ListItem Value="1">单文章</asp:ListItem>
                    <asp:ListItem Value="3">图片列表</asp:ListItem>
                    <asp:ListItem Value="4">父级分类</asp:ListItem>
                    <asp:ListItem Value="5">链接URL</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-row" style="display:none;">
                <div class="head">
                    显示终端：</div>
                <asp:DropDownList ID="ShowTerminal" runat="server" CssClass="select">
                    <asp:ListItem Value="2">PC</asp:ListItem>
                    <asp:ListItem Value="1">移动</asp:ListItem>
                    <asp:ListItem Value="0">全部</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-row">
                <div class="head">
                    英文名称：</div>
                <SkyCES:TextBox ID="EnClassName" CssClass="txt" runat="server" Width="300px" />
            </div>
            <div class="form-row">
                <div class="head">
                    分类图片：</div>
                <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="300px" style="display:none;"/>
            <a <%if (!string.IsNullOrEmpty(Photo.Text)){%>href="<%=Photo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Photo.Text)%>" class="icon"  height="50" id="nailimg"/></a>
            </div>
            <div class="form-row">
              <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=ArticleBLL.TableID%>&FilePath=ArticlePhoto/Original&NeedMark=0"
                        width="80" height="30" frameborder="0" allowtransparency="true" scrolling="no">
                    </iframe>
                </div>
            </div>
            <div class="form-row">
                <div class="head">
                    排序ID：</div>
                <SkyCES:TextBox ID="OrderID" CssClass="txt" runat="server" Width="300px" Text="0" CanBeNull="必填"
                    RequiredFieldType="数据校验" HintInfo="数字越小越排前" />
            </div>
            <div class="form-row"  style="display:none;">
                <div class="head">
                    链接URL：</div>
                <SkyCES:TextBox ID="Description" CssClass="txt" runat="server" Width="400px" TextMode="MultiLine" />
            </div>
            <div class="form-row"  style="display:none;">
                <div class="head">
                    摘要：</div>
                <div class="row">
                    <textarea class="input" id="AddCol2" name="AddCol2" style="width: 700px; height: 400px"
                        runat="server"></textarea></div>
            </div>
            <div class="form-row"  style="display:none;">
                <div class="head">
                    图片宽度：
                </div>
                <div class="row">
                   <SkyCES:TextBox ID="ImageWidth" CssClass="txt" runat="server" Width="400px" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" />
                    添加文章封面图片时显示的建议宽度
                </div>
            </div>
            <div class="form-row"  style="display:none;">
                <div class="head">
                    图片高度：
                </div>
                <div class="row">
                  <SkyCES:TextBox ID="ImageHeight" CssClass="txt" runat="server" Width="400px" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" />
                    添加文章封面图片时显示的建议高度
                </div>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 "
                runat="server" OnClick="SubmitButton_Click" />
        </div>
    </div>
</asp:Content>
