<%@ Page Language="C#"  MasterPageFile="MasterPage.Master"  AutoEventWireup="true" CodeBehind="WriteOffPickUpCode.aspx.cs" Inherits="JWShop.Web.Admin.WriteOffPickUpCode" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
 
    <div class="container ease" id="container">
        <div class="product-container ddhexiao" style="padding-top: 20px;">	
            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">提货码：</div>
                    <input type="text" id="pickCode" class="txt" maxlength="30" />
                   <%-- <SkyCES:TextBox ID="UserName" CssClass="txt" runat="server" maxlength="30"/> --%>
                </dd>
               <%--  <dd>
                    <div class="head">手机号码：</div>
                    <SkyCES:TextBox ID="Mobile" CssClass="txt" runat="server" /> 
                </dd>
                <dd>
                    <div class="head">注册时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartRegisterDate" runat="server" /> <span class="tp">--</span> <SkyCES:TextBox CssClass="txt" ID="EndRegisterDate" runat="server" />
                </dd>         --%>                     
                <dt>
                    <input type="button" class="submit ease" id="searchBtn" value=" 验 证 " />
                   <%-- <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>--%>
            </dl>
            <div class="hxliuc">
            	<span>提货流程</span>
            	<div class="yzliuc">
            		<p><em></em>请买家出示核销码</p>
	            	<p><em></em>商家输入验证码</p>
	            	<p><em></em>验证卡券是否有效</p>
	            	<p><em></em>验证完成，查看使用结果</p>
            	</div>
            </div>
         
        </div>
    </div>
    <script>
        $("#searchBtn").on('click', function () {
            if ($("#pickCode").val() == "") {
                alertMessage("请输入提货码");
                $("#pickCode").focus();
                return false;
            }
            if (!Validate.isNumber($("#pickCode").val())) {
                alertMessage("提货码全为数字");
                $("#pickCode").focus();
                return false;
            }
            $(this).val("请稍后...");
            $.ajax({
                url: 'WriteOffPickUpCode.aspx',
                data: { action: 'searchOrder', pickUpCode: $("#pickCode").val().trim() },
                type: 'Get',
                dataType: 'Json',
                success: function (res) {
                    if (res.ok) {
                        window.location.href = "/admin/orderdetail.aspx?id=" + res.order.Id;
                    }
                    else {
                        $("#searchBtn").val(" 验 证 ");
                        alertMessage(res.msg);
                    }
                },
                error: function () {
                    $("#searchBtn").val(" 验 证 ");
                    alertMessage("系统忙，请稍后重试");
                }
            })
        })
    </script>
</asp:Content>
