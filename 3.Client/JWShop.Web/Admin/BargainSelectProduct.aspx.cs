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
    public partial class BargainSelectProduct : JWShop.Page.AdminBasePage
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

                //ClassID.DataSource = ProductClassBLL.ReadNamedList();
                //ClassID.DataTextField = "Name";
                //ClassID.DataValueField = "ID";
                //ClassID.DataBind();
                foreach (var item in ProductClassBLL.ReadNamedList())
                {
                    ClassID.Items.Add(new ListItem(item.Name,string.Format("|{0}|",item.Id)));
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

                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.IsSale = (int)BoolType.True;
                productSearch.IsDelete = 0;//未删除的
                productSearch.StandardType = (int)ProductStandardType.No;
                productSearch.Key = RequestHelper.GetQueryString<string>("Key");
                productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
                productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
                BindControl(ProductBLL.SearchList(CurrentPage, PageSize, productSearch, ref Count), RecordList, MyPager);
            }
        }
        /// <summary>
        /// 搜索按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "BargainSelectProduct.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "Tag=" + RequestHelper.GetQueryString<string>("Tag");
            ResponseHelper.Redirect(URL);
        }
    }
}