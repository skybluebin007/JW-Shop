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
    public partial class ProductOffSale : JWShop.Page.AdminBasePage
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
                CheckAdminPower("ReadProduct", PowerCheckType.Single);

                foreach (ProductClassInfo productClass in ProductClassBLL.ReadNamedList())
                {
                    ClassID.Items.Add(new ListItem(productClass.Name, "|" + productClass.Id + "|"));
                }
                ClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

                BrandID.DataSource = ProductBrandBLL.ReadList();
                BrandID.DataTextField = "Name";
                BrandID.DataValueField = "ID";
                BrandID.DataBind();
                BrandID.Items.Insert(0, new ListItem("所有品牌", string.Empty));

                ClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                BrandID.Text = RequestHelper.GetQueryString<string>("BrandID");
                Key.Text = RequestHelper.GetQueryString<string>("Key");
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");


                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.Key = RequestHelper.GetQueryString<string>("Key");
                productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
                productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
                productSearch.IsSale = (int)BoolType.False;
                productSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                productSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                productSearch.IsDelete = 0;//没有逻辑删除的商品
                //PageSize = 10;
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                List<ProductInfo> productList = ProductBLL.SearchList(CurrentPage, PageSize, productSearch, ref Count);
                BindControl(productList, RecordList, MyPager);

             
                switch (RequestHelper.GetQueryString<string>("ActionProduct"))
                {

                    case "OnSaleProduct":
                        int Id = RequestHelper.GetQueryString<int>("ID");
                        if (Id > 0)
                        {
                            CheckAdminPower("OnSaleProduct", PowerCheckType.Single);
                            var product = ProductBLL.Read(Id);
                            if (product.IsSale == (int)BoolType.False)
                            {
                                product.IsSale = (int)BoolType.True;
                                ProductBLL.Update(product);
                                AdminLogBLL.Add(ShopLanguage.ReadLanguage("OnSaleRecord"), ShopLanguage.ReadLanguage("Product"), Id.ToString());
                                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OnSaleOK"), Request.UrlReferrer.ToString());
                            }
                        }
                        break;
                    case "DeleteProduct":
                        Id = RequestHelper.GetQueryString<int>("ID");
                        if (Id > 0)
                        {
                            CheckAdminPower("DeleteProduct", PowerCheckType.Single);

                            ProductBLL.DeleteLogically(Id);
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Product"), Id.ToString());
                            ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), Request.UrlReferrer.ToString());

                        }
                        break;
                    default:
                        break;

                }
            }
        }

        /// <summary>
        /// 上架按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSaleButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("OnSaleProduct", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                ProductBLL.OnSale(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("OnSaleRecord"), ShopLanguage.ReadLanguage("Product"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OnSaleOK"), RequestHelper.RawUrl);
            }
        }

        /// <summary>
        /// 删除按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteProduct", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                ProductBLL.DeleteLogically(deleteID);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Product"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 搜索按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "ProductOffSale.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
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
            string URL = "ProductOffSale.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}