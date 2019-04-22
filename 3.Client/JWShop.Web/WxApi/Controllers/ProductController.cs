using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using JWShop.Entity;
using JWShop.Business;
using JWShop.Common;
using SkyCES.EntLib;
using JWShop.XcxApi.Filter;

namespace JWShop.XcxApi.Controllers
{
    [Auth]
    public class ProductController : Controller
    {   
        [HttpPost]
        public ActionResult List(int id = 1, int limit = 10)
        {
            int count = int.MinValue;
            var keywrods = StringHelper.SearchSafe(Request["keywords"]);
            int pageCount = RequestHelper.GetQueryString<int>("page");
            if (pageCount <= 0) pageCount = 1;

            var productList = ProductBLL.SearchList(pageCount, limit, 
                new ProductSearchInfo()
                {
                    ClassId = "|" + id + "|",
                    IsSale = 1
                }, ref count);

            var datalist = productList.Select(k => new {id= k.Id,name= k.Name }).ToList();
            return Json(new { datalist = datalist }, JsonRequestBehavior.AllowGet);
        }
    }
}
