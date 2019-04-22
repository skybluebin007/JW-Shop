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
using System.Web.Security;

namespace JWShop.Page
{
    /// <summary>
    /// 后台页面基类
    /// </summary>
    public class AdminBasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 当前的顶部标签选项卡
        /// </summary>
        public EnumInfo CurNavTab
        {
            get
            {
                var navTabs = EnumHelper.ReadEnumList<NavTab>();
                int curNavTab = RequestHelper.GetQueryString<int>("navTab");
                if (curNavTab == int.MinValue) curNavTab = (int)NavTab.Common;

                return navTabs.SingleOrDefault(k => k.Value == curNavTab) ?? new EnumInfo();
            }
        }

        /// <summary>
        /// 判断权限的AdminID(-1表示无权限， int.MinValue可以操作所有数据，否则就只能操作自己添加的数据)
        /// </summary>
        protected int AdminID = 0;
        private int pageSize = 15;
        public int PageSize {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }
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
        /// 绑定控件
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="rpeater"></param>
        protected void BindControl(object dataSource, Repeater repeater)
        {
            BindControl(dataSource, repeater, null);
        }
        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="commonPager"></param>
        protected void BindControl(CommonPager commonPager)
        {
            BindControl(null, null, commonPager);
        }
        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="rpeater"></param>
        /// <param name="commonPager"></param>
        protected void BindControl(object dataSource, Repeater repeater, CommonPager commonPager)
        {
            if (repeater != null)
            {
                repeater.DataSource = dataSource;
                repeater.DataBind();
            }
            if (commonPager != null)
            {
                commonPager.CurrentPage = CurrentPage;
                commonPager.PageSize = PageSize;
                commonPager.Count = Count;
                commonPager.FirstPage = "首页";
                commonPager.LastPage = "尾页";
            }
        }
        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <returns></returns>
        protected static string GetAddUpdate()
        {
            string content = "添加";
            if (RequestHelper.GetQueryString<int>("ID") > 0)
            {
                content = "修改";
            }
            return content;
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
        protected override void OnInit(EventArgs e)
        {
            CheckAdminLogin();
            base.OnInit(e);
        }
        ///<summary>
        /// 验证用户是否登陆
        ///</summary>
        private void CheckAdminLogin()
        {
            if (Cookies.Admin.GetAdminID(true) == 0)
            {
                ResponseHelper.Write("<script language='javascript'>window.parent.location.href='/Admin/Login.aspx';</script>");
                ResponseHelper.End();
            }
            else
            {
                if (CookiesHelper.ReadCookie("AdminSign") != null)
                {
                    AdminInfo admin = AdminBLL.Read(Cookies.Admin.GetAdminID(true));
                    string signvalue = FormsAuthentication.HashPasswordForStoringInConfigFile(admin.Id.ToString() + admin.Name + admin.GroupId.ToString() + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent + admin.Password, "MD5");
                    if (signvalue != CookiesHelper.ReadCookieValue("AdminSign"))
                    {
                        ResponseHelper.Write("<script language='javascript'>window.parent.location.href='/Admin/Login.aspx';</script>");
                        ResponseHelper.End();
                    }
                }
                else
                {
                    ResponseHelper.Write("<script language='javascript'>window.parent.location.href='/Admin/Login.aspx';</script>");
                    ResponseHelper.End();
                }
            }
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
