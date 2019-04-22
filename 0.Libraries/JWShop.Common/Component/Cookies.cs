using System;
using System.Xml;
using System.Web;
using System.Data;
using System.Web.Security;
using SkyCES.EntLib;

namespace JWShop.Common
{
    /// <summary>
    /// cookies
    /// </summary>
    public sealed class Cookies
    {
        /// <summary>
        /// 后台管理
        /// </summary>
        public sealed class Admin
        {
            private static string cookiesName = ShopConfig.ReadConfigInfo().AdminCookies;
            /// <summary>
            /// 检查cookies
            /// </summary>
            /// <returns></returns>
            private static bool CheckCookies()
            {
                bool flag = false;
                string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                if (cookiesValue != string.Empty)
                {
                    try
                    {
                        string[] strArray = cookiesValue.Split(new char[] { '|' });
                        string sign = strArray[0];
                        string adminID = strArray[1];
                        string adminName = strArray[2];
                        string groupID = strArray[3];
                        string randomNumber = strArray[4];
                        if (FormsAuthentication.HashPasswordForStoringInConfigFile(adminID + adminName + groupID + randomNumber + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5").ToLower() == sign.ToLower())
                        {
                            flag = true;
                        }
                        else
                        {
                            CookiesHelper.DeleteCookie(cookiesName);
                        }
                    }
                    catch
                    {
                        CookiesHelper.DeleteCookie(cookiesName);
                    }
                }
                return flag;
            }
            /// <summary>
            /// 获取管理员ID
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static int GetAdminID(bool check)
            {
                int adminID = 0;
                if (!check || (check && CheckCookies()))
                {
                    string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                    if (cookiesValue != string.Empty)
                    {
                        try
                        {
                            adminID = Convert.ToInt32(cookiesValue.Split(new char[] { '|' })[1]);
                        }
                        catch
                        {
                        }
                    }
                }
                return adminID;
            }
            /// <summary>
            /// 获取管理员名
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static string GetAdminName(bool check)
            {
                string adminName = string.Empty;
                if (!check || (check && CheckCookies()))
                {
                    string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                    if (cookiesValue != string.Empty)
                    {
                        try
                        {
                            adminName = cookiesValue.Split(new char[] { '|' })[2];
                        }
                        catch
                        {
                        }
                    }
                }
                return adminName;
            }
            /// <summary>
            /// 获取管理组ID
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static int GetGroupID(bool check)
            {
                int groupID = 0;
                if (!check || (check && CheckCookies()))
                {
                    string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                    if (cookiesValue != string.Empty)
                    {
                        try
                        {
                            groupID = Convert.ToInt32(cookiesValue.Split(new char[] { '|' })[3]);
                        }
                        catch
                        {
                        }
                    }
                }
                return groupID;
            }
            /// <summary>
            /// 随机数
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static string GetRandomNumber(bool check)
            {
                string randomNumber = string.Empty;
                if (!check || (check && CheckCookies()))
                {
                    string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                    if (cookiesValue != string.Empty)
                    {
                        try
                        {
                            randomNumber = cookiesValue.Split(new char[] { '|' })[4];
                        }
                        catch
                        {
                        }
                    }
                }
                return randomNumber;
            }
        }
        /// <summary>
        /// 前台用户(提供给插件使用)
        /// </summary>
        public sealed class User
        {
            private static string cookiesName = ShopConfig.ReadConfigInfo().UserCookies;
            /// <summary>
            /// 检查cookies
            /// </summary>
            /// <returns></returns>
            private static bool CheckCookies()
            {
                bool flag = false;
                string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                if (cookiesValue != string.Empty)
                {
                    try
                    {
                        string[] strArray = cookiesValue.Split(new char[] { '|' });
                        string sign = strArray[0];
                        string userID = strArray[1];
                        string userName = strArray[2];
                        string gradeID = strArray[3];
                        if (FormsAuthentication.HashPasswordForStoringInConfigFile(userID + userName + gradeID + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5").ToLower() == sign.ToLower())
                        {
                            flag = true;
                        }
                        else
                        {
                            CookiesHelper.DeleteCookie(cookiesName);
                        }
                    }
                    catch
                    {
                        CookiesHelper.DeleteCookie(cookiesName);
                    }
                }
                return flag;
            }
            /// <summary>
            /// 获取用户ID
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static int GetUserID(bool check)
            {
                int userID = 0;
                if (!check || (check && CheckCookies()))
                {
                    string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                    if (cookiesValue != string.Empty)
                    {
                        try
                        {
                            userID = Convert.ToInt32(cookiesValue.Split(new char[] { '|' })[1]);
                        }
                        catch
                        {
                        }
                    }
                }
                return userID;
            }
            /// <summary>
            /// 获取用户名
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static string GetUserName(bool check)
            {
                string userName = string.Empty;
                if (!check || (check && CheckCookies()))
                {
                    string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                    if (cookiesValue != string.Empty)
                    {
                        try
                        {
                            userName = HttpContext.Current.Server.UrlDecode(cookiesValue.Split(new char[] { '|' })[2]);
                        }
                        catch
                        {
                        }
                    }
                }
                return userName;
            }
            /// <summary>
            /// 获取用户消费金额
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static decimal GetMoneyUsed(bool check)
            {
                decimal moneyUsed = 0;
                if (!check || (check && CheckCookies()))
                {
                    string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                    if (cookiesValue != string.Empty)
                    {
                        try
                        {
                            moneyUsed = Convert.ToDecimal(cookiesValue.Split(new char[] { '|' })[3]);
                        }
                        catch
                        {
                        }
                    }
                }
                return moneyUsed;
            }
            /// <summary>
            /// 获取用户等级ID
            /// </summary>
            /// <param name="check"></param>
            /// <returns></returns>
            public static int GetGradeID(bool check)
            {
                int gradeID = 0;
                if (!check || (check && CheckCookies()))
                {
                    string cookiesValue = CookiesHelper.ReadCookieValue(cookiesName);
                    if (cookiesValue != string.Empty)
                    {
                        try
                        {
                            gradeID = Convert.ToInt32(cookiesValue.Split(new char[] { '|' })[4]);
                        }
                        catch
                        {
                        }
                    }
                }
                return gradeID;
            }
        }
        /// <summary>
        /// 其他常用Cookies
        /// </summary>
        public sealed class Common
        {
            /// <summary>
            /// 获取明文验证码
            /// </summary>
            /// <returns></returns>
            public static string CheckCode
            {
                get
                {
                    return StringHelper.Decode(CookiesHelper.ReadCookieValue(SkyCES.EntLib.CheckCode.CookiesName), SkyCES.EntLib.CheckCode.Key);
                }
            }
            ///// <summary>
            ///// 购物车产品购买数量
            ///// </summary>
            //public static int ProductBuyCount
            //{
            //    get
            //    {
            //        int productBuyCount = 0;
            //        try
            //        {
            //            productBuyCount = Convert.ToInt32(StringHelper.Decode(CookiesHelper.ReadCookieValue("ProductBuyCount"), ShopConfig.ReadConfigInfo().SecureKey));
            //        }
            //        catch
            //        {
            //            CookiesHelper.DeleteCookie("ProductBuyCount");
            //        }
            //        return productBuyCount;
            //    }
            //    set
            //    {
            //        CookiesHelper.AddCookie("ProductBuyCount", StringHelper.Encode(value.ToString(), ShopConfig.ReadConfigInfo().SecureKey));
            //    }
            //}
            ///// <summary>
            ///// 购物车产品购买金额
            ///// </summary>
            //public static decimal ProductTotalPrice
            //{
            //    get
            //    {
            //        decimal productTotalPrice = 0;
            //        try
            //        {
            //            productTotalPrice = Convert.ToDecimal(StringHelper.Decode(CookiesHelper.ReadCookieValue("ProductTotalPrice"),ShopConfig.ReadConfigInfo().SecureKey));
            //        }
            //        catch
            //        {
            //            CookiesHelper.DeleteCookie("ProductTotalPrice");
            //        }
            //        return productTotalPrice;
            //    }
            //    set
            //    {
            //        CookiesHelper.AddCookie("ProductTotalPrice", StringHelper.Encode(value.ToString(), ShopConfig.ReadConfigInfo().SecureKey));
            //    }
            //}
            ///// <summary>
            ///// 购物车产品购买重量
            ///// </summary>
            //public static decimal ProductTotalWeight
            //{
            //    get
            //    {
            //        decimal productTotalWeight = 0;
            //        try
            //        {
            //            productTotalWeight = Convert.ToDecimal(StringHelper.Decode(CookiesHelper.ReadCookieValue("ProductTotalWeight"), ShopConfig.ReadConfigInfo().SecureKey));
            //        }
            //        catch
            //        {
            //            CookiesHelper.DeleteCookie("ProductTotalWeight");
            //        }
            //        return productTotalWeight;
            //    }
            //    set
            //    {
            //        CookiesHelper.AddCookie("ProductTotalWeight", StringHelper.Encode(value.ToString(), ShopConfig.ReadConfigInfo().SecureKey));
            //    }
            //}
        }
    }
}
