using System;
using System.Collections.Generic;
using System.Web;
using JWShop.Common;
using SkyCES.EntLib;

namespace JWShop.Pay.TenPay
{
    public class PayConfig
    {
        private string bargainorID = string.Empty;
        private string businessKey = string.Empty;
        /// <summary>
        /// 商户编号
        /// </summary>
        public string BargainorID
        {
            get { return this.bargainorID; }
        }
        /// <summary>
        /// 商户密钥
        /// </summary>
        public string BusinessKey
        {
            get { return this.businessKey; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PayConfig()
        {
            using (XmlHelper xh = new XmlHelper(ServerHelper.MapPath("/Plugins/Pay/TenPay/TenPay.Config")))
            {
                this.bargainorID = xh.ReadAttribute("Pay/BargainorID", "Value");
                this.businessKey = xh.ReadAttribute("Pay/BusinessKey", "Value");
            }
        }
    }
}
