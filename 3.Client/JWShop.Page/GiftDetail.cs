using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace JWShop.Page
{
    public class GiftDetail : CommonBasePage
    {
        protected PointProductInfo Gift = new PointProductInfo();
        protected List<ProductPhotoInfo> ProductPhotoList = new List<ProductPhotoInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            int id = RequestHelper.GetQueryString<int>("Id");
            Gift = PointProductBLL.Read(id);

            //产品图片
            ProductPhotoList.Add(new ProductPhotoInfo { Name = Gift.Name, ImageUrl = Gift.Photo });
            ProductPhotoList.AddRange(ProductPhotoBLL.ReadList(Gift.Id, 0));
        }
    }
}