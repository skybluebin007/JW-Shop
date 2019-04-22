using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JWShop.Entity;
namespace JWShop.IDAL
{
    /// <summary>
    /// 返佣记录 接口层
    /// </summary>
   public interface IRebate
    {
        int Add(RebateInfo entity);
        //void Update(RebateInfo entity);
        //void Delete(int id);
        RebateInfo Read(int id);
        /// <summary>
        /// 统计分销商总佣金
        /// </summary>       
        decimal GetSumCommission(int distributor_Id);
        List<RebateInfo> SearchList(RebateSearchInfo searchModel);
        List<RebateInfo> SearchList(int currentPage,int pageSize, RebateSearchInfo searchModel,ref int count);

    }
}
