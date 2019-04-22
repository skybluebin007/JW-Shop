<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserCount.aspx.cs" Inherits="JWShop.Web.Admin.UserCount" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script src="/Admin/static/js/echarts.min.js"></script>   
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur"><a href="UserCount.aspx">数量分析</a></span>
            <span><a href="UserStatus.aspx">状态分析</a></span>
            <span><a href="UserActive.aspx">活跃度分析</a></span>
            <span><a href="UserConsume.aspx">消费分析</a></span>
        </div>
        <div class="product-container product-container-border">
            <dl class="product-filter clearfix" style="float: none; margin-bottom: 0;">
		        <dd>
                    <div class="head">注册时间：</div>
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
	                <li>1、横坐标表示选择时间内的月或者日，纵坐标表示注册会员数量。</li>
	                <li>2、选择月份则表示该月每天注册的人数，否则就就表示该年每月注册的人数。</li>
	                <li>3、会员是指所有状态的会员。</li>
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
                action: "GetUserCount"
            },
            dataType: "JSON",
            type: "GET",
            success: function (data) {
                var listName="" ;
                if (data.length > 12) {
                    for (i = 1; i <= data.length; i++) {
                        listName = listName + i + ",";
                    }
                    listName = listName.substr(0, listName.length - 1) + "";
                    listName = listName.split(",");
                } else {
                    listName = ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'];
                }
                // 指定图表的配置项和数据
                var option = {                    
                    tooltip: {},
                    legend: {
                        data: ['用户量']
                    },
                    xAxis: {
                        data: listName
                    },
                    yAxis: {},
                    series: [{
                        name: '用户量',
                        type: 'line',
                        data: data
                    }]
                };

                // 使用刚指定的配置项和数据显示图表。
                myChart.setOption(option);
            }
        })

        
        
    </script>
</asp:Content>
