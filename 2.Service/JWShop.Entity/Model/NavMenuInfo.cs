using System;
using System.Collections.Generic;

namespace JWShop.Entity
{
    /// <summary>
    /// 导航菜单实体模型
    /// </summary>
    public sealed class NavMenuInfo
    {
        public int Id
        {
            set;
            get;
        }
        public string Name
        {
            set;
            get;
        }
        public int IsShow
        {
            set;
            get;
        }
        public int OrderId
        {
            set;
            get;
        }
        public string LinkUrl
        {
            set;
            get;
        }
        public string Introduce
        {
            set;
            get;
        }        
    }
}
