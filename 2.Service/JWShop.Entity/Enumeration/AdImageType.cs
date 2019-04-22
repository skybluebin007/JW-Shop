using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 广告类型
    /// </summary>
    public enum AdImageType
    {
        /// <summary>
        /// PC端 - 首页顶部横幅广告
        /// </summary>
        TopBanner = 1,
        /// <summary>
        /// PC端 - 首页Banner
        /// </summary>
        Banner = 2,
        /// <summary>
        /// PC端 - 今日推荐左侧广告
        /// </summary>
        RecommendLeft = 3,
        /// <summary>
        /// PC端 - 今日推荐右侧广告
        /// </summary>
        RecommendRight = 4,
        /// <summary>
        /// PC端 - 分类左侧广告
        /// </summary>
        FloorClass = 5,
        /// <summary>
        /// PC端 - 分类广告-热卖商品
        /// </summary>
        FloorHot = 6,

        /// <summary>
        /// 移动端 - 首页顶部横幅广告
        /// </summary>
        MobileTopBanner = 21,
        /// <summary>
        /// 移动端 - 首页Banner
        /// </summary>
        MobileBanner = 22,
        /// <summary>
        /// 移动端 - 首页顶部推荐
        /// </summary>
        MobileTopRecommend = 23,
        /// <summary>
        /// 移动端 - 首页顶部专题
        /// </summary>
        MobileTopSubject = 24,
        /// <summary>
        /// 移动端 - 首页顶部新品
        /// </summary>
        MobileTopNew = 25,
        /// <summary>
        /// 移动端 - 楼层分类左侧广告
        /// </summary>
        MobileFloorClass = 26,
        /// <summary>
        /// 移动端 - 楼层底部广告
        /// </summary>
        MobileFloorBottom = 27
    }
}