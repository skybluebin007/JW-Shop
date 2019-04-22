<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="OderPayDiscount.aspx.cs" Inherits="JWShop.Web.Admin.OderPayDiscount" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <style>
        .form-row table { box-shadow: none; }
        .form-row table td { border: none; }
    </style>


    <script language="javascript" type="text/javascript" src="/Admin/Js/Color.js"></script>
    <div class="container ease" id="container">

        <div class="product-container product-container-border">
            <div class="product-main">

                <div class="form-row">
                    <div class="head">
                        满立减：
                    </div>
                    <div class="og-radio">
                        <label og-show="paydiscount" class="item  <%if (ShopConfig.ReadConfigInfo().PayDiscount == 1)
                            { %>checked<%}%>">
                            开启<input type="radio" name="ctl00$ContentPlaceHolder$PayDiscount" value="1" <%if (ShopConfig.ReadConfigInfo().PayDiscount == 1)
                                  { %>checked<%}%> /></label>
                        <label og-hide="paydiscount" class="item <%if (ShopConfig.ReadConfigInfo().PayDiscount == 0)
                            { %>checked<%}%>">
                            关闭<input type="radio" name="ctl00$ContentPlaceHolder$PayDiscount" value="0" <%if (ShopConfig.ReadConfigInfo().PayDiscount == 0)
                                  { %>checked<%}%> /></label>
                        开启后，单笔订单购买商品金额达到规定额度，支付可享受指定额度的优惠立减
                    </div>
                </div>
                <div class=" <%if (ShopConfig.ReadConfigInfo().PayDiscount == 0)
                    { %>hidden<%}%>"
                    id="paydiscount">
                    <div class="form-row">
                        <div class="head">
                            商品金额满：
                        </div>
                        <SkyCES:TextBox CssClass="txt" Width="100px" ID="OrderMoney" runat="server" MaxLength="6"
                            Text="0" RequiredFieldType="金额" />元
                    </div>
                    <div class="form-row">
                        <div class="head">
                            优惠(减)：
                        </div>
                        <SkyCES:TextBox CssClass="txt" Width="100px" ID="OrderDisCount" runat="server" MaxLength="6"
                            Text="0" RequiredFieldType="金额" />元
                    </div>
                </div>

            </div>
        </div>
         <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" 
                OnClick="SubmitButton_Click" />
        </div>
    </div>
    <link rel="stylesheet" href="/Admin/static/css/plugin.css" />

</asp:Content>
