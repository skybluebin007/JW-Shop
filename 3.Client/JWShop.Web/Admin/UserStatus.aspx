<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserStatus.aspx.cs" Inherits="JWShop.Web.Admin.UserStatus" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
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
    <script src="/Admin/static/js/echarts.min.js"></script>

    <div class="container ease" id="container">
        <div class="tab-title">
            <span><a href="UserCount.aspx">数量分析</a></span>
            <span class="cur"><a href="UserStatus.aspx">状态分析</a></span>
            <span><a href="UserActive.aspx">活跃度分析</a></span>
            <span><a href="UserConsume.aspx">消费分析</a></span>
        </div>
        <div class="product-container product-container-border">
            <dl class="product-filter clearfix" style="float: none; margin-bottom: 0;">
		        <dd>
                    <div class="head">注册时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartRegisterDate" runat="server" /> <span class="tp">到</span> <SkyCES:TextBox CssClass="txt" ID="EndRegisterDate" runat="server" /> 
		        </dd>
                <dt><asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" /></dt>
            </dl>
            <div id="canvas" style="height:250px;">

            </div>              
        </div>
    </div>
    <script type="text/javascript">
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('canvas'));

        
                // 指定图表的配置项和数据
                var option = {
                    tooltip: {},
                    legend: {
                        orient: 'vertical',
                        left: 'left',
                        data: [<%=statusArr%>]
                    },
                    series: [
                        {
                            name: '用户状态',
                            type: 'pie',
                            radius: '55%',
                            center: ['50%', '60%'],
                            data: <%=result%>,
                            itemStyle: {
                                emphasis: {
                                    shadowBlur: 10,
                                    shadowOffsetX: 0,
                                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                                }
                            }
                        }
                    ]
                };

                // 使用刚指定的配置项和数据显示图表。
                myChart.setOption(option);
            



    </script>
</asp:Content>
