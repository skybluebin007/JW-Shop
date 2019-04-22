using System.Data.SqlClient;
using System.Collections.Generic;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;
using Dapper;
using System.Linq;

namespace JWShop.MssqlDAL
{

    /// <summary>
    /// 砍价详情数据层说明。
    /// </summary>
    public sealed class BargainDetailsDAL:IBargainDetails
	{
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// 增加一条砍价详情数据
        /// </summary>
        /// <param name="model">砍价详情模型变量</param>
        public int AddBargainDetails(BargainDetailsInfo model)
		{
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO BargainDetails([BargainId],[ProductID],[Stock],[Sort],[ReservePrice],[Sales],[ProductName],[ShareImage1],[ShareImage2],[ShareImage3]) VALUES(@bargainId,@productID,@stock,@sort,@reservePrice,@Sales,@ProductName,@ShareImage1,@ShareImage2,@ShareImage3)
	SELECT @@identity";
                return conn.Query<int>(sql,model).Single();
            }
		}
		
		/// <summary>
		/// 更新一条砍价详情数据
		/// </summary>
		/// <param name="model">砍价详情模型变量</param>
		public void UpdateBargainDetails(BargainDetailsInfo model)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE BargainDetails Set [BargainId]=@bargainId,[ProductID]=@productID,[Stock]=@stock,[Sort]=@sort,[ReservePrice]=@reservePrice,[Sales]=@Sales,[ProductName]=@ProductName,[ShareImage1]=@ShareImage1,[ShareImage2]=@ShareImage2,[ShareImage3]=@ShareImage3  WHERE [Id]=@id";
                conn.Execute(sql, model);
            }
        }

		/// <summary>
		/// 删除多条砍价详情数据
		/// </summary>
		/// <param name="strId">砍价详情的主键值,以,号分隔</param>
		public void DeleteBargainDetails(string strId)
		{
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE FROM BargainDetails WHERE ID in (@ids)";
                conn.Execute(sql,new { ids=strId});
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id) {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"DELETE FROM BargainDetails WHERE ID=@id";
                conn.Execute(sql, new { id=id });
            }
        }


        /// <summary>
        /// 读取活动下面的所有商品详情
        /// </summary>
        /// <param name="BargainId"></param>
        /// <returns></returns>
        public List<BargainDetailsInfo> ReadByBargainId(int BargainId) {
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [BargainDetails] WHERE BargainId=@BargainId";

                return conn.Query<BargainDetailsInfo>(sql,new { BargainId= BargainId }).ToList();
            }
        }


        public List<BargainDetailsInfo> ReadList()
        {
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [BargainDetails]";

                return conn.Query<BargainDetailsInfo>(sql).ToList();
            }
        }



        /// <summary>
        /// 读取一条砍价详情数据
        /// </summary>
        /// <param name="id">砍价详情的主键值</param>
        /// <returns>砍价详情数据模型</returns>
        public BargainDetailsInfo ReadBargainDetails(int id)
		{
            using (var conn=new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM BargainDetails WHERE [Id]=@id";

                return conn.Query<BargainDetailsInfo>(sql,new {id=id }).SingleOrDefault()??new BargainDetailsInfo();
            }
		}

		/// <summary>
		/// 准备砍价详情模型
		/// </summary>
		/// <param name="dr">Datareader</param>
		/// <param name="modelList">砍价详情的数据列表</param>
		public void PrepareBargainDetailsModel(SqlDataReader dr,List<BargainDetailsInfo> modelList)
		{
			while (dr.Read())
			{
				BargainDetailsInfo model= new BargainDetailsInfo();
				model.Id=dr.GetInt32(0);
				model.BargainId=dr.GetInt32(1);
				model.ProductID=dr.GetInt32(2);
				model.Stock=dr.GetInt32(3);
				model.Sort=dr.GetInt32(4);
				model.ReservePrice=dr.GetDecimal(5);
				modelList.Add(model);
			}
		} 

        /// <summary>
        /// 读取正在参加砍价的商品列表(Id,Name)
        /// </summary>
        /// <returns></returns>
        public List<BargainDetailsInfo> ReadBargainProducts()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"SELECT * FROM [View_Bargain_Products]";

                return conn.Query<BargainDetailsInfo>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取参与砍价的用户姓名 头像
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<BargainUserInfo> GetBargainUsers(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"select a.[Id], a.UserId,a.[BargainDetailsId], b.[BargainId], b.[ProductID],c.photo as [Avatar],c.[UserName] 
                                FROM [BargainOrder] a
                                LEFT JOIN [BargainDetails] b on a.[BargainDetailsId]=b.[Id]
                                LEFT JOIN [Usr] c on c.[Id]=a.[UserId]
                                WHERE a.[BargainDetailsId]=@BargainDetailsId  ORDER BY a.[Id] desc";

                return conn.Query<BargainUserInfo>(sql, new { @BargainDetailsId = id }).ToList();
            }
        }

    }
}