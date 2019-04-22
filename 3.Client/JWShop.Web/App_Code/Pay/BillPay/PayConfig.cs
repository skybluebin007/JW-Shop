using System;
using System.Collections.Generic;
using System.Web;
using JWShop.Common;
using SkyCES.EntLib;

namespace JWShop.Pay.BillPay
{
    public class PayConfig
    {
        private string merchantID = string.Empty;
        private string merchantKey = string.Empty;
        /// <summary>
        /// 商户编号
        /// </summary>
        public string MerchantID
        {
            get { return this.merchantID; }
        }
        /// <summary>
        /// 商户密钥
        /// </summary>
        public string MerchantKey
        {
            get { return this.merchantKey; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PayConfig()
        {
            using (XmlHelper xh = new XmlHelper(ServerHelper.MapPath("/Plugins/Pay/BillPay/BillPay.Config")))
            {
                this.merchantID = xh.ReadAttribute("Pay/MerchantID", "Value");
                this.merchantKey = xh.ReadAttribute("Pay/MerchantKey", "Value");
            }
        }
    }
}
