using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.MssqlDAL
{
    /// <summary>
    /// 读取数据列表类
    /// </summary>
    public class ShopMssqlPagerClass : MssqlPagerClass
    {
        private int count = 0;
        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int Count
        {
            get
            {
                int result = 0;
                if (this.count != int.MinValue)
                {
                    object count = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "ReadCount", this.PrepareCountParameter());
                    if (count != null && count != DBNull.Value)
                    {
                        if (count.ToString() != "0")
                        {
                            result = Convert.ToInt32(count);
                        }
                    }
                }
                return result;
            }
            set
            {
                this.count = value;
            }
        }

        /// <summary>
        /// 返回DataReader对像
        /// </summary>
        /// <returns></returns>
        public override SqlDataReader ExecuteReader()
        {
            return ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadPageList", this.PrepareParameter());

        }
        /// <summary>
        /// 返回DataTable对像
        /// </summary>
        /// <returns></returns>
        public override DataTable ExecuteDataTable()
        {
            return ShopMssqlHelper.ExecuteDataTable(ShopMssqlHelper.TablePrefix + "ReadPageList", this.PrepareParameter());
        }
    }
}