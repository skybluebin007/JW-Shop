<%@ Page Language="C#"  MasterPageFile="MasterPage.Master"  AutoEventWireup="true" CodeBehind="GroupSign.aspx.cs" Inherits="JWShop.Web.Admin.GroupSign" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<style>
body { width:100% !important; min-width:auto !important; }
</style>
    <div class="container ease" id="container" style="left: 0; min-width: auto; ">
        <div class="product-container" style="padding-top: 20px; margin:0 15px !important;">
         
            <table class="product-list">
                <thead>
                    <tr>    
	                    <td>会员名/微信昵称</td>
	                    <td>头像</td>
	                    <td>参团时间</td>
	                 <td></td>
                       
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>
                            <tr>                             
                                <td><%#Eval("UserName")%><span class="<%#Eval("UserId").ToString()==group.Leader.ToString()?"":"hidden" %>">[团长]</span></td>
			                    <td><div class="scan-img"><img src="<%#ShopCommon.ShowImage(Eval("UserAvatar").ToString()) %>" /></div></td>
			                    <td><%#Eval("SignTime")%></td> 
                              <%# NeedRefund(Eval("StartTime"),Eval("EndTime"),Eval("Quantity"), Eval("SignCount"),Eval("orderId"),Eval("GroupOrderStatus"),Eval("IsRefund"))%>
                                 <%# NeedCheck(Eval("StartTime"),Eval("EndTime"),Eval("Quantity"), Eval("SignCount"),Eval("orderId"),Eval("GroupOrderStatus"),Eval("IsRefund"))%>
                                <td id="hasrefund_<%#Eval("OrderId") %>" class="<%#Eval("GroupOrderStatus").ToString()=="3" && Eval("IsRefund").ToString()=="1"?"":"hidden" %>">
                                    <a href="javascript:void(0)">已退款</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>

            </table>

        </div>
        <div class="form-row" style="float: right;right: 50px;">
            <span id="allPhotosMsg" style="color: red; float: right; font-size: 18px;"></span>
            <br />
            <img id="loading" />
        </div>
    </div>
      <script type="text/javascript">
          function GroupBuyOrderRefund(orderId) {
              $("#allPhotosMsg").text("正在退款，请耐心等待...");
              $("#loading").attr("src", "/admin/images/loading.gif").show();
            //debugger;
            if (orderId == null || orderId == undefined || orderId <= 0) {
                alert("参数错误");
                $("#loading").attr("src", "").hide();
                return false;
            }
         
            $.ajax({
                url: 'ajax.aspx',
                data: { Action: "GroupBuyOrderRefund", orderId: orderId },
                type: 'Post',
                dataType: 'Json',
                cache: false,
                async:false,
                success: function (res) {
                    if (res.ok)
                    {
                        //$(this).hide();
                        //$("#hasrefund_" + orderId).show();
                        location.reload();
                    }
                    else
                    {
                        $("#allPhotosMsg").text("");
                        $("#loading").attr("src", "").hide();
                        alert(res.msg);
                    }
                },
                error: function () {
                    $("#allPhotosMsg").text("");
                    $("#loading").attr("src", "").hide();
                    alert("系统忙，请稍后重试");
                }
            })
        }
    </script>
</asp:Content>
