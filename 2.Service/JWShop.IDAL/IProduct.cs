using System;
using System.Collections;
using System.Collections.Generic;
using JWShop.Entity;
using System.Data;

namespace JWShop.IDAL
{
    public interface IProduct
    {
        int Add(ProductInfo entity);
        void Update(ProductInfo entity);
        /// <summary>
        /// 逻辑删除--isdelete 置为1
        /// </summary>
        /// <param name="id"></param>
        void DeleteLogically(int id);
        /// <summary>
        /// 逻辑删除后恢复--isdelete 置为0
        /// </summary>
        /// <param name="id"></param>
        void Recover(int id);
        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        ProductInfo Read(int id);
        List<ProductInfo> ReadList();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchInfo"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ProductInfo> SearchList(int currentPage, int pageSize, ProductSearchInfo searchInfo, ref int count);
        /// <summary>
        /// 根据条件搜索出所有产品
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        List<ProductInfo> SearchList(ProductSearchInfo searchInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="classId"></param>
        /// <param name="brandIds"></param>
        /// <param name="attributeIds"></param>
        /// <param name="attributeValues"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="keywords"></param>
        /// <param name="count"></param>
        /// <param name="showAttributeList"></param>
        /// <param name="showBrandList"></param>
        /// <returns></returns>
        List<ProductInfo> SearchList(int currentPage, int pageSize, int classId, string brandIds, string attributeIds, string attributeValues, string orderField, string orderType, int minPrice, int maxPrice, string keywords, string stf_Keyword, int isNew, int isHot, int isSpecial, int isTop, ref int count, ref List<ProductTypeAttributeInfo> showAttributeList, ref List<ProductBrandInfo> showBrandList);

        /// <summary>
        /// 手机端商品列表页搜索
        /// </summary>
        List<ProductInfo> SearchList(int currentPage, int pageSize, int classId, string brandIds, string orderField, string orderType, string keywords, ref int count);

        void OffSale(int[] ids);
        void OnSale(int[] ids);
        void ChangeOrderCount(int id, int changeCount);
        void ChangeSendCount(int id, int changeCount);
        void ChangeOrderCountByOrder(int orderId, ChangeAction action);
        void ChangeSendCountByOrder(int orderId, ChangeAction action);

        void ChangeProductCommentCountAndRank(int productId, int rank, ChangeAction action);
        void ChangeProductCollectCount(int productId, ChangeAction action);
        /// <summary>
        /// 改变产品的查看数量
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="changeCount">正数表示增加，负数减少</param>
        void ChangeViewCount(int id, int changeCount);

        int CountByShop(int shopId);
        int CountByClass(int classId);
        string MaxProductNumber(string classId, string prefixNumber);
        bool UniqueProductNumber(string productNumber, int productId = 0);

        /// <summary>
        /// 修改产品状态值
        /// </summary>
        /// <param name="statusType">状态名称</param>
        /// <param name="id"></param>
        /// <param name="status">状态值</param>
        void ChangeProductStatus(int id, int status, ProductStatusType statusType);
        /// <summary>
        /// 更改产品的规格属性
        /// </summary>
        /// <param name="strID">产品的主键值,以,号分隔</param>
        /// <param name="standardType">规格值</param>
        /// <param name="id">当前产品ID</param>
        void UpdateProductStandardType(string strID, int standardType, int id);

        /// <summary>
        /// 产品销量分析
        /// </summary>
        DataTable StatisticsProductSale(int currentPage, int pageSize, ProductSearchInfo productSearch, ref int count, DateTime startDate, DateTime endDate);
        /// <summary>
        /// 待处理事务统计
        /// </summary>
        DataTable NoHandlerStatistics();
        /// <summary>
        /// 批量更新产品数据
        /// </summary>
        /// <param name="productIDList">产品id串</param>
        /// <param name="product">产品模型变量</param>
        void UnionUpdateProduct(string productIDList, ProductInfo product);
    }
}