using System;
using JWShop.Entity;
using System.Collections.Generic;

namespace JWShop.IDAL
{
    /// <summary>
    /// 分销商等级 接口层
    /// </summary>
    public interface IDistributorGrade
    {
        /// <summary>
        /// 添加
        /// </summary>     
        int Add(DistributorGradeInfo entity);
        /// <summary>
        /// 编辑
        /// </summary>
        void Update(DistributorGradeInfo entity);

        DistributorGradeInfo Read(int id);

        List<DistributorGradeInfo> ReadList();

        void Delete(int id);
        void Delete(int[] ids);
    }
}
