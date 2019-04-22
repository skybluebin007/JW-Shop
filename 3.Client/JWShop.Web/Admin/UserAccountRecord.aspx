<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserAccountRecord.aspx.cs" Inherits="JWShop.Web.Admin.UserAccountRecord" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

       <div class="container ease" id="container" >
           <div class="tab-title">
               <span class="cur">积分明细</span>
           </div>
    	
           <div class="product-container">

<dl class="product-filter clearfix" style="float: none; margin-bottom: 0;">
     <dd>
        会员：<%=user.UserName%> 
    </dd>
    <dd>
        可用积分：<%=user.PointLeft%>   <%--；已消费金额：<%=user.MoneyUsed %> 元 ；账户余额：<%=user.MoneyLeft %> 元--%>
    </dd>
</dl>

               <table class="product-list" cellpadding="0" cellpadding="0">
                   <thead>
                       <tr>
                           <td style="width: 5%">ID</td>
                           <td style="width: 20%; text-align: left; text-indent: 8px;">时间</td>
                           <td style="width: 35%">备注</td>

                           <td style="width: 10%">收入</td>
                           <td style="width: 10%">支出</td>
                           <td style="width: 10%">剩余</td>
                       </tr>
                   </thead>
                   <tbody>
                       <tr class="firstH">
                           <td colspan="6" style="padding: 0;">
                               <div class="button">
                                   <%if (Count > 0)
                                     { %>
                                   <input type="button" value="调整积分" class="button-2" onclick="window.location.href='UserAccountRecordAdd.aspx?UserID=<%=userID %>    '" />
                                   <input type="button" value="返回用户列表" class="button-2" onclick="window.location.href='User.aspx'" />                                  
                                   <%} %>
                                   <span style="float: left;">共找到<%=Count %>条记录<%if (Count > 0)
                                                                                 { %> ，<%=MyPager.PageCount %>页<%} %></span>
                               </div>
                           </td>
                       </tr>
                       <asp:Repeater ID="RecordList" runat="server">
                           <ItemTemplate>

                               <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                                   <td style="width: 5%"><%# Eval("Id") %></td>
                                   <td style="width: 20%; text-align: left; text-indent: 8px;"><%#Eval("Date")%></td>
                                   <td style="width: 35%; text-align: left;"><%#Eval("Note")%></td>

                                   <td style="width: 10%"><%#Convert.ToInt32(Eval("Point"))>0?Eval("Point"):"" %>&nbsp;</td>
                                   <td style="width: 10%"><%#Convert.ToInt32(Eval("Point"))<0?Eval("Point"):"" %>&nbsp;</td>
                                   <td style="width: 10%"><%#ShowPointLeft(Eval("Id"),Eval("Point"))%></td>
                               </tr>

                           </ItemTemplate>
                       </asp:Repeater>
                   </tbody>
                   <tfoot>
                       <tr>
                           <td colspan="6">
                               <div class="button">
                                   <input type="button" value="调整积分" class="button-2" onclick="window.location.href='UserAccountRecordAdd.aspx?UserID=<%=userID %>    '" />
                                   <input type="button" value="返回用户列表" class="button-2" onclick="window.location.href='User.aspx'" />

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
