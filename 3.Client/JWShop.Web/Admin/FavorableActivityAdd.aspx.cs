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
    public partial class FavorableActivityAdd : JWShop.Page.AdminBasePage
    {
        protected FavorableActivityInfo favorableActivity = new FavorableActivityInfo();
        protected List<FavorableActivityGiftInfo> giftList = new List<FavorableActivityGiftInfo>();
        //优惠活动ID
        protected int favorableActivityID = int.MinValue;
        //优惠活动类型
        protected int favorableType = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                UserGrade.DataSource = UserGradeBLL.ReadList();
                UserGrade.DataTextField = "Name";
                UserGrade.DataValueField = "Id";
                UserGrade.DataBind();              
               

                RegionID.DataSource = RegionBLL.ReadRegionUnlimitClass();

                ProductClass.DataSource = ProductClassBLL.ReadUnlimitClassList();
                favorableActivityID = RequestHelper.GetQueryString<int>("ID");
                if (favorableActivityID != int.MinValue)
                {
                    CheckAdminPower("ReadFavorableActivity", PowerCheckType.Single);
                    favorableActivity = FavorableActivityBLL.Read(favorableActivityID);
                    Photo.Text = favorableActivity.Photo;
                    Name.Text = favorableActivity.Name;
                    Content.Text = favorableActivity.Content;
                    StartDate.Text = favorableActivity.StartDate.ToString("yyyy-MM-dd");
                    EndDate.Text = favorableActivity.EndDate.ToString("yyyy-MM-dd");
                    ControlHelper.SetCheckBoxListValue(UserGrade, favorableActivity.UserGrade);
                    OrderProductMoney.Text = favorableActivity.OrderProductMoney.ToString();
                    RegionID.ClassIDList = favorableActivity.RegionId??"";
                    ReduceMoney.Text = favorableActivity.ReduceMoney.ToString();
                    ReduceDiscount.Text = favorableActivity.ReduceDiscount.ToString();
                    favorableType = favorableActivity.Type;
                    ProductClass.ClassIDList = favorableActivity.ClassIds??"";
                    if (!string.IsNullOrEmpty(favorableActivity.GiftId))
                    {
                        var giftSearch = new FavorableActivityGiftSearchInfo();
                        giftSearch.InGiftIds = Array.ConvertAll<string, int>(favorableActivity.GiftId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                        giftList = FavorableActivityGiftBLL.SearchList(giftSearch);
                    }
                }
                else
                {//新增:默认所有会员等级全部选中
                    foreach (ListItem item in UserGrade.Items)
                    {
                        item.Selected = true;
                    }
                }

            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FavorableActivityInfo favorableActivity = new FavorableActivityInfo();
            favorableActivity.Id = RequestHelper.GetQueryString<int>("ID");
            favorableActivity.Name = Name.Text;
            favorableActivity.Photo = Photo.Text;
            favorableActivity.Content = Content.Text;
            favorableActivity.StartDate = Convert.ToDateTime(StartDate.Text);
            favorableActivity.EndDate = Convert.ToDateTime(EndDate.Text).AddDays(1).AddSeconds(-1);
            favorableActivity.UserGrade = ControlHelper.GetCheckBoxListValue(UserGrade);
            favorableActivity.OrderProductMoney = Convert.ToDecimal(OrderProductMoney.Text);
            favorableActivity.Type = RequestHelper.GetForm<int>("FavorableType") < 0 ? 0 : RequestHelper.GetForm<int>("FavorableType");
            int shippingWay = RequestHelper.GetForm<int>("ShippingWay");
            string regionID = string.Empty;
            if (UserGradeBLL.ReadList().Count>0 && string.IsNullOrEmpty(favorableActivity.UserGrade))
            {
                ScriptHelper.Alert("至少选择1个会员等级", RequestHelper.RawUrl);
            }
            if (favorableActivity.EndDate < favorableActivity.StartDate)
            {
                ScriptHelper.Alert("结束日期不得小于开始日期");
            }
       //如果是订单优惠类型并选择了运费优惠
            if (favorableActivity.Type==(int)FavorableType.AllOrders && shippingWay == 1)
            {
                //regionID = RegionID.ClassIDList;
               favorableActivity.RegionId =RegionID.ClassIDList;
            }
            //如果是商品分类优惠
            if (favorableActivity.Type == (int)FavorableType.ProductClass)
            {
                favorableActivity.ClassIds = ProductClass.ClassIDList;
            }
            favorableActivity.ShippingWay = shippingWay;
            int reduceWay = RequestHelper.GetForm<int>("ReduceWay");
            decimal reduceMoney = 0;
            decimal reduceDiscount = 0;
            if (reduceWay == 1)
            {
                reduceMoney = Convert.ToDecimal(ReduceMoney.Text);
            }
            else if (reduceWay == 2)
            {
                reduceDiscount = Convert.ToDecimal(ReduceDiscount.Text);
            }
            favorableActivity.ReduceWay = reduceWay;
            favorableActivity.ReduceMoney = reduceMoney;
            favorableActivity.ReduceDiscount = reduceDiscount;
            favorableActivity.GiftId = RequestHelper.GetIntsForm("GiftList");
            string alertMessage = string.Empty;
            //限制同一时间段只能有一种优惠方式
            //if (FavorableActivityBLL.Read(favorableActivity.StartDate, favorableActivity.EndDate, favorableActivity.Id).Id > 0)
            //{
            //    alertMessage = ShopLanguage.ReadLanguage("OneTimeManyFavorableActivity");
            //}
            //else
            //{
                alertMessage = ShopLanguage.ReadLanguage("AddOK");
                if (favorableActivity.Id == int.MinValue)
                {
                    CheckAdminPower("AddFavorableActivity", PowerCheckType.Single);
                    int id = FavorableActivityBLL.Add(favorableActivity);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("FavorableActivity"), id);
                }
                else
                {
                    CheckAdminPower("UpdateFavorableActivity", PowerCheckType.Single);
                    FavorableActivityBLL.Update(favorableActivity);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("FavorableActivity"), favorableActivity.Id);
                    alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
                }
            //}
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}