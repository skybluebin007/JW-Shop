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
    public partial class ProductRecycle : JWShop.Page.AdminBasePage
    {
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
                productSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                productSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                productSearch.IsDelete = 1;//逻辑删除的商品
                //PageSize = 10;
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                List<ProductInfo> productList = ProductBLL.SearchList(CurrentPage, PageSize, productSearch, ref Count);
                BindControl(productList, RecordList, MyPager);

                var pid = RequestHelper.GetQueryString<int>("ID");
            
                switch (RequestHelper.GetQueryString<string>("Action")) { 
                    case "Recover":                       
                        if (pid > 0) {
                            ProductBLL.Recover(pid);
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("RecoverRecord"), ShopLanguage.ReadLanguage("Product"), pid);
                            ScriptHelper.Alert(ShopLanguage.ReadLanguage("RecoverOK"), Request.UrlReferrer.ToString());
                        }
                        break;
                    case "Delete":
                        if (pid > 0)
                        {
                            ProductBLL.Delete(pid);
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecordCompletely"), ShopLanguage.ReadLanguage("Product"), pid);
                            ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteCompletelyOK"), Request.UrlReferrer.ToString());
                        }
                        break;
                     default:
                        break;
                }
              
            }
        }
        /// <summary>
        /// 恢复按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSaleButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("OnSaleProduct", PowerCheckType.Single);
            string ids = RequestHelper.GetIntsForm("SelectID");
            //string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                ProductBLL.Recover(ids);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("RecoverRecord"), ShopLanguage.ReadLanguage("Product"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("RecoverOK"), RequestHelper.RawUrl);
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
                ProductBLL.Delete(deleteID);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecordCompletely"), ShopLanguage.ReadLanguage("Product"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteCompletelyOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 清空回收站按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteProduct", PowerCheckType.Single);
            List<ProductInfo> productRecycleList=ProductBLL.SearchList(new ProductSearchInfo{IsDelete=1});
            string deleteID = string.Empty;
            foreach (ProductInfo pro in productRecycleList) {
                if (deleteID == string.Empty) deleteID = pro.Id.ToString();
                else deleteID += "," + pro.Id.ToString();
            }
       
            if (deleteID != string.Empty)
            {
                ProductBLL.Delete(deleteID);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecordCompletely"), ShopLanguage.ReadLanguage("Product"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteCompletelyOK"), RequestHelper.RawUrl);
            }
        }
       
        /// <summary>
        /// 搜索按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "ProductRecycle.aspx?Action=search&";
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
            string URL = "ProductRecycle.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}