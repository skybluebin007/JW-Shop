using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 网站前台导航类型
    /// </summary>
    public enum NavigationType
    {
        /// <summary>
        /// 主站
        /// </summary>
        [Enum("主站")]
        Default = 1
    }
    public enum NavigationClassType
    {
        /// <summary>
        /// Url链接
        /// </summary>
        [Enum("Url链接")]
        Url = 1,
        /// <summary>
        /// 文章
        /// </summary>
        [Enum("文章")]
        Article,
        /// <summary>
        /// 产品
        /// </summary>
        [Enum("产品")]
        Product
    }
    public enum NavigationShowType
    {
        /// <summary>
        /// 列表形式
        /// </summary>
        [Enum("列表形式")]
        List = 1,
        /// <summary>
        /// 列表形式2，无图片
        /// </summary>
        [Enum("列表形式2，无图片")]
        List2 = 2,
        /// <summary>
        /// 平铺形式
        /// </summary>
        [Enum("平铺形式")]
        Spread = 3

    }
    /// <summary>
    /// 网站后台顶部标签页类型
    /// </summary>
    public enum NavTab
    {
        /// <summary>
        /// 基础设置
        /// </summary>
        [Enum("基础设置")]
        Common = 0
    }
}