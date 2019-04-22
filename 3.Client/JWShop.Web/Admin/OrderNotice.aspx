<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderNotice.aspx.cs" Inherits="JWShop.Web.Admin.OrderNotice"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="SkyCES.EntLib" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <script src="/Admin/static/js/highcharts.min.js"></script>   
    <script src="/admin/Scripts/jquery.signalR-1.2.2.min.js"></script>
     <script src="/Signalr/Hubs"></script>   
    <div class="container ease" id="container" style="padding:10px;">    	    
            <div class="index-msg">
                <div class="row" style="width:45%;">
            	    <div class="head">
                	    <div class="ico ico-order">待审核订单</div>
                    </div>
                    <div class="number"><%=dt.Rows[0][7]%></div>                    
                </div>
        	    <div class="row" style="width:45%;">
            	    <div class="head">
                	    <div class="ico ico-product">订单总数</div>
                    </div>
                    <div class="number"><%=Convert.ToInt32(dt.Rows[0][6])+Convert.ToInt32(dt.Rows[0][7])+Convert.ToInt32(dt.Rows[0][8])+Convert.ToInt32(dt.Rows[0][9])+Convert.ToInt32(dt.Rows[0][11]) %></div>                   
                </div>                           
                <div class="clear"></div>
            </div>            
        </div>    
    <%if(notNoticed>0){ %>
    <div id="musicbox" style="display:none">
        <audio controls='controls' id='audio_player'  autoplay='autoplay'><source src = '/Upload/media/win.mp3' type = 'audio/mpeg'></ audio >
    </div>
    <%} %>
    <script>
        //setInterval(function () {
        //    $.ajax({
        //        url: "/Admin/Ajax.aspx",
        //        data: { action: "CheckOrderNotice" },
        //        success: function (data) {
        //            console.log(data)
        //            if (parseInt(data) > 0) {
        //                $("#musicbox").html("<audio controls='controls' id='audio_player'  autoplay='autoplay'><source src = '/Upload/media/win.mp3' type = 'audio/mpeg' ></audio>");
        //                setTimeout(function () {
        //                    location.reload();
        //                }, 1000);
        //            } else {
                        
        //            }
        //        }
        //    })
        //},3000)


        // 引用自动生成的集线器代理
        var pushHub = $.connection.pushHub;

        
        // 定义服务器端调用的客户端sendMessage来显示新消息

        pushHub.client.sendMessage = function (data) {
            console.log(data);
            if (parseInt(data) > 0) {
                $("#musicbox").html("<audio controls='controls' id='audio_player'  autoplay='autoplay'><source src = '/Upload/media/win.mp3' type = 'audio/mpeg' ></audio>");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } 
        };
        $.connection.hub.logging = true;
        $.connection.hub.start();
    </script>
</asp:Content>
