<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="CouponSend.aspx.cs" Inherits="JWShop.Web.Admin.CouponSend" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <style type="text/css">
	body { width:100% !important; min-width:auto !important; }
        .form-txt .head, .form-txt2 .head { left: -40px; width: 120px; }
        .form-relink { width: 520px; }
        .form-relink .all, .form-relink .select {width: 180px; }
    </style>
    <div class="container ease" id="container" style="left: 0;">
    	<br/>
        <div class="product-container" style="padding: 0;">
            <div class="form-row">
                <div class="head">名称：</div>
                <%=coupon.Name%>
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">优惠券金额：</div>
                <%=coupon.Money%> 元 
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">最小订单金额：</div>
                <%=coupon.UseMinAmount%> 元 
            </div>
            <div class="clear"></div>
            <div class="form-row" style="display:none;">
                <div class="head">线下发放数量：</div>
                <SkyCES:TextBox ID="SendCount" CssClass="txt" runat="server" Width="140px" RequiredFieldType="数据校验" Text="0" />
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">在线发放：</div>
                <SkyCES:TextBox ID="UserName" CssClass="form-select-txt" runat="server" placeholder="用户名" />
                <input type="button" class="form-select-submit ease" value=" 搜 索 " onclick="searchUser()" />
                <div class="clear"></div>
                <div class="form-relink">
                    <div id="CandidateUserBox">
                        <select id="<%=IDPrefix%>CandidateUser" name="<%=NamePrefix %>CandidateUser" class="all" multiple="multiple"></select>
                    </div>
                    <div class="button">
                        <a href="javascript:;" class="addall ease" onclick="addAll('<%=IDPrefix%>CandidateUser','<%=IDPrefix%>User')">全部关联 <font>&gt;&gt;</font></a>
                        <a href="javascript:;" class="addone ease" onclick="addSingle('<%=IDPrefix%>CandidateUser','<%=IDPrefix%>User')">关联 <font>&gt;</font></a>
                        <a href="javascript:;" class="delone ease" onclick="dropSingle('<%=IDPrefix%>User')"><font>&lt;</font>取消关联</a>
                        <a href="javascript:;" class="delall ease" onclick="dropAll('<%=IDPrefix%>User')"><font>&lt;&lt;</font>全部取消关联</a>
                    </div>
                    <asp:ListBox ID="User" runat="server" SelectionMode="Multiple" CssClass="select"></asp:ListBox>
                    <input type="hidden" name="RelationUser" id="RelationUser" />
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head2">&nbsp;</div>
                <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" OnClientClick="return checkSubmit()" />
            </div>
        </div>
    </div>
    
 
    <script type="text/javascript">
        var productID = 0, isUpdate=0;
        //搜索用户
        function searchUser() {
            var userName = o(globalIDPrefix + "UserName").value;

            var index = layer.load();
            $.ajax({
                url: '/Admin/ProductAjax.aspx',
                type: 'GET',
                data: { 'ControlName': 'CandidateUser', 'Action': 'SearchUser', 'UserName': encodeURI(userName) },
                success: function (data) {
                    layer.close(index);
                    var obj = o("CandidateUserBox");
                    obj.removeChild(o(globalIDPrefix + "CandidateUser"));
                    obj.innerHTML = data;
                    $('#' + globalIDPrefix + "CandidateUser").addClass('all');
                }
            });
        }
        //提交检查
        function checkSubmit() {
            checkProductHandler("<%=IDPrefix%>User", "RelationUser");
            Page_ClientValidate();
        }
    </script>
       <script type="text/javascript" src="/Admin/Js/ProductAdd.js"></script>
</asp:Content>
