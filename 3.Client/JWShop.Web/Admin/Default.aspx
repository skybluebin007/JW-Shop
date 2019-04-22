<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JWShop.Web.Admin.Default" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<!DOCTYPE html>
<html lang="zh-cn">
<head>
    <meta charset="utf-8" />
    <title><%=Global.ProductName%></title>
    <link rel="stylesheet" href="/Admin/static/css/main.min.css" />
      <script type="text/javascript" src="/Admin/Js/Common.js"></script>
    <script>
        if (navigator.appName == "Microsoft Internet Explorer" && (navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE7.0" || navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE8.0")) {
            window.location.href = "/ie6.html"
        }
            <%--移动端访问进入移动端后台--%>
 
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
<body>
    <div class="header" id="header">
        <div class="logo"><a href="/Admin/">
        	<img src="/Admin/uploadfile/logo.png" title="竞网商城管理系统" alt="竞网商城管理系统" />
         <h3><%=Global.ProductName %></h3>
        <p><%=Global.Version %></p>
        </a></div>
        <%-- <div class="imenu ease"><a href="javascript:goUrl('/Admin/Cnzz.aspx?cnzzID=1259041501',0,0)" title="流量统计">流量统计</a></div>--%>
        <div class="clear"></div>
        <div class="quick">
    	    <div class="row account" id="account">
        	    <p>欢迎您，<%=Cookies.Admin.GetAdminName(false)%>管理员</p>
                <dl class="drop">
                    <dd class="ease"><a href="javascript:goUrl('ChangePassword.aspx')">修改密码</a></dd>
            	    <dd class="ease"><a href="Logout.aspx">退出</a></dd>
                </dl>
            </div>   
             <div class="row account" dropdown>
                <p>小程序</p>
                <dl class="drop" >
                   <dd> <img src="<%=ShopCommon.ShowImage(ShopConfig.ReadConfigInfo().LittlePrgCode)%>" style="width: 222px;"/>  </dd>        	    
                </dl>
            </div>            
            <%--<div class="row ease"><a href="/" target="_blank" title="商城首页" class="ico-shophome">商城首页</a></div>--%>
            <div class="row ease"><a href="/Admin" title="后台首页" class="ico-shophome">后台首页</a></div>
            <div class="row ease rowqq">
                <!-- WPA Button Begin -->
                <script charset="utf-8" type="text/javascript" src="http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODA2NTE0N180ODE0MDdfNDAwMDczMTc3N18"></script>
                <!-- WPA Button End -->
            </div>
            <div class="row ease roetel">客户服务热线：400-0731-777</div>
  <%--           <div class="row ease"><a href="javascript:goUrl('/Admin/ChangeLog.aspx')" title="更新日志" class="ico-shophome">更新日志</a></div>
           <div class="row account" dropdown>
                <p>帮助中心</p>
                <dl class="drop" id="helpsDL" >
                              	    
                </dl>
            </div>
            <div class="row account" dropdown >
                <p>网络知识</p>
                <dl class="drop" id="saleDL" >                             	    
                </dl>
            </div>--%>
        </div>
    </div>
    <div class="navbar" id="navbar">
	    <div class="nav-pointer" id="nav-pointer"></div>
        <dl class="nav">
            <dd class="ease"><a href="javascript:void(0)" data-id="2" title="商品管理" class="ico-product-manage"><span>商品管理</span></a></dd>
             <dd class="ease"><a href="javascript:void(0)" data-id="5" title="订单管理" class="ico-order"><span>订单管理</span></a></dd>
            <%--<dd class="ease"><a href="javascript:void(0)" data-id="4" title="市场营销" class="ico-order-manage"><span>市场营销</span></a></dd>--%>
            <dd class="ease"><a href="javascript:void(0)" data-id="3" title="会员管理" class="ico-order-sale"><span>会员管理</span></a></dd>
            <dd class="ease"><a href="javascript:void(0)" data-id="161" title="分析统计" class="ico-statistics"><span>分析统计</span></a></dd>           
            <%--<dd class="ease"><a href="javascript:void(0)" data-id="162" title="微站配置" class="ico-operate"><span>微站配置</span></a></dd>--%>
            <dd class="ease"><a href="javascript:void(0)" data-id="1" title="基础设置" class="ico-shop-manage"><span>基础设置</span></a></dd>
            <%--<dd class="ease"><a href="javascript:void(0)" data-id="163" title="其他" class="ico-shop-manage"><span>其他</span></a></dd>--%>
        </dl>
    </div>
    <div class="wrapper" id="wrapper">
        <div class="menubar ease hidden" id="menubar">
        	<div class="boxo">
            	<div class="boxi">
                    <div class="main" id="menu_main"></div>
                </div>
            </div>
            <div class="menu-pointer" id="menu-pointer"></div>
        </div>
        <iframe src="Right.aspx" height="100%" width="100%" frameborder="0" id="RightFrame"></iframe>
    </div>
     
    <script type="text/javascript" src="/Admin/Js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="/Admin/static/js/app.min.js"></script>
    <script type="text/javascript" src="/Admin/layer/layer.js"></script>
    <script type="text/javascript" src="/Admin/Js/jquery.tmpl.min.js"></script>
    <script type="text/javascript">
        $('#navbar .ease a').click(function () {
            var that = $(this).parent();
            var id = $(this).attr('data-id');

            //设定最长等待10秒
            var index = layer.load(0, { time: 10 * 1000 });
            $.ajax({
                url: '/Admin/Default.aspx?Action=GetMenuList&id=' + id,
                type: 'GET',
                dataType: "JSON",
                success: function (result) {
                    if (result.flag == false) {
                        alertMessage("登录状态过期，请重新登录");
                        window.setTimeout(function () { window.location.href = "/admin/login.aspx"; }, 1000);
                    }
                    else {
                        $('#menubar').removeClass('hidden');
                        $('#menu_main').html('');
                        $("#tmplMenu").tmpl(result).appendTo('#menu_main');

                        $(that).addClass('current').siblings().removeClass('current');

                        //默认跳转到第一个网址
                        var isRedirect = false;
                        for (var i in result) {
                            if (isRedirect) break;
                            for (var k in result[i].list) {
                                var url = result[i].list[k].url;
                                if ($.trim(url) != '') {
                                    goUrl(url, index, 0);
                                    isRedirect = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            });
        });
        function goUrl(url, index, i) {
            var _iframe = document.getElementById("RightFrame");
            _iframe.src = url;

            if (index <= 0) {
                index = layer.load(0, { time: 10 * 1000 });
            }
            
            //判断iframe是否加载完成
            //readystatechange 事件相对于 load 事件有一些潜在的问题
            //IE 支持 iframe 的 onload 事件，不过是隐形的，需要通过 attachEvent 来注册。
            if (_iframe.attachEvent) {
                _iframe.attachEvent("onload", function () {
                    handleGoUrl(index, i);
                });
            } else {
                _iframe.onload = function () {
                    handleGoUrl(index, i);
                };
            }
        }
        function handleGoUrl(index, i) {
            layer.close(index);
            $('.menu dd').removeClass('current');
            if (i > 0) {
                $('.menu dd[data-id=' + i + ']').addClass('current');
            }
            else {
                $('.menu dd').eq(0).addClass('current');
            }
        }
    </script>
    <!--menus templates start-->
    <script id="tmplMenu" type="text/x-jquery-tmpl">
        <h2 class="topclass">${name}</h2>
        <dl class="menu">
            {{each(i,menu) list}}
                <dd class="ease " data-id="${menu.id}">
                    <a href="javascript:goUrl('${menu.url}', 0, ${menu.id})" title="${menu.name}">
                        <span>${menu.name}</span>
                    </a>
                </dd>
            {{/each}}
        </dl>
    </script>
    <script>
        setTimeout(showVJ, 2000);
        function showVJ() {
            //$("#helpsDL").load("Ajax.aspx?action=GetVJINGList&masterid=89");
            //$("#saleDL").load("Ajax.aspx?action=GetVJINGList&masterid=83");
        }
    </script>
    <!--menus templates end-->
</body>
</html>