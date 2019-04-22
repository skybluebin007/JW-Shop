using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 支付方式插件模型
    /// </summary>
    public class PayPluginsInfo
    {
        private string key = string.Empty;
        private string name = string.Empty;
        private string photo = string.Empty;
        private string description = string.Empty;
        private int isCod;
        private int isOnline;
        private int isEnabled = 0;

        public string Key
        {
            set { this.key = value; }
            get { return this.key; }
        }
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }
        public int IsCod
        {
            set { this.isCod = value; }
            get { return this.isCod; }
        }
        public int IsOnline
        {
            set { this.isOnline = value; }
            get { return this.isOnline; }
        }
        public int IsEnabled
        {
            set { this.isEnabled = value; }
            get { return this.isEnabled; }
        }        
    }
}
