using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 模板模型
    /// </summary>
    public class TemplatePluginsInfo
    {
        private string path = string.Empty;
        private string name = string.Empty;
        private string photo = string.Empty;
        private string disCreateFile = string.Empty;
        private string copyRight = string.Empty;
        private string publishDate = string.Empty;

        public string Path
        {
            set { this.path = value; }
            get { return this.path; }
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
        public string DisCreateFile
        {
            set { this.disCreateFile = value; }
            get { return this.disCreateFile; }
        }
        public string CopyRight
        {
            set { this.copyRight = value; }
            get { return this.copyRight; }
        }
        public string PublishDate
        {
            set { this.publishDate = value; }
            get { return this.publishDate; }
        }
    }
}
