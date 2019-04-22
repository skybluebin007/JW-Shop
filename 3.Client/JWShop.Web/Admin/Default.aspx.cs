using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = RequestHelper.GetQueryString<string>("action");
            if (action == "GetMenuList") this.GetMenuList();
        }


        private void GetMenuList()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = false, name = "", list = "" });           
           
            if (Cookies.Admin.GetAdminID(true) != 0)
            {
                int id = RequestHelper.GetQueryString<int>("id");
                if (id < 1) id = 1;
                var menuList = MenuBLL.ReadMenuCacheList();
                var rootMenuList = menuList.Where(k => k.FatherID == id).ToList();

                json = Newtonsoft.Json.JsonConvert.SerializeObject(from item in rootMenuList select new { name = item.MenuName, list = from child in menuList.Where(k => k.FatherID == item.ID) select new { id = child.ID, url = child.URL, name = child.MenuName } });
            }
            ResponseHelper.Write(json);
            ResponseHelper.End();
        }

    }
}