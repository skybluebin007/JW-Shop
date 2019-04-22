<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserAccountRecordAdd.aspx.cs" Inherits="SocoShop.Web.Admin.UserAccountRecordAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <style>
.button-2 {
    float: left;
    height: 28px;
    line-height: 26px;
    padding: 0 15px;
    background: #BBB;
    border: 1px solid #BBB;
    margin-right: 5px;
    color: #FFF;
    cursor: pointer;
}
.button-2:hover {
    box-shadow: 0 0 0 1000px #888 inset;
    border-color: #888;
}
    </style>
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur" style="width: 120px;">调整积分</span>          
        </div>
        <div class="product-container">
            <div class="form-row">
                <div class="head">
                    会员：
                </div>
                <%=user.UserName %>
            </div>
            <div class="form-row">
                <div class="head">
                    可用积分：
                </div>
               <%=user.PointLeft%>
            </div>
            

<%--    <div class="form-row">
        <div class="head">账户数额：</div>
        <SkyCES:TextBox ID="Money" CssClass="txt" runat="server" Width="100px" CanBeNull="必填" MaxLength="8" RequiredFieldType="数据校验" />
        <span class="red">如果是减少用户余额，金额前面必须带上“-”</span>
    </div>--%>
    <div class="form-row">
        <div class="head">积分数额：</div>
        <SkyCES:TextBox ID="Point" CssClass="txt" runat="server" Width="100px" CanBeNull="必填" MaxLength="8" RequiredFieldType="数据校验" />
        <span class="red">正数加，负数减</span>
    </div>
   
    <div class="form-row">
        <div class="head">调整备注：</div>
        <SkyCES:TextBox ID="Note" CssClass="txt" runat="server" Width="400px" CanBeNull="必填" Height="50px" />

    </div>
    <div class="clear"></div>


    <div class="form-row">
        <div class="head2">&nbsp;</div>
        <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
        <br /><input type="button"  class="button-2" value="返回积分明细" onclick="window.location.href='UserAccountRecord.aspx?UserID=<%=RequestHelper.GetQueryString<int>("UserID") %>'" />
    </div>
</div>
    </div>

</asp:Content>
