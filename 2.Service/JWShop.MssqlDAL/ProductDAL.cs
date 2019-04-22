using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using System.Linq;

namespace JWShop.MssqlDAL
{
    public sealed class ProductDAL : IProduct
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ProductInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO Product( ShopId,Name,SubTitle,ClassId,Spelling,Color,FontStyle,BrandId,ProductNumber,ProductCode,MarketPrice,BidPrice,SalePrice,Weight,SendPoint,Photo,Keywords,Summary,Introduction1,Introduction1_Mobile,Introduction2,Introduction2_Mobile,Introduction3,Introduction3_Mobile,Remark,IsSpecial,IsNew,IsHot,IsSale,IsTop,Accessory,RelationProduct,RelationArticle,ViewCount,AllowComment,CommentCount,SumPoint,PerPoint,PhotoCount,CollectCount,TotalStorageCount,OrderCount,SendCount,ImportActualStorageCount,ImportVirtualStorageCount,LowerCount,UpperCount,AddDate,TaobaoId,OrderId,Unit,TaxRate,LikeNum,UnLikeNum,ProductionPlace,GrossRate,StandardType,[SellPoint],[IsDelete],[Qrcode],[UnlimitedStorage],[VirtualOrderCount],[UseVirtualOrder],[OpenGroup],[GroupPrice],[GroupQuantity],[GroupPhoto],[YejiRatio]) VALUES(@ShopId,@Name,@SubTitle,@ClassId,@Spelling,@Color,@FontStyle,@BrandId,@ProductNumber,@ProductCode,@MarketPrice,@BidPrice,@SalePrice,@Weight,@SendPoint,@Photo,@Keywords,@Summary,@Introduction1,@Introduction1_Mobile,@Introduction2,@Introduction2_Mobile,@Introduction3,@Introduction3_Mobile,@Remark,@IsSpecial,@IsNew,@IsHot,@IsSale,@IsTop,@Accessory,@RelationProduct,@RelationArticle,@ViewCount,@AllowComment,@CommentCount,@SumPoint,@PerPoint,@PhotoCount,@CollectCount,@TotalStorageCount,@OrderCount,@SendCount,@ImportActualStorageCount,@ImportVirtualStorageCount,@LowerCount,@UpperCount,@AddDate,@TaobaoId,@OrderId,@Unit,@TaxRate,@LikeNum,@UnLikeNum,@ProductionPlace,@GrossRate,@StandardType,@SellPoint,@IsDelete,@Qrcode,@UnlimitedStorage,@VirtualOrderCount,@UseVirtualOrder,@OpenGroup,@GroupPrice,@GroupQuantity,@GroupPhoto,@YejiRatio);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ProductInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE Product SET ShopId = @ShopId, Name = @Name, SubTitle = @SubTitle, ClassId = @ClassId, Spelling = @Spelling, Color = @Color, FontStyle = @FontStyle, BrandId = @BrandId, ProductNumber = @ProductNumber, ProductCode = @ProductCode, MarketPrice = @MarketPrice, BidPrice = @BidPrice, SalePrice = @SalePrice, Weight = @Weight, SendPoint = @SendPoint, Photo = @Photo, Keywords = @Keywords, Summary = @Summary, Introduction1 = @Introduction1, Introduction1_Mobile = @Introduction1_Mobile, Introduction2 = @Introduction2, Introduction2_Mobile = @Introduction2_Mobile, Introduction3 = @Introduction3, Introduction3_Mobile = @Introduction3_Mobile, Remark = @Remark, IsSpecial = @IsSpecial, IsNew = @IsNew, IsHot = @IsHot, IsSale = @IsSale, IsTop = @IsTop, Accessory = @Accessory, RelationProduct = @RelationProduct, RelationArticle = @RelationArticle, ViewCount = @ViewCount, AllowComment = @AllowComment, CommentCount = @CommentCount, SumPoint = @SumPoint, PerPoint = @PerPoint, PhotoCount = @PhotoCount, CollectCount = @CollectCount, TotalStorageCount = @TotalStorageCount, OrderCount = @OrderCount, SendCount = @SendCount, ImportActualStorageCount = @ImportActualStorageCount, ImportVirtualStorageCount = @ImportVirtualStorageCount, LowerCount = @LowerCount, UpperCount = @UpperCount, AddDate = @AddDate, TaobaoId = @TaobaoId, OrderId = @OrderId, Unit = @Unit, TaxRate = @TaxRate, LikeNum = @LikeNum, UnLikeNum = @UnLikeNum, ProductionPlace = @ProductionPlace, GrossRate = @GrossRate,StandardType=@StandardType,[SellPoint]=@SellPoint,[IsDelete]=@IsDelete,[Qrcode]=@Qrcode,[UnlimitedStorage]=@UnlimitedStorage,[VirtualOrderCount]=@VirtualOrderCount,[UseVirtualOrder]=@UseVirtualOrder,[OpenGroup]=@OpenGroup,[GroupPrice]=@GroupPrice,[GroupQuantity]=@GroupQuantity,[GroupPhoto]=@GroupPhoto,[YejiRatio]=@YejiRatio              
                    where Id=@Id";

