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

namespace JWShop.Web.Admin
{
    public partial class Shipping : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadShipping", PowerCheckType.Single);
                string action = RequestHelper.GetQueryString<string>("Action");
                int shippingId = RequestHelper.GetQueryString<int>("Id");
                if (shippingId != 0 && action != string.Empty)
                {
                    CheckAdminPower("UpdateShipping", PowerCheckType.Single);
                    ChangeAction changeAction = ChangeAction.Up;
                    if (action == "Down")
                    {
                        changeAction = ChangeAction.Down;
                    }
                    ShippingBLL.Move(changeAction, shippingId);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("Shipping"), shippingId);
                }
                BindControl(ShippingBLL.ReadList(), RecordList);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteShipping", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                ShippingBLL.Delete(Array.ConvertAll<string, int>(deleteID.Split(','), k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Shipping"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }
    }
}