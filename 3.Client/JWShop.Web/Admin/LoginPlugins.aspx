<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="LoginPlugins.aspx.cs" Inherits="JWShop.Web.Admin.LoginPlugins" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    
    <div class="container ease" id="container">
        <div class="product-container">
            <table class="product-list">
                <thead>
                    <tr>
                        <td style="width:10%">图片</td>
                        <td style="width:25%;">名称</td>
                        <td style="width:50%">描述</td>                      
                        <td style="width:10%">是否启用</td>
                        <td style="width:5%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>
                            <tr>
                                <td><img src="<%# Eval("Photo") %>" /></td>
		                        <td><%# Eval("Name") %></td>
		                        <td style="text-align:left;text-indent:8px;"><%# Eval("Description")%></td>		                      
                                <td><%# ShopCommon.GetBoolText(Eval("IsEnabled"))%></td> 
		                        <td class="link">
		                            <a href="LoginPluginAdd.aspx?Key=<%# Eval("Key") %>">修改</a>  
		                        </td>
                            </tr>
	                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>