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
    /// <summary>
    /// 数据层说明。
    /// </summary>
    public sealed class OtherShiyaDAL:IOtherShiya
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(OtherShiyaInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO OtherShiya(saleid,shuigongid,shiyaid,customerid,orderId,truename,mobile,address,anname,anmobile,housetype,kaifashang,jiazhuang,kongtiao,guanjian,pushe,baohu,beizhu,zongfa,hunzhuang,huaheng,xiangqian,guolvqi,jietou,baoyastart,baoyaend,yunxing,jiance,shiyaname,shiyamobile,gaozhi,stat,addtime) VALUES(@saleid,@shuigongid,@shiyaid,@customerid,@orderId,@truename,@mobile,@address,@anname,@anmobile,@housetype,@kaifashang,@jiazhuang,@kongtiao,@guanjian,@pushe,@baohu,@beizhu,@zongfa,@hunzhuang,@huaheng,@xiangqian,@guolvqi,@jietou,@baoyastart,@baoyaend,@yunxing,@jiance,@shiyaname,@shiyamobile,@gaozhi,@stat,@addtime);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(OtherShiyaInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE OtherShiya SET saleid=@saleid,shuigongid=@shuigongid,shiyaid=@shiyaid,customerid=@customerid,orderId=@orderId,truename=@truename,mobile=@mobile,address=@address,anname=@anname,anmobile=@anmobile,housetype=@housetype,kaifashang=@kaifashang,jiazhuang=@jiazhuang,kongtiao=@kongtiao,guanjian=@guanjian,pushe=@pushe,baohu=@baohu,beizhu=@beizhu,zongfa=@zongfa,hunzhuang=@hunzhuang,huaheng=@huaheng,xiangqian=@xiangqian,guolvqi=@guolvqi,jietou=@jietou,baoyastart=@baoyastart,baoyaend=@baoyaend,yunxing=@yunxing,jiance=@jiance,shiyaname=@shiyaname,shiyamobile=@shiyamobile,gaozhi=@gaozhi,stat=@stat, addtime = @addtime
                            where id=@id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OtherShiya where id=" + id;

                var para = new DynamicParameters();
                para.Add("ids", id);

                conn.Execute(sql, para);
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from OtherShiya where id in @ids";

                var para = new DynamicParameters();
                para.Add("ids", ids);

                conn.Execute(sql, para);
            }
        }

        public OtherShiyaInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OtherShiya where id=@id";

                var para = new DynamicParameters();
                para.Add("id", id);

                var data = conn.Query<OtherShiyaInfo>(sql, para).SingleOrDefault();
                return data ?? new OtherShiyaInfo();
            }
        }


        public List<OtherShiyaInfo> ReadList(int userid,int usertype)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select top 1 * from OtherShiya ";
                var para = new DynamicParameters();

                if (usertype == 1)
                {
                    sql += " where shiyaid=@shiyaid";
                    para.Add("shiyaid", userid);
                }
                else if (usertype == 2)
                {
                    sql += " where shuigongid=@shuigongid";
                    para.Add("shuigongid", userid);
                }
                else if (usertype == 3)
                {
                    sql += " where saleid=@saleid";
                    para.Add("saleid", userid);
                }
                else
                {
                    sql += " where customerid=@customerid";
                    para.Add("customerid", userid);
                }

                return conn.Query<OtherShiyaInfo>(sql, para).ToList();
            }
        }

        public List<OtherShiyaInfo> SearchList(OtherShiyaSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from OtherShiya";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<OtherShiyaInfo>(sql).ToList();
            }
        }

        public List<OtherShiyaInfo> SearchList(int currentPage, int pageSize, OtherShiyaSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "OtherShiya";
                pc.Fields = "saleid,shuigongid,shiyaid,customerid,orderId,truename,mobile,address,anname,anmobile,housetype,kaifashang,jiazhuang,kongtiao,guanjian,pushe,baohu,beizhu,zongfa,hunzhuang,huaheng,xiangqian,guolvqi,jietou,baoyastart,baoyaend,yunxing,jiance,shiyaname,shiyamobile,gaozhi,stat,addtime ";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[id]";
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<OtherShiyaInfo>(pc).ToList();
            }
        }

        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <param name="mssqlCondition"></param>
        /// <param name="userMessageSearch">UserPifaSeachInfo模型变量</param>
        public MssqlCondition PrepareCondition(OtherShiyaSearchInfo SearchInfo)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[saleid]", SearchInfo.saleid, ConditionType.Equal);
            mssqlCondition.Add("[shuigongid]", SearchInfo.shuigongid, ConditionType.Equal);
            mssqlCondition.Add("[shiyaid]", SearchInfo.shiyaid, ConditionType.Equal);
            mssqlCondition.Add("[customerid]", SearchInfo.customerid, ConditionType.Equal);
            mssqlCondition.Add("[orderId]", SearchInfo.orderId, ConditionType.Equal);
            mssqlCondition.Add("[truename]", SearchInfo.truename, ConditionType.Like);
            mssqlCondition.Add("[mobile]", SearchInfo.mobile, ConditionType.Like);
            mssqlCondition.Add("[address]", SearchInfo.address, ConditionType.Like);
            mssqlCondition.Add("[anname]", SearchInfo.anname, ConditionType.Like);
            mssqlCondition.Add("[anmobile]", SearchInfo.anmobile, ConditionType.Like);
            mssqlCondition.Add("[shiyaname]", SearchInfo.shiyaname, ConditionType.Like);
            mssqlCondition.Add("[shiyamobile]", SearchInfo.shiyamobile, ConditionType.Like);
            mssqlCondition.Add("[stat]", SearchInfo.stat, ConditionType.Equal);
            mssqlCondition.Add("[addtime]", SearchInfo.addtime, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[addtime]", SearchInfo.addtime, ConditionType.Less);

            return mssqlCondition;
        }
    }
}
