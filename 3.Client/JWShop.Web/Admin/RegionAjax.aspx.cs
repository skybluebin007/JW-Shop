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
    public partial class RegionAjax : JWShop.Page.AdminBasePage
    {
       
        protected List<RegionInfo> regionList = new List<RegionInfo>();
        protected int id = 0;
        protected string name = string.Empty;
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearCache();

            string action = RequestHelper.GetQueryString<string>("Action");           
            switch (action)
            {
                case "Read":
                    ReadRegion();
                    break;
                case "Add":
                    AddRegion();
                    break;
                case "Delete":
                    DeleteRegion();
                    break;
                case "Update":
                    UpdateRegion();
                    break;
                default:
                    break;
            }            
        }
        /// <summary>
        /// 读取地区数据
        /// </summary>
        protected void ReadRegion()
        {
            CheckAdminPower("ReadProduct", PowerCheckType.Single);
            id = RequestHelper.GetQueryString<int>("ID");
            if (id > 0)
            {
                name = RegionBLL.ReadRegionCache(id).RegionName;
            }
            regionList = RegionBLL.ReadRegionChildList(id);          
        }
        /// <summary>
        /// 增加一条地区数据
        /// </summary>
        protected void AddRegion()
        {
            CheckAdminPower("ReadProduct", PowerCheckType.Single);
            RegionInfo region = new RegionInfo();
            region.FatherID = RequestHelper.GetQueryString<int>("FatherID");
            region.RegionName = RequestHelper.GetQueryString<string>("RegionName");          
            int id = RegionBLL.AddRegion(region);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Region"), id);
            ResponseHelper.End();
        }
        /// <summary>
        /// 删除一条地区数据
        /// </summary>
        protected void DeleteRegion()
        {
            CheckAdminPower("ReadProduct", PowerCheckType.Single);
            id = RequestHelper.GetQueryString<int>("ID");            
            if (RegionBLL.ReadRegionChildList(id).Count > 0)
            {
                Response.Write("error");
                Response.End();
            }
            else
            {
                RegionBLL.DeleteRegion(id);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Region"), id);
            }
        }
        /// <summary>
        /// 更新一条地区数据
        /// </summary>
        protected void UpdateRegion()
        {
            CheckAdminPower("ReadProduct", PowerCheckType.Single);
            RegionInfo region = new RegionInfo();
            region.ID = RequestHelper.GetQueryString<int>("ID");
            region.FatherID = RegionBLL.ReadRegionCache(region.ID).FatherID;
            region.RegionName = RequestHelper.GetQueryString<string>("Name");
            RegionBLL.UpdateRegion(region);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Region"), region.ID);
            Response.End();
        }
    }
}