                conn.Execute(sql, entity);
            }
        }
        /// <summary>
        /// 逻辑删除--isdelete 置为1
        /// </summary>
        /// <param name="id"></param>
        public void DeleteLogically(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "update Product set [IsDelete]=1 where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }
        /// <summary>
        /// 逻辑删除后恢复--isdelete 置为0
        /// </summary>
        /// <param name="id"></param>
        public void Recover(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "update Product set [IsDelete]=0 where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }
        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from Product where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public ProductInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Product where id=@id";

                var data = conn.Query<ProductInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ProductInfo();
            }
        }

        public List<ProductInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Product";

                return conn.Query<ProductInfo>(sql).ToList();
            }
        }

        #region search
        public List<ProductInfo> SearchList(ProductSearchInfo productSearch)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from Product";

                string condition = PrepareCondition(productSearch).ToString();
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }

                return conn.Query<ProductInfo>(sql).ToList();
            }
        }
        public List<ProductInfo> SearchList(int currentPage, int pageSize, ProductSearchInfo searchInfo, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
                //pc.TableName = "Product";
                pc.TableName = "View_productStorage";//视图统计规格库存                
                pc.Fields = "[Id], [ShopId], [Name], [SubTitle], [ClassId], [Spelling], [Color], [FontStyle], [BrandId], [ProductNumber], [ProductCode], [MarketPrice], [BidPrice], [SalePrice], [Weight], [SendPoint], [Photo], [Keywords], [Summary], [Remark], [IsSpecial], [IsNew], [IsHot], [IsSale], [IsTop], [Accessory], [RelationProduct], [RelationArticle], [ViewCount], [AllowComment], [CommentCount], [SumPoint], [PerPoint], [PhotoCount], [CollectCount], [TotalStorageCount], [OrderCount], [SendCount], [ImportActualStorageCount], [ImportVirtualStorageCount], [LowerCount], [UpperCount], [AddDate], [TaobaoId], [OrderId], [Unit], [TaxRate], [LikeNum], [UnLikeNum], [ProductionPlace], [GrossRate],[StandardType],[SellPoint],[IsDelete],[Qrcode],[VirtualOrderCount],[UseVirtualOrder],[OpenGroup],[GroupPrice],[GroupQuantity],[GroupPhoto],[YejiRatio]";

                pc.CurrentPage = currentPage;
                pc.PageSize = pageSize;
                //如果只用一个排序字段，分页存储过程则使用top方式。否则，使用row_number方式进行分页
                //pc.OrderField = "[OrderId]";
                pc.OrderField = "[OrderId],[Id]";
                pc.OrderType = searchInfo.OrderType;
                if (searchInfo.ProductOrderType != string.Empty)
                {
                    switch (searchInfo.ProductOrderType)
                    {
                        case "CommentCount":
                            pc.OrderField = "[CommentCount],[OrderId],[ID]";
                            break;
                        case "SalePriceDown":
                            pc.OrderField = "[SalePrice],[OrderId],[ID]";
                            pc.OrderType = OrderType.Desc;
                            break;
                        case "SalePriceUp":
                            pc.OrderField = "[SalePrice],[OrderId],[ID]";
                            pc.OrderType = OrderType.Asc;
                            break;
                        case "CollectCount":
                            pc.OrderField = "[CollectCount],[OrderId],[ID]";
                            break;
                        case "ViewCount":
                            pc.OrderField = "[ViewCount],[OrderId],[ID]";
                            break;
                        case "OrderCount":
                            pc.OrderField = "[OrderCount],[OrderId],[ID]";
                            break;
                        case "LikeNum":
                            pc.OrderField = "[LikeNum],[OrderId],[ID]";
                            break;
                        case "AddDate":
                            pc.OrderField = "[AddDate],[OrderId],[ID]";
                            break;
                        default:
                            break;
                    }
                    
                }               
                //pc.OrderType =OrderType.Desc;                
                pc.MssqlCondition = PrepareCondition(searchInfo);

                count = pc.Count;
                var temppc = conn.Query<ProductInfo>(pc);
                return conn.Query<ProductInfo>(pc).ToList();
            }
        }
        public List<ProductInfo> SearchList(int currentPage, int pageSize, int classId, string brandIds, string attributeIds, string attributeValues, string orderField, string orderType, int minPrice, int maxPrice, string keywords, string stf_Keyword, int isNew, int isHot, int isSpecial, int isTop, ref int count, ref List<ProductTypeAttributeInfo> showAttributeList, ref List<ProductBrandInfo> showBrandList)
        {
            using (var conn = new SqlConnection(connectString))
            {
                List<ProductInfo> entites = new List<ProductInfo>();
                using (var multi = conn.QueryMultiple("usp_Product", new
                {
                    type = "search",
                    classId = classId,
                    pageSize = pageSize,
                    currentPage = currentPage,
                    brandIds = brandIds,
                    attributeIds = attributeIds,
                    attributeValues = attributeValues,
                    orderField = orderField,
                    orderType = orderType,
                    minPrice = minPrice,
                    maxPrice = maxPrice,
                    keywords = keywords,
                    stf_Keyword = stf_Keyword,
                    isNew = isNew,
                    isHot = isHot,
                    isSpecial = isSpecial,
                    isTop = isTop
                }, null, null, CommandType.StoredProcedure))
                {
                    //multi.IsConsumed   reader的状态 ，true 是已经释放
                    if (!multi.IsConsumed)
                    {
                        entites = multi.Read<ProductInfo>().ToList();
                        count = entites.Count > 0 ? entites[0].Total : 0;

                        showAttributeList = multi.Read<ProductTypeAttributeInfo>().ToList();
                        showBrandList = multi.Read<ProductBrandInfo>().ToList();
                    }
                }

                return entites;
            }
        }
        /// <summary>
        /// 手机端商品列表页搜索
        /// </summary>
        public List<ProductInfo> SearchList(int currentPage, int pageSize, int classId, string brandIds, string orderField, string orderType, string keywords, ref int count)
        {
            using (var conn = new SqlConnection(connectString))
            {
                var entites =
                    conn.Query<ProductInfo>("usp_Product", new
                    {
                        type = "mobile_list_search",
                        classId = classId,
                        pageSize = pageSize,
                        currentPage = currentPage,
                        brandIds = brandIds,
                        orderField = orderField,
                        orderType = orderType,
                        keywords = keywords
                    }, null, true, null, CommandType.StoredProcedure).ToList();

                count = entites.Count > 0 ? entites[0].Total : 0;
                return entites;
            }
        }
        public MssqlCondition PrepareCondition(ProductSearchInfo productSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();

            string condition = string.Empty;
            if (!string.IsNullOrEmpty(productSearch.Key))
            {
                condition = "([Name]" + " LIKE '%" + StringHelper.SearchSafe(productSearch.Key) + "%' OR ";
                condition += "[Spelling]" + " LIKE '%" + StringHelper.SearchSafe(productSearch.Key) + "%' OR ";
                condition += "[ProductNumber]" + " LIKE '%" + StringHelper.SearchSafe(productSearch.Key) + "%')";
                mssqlCondition.Add(condition);
            }
            mssqlCondition.Add("[Name]", productSearch.Name, ConditionType.Like);
            mssqlCondition.Add("[Spelling]", productSearch.Spelling, ConditionType.Like);
            mssqlCondition.Add("[ProductNumber]", productSearch.ProductNumber, ConditionType.Like);
            mssqlCondition.Add("[Keywords]", productSearch.Keywords, ConditionType.Like);
            mssqlCondition.Add("[BrandId]", productSearch.BrandId, ConditionType.Equal);
            mssqlCondition.Add("[ClassId]", productSearch.ClassId, ConditionType.Like);
            mssqlCondition.Add("[IsSpecial]", productSearch.IsSpecial, ConditionType.Equal);
            mssqlCondition.Add("[IsNew]", productSearch.IsNew, ConditionType.Equal);
            mssqlCondition.Add("[IsHot]", productSearch.IsHot, ConditionType.Equal);
            mssqlCondition.Add("[IsSale]", productSearch.IsSale, ConditionType.Equal);
            mssqlCondition.Add("[IsTop]", productSearch.IsTop, ConditionType.Equal);
            mssqlCondition.Add("[AddDate]", productSearch.StartAddDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[AddDate]", productSearch.EndAddDate, ConditionType.Less);
            mssqlCondition.Add("[Id]", productSearch.InProductId, ConditionType.In);
            mssqlCondition.Add("[Id]", productSearch.NotInProductId, ConditionType.NotIn);
            mssqlCondition.Add("[StandardType]", productSearch.StandardType, ConditionType.Equal);
            mssqlCondition.Add("[ShopId]", productSearch.InShopId, ConditionType.In);
            mssqlCondition.Add("[IsDelete]", productSearch.IsDelete, ConditionType.Equal);
            mssqlCondition.Add("[Id]", productSearch.InProductId, ConditionType.In);
            mssqlCondition.Add("[OpenGroup]", productSearch.OpenGroup, ConditionType.Equal);
            if (productSearch.StockStart > 0) mssqlCondition.Add("[TotalStorageCount]-[OrderCount]", productSearch.StockStart, ConditionType.MoreOrEqual);
            if (productSearch.StockEnd > 0) mssqlCondition.Add("[TotalStorageCount]-[OrderCount]", productSearch.StockEnd, ConditionType.LessOrEqual);

            if (productSearch.StorageAnalyse != 0)
            {
                string field = "[TotalStorageCount]-[OrderCount]";
                if (ShopConfig.ReadConfigInfo().ProductStorageType == (int)ProductStorageType.ImportStorageSystem)
                {
                    field = "[ImportActualStorageCount]";
                }
                //edit by suano at 2116-11-8, change to normal storage type;
                switch (productSearch.StorageAnalyse)
                {
                    case (int)StorageAnalyseType.Lack:
                        mssqlCondition.Add(field + "<=[LowerCount]");
                        break;
                    case (int)StorageAnalyseType.Safe:
                        mssqlCondition.Add(field + ">[LowerCount]");
                        break;
                    case (int)StorageAnalyseType.Over:
                        mssqlCondition.Add(field + ">[UpperCount]");
                        break;
                    default:
                        break;
                }
            }
            //售价筛选
            if (productSearch.LowerSalePrice >= 0) mssqlCondition.Add("[SalePrice]", productSearch.LowerSalePrice, ConditionType.MoreOrEqual);
            if (productSearch.UpperSalePrice >= 0) mssqlCondition.Add("[SalePrice]", productSearch.UpperSalePrice, ConditionType.LessOrEqual);
            //销量筛选
            if (productSearch.LowerOrderCount >= 0) mssqlCondition.Add("[OrderCount]", productSearch.LowerOrderCount, ConditionType.MoreOrEqual);
            if (productSearch.UpperOrderCount >= 0) mssqlCondition.Add("[OrderCount]", productSearch.UpperOrderCount, ConditionType.LessOrEqual);
            //if (productSearch.Tags != string.Empty)
            //{
            //    mssqlCondition.Add("[Id] IN(SELECT [ProductId] FROM " + ShopMssqlHelper.TablePrefix + "Tags WHERE [Word]='" + StringHelper.SearchSafe(productSearch.Tags) + "')");
            //}

            return mssqlCondition;
        }
        #endregion

        #region 统计分析
        public DataTable StatisticsProductSale(int currentPage, int pageSize, ProductSearchInfo productSearch, ref int count, DateTime startDate, DateTime endDate)
        {
            List<ProductInfo> productList = new List<ProductInfo>();
            string orderCondition = string.Empty;
            MssqlCondition orderCh = new MssqlCondition();
            orderCh.Add("[AddDate]", startDate, ConditionType.MoreOrEqual);
            orderCh.Add("[AddDate]", endDate, ConditionType.Less);
            orderCondition = orderCh.ToString();
            if (orderCondition != string.Empty)
            {
                orderCondition = " AND " + orderCondition;
            }
            ShopMssqlPagerClass pc = new ShopMssqlPagerClass();
            pc.TableName = "(SELECT Id,Name,ClassId,BrandId,IsSale, ISNULL(SellCount, 0) AS SellCount, ISNULL(SellMoney,0) AS SellMoney FROM [Product] ";
            pc.TableName += "LEFT OUTER JOIN (SELECT ProductId, SUM(BuyCount) AS SellCount, SUM(ProductPrice * BuyCount) AS SellMoney FROM ( ";
            pc.TableName += "SELECT OrderDetail.ProductId,OrderDetail.ProductPrice, OrderDetail.BuyCount FROM [OrderDetail] ";
            pc.TableName += " INNER JOIN [Order] ON [OrderDetail].OrderId = [Order].Id WHERE ([Order].OrderStatus = 6 " + orderCondition + ")) AS TEMP1 GROUP BY ProductId) AS TEMP2 ON [Product].Id = TEMP2.ProductId) As PageTable";
            pc.Fields = "[Id],[Name],[ClassId],[SellCount],[SellMoney]";
            pc.CurrentPage = currentPage;
            pc.PageSize = pageSize;
            pc.OrderField = "[Id]";
            if (productSearch.ProductOrderType != string.Empty)
            {
                switch (productSearch.ProductOrderType)
                {
                    case "SellCount":
                        pc.OrderField = "[SellCount],[Id]";
                        break;
                    case "SellMoney":
                        pc.OrderField = "[SellMoney],[Id]";
                        break;
                    default:
                        break;
                }
            }
            pc.OrderType = OrderType.Desc;
            pc.MssqlCondition = PrepareCondition(productSearch);
            pc.Count = count;
            count = pc.Count;
            return pc.ExecuteDataTable();
        }
        public DataTable NoHandlerStatistics()
        {
            return ShopMssqlHelper.ExecuteDataTable(ShopMssqlHelper.TablePrefix + "NoHandlerStatistics");
        }
        #endregion

        public void OffSale(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE Product SET [IsSale]=0 WHERE [ID] IN @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public void OnSale(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE Product SET [IsSale]=1 WHERE [ID] IN @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public void ChangeOrderCount(int id, int changeCount)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE Product SET [OrderCount]=OrderCount+@changeCount where id=@id";

                conn.Execute(sql, new { id = id, changeCount = changeCount });
            }
        }

        public void ChangeSendCount(int id, int changeCount)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE Product SET [SendCount]=SendCount+@changeCount where id=@id";

                conn.Execute(sql, new { id = id, changeCount = changeCount });
            }
        }

        /// <summary>
        /// 改变产品的查看数量
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="changeCount">正数表示增加，负数减少</param>
        public void ChangeViewCount(int id, int changeCount)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "UPDATE Product SET [ViewCount]=[ViewCount]+@changeCount where id=@id";

                conn.Execute(sql, new { id = id, changeCount = changeCount });
            }
        }
        public void ChangeOrderCountByOrder(int orderId, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query<ProductInfo>("usp_Product", new
                {
                    type = "ChangeOrderCount",
                    orderId = orderId,
                    changeAction = action.ToString()
                }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void ChangeSendCountByOrder(int orderId, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query<ProductInfo>("usp_Product", new
                {
                    type = "ChangeSendCount",
                    orderId = orderId,
                    changeAction = action.ToString()
                }, null, true, null, CommandType.StoredProcedure).ToList();
            }
        }

        public void ChangeProductCommentCountAndRank(int productId, int rank, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query("usp_Product", new
                {
                    type = "ChangeProductCommentCountAndRank",
                    id = productId,
                    rank = rank,
                    changeAction = action.ToString(),
                }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void ChangeProductCollectCount(int productId, ChangeAction action)
        {
            using (var conn = new SqlConnection(connectString))
            {
                conn.Query("usp_Product", new
                {
                    type = "ChangeProductCollectCount",
                    id = productId,
                    changeAction = action.ToString(),
                }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        /// <summary>
        /// 修改产品状态值
        /// </summary>
        /// <param name="statusType">状态名称</param>
        /// <param name="id">ID</param>
        /// <param name="status">状态值</param>
        public void ChangeProductStatus(int id, int status, ProductStatusType statusType)
        {
            SqlParameter[] parameters = {
				new SqlParameter("@id",SqlDbType.Int),
                new SqlParameter("@status",SqlDbType.Int),
                new SqlParameter("@statusType",SqlDbType.NVarChar)
			};
            parameters[0].Value = id;
            parameters[1].Value = status;
            parameters[2].Value = statusType;

            ProductInfo tempPro = Read(id);
            switch (statusType)
            {
                case ProductStatusType.IsSpecial:
                    tempPro.IsSpecial = status;
                    break;
                case ProductStatusType.IsNew:
                    tempPro.IsNew = status;
                    break;
                case ProductStatusType.IsHot:
                    tempPro.IsHot = status;
                    break;
                case ProductStatusType.IsSale:
                    tempPro.IsSale = status;
                    break;
                case ProductStatusType.IsTop:
                    tempPro.IsTop = status;
                    break;
                case ProductStatusType.AllowComment:
                    tempPro.AllowComment = status;
                    break;
                default:
                    break;
            }
            Update(tempPro);
        }

        public int CountByShop(int shopId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select COUNT(1) from Product where ShopId=@shopId";

                return conn.ExecuteScalar<int>(sql, new { shopId = shopId });
            }
        }

        public int CountByClass(int classId)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select COUNT(1) from Product where ClassId like @classId";

                return conn.ExecuteScalar<int>(sql, new { classId = string.Concat("%|", classId, "|%") });
            }
        }

        public string MaxProductNumber(string classId, string prefixNumber)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select top 1 ProductNumber from Product where ClassId like @classId and ProductNumber like @prefixNumber order by ProductNumber desc";

                return conn.ExecuteScalar<string>(sql, new { classId = string.Concat("%", classId, "%"), prefixNumber = string.Concat("%", prefixNumber, "%") });
            }
        }

        public bool UniqueProductNumber(string productNumber, int productId = 0)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select count(1) from Product where ProductNumber = @productNumber and id<>@productId";

                return conn.ExecuteScalar<int>(sql, new { productNumber = productNumber, productId = productId }) < 1;
            }
        }
        /// <summary>
        /// 更改产品的规格属性
        /// </summary>
        /// <param name="strID">产品的主键值,以,号分隔</param>
        /// <param name="standardType">规格值</param>
        /// <param name="id">当前产品ID</param>
        public void UpdateProductStandardType(string strID, int standardType, int id)
        {
            SqlParameter[] parameters = {
				new SqlParameter("@strID",SqlDbType.NVarChar),
                new SqlParameter("@standardType",SqlDbType.Int),
                new SqlParameter("@id",SqlDbType.Int)
			};
            parameters[0].Value = strID;
            parameters[1].Value = standardType;
            parameters[2].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateProductStandardType", parameters);
        }

        /// <summary>
        /// 批量更新产品数据
        /// </summary>
        /// <param name="productIDList">产品id串</param>
        /// <param name="product">产品模型变量</param>
        public void UnionUpdateProduct(string productIDList, ProductInfo product)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@productIDList",SqlDbType.NVarChar),
                new SqlParameter("@marketPrice",SqlDbType.Decimal),
                new SqlParameter("@weight",SqlDbType.Int),
                new SqlParameter("@sendPoint",SqlDbType.Int),
                new SqlParameter("@totalStorageCount",SqlDbType.Int),
                new SqlParameter("@lowerCount",SqlDbType.Int),
                new SqlParameter("@upperCount",SqlDbType.Int)
            };
            parameters[0].Value = productIDList;
            parameters[1].Value = product.MarketPrice;
            parameters[2].Value = product.Weight;
            parameters[3].Value = product.SendPoint;
            parameters[4].Value = product.TotalStorageCount;
            parameters[5].Value = product.LowerCount;
            parameters[6].Value = product.UpperCount;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UnionUpdateProduct", parameters);
        }
    }
}