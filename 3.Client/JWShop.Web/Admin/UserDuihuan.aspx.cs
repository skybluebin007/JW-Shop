using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class UserDuihuan : JWShop.Page.AdminBasePage
    {
        protected int status = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //CheckAdminPower("ReadPifa", PowerCheckType.Single);
                string action = RequestHelper.GetQueryString<string>("Action");
                int id = RequestHelper.GetQueryString<int>("Id");
                if (id > 0)
                {
                    switch (action)
                    {
                        case "Delete":
                            //CheckAdminPower("DeleteUserPifa", PowerCheckType.Single);
                            OtherDuihuanBLL.Delete(id);
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), "兑换记录", id);
                            break;
                        default:
                            break;
                    }
                }

                OtherDuihuanSearchInfo Searchmodel = new OtherDuihuanSearchInfo();
                Searchmodel.mobile = RequestHelper.GetQueryString<string>("mobile");
                Searchmodel.truename = RequestHelper.GetQueryString<string>("truename");

                mobile.Text = Searchmodel.mobile;
                truename.Text = Searchmodel.truename;

                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                var Lists = OtherDuihuanBLL.SearchList(CurrentPage, PageSize, Searchmodel, ref Count);
                //Count = userList.Count;

                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                BindControl(Lists, RecordList, MyPager);
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "UserDuihuan.aspx?Action=search&";
            URL += "mobile=" + mobile.Text + "&";
            URL += "truename=" + truename.Text;
            ResponseHelper.Redirect(URL);
        }


        /// <summary>
        /// 每页显示条数控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["AdminPageSize"] = AdminPageSize.Text;
            string URL = "UserDuihuan.aspx?Action=search&";
            URL += "mobile=" + mobile.Text + "&";
            URL += "truename=" + truename.Text;
            ResponseHelper.Redirect(URL);
        }

        /// <summary>
        /// 获取会员基本信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        protected UserInfo getuserinfo(string uid)
        {
            UserInfo model = new UserInfo();
            model = UserBLL.Read(Int32.Parse(uid));

            return model;
        }
    }
}