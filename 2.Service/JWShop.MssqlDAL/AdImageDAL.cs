using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Linq;

namespace JWShop.MssqlDAL
{
    public sealed class AdImageDAL : IAdImage
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(AdImageInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                long id = conn.Insert<AdImageInfo>(entity);
                return Convert.ToInt32(id);
            }
        }

        public void Update(AdImageInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Update<AdImageInfo>(entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Delete(new AdImageInfo { Id = id });
            }
        }

        public void DeleteByAdType(int adType)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from [AdImage] WHERE AdType=@adType";
                conn.Execute(sql, new { adType = adType });
            }
        }

        public AdImageInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                return conn.Get<AdImageInfo>(id) ?? new AdImageInfo();
            }
        }

        public List<AdImageInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                return conn.GetAll<AdImageInfo>().OrderBy(k => k.OrderId).ToList();
            }
        }

        public List<AdImageInfo> ReadList(int adType)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from AdImage where AdType=@adType order by OrderId desc,Id desc";

                return conn.Query<AdImageInfo>(sql, new { adType = adType }).ToList();
            }
        }
    }
}