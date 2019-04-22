using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 活动插件模型
    /// </summary>
    public class ActivityPluginsInfo
    {
        private string name = string.Empty;
        private string key = string.Empty;       
        private string description = string.Empty;
        private string adminUrl=string.Empty;
        private string showUrl = string.Empty;
        private string photo = string.Empty;
        private int isEnabled;
        private string applyVersion = string.Empty;
        private string copyRight = string.Empty;

        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        public string Key
        {
            set { this.key = value; }
            get { return this.key; }
        }  
        public string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }
        public string AdminUrl
        {
            set { this.adminUrl = value; }
            get { return this.adminUrl; }
        }
        public string ShowUrl
        {
            set { this.showUrl = value; }
            get { return this.showUrl; }
        }  
        public string Photo
        {
            set { this.photo = value; }
            get { return this.photo; }
        }
        public int IsEnabled
        {
            set { this.isEnabled = value; }
            get { return this.isEnabled; }
        }
        public string ApplyVersion
        {
            set { this.applyVersion = value; }
            get { return this.applyVersion; }
        }
        public string CopyRight
        {
            set { this.copyRight = value; }
            get { return this.copyRight; }
        }
    }
}
