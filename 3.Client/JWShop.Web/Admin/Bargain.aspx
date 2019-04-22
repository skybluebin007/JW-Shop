<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Bargain.aspx.cs" Inherits="JWShop.Web.Admin.Bargain" Async="true" %>

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
       <%-- <div class="tab-title" style="margin: 30px 15px -1px 15px;">
            <span <%if(status == -1)
                { %>class="cur"
                <%} %>><a href="GroupBuy.aspx?Action=search&Status=-1">失效活动</a></span>
            <span <%if (status == 0)
                { %>class="cur"
                <%} %>><a href="GroupBuy.aspx?Action=search&Status=0">砍价活动</a></span>
            
        </div>--%>
        <div class="product-container" style="padding-top: 20px;">	

              <table class="product-list user-add-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
	                    <td style="width:15%">活动名称</td>
	                    <td style="width:15%">发起砍价</td>
                        <td style="width:10%">销售量</td>
                        <td style="width:15%">参与砍价</td>
	                    <td style="width:20%;">活动时间</td>
                        <td style="width:10%;">活动状态</td>
	                    <td style="width:10%">管理</td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
	                    <ItemTemplate>	     
        	                <tr>			                  
			                    <td><%#Eval("Name") %></td>
                                <td><%#Eval("Number") %></td>
			                    <td><%#Eval("SalesVolume") %></td>
                                <td><%#Eval("Bargain_Records_Total") %></td>
                                <td><%#Eval("StartDate") %><br /><%#Eval("EndDate") %></td>
                                <td>
                                    <%#ShopCommon.ActiveStatus(Convert.ToDateTime(Eval("StartDate")),Convert.ToDateTime(Eval("EndDate")),Convert.ToInt16(Eval("Status"))) %>
                                </td>
			                    <td >		
                                    <%# ShowHref(Eval("Id")) %>	                        
                                   <%-- <a href="BargainAdd.aspx?ID=<%#Eval("Id") %>"  class="ig-colink">修改</a> 
                                    <a onclick="SelectRecording(<%#Eval("Id") %>);" class="ig-colink">砍价记录</a>   --%>                                                        
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
        function SelectRecording(id) {
            layer.open({
                type: 2,
                title: '砍价记录',
                fix: false,
                shadeClose: true,
                maxmin: true,
                area: ['1200px', '600px'],
                content: 'Recording.aspx?ID='+id
            });
        }

</script>
</asp:Content>

