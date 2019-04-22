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
    public partial class ProductClassAdd : JWShop.Page.AdminBasePage
    {

        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FatherID.DataSource = ProductClassBLL.ReadNamedList();
                FatherID.DataTextField = "Name";
                FatherID.DataValueField = "ID";
                FatherID.DataBind();
                FatherID.Items.Insert(0, new ListItem("作为最大类", "0"));
                //foreach (ProductClassInfo productClass in ProductClassBLL.ReadNamedList())
                //{
                //    FatherID.Items.Add(new ListItem(productClass.Name, "|" + productClass.Id + "|"));
                //}
                //FatherID.Items.Insert(0, new ListItem("作为最大类", "0"));

                ProductType.DataSource = ProductTypeBLL.ReadList();
                ProductType.DataTextField = "Name";
                ProductType.DataValueField = "ID";
                ProductType.DataBind();
                ProductType.Items.Insert(0, new ListItem("请选择类型", "0"));

                int productClassID = RequestHelper.GetQueryString<int>("ID");
                int fatherID = RequestHelper.GetQueryString<int>("FatherID");
                if (productClassID != int.MinValue)
                {
                    CheckAdminPower("ReadProductClass", PowerCheckType.Single);
                    ProductClassInfo productClass = ProductClassBLL.Read(productClassID);
                    FatherID.Text = productClass.ParentId.ToString();
                    OrderID.Text = productClass.OrderId.ToString();
                    ClassName.Text = productClass.Name;
                    Keywords.Text = productClass.Keywords;
                    Description.Value = productClass.Remark;
                    Photo.Text = productClass.Photo;
                    ProductType.Text = productClass.ProductTypeId.ToString();

                    EnClassName.Text = productClass.EnClassName;
                    PageTitle.Text = productClass.PageTitle;
                    PageKeyWord.Text = productClass.PageKeyWord;
                    PageSummary.Text = productClass.PageSummary;
                }
                else
                {
                    FatherID.Text = fatherID.ToString();
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
            ProductClassInfo productClass = new ProductClassInfo();
            productClass.Id = RequestHelper.GetQueryString<int>("ID");
            if (FatherID.Text.Trim() == RequestHelper.GetQueryString<string>("ID"))
            {
                ScriptHelper.Alert("不能将上级分类设置成自己", RequestHelper.RawUrl);
                Response.End();
            }
            if (string.IsNullOrEmpty(ProductType.Text))
            {
                ScriptHelper.Alert("必须选择产品类型", RequestHelper.RawUrl);
                Response.End();
            }
            productClass.ParentId = Convert.ToInt32(FatherID.Text);
            productClass.OrderId = Convert.ToInt32(OrderID.Text);
            productClass.Name = ClassName.Text;
            productClass.Keywords = Keywords.Text;
            productClass.Remark = Description.Value;
            productClass.Photo = Photo.Text;
            productClass.ProductTypeId = Convert.ToInt32(ProductType.Text);

            productClass.Tm = DateTime.Now;

            productClass.EnClassName = EnClassName.Text.Trim();
            productClass.PageTitle = PageTitle.Text.Trim();
            productClass.PageKeyWord = PageKeyWord.Text.Trim();
            productClass.PageSummary = PageSummary.Text.Trim();

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (productClass.Id == int.MinValue)
            {                
                CheckAdminPower("AddProductClass", PowerCheckType.Single);
                int id = ProductClassBLL.Add(productClass);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("ProductClass"), id);
            }
            else
            {
                CheckAdminPower("UpdateProductClass", PowerCheckType.Single);
                ProductClassBLL.Update(productClass);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("ProductClass"), productClass.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}