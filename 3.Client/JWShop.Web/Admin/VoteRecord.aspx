<%@ Page Language="C#"  MasterPageFile="MasterPage.Master"  AutoEventWireup="true" CodeBehind="VoteRecord.aspx.cs" Inherits="JWShop.Web.Admin.VoteRecord" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 20px;">	
        
            <table class="product-list">
                <thead>
                    <tr>       
	                    <td>选择</td>
	                    <td>ID</td>
	                    <td>选项名称</td>
	                    <td>用户</td> 
                         <td>时间</td> 
	                    <td>IP</td>  	                      
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>	     
                            <tr>
                                <td><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" ig-bind="list" /></label></td> 	        
		                        <td><%# Eval("ID") %></td>
		                        <td><%#VoteItemBLL.ReadItemName(Eval("ItemID").ToString(), voteItemList)%></td>
		                        <td><%#Eval("UserName")%></td> 
                                <td><%#Eval("AddDate")%></td>
                                <td><%# Eval("UserIP")%></td>
	                            
	                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6">
                            <div class="button">
	                            <label class="ig-checkbox"><input type="checkbox" name="All"  class="checkall" bind="list"/>全选</label>                                     
                                <asp:Button CssClass="button-2 del" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>                                
                            </div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />                            
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>

</asp:Content>
