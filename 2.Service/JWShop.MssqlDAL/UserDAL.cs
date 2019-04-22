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
    public sealed class UserDAL : IUser
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(UserInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO Usr( UserName,UserPassword,Email,Sex,Introduce,Photo,MSN,QQ,Tel,Mobile,RegionId,Address,Birthday,RegisterIP,RegisterDate,LastLoginIP,LastLoginDate,LoginTimes,SafeCode,FindDate,Status,OpenId,UserType,CardNo,CardPwd,ProviderNo,ProviderName,ProviderAddress,ProviderBankNo,ProviderTaxRegistration,ProviderCorporate,ProviderLinkerTel,ProviderFax,ProviderLinker,ProviderOperateBrand,ProviderOperateClass,ProviderAccount,ProviderAccountCycle,ProviderShipping,ProviderService,ProviderEnsure,[RealName],[HasRegisterCoupon],[HasBirthdayCoupon],[GetBirthdayCouponDate],[Recommend_UserId],[Distributor_Status],[Total_Commission],[Total_Withdraw],[Total_Subordinate]) VALUES(@UserName,@UserPassword,@Email,@Sex,@Introduce,@Photo,@MSN,@QQ,@Tel,@Mobile,@RegionId,@Address,@Birthday,@RegisterIP,@RegisterDate,@LastLoginIP,@LastLoginDate,@LoginTimes,@SafeCode,@FindDate,@Status,@OpenId,@UserType,@CardNo,@CardPwd,@ProviderNo,@ProviderName,@ProviderAddress,@ProviderBankNo,@ProviderTaxRegistration,@ProviderCorporate,@ProviderLinkerTel,@ProviderFax,@ProviderLinker,@ProviderOperateBrand,@ProviderOperateClass,@ProviderAccount,@ProviderAccountCycle,@ProviderShipping,@ProviderService,@ProviderEnsure,@RealName,@HasRegisterCoupon,@HasBirthdayCoupon,@GetBirthdayCouponDate,@Recommend_UserId,@Distributor_Status,@Total_Commission,@Total_Withdraw,0);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(UserInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE Usr SET UserName = @UserName, UserPassword = @UserPassword, Email = @Email, Sex = @Sex, Introduce = @Introduce, Photo = @Photo, MSN = @MSN, QQ = @QQ, Tel = @Tel, Mobile = @Mobile, RegionId = @RegionId, Address = @Address, Birthday = @Birthday, RegisterIP = @RegisterIP, RegisterDate = @RegisterDate, LastLoginIP = @LastLoginIP, LastLoginDate = @LastLoginDate, LoginTimes = @LoginTimes, SafeCode = @SafeCode, FindDate = @FindDate, Status = @Status, OpenId = @OpenId, UserType = @UserType, CardNo = @CardNo, CardPwd = @CardPwd, ProviderNo = @ProviderNo, ProviderName = @ProviderName, ProviderAddress = @ProviderAddress, ProviderBankNo = @ProviderBankNo, ProviderTaxRegistration = @ProviderTaxRegistration, ProviderCorporate = @ProviderCorporate, ProviderLinkerTel = @ProviderLinkerTel, ProviderFax = @ProviderFax, ProviderLinker = @ProviderLinker, ProviderOperateBrand = @ProviderOperateBrand, ProviderOperateClass = @ProviderOperateClass, ProviderAccount = @ProviderAccount, ProviderAccountCycle = @ProviderAccountCycle, ProviderShipping = @ProviderShipping, ProviderService = @ProviderService, ProviderEnsure = @ProviderEnsure,[RealName]=@RealName 
                            where Id=@Id";                                                                                                          
                                                                                                                                                    
                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Usr where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public UserInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Usr where id=@id";

                var data = conn.Query<UserInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new UserInfo();
            }
        }

        public UserInfo Read(string loginName)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Usr where @loginName in(UserName, Mobile,Email,OpenId)";

                var data = conn.Query<UserInfo>(sql, new { loginName = loginName }).SingleOrDefault();
                return data ?? new UserInfo();
            }
        }

        //用户名、手机、邮箱登录
        public UserInfo Read(string loginName, string password)
        {
            using (var conn = new SqlConnection(connectString))
            {
                //会员登陆方式 1).用户名  2).手机号码 3).Email
                string sql = "select * from Usr where @loginName in(UserName, Mobile,Email) and [UserPassword]=@password";

                var data = conn.Query<UserInfo>(sql, new { loginName = loginName, password = password}).SingleOrDefault();
                return data ?? new UserInfo();
            }
        }

        public List<UserInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Usr";

                return conn.Query<UserInfo>(sql).ToList();
            }
        }

        public List<UserInfo> ReadList(UserType userType)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Usr where userType = @userType";

                return conn.Query<UserInfo>(sql, new { userType = (int)userType }).ToList();
            }
        }

        public List<UserInfo> SearchList(UserSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from [Usr]";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition ;
                }

                sql += " Order by Id desc";
                return conn.Query<UserInfo>(sql).ToList();
            }
        }
        public  List<UserInfo> SearchList(int currentPage, int pageSize, UserSearchInfo searchModel, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass()
                {
                    TableName = "[Usr]",
                    Fields = "*",
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    MssqlCondition = PrepareCondition(searchModel),
                    OrderField = "[Id]",
                    OrderType=OrderType.Desc                    
                };
                count = pc.Count;
                var data = conn.Query<UserInfo>(pc).ToList();
              
                return data;
            }
        }
        /// <summary>
        /// 获取userlist 包括usergrade
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public List<UserInfo> SearchListAndUserGrade(UserSearchInfo searchInfo)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from View_UserGrade";

                string condition = PrepareCondition(searchInfo).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                sql += " Order by Id desc";
                return conn.Query<UserInfo>(sql).ToList();
            }
        }
        /// <summary>
        /// 获取userlist 包括usergrade
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public List<UserInfo> SearchListAndUserGrade(int currentPage, int pageSize, UserSearchInfo searchModel, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            { 
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass()
                {
                    TableName = "[View_UserGrade]",
                    Fields = "*",
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    MssqlCondition = PrepareCondition(searchModel),
                    OrderField = "[Id]",
                    OrderType = OrderType.Desc
                };
                count = pc.Count;
                var data = conn.Query<UserInfo>(pc).ToList();
               
                return data;
            }
        }
        

        public bool UniqueUser(string loginName, int usrId = 0)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select COUNT(1) from Usr where @loginName in(UserName, Mobile,Email) and Id<>@usrId";

                return conn.Query<int>(sql, new { loginName = loginName, usrId = usrId }).Single() < 1;
            }
        }
        // 根据userNames[]集合检查是否有重复 true:无重复 false:有重复
        public bool CheckUserNames(string[] userNames)
        {
            bool result = true;

            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from Usr where [UserName] in @UserNames";

                result = conn.ExecuteScalar<int>(sql, new { UserNames = userNames }) < 1;

            }
            return result;
        }
        // 根据mobiles[]集合检查是否有重复 true:无重复 false:有重复
        public bool CheckMobiles(string[] mobiles)
        {
            bool result = true;

            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from Usr where [Mobile] in @Mobiles";

                result = conn.ExecuteScalar<int>(sql, new { Mobiles = mobiles }) < 1;

            }
            return result;
        }
        // 根据emails[]集合检查是否有重复 true:无重复 false:有重复
        public bool CheckEmails(string[] emails)
        {
            bool result = true;

            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from Usr where [Email] in @Emails";

                result = conn.ExecuteScalar<int>(sql, new { Emails = emails }) < 1;

            }
            return result;
        }
        public void ChangePassword(int id, string oldPassword, string newPassword)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "update Usr set [UserPassword]=@newPassword where id=@id and [UserPassword]=@oldPassword";

                conn.Execute(sql, new { id = id, oldPassword = oldPassword, newPassword = newPassword });
            }
        }

        public void ChangePassword(int id, string newPassword)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "update Usr set [UserPassword]=@newPassword where id=@id";

                conn.Execute(sql, new { id = id, newPassword = newPassword });
            }
        }

        public int CountProvider()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select isnull(MAX(CAST(ProviderNo as int)),0) from Usr where userType=@userType";

                return conn.ExecuteScalar<int>(sql, new { userType = (int)UserType.Provider });
            }
        }
        //批量添加
        public void AddBatch(List<object[]> entities)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO [usr]([UserName],[Mobile],[Email],[Tel],[QQ],[UserPassword],RegisterIP,RegisterDate,LastLoginIP,LastLoginDate,FindDate,Sex,Status) VALUES(@UserName,@Mobile,@Email,@Tel,@QQ,@Password,@RegisterIP,@RegisterDate,@LastLoginIP,@LastLoginDate,@FindDate,@Sex,@Status)";


                List<object> paras = new List<object>();
                foreach (var entity in entities)
                {
                    UserInfo user = (UserInfo)entity[0];
                    paras.Add(new
                    {
                        UserName = user.UserName,
                        Mobile = user.Mobile,
                        Email = user.Email,
                        Tel = user.Tel,
                        QQ = user.QQ,
                        Password = user.UserPassword,
                        RegisterIP = user.RegisterIP,
                        RegisterDate = user.RegisterDate,
                        LastLoginIP = user.LastLoginIP,
                        LastLoginDate = user.LastLoginDate,
                        FindDate = user.FindDate,
                        Sex = user.Sex,
                        Status = user.Status

                    });
                }
                conn.Execute(sql, paras.ToArray());
            }
        }
        #region 统计
        public DataTable UserIndexStatistics(int userId)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@userId",SqlDbType.Int)
			};
            parameters[0].Value = "UserIndexStatistics";
            parameters[1].Value = userId;
            return ShopMssqlHelper.ExecuteDataTable("usp_Usr", parameters);
        }
        public DataTable ShopIndexStatistics(int shopId)
        {
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@userId",SqlDbType.Int)
			};
            parameters[0].Value = "ShopIndexStatistics";
            parameters[1].Value = shopId;
            return ShopMssqlHelper.ExecuteDataTable("usp_Usr", parameters);
        }
        /// <summary>
        /// 用户活跃度分析
        /// </summary>
        public DataTable StatisticsUserActive(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField)
        {
            List<UserInfo> userList = new List<UserInfo>();
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "View_UserActive";
            pc.Fields = "[Id],[UserName],[Sex],[RegisterDate],[LoginTimes],[CommentCount]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            switch (orderField)
            {
                case "LoginTimes":
                    pc.OrderField = "[LoginTimes],[Id]";
                    break;
                case "CommentCount":
                    pc.OrderField = "[CommentCount],[Id]";
                    break;
                case "ReplyCount":
                    pc.OrderField = "[ReplyCount],[Id]";
                    break;
                case "MessageCount":
                    pc.OrderField = "[MessageCount],[Id]";
                    break;
                default:
                    pc.OrderField = "[LoginTimes],[Id]";
                    break;
            }
            pc.OrderType = OrderType.Desc;
            pc.MssqlCondition = PrepareCondition(userSearch);
            pc.Count = count;
            count = pc.Count;
            return pc.ExecuteDataTable();
        }
        /// <summary>
        /// 用户消费分析
        /// </summary>
        public DataTable StatisticsUserConsume(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField, DateTime startDate, DateTime endDate)
        {
            List<UserInfo> userList = new List<UserInfo>();
            string orderCondition = string.Empty;
            MssqlCondition orderCh = new MssqlCondition();
            orderCh.Add("[AddDate]", startDate, ConditionType.MoreOrEqual);
            orderCh.Add("[AddDate]", endDate, ConditionType.Less);
            orderCondition = orderCh.ToString();
            if (!string.IsNullOrEmpty(orderCondition))
            {
                orderCondition = " AND " + orderCondition;
            }
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "(SELECT Id,UserName,Sex,ISNULL(OrderCount,0) AS OrderCount,ISNULL(OrderMoney,0) AS OrderMoney ";
            pc.TableName += "FROM Usr ";
            pc.TableName += "LEFT OUTER JOIN (SELECT UserId, COUNT(*) AS OrderCount,Sum(ProductMoney-FavorableMoney+ShippingMoney+OtherMoney-CouponMoney) AS OrderMoney FROM [Order] WHERE OrderStatus=6 " + orderCondition + " GROUP BY UserId) AS TEMP3 ON Usr.Id = TEMP3.UserId where Usr.status = 2) AS PageTable";
            pc.Fields = "[Id],[UserName],[Sex],[OrderCount],[OrderMoney]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            switch (orderField)
            {
                case "OrderCount":
                    pc.OrderField = "[OrderCount],[Id]";
                    break;
                case "OrderMoney":
                    pc.OrderField = "[OrderMoney],[Id]";
                    break;
                default:
                    pc.OrderField = "[OrderCount],[Id]";
                    break;
            }
            pc.OrderType = OrderType.Desc;
            pc.MssqlCondition = PrepareCondition(userSearch);
            pc.Count = count;
            count = pc.Count;
            return pc.ExecuteDataTable();
        }
        /// <summary>
        /// 统计用户状态
        /// </summary>
        public DataTable StatisticsUserStatus(UserSearchInfo userSearch)
        {
            string condition = PrepareCondition(userSearch).ToString();
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@condition",SqlDbType.NVarChar)
			};
            parameters[0].Value = "StatisticsUserStatus";
            parameters[1].Value = condition;
            return ShopMssqlHelper.ExecuteDataTable("usp_Usr", parameters);
        }
        /// <summary>
        /// 统计用户数量
        /// </summary>
        public DataTable StatisticsUserCount(UserSearchInfo userSearch, DateType dateType)
        {
            string condition = PrepareCondition(userSearch).ToString();
            SqlParameter[] parameters ={
				new SqlParameter("@type",SqlDbType.VarChar),
				new SqlParameter("@condition",SqlDbType.NVarChar),
                new SqlParameter("@dateType",SqlDbType.Int)
			};
            parameters[0].Value = "StatisticsUserCount";
            parameters[1].Value = condition;
            parameters[2].Value = (int)dateType;
            return ShopMssqlHelper.ExecuteDataTable("usp_Usr", parameters);
        }
        #endregion

        private MssqlCondition PrepareCondition(UserSearchInfo userSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            mssqlCondition.Add("[UserName]", userSearch.UserName, ConditionType.Like);
            mssqlCondition.Add("[Mobile]", userSearch.Mobile, ConditionType.Like);
            mssqlCondition.Add("[Email]", userSearch.Email, ConditionType.Like);
            mssqlCondition.Add("[Sex]", userSearch.Sex, ConditionType.Equal);
            mssqlCondition.Add("[RegisterDate]", userSearch.StartRegisterDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[RegisterDate]", userSearch.EndRegisterDate, ConditionType.Less);
            mssqlCondition.Add("[Status]", userSearch.Status, ConditionType.Equal);
            mssqlCondition.Add("[Id]", userSearch.InUserId, ConditionType.In);
            mssqlCondition.Add("[UserType]", userSearch.UserType, ConditionType.Equal);
            mssqlCondition.Add("[CardNo]", userSearch.CardNo, ConditionType.Like);
            mssqlCondition.Add("[ProviderNo]", userSearch.ProviderNo, ConditionType.Like);
            mssqlCondition.Add("[Distributor_Status]", userSearch.Distributor_Status, ConditionType.Equal);         
            return mssqlCondition;
        }

        /// <summary>
        /// 读取一条用户数据(包含用户的统计信息)
        /// </summary>
        /// <param name="id">用户的主键值</param>
        /// <returns>用户数据模型</returns>
        public UserInfo ReadUserMore(int userID)
        {
            using (var conn = new SqlConnection(connectString))
            {
                return conn.Query<UserInfo>(ShopMssqlHelper.TablePrefix + "ReadUserMore", new
                {
                    id = userID.ToString()
                }, null, true, null, CommandType.StoredProcedure).SingleOrDefault() ?? new UserInfo();
            }
        }

        /// <summary>
        /// 检查用户名是否被占用
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public int CheckUserName(string userName)
        {
            int userID = 0;
            SqlParameter[] parameters ={
				new SqlParameter("@userName",SqlDbType.NVarChar)
			};
            parameters[0].Value = userName;
            object obj = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "CheckUserName", parameters);
            if (obj != null && obj != DBNull.Value)
            {
                userID = Convert.ToInt32(obj);
            }
            return userID;
        }

        /// <summary>
        /// 检查E-mail是否被占用
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>真/假</returns>
        public bool CheckEmail(string email)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT Count(ID) FROM  Usr WHERE [Email] =@email";

                return conn.Query<int>(sql, new { email = email }).Single() < 1;
            }
        }
        /// <summary>
        /// 检查E-mail是否被占用
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>真/假</returns>
        public bool CheckEmail(string email, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT Count(ID) FROM  Usr WHERE [Email] =@email and [Id]<>@Id";

                return conn.Query<int>(sql, new { email = email, Id = userId }).Single() < 1;
            }
        }
        /// <summary>
        /// 检查Mobile是否被占用
        /// </summary>
        /// <param name="email">email</param>
        ///  /// <param name="userId">userId</param>
        /// <returns>真/假</returns>
        public bool CheckMobile(string mobile, int userId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "SELECT Count(ID) FROM  Usr WHERE [Mobile] =@mobile";
                if (userId > 0)
                {
                    sql = "SELECT Count(ID) FROM  Usr WHERE [Mobile] =@mobile and [Id]<>@Id";
                }
                else
                {
                    sql = "SELECT Count(ID) FROM  Usr WHERE [Mobile] =@mobile";
                }
                return conn.Query<int>(sql, new { mobile = mobile, Id = userId }).Single() < 1;
            }
        }
        /// <summary>
        /// 更改用户的的安全码（找回密码使用）
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="safeCode"></param>
        /// <param name="findDate"></param>
        public void ChangeUserSafeCode(int userID, string safeCode, DateTime findDate)
        {
            UserInfo tempUser = Read(userID);
            tempUser.SafeCode = safeCode;
            tempUser.FindDate = findDate;
            Update(tempUser);
        }
        /// <summary>
        /// 用户归属分销商
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="distributorId">分销商Id</param>
        public void ChangeUserToDistributor(int userId,int distributorId)
        {
          using(var conn=new SqlConnection(connectString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        //更新user：recommend_userid
                        string sql = "UPDATE [USR] SET [Recommend_UserId]=@Recommend_UserId WHERE [Id]=@Id";
                        int rows = conn.Execute(sql, new { @Recommend_UserId = distributorId, @Id = userId }, tran);
                        if (rows > 0)
                        {
                            //更新recommend_userid分销商的下级数：+1
                            sql = "UPDATE [USR] SET [Total_Subordinate]=[Total_Subordinate]+1 WHERE [Id]=@Id";
                            rows = conn.Execute(sql, new { @Id = distributorId }, tran);
                            if (rows > 0)
                            {
                                //更新recommend_userid分销商的上级分销商的下级数：+1
                                sql = "SELECT * FROM [USR] WHERE [Id]=@Id";
                                var user = conn.Query<UserInfo>(sql, new { @Id = distributorId }, tran).FirstOrDefault() ?? new UserInfo();
                                if (user.Id > 0 && user.Recommend_UserId > 0)
                                {
                                    sql = "UPDATE [USR] SET [Total_Subordinate]=[Total_Subordinate]+1 WHERE [Id]=@Id";
                                    conn.Execute(sql, new { @Id = user.Recommend_UserId }, tran);
                                }

                                //如果分销商没有上级，则只给此分销商的下级+1
                                tran.Commit();

                            }
                            else
                            {
                                tran.Rollback();
                            }
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        //将user的登录次数置为1，恢复初始注册状态
                        string sql = "UPDATE [USR] SET [LoginTimes]=1 WHERE [Id]=@Id";
                        conn.Execute(sql, new { @Id = userId });
                    }
                }
            }
        }
    }
}