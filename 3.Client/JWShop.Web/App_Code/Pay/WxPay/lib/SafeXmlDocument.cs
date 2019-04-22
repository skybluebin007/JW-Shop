using System;
using System.Xml;

namespace JWShop.Pay.WxPay
{
    public class SafeXmlDocument: XmlDocument
    {
        public SafeXmlDocument()
        {
            this.XmlResolver = null;
        }
    }
}