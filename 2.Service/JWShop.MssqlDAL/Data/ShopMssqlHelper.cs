using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.MssqlDAL
{
    public sealed class ShopMssqlHelper
    {
        private static MssqlHelper mssqlHelper;
        private static string tablePrefix = string.Empty;
        /// <summary>
        /// 数据库前缀
        /// </summary>
        /// <returns></returns>
        public static string TablePrefix
        {
            get { return tablePrefix; }
            set { tablePrefix = value; }
        }
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ShopMssqlHelper()
        {
            mssqlHelper = new MssqlHelper();
            mssqlHelper.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            tablePrefix = ConfigurationManager.AppSettings["TablePrefix"];
        }

        /// <summary>
        /// 执行无返回记录
        /// </summary>
        /// <param name="storedProcName"></param>
        public static void ExecuteNonQuery(string storedProcName)
        {
            mssqlHelper.ExecuteNonQuery(storedProcName);
        }
        public static void ExecuteNonQuery(string storedProcName, SqlParameter[] pt)
        {
            mssqlHelper.ExecuteNonQuery(storedProcName, pt);
        }
        /// <summary>
        /// 返回SCALAR对像
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string storedProcName)
        {
            return mssqlHelper.ExecuteScalar(storedProcName);
        }
        public static object ExecuteScalar(string storedProcName, SqlParameter[] pt)
        {
            return mssqlHelper.ExecuteScalar(storedProcName, pt);
        }
        /// <summary>
        /// 返回DATAREADER对像
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string storedProcName)
        {
            return mssqlHelper.ExecuteReader(storedProcName);
        }
        public static SqlDataReader ExecuteReader(string storedProcName, SqlParameter[] pt)
        {
            return mssqlHelper.ExecuteReader(storedProcName, pt);
        }
        /// <summary>
        /// 返回DATATABLE对像
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string storedProcName)
        {
            return mssqlHelper.ExecuteDataTable(storedProcName);
        }
        public static DataTable ExecuteDataTable(string storedProcName, SqlParameter[] pt)
        {
            return mssqlHelper.ExecuteDataTable(storedProcName, pt);
        }
    }
}
