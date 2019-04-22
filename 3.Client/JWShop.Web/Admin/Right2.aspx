<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master"  AutoEventWireup="true" CodeBehind="Right2.aspx.cs" Inherits="JWShop.Web.Admin.Right2" %>

<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="SkyCES.EntLib" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <script src="/Admin/static/js/echarts.min.js"></script>     
    <div class="index-page">
    	    <div class="index-head hidden">总交易金额</div>
            <div class="filter-date" id="filter-date">
        	    <span did="5" class="cur">全部</span><span did="1">今天</span><span did="2">最近7天</span><span did="3">最近一个月</span><span did="4">最近三个月</span>
            </div>
            <div class="index-money">
        	    <div class="head">支付金额</div>
                <div class="money" id="moneyscroller"></div>
                <div class="number">客户新增数 0人</div>
            </div>
            <div class="index-msg">
        	    <div class="row">
            	    <div class="head">
                	    <div class="ico ico-product">商品</div>
                    </div>
                    <div class="number"><%=Convert.ToInt32(dt.Rows[0][0])+Convert.ToInt32(dt.Rows[0][1]) %></div>
                    <dl class="list">
                	    <dd>未处理商品评论<span><%=dt.Rows[0][0]%></span></dd>
                        <dd>未处理的缺货登记<span><%=dt.Rows[0][1]%></span></dd>
                    </dl>
                </div>
                <div class="row">
            	    <div class="head">
                	    <div class="ico ico-user">用户</div>
                    </div>
                    <div class="number"><%=Convert.ToInt32(dt.Rows[0][2])+Convert.ToInt32(dt.Rows[0][3])+Convert.ToInt32(dt.Rows[0][4])+Convert.ToInt32(dt.Rows[0][5]) %></div>
                    <dl class="list">
                	    <dd>未激活用户<span><%=dt.Rows[0][2]%></span></dd>
                        <dd>冻结用户<span><%=dt.Rows[0][3]%></span></dd>
                        <dd>未处理的用户留言<span><%=dt.Rows[0][4]%></span></dd>
                        <dd>未处理的提现申请<span><%=dt.Rows[0][5]%></span></dd>
                    </dl>
                </div>
                <div class="row">
            	    <div class="head">
                	    <div class="ico ico-order">订单</div>
                    </div>
                    <div class="number"><%=Convert.ToInt32(dt.Rows[0][6])+Convert.ToInt32(dt.Rows[0][7])+Convert.ToInt32(dt.Rows[0][8])+Convert.ToInt32(dt.Rows[0][9]) %></div>
                    <dl class="list">
                	    <dd>待付款订单<span><%=dt.Rows[0][6]%></span></dd>
                        <dd>待审核订单<span><%=dt.Rows[0][7]%></span></dd>
                        <dd>待发货订单<span><%=dt.Rows[0][8]%></span></dd>
                        <dd>待收货确认订单<span><%=dt.Rows[0][9]%></span></dd>
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
    <script type="text/javascript">
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('chart-user'));

        // 指定图表的配置项和数据
        var option = {
            title: {
                text: 'ECharts 入门示例'
            },
            tooltip: {},
            legend: {
                data: ['销量']
            },
            xAxis: {
                data: ['1', '2', '3', '4', '5', '6', '7']
            },
            yAxis: {},
            series: [{
                name: '销量',
                type: 'line',
                data: [5, 20, 36, 10, 10, 20]
            }]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
    </script>
</asp:Content>
