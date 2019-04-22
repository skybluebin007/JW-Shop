<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="BookingProductAdd.aspx.cs" Inherits="JWShop.Web.Admin.BookingProductAdd" %>

<%@ Import Namespace="JWShop.Common" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
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
        <div class="product-container product-container-border product-container-mt70">
            <div class="form-row">
                <div class="head">产品名称：</div>
                <%=bookingProduct.ProductName%>
            </div>
            <div class="form-row">
                <div class="head">联系人：</div>
                <%=bookingProduct.RelationUser %>
            </div>
            <div class="form-row">
                <div class="head">Email：</div>
                <%=bookingProduct.Email %>
            </div>
            <div class="form-row">
                <div class="head">联系电话：</div>
                <%=bookingProduct.Tel %>
            </div>
            <div class="form-row">
                <div class="head">用户备注：</div>
                <%=bookingProduct.UserNote %>
            </div>
            <div class="form-row">
                <div class="head">登记时间：</div>
                <%=bookingProduct.BookingDate.ToString("yyyy-MM-dd") %>
            </div>
            <div class="form-row">
                <div class="head">IP：</div>
                <%=bookingProduct.BookingIP %>
            </div>
            <div class="form-row">
                <div class="head">用户名：</div>
                <%if (bookingProduct.UserID == 0) { ResponseHelper.Write("匿名用户"); } else { ResponseHelper.Write(bookingProduct.UserName); } %>
            </div>
            <div class="form-row">
                <div class="head">是否处理：</div>
                <asp:RadioButtonList ID="IsHandler" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">否</asp:ListItem>
                    <asp:ListItem Value="1">是</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="form-row">
                <div class="head">处理备注：</div>
                <SkyCES:TextBox ID="HandlerNote" CssClass="input" runat="server" TextMode="MultiLine" Width="400px" Height="50px" />
            </div>            
        </div>
        <div class="form-foot">
                <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
            </div>
    </div>
</asp:Content>
