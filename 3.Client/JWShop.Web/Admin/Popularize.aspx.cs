using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Data;
using System.Data.SqlClient;

namespace JWShop.Web.Admin
{
    public partial class Popularize : JWShop.Page.AdminBasePage
    {
        protected CommonPagerClass commonPager = new CommonPagerClass();
        protected int cid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            cid = RequestHelper.GetQueryString<int>("CID");
            
        }
    }
}