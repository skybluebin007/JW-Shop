using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    /// <summary>
    /// 文章分类接口层说明。
    /// </summary>
    public interface IArticleClass
    {
        int Add(ArticleClassInfo entity);
        void Update(ArticleClassInfo entity);
        void Delete(int id);
        ArticleClassInfo Read(int id);
        List<ArticleClassInfo> ReadList();
        void ChangeArticleClassOrder(int id, int orderId);
        void Move(int id, ChangeAction action);
    }
}