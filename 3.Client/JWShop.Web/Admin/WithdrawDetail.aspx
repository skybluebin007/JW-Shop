<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="WithdrawDetail.aspx.cs" Inherits="JWShop.Web.Admin.WithdrawDetail" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
        <div class="product-container product-container-border">

            <div class="formData clearfix">
                <div class="form-row">
                <div class="head">状态：</div>
                <span style="font-size:22px;color:red;"><%= EnumHelper.ReadEnumChineseName<Withdraw_Status>(entity.Status) %></span>
            </div>
            <div class="form-row">
                <div class="head">申请人：</div>
                <%= entity.UserName %>
            </div>
            <div class="form-row">
                <div class="head">真实姓名：</div>
                <%= entity.RealName %>
            </div>
            <div class="form-row">
                <div class="head">手机：</div>
                <%= entity.Mobile %>
            </div>
            <div class="form-row">
                <div class="head">申请提现金额：</div>
                <span style="font-size:18px;color:red;"><%= entity.Amount %></span>
            </div>
            <div class="form-row">
                <div class="head">申请时间：</div>
                <%= entity.Time %>
            </div> 
             </div>

            <div id="orderinfo_bottom" style="margin-top:60px;">
                <div class="product-row" id="">
                    <%if (entity.Status==(int)Withdraw_Status.Apply) { %>
                  
                    <div class="product-main">
                          <div class="form-row" style="padding-left: 90px;">
                          <div class="head" style="width: 80px;font-weight:bold;font-size:22px;">操作</div>    
                          </div>
                        <div class="form-row" style="padding-left: 90px;">
                            <div class="head" style="width: 80px;">备注：</div>
                            <SkyCES:TextBox CssClass="txt" Width="400px" ID="Note" runat="server" TextMode="MultiLine" MaxLength="50" Height="50px" />
                        </div>
                        <div class="clear"></div>
                        <div class="form-row">
                            <div class="head"></div>
                            <% if (distributor.Total_Commission - distributor.Total_Withdraw < 0)
                                { %>
                            <p class="red">当前分销商当前剩余提现额度:[<%=(distributor.Total_Commission - distributor.Total_Withdraw) %>]，低于本次申请提现金额</p>
                            <%}
                            else
                            { %>
                            <asp:Button CssClass="button" ID="ApproveButton" Style="font-size: 12px; line-height: 12px; margin-right: 5px; padding: 8px 16px; border: 1px solid #dddddd; background: #f7f7f7; cursor: pointer;"
                                runat="server" Text="提现完成" OnClick="ApproveButton_Click" OnClientClick="return confirm('确认已经完成线下提现操作？')" />
                            <%} %>
                            <asp:Button CssClass="button" ID="RejectButton" Style="font-size: 12px; line-height: 12px; margin-right: 5px; padding: 8px 16px; border: 1px solid #dddddd; background: #f7f7f7; cursor: pointer;"
                                runat="server" Text="拒绝提现" OnClick="RejectButton_Click" OnClientClick="return confirm('确认拒绝该提现申请？')" />
                        </div>
                    </div>
                    <%} %>
                    <div class="product-main">
                        <%if (actions.Count > 0)
                            { %>
                        <div class="clear"></div>
                        <div class="head" style="width: 100px;margin-bottom:12px;font-weight:bold;font-size:16px;">操作记录</div> 
                        <table class="product-list" style="margin-top: 5px;">
                            <thead>
                                <tr class="tableHead">
                                    <td style="width: 18%">操作人</td>
                                    <td style="width: 10%">操作</td>
                                    <td style="width: 50%">备注</td>
                                    <td style="width: 22%">处理时间</td>
                                </tr>
                            </thead>
                            <tbody>
                                <%foreach (var item in actions)
                                    { %>
                                <tr class="tableMain">
                                    <td><%= item.Operator_Name %></td>
                                    <td><%= EnumHelper.ReadEnumChineseName<Withdraw_Operate>(item.Operate) %>
                                    </td>
	                    <td><%= item.Note %></td>
                                    <td><%= item.Time %></td>
                                </tr>
                                <%} %>
                            </tbody>
                        </table>
                        <%} %>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
