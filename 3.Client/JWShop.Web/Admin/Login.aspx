<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="JWShop.Web.Admin.Login" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>登录 - <%=Global.ProductName%> <%=Global.Version%></title>
<link rel="stylesheet" href="/Admin/static/css/main.min.css" />
    <link href="/Admin/static/css/drag.css" rel="stylesheet" type="text/css">
<script src="/Admin/static/js/jquery-1.10.1.min.js" type="text/javascript"></script>
<script src="/Admin/static/js/drag.js" type="text/javascript"></script>
<script>
    if (navigator.appName == "Microsoft Internet Explorer" && (navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE7.0" || navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE8.0")) {
            window.location.href = "/ie6.html"
    }
       //<!--移动端访问进入移动端后台-->  
    var host = window.location.host;
    var hrefs = window.location.href;
    var system = {
        win: false,
        mac: false,
        xll: false
    };
    //检测平台
    var p = navigator.platform;
    system.win = p.indexOf("Win") == 0;
    system.mac = p.indexOf("Mac") == 0;
    system.x11 = (p == "X11") || (p.indexOf("Linux") == 0);
    if (system.win || system.mac || system.xll) {
    } else {
        var urlsprit = hrefs.replace("http://" + host, "");

        //window.location.href = "http://" + host + "/mobile" + urlsprit;
        window.location.href = "http://" + host + "/mobileadmin/index.html";
    }

</script> 
</head>

<body class="loginpage" style="height:100%;background:#6b0001; overflow:hidden;">
<div id="particles-js" style="width:100%; height:100%;">
	<canvas width="1147" height="955" style="width: 100%; height: 100%;"></canvas>
</div>
<form id="form1" runat="server">

<div class="loginbox">
    <div class="head">
        <div class="logo">
        <img src="/Admin/uploadfile/logo.png" title="<%=Global.ProductName %>" alt="<%=Global.ProductName %>" />
        <h3><%=Global.ProductName %></h3>
        <p><%=Global.Version %></p>
        </div>
        <span>用户登录中心</span>
    </div>
    <div class="form">
        <div class="row ico-user">
            <SkyCES:TextBox ID="AdminName" CssClass="txt" runat="server" MaxLength="20" placeholder="账号"/>
        </div>
        <div class="row ico-password">
            <SkyCES:TextBox ID="Password" TextMode="password" CssClass="txt" MaxLength="20" runat="server"  placeholder="密码"/>            
        </div>

      <div class="row" style="border: none; background:none; margin: 10px 0;">
				<div id="captcha"></div>
          <script src="http://static.geetest.com/static/tools/gt.js"></script>
				<script>
				    $(function () { 
				    var handler = function (captchaObj) {
				        // 将验证码加到id为captcha的元素里
				        captchaObj.appendTo("#captcha");
				    };
				    $.ajax({
				        // 获取id，challenge，success（是否启用failback）
				        url: "getcaptcha.aspx",
				        type: "get",
				        dataType: "json", // 使用jsonp格式
				        cache:false,
				        success: function (data) {
				            // 使用initGeetest接口
				            // 参数1：配置参数，与创建Geetest实例时接受的参数一致
				            // 参数2：回调，回调的第一个参数验证码对象，之后可以使用它做appendTo之类的事件
				            initGeetest({
				                gt: data.gt,
				                challenge: data.challenge,
				                product: "float", // 产品形式
				                offline: !data.success
				            }, handler);
				        }
				    });
				    })
				</script>
        <div class="button" style="margin-top: 10px;">
            <%--<input type="button" class="submit ease" id="submitbtn" value=" 登 录 " onclick="checkLogin();" />--%>
            <asp:Button CssClass="submit ease" ID="SubmitButton" Text=" 登 录 " runat="server" OnClientClick="return checkLogin();"  OnClick="SubmitButton_Click"  />
        </div>
        <div class="tap" style="width: 167%;">
            <label class="iskeep">
                <asp:CheckBox ID="Remember" runat="server"/>保持登录状态</label>
            <span style="font-size:12px; float:right;"><a href="/admin/findpassword.aspx" style=" margin-right:10px;" target="_blank" >忘记密码</a>|<a href="http://z.hnjing.com" target="_blank" style=" margin-right:10px;"><%=Global.CopyRight%></a></span>
        </div>
        <div class="tap" id="logMsg">
          <%=RequestHelper.GetQueryString<string>("errorMsg") %>
            </div>
          <%if(!string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("errorMsg"))) {%>
            <script>                 
                    alert('<%=StringHelper.KillHTML(RequestHelper.GetQueryString<string>("errorMsg"))%>');              
            </script>
        <%} %>
    </div>
    <div class="clear"></div>
</div>
</form>
<script src="/Admin/static/js/jquery.min.js"></script>
<script src="/Admin/static/js/jquery.particles.min.js"></script>
<script src="/Admin/static/js/app.min.js"></script>
<script>
window.particlesJS && particlesJS("particles-js", {
	particles: {
		color: "#fff",
		shape: "circle",
		opacity: 1,
		size: 1,
		size_random: !0,
		nb: 200,
		line_linked: {
			enable_auto: !0,
			distance: 100,
			// color: "#46BCF3",
			color: "#fff",
			opacity: .5,
			width: 1,
			condensed_mode: {
				enable: !1,
				rotateX: 600,
				rotateY: 600
			}
		},
		anim: {
			enable: !0,
			speed: 2.5
		}
	},
	interactivity: {
		enable: !0,
		mouse: {
			distance: 250
		},
		detect_on: "canvas",
		mode: "grab",
		line_linked: {
			opacity: .35
		},
		events: {
			onclick: {
				enable: !0,
				mode: "push",
				nb: 3
			}
		}
	},
	retina_detect: !0
})

$(function () {
    $("#AdminName").focus();
})

function checkLogin() {
    if ($("#AdminName").val() == "") {
        $("#logMsg").text(" *请输入登录账号");
        $("#AdminName").focus();
        return false;
    }
    if ($("#Password").val() == "") {
        $("#logMsg").text(" *请输入密码");
        $("#Password").focus();
        return false;
    }
    /*ajax提交*/

    //$.ajax({
    //    url: "login.aspx?Action=GeetestValidate",
    //    type: "post",
    //    async : false,
    //    data:$("form").serialize(),
    //    dataType: "json",
    //    success: function (data) {
    //        if (data.flag == "ok") {
    //            window.location.href = "/admin/default.aspx";
    //        }
    //        else {//滑块验证失败
    //            $("#logMsg").text(data.msg);
    //            return false;
    //        }
    //    },
    //    error: function () { $("#logMsg").text(" *系统忙，请稍后重试"); return false; }
    //})
    /*ajax提交 end*/
  
}

</script>
</body>
</html>
