﻿using System;
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
    public class SubjectShow : CommonBasePage
    {
        protected ThemeActivityInfo theme = new ThemeActivityInfo();
        protected string[] styleArray;
        protected string[] productGroupArray = new string[] { };
        protected List<dynamic> productGroup = new List<dynamic>();

        protected override void PageLoad()
        {
            base.PageLoad();

            int id = RequestHelper.GetQueryString<int>("id");
            theme = ThemeActivityBLL.Read(id);
            styleArray = theme.Style.Split('|');           

            if (!string.IsNullOrEmpty(theme.ProductGroup))
            {
                productGroupArray = theme.ProductGroup.Split('#');
                foreach (var group in productGroupArray)
                {
                    var productIds = group.Split('|')[2];
                    if (!string.IsNullOrEmpty(productIds))
                    {
                        int length = productIds.Split(',').Length;
                        int count = 0;
                        var products = ProductBLL.SearchList(1, length, new ProductSearchInfo { InProductId = productIds }, ref count);

                        dynamic dy = new System.Dynamic.ExpandoObject();
                        dy.index = Array.IndexOf(productGroupArray, group);
                        dy.list = products;

                        productGroup.Add(dy);
                    }
                }
            }

            Title = theme.Name;
        }
    }
}