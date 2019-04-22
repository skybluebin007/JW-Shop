using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Page.Mobile
{
    public class ProductDetail : CommonBasePage
    {
        /// <summary>
        /// 产品
        /// </summary>
        protected ProductInfo product = new ProductInfo();        
        /// <summary>
        /// 会员等级列表
        /// </summary>
        protected List<UserGradeInfo> userGradeList = new List<UserGradeInfo>();        
        /// <summary>
        /// 产品图片列表
        /// </summary>
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();
        /// <summary>
        /// 关联产品，配件，浏览过的商品
        /// </summary>
        protected List<ProductInfo> tempProductList = new List<ProductInfo>();
        /// <summary>
        /// 浏览过的商品ID串
        /// </summary>
        protected string strHistoryProduct = string.Empty;
        
        /// <summary>
        /// 产品规格值字符串
        /// </summary>
        protected string standardRecordValueList = "|";
        /// <summary>
        /// 规格列表
        /// </summary>
        protected List<ProductTypeStandardInfo> standardList = new List<ProductTypeStandardInfo>();
        /// <summary>
        /// 产品规格列表
        /// </summary>
        protected List<ProductTypeStandardRecordInfo> standardRecordList = new List<ProductTypeStandardRecordInfo>();
        /// <summary>
        /// 剩余库存量
        /// </summary>
        protected int leftStorageCount = 0;

        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();

            int count = int.MinValue;

            int id = RequestHelper.GetQueryString<int>("ID");
            if (id <=0)
            {
                ScriptHelper.AlertFront("该产品未上市，不能查看");
            }
            string fromwhere = RequestHelper.GetQueryString<string>("fw");
            product = ProductBLL.Read(id);
            if (product.IsSale == (int)BoolType.False || product.IsDelete == 1)
            {
                if (fromwhere.ToLower() != "admin")
                    ScriptHelper.Alert("该产品未上市，不能查看");
                else
                {
                    if (Cookies.Admin.GetAdminID(true) == 0)//用户未登录
                    {
                        ScriptHelper.Alert("该产品未上市，不能查看");
                    }
                }
            }

            navList = ProductClassBLL.ProductClassNameList(product.ClassId);
            //更新查看数量
            ProductBLL.ChangeViewCount(id, 1);
            //会员等级
            userGradeList = UserGradeBLL.ReadList();
            
            //产品图片
            ProductPhotoInfo productPhoto = new ProductPhotoInfo();
            productPhoto.Name = product.Name;
            productPhoto.ImageUrl = product.Photo.Replace("Original", "75-75");
            productPhotoList.Add(productPhoto);
            productPhotoList.AddRange(ProductPhotoBLL.ReadList(id, 0));
            // 关联产品，配件，浏览过的商品
            strHistoryProduct = Server.UrlDecode(CookiesHelper.ReadCookieValue("HistoryProduct"));
            string tempStrProductID = product.RelationProduct + "," + product.Accessory + "," + strHistoryProduct;
            tempStrProductID = tempStrProductID.Replace(",,", ",");
            if (tempStrProductID.StartsWith(","))
            {
                tempStrProductID = tempStrProductID.Substring(1);
            }
            if (tempStrProductID.EndsWith(","))
            {
                tempStrProductID = tempStrProductID.Substring(0, tempStrProductID.Length - 1);
            }

            ProductSearchInfo productSearch = new ProductSearchInfo();

            productSearch.InProductId = tempStrProductID;
            productSearch.IsDelete = (int)BoolType.False;
            tempProductList = ProductBLL.SearchList(productSearch);
            //产品规格
            standardRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(product.Id, product.StandardType);
            if (standardRecordList.Count > 0)
            {
                string[] standardIDArray = standardRecordList[0].StandardIdList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < standardIDArray.Length; i++)
                {
                    int standardID = Convert.ToInt32(standardIDArray[i]);
                    ProductTypeStandardInfo standard = ProductTypeStandardBLL.Read(standardID);
                    string[] valueArray = standard.ValueList.Split(';');
                    string valueList = string.Empty;
                    for (int k = 0; k < valueArray.Length; k++)
                    {
                        foreach (ProductTypeStandardRecordInfo standardRecord in standardRecordList)
                        {
                            string[] tempValueArray = standardRecord.ValueList.Split(';');
                            if (valueArray[k] == tempValueArray[i])
                            {
                                valueList += valueArray[k] + ";";
                                break;
                            }
                        }
                    }
                    if (valueList != string.Empty)
                    {
                        valueList = valueList.Substring(0, valueList.Length - 1);
                    }
                    standard.ValueList = valueList;
                    standardList.Add(standard);
                }
                //规格值
                foreach (ProductTypeStandardRecordInfo standardRecord in standardRecordList)
                {
                    standardRecordValueList += standardRecord.ProductId + ";" + standardRecord.ValueList + "|";
                }
            }
            //计算剩余库存量
            if (ShopConfig.ReadConfigInfo().ProductStorageType == (int)ProductStorageType.SelfStorageSystem)
            {
                leftStorageCount = product.TotalStorageCount - product.OrderCount;
            }
            else
            {
                leftStorageCount = product.ImportVirtualStorageCount;
            }
            //搜索优化
            Title = product.Name;
            Keywords = (product.Keywords == string.Empty) ? product.Name : product.Keywords;
            Description = (product.Summary == string.Empty) ? StringHelper.Substring(StringHelper.KillHTML(product.Introduction1), 200) : product.Summary;
        }
    }
}
