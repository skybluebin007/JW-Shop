<%@ Page Language="C#"  MasterPageFile="MasterPage.Master"  AutoEventWireup="true" CodeBehind="GroupBuy.aspx.cs" Inherits="JWShop.Web.Admin.GroupBuy" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartRegisterDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndRegisterDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>

    <div class="container ease" id="container">
        <div class="tab-title" style="margin: 30px 15px -1px 15px;">
            <span <%if(status == -1)
                { %>class="cur"
                <%} %>><a href="GroupBuy.aspx?Action=search&Status=-1">拼团失败</a></span>
            <span <%if (status == 0)
                { %>class="cur"
                <%} %>><a href="GroupBuy.aspx?Action=search&Status=0">正在拼团</a></span>
            <span <%if (status == 1)
                { %>class="cur"
                <%} %>><a href="GroupBuy.aspx?Action=search&Status=1">拼团成功</a></span>
        </div>
        <div class="product-container" style="padding-top: 20px;">	
<%--            <dl class="product-filter clearfix">
                <dd>
                    <div class="head">微信昵称：</div>
                    <SkyCES:TextBox ID="UserName" CssClass="txt" runat="server" /> 
                </dd>
                 <dd>
                    <div class="head">手机号码：</div>
                    <SkyCES:TextBox ID="Mobile" CssClass="txt" runat="server" /> 
                </dd>
                <dd>
                    <div class="head">注册时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartRegisterDate" runat="server" /> <span class="tp">--</span> <SkyCES:TextBox CssClass="txt" ID="EndRegisterDate" runat="server" />
                </dd>                              
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>--%>
            <table class="product-list user-add-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
	                    <td style="width:25%" colspan="2">商品</td>
	                    <td style="width:3%">团长</td>
	                    <td style="width:20%;">开始时间</td>
                        <td style="width:10%;">结束时间</td>
                         <td style="width:10%">几人团</td>
                         <td style="width:10%;">已参与</td>
	                    <td style="width:10%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
        	                <tr>			                  
			                    <td><div class="scan-img"><img src="<%#ShopCommon.ShowImage(Eval("ProductPhoto").ToString().Replace("Original","90-90")) %>" /></div></td>
                                <td><%#Eval("ProductName") %></td>
			                    <td><%# Eval("GroupUserName") %></td>
                                <td><%#Eval("StartTime") %></td>
                                <td><%#Eval("EndTime") %></td>
                                <td><%#Eval("Quantity") %></td>
                                <td><%# Eval("SignCount") %></td>
			                    <td >			                        
                                    <a href="GroupSign.aspx?GroupId=<%#Eval("Id")%>"  class="ig-colink">参团记录</a>   
                                    <a href="javascript:void(0)" id="refund_<%#Eval("Id")%>" data-id="<%#Eval("Id") %>" data-quantity="<%#Eval("Quantity") %>" class="js-needrefund"></a>                                                            
			                    </td>
		                    </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="8">
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                            <div class="clear"></div>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">    
        function loadPage(url, title, width, height) {
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: title,
                fix: false,
                shadeClose: true,
                maxmin: false,
                area: [width, height],
                content: url,

            });
        }
        $(function () {
            $(".js-needrefund").each(function () {
                var _id = $(this).attr("data-id");
                var _quantity = $(this).attr("data-quantity");
                needRefund(_id, _quantity)
            })

        //计算是否待退款
        function needRefund(id, quantity) {
            if (id > 0 && quantity > 0) {
                $.ajax({
                    url: 'Ajax.aspx',
                    data: { Action: 'needrefund', id: id,quantity:quantity },
                    type: 'Get',
                    dataType: 'Json',
                    success: function (res) {
                        if (res.needrefund) {
                            $("#refund_"+id).html("待退款");
                        }
                        else if (res.needcheck) {
                            $("#refund_" + id).html("待审核");
                        }
                        else {
                            //console.log(res.msg);
                        }
                    },
                    error: function () {
                       console.log("系统忙，请稍后重试");
                    }
                })
            }
        }
        })
</script>
</asp:Content>
