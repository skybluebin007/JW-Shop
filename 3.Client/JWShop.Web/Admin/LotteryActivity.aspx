<%@ Page Language="C#"  MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="LotteryActivity.aspx.cs" Inherits="JWShop.Web.Admin.LotteryActivity" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <div class="container ease" id="container">
     
        <div class="product-container">
        <div class="add-button"><a href="LotteryActivityAdd.aspx?type=<%=type %>" class="ease">添加<%=EnumHelper.ReadEnumChineseName<LotteryActivityType>(type) %></a></div>
            <div class="clear"></div>
            <table class="product-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
                    <td style="width: 8%">
                            Id
                        </td>
                        <td style="width: 22%;">
                           活动名称
                        </td>                  
                        <td style="width: 20%">
                           活动关键字
                        </td>
                        <td style="width: 15%">
                           开始时间
                        </td>
                        <td style="width: 15%">
                           结束时间
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
                                    <td><%# Eval("Id")%></td>
                                    <td><%# Eval("ActivityName")%>
                                        <br />
                                        <a href="<%#GetUrl(Eval("Id")) %>" target="_blank"><%#GetUrl(Eval("Id")) %></a>
                                    </td>
                                    <td title="<%#Eval("ActivityKey") %>"><%# StringHelper.Substring(Eval("ActivityKey").ToString(),25)%></td>
                                    <td><%# Eval("StartTime")%> </td>
                                   <td><%# Eval("EndTime")%> </td>
                                    <td>
                                        <a href="LotteryActivityAdd.aspx?id=<%# Eval("Id")%>&type=<%# Eval("ActivityType")%>" class="ig-colink">修改</a> 
                                        &nbsp;&nbsp;&nbsp;
                                        <a href="FlashAdd.aspx?ID=<%# Eval("ID")%>" class="ig-colink">中奖纪录</a>
                                        <a style="display:none;" href="?Action=Delete&Id=<%# Eval("Id") %>" onclick="return check()" class="ig-colink">删除</a>  
                                    </td>
                                </tr>
                           
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6">
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</asp:Content>