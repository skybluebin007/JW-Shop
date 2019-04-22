using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using LitJson;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Data;
using System.Xml;
using System.Collections;

namespace JWShop.Common
{
    /// <summary>
    /// 微信共用：获取Access_Token,存储到"config\access_token.xml"文件
    /// </summary>
    public class WechatCommon
    {
        private static string appId=ShopConfig.ReadConfigInfo().AppID;
        private static string appSecret = ShopConfig.ReadConfigInfo().Appsecret;
        ////构造函数
        //public WechatCommon(string appId, string appSecret)
        //{
        //    this.appId = appId;
        //    this.appSecret = appSecret;
        //}


        //得到数据包，返回使用页面  
        public System.Collections.Hashtable getSignPackage()
        {
            string jsapiTicket = IsExistTicket();
            string url = "http://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.RawUrl.ToString();
            string timestamp = Convert.ToString(ConvertDateTimeInt(DateTime.Now));
            string nonceStr = createNonceStr();


            // 这里参数的顺序要按照 key 值 ASCII 码升序排序  
            string rawstring = "jsapi_ticket=" + jsapiTicket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + url + "";


            string signature = SHA1_Hash(rawstring);


            System.Collections.Hashtable signPackage = new System.Collections.Hashtable();
            signPackage.Add("appId", appId);
            signPackage.Add("nonceStr", nonceStr);
            signPackage.Add("timestamp", timestamp);
            signPackage.Add("url", url);
            signPackage.Add("signature", signature);
            signPackage.Add("rawString", rawstring);


            return signPackage;
        }


        //创建随机字符串  
        private string createNonceStr()
        {
            int length = 16;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }


        //得到ticket 如果文件里时间 超时则重新获取
        /*
        private string getJsApiTicket()
        {
            string ticket = string.Empty;
            //这里我从数据库读取
            //  DT = DbSession.Default.FromSql("select jsapi_ticket,ticket_expires from table where ID=1").ToDataTable();
            //  int expire_time = (int)DT.Rows[0]["ticket_expires"];
            // string ticket = DT.Rows[0]["jsapi_ticket"].ToString();
            int expire_time = ShopConfig.ReadConfigInfo().ExpireTime;
            ticket = ShopConfig.ReadConfigInfo().Ticket;

            string accessToken = GetAccessToken(appId,appSecret);//获取系统的全局token 
            if (expire_time < ConvertDateTimeInt(DateTime.Now))
            {
                string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + accessToken + "";
                Jsapi api = JsonConvert.DeserializeObject<Jsapi>(httpGet(url).Trim());
                ticket = api.ticket;
                if (ticket != "")
                {
                    expire_time = ConvertDateTimeInt(DateTime.Now) + 7000;
                    //存入数据库操作

                    ShopConfigInfo config = ShopConfig.ReadConfigInfo();
                    config.Ticket = ticket;
                    config.ExpireTime = expire_time;

                    ShopConfig.UpdateConfigInfo(config);
                }
            }
            return ticket;
        }
         * */
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
            string filepath = System.Web.HttpContext.Current.Server.MapPath(@"/Config/access_token.xml");
            string errorfilepath = System.Web.HttpContext.Current.Server.MapPath(@"/Config/error.xml");

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
                
                XmlElement node = errorXml.CreateElement("access_token");
                node.InnerText = mode.access_token;
                errorXml.SelectSingleNode("error").AppendChild(node);
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
        public static Access_token GetAccess_token()
        {         
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret;
            string result = httpGet(url);
            Access_token mode = new Access_token();
            if (!string.IsNullOrEmpty(result))
            {
                JsonData jd = JsonMapper.ToObject(result);

                if (!((IDictionary)jd).Contains("errmsg"))
                {
                    mode.access_token = (string)jd["access_token"];
                    mode.expires_in = (int)jd["expires_in"];
                }
            }
            return mode;
        }
        public static string IsExistTicket()
        {
            string Ticket = string.Empty;
            DateTime TicketExpire;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            string filepath = System.Web.HttpContext.Current.Server.MapPath(@"/Config/ticket.xml");

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
        public static Ticket getJsApiTicket()
        {          
            string accessToken = IsExistAccess_Token();

            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + accessToken;

            string result = httpGet(url);

            JsonData jd = JsonMapper.ToObject(result);

            Ticket mode = new Ticket();
            if ((string)jd["errmsg"] == "ok")
            {
                mode.ticket = (string)jd["ticket"];
                mode.TicketExpire = (int)jd["expires_in"];
            }

            return mode;
        }
        //SHA1哈希加密算法  
        public string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = System.Text.UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>double</returns>  
        public int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt32((time - startTime).TotalSeconds);
            return intResult;
        }

