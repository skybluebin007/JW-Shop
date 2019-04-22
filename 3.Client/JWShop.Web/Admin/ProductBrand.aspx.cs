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
    public partial class ProductBrand : JWShop.Page.AdminBasePage
    {
        protected List<ProductBrandInfo> brandList = new List<ProductBrandInfo>();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadProductBrand", PowerCheckType.Single);
                
                ProductBrandSearchInfo brandSearch = new ProductBrandSearchInfo();
                brandSearch.Key = RequestHelper.GetQueryString<string>("Key");
                brandSearch.IsTop = RequestHelper.GetQueryString<int>("IsTop");
                Key.Text = brandSearch.Key;
                IsTop.Text = brandSearch.IsTop.ToString();
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                brandList = ProductBrandBLL.SearchList(CurrentPage, PageSize, brandSearch,ref Count);
                //BindControl(ProductBrandBLL.ReadList(), RecordList);
                BindControl(brandList, RecordList, MyPager);

                if (RequestHelper.GetQueryString<string>("Action") == "Delete") {
                 
                    int brandId = RequestHelper.GetQueryString<int>("ID");
                    if (brandId > 0)
                    {
                        CheckAdminPower("DeleteProductBrand", PowerCheckType.Single);
                        string URL = "ProductBrand.aspx?Action=search&";
                        URL += "Key=" + Key.Text + "&";
                        URL += "IsTop=" + IsTop.Text;
                        ProductBrandBLL.Delete(brandId);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("ProductBrand"), brandId.ToString());
                        ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), URL);
                    }                    
                }
            }
        }

        /// <summary>
        /// 删除按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteProductBrand", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                ProductBrandBLL.DeleteList(deleteID);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("ProductBrand"), deleteID);
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
            string URL = "ProductBrand.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&";         
            URL += "IsTop=" + IsTop.Text;
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
            string URL = "ProductBrand.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&";
            URL += "IsTop=" + IsTop.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}