<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="t1.aspx.cs" Inherits="JWShop.Web.Admin.t1" %>

<!DOCTYPE html>

<html>
<head runat="server">

    <meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no, minimal-ui" />
    <meta name="format-detection" content="telephone=no" />
    <meta http-equiv="Cache-Control" content="no-transform" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <script type="text/javascript">
        (function (d, c) { var e = d.documentElement, b = "orientationchange" in window ? "orientationchange" : "resize", a = function () { var f = e.clientWidth; if (!f) { return } if (f > 640) { f = 640 } e.style.fontSize = 100 * (f / 640) + "px" }; if (!d.addEventListener) { return } c.addEventListener(b, a, false); d.addEventListener("DOMContentLoaded", a, false) })(document, window);
    </script>
    <style>
        .clearfix:after {
            content: "";
            display: block;
            clear: both;
            width: 100%;
            height: 0;
            line-height: 0;
            font-size: 0;
        }

        .clear, .clearfix:after {
            content: "";
            display: block;
            clear: both;
            width: 100%;
            height: 0;
            line-height: 0;
            font-size: 0;
        }

        .shared {
            margin:20px auto 0;width: 300px;font-family: "microsoft yahei";
        }

            .shared .topTit {
                margin-bottom:5px;
                font-size:18px;
                text-indent:0px;padding-left:10px;
            }
            .shared .topTit b{color:#d02614;font-weight: bold;padding: 0 5px;}
            .shared .fenxiang{    background-color: #f1f1f1;}
            .shared .fenxiang .img {
                float: left;
                width: 100px;
                height: 100px;
                margin:8px;
            }

            .shared .fenxiang .txt {
                float: left;
                width:180px;
                height: 114px;
                line-height: 0;
                
            }

                .shared .fenxiang .txt .name {
                    display:block;
                    padding-top:5px;
                    font-size:14px;
                    line-height:20px;
                    height:60px;
                }

                .shared .fenxiang .txt .price {
                    font-size:18px;
                }

                    .shared .fenxiang .txt .price .t1 {
                        font-size:18px;
                        color: #f55;
                    }

                    .shared .fenxiang .txt .price .t2 {
                        margin-left:5px;
                        font-size: 14px;
                        text-decoration: line-through;
                        color: #999;
                    }
            .btn{width:100%;height:150px;display: block;position: absolute;left:0;bottom:20px;background: url(btn1.png) no-repeat center bottom;}
        /*字体*/
        @media only screen and (min-width:641px) {
            html {
                font-size: 100px;
            }
        }

        @media only screen and (max-width:640px) {
            html {
                font-size: 100px;
            }
        }

        @media only screen and (max-width:540px) {
            html {
                font-size: 84.375px;
            }
        }

        @media only screen and (max-width:480px) {
            html {
                font-size: 75px;
            }
        }

        @media only screen and (max-width:414px) {
            html {
                font-size: 64.6875px;
            }
        }

        @media only screen and (max-width: 400px) {
            html {
                font-size: 62.5px;
            }
        }

        @media only screen and (max-width: 375px) {
            html {
                font-size: 58.5938px;
            }
        }

        @media only screen and (max-width: 360px) {
            html {
                font-size: 56.25px;
            }
        }

        @media only screen and (max-width: 320px) {
            html {
                font-size: 50px;
            }
        }
    </style>
</head>
<body style="width: 320px; height: 256px; margin: 0 auto; background: url(01.png) no-repeat center top;position: relative;">   
    <div class="shared">
        <div class="topTit">送你一件<%if (reservePrice <= 0)
                                    {%><b>免费</b><%} %>商品</div>
        <div class="fenxiang clearfix">
            <img class="img" src="<%=JWShop.Common.ShopCommon.ShowImage(barGainProduct.Photo)%>" />
            <div class="txt">
                <span class="name"><%=barGainProduct.Name %></span>
                <p class="price clearfix"><span class="t1">￥<%=reservePrice %></span> <span class="t2"><%=barGainProduct.MarketPrice %></span></p>
            </div>
        </div>
    </div>
    <div class="btn"></div>
</body>
</html>
