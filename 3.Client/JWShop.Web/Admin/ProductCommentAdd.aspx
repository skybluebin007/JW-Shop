<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductCommentAdd.aspx.cs" Inherits="JWShop.Web.Admin.ProductCommentAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
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
         .msgimgbox li { float:left;                    }
    </style>
    <div class="container ease" id="container">
        <div class="path-title">
        	<!--<h2>商品评论审核</h2>-->
        </div>
        <div class="product-container product-container-border">
            <div class="product-row">
                <div class="form-row">
                    <div class="head">商品名称：</div>
                    <asp:HyperLink ID="Name" runat="server"></asp:HyperLink>
                </div>
                <div class="form-row">
                    <div class="head">评论内容：</div>
                    <asp:Label ID="Content" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">评论IP：</div>
                    <asp:Label ID="UserIP" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">评论时间：</div>
                    <asp:Label ID="PostDate" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">分数：</div>
                    <asp:Label ID="Rank" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">用户：</div>
                    <asp:Label ID="UserName" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">图片：</div>
                    <ul class="msgimgbox clearfix" style="margin-left:12px;">
                    <%
                        var prophotolist = ProductPhotoBLL.ReadList(productComment.Id, 3);
                        foreach (var item in prophotolist)
                        {%>
                        <li class="productPhoto">
                            <a href="<%=item.ImageUrl%>" target="_blank"> <img src="<%=item.ImageUrl.Replace("Original", "75-75")%>" alt="" onload="photoLoad(this,90,90)" /> </a>
                        </li>
                    <%} %>
                        </ul>
                </div>
                <div class="form-row">
                    <div class="head">状态：</div>
                    <asp:RadioButtonList ID="Status" runat="server" RepeatDirection="Horizontal">
                       <%-- <asp:ListItem Value="1" Selected="True">未处理</asp:ListItem>--%>
                        <asp:ListItem Value="2">显示</asp:ListItem>
                        <asp:ListItem Value="3">不显示</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="form-row">
                    <div class="head">管理员回复 ：</div>
                    <SkyCES:TextBox ID="AdminReplyContent" CssClass="text" runat="server" Width="400px"  TextMode="MultiLine" Height="100px" MaxLength="100"/>
                </div>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" style=" margin:0;" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
        </div>
    </div>
</asp:Content>
