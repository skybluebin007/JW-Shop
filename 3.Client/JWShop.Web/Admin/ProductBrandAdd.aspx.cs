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
    public partial class ProductBrandAdd : JWShop.Page.AdminBasePage
    {
        protected ProductBrandInfo productBrand = new ProductBrandInfo();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int productBrandID = RequestHelper.GetQueryString<int>("ID");

                if (productBrandID != int.MinValue)
                {
                    CheckAdminPower("ReadProductBrand", PowerCheckType.Single);
                    productBrand = ProductBrandBLL.Read(productBrandID);
                    Name.Text = productBrand.Name;
                    Logo.Text = productBrand.ImageUrl;
                    Url.Text = productBrand.LinkUrl;
                    Description.Text = productBrand.Remark;
                    OrderID.Text = productBrand.OrderId.ToString();

                    if (Convert.ToBoolean(productBrand.IsTop)) IsTop.Checked = true;
                    Spell.Visible = true;
                    Spelling.Text = productBrand.Spelling;
                }
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ProductBrandInfo productBrand = new ProductBrandInfo();
            productBrand.Id = RequestHelper.GetQueryString<int>("ID");
            productBrand.Name = Name.Text;
            var _brand = ProductBrandBLL.Read(productBrand.Name);
            if (( productBrand.Id>0 && _brand.Id > 0 && _brand.Id != productBrand.Id) || (productBrand.Id <= 0 && _brand.Id>0))
            {
                ScriptHelper.Alert("该品牌已存在，请重新输入", RequestHelper.RawUrl);
            }
            else
            {
               
                productBrand.ImageUrl = Logo.Text;
                productBrand.LinkUrl = Url.Text;
                productBrand.Remark = Description.Text;
                productBrand.IsTop = Convert.ToInt32(IsTop.Checked);
                productBrand.OrderId = Convert.ToInt32(OrderID.Text);

                string alertMessage = ShopLanguage.ReadLanguage("AddOK");
                if (productBrand.Id == int.MinValue)
                {
                    CheckAdminPower("AddProductBrand", PowerCheckType.Single);
                    productBrand.Spelling = ChineseCharacterHelper.GetChineseSpell(Name.Text.Trim());
                    int id = ProductBrandBLL.Add(productBrand);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("ProductBrand"), id);
                }
                else
                {
                    CheckAdminPower("UpdateProductBrand", PowerCheckType.Single);
                    productBrand.Spelling = Spelling.Text;
                    ProductBrandBLL.Update(productBrand);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("ProductBrand"), productBrand.Id);
                    alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
                }
                ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
            }
        }
    }
}