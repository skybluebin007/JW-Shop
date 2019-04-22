using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWShop
{
    /// <summary>
    /// 微信退款结果模型
    /// </summary>
   public sealed class WxpayResult
    {
        public bool result_code { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }
    }
}
