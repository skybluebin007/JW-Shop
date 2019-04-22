<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FindPassword.aspx.cs" Inherits="JWShop.Web.Admin.FindPassword" %>

<%@ Import Namespace="JWShop.Common" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>找回密码 - <%=Global.ProductName%> <%=Global.Version%></title>
<link rel="stylesheet" href="/Admin/static/css/main.min.css" />
<script type="text/javascript" src="/Admin/Js/jquery-1.7.2.min.js"></script>
    <script src="/Admin/js/common.js" type="text/javascript"></script>
<script>
    if (navigator.appName == "Microsoft Internet Explorer" && (navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE7.0" || navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE8.0")) {
        window.location.href = "/ie6.html"
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
        <span>找回密码</span>
    </div>
    <div class="form">
        <%if (string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("emailResult"))) {%>
        <div class="row ico-user">
            <SkyCES:TextBox ID="AdminName" CssClass="txt" runat="server" MaxLength="20" placeholder="账号"/>
        </div>
        <div class="row ico-email">
            <SkyCES:TextBox ID="Email" TextMode="email" CssClass="txt" MaxLength="30" runat="server"  placeholder="邮箱"/>            
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
				            cache: false,
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
         
            <asp:Button CssClass="submit ease" ID="SubmitButton" Text=" 提 交 " runat="server" OnClientClick="return checkLogin();"  OnClick="SubmitButton_Click"  />
        </div>
        <div class="tap" style="width: 167%;">
            <label class="iskeep">             
            <span style="font-size:12px; float:right;"><a href="/" style=" margin-right:10px;" target="_blank" >网站主页</a>|<a href="http://z.hnjing.com" target="_blank" style=" margin-right:10px;"><%=Global.CopyRight%></a></span>
        </div>
       
    </div>
        <%}else{ %>
         <div class="row" style="border: none; background:none; margin: 10px 0;">
             <div class="tap" id="logMsg">
                 <%=RequestHelper.GetQueryString<string>("emailResult") %>
                 </div>
             </div>
        <%} %>
    <div class="clear"></div>
</div>
</form>

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
            alert("请输入登录账号");
            $("#AdminName").focus();
            return false;
        }
        if ($("#Email").val() == "") {
           alert("请输入邮箱");
            $("#Email").focus();
            return false;
        }
        if (!Validate.isEmail($("#Email").val())) {
            alert("邮箱格式不正确");
            $("#Email").focus();
            return false;
        }     
    }

</script>
</body>
</html>