using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Business;
using JWShop.Entity;
using JWShop.Common;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Page.Admin
{
   public class FavorableActivity:AdminBase
    {
        //用户等级集合
        protected List<UserGradeInfo> userGrades = UserGradeBLL.ReadList();
        protected int pageSize =3;
        protected List<FavorableActivityInfo> activities = new List<FavorableActivityInfo>();
        //默认进行中
        protected int timePeriod = 2;
        protected int count = 0;
        protected override void PageLoad()
        {
            base.PageLoad();
            timePeriod = RequestHelper.GetQueryString<int>("timeperiod") < 1 ? 2 : RequestHelper.GetQueryString<int>("timeperiod");
            activities = FavorableActivityBLL.ReadList(1, pageSize, ref count, timePeriod);
        }
        /// <summary>
        /// 根据优惠活动选择的用户等级，获取对应的等级名称 
        /// </summary>
        /// <param name="userGrade"></param>
        /// <returns></returns>
        protected string GetUserGradeOfActivity(string userGrade="")
        {
            string result = string.Empty;
            List<string> gradeNames = new List<string>();
            if (!string.IsNullOrEmpty(userGrade))
            {
                string[] a = userGrade.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
               
                foreach(var b in a)
                {
                    UserGradeInfo gd = userGrades.Find(k => k.Id.ToString() == b) ?? new UserGradeInfo();
                    if (gd.Id > 0) gradeNames.Add(gd.Name);
                }
            }
            if (gradeNames.Count > 0)
            {
                result = string.Join(",", gradeNames);
            }
            return result;
        }
    }
}
