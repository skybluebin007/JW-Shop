﻿using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using JWShop.Business;
using JWShop.Common;

namespace JWShop.Web.Admin
{
    public partial class Right : JWShop.Page.AdminBasePage
    {
        protected DataTable dt = new DataTable();

        protected string userCount = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            dt = ProductBLL.NoHandlerStatistics();
        }
    }
}