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
using System.Linq;
using System.Text;

namespace JWShop.MssqlDAL
{
    public sealed class BaseDAL : IBase
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int MaxOrderId(string table)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select isnull(max(OrderId),0)+1 from " + table;
                return conn.ExecuteScalar<int>(sql);
            }
        }

        public void UpdatePart(string table, Dictionary<string, object> dict, int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, object> item in dict)
                {
                    string keyValue = sb.Length == 0 ? "{0}='{1}'" : ",{0}='{1}'";
                    sb.Append(string.Format(keyValue, item.Key, item.Value));
                }
                string sql = "update " + table + " set " + sb.ToString() + " where Id=" + id;
                conn.Execute(sql);
            }
        }

    }
}