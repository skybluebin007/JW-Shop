<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Flash.aspx.cs" Inherits="JWShop.Web.Admin.Flash" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <div class="container ease" id="container">
        <asp:Label Text="" ID="lab_sty"  runat="server" Visible="False" />
        <div class="product-container">
        
            <table class="product-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
                    <td style="width: 8%">
                            Id
                        </td>
                        <td style="width: 22%;">
                            广告位标题
                        </td>                  
                        <td style="width: 35%">
                            说明
                        </td>
                        <td style="width: 15%">
                            建议宽高
                        </td>
                       <td style="width: 20%">
                            管理
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                        
                               <tr class="listTableMain" onmousemove="changeColor(this,'#f1f1f1')" onmouseout="changeColor(this,'#FFF')">
                                    <td><%# Eval("ID")%></td>
                                    <td><%# Eval("Title")%></td>
                                    <td title="<%#Eval("Introduce") %>"><%# StringHelper.Substring(Eval("Introduce").ToString(),25)%></td>
                                    <td><%# Eval("Width")%> X <%# Eval("Height")%></td>
                                    <td>
                                        <a href="FpImage.aspx?fp_type=<%# Eval("ID")%>" class="ig-colink">广告图片</a> 
                                        &nbsp;&nbsp;&nbsp;
                                        <a href="FlashAdd.aspx?ID=<%# Eval("ID")%>" class="ig-colink">修改</a>
                                        <a style="display:none;" href="?Action=Delete&Id=<%# Eval("Id") %>" onclick="return check()" class="ig-colink">删除</a>  
                                    </td>
                                </tr>
                           
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="5">
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>
