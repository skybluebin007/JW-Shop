using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 登录插件模型
    /// </summary>
    public class LoginPluginsInfo
    {
        private string key = string.Empty;
        private string name = string.Empty;
        private string ename = string.Empty;
        private string photo = string.Empty;
        private string description = string.Empty;  
        private int isEnabled = 0;
        private string doMain = string.Empty;
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
        public string EName
        {
            set { this.ename = value; }
            get { return this.ename; }
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
        public int IsEnabled
        {
            set { this.isEnabled = value; }
            get { return this.isEnabled; }
        }
        public string DoMain
        {
            get { return this.doMain; }
            set { this.doMain = value; }
        }
    }
}
