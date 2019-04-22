using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Collections;
using NetDimension.Json;

namespace JWShop.Web.Admin
{
    public partial class Product : JWShop.Page.AdminBasePage
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
                IsSpecial.Text = RequestHelper.GetQueryString<string>("IsSpecial");
                IsNew.Text = RequestHelper.GetQueryString<string>("IsNew");
                IsHot.Text = RequestHelper.GetQueryString<string>("IsHot");
                IsTop.Text = RequestHelper.GetQueryString<string>("IsTop");
                ProductNumber.Text= RequestHelper.GetQueryString<string>("ProductNumber");

                #region 销量 价格
                decimal _LowerSalePrice = RequestHelper.GetQueryString<decimal>("LowerSalePrice");
                decimal _UpperSalePrice = RequestHelper.GetQueryString<decimal>("UpperSalePrice");
                int _LowerOrderCount = RequestHelper.GetQueryString<int>("LowerOrderCount");
                int _UpperOrderCount = RequestHelper.GetQueryString<int>("UpperOrderCount");
                if (_LowerSalePrice >= 0 && _UpperSalePrice >= 0)
                {
                    this.LowerSalePrice.Text = _LowerSalePrice < _UpperSalePrice ? _LowerSalePrice.ToString() : _UpperSalePrice.ToString();
                    this.UpperSalePrice.Text = _LowerSalePrice < _UpperSalePrice ? _UpperSalePrice.ToString() : _LowerSalePrice.ToString();
                }
                if (_LowerOrderCount >= 0 && _UpperOrderCount >= 0)
                {
                    this.LowerOrderCount.Text = _LowerOrderCount < _UpperOrderCount ? _LowerOrderCount.ToString() : _UpperOrderCount.ToString();
                    this.UpperOrderCount.Text = _LowerOrderCount < _UpperOrderCount ? _UpperOrderCount.ToString() : _LowerOrderCount.ToString();
                }
                #endregion

                List<ProductInfo> productList = new List<ProductInfo>();
                ProductSearchInfo productSearch = new ProductSearchInfo();
                //productSearch.Key = RequestHelper.GetQueryString<string>("Key");
                productSearch.Name = RequestHelper.GetQueryString<string>("Key");
                productSearch.ProductNumber = RequestHelper.GetQueryString<string>("ProductNumber");
                productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
                productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
                productSearch.IsSpecial = RequestHelper.GetQueryString<int>("IsSpecial");
                productSearch.IsNew = RequestHelper.GetQueryString<int>("IsNew");
                productSearch.IsHot = RequestHelper.GetQueryString<int>("IsHot");
                productSearch.IsSale = (int)BoolType.True;
                productSearch.IsTop = RequestHelper.GetQueryString<int>("IsTop");
                productSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                productSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                productSearch.IsDelete = 0;//没有逻辑删除的商品
                if (_LowerSalePrice >= 0 && _UpperSalePrice >= 0)
                {
                    productSearch.LowerSalePrice = _LowerSalePrice < _UpperSalePrice ? _LowerSalePrice : _UpperSalePrice;
                    productSearch.UpperSalePrice = _LowerSalePrice < _UpperSalePrice ? _UpperSalePrice : _LowerSalePrice;
                }
                if (_LowerOrderCount >= 0 && _UpperOrderCount >= 0)
                {
                    productSearch.LowerOrderCount = _LowerOrderCount < _UpperOrderCount ? _LowerOrderCount : _UpperOrderCount;
                    productSearch.UpperOrderCount = _LowerOrderCount < _UpperOrderCount ? _UpperOrderCount : _LowerOrderCount;
                }
                string productOrderType = RequestHelper.GetQueryString<string>("ProductOrderType");
                if (!string.IsNullOrEmpty(productOrderType)) productSearch.ProductOrderType = productOrderType;
                string orderType = RequestHelper.GetQueryString<string>("OrderType");
                productSearch.OrderType =orderType == "Asc"? OrderType.Asc:OrderType.Desc;
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                productList = ProductBLL.SearchList(CurrentPage, PageSize, productSearch, ref Count);

                BindControl(productList, RecordList, MyPager);

             
                switch (RequestHelper.GetQueryString<string>("ActionProduct")) { 
                         
                    case"OffSaleProduct":
                        int Id = RequestHelper.GetQueryString<int>("ID");
                        if (Id > 0) {
                            CheckAdminPower("OffSaleProduct", PowerCheckType.Single);
                            var product = ProductBLL.Read(Id);
                            if (product.IsSale == (int)BoolType.True) {
                                product.IsSale = (int)BoolType.False;
                                ProductBLL.Update(product);
                                AdminLogBLL.Add(ShopLanguage.ReadLanguage("OffSaleRecord"), ShopLanguage.ReadLanguage("Product"), Id.ToString());
                                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OffSaleOK"), Request.UrlReferrer.ToString());                             
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
                    case"ModifyPrice":
                        ModifyPrice();
                        break;
                    case"ModifyStorage":
                        ModifyStorage();
                        break;
                    default:
                        break;
                
                }
            }
        }
        
