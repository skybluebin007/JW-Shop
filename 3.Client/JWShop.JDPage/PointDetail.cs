﻿using System;
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

namespace JWShop.Page
{
   public class PointDetail:CommonBasePage
    {
        protected PointProductInfo gift = new PointProductInfo();
        protected List<ProductPhotoInfo> photoList = new List<ProductPhotoInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            int id = RequestHelper.GetQueryString<int>("Id");
            gift = PointProductBLL.Read(id);
            //如果暂未开始或者已经结束
            if (gift.EndDate.Date < DateTime.Now.Date || gift.BeginDate.Date>DateTime.Now.Date) ResponseHelper.Redirect("/pointproduct.html");

            //产品图片
            photoList.Add(new ProductPhotoInfo { Name = gift.Name, ImageUrl = gift.Photo });
            photoList.AddRange(ProductPhotoBLL.ReadList(gift.Id,0));

            Title = gift.Name + " - 积分商品兑换";
        }
    }
}
