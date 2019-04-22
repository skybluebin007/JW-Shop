using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 读取数据列表类，依赖于Dapper
    /// </summary>
    public static class ShopMssqlExtensions
    {
        public static IEnumerable<T> Query<T>(this IDbConnection cnn, ShopMssqlPagerClass pc)
        {
            return cnn.Query<T>(ShopMssqlHelper.TablePrefix + "ReadPageList", new
            {
                tableName = pc.TableName,
                fields = pc.Fields,
                pageSize = pc.PageSize,
                currentPage = pc.CurrentPage,
                fieldName = pc.OrderField,
                orderType = pc.OrderType,
                condition = pc.MssqlCondition.ToString()
            }, commandType: CommandType.StoredProcedure);
        }

    }
}