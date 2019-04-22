using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeetestSDK;

namespace JWShop.Web.Admin
{
    public partial class getcaptcha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            Response.Write(getCaptcha());
            Response.End();
        }
        private String getCaptcha()
        {
            GeetestLib geetest = new GeetestLib("b46d1900d0a894591916ea94ea91bd2c", "36fc3fe98530eea08dfc6ce76e3d24c4");
            String userID = "test";
            Byte gtServerStatus = geetest.preProcess(userID);
            Session[GeetestLib.gtServerStatusSessionKey] = gtServerStatus;
            Session["userID"] = userID;
            return geetest.getResponseStr();
        }
    }
}