using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 商品属性类别
    /// </summary>
    public enum ProductStatusType
    {
        /// <summary>
        /// 是否特价
        /// </summary>
        IsSpecial,
        /// <summary>
        /// 是否新品
        /// </summary>
        IsNew,
        /// <summary>
        /// 是否热卖
        /// </summary>
        IsHot,
        /// <summary>
        /// 是否上架
        /// </summary>
        IsSale,
        /// <summary>
        /// 是否推荐
        /// </summary>
        IsTop,
        /// <summary>
        /// 是否允许评论
        /// </summary>
        AllowComment
    }
}
