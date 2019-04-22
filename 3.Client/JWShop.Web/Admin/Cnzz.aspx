<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Cnzz.aspx.cs" Inherits="JWShop.Web.Admin.Cnzz" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="helper-container" >
	<div class="helper-main" >
        <iframe width="100%" height="100%" src="http://new.cnzz.com/v1/login.php?siteid=<%=cnzzID%>" id="cnzz"></iframe>

    </div>
        </div>
    <!--div class="layout-cnzz-top" cnzz-shadow></div>
    <div class="layout-cnzz-bottom" cnzz-shadow></div>
    <div class="layout-cnzz-left" cnzz-shadow></div>
    <div class="layout-cnzz-right" cnzz-shadow></div>
    <script>
	function cnzz_fun(){
		var _width = $(window).width(), _wid = 720, _left = 180, _con = 980;
		$(".layout-cnzz-left").css({"width": _left + (_width - _left - _con)/2 - _left});
		$(".layout-cnzz-right").css({"left": _left + (_width - _left - _con)/2 + _wid});
	}
	cnzz_fun();
	$(window).resize(cnzz_fun);
	$(window).bind("click", function(){
		$("[cnzz-shadow]").fadeOut(500);
	})
	</script-->
</asp:Content>
