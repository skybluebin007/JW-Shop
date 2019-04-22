using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JWShop.Web.Admin
{
    public partial class Cnzz : System.Web.UI.Page
    {
        protected string cnzzID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            cnzzID = Request["cnzzID"];
        }
    }
}