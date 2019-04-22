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
    public partial class PointProductAdd : JWShop.Page.AdminBasePage
    {
        protected string color = string.Empty;
        protected PointProductInfo pointProduct = new PointProductInfo();
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("Id");
                if (id != int.MinValue)
                {
                    CheckAdminPower("ReadPointProduct", PowerCheckType.Single);
                    pointProduct = PointProductBLL.Read(id);

                    Name.Text = pointProduct.Name;
                    SubTitle.Text = pointProduct.SubTitle;

                    Point.Text = pointProduct.Point.ToString();
                    TotalCount.Text = pointProduct.TotalStorageCount.ToString();
                    BeginDate.Text = pointProduct.BeginDate.ToString("yyyy-MM-dd");
                    EndDate.Text = pointProduct.EndDate.ToString("yyyy-MM-dd");

                    MarketPrice.Text = pointProduct.MarketPrice.ToString();
                    Photo.Text = pointProduct.Photo;
                    Introduction.Value = pointProduct.Introduction1;
                    Introduction_Mobile.Value = pointProduct.Introduction1_Mobile;

                    productPhotoList = ProductPhotoBLL.ReadList(pointProduct.Id,0);
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int productId = RequestHelper.GetQueryString<int>("Id");

            PointProductInfo pointProductSubmit = PointProductBLL.Read(productId);
            pointProductSubmit.Id = productId;
            pointProductSubmit.Name = Name.Text;
            pointProductSubmit.SubTitle = SubTitle.Text;
            pointProductSubmit.Point = Convert.ToInt32(Point.Text);
            pointProductSubmit.TotalStorageCount = Convert.ToInt32(TotalCount.Text);
            pointProductSubmit.BeginDate = Convert.ToDateTime(BeginDate.Text);
            pointProductSubmit.EndDate = Convert.ToDateTime(EndDate.Text);

            pointProductSubmit.MarketPrice = Convert.ToDecimal(MarketPrice.Text);
            pointProductSubmit.Photo = Photo.Text;
            pointProductSubmit.Introduction1 = Introduction.Value;
            pointProductSubmit.Introduction1_Mobile = Introduction_Mobile.Value;
            pointProductSubmit.IsSale = RequestHelper.GetForm<int>("IsSale");
            pointProductSubmit.AddDate = RequestHelper.DateNow;

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (pointProductSubmit.Id <= 0)
            {
                CheckAdminPower("AddPointProduct", PowerCheckType.Single);

                int id = PointProductBLL.Add(pointProductSubmit);
                if (id > 0) AddProductPhoto(id);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("PointProduct"), id);
            }
            else
            {
                CheckAdminPower("UpdatePointProduct", PowerCheckType.Single);

                ProductPhotoBLL.DeleteList(productId,0);
                AddProductPhoto(productId);
                PointProductBLL.Update(pointProductSubmit);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("PointProduct"), pointProductSubmit.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }

        protected void AddProductPhoto(int productId)
        {
            string productPhotoList = RequestHelper.GetForm<string>("ProductPhoto");
            if (!string.IsNullOrEmpty(productPhotoList))
            {
                foreach (string temp in productPhotoList.Split(','))
                {
                    ProductPhotoInfo productPhoto = new ProductPhotoInfo();
                    productPhoto.ProductId = productId;
                    productPhoto.Name = temp.Split('|')[0];
                    productPhoto.ImageUrl = temp.Split('|')[1].Replace("75-75", "Original");
                    ProductPhotoBLL.Add(productPhoto);
                }
            }
        }
    }
}