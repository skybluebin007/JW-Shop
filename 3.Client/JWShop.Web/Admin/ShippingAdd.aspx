<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ShippingAdd.aspx.cs" Inherits="JWShop.Web.Admin.ShippingAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<div class="path-title"></div>
    	<div class="product-container product-container-border">
	        <div class="form-row">
		        <div class="head">名称：</div>
		        <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" />
	        </div>
            <div class="form-row">
		        <div class="head">公司代码：</div>
		        <SkyCES:TextBox ID="ShippingCode" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" />
	        </div>
	        <div class="form-row">
		        <div class="head">描述：</div>
		        <SkyCES:TextBox ID="Description" CssClass="text" runat="server" Width="400px" Height="100px" TextMode="MultiLine" />
	        </div>
	        <div class="form-row">
		        <div class="head">是否启用：</div>
                <div class="og-radio clearfix">
                    <label class="item <%if(shipping.Id == 0 || shipping.IsEnabled == 1){ %>checked<%} %>">是<input type="radio" name="IsEnabled" value="1" <%if(shipping.Id == 0 || shipping.IsEnabled == 1){ %>checked<%} %>></label>
                    <label class="item <%if(shipping.Id > 0 && shipping.IsEnabled == 0){ %>checked<%} %>">否<input type="radio" name="IsEnabled" value="0" <%if(shipping.Id > 0 && shipping.IsEnabled == 0){ %>checked<%} %>></label>
                </div>
	        </div>
	        <div class="form-row">
		        <div class="head">运费方式：</div>
                <div class="og-radio clearfix" id="shippingType">
                    <label class="item <%if(shipping.ShippingType == 1 || shipping.ShippingType == 0){ %>checked<%} %>">固定运费<input type="radio" name="ShippingType" value="1" <%if(shipping.ShippingType == 1 || shipping.ShippingType == 0){ %>checked<%} %>></label>
                    <label class="item <%if(shipping.ShippingType == 2){ %>checked<%} %>">按重量计算<input type="radio" name="ShippingType" value="2" <%if(shipping.ShippingType == 2){ %>checked<%} %>></label>
                    <label class="item <%if(shipping.ShippingType == 3){ %>checked<%} %>">按商品数量计算<input type="radio" name="ShippingType" value="3" <%if(shipping.ShippingType == 3){ %>checked<%} %>></label>
                </div>
	        </div>
            <div class="clear"></div>
	        <div class="" id="ShippingTypeDiv">
                <div class="form-row" >
		            <div class="head" >首重：</div>
                    <SkyCES:TextBox ID="FirstWeight" CssClass="txt" runat="server" Width="80px" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"  />（克）
                </div>
                <div class="form-row" >
		            <div class="head" >续重：</div>
                    <SkyCES:TextBox ID="AgainWeight" CssClass="txt" runat="server" Width="80px" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/>（克）
                </div>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" style=" margin:0;" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#shippingType label').click(function () {
                selectShippingType();
            });

            selectShippingType();

            function selectShippingType() {
                var shippingType = $('input[name=ShippingType]:checked').val();

                if (shippingType == "2") {
                    $('#ShippingTypeDiv').css('display', "");
                }
                else {
                    $('#ShippingTypeDiv').css('display', "none");
                }
            }
        });
    </script>
</asp:Content>
