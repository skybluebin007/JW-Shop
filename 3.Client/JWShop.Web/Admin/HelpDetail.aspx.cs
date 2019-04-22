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
    public partial class HelpDetail : JWShop.Page.AdminBasePage
    {
        protected DataTable dt = new DataTable();
        protected int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestHelper.GetQueryString<int>("CID");

            //SqlConnection conn = new SqlConnection("server=.;pwd=sa123;uid=sa;database=CasePlatform;");
            //SqlConnection conn = new SqlConnection("server=175.6.5.91,1433;pwd=sasfDSHAsdgh;uid=hnjing.cn;database=CasePlatform;");

            //id = RequestHelper.GetQueryString<int>("ID");
            //if (id <= 0)
            //{
            //    Response.Write("<script> alert('无效访问，请返回');history.back();");
            //    Response.End();
            //}

            //SqlDataAdapter adp = new SqlDataAdapter("select * from j_news where n_id=" + id + " ", conn);
            //DataSet dataset = new DataSet();
            //adp.Fill(dataset);

            //dt = dataset.Tables[0];
        }
    }
}