<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="FavorableActivityGiftAdd.aspx.cs" Inherits="JWShop.Web.Admin.FavorableActivityGiftAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
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
    <div class="container ease" id="container">
    	<!--<div class="path-title"></div>-->
        <div class="product-container product-container-border product-container-mt70">
            <div class="form-row">
                <div class="head">名称：</div>
                <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="400px" CanBeNull="必填" />
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">图片：</div>
               <a <%if (!string.IsNullOrEmpty(Photo.Text)){%>href="<%=Photo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Photo.Text)%>" class="icon"  height="50" id="nailimg"/></a>
                    <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=FavorableActivityGiftBLL.TableID%>&FilePath=GiftPhoto/Original&NeedMark=0" width="300" height="40px" frameborder="0" allowTransparency="true" scrolling="no" id="uploadIFrame"></iframe>
                    </div>
                    <input class="form-cut" type="button" id="cutImage" value="裁剪图片" style="display:none;"/>
                </div>            
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">介绍：</div>
                <SkyCES:TextBox ID="Description" CssClass="txt" runat="server" Width="400px" TextMode="MultiLine" Height="80px" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
        </div>
    </div>
    <script>
        $(function () {
            //页面一打开就执行，放入ready是为了layer所需配件（css、扩展模块）加载完毕
            layer.ready(function () {
                $("#cutImage").click(function () {
                    var orgImage = $("#ctl00_ContentPlaceHolder_Photo").val();
                    if (orgImage.length > 0) {
                        layer.open({
                            type: 2,
                            //skin: 'layui-layer-lan',
                            title: '图片裁剪',
                            fix: false,
                            shadeClose: true,
                            maxmin: true,
                            area: ['900px', '500px'],
                            content: 'EditPhoto.aspx?Photo=' + orgImage + '&TableID=<%=FavorableActivityGiftBLL.TableID%>&TargetID=ctl00_ContentPlaceHolder_Photo&MakeNail=1'
                        });
                    } else {
                        layer.alert("请先上传图片再裁剪");
                    }
                });        
            });
        });
    </script>
</asp:Content>