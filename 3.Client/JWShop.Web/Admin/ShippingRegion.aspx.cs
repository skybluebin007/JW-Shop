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
    public partial class ShippingRegion : JWShop.Page.AdminBasePage
    {
        protected int ShippingId = 0;
        protected ShippingInfo shipping = new ShippingInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadShippingRegion", PowerCheckType.Single);
                RegionID.DataSource = RegionBLL.ReadRegionUnlimitClass();
                ShippingId = RequestHelper.GetQueryString<int>("ShippingId");
                shipping = ShippingBLL.Read(ShippingId);
                int shippingRegionId = RequestHelper.GetQueryString<int>("Id");
                if (shippingRegionId != int.MinValue)
                {
                    ShippingRegionInfo shippingRegion = new ShippingRegionInfo();
                    shippingRegion = ShippingRegionBLL.Read(shippingRegionId);
                    Name.Text = shippingRegion.Name;
                    RegionID.ClassIDList = shippingRegion.RegionId;
                    FixedMoeny.Text = shippingRegion.FixedMoeny.ToString();
                    FirstMoney.Text = shippingRegion.FirstMoney.ToString();
                    AgainMoney.Text = shippingRegion.AgainMoney.ToString();
                    OneMoeny.Text = shippingRegion.OneMoeny.ToString();
                    AnotherMoeny.Text = shippingRegion.AnotherMoeny.ToString();
                }
                BindControl(ShippingRegionBLL.ReadList(ShippingId), RecordList);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteShippingRegion", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                ShippingRegionBLL.Delete(Array.ConvertAll<string, int>(deleteID.Split(','), k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("ShippingRegion"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ShippingRegionInfo shippingRegion = new ShippingRegionInfo();
            shippingRegion.Id = RequestHelper.GetQueryString<int>("Id");
            shippingRegion.Name = Name.Text;
            shippingRegion.ShippingId = RequestHelper.GetQueryString<int>("ShippingId");
            shippingRegion.RegionId = RegionID.ClassIDList;
            try
            {
                shippingRegion.FixedMoeny = Convert.ToDecimal(FixedMoeny.Text);
                shippingRegion.FirstMoney = Convert.ToDecimal(FirstMoney.Text);
                shippingRegion.AgainMoney = Convert.ToDecimal(AgainMoney.Text);
                shippingRegion.OneMoeny = Convert.ToDecimal(OneMoeny.Text);
                shippingRegion.AnotherMoeny = Convert.ToDecimal(AnotherMoeny.Text);
            }
            catch { }

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (shippingRegion.Id == int.MinValue)
            {
                CheckAdminPower("AddShippingRegion", PowerCheckType.Single);
                int id = ShippingRegionBLL.Add(shippingRegion);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("ShippingRegion"), id);
            }
            else
            {
                CheckAdminPower("UpdateShippingRegion", PowerCheckType.Single);
                ShippingRegionBLL.Update(shippingRegion);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("ShippingRegion"), shippingRegion.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}