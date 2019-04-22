<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderAdd.aspx.cs" Inherits="JWShop.Web.Admin.OrderAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<script type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
<style>
body { min-width:100%; width:100%; }
</style>
    <asp:PlaceHolder ID="Shipping" runat="server" Visible="false">
	    
            <div class="layer-container">
                <div class="form-row">
                    <div class="head">收货人：</div>
                    <SkyCES:TextBox CssClass="txt" Width="300px" ID="Consignee" runat="server" CanBeNull="必填" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">收货地区：</div>
                    <SkyCES:SingleUnlimitControl ID="RegionID" runat="server" FunctionName="readShippingID()" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">收货地址：</div>
                    <SkyCES:TextBox ID="Address" CssClass="txt" runat="server" Width="300px" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">邮编：</div>
                    <SkyCES:TextBox ID="ZipCode" CssClass="txt" runat="server" Width="300px" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">联系电话：</div>
                    <SkyCES:TextBox ID="Tel" CssClass="txt" runat="server" Width="300px" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">Email：</div>
                    <SkyCES:TextBox ID="Email" CssClass="txt" runat="server" Width="300px" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">手机：</div>
                    <SkyCES:TextBox ID="Mobile" CssClass="txt" runat="server" Width="300px" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">配送方式：</div>
                    <div id="ShippingAjax"></div>
                </div>
                <div class="clear"></div>
	            <asp:PlaceHolder ID="ShippingInfo" runat="server" Visible="false">
                    <div class="form-row">
                        <div class="head">配送日期：</div>
                        <SkyCES:TextBox ID="ShippingDate" CssClass="txt" runat="server" Width="300px" />
                    </div>
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">配送方式：</div>
                        <SkyCES:TextBox ID="ShippingNumber" CssClass="txt" runat="server" Width="300px" />
                    </div>
                    <div class="clear"></div>
	            </asp:PlaceHolder>
                <div class="form-row">
                    <div class="head"></div>
                    <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton0" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
                </div>
            </div>
        
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="Other" runat="server" Visible="false">
        
            <div class="layer-container">
                <div class="form-row">
                    <div class="head">用户留言：</div>
                    <asp:Label ID="UserMessage" runat="server"/>
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">订单备注：</div>
                    <SkyCES:TextBox ID="OrderNote" CssClass="txt" runat="server" Width="300px" TextMode="MultiLine" Height="100px" />
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head"></div>
                    <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton1" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
                </div>
            </div>
        
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="Money" runat="server" Visible="false">
        
            <div class="layer-container">
                <div class="form-row">
                    <div class="head">产品金额：</div>
                    <asp:Label ID="ProductMoney" runat="server" />  元 
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">运费：</div>
                    <asp:Label ID="ShippingMoney" runat="server"/> 元 
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">积分抵扣金额：</div>
                    <asp:Label ID="PointMoney" runat="server"/> 元 
                </div>
                <div class="clear"></div>
                <div class="form-row" style="display:none;">
                    <div class="head">余额：</div>
                    <asp:Label ID="Balance" runat="server"/> 元 
                </div>
                <div class="form-row">
                    <div class="head">优惠券：</div>
                    <asp:Label ID="CouponMoney" runat="server"/> 元 
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">优惠活动：</div>
                    <asp:Label ID="FavorableMoney" runat="server" />
                    元 
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head">其它费用：</div>
                    <SkyCES:TextBox ID="OtherMoney" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" RequiredFieldType="金额" /> 元 <br /> 后台调节订单费用项，“+”表示增加订单费用，“-”表示减去订单费用
                </div>
                <div class="clear"></div>
                <div class="form-row">
                    <div class="head"></div>
                    <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton2" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
                </div>
            </div>
       
	</asp:PlaceHolder>	
    
    <script language="javascript" type="text/javascript">
    var shippingID="<%=order.ShippingId %>";
    //读取配送方式
        function readShippingID() {
            var url = "OrderShippingAjax.aspx?RegionID=" + readSearchClassID("") + "&ShippingID=" + shippingID;
            Ajax.requestURL(url, dealReadShippingID);
        }
    function dealReadShippingID(data) {
        o("ShippingAjax").innerHTML = data;
    }
    if (o("ShippingAjax") != null) {
        readShippingID();
    }
    </script>
</asp:Content>
