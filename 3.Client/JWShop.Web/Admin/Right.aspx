<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Right.aspx.cs" Inherits="JWShop.Web.Admin.Right"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="SkyCES.EntLib" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <script src="/Admin/static/js/highcharts.min.js"></script>      
    <div class="index-page">
    	    <div class="index-head hidden">总交易金额</div>
            <div class="filter-date hidden" id="filter-date">
        	    <span did="1"  class="cur">今天</span><span did="2">最近7天</span><span did="3">最近一个月</span><span did="4">最近三个月</span><span did="5">全部</span>
            </div>
            <div class="index-money">
        	    <div class="head">今日营业额</div>
                <div class="money" id="moneyscroller"></div>
              <%--  <div class="number">活跃客户数 <span id="usercount"><%=dt.Rows[0][10]%></span> 人</div>--%>
            </div>
        <div class="index-money">
            <div class="head">今日付款单数</div>
            <div class="money" id="ordercountscroller"></div>

        </div>
            <div class="index-msg">
                <div class="row">
            	    <div class="head">
                	    <div class="ico ico-order">订单</div>
                    </div>
                    <div class="number"><%=Convert.ToInt32(dt.Rows[0][6])+Convert.ToInt32(dt.Rows[0][7])+Convert.ToInt32(dt.Rows[0][8])+Convert.ToInt32(dt.Rows[0][9])+Convert.ToInt32(dt.Rows[0][11]) %></div>
                    <dl class="list">
                	    <dd><a href="Order.aspx?OrderStatus=1"> 待付款订单<span><%=dt.Rows[0][6]%></span></a></dd>
                        <dd><a href="Order.aspx?OrderStatus=2">待审核订单<span><%=dt.Rows[0][7]%></span></a></dd>
                        <dd><a href="Order.aspx?OrderStatus=4">待发货订单<span><%=dt.Rows[0][8]%></span></a></dd>
                        <dd><a href="Order.aspx?OrderStatus=5">待收货确认订单<span><%=dt.Rows[0][9]%></span></a></dd>
                        <dd><a href="Order.aspx?OrderStatus=6">已完成订单<span><%=dt.Rows[0][11]%></span></a></dd>
                    </dl>
                </div>
        	    <div class="row">
            	    <div class="head">
                	    <div class="ico ico-product">商品</div>
                    </div>
                    <div class="number"><%=ProductBLL.SearchList(new ProductSearchInfo() {IsSale=1,IsDelete=0 }).Count%></div>
                    <dl class="list">
                	    <dd><a href="ProductComment.aspx"> 未处理商品评论<span><%=dt.Rows[0][0]%></span></a></dd>
                        <dd><a href="BookingProduct.aspx" style="display:none;">未处理的缺货登记<span><%=dt.Rows[0][1]%></span></a></dd>
                    </dl>
                </div>
                <div class="row">
            	    <div class="head">
                	    <div class="ico ico-user">用户</div>
                    </div>
                    <div class="number"><%=UserBLL.SearchList(new UserSearchInfo() {Status=(int)UserStatus.Normal }).Count %></div>
                    <dl class="list">
                	    <dd  style="display:none;">未激活用户<span><%=dt.Rows[0][2]%></span></dd>
                        <dd>冻结用户<span><%=dt.Rows[0][3]%></span></dd>
                        <dd  style="display:none;">未处理的用户留言<span><%=dt.Rows[0][4]%></span></dd>
                       <%-- <dd>未处理的提现申请<span><%=dt.Rows[0][5]%></span></dd>--%>
                    </dl>
                </div>                
                <div class="clear"></div>
            </div>
            <div class="index-chart">
        	    <div class="row">
            	    <div class="head ico-order">订单统计</div>
                    <div class="main">
                	    <dl class="tab">
                            <dd class="cur" did="4">最近3个月</dd>
                    	    <dd did="1">今天</dd>
                            <dd did="2">最近7天</dd>
                            <dd did="3">最近30天</dd>                        
                        </dl>
                        <div class="chart" id="chart-order"></div>
                        <div class="title">订单统计数据</div>
                    </div>
                </div>
                <div class="row">
            	    <div class="head ico-user">会员统计</div>
                    <div class="main">
                	    <dl class="tab">
                            <dd class="cur" did="8">最近3个月</dd>
                    	    <dd did="5">今天</dd>
                            <dd did="6">最近7天</dd>
                            <dd did="7">最近30天</dd>                        
                        </dl>
                        <div class="chart" id="chart-user"></div>
                        <div class="title">会员统计数据</div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </div>    
    <div id="musicbox" style=" display:none;">
        
    </div>
    <script>
        /*
        setInterval(function () {
            $.ajax({
                url: "/Admin/Ajax.aspx",
                data: { action: "CheckOrderNotice" },
                success: function (data) {
                    if (parseInt(data) > 0) {
                        $.ajax({
                            url: "/Admin/Ajax.aspx",
                            data: { action: "LoadSound", mtype: 1 },
                            success: function (data) {
                                $("#musicbox").html(data);
                            }
                        })                        
                    } else {
                        //$.ajax({
                        //    url: "/Admin/Ajax.aspx",
                        //    data: { action: "LoadSound" },
                        //    success: function (data) {
                        //        $("#musicbox").html(data);
                        //    }
                        //})
                    }
                }
            })
        },2000)
        */
    </script>
</asp:Content>
