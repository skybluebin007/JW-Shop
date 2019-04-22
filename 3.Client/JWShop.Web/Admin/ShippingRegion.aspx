<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ShippingRegion.aspx.cs" Inherits="JWShop.Web.Admin.ShippingRegion" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server"> 
    <script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
    <div class="container ease" id="container">
    <div class="path-title"></div>
    <div class="product-container product-container-border">
        <table  class="product-list" cellpadding="0" cellspacing="0" border="0" width="100%">
            <thead>
                <tr>
                    <td width="80">ID</td>
                    <td align="left" width="200">物流区域</td>
                    <td align="left" width="200">详细区域</td>
                    <td width="100" >管理</td>
                    <td width="50">选择</td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RecordList" runat="server">
                    <ItemTemplate>
                        <tr >
                            <td ><%# Eval("ID") %></td>
                            <td align="left"><%# Eval("Name") %></td>
                            <td align="left"><%#RegionBLL.RegionNameList(Eval("RegionID").ToString())%></td>
                            <td ><a href='ShippingRegion.aspx?ID=<%# Eval("ID") %>&ShippingID=<%=ShippingId%>'><img src="Style/Images/edit.gif" alt="修改" title="修改" /></a></td>
                            <td ><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="5">
                    	<div class="button">
                            <label class="ig-checkbox">
                                <input type="checkbox" name="All" value="" class="checkall" bind="list" />
                                全选</label>
                            <input type="button"  value=" 添 加 " class="button-3"  onclick="window.location.href='ShippingRegion.aspx?ShippingID=<%=ShippingId%>'"/>
                            &nbsp;
                            <asp:Button CssClass="button-2" ID="Button1" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>
                        </div>
                        <div class="clear"></div>
                    </td>
                </tr>
            </tfoot>
        </table>
        <!--div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>物流区域<%=GetAddUpdate()%></div-->
        <br/><br/>
        <div class="form-row">
            <div class="head">名称：</div>
            <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="400px" CanBeNull="必填"/>
        </div>
        <div class="clear"></div>
        <%if(shipping.ShippingType==(int)ShippingType.Fixed){%>
        <div class="form-row">
            <div class="head">固定运费：</div>
            <SkyCES:TextBox ID="FixedMoeny" CssClass="txt" runat="server" Width="400px" Text="0" CanBeNull="必填" RequiredFieldType="金额"/>
            （元） </div>
        <div class="clear"></div>
        <%} %>
        <%if(shipping.ShippingType==(int)ShippingType.Weight){%>
        <div class="form-row">
            <div class="head">首重<%=shipping.FirstWeight %>克：</div>
            <SkyCES:TextBox ID="FirstMoney" CssClass="txt" runat="server" Width="100px" Text="0" CanBeNull="必填" RequiredFieldType="金额"/>
            （元）
        </div>
        <div class="form-row">
            <div class="head">续重<%=shipping.AgainWeight %>克：</div>
            <SkyCES:TextBox ID="AgainMoney" CssClass="txt" runat="server" Width="100px" CanBeNull="必填" RequiredFieldType="金额"/>
            （元）
        </div>
        <div class="clear"></div>
        <%} %>
        <%if(shipping.ShippingType==(int)ShippingType.ProductCount){%>
        <div class="form-row">
            <div class="head">单件商品：</div>
            <SkyCES:TextBox ID="OneMoeny" CssClass="txt" runat="server" Width="100px" CanBeNull="必填" RequiredFieldType="金额"/>
            （元）
        </div>
        <div class="form-row">
            <div class="head">加一件商品：</div>
            <SkyCES:TextBox ID="AnotherMoeny" CssClass="txt" runat="server" Width="100px" CanBeNull="必填" RequiredFieldType="金额"/>
            （元）
            <div class="clear"></div>            
        </div>
        <%} %>
        <div class="form-row">
            <div class="head">选择区域：</div>
            <SkyCES:MultiUnlimitControl ID="RegionID" runat="server"/>
        </div>
        <div class="clear"></div>
    </div>
    <div class="form-foot">
        <asp:Button CssClass="form-submit" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
    </div>
</div>
</asp:Content>
