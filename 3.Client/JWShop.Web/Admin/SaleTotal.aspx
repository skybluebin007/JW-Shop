<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="SaleTotal.aspx.cs" Inherits="JWShop.Web.Admin.SaleTotal" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">	
    <script src="/Admin/static/js/echarts.min.js"></script>   
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur"><a href="SaleTotal.aspx">销售汇总</a></span>
            <span><a href="SaleStop.aspx">滞销分析</a></span>
            <span><a href="SaleDetail.aspx">销售流水账</a></span>
        </div>
        <div class="product-container product-container-border">
            <dl class="product-filter clearfix" style="float: none; margin-bottom: 0;">
		        <dd>
                    <div class="head">下单时间：</div>
                    <asp:DropDownList ID="Year" runat="server" CssClass="select"></asp:DropDownList>  <span class="tp">年</span> 
                    <asp:DropDownList ID="Month" runat="server" CssClass="select"></asp:DropDownList> <span class="tp">月</span>
		        </dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <div id="canvas" style="height:250px;">

            </div> 
	        <div class="note">	    
	            <ul>
	                <li class="title">说明</li>	          
	                <li>1、横坐标表示选择时间内的月或者日，纵坐标表示订单数量或者金额。</li>
	                <li>2、选择月份则表示该月每天订单的数量或者金额，否则就就表示该年每月订单的数量或者金额。</li>
	                <li>3、销售汇总只统计完成的订单（即订单的状态为收货确认）。</li>	
	            </ul>
	        </div>
        </div>
    </div>
    <script type="text/javascript">
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('canvas'));

        $.ajax({
            url: "/Admin/Ajax.aspx<%=queryString%>",
            data: {
                action: "GetSaleTotal"
            },
            type: "GET",
			dataType: "json",
            success: function (data) {
                var listName = "";
                //console.log(data[1].data.length);
                if (data.length >0 && data[1].data.length>12) {
                    for (i = 1; i <= data[1].data.length; i++) {
                        listName = listName + i + ",";
                    }
                    listName = listName.substr(0, listName.length - 1) + "";
                    listName = listName.split(",");
                } else {
                    listName = ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'];
                }
                // 指定图表的配置项和数据
                var option = {                   
                        tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        data: ['订单量','销售额']
                    },
                    xAxis: {
                        data: listName
                    },
                    yAxis: { type: 'value' },
                    series: data
                };
                //series: [{ name: '订单量', type: 'line', itemStyle: { normal: {color:'#00ff00'}}, data: [0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0] }, { name: '销售额', type: 'line', data: [0, 0, 2825, 0, 0, 0, 0, 0, 0, 0, 0, 0] }]
                // 使用刚指定的配置项和数据显示图表。
                myChart.setOption(option);
            }
        })



    </script>
</asp:Content>
