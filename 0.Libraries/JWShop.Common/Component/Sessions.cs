using System;
using System.Web;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Common
{
    /// <summary>
    /// Session
    /// </summary>
    public sealed class Sessions
    {
        /// <summary>
        /// 购物车产品购买数量
        /// </summary>
        public static int ProductBuyCount
        {
            get
            {
                int productBuyCount = 0;
                if (HttpContext.Current.Session["ProductBuyCount"] != null)
                {
                    productBuyCount = Convert.ToInt32(HttpContext.Current.Session["ProductBuyCount"]);
                }
                return productBuyCount;
            }
            set
            {
                HttpContext.Current.Session["ProductBuyCount"] = value.ToString();
            }
        }
        /// <summary>
        /// 购物车产品购买金额
        /// </summary>
        public static decimal ProductTotalPrice
        {
            get
            {
                decimal productTotalPrice = 0;
                if (HttpContext.Current.Session["ProductTotalPrice"] != null)
                {
                    productTotalPrice = Convert.ToDecimal(HttpContext.Current.Session["ProductTotalPrice"]);
                }
                return productTotalPrice;
            }
            set
            {
                HttpContext.Current.Session["ProductTotalPrice"] = value.ToString();
            }
        }
        /// <summary>
        /// 购物车产品购买重量
        /// </summary>
        public static decimal ProductTotalWeight
        {
            get
            {
                decimal productTotalWeight = 0;
                if (HttpContext.Current.Session["ProductTotalWeight"] != null)
                {
                    productTotalWeight = Convert.ToDecimal(HttpContext.Current.Session["ProductTotalWeight"]);
                }
                return productTotalWeight;
            }
            set
            {
                HttpContext.Current.Session["ProductTotalWeight"] = value.ToString();
            }
        }
    }
}
