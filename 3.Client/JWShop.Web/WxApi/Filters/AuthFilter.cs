using System.Web.Mvc;
using System.Web.Routing;
using SkyCES.EntLib;
using System.Xml;
using JWShop.Business;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JWShop.XcxApi.Filter
{
    public class AuthAttribute:FilterAttribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext authContext)
        {
            string token = authContext.HttpContext.Request["token"];
            var context = authContext.HttpContext;
            if ("post".Equals(context.Request.HttpMethod.ToLower())&&token==null)
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string json = HttpUtility.UrlDecode(reader.ReadToEnd());
                var jobj = (JObject)JsonConvert.DeserializeObject(json);
                if (jobj["token"] != null)
                    token = jobj["token"].ToString();
            }            
            if (!string.IsNullOrEmpty(token))
            {
                string checkStr = string.Empty;
                string tokenfilepath = ServerHelper.MapPath("/WxApi/Content/xcxtoken.xml");
                XmlHelper xml = new XmlHelper(tokenfilepath);
                XmlNode rootNode = xml.ReadNode("TokenCode");
                if (rootNode == null)
                {
                    checkStr = "xml error";
                }
                else
                {
                    //var admin = AdminBLL.Read(1);
                    string tokenstr = rootNode.InnerText;                    
                    string oritoken = "1|" + tokenstr;
                    oritoken = StringHelper.Password(oritoken, PasswordType.MD532);
                    if (!oritoken.Equals(token))
                    {
                        checkStr = "access denied";
                    }                    
                }
                if (checkStr == string.Empty)
                {
                    return;
                }
                else
                {
                    authContext.Result = new ContentResult()
                    {
                        Content = checkStr
                    };
                }
                
            }
            else
            {
                authContext.Result = new ContentResult()
                {
                    Content = "no autorization"
                };
            }
        }
    }
}
