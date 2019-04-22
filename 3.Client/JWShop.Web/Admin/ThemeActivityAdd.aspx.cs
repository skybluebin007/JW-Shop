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
    public partial class ThemeActivityAdd : JWShop.Page.AdminBasePage
    {
        protected string[] photoArray = new string[0];
        protected string[] linkArray = new string[0];
        protected string[] photoMobileArray = new string[0];
        protected string[] linkMobileArray = new string[0];
        protected string[] idArray = new string[0];
        protected string strThemeActivityId = string.Empty;
        protected List<ProductInfo> productList = new List<ProductInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int themeActivityId = RequestHelper.GetQueryString<int>("id");
                if (themeActivityId != int.MinValue)
                {
                    CheckAdminPower("ReadThemeActivity", PowerCheckType.Single);
                    strThemeActivityId = themeActivityId.ToString();
                    ThemeActivityInfo themeActivity = ThemeActivityBLL.Read(themeActivityId);
                    Name.Text = themeActivity.Name;
                    Photo.Text = themeActivity.Photo;
                    Description.Text = themeActivity.Description;
                    Css.Text = themeActivity.Css;
                    CssMobile.Text = themeActivity.CssMobile;
                    if (themeActivity.ProductGroup != string.Empty)
                    {
                        string idList = string.Empty;
                        int count = themeActivity.ProductGroup.Split('#').Length;
                        photoArray = new string[count];
                        linkArray = new string[count];
                        photoMobileArray = new string[count];
                        linkMobileArray = new string[count];
                        idArray = new string[count];
                        for (int i = 0; i < count; i++)
                        {
                            string[] productGroupArray = themeActivity.ProductGroup.Split('#')[i].Split('|');
                            photoArray[i] = productGroupArray[0];
                            linkArray[i] = productGroupArray[1];
                            photoMobileArray[i] = productGroupArray[2];
                            linkMobileArray[i] = productGroupArray[3];
                            idArray[i] = productGroupArray[4];
                            if (productGroupArray[4] != string.Empty)
                            {
                                idList += productGroupArray[4] + ",";
                            }
                        }
                        if (idList != string.Empty)
                        {
                            idList = idList.Substring(0, idList.Length - 1);
                        }
                        ProductSearchInfo productSearch = new ProductSearchInfo();
                        productSearch.InProductId = idList;
                        productList = ProductBLL.SearchList(1, idList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length, productSearch, ref count);
                    }

                    string[] themeStyleArr = themeActivity.Style.Split('|');
                    TopImage.Text = themeStyleArr[0];
                    BackgroundImage.Text = themeStyleArr[1];
                    BottomImage.Text = themeStyleArr[2];
                    ProductColor.Text = themeStyleArr[3];
                    ProductColor.Attributes.Add("style", "color:" + themeStyleArr[3] + ";");
                    ProductSize.Text = themeStyleArr[4];
                    PriceColor.Text = themeStyleArr[5];
                    PriceColor.Attributes.Add("style", "color:" + themeStyleArr[5] + ";");
                    PriceSize.Text = themeStyleArr[6];
                    OtherColor.Text = themeStyleArr[7];
                    OtherColor.Attributes.Add("style", "color:" + themeStyleArr[7] + ";");
                    OtherSize.Text = themeStyleArr[8];
                    TopImageMobile.Text = themeStyleArr[9];
                    BackgroundImageMobile.Text = themeStyleArr[10];
                    BottomImageMobile.Text = themeStyleArr[11];
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ThemeActivityInfo themeActivity = new ThemeActivityInfo();
            themeActivity.Id = RequestHelper.GetQueryString<int>("id");
            themeActivity.Name = Name.Text;
            themeActivity.Photo = Photo.Text;
            themeActivity.Description = Description.Text;
            themeActivity.Css = Css.Text;
            themeActivity.CssMobile = CssMobile.Text;
            int count = RequestHelper.GetForm<int>("ProductGroupCount");
            string productGroup = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (RequestHelper.GetForm<string>("ProductGroupValue" + i) != string.Empty)
                {
                    productGroup += RequestHelper.GetForm<string>("ProductGroupValue" + i) + "#";
                }
            }
            if (productGroup.EndsWith("#"))
            {
                productGroup = productGroup.Substring(0, productGroup.Length - 1);
            }
            themeActivity.ProductGroup = productGroup;
            themeActivity.Style = TopImage.Text + '|' + BackgroundImage.Text + '|' + BottomImage.Text + '|' + ProductColor.Text + '|' + ProductSize.Text + '|' + PriceColor.Text + '|' + PriceSize.Text + '|' + OtherColor.Text + '|' + OtherSize.Text + '|' + TopImageMobile.Text + '|' + BackgroundImageMobile.Text + '|' + BottomImageMobile.Text;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (themeActivity.Id == int.MinValue)
            {
                CheckAdminPower("AddThemeActivity", PowerCheckType.Single);
                int id = ThemeActivityBLL.Add(themeActivity);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("ThemeActivity"), id);
            }
            else
            {
                CheckAdminPower("UpdateThemeActivity", PowerCheckType.Single);
                ThemeActivityBLL.Update(themeActivity);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("ThemeActivity"), themeActivity.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }

        protected ProductInfo ReadProduct(List<ProductInfo> productList, int id)
        {
            ProductInfo product = new ProductInfo();
            foreach (ProductInfo temp in productList)
            {
                if (temp.Id == id)
                {
                    product = temp;
                }
            }
            return product;
        }
    }
}