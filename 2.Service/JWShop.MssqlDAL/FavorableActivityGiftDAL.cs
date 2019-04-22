using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JWShop.MssqlDAL
{
    public sealed class FavorableActivityGiftDAL : IFavorableActivityGift
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(FavorableActivityGiftInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO FavorableActivityGift( Name,Photo,Description) VALUES(@Name,@Photo,@Description);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(FavorableActivityGiftInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE FavorableActivityGift SET Name = @Name, Photo = @Photo, Description = @Description
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from FavorableActivityGift where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public FavorableActivityGiftInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from FavorableActivityGift where Id=@id";

                var data = conn.Query<FavorableActivityGiftInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new FavorableActivityGiftInfo();
            }
        }

        public List<FavorableActivityGiftInfo> SearchList(FavorableActivityGiftSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from FavorableActivityGift where 1=1 ";

                var para = new DynamicParameters();
                if (searchInfo.InGiftIds.Length > 0)
                {
                    sql += " and Id in @ids";
                    para.Add("ids", searchInfo.InGiftIds);
                }
                if (!string.IsNullOrEmpty(searchInfo.Name))
                {
                    sql += " and Name like @name";
                    para.Add("name", "%" + searchInfo.Name + "%");
                }

                return conn.Query<FavorableActivityGiftInfo>(sql, para).ToList();
            }
        }

        public List<FavorableActivityGiftInfo> SearchList(int currentPage, int pageSize, FavorableActivityGiftSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "FavorableActivityGift";
                pc.Fields = "[Id], [Name], [Photo], [Description] ";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<FavorableActivityGiftInfo>(pc).ToList();
            }
        }

        public MssqlCondition  PrepareCondition(FavorableActivityGiftSearchInfo searchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[Name]", searchInfo.Name, ConditionType.Like);
            mssqlCondition.Add("[Id]", string.Join(",", searchInfo.InGiftIds), ConditionType.In);

            return mssqlCondition;
        }

    }
}