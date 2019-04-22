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
    public partial class ProductSingleEdit : JWShop.Page.AdminBasePage
    {
       
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected List<UserGradeInfo> userGradeList = new List<UserGradeInfo>();
        protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        protected string userGradeIDList = string.Empty;
        protected string userGradeNameList = string.Empty;
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower( "ProductBatchEdit", PowerCheckType.Single);

                string action = RequestHelper.GetQueryString<string>("Action");
                if (action == "SingleEdit")
                {
                    SingleEdit();
                }

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

                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.Name = RequestHelper.GetQueryString<string>("Name");
                productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID"); 
                productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");;
                productSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                productSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                productSearch.IsSale = (int)BoolType.True;
                ClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                BrandID.Text = RequestHelper.GetQueryString<string>("BrandID");
                Name.Text = RequestHelper.GetQueryString<string>("Name");
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");
                productList = ProductBLL.SearchList(CurrentPage, PageSize, productSearch, ref Count);
                BindControl(MyPager);

                string strProductID = string.Empty;
                foreach (ProductInfo product in productList)
                {
                    if (strProductID == string.Empty)
                    {
                        strProductID = product.Id.ToString();
                    }
                    else
                    {
                        strProductID += "," + product.Id.ToString();
                    }
                }
                userGradeList = UserGradeBLL.ReadList();
                foreach (UserGradeInfo userGrade in userGradeList)
                {
                    if (userGradeIDList == string.Empty)
                    {
                        userGradeIDList = userGrade.Id.ToString();
                        userGradeNameList = userGrade.Name;                        
                    }
                    else
                    {
                        userGradeIDList += "," + userGrade.Id.ToString();
                        userGradeNameList += "," + userGrade.Name;                        
                    }
                }
            }
        }
        /// <summary>
        /// 搜索按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "ProductSingleEdit.aspx?Action=search&";
            URL += "Name=" + Name.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }          
        /// <summary>
        /// 逐个编辑
        /// </summary>
        protected void SingleEdit()
        {
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            ProductInfo product = ProductBLL.Read(productID);
            product.ProductNumber = RequestHelper.GetQueryString<string>("ProductNumber");
            product.Weight = RequestHelper.GetQueryString<int>("Weight");
            product.MarketPrice = RequestHelper.GetQueryString<decimal>("MarketPrice");
            product.SalePrice = RequestHelper.GetQueryString<decimal>("SalePrice");
            product.SendPoint = RequestHelper.GetQueryString<int>("SendPoint");
            product.TotalStorageCount = RequestHelper.GetQueryString<int>("TotalStorageCount");
            product.LowerCount = RequestHelper.GetQueryString<int>("LowerCount");
            product.UpperCount = RequestHelper.GetQueryString<int>("UpperCount");
            ProductBLL.Update(product);            
            
            ResponseHelper.Write(ShopLanguage.ReadLanguage("UpdateOK"));
            ResponseHelper.End();
        }
    }
}