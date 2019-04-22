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
    /// 用户留言数据层说明。
    /// </summary>
    public sealed class UserMessageDAL : IUserMessage
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(UserMessageInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO UsrMessage( MessageClass,Title,Content,UserIP,PostDate,IsHandler,AdminReplyContent,AdminReplyDate,UserId,UserName,[Tel],[Email],[Gender],[Birthday],[Birthplace],[LivePlace],[Address],[Servedays],[Servemode],[AddCol1],[AddCol2]) VALUES(@MessageClass,@Title,@Content,@UserIP,@PostDate,@IsHandler,@AdminReplyContent,@AdminReplyDate,@UserId,@UserName,@Tel,@Email,@Gender,@Birthday,@Birthplace,@LivePlace,@Address,@Servedays,@Servemode,@AddCol1,@AddCol2);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(UserMessageInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE UsrMessage SET MessageClass = @MessageClass, Title = @Title, Content = @Content, UserIP = @UserIP, PostDate = @PostDate, IsHandler = @IsHandler, AdminReplyContent = @AdminReplyContent, AdminReplyDate = @AdminReplyDate, UserId = @UserId, UserName = @UserName
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int[] ids, int userId = 0)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from UsrMessage where id in @ids";

                var para = new DynamicParameters();
                para.Add("ids", ids);
                if (userId > 0)
                {
                    sql += " and UserId = @userId";
                    para.Add("userId", userId);
                }

                conn.Execute(sql, para);
            }
        }
        /// <summary>
        /// 按分类删除用户留言数据
        /// </summary>
        /// <param name="strUserID">分类ID,以,号分隔</param>
        public void DeleteUserMessageByUserID(int[] strUserID)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "DELETE FROM UsrMessage WHERE [UserID] IN @userIdS";

                var para = new DynamicParameters();
                para.Add("userIdS", strUserID);
              
                conn.Execute(sql, para);
            }
        }		 
        public UserMessageInfo Read(int id, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrMessage where Id=@id";

                var para = new DynamicParameters();
                para.Add("id", id);
                if (userId > 0)
                {
                    sql += " and UserId = @userId";
                    para.Add("userId", userId);
                }

                var data = conn.Query<UserMessageInfo>(sql, para).SingleOrDefault();
                return data ?? new UserMessageInfo();
            }
        }

        public List<UserMessageInfo> SearchList(UserMessageSeachInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from UsrMessage";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<UserMessageInfo>(sql).ToList();
            }
        }

        public List<UserMessageInfo> SearchList(int currentPage, int pageSize, UserMessageSeachInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                pc.TableName = "UsrMessage";
                pc.Fields = "[Id], [MessageClass], [Title], [Content], [UserIP], [PostDate], [IsHandler], [AdminReplyContent], [AdminReplyDate], [UserId], [UserName],[Tel],[Email],[Gender],[Birthday],[Birthplace],[LivePlace],[Address],[Servedays],[Servemode],[AddCol1],[AddCol2] ";
                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                pc.OrderField = "[Id]";
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                return conn.Query<UserMessageInfo>(pc).ToList();
            }
        }
        /// <summary>
        /// 搜索用户留言数据列表[~~~只显示允许展示案例的评论~~]
        /// </summary>
        /// <param name="currentPage">当前的页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="userMessageSearch">UserMessageSeachInfo模型变量</param>
        /// <param name="count">总数量</param>
        /// <returns>用户留言数据列表</returns>
        public List<UserMessageInfo> SearchUserMessageList1(int currentPage, int pageSize, UserMessageSeachInfo userMessageSearch, ref int count)
        {
            List<UserMessageInfo> userMessageList = new List<UserMessageInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "(select a.* from UsrMessage a inner join voteitem b on a.[userid]=b.id and b.[exp2]='1' union all select c.* from UsrMessage c where c.[userid]=0) as tmp";
            pc.Fields = "[Id], [MessageClass], [Title], [Content], [UserIP], [PostDate], [IsHandler], [AdminReplyContent], [AdminReplyDate], [UserId], [UserName],[Tel],[Email],[Gender],[Birthday],[Birthplace],[LivePlace],[Address],[Servedays],[Servemode],[AddCol1],[AddCol2]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.OrderField = "[Id]";
            pc.OrderType = OrderType.Desc;
            PrepareCondition(userMessageSearch);
            //pc.Count = count;
            count = pc.Count;
            using (SqlDataReader dr = pc.ExecuteReader())
            {
                while (dr.Read())
                {
                    UserMessageInfo userMessage = new UserMessageInfo();
                    userMessage.Id = dr.GetInt32(0);
                    userMessage.MessageClass = dr.GetInt32(1);
                    userMessage.Title = dr[2].ToString();
                    userMessage.Content = dr[3].ToString();
                    userMessage.UserIP = dr[4].ToString();
                    userMessage.PostDate = dr.GetDateTime(5);
                    userMessage.IsHandler = dr.GetInt32(6);
                    userMessage.AdminReplyContent = dr[7].ToString();
                    userMessage.AdminReplyDate = dr.GetDateTime(8);
                    userMessage.UserId = dr.GetInt32(9);
                    userMessage.UserName = dr[10].ToString();
                    userMessage.Email = dr[11].ToString();
                
                    userMessageList.Add(userMessage);
                }
                dr.Close();
            }
            return userMessageList;
        }
        /// <summary>
        /// 组合搜索条件
        /// </summary>
        /// <param name="mssqlCondition"></param>
        /// <param name="userMessageSearch">UserMessageSeachInfo模型变量</param>
        public MssqlCondition PrepareCondition(UserMessageSeachInfo userMessageSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[MessageClass]", userMessageSearch.MessageClass, ConditionType.Equal);
            mssqlCondition.Add("[MessageClass]", userMessageSearch.InMessageClass, ConditionType.In);

            mssqlCondition.Add("[Title]", userMessageSearch.Title, ConditionType.Like);
            mssqlCondition.Add("[PostDate]", userMessageSearch.StartPostDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[PostDate]", userMessageSearch.EndPostDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[UserName]", userMessageSearch.UserName, ConditionType.Like);
            mssqlCondition.Add("[UserId]", userMessageSearch.UserId, ConditionType.Equal);
            mssqlCondition.Add("[IsHandler]", userMessageSearch.IsHandler, ConditionType.Equal);

            return mssqlCondition;
        }

    }
}