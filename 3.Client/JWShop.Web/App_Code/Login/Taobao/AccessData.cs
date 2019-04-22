using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace JWShop.Login.Taobao
{
    /// <summary>
    /// 请求淘宝访问令牌返回的数据
    /// </summary>
    public class AccessData
    {
        public string w2_expires_in = string.Empty;
        public string taobao_user_id = string.Empty;
        public string taobao_user_nick = string.Empty;
        public string w1_expires_in = string.Empty;
        public string re_expires_in = string.Empty;
        public string r2_expires_in = string.Empty;
        public string expires_in = string.Empty;
        public string token_type = string.Empty;
        public string refresh_token = string.Empty;
        public string access_token = string.Empty;
        public string r1_expires_in = string.Empty;     
    }
}
