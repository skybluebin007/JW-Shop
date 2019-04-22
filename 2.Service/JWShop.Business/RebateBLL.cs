using JWShop.Common;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Collections.Generic;
using System.Linq;
using JWShop.Entity;
using System;

namespace JWShop.Business
{
    /// <summary>
    /// 返佣记录 业务层
    /// </summary>
    public sealed class RebateBLL : BaseBLL
    {
        private static readonly IRebate dal = FactoryHelper.Instance<IRebate>(Global.DataProvider, "RebateDAL");
        public static int Add(RebateInfo entity)
        {
            return dal.Add(entity);
        }

        public static RebateInfo Read(int id)
        {
            return dal.Read(id);
        }
        public static List<RebateInfo> SearchList(RebateSearchInfo searchModel)
        {
            return dal.SearchList(searchModel);
        }
        public static List<RebateInfo> SearchList(int currentPage, int pageSize, RebateSearchInfo searchModel, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchModel, ref count);
        }
        /// <summary>
        /// 获取 分销商 返佣记录 
        /// </summary>
        /// <param name="distributor_Id">分销商Id</param>
        /// <returns></returns>
        public static List<RebateInfo> ReadListByDistributor(int distributor_Id)
        {
            return SearchList(new RebateSearchInfo { Distributor_Id = distributor_Id });
        }

        /// <summary>
        /// 统计分销商总佣金
        /// </summary>       
        public static  decimal GetSumCommission(int distributor_Id)
        {
            return dal.GetSumCommission(distributor_Id);
        }
        /// <summary>
        /// 用户确认收货 给 2级分销商返佣
        /// 分别获取1级、2级分销商
        /// 判断分销商状态及所属分销商等级
        /// 根据实付款和分销商返佣比例进行返佣
        /// </summary>
        /// <param name="buy_user"></param>
        /// <param name="paid_money"></param>
        public static void RebateToDistributor(UserInfo buy_user, decimal paid_money,int order_id)
        {
            //购买者有推荐人 且 实际支付金额大于0才进行返佣
            if (buy_user.Recommend_UserId > 0 && paid_money > 0)
            {
                //获取购买者的底层分销商,底层分销商的上级分销商
                //判断分销商状态是否正常，再判断分销商等级
                //根据等级进行该等级比例的返佣
                //分销商等级
                List<DistributorGradeInfo> distributor_grade_list = DistributorGradeBLL.ReadList();

                #region 1级分销商返佣
                //1级分销商
                UserInfo distributor_01 = UserBLL.Read(buy_user.Recommend_UserId);
                if (distributor_01.Id > 0)
                {
                    Receive_Shipping_Rebate(distributor_01, buy_user, paid_money, order_id, distributor_grade_list,1);
                }
                #endregion
                #region 2级分销商返佣
                if (distributor_01.Recommend_UserId > 0)
                {
                    //2级分销商
                    UserInfo distributor_02 = UserBLL.Read(distributor_01.Recommend_UserId);
                    if (distributor_02.Id > 0)
                    {
                        Receive_Shipping_Rebate(distributor_02, buy_user, paid_money, order_id, distributor_grade_list,2);
                    }
                }
                #endregion
            }
     
        }
        /// <summary>
        /// 返佣操作
        /// </summary>
        /// <param name="distributor"></param>
        /// <param name="paid_money"></param>
        /// <param name="distributor_grade_list"></param>
        private static void Receive_Shipping_Rebate(UserInfo distributor,UserInfo buy_user, decimal paid_money, int order_id, List<DistributorGradeInfo> distributor_grade_list, int distributor_level=1)
        {
            if (distributor.Distributor_Status == (int)Distributor_Status.Normal && paid_money > 0)
            {
                decimal first_level_percent = ShopConfig.ReadConfigInfo().FirstLevelDistributorRebatePercent / 100.00M;
                decimal second_level_percent = ShopConfig.ReadConfigInfo().SecondLevelDistributorRebatePercent / 100.00M;
                decimal level_rebate = 0;
                //先根据分销商级别返佣
                if(distributor_level==1) level_rebate= paid_money * first_level_percent;
                if (distributor_level == 2) level_rebate = paid_money * second_level_percent;
                //分销商等级判断,另算等级返佣        
                var dis_grade = distributor_grade_list.Where(d => distributor.Total_Commission >= d.Min_Amount && distributor.Total_Commission < d.Max_Amount).FirstOrDefault() ?? new DistributorGradeInfo();
                decimal percent = dis_grade.Discount / 100.00M;
                decimal commission_amout = level_rebate * percent;
                //返佣
                Add(new RebateInfo
                {
                    Distributor_Id = distributor.Id,
                    User_Id= buy_user.Id,
                    Order_Id=order_id,
                    Commission = commission_amout+level_rebate,
                    Time = DateTime.Now
                });
            }
        }
    }
}
