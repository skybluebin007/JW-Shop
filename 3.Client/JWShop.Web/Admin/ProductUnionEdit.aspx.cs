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
    public partial class ProductUnionEdit : JWShop.Page.AdminBasePage
    {
       
        protected string userGradeIDList = string.Empty;
        protected string userGradeNameList = string.Empty;
        protected List<UserGradeInfo> userGradeList = new List<UserGradeInfo>();
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

                string action = RequestHelper.GetQueryString<string>("Action");
                switch (action)
                {
                    case "UnionEdit":
                        UnionEdit();
                        break;
                    case "search":
                        ProductSearchInfo productSearch = new ProductSearchInfo();
                        productSearch.Name = RequestHelper.GetQueryString<string>("Name");
                        productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID"); 
                        productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
                        productSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                        productSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                        productSearch.IsSale = (int)BoolType.True;
                        ClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                        BrandID.Text = RequestHelper.GetQueryString<string>("BrandID");
                        Name.Text = RequestHelper.GetQueryString<string>("Name");
                        StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                        EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");
                        BindControl(ProductBLL.SearchList(productSearch), RecordList);    
                        break;
                    default:
                        break;
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
            string URL = "ProductUnionEdit.aspx?Action=search&";
            URL += "Name=" + Name.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }
        /// <summary>
        /// 统一编辑
        /// </summary>
        protected void UnionEdit()
        {
            string productIDList = RequestHelper.GetQueryString<string>("ProductIDList");
            ProductInfo product = new ProductInfo();
            if (RequestHelper.GetQueryString<string>("Weight") != string.Empty)
            {
                product.Weight = RequestHelper.GetQueryString<int>("Weight");
            }
            else
            {
                product.Weight = -2;
            }
            if (RequestHelper.GetQueryString<string>("MarketPrice") != string.Empty)
            {
                product.MarketPrice = RequestHelper.GetQueryString<decimal>("MarketPrice");
            }
            else
            {
                product.MarketPrice = -2;
            }
            if (RequestHelper.GetQueryString<string>("SendPoint") != string.Empty)
            {
                product.SendPoint = RequestHelper.GetQueryString<int>("SendPoint");
            }
            else
            {
                product.SendPoint = -2;
            }
            if (RequestHelper.GetQueryString<string>("TotalStorageCount") != string.Empty)
            {
                product.TotalStorageCount = RequestHelper.GetQueryString<int>("TotalStorageCount");
            }
            else
            {
                product.TotalStorageCount = -2;
            }
            if (RequestHelper.GetQueryString<string>("LowerCount") != string.Empty)
            {
                product.LowerCount = RequestHelper.GetQueryString<int>("LowerCount");
            }
            else
            {
                product.LowerCount = -2;
            }
            if (RequestHelper.GetQueryString<string>("UpperCount") != string.Empty)
            {
                product.UpperCount = RequestHelper.GetQueryString<int>("UpperCount");
            }
            else
            {
                product.UpperCount = -2;
            }
            ProductBLL.UnionUpdateProduct(productIDList, product);
            
            ResponseHelper.Write(ShopLanguage.ReadLanguage("UpdateOK"));
            ResponseHelper.End();
        }
    }
}