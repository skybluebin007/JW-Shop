using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;
using System.Web.Security;
using System.IO;
using System.Text;
using System.Xml;

namespace JWShop.Web.WeiXin
{
    /// <summary>
    /// WechatMessage 的摘要说明
    /// </summary>
    public class WechatMessage : IHttpHandler
    {
        protected string Appid { get { return ShopConfig.ReadConfigInfo().AppID; } }
        /// <summary>
        /// 微信登录URL
        /// </summary>
        protected string WechatLoginUrl { get { return ShopConfig.ReadConfigInfo().WechatLoginURL; } }
        /// <summary>
        /// 微信令牌
        /// </summary>
        protected string Token { get { return ShopConfig.ReadConfigInfo().Token; } }
        /// <summary>
        /// 默认回复消息
        /// </summary>
        protected string DefaultReply { get { return ShopConfig.ReadConfigInfo().DefaultReply; } }
        /// <summary>
        /// 关注回复标题
        /// </summary>
        protected string AttentionTitle { get { return ShopConfig.ReadConfigInfo().AttentionTitle; } }
        /// <summary>
        /// 关注回复内容
        /// </summary>
        protected string AttentionSummary { get { return ShopConfig.ReadConfigInfo().AttentionSummary; } }
        /// <summary>
        /// 关注回复图片
        /// </summary>
        protected string AttentionPicture { get { return ShopConfig.ReadConfigInfo().AttentionPicture; } }

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            if (context.Request.HttpMethod.ToLower().Equals("get"))
            //if (!string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("signature")))
            {
                //开发者通过检验signature对请求进行校验（下面有校验方式）。
                //若确认此次GET请求来自微信服务器，请原样返回echostr参数内
                //容，则接入生效，成为开发者成功，否则接入失败。 
                //context.Response.Write(msg);
                checkSignature(context);
                //用于微信接口的URL校验
            }
            else
            {//接收微信服务器post来的XML

                ResponseXML(context);//回复消息

            }
        }
        /// <summary>
        /// 检查签名
        /// </summary>
        private void checkSignature(HttpContext context)
        {
            string signature = RequestHelper.GetQueryString<string>("signature");
            string timestamp = RequestHelper.GetQueryString<string>("timestamp");
            string nonce = RequestHelper.GetQueryString<string>("nonce");
            string echostr = context.Request["echostr"];
            string token = this.Token;

            //加密/校验流程如下：

            string[] temp1 = { token, timestamp, nonce };
            //1. 将token、timestamp、nonce三个参数进行字典序排序
            Array.Sort(temp1);//排序
            //2. 将三个参数字符串拼接成一个字符串进行sha1加密
            string temp2 = string.Join("", temp1);
            string temp3 = FormsAuthentication.HashPasswordForStoringInConfigFile(temp2, "SHA1");

            //3. 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信
            //SHA1有大小写区别，先转成小写再对比
            if (temp3.ToLower().Equals(signature))
            {
                context.Response.Write(echostr);
                //如果相同就返回微信服务器要求的signature，不相同就没有必要处理
                context.Response.End();
            }

        }
        /// 获取用户发送的消息
        private void ResponseXML(HttpContext context)
        {           
            //接收XML数据包
            Stream XmlStream = context.Request.InputStream;
            //构造xml对象
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlStream);
            XmlElement rootElement = xmlDoc.DocumentElement;//获取文档的根 
            XmlNode MsgType = rootElement.SelectSingleNode("MsgType"); //获取消息的文本类型
            WxXmlModel requestXML = new WxXmlModel();//声明实例，获取各个属性并赋值
            requestXML.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;//公众号
            requestXML.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;//用户
            requestXML.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;//创建时间
            requestXML.MsgType = MsgType.InnerText;//消息类型
            ///对消息的不同类型进行赋值
            if (requestXML.MsgType == "text")
            {
                //赋值文本信息内容
                requestXML.Content = rootElement.SelectSingleNode("Content").InnerText;

            }
            if (requestXML.MsgType.Trim() == "location")
            {
                ///赋值地理位置纬度，经度，地图缩放比例，地理位置说明
                requestXML.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
                requestXML.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
                requestXML.Scale = rootElement.SelectSingleNode("Scale").InnerText;
                requestXML.Label = rootElement.SelectSingleNode("Label").InnerText;
            }
            if (requestXML.MsgType.Trim().ToLower() == "event")
            {
                ///赋值事件名称和事件key值
                requestXML.Event = rootElement.SelectSingleNode("Event").InnerText;
                requestXML.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;

            }
            ResponseMsg(context,requestXML);

        }

        private void ResponseMsg(HttpContext context,WxXmlModel requestXML)
        {
            string MsgType = requestXML.MsgType;
            string xmlMsg = string.Empty;
            try
            {
                //根据消息类型判断发送何种类型消息
                switch (MsgType)
                {
                    case "text":
                        //SendTextCase(requestXML);//发送文本消息
                     
                        string _url = string.Empty;
                        if (requestXML.Content.IndexOf("分类") > -1 || requestXML.Content.IndexOf("类别") > -1)
                        {
                             _url = context.Request.Url.Host + "/mobile/ProductClass.html";
                            xmlMsg = "<xml>" + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
   + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
   + "<CreateTime>" + GetCreateTime() + "</CreateTime>"
   + "<MsgType><![CDATA[news]]></MsgType>"
   + "<ArticleCount>1</ArticleCount>"
   + "<Articles><item>"
   + "<Title><![CDATA[商品分类]]></Title>"
+ "<Description><![CDATA[商品分类]]></Description>"
+ "<Url><![CDATA[" + _url + "]]></Url>"
+ "</item></Articles>"
   + "</xml>";
                        }
                        else if (requestXML.Content.IndexOf("homepage") > -1 || requestXML.Content.IndexOf("官网") > -1 || requestXML.Content.IndexOf("首页") > -1)
                        {
                            _url = context.Request.Url.Host + "/Mobile/default.html";
                            xmlMsg = "<xml>" + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
   + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
   + "<CreateTime>" + GetCreateTime() + "</CreateTime>"
   + "<MsgType><![CDATA[news]]></MsgType>"
   + "<ArticleCount>1</ArticleCount>"
   + "<Articles><item>"
   + "<Title><![CDATA[微官网]]></Title>"
+ "<Description><![CDATA[微官网]]></Description>"                     
+ "<Url><![CDATA[" + _url + "]]></Url>"
+ "</item></Articles>"
   + "</xml>";
                        }
                        else if (requestXML.Content.IndexOf("会员") > -1 || requestXML.Content.IndexOf("个人") > -1 || requestXML.Content.IndexOf("我") > -1)
                        {
                            _url = context.Request.Url.Host + "/Mobile/user/index.html";
                            xmlMsg = "<xml>" + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
   + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
   + "<CreateTime>" + GetCreateTime() + "</CreateTime>"
   + "<MsgType><![CDATA[news]]></MsgType>"
   + "<ArticleCount>1</ArticleCount>"
   + "<Articles><item>"
   + "<Title><![CDATA[会员中心]]></Title>"
+ "<Description><![CDATA[会员中心]]></Description>"
+ "<Url><![CDATA[" + _url + "]]></Url>"
+ "</item></Articles>"
   + "</xml>";
                        }
                        else
                        {
                            string _keyword = StringHelper.AddSafe(requestXML.Content);
                            var productlist = ProductBLL.SearchList(new ProductSearchInfo { IsSale = (int)BoolType.True, Key = _keyword});
                            if (productlist.Count > 0) {
                                _url = context.Request.Url.Host + "/mobile/list.html?kw="+_keyword;
                                xmlMsg = "<xml>" + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
       + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
       + "<CreateTime>" + GetCreateTime() + "</CreateTime>"
       + "<MsgType><![CDATA[news]]></MsgType>"
       + "<ArticleCount>1</ArticleCount>"
       + "<Articles><item>"
       + "<Title><![CDATA[商品搜索："+_keyword+"]]></Title>"
    + "<Description><![CDATA[约"+productlist.Count+"个]]></Description>"
    + "<Url><![CDATA[" + _url + "]]></Url>"
    + "</item></Articles>"
       + "</xml>";
                            
                            }
                            else
                            {
                                string replycontent = string.IsNullOrEmpty(DefaultReply) ? requestXML.Content : DefaultReply;
                                xmlMsg = "<xml>" + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
                                   + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
                                   + "<CreateTime>" + GetCreateTime() + "</CreateTime>"
                                   + "<MsgType><![CDATA[text]]></MsgType>"
                                   + "<Content><![CDATA[" + replycontent + "]]></Content>"
                                   + "</xml>";
                            }

                        }
                         context.Response.Clear();
                            context.Response.Write(xmlMsg);
                            context.Response.End();
                        break;
                    case "event"://发送事件消息
                        if (!string.IsNullOrWhiteSpace(requestXML.Event) && requestXML.Event.ToString().Trim().ToLower().Equals("subscribe"))
                        {
                            //SendWelComeMsg(requestXML);//关注时返回的图文消息
                            string _redirecturi = HttpUtility.UrlDecode(this.WechatLoginUrl);
                            string _state = new Random().Next(0, 9999).ToString();
                             _url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid="+Appid+"&redirect_uri=" + _redirecturi + "&response_type=code&scope=snsapi_userinfo&state=" + _state + "#wechat_redirect";
                            string replytitle = string.IsNullOrEmpty(AttentionTitle) ? ShopConfig.ReadConfigInfo().SiteName : AttentionTitle;
                            string replysummary = string.IsNullOrEmpty(AttentionSummary) ? ShopConfig.ReadConfigInfo().Description : AttentionSummary;
                            string replyimage = string.IsNullOrEmpty(AttentionPicture) ? "/admin/images/welcome.jpg" : AttentionPicture;
                            xmlMsg = "<xml>" + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
                           + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
                           + "<CreateTime>" + GetCreateTime() + "</CreateTime>"
                           + "<MsgType><![CDATA[news]]></MsgType>"
                           + "<ArticleCount>1</ArticleCount>"
                           +"<Articles><item>"
                           + "<Title><![CDATA[" + replytitle + "]]></Title>"
  + "<Description><![CDATA[" + replysummary + "]]></Description>"
+ "<PicUrl><![CDATA[http://" + context.Request.Url.Host +  replyimage + "]]></PicUrl>"
  //+ "<PicUrl><![CDATA[http://" + context.Request.Url.Host + (context.Request.Url.Port > 0 ? ":" + context.Request.Url.Port : "") + replyimage + "]]></PicUrl>"
  + "<Url><![CDATA[" + _url + "]]></Url>"
  + "</item></Articles>"
                           + "</xml>";

                          

                        }
                            //菜单点击事件click
                        else if (!string.IsNullOrWhiteSpace(requestXML.Event) && requestXML.Event.ToString().Trim().ToLower().Equals("click"))
                        {
                            //SendEventMsg(requestXML);//发送事件消息
                            string _keyword = StringHelper.AddSafe(requestXML.EventKey);
                            var productlist = ProductBLL.SearchList(new ProductSearchInfo { IsSale = (int)BoolType.True, Key = _keyword });
                            if (productlist.Count > 0)
                            {
                                _url = context.Request.Url.Host + "/mobile/list.html?kw=" + _keyword;
                                xmlMsg = "<xml>" + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
       + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
       + "<CreateTime>" + GetCreateTime() + "</CreateTime>"
       + "<MsgType><![CDATA[news]]></MsgType>"
       + "<ArticleCount>1</ArticleCount>"
       + "<Articles><item>"
       + "<Title><![CDATA[商品搜索：" + _keyword + "]]></Title>"
    + "<Description><![CDATA[约" + productlist.Count + "个]]></Description>"
    + "<Url><![CDATA[" + _url + "]]></Url>"
    + "</item></Articles>"
       + "</xml>";

                            }
                            else
                            {
                                string replycontent = string.IsNullOrEmpty(DefaultReply) ? requestXML.EventKey : DefaultReply;
                                xmlMsg = "<xml>" + "<ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName>"
                                   + "<FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName>"
                                   + "<CreateTime>" + GetCreateTime() + "</CreateTime>"
                                   + "<MsgType><![CDATA[text]]></MsgType>"
                                   + "<Content><![CDATA[" + replycontent + "]]></Content>"
                                   + "</xml>";
                            }
                        }
                          context.Response.Clear();
                            context.Response.Write(xmlMsg);
                            context.Response.End();
                        break;
                    case "location"://发送位置消息
                        //SendMapMsg(requestXML);
                        break;
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.ToString());
            }
        }
        /// <summary>
        /// 创建时间戳
        /// </summary>
        /// <returns></returns>
        protected int GetCreateTime()
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);//格林威治时间1970，1，1，0，0，0
            return (int)(DateTime.Now - dateStart).TotalSeconds;
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