        ////得到accesstoken 如果文件里时间 超时则重新获取  
        //public static string GetAccessToken( string appId,string appSecret )
        //{
        //    try
        //    {
        //        // access_token 应该全局存储与更新，以下代码以写入到文件中做示例
        //        string access_token = "";
        //        AccToken readJSTicket = new AccToken();
        //        string path = HttpContext.Current.Server.MapPath(@"/Config/access_token.json");
        //        //FileStream file = new FileStream(path, FileMode.Open);
        //        //var serializer = new DataContractJsonSerializer(typeof(AccToken));
        //        //AccToken readJSTicket = (AccToken)serializer.ReadObject(file);
        //        //file.Close();
        //        string strRead = File.ReadAllText(path);
        //        if (!string.IsNullOrEmpty(strRead))
        //        {
        //            readJSTicket = JsonConvert.DeserializeObject<AccToken>(strRead);
        //        }
                
        //        if (readJSTicket.expires_in < ShopCommon.ConvertDateTimeInt(DateTime.Now))
        //        {
        //            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret + "";
        //            string str = httpGet(url);

        //            //有时候版本不同上面的有错误，可以试试这种

        //            AccToken iden = new AccToken();
        //            string jsontext = httpGet(url).Trim();

        //            iden = (AccToken)JsonConvert.DeserializeObject(jsontext, typeof(AccToken));

        //            access_token = iden.access_token;
        //            if (!string.IsNullOrEmpty(access_token))
        //            {
        //                iden.expires_in = ShopCommon.ConvertDateTimeInt(DateTime.Now) + 7000;
        //                iden.access_token = access_token;

        //                //  string json = Serialize<AccToken>(iden);
        //                string json = JsonConvert.SerializeObject(iden);
        //                //   string json = "{\"access_token\":"+iden.access_token+",\"expires_in\":"+iden.expires_in+"}";
        //                StreamWriterMetod(json, path);
        //            }
        //        }
        //        else
        //        {
        //            access_token = readJSTicket.access_token;
        //        }
        //        return access_token;
        //    }
        //    catch(Exception ex){
               
        //        return string.Empty;
        //    }
        //}


        //发起一个http请球，返回值  
        private static string httpGet(string url)
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据  
                string pageHtml = System.Text.Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句              

                return pageHtml;
            }


            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
                return null;
            }
        }


        /// <summary>  
        /// StreamWriter写入文件方法  
        /// </summary>  
        private static void StreamWriterMetod(string str, string fileName)
        {
            try
            {
                //FileStream fsFile = new FileStream(path, FileMode.OpenOrCreate);
                //StreamWriter swWriter = new StreamWriter(fsFile);
                //swWriter.WriteLine(str);
                //swWriter.Close();
                //如果存在则先删除该文件
                if (File.Exists(fileName))
                {
                  System.IO.File.Delete(fileName);  //删除文件                 
                }
                if (!File.Exists(fileName))
                {
                    FileStream fs = System.IO.File.Create(fileName);  //创建文件
                    fs.Close();
                }
           
                StreamWriter sw = new StreamWriter(fileName);  //创建写入流
                sw.Write(str);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

    //创建Json序列化 及反序列化类目  
    #region
    //创建JSon类 保存文件 jsapi_ticket.json  

    //public class JSTicket
    //{

    //    public string jsapi_ticket { get; set; }

    //    public double expire_time { get; set; }
    //}
    ////创建 JSon类 保存文件 access_token.json  

    //public class AccToken
    //{

    //    public string access_token { get; set; }

    //    public double expires_in { get; set; }
    //}


    //创建从微信返回结果的一个类 用于获取ticket  

    public class Jsapi
    {

        public int errcode { get; set; }

        public string errmsg { get; set; }

        public string ticket { get; set; }

        public string expires_in { get; set; }
    }

    public class Ticket
    {
        string _ticket;
        int _ticketExpire;

        /// <summary>
        /// 获取到的凭证 
        /// </summary>
        public string ticket
        {
            get { return _ticket; }
            set { _ticket = value; }
        }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int TicketExpire
        {
            get { return _ticketExpire; }
            set { _ticketExpire = value; }
        }
    }
    /// <summary>
    ///Access_token 的摘要说明
    /// </summary>
    public class Access_token
    {
        public Access_token()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        string _access_token;
        double _expires_in;

        /// <summary>
        /// 获取到的凭证 
        /// </summary>
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public double expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
    }
    #endregion


}
