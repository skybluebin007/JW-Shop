using System;
using System.Xml;

namespace JWShop.XcxApi.Pay
{
   public class SafeXmlDocument: XmlDocument
    {
        public SafeXmlDocument()
        {
            this.XmlResolver = null;
        }
    }
}
