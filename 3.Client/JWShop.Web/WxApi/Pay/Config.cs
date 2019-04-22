using System;
using System.Collections.Generic;
using System.Web;
using SkyCES.EntLib;

namespace JWShop.XcxApi.Pay
{
    /**
    * 	配置账号信息 - 原始 start
    */
    //public class WxPayConfig
    //{
    //    //=======【基本信息设置】=====================================
    //    /* 微信公众号信息配置
    //    * APPID：绑定支付的APPID（必须配置）
    //    * MCHID：商户号（必须配置）
    //    * KEY：商户支付密钥，参考开户邮件设置（必须配置）
    //    * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
    //    */
    //    public const string APPID = "wx2428e34e0e7dc6ef";
    //    public const string MCHID = "1233410002";
    //    public const string KEY = "e10adc3849ba56abbe56e056f20f883e";
    //    public const string APPSECRET = "51c56b886b5be869567dd389b3e5d1d6";

    //    //=======【证书路径设置】===================================== 
    //    /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
    //    */
    //    public const string SSLCERT_PATH = "cert/apiclient_cert.p12";
    //    public const string SSLCERT_PASSWORD = "1233410002";



    //    //=======【支付结果通知url】===================================== 
    //    /* 支付结果通知回调url，用于商户接收支付结果
    //    */
    //    public const string NOTIFY_URL = "http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx";

    //    //=======【商户系统后台机器IP】===================================== 
    //    /* 此参数可手动配置也可在程序中自动获取
    //    */
    //    public const string IP = "8.8.8.8";


    //    //=======【代理服务器设置】===================================
    //    /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
    //    */
    //    public const string PROXY_URL = "http://10.152.18.220:8080";

    //    //=======【上报信息配置】===================================
    //    /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
    //    */
    //    public const int REPORT_LEVENL = 1;

    //    //=======【日志级别】===================================
    //    /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
    //    */
    //    public const int LOG_LEVENL = 0;
    //}
    /**
    * 	配置账号信息 - 原始 end
    */


    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        private static string _APPID = "";
        private static string _MCHID = "";
        private static string _KEY = "apihnjing4000731777qwertyuiopasd";
        private static string _APPSECRET = "";
        private static string _SERVERAPPID = "wxe879bf9bef9db6cc";
        private static string _SERVERMCHID = "1492892432";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public static string SSLCERT_PATH = "Plugins/Pay/WxPay/cert/apiclient_cert.p12";
        public static string SSLCERT_PASSWORD { get { return _SERVERMCHID; } }



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
         * https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_3
         * 证书文件不能放在web服务器虚拟目录，应放在有访问权限控制的目录中，防止被他人下载。商户服务器要做好病毒和木马防护工作，不被非法侵入者窃取证书文件。
        */
        private static string _NOTIFY_URL = "";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public static string IP { get { return "8.8.8.8"; } }


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public static string PROXY_URL { get { return "http://10.152.18.220:8080"; } }

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public static int REPORT_LEVENL { get { return 1; } }

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public static int LOG_LEVENL { get { return 3; } }


        static WxPayConfig()
        {
            using (XmlHelper xh = new XmlHelper(ServerHelper.MapPath("/Plugins/Pay/WxPay/WxPay.Config")))
            {
                _APPID = xh.ReadAttribute("Pay/Partner", "Value");
                _MCHID = xh.ReadAttribute("Pay/MCHID", "Value");
                //_KEY = xh.ReadAttribute("Pay/SecurityKey", "Value"); //服务商支付使用服务商支付密钥
                _APPSECRET = xh.ReadAttribute("Pay/APPSECRET", "Value");
                _NOTIFY_URL = xh.ReadAttribute("Pay/SiteDomain", "Value");                
            }
        }

        /// <summary>
        /// APPID：绑定支付的APPID（必须配置）
        /// </summary>
        public static string APPID
        {
            get { return _APPID; }
        }
        /// <summary>
        /// MCHID：商户号（必须配置）
        /// </summary>
        public static string MCHID
        {
            get { return _MCHID; }
        }
        /// <summary>
        /// KEY：商户支付密钥，参考开户邮件设置（必须配置）
        /// </summary>
        public static string KEY
        {
            get { return _KEY; }
        }
        /// <summary>
        /// APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        /// </summary>
        public static string APPSECRET
        {
            get { return _APPSECRET; }
        }

        public static string SERVERAPPID
        {
            get { return _SERVERAPPID; }
        }
        public static string SERVERMCHID
        {
            get { return _SERVERMCHID; }
        }
        /// <summary>
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        public static string NOTIFY_URL
        {
            get { return "http://" + _NOTIFY_URL.Trim() + "/Plugins/Pay/WxPay/NotifyPay.aspx"; }
            //直接使用请求的域名，无需后台填写
            //get { return "http://" + HttpContext.Current.Request.Url.Host + "/Plugins/Pay/WxPay/NotifyPay.aspx"; }            
        }

    }
}