        /// <summary>
        /// 下架按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OffSaleButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("OffSaleProduct", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                ProductBLL.OffSale(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("OffSaleRecord"), ShopLanguage.ReadLanguage("Product"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OffSaleOK"), RequestHelper.RawUrl);
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
            string URL = "Product.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&";
            URL += "ProductNumber=" + ProductNumber.Text + "&";
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text + "&";
            URL += "LowerSalePrice=" + LowerSalePrice.Text + "&";
            URL += "UpperSalePrice=" + UpperSalePrice.Text + "&";
            URL += "LowerOrderCount=" + LowerOrderCount.Text + "&";
            URL += "UpperOrderCount=" + UpperOrderCount.Text + "&";
            URL += "IsSpecial=" + IsSpecial.Text + "&";
            URL += "IsNew=" + IsNew.Text + "&";
            URL += "IsHot=" + IsHot.Text + "&";
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
            string URL = "Product.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text + "&";
            URL += "LowerSalePrice=" + LowerSalePrice.Text + "&";
            URL += "UpperSalePrice=" + UpperSalePrice.Text + "&";
            URL += "LowerOrderCount=" + LowerOrderCount.Text + "&";
            URL += "UpperOrderCount=" + UpperOrderCount.Text + "&";
            URL += "IsSpecial=" + IsSpecial.Text + "&";
            URL += "IsNew=" + IsNew.Text + "&";
            URL += "IsHot=" + IsHot.Text + "&";
            URL += "IsTop=" + IsTop.Text;
            ResponseHelper.Redirect(URL);
        }
        /// <summary>
        /// 修改商品价格
        /// </summary>
        protected void ModifyPrice() {
            string result = string.Empty;
            bool flag = true;
            int productId = RequestHelper.GetQueryString<int>("productId");
            decimal salePrice = RequestHelper.GetQueryString<decimal>("salePrice");
            string valueList = Server.UrlDecode(RequestHelper.GetQueryString<string>("valueList"));
            string standIdList = RequestHelper.GetQueryString<string>("standIdList");
            string priceList = RequestHelper.GetQueryString<string>("priceList");
            //无规格或产品组规格  修改一口价
            if (salePrice < 0)
            {
                flag = false;
                result = "一口价填写不规范";
            }
            if (flag)
            {
                if (productId > 0)
                {
                    var product = ProductBLL.Read(productId);

                    if (product.StandardType == (int)ProductStandardType.Single)
                    {//如果是单产品规格
                        string[] valueArr = valueList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] salePriceArr = priceList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        decimal[] standardPriceList = ProductTypeStandardRecordBLL.ReadListByProduct(productId, product.StandardType).Select(k=>k.SalePrice).ToArray();
                      
                        decimal[] arr = Array.ConvertAll<string, decimal>(salePriceArr, s => decimal.Parse(s));                       
                        ArrayList list = new ArrayList(arr);
                        list.AddRange(standardPriceList);
                        list.Sort();
                        decimal min = Convert.ToDecimal(list[0]);
                        decimal max = Convert.ToDecimal(list[list.Count - 1]);
                        if (salePrice > max || salePrice < min)
                        {
                            flag = false;
                            result = "一口价必须在" + min + "-" + max + "之间";
                        }
                        else
                        {
                            product.SalePrice = salePrice;
                            ProductBLL.Update(product);
                            result = product.SalePrice.ToString();
                            for (int i = 0; i < valueArr.Length; i++)
                            {
                                ProductTypeStandardRecordInfo standardRecord = new ProductTypeStandardRecordInfo();
                                standardRecord.ProductId = product.Id;
                                standardRecord.ValueList = valueArr[i];
                                standardRecord.SalePrice = Convert.ToDecimal(salePriceArr[i]);
                                ProductTypeStandardRecordBLL.UpdateSalePrice(standardRecord);
                            }
                        }
                    }
                    else
                    {
                        //无规格或产品组规格  修改一口价                      
                            product.SalePrice = salePrice;
                            ProductBLL.Update(product);
                            result = product.SalePrice.ToString();
                    }
                }
                else
                {
                    flag = false;
                    result = "参数错误";
                }
            }
            Response.Clear();
            ResponseHelper.Write(JsonConvert.SerializeObject(new { flag = flag, msg = result }));
            Response.End();
        }
        /// <summary>
        /// 修改商品库存
        /// </summary>
        protected void ModifyStorage() {
            string result = string.Empty;
            bool flag = true;
            int productId = RequestHelper.GetQueryString<int>("productId");
            int totalStorage = RequestHelper.GetQueryString<int>("totalStorage");
            string valueList = Server.UrlDecode(RequestHelper.GetQueryString<string>("valueList"));
            string standIdList = RequestHelper.GetQueryString<string>("standIdList");
            string storageList = RequestHelper.GetQueryString<string>("storageList");
            //无规格或产品组规格  修改一口价
            if (totalStorage < 0)
            {
                flag = false;
                result = "总库存填写不规范";
            }
            if (flag)
            {
                if (productId > 0)
                {
                    var product = ProductBLL.Read(productId);
                    if (product.StandardType == (int)ProductStandardType.Single)
                    {//如果是单产品规格
                        string[] valueArr = valueList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] storageArr = storageList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                       
                        for (int i = 0; i < valueArr.Length; i++)
                        {
                            ProductTypeStandardRecordInfo standardRecord = new ProductTypeStandardRecordInfo();
                            standardRecord.ProductId = product.Id;
                            standardRecord.ValueList = valueArr[i];
                            standardRecord.Storage = Convert.ToInt32(storageArr[i]);
                            ProductTypeStandardRecordBLL.UpdateStorage(standardRecord);                           
                        }
                        product.TotalStorageCount = ProductTypeStandardRecordBLL.GetSumStorageByProduct(productId);
                        ProductBLL.Update(product);
                        result = product.TotalStorageCount.ToString();
                    }
                    else
                    {
                        //无规格或产品组规格  修改总库存                      
                        product.TotalStorageCount = totalStorage;
                        ProductBLL.Update(product);
                        result = product.TotalStorageCount.ToString();
                    }
                }
                else
                {
                    flag = false;
                    result = "参数错误";
                }
            }
            Response.Clear();
            ResponseHelper.Write(JsonConvert.SerializeObject(new { flag = flag, msg = result }));
            Response.End();
        }
    }
}