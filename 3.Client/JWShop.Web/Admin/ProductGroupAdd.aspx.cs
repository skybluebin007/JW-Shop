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
    public partial class ProductGroupAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                foreach (ProductClassInfo productClass in ProductClassBLL.ReadNamedList())
                {
                    RelationClassID.Items.Add(new ListItem(productClass.Name, "|" + productClass.Id + "|"));
                }
                RelationClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

                List<ProductBrandInfo> productBrandList = ProductBrandBLL.ReadList();
                RelationBrandID.DataSource = productBrandList;
                RelationBrandID.DataTextField = "Name";
                RelationBrandID.DataValueField = "ID";
                RelationBrandID.DataBind();
                RelationBrandID.Items.Insert(0, new ListItem("选择品牌", string.Empty));
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string photo = Photo.Text;
            string link = Link.Text.Replace("#", "");
            string photoMobile = PhotoMobile.Text;
            string linkMobile = LinkMobile.Text.Replace("#", "");
            string idList = RequestHelper.GetForm<string>("RelationProductID");
            string nameList = string.Empty;
            string photoList = string.Empty;
            List<ProductInfo> productList = new List<ProductInfo>();
            if (idList != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductId = idList;
                productSearch.IsSale = (int)BoolType.True;
                int count = 0;
                productList = ProductBLL.SearchList(1, idList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length, productSearch, ref count);
                foreach (ProductInfo product in productList)
                {
                    nameList += product.Name.Replace(",", "") + ",";
                    photoList += product.Photo.Replace(",", "") + ",";
                }
                if (nameList.EndsWith(","))
                {
                    nameList = nameList.Substring(0, nameList.Length - 1);
                    photoList = photoList.Substring(0, photoList.Length - 1);
                }
            }
            string action = RequestHelper.GetQueryString<string>("Action");
            int id = RequestHelper.GetQueryString<int>("Id");
            int themeActivityId = RequestHelper.GetQueryString<int>("ThemeActivityId");
            string js = "<script language=\"javascript\" type=\"text/javascript\">";
            if (action == "Update")
            {
                js += "parent.updateProductGroup('" + photo + "','" + link + "','" + photoMobile + "','" + linkMobile + "','" + idList + "','" + nameList + "','" + photoList + "'," + id + "," + themeActivityId + ");";
            }
            else
            {
                js += "parent.addProductGroup('" + photo + "','" + link + "','" + photoMobile + "','" + linkMobile + "','" + idList + "','" + nameList + "','" + photoList + "'," + themeActivityId + ");";
            }
            js += "var index = parent.layer.getFrameIndex(window.name);parent.layer.close(index);</script>";
            ResponseHelper.Write(js);
            ResponseHelper.End();
        }
    }
}