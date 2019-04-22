using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace JWShop.Business
{
    public sealed class ProductBLL : BaseBLL
    {
        private static readonly IProduct dal = FactoryHelper.Instance<IProduct>(Global.DataProvider, "ProductDAL");
        public static readonly int TableID = UploadTable.ReadTableID("Product");

        public static int Add(ProductInfo entity)
        {
            entity.Id = dal.Add(entity);

            UploadBLL.UpdateUpload(TableID, 0, entity.Id, Cookies.Admin.GetRandomNumber(false));

            int[] classIds = Array.ConvertAll<string, int>(entity.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
            ProductClassBLL.ChangeProductCount(classIds, ChangeAction.Plus);
            CacheHelper.Remove("ProductClass");

            return entity.Id;
        }

        public static void Update(ProductInfo entity)
        {
            dal.Update(entity);
            UploadBLL.UpdateUpload(TableID, 0, entity.Id, Cookies.Admin.GetRandomNumber(false));
        }
        /// <summary>
        /// 逻辑删除--isdelete 置为1
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteLogically(int id)
        {
            bool canDel = true;
            if (OrderDetailBLL.ReadListByProductId(id).Count > 0)
            {
                foreach (OrderDetailInfo myOD in OrderDetailBLL.ReadListByProductId(id))
                {
                    OrderInfo tempOrder = OrderBLL.Read(myOD.OrderId, 0);
                    if (tempOrder.IsDelete == 0)
                    {
                        canDel = false;
                        break;
                    }
                }
                if (!canDel)
                {
                    ScriptHelper.Alert("该产品存在相关订单，不能删除。");
                }
                else
                {
                    var product = Read(id);

                    if (product.Id > 0)
                    {
                        dal.DeleteLogically(id);
                    
                        CacheHelper.Remove("ProductClass");
                    }
                }
            }
            else
            {
                var product = Read(id);

                if (product.Id > 0)
                {                  
                    dal.DeleteLogically(id);                  
                }
            }
        }
        /// <summary>
        /// 逻辑删除后恢复--isdelete 置为0
        /// </summary>
        /// <param name="id"></param>
        public static void Recover(int id)
        {
            dal.Recover(id);
        }
        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(int id)
        {
            bool canDel = true;
            if (OrderDetailBLL.ReadListByProductId(id).Count > 0)
            {
                foreach (OrderDetailInfo myOD in OrderDetailBLL.ReadListByProductId(id))
                {
                    OrderInfo tempOrder = OrderBLL.Read(myOD.OrderId, 0);
                    if (tempOrder.IsDelete == 0)
                    {
                        canDel = false;
                        break;
                    }
                }
                if (!canDel)
                {
                    ScriptHelper.Alert("该产品存在相关订单，不能删除。");
                }
                else
                {
                    var product = Read(id);

                    if (product.Id > 0)
                    {
                        UploadBLL.DeleteUploadByRecordID(TableID, id.ToString());
                        dal.Delete(id);
                        ProductPhotoBLL.DeleteList(id, 0);

                        int[] classIds = Array.ConvertAll<string, int>(product.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                        ProductClassBLL.ChangeProductCount(classIds, ChangeAction.Minus);
                        CacheHelper.Remove("ProductClass");
                    }
                }
            }
            else
            {
                var product = Read(id);

                if (product.Id > 0)
                {
                    UploadBLL.DeleteUploadByRecordID(TableID, id.ToString());
                    dal.Delete(id);
                    ProductPhotoBLL.DeleteList(id, 0);

                    int[] classIds = Array.ConvertAll<string, int>(product.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                    ProductClassBLL.ChangeProductCount(classIds, ChangeAction.Minus);
                    CacheHelper.Remove("ProductClass");
                }
            }
        }
        /// <summary>
        /// 根据产品列表，产品ID读取产品
        /// </summary>
        /// <param name="bookingProductList"></param>
        /// <returns></returns>
        public static ProductInfo ReadProductByProductList(List<ProductInfo> productList, int productID)
        {
            ProductInfo product = new ProductInfo();
            foreach (ProductInfo temp in productList)
            {
                if (temp.Id == productID)
                {
                    product = temp;
                }
            }
            return product;
        }
        /// <summary>
        /// 计算当前产品价格
        /// </summary>
        /// <param name="price"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static decimal GetCurrentPrice(decimal price, int grade)
        {
            return Math.Round(price * UserGradeBLL.Read(grade).Discount / 100, 2);
        }

        public static decimal GetCurrentPriceWithStandard(int id, int grade, string standardValue)
        {
            var proStandRecord = ProductTypeStandardRecordBLL.Read(id, standardValue);
            if (!string.IsNullOrEmpty(standardValue.Trim()))
            {
                return Math.Round(proStandRecord.SalePrice * UserGradeBLL.Read(grade).Discount / 100, 2);
            }
            else
            {
                return Math.Round(ProductBLL.Read(id).SalePrice * UserGradeBLL.Read(grade).Discount / 100, 2);
            }
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids"></param>
        public static void DeleteLogically(string ids)
        {
            string[] delArr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (delArr.Length > 0)
            {
                foreach (string did in delArr)
                {
                    int id = 0;
                    if (int.TryParse(did, out id))
                    {
                        bool canDel = true;
                        if (OrderDetailBLL.ReadListByProductId(id).Count > 0)
                        {
                            foreach (OrderDetailInfo myOD in OrderDetailBLL.ReadListByProductId(id))
                            {
                                OrderInfo tempOrder = OrderBLL.Read(myOD.OrderId, 0);
                                if (tempOrder.IsDelete == 0)
                                {
                                    canDel = false;
                                    break;
                                }
                            }
                            if (!canDel)
                            {
                                ScriptHelper.Alert("该产品存在相关订单，不能删除。");
                            }
                            else
                            {
                                var product = Read(id);

                                if (product.Id > 0)
                                {
                           
                                    dal.DeleteLogically(id);
                                    CacheHelper.Remove("ProductClass");
                                }
                            }
                        }
                        else
                        {
                            var product = Read(id);

                            if (product.Id > 0)
                            {
                                dal.DeleteLogically(id);  
                                CacheHelper.Remove("ProductClass");
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 逻辑删除后恢复--isdelete 置为0
        /// </summary>
        /// <param name="id"></param>
        public static void Recover(string ids)
        {
            string[] delArr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (delArr.Length > 0)
            {
                foreach (string did in delArr)
                {
                    int id = 0;
                    if (int.TryParse(did, out id))
                    {
                        dal.Recover(id);                      
                    }
                }
            }
        }
        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="ids"></param>
        public static void Delete(string ids)
        {
            string[] delArr = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (delArr.Length > 0)
            {
                foreach (string did in delArr)
                {
                    int id = 0;
                    if (int.TryParse(did, out id))
                    {
                        bool canDel = true;
                        if (OrderDetailBLL.ReadListByProductId(id).Count > 0)
                        {
                            foreach (OrderDetailInfo myOD in OrderDetailBLL.ReadListByProductId(id))
                            {
                                OrderInfo tempOrder = OrderBLL.Read(myOD.OrderId, 0);
                                if (tempOrder.IsDelete == 0)
                                {
                                    canDel = false;
                                    break;
                                }
                            }
                            if (!canDel)
                            {
                                ScriptHelper.Alert("该产品存在相关订单，不能删除。");
                            }
                            else
                            {
                                var product = Read(id);

                                if (product.Id > 0)
                                {
                                    UploadBLL.DeleteUploadByRecordID(TableID, id.ToString());
                                    dal.Delete(id);
                                    ProductPhotoBLL.DeleteList(id, 0);

                                    int[] classIds = Array.ConvertAll<string, int>(product.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                                    ProductClassBLL.ChangeProductCount(classIds, ChangeAction.Minus);
                                    CacheHelper.Remove("ProductClass");
                                }
                            }
                        }
                        else
                        {
                            var product = Read(id);

                            if (product.Id > 0)
                            {
                                UploadBLL.DeleteUploadByRecordID(TableID, id.ToString());
                                dal.Delete(id);
                                ProductPhotoBLL.DeleteList(id, 0);

                                int[] classIds = Array.ConvertAll<string, int>(product.ClassId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k));
                                ProductClassBLL.ChangeProductCount(classIds, ChangeAction.Minus);
                                CacheHelper.Remove("ProductClass");
                            }
                        }
                    }
                }
            }
        }

        public static ProductInfo Read(int id)
        {
            return dal.Read(id);
        }

        public static List<ProductInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<ProductInfo> SearchList(int currentPage, int pageSize, ProductSearchInfo searchInfo, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, searchInfo, ref count);
        }
        public static List<ProductInfo> SearchList(ProductSearchInfo searchInfo)
        {
            return dal.SearchList(searchInfo);
        }
        public static List<ProductInfo> SearchList(int currentPage, int pageSize, int classId, string brandIds, string attributeIds, string attributeValues, string orderField, string orderType, int minPrice, int maxPrice, string keywords, string stf_Keyword, int isNew, int isHot, int isSpecial, int isTop, ref int count, ref List<ProductTypeAttributeInfo> showAttributeList, ref List<ProductBrandInfo> showBrandList)
        {
            return dal.SearchList(currentPage, pageSize, classId, brandIds, attributeIds, attributeValues, orderField, orderType, minPrice, maxPrice, keywords, stf_Keyword, isNew, isHot, isSpecial, isTop, ref count, ref showAttributeList, ref showBrandList);
        }

        /// <summary>
        /// 手机端商品列表页搜索
        /// </summary>
        public static List<ProductInfo> SearchList(int currentPage, int pageSize, int classId, string brandIds, string orderField, string orderType, string keywords, ref int count)
        {
            return dal.SearchList(currentPage, pageSize, classId, brandIds, orderField, orderType, keywords, ref count);
        }

        public static void OffSale(int[] ids)
        {
            dal.OffSale(ids);
        }

        public static void OnSale(int[] ids)
        {
            dal.OnSale(ids);
        }

        public static void ChangeOrderCount(int id, int changeCount)
        {
            dal.ChangeOrderCount(id, changeCount);
        }

        public static void ChangeSendCount(int id, int changeCount)
        {
            dal.ChangeSendCount(id, changeCount);
        }

        /// <summary>
        /// 改变产品的查看数量
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="changeCount">正数表示增加，负数减少</param>
        public static void ChangeViewCount(int id, int changeCount)
        {
            dal.ChangeViewCount(id, changeCount);
        }

        public static void ChangeOrderCountByOrder(int orderId, ChangeAction action)
        {
            dal.ChangeOrderCountByOrder(orderId, action);
            //购买的是该商品下面的具体规格产品
            var orderDetailList = OrderDetailBLL.ReadList(orderId);
            foreach (var orderDetail in orderDetailList)
            {
                if (!string.IsNullOrEmpty(orderDetail.StandardValueList))
                {
                    ProductTypeStandardRecordBLL.ChangeOrderCount(orderDetail.ProductId, orderDetail.StandardValueList, orderDetail.BuyCount, action);
                }
            }
        }

        public static void ChangeSendCountByOrder(int orderId, ChangeAction action)
        {
            dal.ChangeSendCountByOrder(orderId, action);
            //购买的是该商品下面的具体规格产品
            var orderDetailList = OrderDetailBLL.ReadList(orderId);
            foreach (var orderDetail in orderDetailList)
            {
                if (!string.IsNullOrEmpty(orderDetail.StandardValueList))
                {
                    ProductTypeStandardRecordBLL.ChangeSendCount(orderDetail.ProductId, orderDetail.StandardValueList, orderDetail.BuyCount, action);
                }
            }
        }

        public static void ChangeProductCommentCountAndRank(int productId, int rank, ChangeAction action)
        {
            dal.ChangeProductCommentCountAndRank(productId, rank, action);
        }

        public static void ChangeProductCollectCount(int productId, ChangeAction action)
        {
            dal.ChangeProductCollectCount(productId, action);
        }

        public static int CountByShop(int shopId)
        {
            return dal.CountByShop(shopId);
        }

        public static int CountByClass(int classId)
        {
            return dal.CountByClass(classId);
        }

        public static string MaxProductNumber(string classId, string prefixNumber)
        {
            return dal.MaxProductNumber(classId, prefixNumber);
        }

        /// <summary>
        /// 批量更新产品数据
        /// </summary>
        /// <param name="productIDList">产品id串</param>
        /// <param name="product">产品模型变量</param>
        public static void UnionUpdateProduct(string productIDList, ProductInfo product)
        {
            dal.UnionUpdateProduct(productIDList, product);
        }
        public static bool UniqueProductNumber(string productNumber, int productId = 0)
        {
            return dal.UniqueProductNumber(productNumber, productId);
        }
        /// <summary>
        /// 修改产品状态值
        /// </summary>
        /// <param name="statusType">状态名称</param>
        /// <param name="id">ID</param>
        /// <param name="status">状态值</param>
        public static void ChangeProductStatus(int id, int status, ProductStatusType statusType)
        {
            dal.ChangeProductStatus(id, status, statusType);
        }
        /// <summary>
        /// 更改产品的规格属性
        /// </summary>
        /// <param name="strID">产品的主键值,以,号分隔</param>
        /// <param name="standardType">规格值</param>
        /// <param name="id">当前产品ID</param>
        public static void UpdateProductStandardType(string strID, int standardType, int id)
        {
            dal.UpdateProductStandardType(strID, standardType, id);
        }
        #region 统计分析
        /// <summary>
        /// 产品销量分析
        /// </summary>
        public static DataTable StatisticsProductSale(int currentPage, int pageSize, ProductSearchInfo productSearch, ref int count, DateTime startDate, DateTime endDate)
        {
            return dal.StatisticsProductSale(currentPage, pageSize, productSearch, ref count, startDate, endDate);
        }
        /// <summary>
        /// 待处理事务统计
        /// </summary>
        public static DataTable NoHandlerStatistics()
        {
            return dal.NoHandlerStatistics();
        }
        #endregion
        

        public static object[] ParseProductData(params object[] importParams)
        {
            string str = (string)importParams[0];
            HttpContext current = HttpContext.Current;
            DataTable productSet = GetProductSet();
            StreamReader reader = new StreamReader(Path.Combine(str, "products.csv"), Encoding.Unicode);
            string str2 = reader.ReadToEnd();
            reader.Close();
            str2 = str2.Substring(str2.IndexOf('\n') + 1);
            str2 = str2.Substring(str2.IndexOf('\n') + 1);
            StreamWriter writer = new StreamWriter(Path.Combine(str, "products.csv"), false, Encoding.Unicode);
            writer.Write(str2);
            writer.Close();
            using (CsvReader reader2 = new CsvReader(new StreamReader(Path.Combine(str, "products.csv"), Encoding.Default), true, '\t'))
            {
                int num = 0;
                while (reader2.ReadNextRecord())
                {
                    num++;
                    DataRow row = productSet.NewRow();
                    new Random();
                    row["SKU"] = reader2["商家编码"];
                    row["SalePrice"] = decimal.Parse(reader2["宝贝价格"]);
                    row["Num"] = 0;
                    if (!string.IsNullOrEmpty(reader2["宝贝数量"]))
                    {
                        row["Num"] = row["Stock"] = Convert.ToInt64(reader2["宝贝数量"]);
                    }
                    row["ProductName"] = Trim(reader2["宝贝名称"]);
                    if (!string.IsNullOrEmpty(reader2["宝贝描述"]))
                    {
                        row["Description"] = Trim(reader2["宝贝描述"].Replace("\"\"", "\"").Replace("alt=\"\"", "").Replace("alt=\"", "").Replace("alt=''", ""));
                    }
                    string str3 = Trim(reader2["新图片"]);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        if (str3.EndsWith(";"))
                        {
                            string[] strArray = str3.Split(new char[] { ';' });
                            for (int i = 0; i < (strArray.Length - 1); i++)
                            {
                                string str4 = strArray[i].Substring(0, strArray[i].IndexOf(":"));
                                string str5 = str4 + ".jpg";
                                if (File.Exists(Path.Combine(str + @"\products", str4 + ".tbi")))
                                {
                                    File.Copy(Path.Combine(str + @"\products", str4 + ".tbi"), current.Request.MapPath("~/Upload/TaoBaoPhoto/Original/" + str5), true);
                                    switch (i)
                                    {
                                        case 0:
                                            row["ImageUrl1"] = "/Upload/TaoBaoPhoto/Original/" + str5;
                                            break;

                                        case 1:
                                            row["ImageUrl2"] = "/Upload/TaoBaoPhoto/Original/" + str5;
                                            break;

                                        case 2:
                                            row["ImageUrl3"] = "/Upload/TaoBaoPhoto/Original/" + str5;
                                            break;

                                        case 3:
                                            row["ImageUrl4"] = "/Upload/TaoBaoPhoto/Original/" + str5;
                                            break;

                                        case 4:
                                            row["ImageUrl5"] = "/Upload/TaoBaoPhoto/Original/" + str5;
                                            break;
                                    }
                                }
                            }
                        }
                        else if (File.Exists(Path.Combine(str + @"\products", str3.Replace(".jpg", ".tbi"))))
                        {
                            File.Copy(Path.Combine(str + @"\products", str3.Replace(".jpg", ".tbi")), current.Request.MapPath("~/Upload/TaoBaoPhoto/Original/" + str3), true);
                            row["ImageUrl1"] = "/Upload/TaoBaoPhoto/Original/" + str3;
                        }
                    }
                    row["Cid"] = 0;
                    if (!string.IsNullOrEmpty(reader2["宝贝类目"]))
                    {
                        row["Cid"] = Convert.ToInt64(reader2["宝贝类目"]);
                    }
                    row["StuffStatus"] = (reader2["新旧程度"] == "1") ? "new" : "second";
                    row["LocationState"] = reader2["省"];
                    row["LocationCity"] = reader2["城市"];
                    row["FreightPayer"] = (reader2["运费承担"] == "1") ? "seller" : "buyer";
                    try
                    {
                        row["PostFee"] = decimal.Parse(reader2["平邮"]);
                        row["ExpressFee"] = decimal.Parse(reader2["快递"]);
                        row["EMSFee"] = decimal.Parse(reader2["EMS"]);
                    }
                    catch
                    {
                        row["PostFee"] = 0M;
                        row["ExpressFee"] = 0M;
                        row["EMSFee"] = 0M;
                    }
                    row["HasInvoice"] = reader2["发票"] == "1";
                    row["HasWarranty"] = reader2["保修"] == "1";
                    row["HasDiscount"] = reader2["会员打折"] == "1";
                    if (!string.IsNullOrEmpty(reader2["有效期"]))
                    {
                        row["ValidThru"] = long.Parse(reader2["有效期"]);
                    }
                    if (!string.IsNullOrEmpty(reader2["开始时间"]))
                    {
                        row["ListTime"] = DateTime.Parse(reader2["开始时间"]);
                    }
                    row["PropertyAlias"] = reader2["宝贝属性"];
                    row["InputPids"] = reader2["用户输入ID串"];
                    row["InputStr"] = reader2["用户输入名-值对"];
                    row["Has_ShowCase"] = reader2["橱窗推荐"];
                    string str6 = string.Empty;
                    string str7 = string.Empty;
                    string str8 = string.Empty;
                    string str9 = string.Empty;
                    string str10 = reader2["销售属性组合"];
                    if (!string.IsNullOrEmpty(str10))
                    {
                        string pattern = @"(?<Price>[^:]+):(?<Quantities>[^:]+):(?<Outid>[^:]*):(?<Skuprop>[^;]+;(?:\d+:\d+;)?)";
                        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                        foreach (Match match in regex.Matches(str10))
                        {
                            str7 = str7 + match.Groups["Quantities"] + ",";
                            str6 = str6 + match.Groups["Price"] + ",";
                            str8 = str8 + match.Groups["Outid"] + ",";
                            str9 = str9 + match.Groups["Skuprop"].ToString().Substring(0, match.Groups["Skuprop"].ToString().Length - 1) + ",";
                        }
                        if (str7.Length > 0)
                        {
                            str7 = str7.Substring(0, str7.Length - 1);
                        }
                        if (str6.Length > 0)
                        {
                            str6 = str6.Substring(0, str6.Length - 1);
                        }
                        if (str8.Length > 0)
                        {
                            str8 = str8.Substring(0, str8.Length - 1);
                        }
                        if (str9.Length > 0)
                        {
                            str9 = str9.Substring(0, str9.Length - 1);
                        }
                    }
                    row["SkuProperties"] = str9;
                    row["SkuQuantities"] = str7;
                    row["SkuPrices"] = str6;
                    row["SkuOuterIds"] = str8;
                    productSet.Rows.Add(row);
                }
            }
            return new object[] { productSet };
        }

        private static DataTable GetProductSet()
        {
            DataTable table = new DataTable("products");
            DataColumn column = new DataColumn("ProductName")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("Description")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column2);
            DataColumn column3 = new DataColumn("ImageUrl1")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column3);
            DataColumn column4 = new DataColumn("ImageUrl2")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column4);
            DataColumn column5 = new DataColumn("ImageUrl3")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column5);
            DataColumn column6 = new DataColumn("ImageUrl4")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column6);
            DataColumn column7 = new DataColumn("ImageUrl5")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column7);
            DataColumn column8 = new DataColumn("SKU")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column8);
            DataColumn column9 = new DataColumn("Stock")
            {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column9);
            DataColumn column10 = new DataColumn("SalePrice")
            {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column10);
            DataColumn column11 = new DataColumn("Weight")
            {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column11);
            DataColumn column12 = new DataColumn("Cid")
            {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column12);
            DataColumn column13 = new DataColumn("StuffStatus")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column13);
            DataColumn column14 = new DataColumn("Num")
            {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column14);
            DataColumn column15 = new DataColumn("LocationState")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column15);
            DataColumn column16 = new DataColumn("LocationCity")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column16);
            DataColumn column17 = new DataColumn("FreightPayer")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column17);
            DataColumn column18 = new DataColumn("PostFee")
            {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column18);
            DataColumn column19 = new DataColumn("ExpressFee")
            {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column19);
            DataColumn column20 = new DataColumn("EMSFee")
            {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column20);
            DataColumn column21 = new DataColumn("HasInvoice")
            {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column21);
            DataColumn column22 = new DataColumn("HasWarranty")
            {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column22);
            DataColumn column23 = new DataColumn("HasDiscount")
            {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column23);
            DataColumn column24 = new DataColumn("ValidThru")
            {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column24);
            DataColumn column25 = new DataColumn("ListTime")
            {
                DataType = Type.GetType("System.DateTime")
            };
            table.Columns.Add(column25);
            DataColumn column26 = new DataColumn("PropertyAlias")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column26);
            DataColumn column27 = new DataColumn("InputPids")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column27);
            DataColumn column28 = new DataColumn("InputStr")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column28);
            DataColumn column29 = new DataColumn("SkuProperties")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column29);
            DataColumn column30 = new DataColumn("SkuQuantities")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column30);
            DataColumn column31 = new DataColumn("SkuPrices")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column31);
            DataColumn column32 = new DataColumn("SkuOuterIds")
            {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column32);
            DataColumn column33 = new DataColumn("Has_ShowCase")
            {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column33);
            return table;
        }

        private static string Trim(string str)
        {
            while (str.StartsWith("\""))
            {
                str = str.Substring(1);
            }
            while (str.EndsWith("\""))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }
    }
}