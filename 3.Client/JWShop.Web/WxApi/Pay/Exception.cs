using System;
using System.Collections.Generic;
using System.Web;

namespace JWShop.XcxApi.Pay
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg)
            : base(msg)
        {

        }
    }
}