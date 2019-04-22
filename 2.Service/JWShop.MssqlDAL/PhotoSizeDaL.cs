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
    /// 图片尺寸数据层
    /// </summary>
    public sealed class PhotoSizeDaL:IPhotoSize
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
         public int Add(PhotoSizeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [PhotoSize]([Type],[Title],[Width],[Height],[Introduce]) VALUES(@Type,@Title,@Width,@Height,@Introduce);
                            select SCOPE_IDENTITY()";
                return conn.Query<int>(sql, entity).Single();
            }
        }

         public void Update(PhotoSizeInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE [PhotoSize] SET [Type]=@Type, Title = @Title,[Introduce]=@Introduce,[Width]=@Width,[Height]=@Height where Id=@Id";
                conn.Execute(sql, entity);
            }
        }
        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [PhotoSize] where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [PhotoSize] where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }
        public PhotoSizeInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [PhotoSize] where id=@id";

                var data = conn.Query<PhotoSizeInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new PhotoSizeInfo();
            }
        }

        public List<PhotoSizeInfo> SearchList(int type)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [PhotoSize]";
                if(type>0) sql = "select * from [PhotoSize] where [Type]=@Type";

                return conn.Query<PhotoSizeInfo>(sql, new { Type=type}).ToList();
            }
        }
        public List<PhotoSizeInfo> SearchAllList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [PhotoSize]";
           
                return conn.Query<PhotoSizeInfo>(sql).ToList();
            }
        }
        public List<PhotoSizeInfo> SearchList(int type, int currentPage, int pageSize, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "PhotoSize";
                pc.Fields = "*";
                if (type > 0) pc.MssqlCondition.Add("[Type]", type, ConditionType.Equal);
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.OrderType = OrderType.Asc;

                count = pc.Count;
                return conn.Query<PhotoSizeInfo>(pc).ToList();
            }
        }
    }
}
