using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;

namespace JWShop.Common
{
    /// <summary>
    /// 砍价 
    /// </summary>
    public sealed class BargainHelper
    {
        #region 创建砍价时，自动分配每一刀金额，含 分享 金额 
 
      public  static List<decimal> ComputeBargainMoney(BargainOrderInfo _redPackage)
        {
            List<decimal> bargain_Moneys = new List<decimal>();
            for (int i = 1; i <= _redPackage.Total_Num; i++)
            {
                decimal bg_money = getRandomMoney(_redPackage);
                //Console.WriteLine("第" + i + "刀：" + bg_money);
                bargain_Moneys.Add(bg_money);
            }
            //Console.WriteLine("分享刀：" + bg.share_money);
            //Console.ReadKey();
            return bargain_Moneys;
        }
        static decimal getRandomMoney(BargainOrderInfo _redPackage)
        {
            Random r = new Random();

            //如果是第一刀，砍28~35%，还生成分享刀5~15%
            if (_redPackage.Has_Bargain_Num == 0)
            {

                decimal first_money = (decimal)(r.Next(28, 35) / 100.00) * _redPackage.Total_Money;
                first_money = Math.Floor(first_money * 100) / 100;
                _redPackage.Has_Bargain_Num++;
                _redPackage.Has_Bargain_Money += first_money;
                //分享刀
                decimal share_money = (decimal)(r.Next(5, 15) / 100.00) * (_redPackage.Total_Money - _redPackage.Has_Bargain_Money);
                share_money = Math.Floor(share_money * 100) / 100;
                _redPackage.SharePrice = share_money;
                _redPackage.Has_Bargain_Money += share_money;

                return first_money;
            }
            if (_redPackage.Total_Num - _redPackage.Has_Bargain_Num == 1)
            {//如果是最后一刀
                _redPackage.Has_Bargain_Num++;
                return Math.Round((_redPackage.Total_Money - _redPackage.Has_Bargain_Money) * 100) / 100;
            }
            //砍的最小值
            decimal min = 0.01M;
            //砍的最大值
            decimal max = (_redPackage.Total_Money - _redPackage.Has_Bargain_Money) / (_redPackage.Total_Num - _redPackage.Has_Bargain_Num) * 2;
            //随机砍价金额
            decimal money = (decimal)r.NextDouble() * max;
            money = money <= min ? 0.01M : money;
            money = Math.Floor(money * 100) / 100;
            _redPackage.Has_Bargain_Num++;
            _redPackage.Has_Bargain_Money += money;
            return money;
        }
        #endregion

        /// <summary>
        /// 砍价
        /// </summary>
        /// <param name="original">原价</param>
        /// <param name="ReservePrice">最低价</param>
        /// <param name="bargainPrice">已砍价金额</param>
        /// <param name="percentage">砍价成功率百分比</param>
        /// <returns></returns>
        public static decimal BargainPrice(decimal original,decimal bargainPrice,decimal ReservePrice, int percentage) {
            Random rd = new Random(Guid.NewGuid().GetHashCode());

            decimal SuccessRate= (decimal)percentage/100;
            //随机整数部分
            // int Integer = rd.Next(((int)original - (int)bargainPrice-(int)ReservePrice), (int)(original*SuccessRate));
            int Integer = rd.Next((int)((original - bargainPrice - ReservePrice)*SuccessRate));
            //随机小数部分
            int Decimal = rd.Next(100);
            decimal result = Integer + (decimal)Decimal/100;
            //if (percentage>0)
            //{
            //    result = result * ((decimal)percentage/100);
            //}
            return Math.Round(result,2);
        }
    }
}
