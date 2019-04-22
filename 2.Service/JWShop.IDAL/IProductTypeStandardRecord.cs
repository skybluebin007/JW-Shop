using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;

namespace JWShop.IDAL
{
    public interface IProductTypeStandardRecord
    {
        void Add(ProductTypeStandardRecordInfo entity);
        void Update(ProductTypeStandardRecordInfo entity);
        void UpdateSalePrice(ProductTypeStandardRecordInfo entity);
        void UpdateStorage(ProductTypeStandardRecordInfo entity);
        void Delete(int productId);
        void DeleteByStandardId(int standardId);
        ProductTypeStandardRecordInfo Read(int productId, string valueList);
        List<ProductTypeStandardRecordInfo> ReadList();
        List<ProductTypeStandardRecordInfo> ReadList(int productId);

        void ChangeOrderCount(int productId, string valueList, int buyCount, ChangeAction action);
        void ChangeSendCount(int productId, string valueList, int buyCount, ChangeAction action);

        /// <summary>
        /// 按产品ID获得规格记录所有数据
        /// </summary>
        /// <param name="productID">产品ID</param>
        /// <returns>规格记录数据列表</returns>
        List<ProductTypeStandardRecordInfo> ReadListByProduct(int productID, int standardType);
        /// <summary>
        /// 删除该产品下的规格记录数据
        /// </summary>
        /// <param name="strProductID">产品的主键值,以,号分隔</param>
        void DeleteByProductID(string strProductID); 
         /// <summary>
        /// 获取规格总库存
        /// </summary>
        /// <param name="entity"></param>
         int GetSumStorageByProduct(int productId);
    }
}