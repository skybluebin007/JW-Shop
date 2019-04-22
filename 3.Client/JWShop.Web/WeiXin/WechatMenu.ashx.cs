using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace JWShop.Web.WeiXin
{
    /// <summary>
    /// WechatMenu 的摘要说明
    /// </summary>
    public class WechatMenu : IHttpHandler
    {
        protected string Appid { get { return ShopConfig.ReadConfigInfo().AppID; } }
        protected string Appsecret { get { return ShopConfig.ReadConfigInfo().Appsecret; } }
        public void ProcessRequest(HttpContext context)
        {
            //获取access_token
            string access_Token = WechatCommon.IsExistAccess_Token();
            if (!string.IsNullOrEmpty(access_Token) && access_Token != "error") {
                 #region 删除原菜单            
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", access_Token);
                    //HTTP get 请求
                WebRequestPostOrGet(url, "");
                #endregion
               
               
                #region 创建菜单
                 url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_Token);
                List<Menu> menus = new List<Menu>();
          
            List<WechatMenuInfo> topMenuList = WechatMenuBLL.ReadRootList();
            //subbuttons.Clear();
            foreach (var topmenu in topMenuList) {
                List<SubButton> subbuttons = new List<SubButton>();
                foreach (var submenu in WechatMenuBLL.ReadChildList(topmenu.Id))
                {
                    //if (submenu.Type == "click")
                    //{
                    //    subbuttons.Add(new SubButton { name = submenu.Name, type = submenu.Type, key = submenu.Key });
                    //}
                    //else
                    //{
                    subbuttons.Add(new SubButton { name = submenu.Name, type = submenu.Type, url = submenu.Url, key = submenu.Key });
                    //}                    
                }
                //if (subbuttons.Count > 0)
                //{//如果有二级菜单
                //    menus.Add(new Menu { name = topmenu.Name, sub_button = subbuttons.ToArray<Button>() });
                //}
                //else
                //{
                    menus.Add(new Menu { name = topmenu.Name, type = topmenu.Type, key = topmenu.Key, url = topmenu.Url, sub_button = subbuttons.ToArray<Button>() });
                //}
            }
            #region
            //           menus.Add(
 //               new Menu()
 //               {
 //                   name = "微商城",
 //                   sub_button = new SubButton[]
 //       {
 //           new SubButton()
 //           {
 //               name = "首页",
 //               url = "http://dzwz18.ncms5.hnjing.net/mobile",
 //               type = "view"
 //           },
 //           new SubButton()
 //           {
 //               name = "分类",
 //               url = "http://dzwz18.ncms5.hnjing.net/Mobile/ProductClass.html",
 //               type = "view"
 //           }
 //       }
 //               });
 //           menus.Add(
 //                new Menu()
 //                {
 //                    name = "个人中心",
 //                    sub_button = new SubButton[]
 //       {
 //           new SubButton()
 //           {
 //               name = "登录",
 //               url = "http://dzwz18.ncms5.hnjing.net/Mobile/User/login.html",
 //               type = "view"
 //           },
 //           new SubButton()
 //           {
 //               name = "注册",
 //               url = "http://dzwz18.ncms5.hnjing.net/Mobile/User/Register.html",
 //               type = "view"
 //           },
 //           new SubButton()
 //           {
 //               name = "我的竞网",
 //               url = "http://dzwz18.ncms5.hnjing.net/Mobile/User/index.html",
 //               type = "view"
 //           }
 //       }
 //                });
 //           menus.Add(
 //new Menu()
 //{
 //    name = "用户服务",
 //    sub_button = new SubButton[]
 //       {
 //           new SubButton()
 //           {
 //               name = "购物车",
 //               url = "http://dzwz18.ncms5.hnjing.net/Mobile/cart.html",
 //               type = "view"
 //           },
 //           new SubButton()
 //           {
 //               name = "找回密码",
 //               url = "http://dzwz18.ncms5.hnjing.net/Mobile/User/FindPasswordInit.html",
 //               type = "view"
 //           },  
 //            new SubButton()
 //           {
 //               name = "男装搜索",
 //              key="男装",
 //               type = "click"
 //           }    
 //       }
            //});
            #endregion

            WeixinMenu weixinMenu = new WeixinMenu() { button = menus };

  //参数
            string param = JsonConvert.SerializeObject(weixinMenu);
            byte[] bs = Encoding.UTF8.GetBytes(param);    //参数转化为ascii码

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);  //创建request
          
            req.Method = "POST";    //确定传值的方式，此处为post方式传值

            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            req.ContentLength = bs.Length;

            using (Stream reqStream = req.GetRequestStream())
            {

                reqStream.Write(bs, 0, bs.Length);
            }
                 string responseString = string.Empty;
            using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
            {
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                 responseString = sr.ReadToEnd();
            }
            ReturnMsg msg = JsonConvert.DeserializeObject<ReturnMsg>(responseString);
            if (msg.errcode != "0")
            {
                //context.Response.Write("系统忙，请稍后重试");
                context.Response.Write("error|"+msg.errcode+":"+ msg.errmsg);
                context.Response.End();
            }
            else
            {
                context.Response.Write("ok|自定义菜单成功");
                context.Response.End();
            }
                #endregion
            }
        }

        [Serializable]
        public class Button
        {
            public string name { get; set; }
        }
        /// <summary>
        /// 二级菜单
        /// </summary>
        [Serializable]
        public class SubButton:Button
        {
            //public string name { get; set; }
            public string type { get; set; }
            public string key { get; set; }
            public string url { get; set; }
        }
        /// <summary>
        /// 微信菜单（一个）
        /// </summary>
        [Serializable]
        public class Menu : SubButton
        {
            //public string name { get; set; }
            public Button[] sub_button { get; set; }
            //public List<SubButton> sub_button { get; set; }
        }
        /// <summary>
        /// 微信菜单(总)
        /// </summary>
        [Serializable]
        public class WeixinMenu
        {
            public List<Menu> button { get; set; }
        }
        /// <summary>
        /// 返回消息
        /// </summary>
        protected class ReturnMsg { 
            public string errcode{get;set;}
            public string errmsg{get;set;}
       }

        /// <summary>
        /// 服务器端发起HTTP请求
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_method">GET  POST</param>
        /// <returns></returns>
        protected string WebRequestPostOrGet(string _url, string _method)
        {
            string responseString = string.Empty;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(_url);

            myRequest.Method = string.IsNullOrEmpty(_method) ? "GET" : "POST";
            myRequest.ContentType = "text/html;charset=utf-8";

            using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
            {
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                responseString = sr.ReadToEnd();
            }
            return responseString;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}