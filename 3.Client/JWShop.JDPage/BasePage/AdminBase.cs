using System;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using System.Linq;

namespace JWShop.Page.Admin
{
    /// <summary>
    /// 商家移动版后台基类
    /// </summary>
   public class AdminBase:CommonBasePage
    {


        /// <summary>
        /// 判断权限的AdminID(-1表示无权限， int.MinValue可以操作所有数据，否则就只能操作自己添加的数据)
        /// </summary>
        protected int AdminID = 0;
        protected int PageSize = 15;
        protected int Count = 0;
        protected int CurrentPage
        {
            get
            {
                int currentPage = RequestHelper.GetQueryString<int>("Page");
                if (currentPage < 1)
                {
                    currentPage = 1;
                }
                return currentPage;
            }
        }
        protected string NamePrefix = ShopConfig.ReadConfigInfo().NamePrefix;
        protected string IDPrefix = ShopConfig.ReadConfigInfo().IDPrefix;
        /// <summary>
        /// 清除缓存
        /// </summary>
        protected void ClearCache()
        {
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            Response.Expires = 0;
        }

        /// <summary>
        /// 后台保存完之后是否自动关闭弹窗
        /// </summary>
        /// <param name="alertMessage"></param>
        /// <param name="url"></param>
        protected static void Alert(string alertMessage, string url)
        {
            if (ShopConfig.ReadConfigInfo().SaveAutoClosePop == (int)BoolType.True)
            {
                string js = "<script language='javascript'>top.window.layer.alert('" + alertMessage + "', {skin: 'layui-layer-jw',icon: 1,closeBtn: 0}, function(){";
                if (ShopConfig.ReadConfigInfo().PopCloseRefresh == (int)BoolType.True)
                {
                    js += "top.window.frames[0].location.reload();";
                }
                js += "top.window.layer.closeAll();";
                js += "});</script>";
                ResponseHelper.Write(js);
                ResponseHelper.End();
            }
            else
            {
                ScriptHelper.AlertFront(alertMessage, url);
            }
        }


        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void PageLoad()
        {
            base.PageLoad();
            CheckAdminLogin();        
        }
        ///<summary>
        /// 验证用户是否登陆
        ///</summary>
        private void CheckAdminLogin()
        {
            if (Cookies.Admin.GetAdminID(true) == 0)
            {
                ResponseHelper.Write("<script language='javascript'>window.parent.location.href='/MobileAdmin/Login.html?redirecturl="+RequestHelper.RawUrl+"';</script>");
                ResponseHelper.End();
            }
        }
        /// <summary>
        /// 获取对应状态下订单的个数
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        protected int GetOrderCount(int orderStatus)
        {
            int result = 0;
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            if (orderStatus > 0)
            {
                //如果查找已删除订单
                if (orderStatus == (int)Entity.OrderStatus.HasDelete)
                {
                    orderSearch.IsDelete = (int)BoolType.True;//已删除
                }
                else
                {
                    orderSearch.OrderStatus = orderStatus;
                    orderSearch.IsDelete = (int)BoolType.False;//未删除
                }
            }
            var orderList = OrderBLL.SearchList(CurrentPage, PageSize, orderSearch, ref result);
            return result;
        }
        ///<summary>
        ///检查用户权限
        ///</summary>
        ///<param name="powerKey">权限的前缀</param>
        ///<param name="powerString">要检查的权限值</param>
        ///<param name="checktype">检查类型</param>
        ///<returns></returns>
        protected void CheckAdminPower(string powerString, PowerCheckType powerCheckType)
        {
            CheckAdminPower(ShopConfig.ReadConfigInfo().PowerKey, powerString, PowerCheckType.Single, ref AdminID);
        }
        ///<summary>
        ///检查用户权限
        ///</summary>
        ///<param name="powerKey">权限的前缀</param>
        ///<param name="powerString">要检查的权限值</param>
        ///<param name="checktype">检查类型</param>
        ///<returns></returns>
        private void CheckAdminPower(string powerKey, string powerString, PowerCheckType powerCheckType, ref int adminID)
        {
            string power = AdminGroupBLL.Read(Cookies.Admin.GetGroupID(false)).Power;
            //检查权限
            bool checkPower = false;
            switch (powerCheckType)
            {
                case PowerCheckType.Single:
                    if (power.IndexOf("|" + powerKey + powerString + "|") > -1)
                    {
                        checkPower = true;
                    }
                    break;
                case PowerCheckType.OR:
                    foreach (string TempPowerString in powerString.Split(','))
                    {
                        if (power.IndexOf("|" + powerKey + TempPowerString + "|") > -1)
                        {
                            checkPower = true;
                            break;
                        }
                    }
                    break;
                case PowerCheckType.AND:
                    checkPower = true;
                    foreach (string TempPowerString in powerString.Split(','))
                    {
                        if (power.IndexOf("|" + powerKey + TempPowerString + "|") == -1)
                        {
                            checkPower = false;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            if (checkPower)
            {
                //是否需要检查具有操作别人的权限
                bool needOther = false;
                Hashtable ht = ReadAllNeedOther();
                foreach (DictionaryEntry dic in ht)
                {
                    if (dic.Key.ToString() == powerString)
                    {
                        needOther = Convert.ToBoolean(dic.Value);
                        if (!needOther)
                        {
                            break;
                        }
                    }
                }

                // 检查是否具有操作别人的权限
                if (needOther)
                {
                    if (power.IndexOf("|" + powerKey + "ManageOther|") > -1)
                    {
                        adminID = int.MinValue;
                    }
                    else
                    {
                        adminID = Cookies.Admin.GetAdminID(false);
                    }
                }
                else
                {
                    adminID = int.MinValue;
                }
            }
            else
            {
                adminID = -1;
            }
            if (adminID == -1)
            {
                ScriptHelper.AlertFront(ShopLanguage.ReadLanguage("NoPower"));
            }
        }
        /// <summary>
        /// 缓存所有权限的“是否需要检查具有操作别人的权限”的值
        /// </summary>
        /// <returns></returns>
        private Hashtable ReadAllNeedOther()
        {
            string cacheKey = CacheKey.ReadCacheKey("NeedOther");
            Hashtable ht = new Hashtable();
            if (CacheHelper.Read(cacheKey) == null)
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(ServerHelper.MapPath("/Config/AdminPower.config"));
                XmlNodeList xnList = xd.GetElementsByTagName("Item");
                foreach (XmlNode xn in xnList)
                {
                    ht.Add(xn.Attributes["Value"].Value, xn.Attributes["NeedOther"].Value);
                }
                xd = null;
                CacheHelper.Write(cacheKey, ht);
            }
            else
            {
                ht = (Hashtable)CacheHelper.Read(cacheKey);
            }
            return ht;
        }
      
    }
}
