<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPage.Master" CodeBehind="SendMessageAdd.aspx.cs" Inherits="JWShop.Web.Admin.SendMessageAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">



    <div class="container ease" id="container">
        <div class="product-container product-container-border" style="padding:20px 50px 50px 50px; margin: 70px 50px 20px;">
            <div class="form-row">
                <div class="head">标题：</div>
               <SkyCES:TextBox ID="Title" CssClass="txt" runat="server" Width="400px" CanBeNull="必填" MaxLength="30"/>
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">内容：</div>                                     
                <SkyCES:TextBox ID="Content" CssClass="txt" runat="server" Width="500px" Height="200"  CanBeNull="必填" MaxLength="300" TextMode="MultiLine"/>
            </div>
            <div class="clear"></div>

            <div class="form-row">
                <div class="head">选择接收用户：</div>
                <SkyCES:TextBox ID="UserName" CssClass="form-select-txt" runat="server" placeholder="用户名" />
                <input type="button" class="form-select-submit ease" value=" 搜 索 " onclick="searchUser()" />
                <div class="clear"></div>
                <div class="form-relink">
                    <div id="CandidateUserBox">
                        <select id="<%=IDPrefix%>CandidateUser" name="<%=NamePrefix %>CandidateUser" class="all" multiple="multiple"></select>
                    </div>
                    <div class="button">
                        <a href="javascript:;" class="addall ease" onclick="addAll('<%=IDPrefix%>CandidateUser','<%=IDPrefix%>User')">全部选择 <font>&gt;&gt;</font></a>
                        <a href="javascript:;" class="addone ease" onclick="addSingle('<%=IDPrefix%>CandidateUser','<%=IDPrefix%>User')">选择 <font>&gt;</font></a>
                        <a href="javascript:;" class="delone ease" onclick="dropSingle('<%=IDPrefix%>User')"><font>&lt;</font>取消选择</a>
                        <a href="javascript:;" class="delall ease" onclick="dropAll('<%=IDPrefix%>User')"><font>&lt;&lt;</font>全部取消选择</a>
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
    
    <script type="text/javascript" src="/Admin/Js/ProductAdd.js"></script>
    <script type="text/javascript">
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
</asp:Content>
