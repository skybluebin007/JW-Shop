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

namespace JWShop.Page.Lab
{
    public class Detail : CommonBasePage
    {
        protected ProductInfo product = new ProductInfo();
        protected UserInfo shop = new UserInfo();
        protected ProductBrandInfo productBrand = new ProductBrandInfo();
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();
        protected string paths;
        protected List<string[]> listPaths;
        protected List<ProductTypeAttributeRecordInfo> attributeRecords = new List<ProductTypeAttributeRecordInfo>();
        protected List<ProductTypeStandardRecordInfo> standardRecords = new List<ProductTypeStandardRecordInfo>();
        protected List<ProductInfo> topProductList = new List<ProductInfo>();
        protected List<ProductInfo> classProductList = new List<ProductInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "GetStandardPrice":
                    GetStandardPrice();
                    break;
                case "Like":
                    Like();
                    break;
                case "Collect":
                    Collect();
                    break;
            }

            int id = RequestHelper.GetQueryString<int>("id");
            product = ProductBLL.Read(id);
            if (product.IsSale == (int)BoolType.False)
            {
                ScriptHelper.AlertFront("该产品未上市，不能查看");
            }

            //更新查看数量
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ViewCount", product.ViewCount + 1);
            ProductBLL.UpdatePart(ProductInfo.TABLENAME, dict, id);

            if (product.BrandId > 0) productBrand = ProductBrandBLL.Read(product.BrandId);
            if (product.ShopId > 0) shop = UserBLL.Read(product.ShopId);
            productPhotoList = ProductPhotoBLL.ReadList(id);
            attributeRecords = ProductTypeAttributeRecordBLL.ReadList(id);
            standardRecords = ProductTypeStandardRecordBLL.ReadList(id);
            //导航路径
            listPaths = ProductClassBLL.ReadNavigationPath(product.ClassId);
            listPaths.ForEach(k => paths += string.Format(k[0], k[1], k[2]));

            //排行榜
            //按销量倒序
            int count = 0;
            topProductList = ProductBLL.SearchList(1, 6, new ProductSearchInfo { ClassId = product.ClassId, IsSale = (int)BoolType.True, ProductOrderType = "SendCount", OrderType = OrderType.Desc }, ref count);

            //同类推荐
            classProductList = ProductBLL.SearchList(1, 3, new ProductSearchInfo { ClassId = product.ClassId, IsSale = (int)BoolType.True, IsTop = (int)BoolType.True }, ref count);

            //搜索优化
            Title = product.Name;
            Keywords = string.IsNullOrEmpty(product.Keywords) ? product.Name : product.Keywords;
            Description = string.IsNullOrEmpty(product.Summary) ? StringHelper.Substring(StringHelper.KillHTML(product.Introduction1), 200) : product.Summary;
        }

        private void GetStandardPrice()
        {
            int id = RequestHelper.GetQueryString<int>("id");
            string valueList = RequestHelper.GetQueryString<string>("value");

            var entity = ProductTypeStandardRecordBLL.Read(id, valueList);
            ResponseHelper.Write(entity.SalePrice.ToString("C") + "|" + (entity.Storage - entity.OrderCount));
            ResponseHelper.End();
        }
        private void Like()
        {
            string result = string.Empty;
            int id = RequestHelper.GetQueryString<int>("id");
            string type = RequestHelper.GetQueryString<string>("type");

            if (id < 1)
            {
                ResponseHelper.Write("error|请选择产品");
                ResponseHelper.End();
            }
            if (base.UserId == 0)
            {
                ResponseHelper.Write("error|还未登录");
                ResponseHelper.End();
            }
            string hasLike = CookiesHelper.ReadCookieValue("like");
            if (!string.IsNullOrEmpty(hasLike))
            {
                ResponseHelper.Write("error|您已经参与过了");
                ResponseHelper.End();
            }

            var entity = ProductBLL.Read(id);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (type == "like") dict.Add("LikeNum", entity.LikeNum + 1);
            if (type == "unlike") dict.Add("UnLikeNum", entity.UnLikeNum + 1);

            ProductBLL.UpdatePart(ProductInfo.TABLENAME, dict, id);

            int num = type == "like" ? entity.LikeNum + 1 : entity.UnLikeNum + 1;

            CookiesHelper.AddCookie("like", "hasLike", 1, TimeType.Day);
            ResponseHelper.Write("ok|" + num);
            ResponseHelper.End();
        }
        private void Collect()
        {
            string result = string.Empty;
            int productId = RequestHelper.GetQueryString<int>("id");
            if (productId > 0)
            {
                if (base.UserId == 0)
                {
                    result = "还未登录";
                }
                else
                {
                    var productCollect = ProductCollectBLL.Read(productId, base.UserId) ?? new ProductCollectInfo();
                    if (productCollect.Id > 0)
                    {
                        ProductCollectBLL.Delete(new int[] { productCollect.Id }, base.UserId);
                        result = "已取消收藏";
                    }
                    else
                    {
                        productCollect.ProductId = productId;
                        productCollect.Tm = RequestHelper.DateNow;
                        productCollect.UserId = base.UserId;
                        ProductCollectBLL.Add(productCollect);
                        result = "成功收藏";
                    }
                }
            }
            else
            {
                result = "请选择产品";
            }
            ResponseHelper.Write(result);
            ResponseHelper.End();
        }
    }
}