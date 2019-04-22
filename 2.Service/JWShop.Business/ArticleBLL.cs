using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Business
{
    /// <summary>
    /// 文章业务逻辑。
    /// </summary>
    public sealed class ArticleBLL : BaseBLL
    {
        private static readonly IArticle dal = FactoryHelper.Instance<IArticle>(Global.DataProvider, "ArticleDAL");
        public static readonly int TableID = UploadTable.ReadTableID("Article");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("BottomList");

        public static int Add(ArticleInfo entity)
        {
            entity.Id = dal.Add(entity);
            UploadBLL.UpdateUpload(TableID, 0, entity.Id, Cookies.Admin.GetRandomNumber(false));
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(ArticleInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
            UploadBLL.UpdateUpload(TableID, 0, entity.Id, Cookies.Admin.GetRandomNumber(false));
        }

        public static void Delete(int[] ids)
        {
            UploadBLL.DeleteUploadByRecordID(TableID, string.Join(",", ids));
            dal.Delete(ids);
            CacheHelper.Remove(cacheKey);
        }

        public static ArticleInfo Read(int id)
        {
            return dal.Read(id);
        }
        public static void ChangeArticleStatus(int id, string field, int status) {
            dal.ChangeArticleStatus(id, field, status);
        }
        public static List<ArticleInfo> SearchList(int currentPage, int pageSize, ArticleSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }

        /// <summary>
        /// 通过RowNumber排序显示上一篇下一篇
        /// </summary>
        public static List<ArticleInfo> SearchListRowNumber(string condition)
        {
            return dal.SearchListRowNumber(condition);
        }

        public static List<ArticleInfo> SearchList(ArticleSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }

        public static List<ArticleInfo> ReadBottomList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.ClassId = "|3|";
                CacheHelper.Write(cacheKey, dal.SearchList(articleSearch));
            }
            return (List<ArticleInfo>)CacheHelper.Read(cacheKey);
        }

        public static int GetArticleCounts(int id) {
            return dal.SearchList(new ArticleSearchInfo { ClassId = "|" + id + "|" }).Count;
        }
    }
}