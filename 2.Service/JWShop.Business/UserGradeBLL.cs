using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;

namespace JWShop.Business
{
    public sealed class UserGradeBLL : BaseBLL
    {
        private static readonly IUserGrade dal = FactoryHelper.Instance<IUserGrade>(Global.DataProvider, "UserGradeDAL");

        public static int Add(UserGradeInfo entity)
        {
            return dal.Add(entity);
        }

        public static void Update(UserGradeInfo entity)
        {
            dal.Update(entity);
        }

        public static void Delete(int[] ids)
        {
            dal.Delete(ids);
        }

        public static UserGradeInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<UserGradeInfo> ReadList()
        {
            return dal.ReadList();
        }

        /// <summary>
        /// 根据消费金额读取一条用户等级数据
        /// </summary>
        /// <param name="id">消费金额</param>
        /// <returns>用户等级数据模型</returns>
        public static UserGradeInfo ReadByMoney(decimal money)
        {
            UserGradeInfo userGrade = new UserGradeInfo();
            List<UserGradeInfo> userGradeList = ReadList();
            foreach (UserGradeInfo temp in userGradeList)
            {
                if (money >= temp.MinMoney && money < temp.MaxMoney)
                {
                    userGrade = temp;
                    break;
                }
            }
            return userGrade;
        }	
    }
}