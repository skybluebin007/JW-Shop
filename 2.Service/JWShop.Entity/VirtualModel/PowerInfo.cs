using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 权限模型
    /// </summary>
    public sealed class PowerInfo
    {
        private string text = string.Empty;
        private string key = string.Empty;
        private string xml = string.Empty;
        private string value = string.Empty;

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }
        public string XML
        {
            get { return this.xml; }
            set { this.xml = value; }
        }
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}