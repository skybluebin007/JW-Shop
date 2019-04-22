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
using System.Data;
using Newtonsoft.Json;

namespace JWShop.Web.Admin
{
    public partial class ProductAddInit : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestHelper.GetQueryString<string>("Action")) 
            {
                case"GetTopClass":
                    GetTopClass();
                    break;
                case "GetSecondClass":
                    GetSecondClass();
                    break;
                case"GetThirdClass":
                    GetThirdClass();
                    break;
                case "GetBrandList":
                    GetBrandList();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 搜索1级分类列表
        /// </summary>
        protected void GetTopClass()
        {
           
            string classname = RequestHelper.GetQueryString<string>("classname");
            List<ProductClassInfo> childList = ProductClassBLL.ReadRootList();
          
                if (!string.IsNullOrEmpty(classname)) childList = childList.Where(k => k.Name.Contains(classname)).ToList();
          
            Response.Clear();
            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { count = childList.Count, dataList = childList }));
            Response.End();
        }
        /// <summary>
        /// 根据一级分类获取二级分类列表
        /// </summary>
        protected void GetSecondClass() {
            int classId = RequestHelper.GetQueryString<int>("topClassId");
            string classname = RequestHelper.GetQueryString<string>("classname");
            List<ProductClassInfo> childList = new List<ProductClassInfo>();
            if (classId > 0) {
              childList = ProductClassBLL.ReadChilds(classId);
              if (!string.IsNullOrEmpty(classname)) childList = childList.Where(k => k.Name.Contains(classname)).ToList();  
            }
            Response.Clear();
            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { count = childList.Count, dataList = childList }));
            Response.End();
        }
        /// <summary>
        /// 根据二级分类获取三级分类
        /// </summary>
        protected void GetThirdClass() {
            int classId = RequestHelper.GetQueryString<int>("secondClassId");
            string classname = RequestHelper.GetQueryString<string>("classname");
            List<ProductClassInfo> childList = new List<ProductClassInfo>();
            if (classId > 0)
            {
                childList = ProductClassBLL.ReadChilds(classId);
                if (!string.IsNullOrEmpty(classname)) childList = childList.Where(k => k.Name.Contains(classname)).ToList();  
            }
            Response.Clear();
            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { count = childList.Count, dataList = childList }));
            Response.End();
        }
        /// <summary>
        /// 根据选择分类获取对应的品牌列表
        /// </summary>
        protected void GetBrandList() {
            int classId = RequestHelper.GetQueryString<int>("classId");
            string brandname = RequestHelper.GetQueryString<string>("brandname");
            int proTypeID = ProductClassBLL.GetProductClassType(classId);
            ProductTypeInfo aci = ProductTypeBLL.Read(proTypeID);
            List<ProductBrandInfo> productBrandList = new List<ProductBrandInfo>();
            if (aci.Id > 0)
            {
                string[] strArray = aci.BrandIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                int[] intArray;

                intArray = Array.ConvertAll<string, int>(strArray, s => int.Parse(s));
                productBrandList = ProductBrandBLL.ReadList(intArray);
                if (!string.IsNullOrEmpty(brandname)) productBrandList = productBrandList.Where(k => k.Name.ToLower().Contains(brandname.ToLower())).ToList();  
            }
            Response.Clear();
            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { count = productBrandList.Count, dataList = productBrandList }));
            Response.End();
        }
    }
}