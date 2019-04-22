<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Vote.aspx.cs" Inherits="JWShop.Web.Admin.Vote" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script>
        function ChangeOrder(voteId, orderId) {
            $.get("Ajax.aspx", { action: "ChangeVoteOrder", voteId: voteId, orderId: orderId }, function (data) {
                //alert(data);
            })
        }
    </script>
    <div class="container ease" id="container">
        <div class="product-container" style="padding-top: 30px;">
            <dl class="classify-menu">
                <dd class="add" onclick="location.href='VoteAdd.aspx'">添加</dd>         
                <dd class="show" id="sub-showall">展开全部</dd>
                <dd class="hide" id="sub-hideall">收起全部</dd>
            </dl>
            <table cellpadding="0" cellspacing="0" border="0" class="product-list" width="100%">
                <thead>
                    <tr>
                        <td width="50" height="40">选择</td>
                        <td width="150">标题</td>
                        <td width="" align="left" style="padding-left: 20px;">排序</td>
                        <td width="100">选项数</td>
                        <td width="250">操作</td>
                    </tr>
                </thead>
                <tbody>
                    <%
                        int topCount = 1;
                        foreach (VoteInfo topVote in   VoteBLL.ReadVoteRootList())
                        { %>

                        <tr bind="column-<%=topCount%>">
                        <td height="40"><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%=topVote.ID%>" ig-bind="list" /></label></td>
                        <td colspan="2" align="left">
                            <div class="classify-column">
                                <div class="sub sub-hide"></div>
                                <div class="title"><%=topVote.Title%></div>
                                <input type="text" placeholder="" value="<%=topVote.OrderID%>" maxlength="4" class="order" onblur="ChangeOrder(<%=topVote.ID %>,this.value)" />
                                <div class="clear"></div>
                            </div>
                        </td>
                        <td><a href="VoteItem.aspx?VoteID=<%=topVote.ID%>"><%=topVote.ItemCount%></a></td>
                        <td>

                             <a href="VoteAdd.aspx?ID=<%=topVote.ID%>" class="ig-colink" >修改</a>|
                            <a href="VoteItem.aspx?VoteID=<%=topVote.ID%>" class="ig-colink" >选项管理</a>|  
                <a href="VoteRecord.aspx?VoteID=<%=topVote.ID%>" class="ig-colink" >投票记录</a>
               
                        </td>
                    </tr>
                    <%
                            List<VoteInfo> subClassList =VoteBLL.ReadChilds(topVote.ID);
                            foreach (VoteInfo subClass in subClassList)
                            {
                    %>
                            <tr ig-bind="column-<%=topCount%>">
                                <td height="40"><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%=subClass.ID%>" ig-bind="list" /></label></td>
                                <td colspan="2" align="left">
                                    <div class="classify-column">
                                        <div class="sub"></div>
                                        <div class="title subtitle"><%=subClass.Title%></div>
                                        <input type="text" placeholder="" value="<%=subClass.OrderID%>" maxlength="4" class="order" onblur="ChangeOrder(<%= subClass.ID%>,this.value)" />
                                        <div class="clear"></div>
                                    </div>
                                </td>
                                <td><a href="VoteItem.aspx?VoteID=<%=subClass.ID%>"><%=subClass.ItemCount%></a></td>
                                <td>
                                   <a href="VoteAdd.aspx?ID=<%=subClass.ID%>" class="ig-colink" >修改</a>|
                            <a href="VoteItem.aspx?VoteID=<%=subClass.ID%>" class="ig-colink" >选项管理</a>|  
                <a href="VoteRecord.aspx?VoteID=<%=subClass.ID%>" class="ig-colink" >投票记录</a>
                                </td>
                            </tr>
                    <%
                                List<VoteInfo> thirdClassList = VoteBLL.ReadChilds(subClass.ID);
                                foreach (VoteInfo thirdClass in thirdClassList)
                                {
                    %>
                                <tr ig-bind="column-<%=topCount%>">
                                    <td height="40"><label class="ig-checkbox"><input type="checkbox" name="SelectID" value="<%=thirdClass.ID%>"  ig-bind="list" /></label></td>
                                    <td colspan="2" align="left">
                                        <div class="classify-column">
                                            <div class="sub"></div>
                                            <div class="title subtitle" style="margin-left: 90px;"><%=thirdClass.Title%></div>
                                            <input type="text" placeholder="" value="<%=thirdClass.OrderID%>" maxlength="4" class="order" onblur="ChangeOrder(<%= thirdClass.ID%>,this.value)" />
                                            <div class="clear"></div>
                                        </div>
                                    </td>
                                     <td><a href="VoteItem.aspx?VoteID=<%=thirdClass.ID%>"><%=thirdClass.ItemCount%></a></td>
                                    <td> <a href="VoteAdd.aspx?ID=<%=thirdClass.ID%>" class="ig-colink" >修改</a>|
                            <a href="VoteItem.aspx?VoteID=<%=thirdClass.ID%>" class="ig-colink" >选项管理</a>|  
                <a href="VoteRecord.aspx?VoteID=<%=thirdClass.ID%>" class="ig-colink" >投票记录</a>
                                    </td>
                                </tr>

                    <%
                            }
                            }

                            topCount++;
                        } %>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="5" align="left">
                            <div class="button">
                                <label class="ig-checkbox">
                                 <input type="checkbox" value="" class="checkall" bind="list" />全选</label>
                                <input type="button" class="button-2  submit" value="添加" onclick="location.href='VoteAdd.aspx'" />     
                                <asp:Button ID="Delete_Button" CssClass="button-2 del" runat="server" Text=" 删 除 " OnClientClick="return checkSelect();" OnClick="DeleteButton_Click" />
                                <asp:Button ID="Button1" CssClass="button-2 del" runat="server" Text="投票结果ToExcel" OnClick="BtnTOExcel_Click" />
                            </div>
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>

        </div>
    </div>
</asp:Content>
