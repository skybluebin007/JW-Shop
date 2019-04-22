using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 文章接口层说明。
    /// </summary>
    public interface IArticle
    {
        int Add(ArticleInfo entity);
        void Update(ArticleInfo entity);
        void Delete(int[] ids);
        ArticleInfo Read(int id);
        void ChangeArticleStatus(int id, string field, int status);
        List<ArticleInfo> SearchList(ArticleSearchInfo searchInfo);
        List<ArticleInfo> SearchList(int currentPage, int pageSize, ArticleSearchInfo searchInfo, ref int count);
        
        List<ArticleInfo> SearchListRowNumber(string condition);
    }
}