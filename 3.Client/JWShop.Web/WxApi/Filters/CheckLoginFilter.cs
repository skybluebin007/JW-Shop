using System.Web.Mvc;
using System.Web.Routing;
using SkyCES.EntLib;
using System.Xml;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JWShop.XcxApi.Filter
{
    public class CheckLoginAttribute : FilterAttribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext authContext)
        {
            var re_uid = authContext.HttpContext.Request["uid"];
            var context = authContext.HttpContext;
            if ("post".Equals(context.Request.HttpMethod.ToLower()) && re_uid == null)
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                //token验证已读取过stream，需要重新设置读取位置
                reader.DiscardBufferedData();
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                reader.BaseStream.Position = 0;
                string json = HttpUtility.UrlDecode(reader.ReadToEnd());
                var jobj = (JObject)JsonConvert.DeserializeObject(json);
                if (jobj["uid"] != null)
                    re_uid = jobj["uid"].ToString();
            }
            int uid = 0;
            if (int.TryParse(re_uid, out uid))
            {
                if (uid <= 0)
                {
                    authContext.Result = new ContentResult()
                    {
                        Content = "error userid"
                    };
                }
                else
                {
                    if (UserBLL.Read(uid).Id <= 0)
                    {
                        authContext.Result = new ContentResult()
                        {
                            Content = "no user"
                        };
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                authContext.Result = new ContentResult()
                {
                    Content = "error userid"
                };
            }
            
        }
    }
}
