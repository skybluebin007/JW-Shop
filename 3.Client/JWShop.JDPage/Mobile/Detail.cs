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

namespace JWShop.Page.Mobile
{
    public class Detail : CommonBasePage
    {
        protected ProductInfo product = new ProductInfo();
        protected ProductBrandInfo productBrand = new ProductBrandInfo();
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();
        protected List<ProductTypeAttributeRecordInfo> attributeRecords = new List<ProductTypeAttributeRecordInfo>();
        protected List<ProductTypeStandardRecordInfo> standardRecords = new List<ProductTypeStandardRecordInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            int id = RequestHelper.GetQueryString<int>("id");
            product = ProductBLL.Read(id);
            if (product.IsSale == (int)BoolType.False)
            {
                ScriptHelper.AlertFront("该产品未上市，不能查看");
            }
            //如果为移动端单独设置了内容，则取移动端内容
            if (!string.IsNullOrEmpty(product.Introduction1_Mobile)) product.Introduction1 = product.Introduction1_Mobile;
            if (!string.IsNullOrEmpty(product.Introduction2_Mobile)) product.Introduction2 = product.Introduction2_Mobile;
            if (!string.IsNullOrEmpty(product.Introduction3_Mobile)) product.Introduction3 = product.Introduction3_Mobile;

            //更新查看数量
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ViewCount", product.ViewCount + 1);
            ProductBLL.UpdatePart(ProductInfo.TABLENAME, dict, id);

            if (product.BrandId > 0) productBrand = ProductBrandBLL.Read(product.BrandId);
            productPhotoList = ProductPhotoBLL.ReadList(id,0);
            attributeRecords = ProductTypeAttributeRecordBLL.ReadList(id);
            standardRecords = ProductTypeStandardRecordBLL.ReadList(id);

            //搜索优化
            Title = product.Name;
            Keywords = string.IsNullOrEmpty(product.Keywords) ? product.Name : product.Keywords;
            Description = string.IsNullOrEmpty(product.Summary) ? StringHelper.Substring(StringHelper.KillHTML(product.Introduction1), 200) : product.Summary;
        }
    }
}