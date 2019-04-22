using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class Navigation : JWShop.Page.AdminBasePage
    {
        protected int _CurrentNavigationType;
        protected List<EnumInfo> _NavigationTypeList;
            
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string action = RequestHelper.GetQueryString<string>("Action");
            int id = RequestHelper.GetQueryString<int>("Id");
            if (action == "Delete")
            {
                NavigationBLL.Delete(id);
            }


            _CurrentNavigationType = RequestHelper.GetQueryString<int>("navigationType");
            if (_CurrentNavigationType == int.MinValue) _CurrentNavigationType = (int)NavigationType.Default;
            _NavigationTypeList = EnumHelper.ReadEnumList<NavigationType>();

            var entites = NavigationBLL.ReadList(_CurrentNavigationType);
            List<NavigationInfo> records = new List<NavigationInfo>();
            foreach (var entity in entites)
            {
                if (entity.ParentId < 1) records.Add(entity);
                foreach (var child in entites.Where(k => k.ParentId == entity.Id))
                {
                    child.Name = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + child.Name;
                    records.Add(child);
                }
            }

            BindControl(records, RecordList);
        }

    }
}