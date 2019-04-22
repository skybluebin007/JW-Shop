using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace JWShop.XcxApi.Pay
{
    public class WxGetInfo
    {
        public static string appId; // 必填，公众号的唯一标识
        public static string timestamp; // 必填，生成签名的时间戳
        public static string nonceStr; // 必填，生成签名的随机串
        public static string signature;
        public static string ticket;
        public static string url;

        protected string access_token;

        public static void Inition()
        {
            appId = WxPayConfig.APPID;
            timestamp = WxPayApi.GenerateTimeStamp();
            nonceStr = WxPayApi.GenerateNonceStr();
        }

        public static Access_token GetAccess_token()
        {
            Inition();
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + WxPayConfig.APPSECRET;
            string result = HttpService.Get(url);
            Access_token mode = new Access_token();
            if (!string.IsNullOrEmpty(result))
            {
                JObject jd = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                if (jd.Property("access_token")!=null)
                {
                    mode.access_token = (string)jd["access_token"];
                    mode.expires_in = (int)jd["expires_in"];
                }
                else
                {
                    Log.Debug("GET ACCESS_TOKEN:", result + " url:" + url);
                }
            }
            return mode;
        }

        public static Ticket getJsApiTicket()
        {
            Inition();
            string accessToken = IsExistAccess_Token();

            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + accessToken;

            string result = HttpService.Get(url);

            JObject jd = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);

            Ticket mode = new Ticket();
            if ((string)jd["errmsg"] == "ok")
            {
                mode.ticket = (string)jd["ticket"];
                mode.TicketExpire = (int)jd["expires_in"];
            }

            return mode;
        }

        /// <summary>
        /// 根据当前日期 判断Access_Token 是否超期 如果超期返回新的Access_Token 否则返回之前的Access_Token
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string IsExistAccess_Token()
        {

            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            string filepath = System.Web.HttpContext.Current.Server.MapPath("/XMLFile.xml");
            string errorfilepath = System.Web.HttpContext.Current.Server.MapPath("/Error.xml");

            StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();            
            xml.Load(str);
            str.Close();
            str.Dispose();

            StreamReader errorstr = new StreamReader(errorfilepath, System.Text.Encoding.UTF8);
            XmlDocument errorXml = new XmlDocument();
            errorXml.Load(errorstr);
            errorstr.Close();
            errorstr.Dispose();

            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);

            errorXml.SelectSingleNode("error").SelectSingleNode("errorstr").InnerText = "date later(" + DateTime.Now.AddMinutes(-2) + ") than now(" + YouXRQ + ")";      
            if (DateTime.Now.AddMinutes(-2) > YouXRQ)
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_token();

                //XmlElement node = errorXml.CreateElement("access_token");
                //node.InnerText = mode.access_token;
                //errorXml.SelectSingleNode("error").AppendChild(node);

                if (!string.IsNullOrEmpty(mode.access_token))
                {
                    xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
                    _youxrq = _youxrq.AddSeconds(mode.expires_in);
                    xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
                    xml.Save(filepath);
                    Token = mode.access_token;
                }
            }
            errorXml.Save(errorfilepath);
            return Token;
        }

        public static string IsExistTicket()
        {
            string Ticket = string.Empty;
            DateTime TicketExpire;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            string filepath = System.Web.HttpContext.Current.Server.MapPath("/Vshop/XMLFile.xml");

            StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Ticket = xml.SelectSingleNode("xml").SelectSingleNode("Ticket").InnerText;
            TicketExpire = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("TicketExpire").InnerText);

            if (DateTime.Now.AddMinutes(-2) > TicketExpire)
            {
                DateTime _youxrq = DateTime.Now;
                Ticket mode = getJsApiTicket();
                if (!string.IsNullOrEmpty(mode.ticket))
                {
                    xml.SelectSingleNode("xml").SelectSingleNode("Ticket").InnerText = mode.ticket;
                    _youxrq = _youxrq.AddSeconds(mode.TicketExpire);
                    xml.SelectSingleNode("xml").SelectSingleNode("TicketExpire").InnerText = _youxrq.ToString();
                    xml.Save(filepath);
                    Ticket = mode.ticket;
                }
            }
            return Ticket;
        }

        //发起一个http请球，返回值  
        private string httpGet(string url)
        {
            try
            {


                WebClient MyWebClient = new WebClient();


                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  


                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据  


                string pageHtml = System.Text.Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句              


                //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句  


                return pageHtml;


            }


            catch (WebException webEx)
            {


                Console.WriteLine(webEx.Message.ToString());
                return null;
            }
        } 
    }
}
