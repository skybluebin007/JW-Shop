using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;
using Dapper;
using System.Linq;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 文章数据层说明。
    /// </summary>
    public sealed class ArticleDAL : IArticle
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ArticleInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO Article(  [Title], [ClassId], [IsTop], [Author], [Resource], [Keywords], [Url], [Photo], [Summary], [Content], [Date], [OrderId], [ViewCount], [LoveCount], [RealDate], [FilePath], [ParentId], [AddCol1], [AddCol2],[Content1],[Mobilecontent1],[Content2],[Mobilecontent2],[AddCol3] ) VALUES(@Title,@ClassId,@IsTop,@Author,@Resource,@Keywords,@Url,@Photo,@Summary,@Content,@Date,@OrderId,@ViewCount,@LoveCount,@RealDate,@FilePath,@ParentId,@AddCol1,@AddCol2,@Content1,@Mobilecontent1,@Content2,@Mobilecontent2,@AddCol3);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ArticleInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE Article SET Title = @Title, ClassId = @ClassId, IsTop = @IsTop, Author = @Author, [Resource] = @Resource, Keywords = @Keywords, Url = @Url, Photo = @Photo, Summary = @Summary, Content = @Content, [Date] = @Date, OrderId = @OrderId, ViewCount = @ViewCount, LoveCount = @LoveCount, RealDate = @RealDate, FilePath = @FilePath, ParentId = @ParentId, AddCol1 = @AddCol1, AddCol2 = @AddCol2,Content1=@Content1,Mobilecontent1=@Mobilecontent1,Content2=@Content2,Mobilecontent2=@Mobilecontent2,AddCol3=@AddCol3
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Article where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public ArticleInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Article where id=@id";

                var data = conn.Query<ArticleInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ArticleInfo();
            }
        }
        /// <summary>
        /// 改变文章状态
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="field">要修改的字段</param>
        /// <param name="status">给字段赋的值</param>
        public void ChangeArticleStatus(int id,string field,int status) {
            if (id > 0) {
                using (var conn = new SqlConnection(connectString))
                {
                    string sql = @"UPDATE Article SET "+@field+"=@status where Id=@Id";

                    conn.Execute(sql, new { id = id, field = field, status = status });
                }
            }
        
        }
        public List<ArticleInfo> SearchList(ArticleSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Article";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }
                sql += " Order by [OrderId] desc,[RealDate] desc,[Id] desc";

                return conn.Query<ArticleInfo>(sql).ToList();
            }
        }

        public List<ArticleInfo> SearchList(int currentPage, int pageSize, ArticleSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "Article";
                pc.Fields = "*";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[OrderId],[RealDate],[Id]";
                pc.OrderType = OrderType.Desc;
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<ArticleInfo>(pc).ToList();
            }
        }

        public List<ArticleInfo> SearchListRowNumber(string condition)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from SocoShop_View_ArticleRowNumber";

                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<ArticleInfo>(sql).ToList();
            }
        }

        public MssqlCondition PrepareCondition(ArticleSearchInfo articleSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Title]", articleSearch.Title, ConditionType.Like);
            mssqlCondition.Add("[ClassId]", articleSearch.ClassId, ConditionType.Like);
            mssqlCondition.Add("[IsTop]", articleSearch.IsTop, ConditionType.Equal); ;
            mssqlCondition.Add("[Author]", articleSearch.Author, ConditionType.Like);
            mssqlCondition.Add("[Resource]", articleSearch.Resource, ConditionType.Like);
            mssqlCondition.Add("[Keywords]", articleSearch.Keywords, ConditionType.Like);
            mssqlCondition.Add("[Date]", articleSearch.StartDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[Date]", articleSearch.EndDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[Id]", articleSearch.InArticleId, ConditionType.In);
            mssqlCondition.Add("[ParentId]", articleSearch.ParentId, ConditionType.Equal);
            //指定分类
            if (!string.IsNullOrEmpty(articleSearch.InClassId))
            {
                string tmpCondition =string.Empty;
                if (articleSearch.InClassId.IndexOf(",") < 0) { mssqlCondition.Add("[ClassID]","|" + articleSearch.InClassId + "|",ConditionType.Like); }
                else
                {
                    foreach (string str in articleSearch.InClassId.Split(',')) {
                        if(tmpCondition==string.Empty)
                       tmpCondition="[ClassID] like '|" + str + "|'";                    
                        else
                      tmpCondition+=" or [ClassID] like '|" + str + "|'";
                       
                    }
                    mssqlCondition.Add("("+tmpCondition+")");     
                }
            }
            return mssqlCondition;
        }
    }
}