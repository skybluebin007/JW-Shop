using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Web.Admin
{
    public partial class OrderPrint : JWShop.Page.AdminBasePage
    {
        protected string orderHtml = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            int orderId = RequestHelper.GetQueryString<int>("OrderId");
            string action = RequestHelper.GetQueryString<string>("Action");
            if (orderId > 0 && action != string.Empty)
            {
                OrderInfo order = OrderBLL.Read(orderId);
                List<OrderDetailInfo> orderDetailList = OrderDetailBLL.ReadList(orderId);
                switch (action)
                {
                    case "Html":
                        HtmlPrint(order, orderDetailList);
                        break;
                    case "Excel":
                        ExcelPrint(order, orderDetailList);
                        break;
                    case "ExportSingle":
                        ExportSingle();
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Html打印
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderDetailList"></param>
        private void HtmlPrint(OrderInfo order, List<OrderDetailInfo> orderDetailList)
        {
            using (StreamReader sr = new StreamReader(ServerHelper.MapPath("/Admin/Print/Template.htm")))
            {
                orderHtml = sr.ReadToEnd();
            }
            //特殊标签
            orderHtml = orderHtml.Replace("<$RegionID$>", RegionBLL.RegionNameList(order.RegionId));
            orderHtml = orderHtml.Replace("<$ShippingName$>", ShippingBLL.Read(order.ShippingId).Name);
            orderHtml = orderHtml.Replace("<$PrintTime$>", RequestHelper.DateNow.ToString("yyyy-MM-dd"));
            orderHtml = orderHtml.Replace("<$ActionUser$>", Cookies.Admin.GetAdminName(false));
            orderHtml = orderHtml.Replace("<$NoPayMoney$>", OrderBLL.ReadNoPayMoney(order).ToString());
            //订单字段
            Type type = typeof(OrderInfo);
            PropertyInfo[] propertyArray = type.GetProperties().Take(44).ToArray();
            foreach (PropertyInfo property in propertyArray)
            {
                orderHtml = orderHtml.Replace("<$" + property.Name + "$>", property.GetValue(order, null).ToString());
            }
            //订单详细
            string strOrderDetailList = string.Empty;
            int i = 1;
            foreach (OrderDetailInfo orderDetail in orderDetailList)
            {
                strOrderDetailList += "<tr align=\"middle\">";
                strOrderDetailList += "<td>" + i.ToString() + "</td>";
                strOrderDetailList += "<td align=\"left\">" + orderDetail.ProductName + "</td>";
                strOrderDetailList += "<td>" + orderDetail.BuyCount + "</td>";
                strOrderDetailList += "<td>" + orderDetail.ProductPrice.ToString("n") + "</td>";
                strOrderDetailList += "<td>" + (orderDetail.BuyCount * orderDetail.ProductPrice).ToString("n") + "</td>";
                strOrderDetailList += "</tr>";
                i++;
            }
            orderHtml = orderHtml.Replace("<$OrderDetailList$>", strOrderDetailList);
        }
        /// <summary>
        /// Excel打印
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderDetailList"></param>
        private void ExcelPrint(OrderInfo order, List<OrderDetailInfo> orderDetailList)
        {
            try
            {
                string templetFile = Server.MapPath("/Admin/Print/Template.xls");
                string outFile = "/Admin/Print/Order/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + Guid.NewGuid().ToString() + ".xls";
                string outFullFile = Server.MapPath(outFile);

                OrderExcelHelper orderExcel = new OrderExcelHelper(templetFile, outFullFile);
                orderExcel.CellParameters = ReadCellParameters(order);
               orderExcel.Dt = ReadDataTable(orderDetailList);
                orderExcel.Rows = 14;
                orderExcel.Left = 0;
                orderExcel.Top = 7;
                orderExcel.DataTableToExcel();

                string excelPath = "http://" + HttpContext.Current.Request.ServerVariables["Http_Host"] + outFile.Replace("\\", "/");
                ResponseHelper.Redirect(excelPath);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, true);
            }
        }
        /// <summary>
        /// 组装订单详细
        /// </summary>
        /// <param name="orderDetailList"></param>
        /// <returns></returns>
        private DataTable ReadDataTable(List<OrderDetailInfo> orderDetailList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Index", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("ProductName", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("BuyCount", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("ProductPrice", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("TotalProductPrice", Type.GetType("System.String")));
            int i = 0;
            foreach (OrderDetailInfo orderDetail in orderDetailList)
            {
                i++;
                DataRow row = dt.NewRow();
                row["Index"] = i;
                row["ProductName"] = orderDetail.ProductName;
                row["BuyCount"] = orderDetail.BuyCount;
                row["ProductPrice"] = (orderDetail.ProductPrice).ToString("n");
                row["TotalProductPrice"] = (orderDetail.BuyCount * orderDetail.ProductPrice).ToString("n");
                dt.Rows.Add(row);
            }
            return dt;
        }
        /// <summary>
        /// 读取单元格的值
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private Dictionary<int[], string> ReadCellParameters(OrderInfo order)
        {
            Dictionary<int[], string> cellDic = new Dictionary<int[], string>();
            cellDic.Add(new int[] { 1, 1 }, order.OrderNumber);
            cellDic.Add(new int[] { 1, 3 }, order.UserName);
            cellDic.Add(new int[] { 1, 5 }, order.AddDate.ToString());
            cellDic.Add(new int[] { 2, 1 }, order.PayName);
            cellDic.Add(new int[] { 2, 3 }, ShippingBLL.Read(order.ShippingId).Name);
            cellDic.Add(new int[] { 2, 5 }, order.ShippingNumber);
            cellDic.Add(new int[] { 3, 1 }, order.Consignee);
            cellDic.Add(new int[] { 3, 3 }, order.ZipCode);
            cellDic.Add(new int[] { 3, 5 }, order.Tel + " " + order.Mobile);
            cellDic.Add(new int[] { 4, 1 }, "[" + RegionBLL.RegionNameList(order.RegionId) + "] " + order.Address);
            cellDic.Add(new int[] { 5, 1 }, order.UserMessage);
            cellDic.Add(new int[] { 21, 0 }, "产品金额：" + order.ProductMoney + " 元" + " + 运费：" + order.ShippingMoney + " 元 + 其它费用：" + order.OtherMoney +"元 - 优惠券："+order.CouponMoney+"元 - 优惠活动"+order.FavorableMoney+ "元 - 积分抵扣金额：" + order.PointMoney + " 元 - 余额：" + order.Balance + " 元");
            cellDic.Add(new int[] { 22, 0 }, "应付金额：" + OrderBLL.ReadNoPayMoney(order).ToString() + " 元");
            cellDic.Add(new int[] { 23, 0 }, "打印时间：" + RequestHelper.DateNow.ToString("yyyy-MM-dd") + "  操作者：" + Cookies.Admin.GetAdminName(false));
            return cellDic;
        }

        /// <summary>
        /// 导出
        /// </summary>
        private void ExportSingle()
        {
            int orderId = RequestHelper.GetQueryString<int>("OrderId");
            var order = OrderBLL.Read(orderId);
            var orderDetailList = OrderDetailBLL.ReadList(orderId);

            try
            {
                string templetFile = Server.MapPath("/Admin/Print/Template_ExportSingle.xls");
                string outFile = "/Admin/Print/Order/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + Guid.NewGuid().ToString() + ".xls";
                string outFullFile = Server.MapPath(outFile);

                OrderExcelHelper orderExcel = new OrderExcelHelper(templetFile, outFullFile);

                /*---------组装订单详细 start-----------------------------------*/
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Index", Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("ProductName", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("BuyCount", Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("ProductPrice", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("Weight", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("TotalProductPrice", Type.GetType("System.String")));
                int i = 0;
                foreach (OrderDetailInfo orderDetail in orderDetailList)
                {
                    i++;
                    DataRow row = dt.NewRow();
                    row["Index"] = i;
                    row["ProductName"] = orderDetail.ProductName;
                    row["BuyCount"] = orderDetail.BuyCount;
                    row["ProductPrice"] = (orderDetail.ProductPrice).ToString("n");
                    row["Weight"] = (orderDetail.ProductWeight).ToString("n");
                    row["TotalProductPrice"] = (orderDetail.BuyCount * orderDetail.ProductPrice).ToString("n");
                    dt.Rows.Add(row);
                }

                orderExcel.Dt = dt;
                /*---------组装订单详细 end-------------------------------------*/

                /*---------设置单元格值 start-----------------------------------*/
                Dictionary<int[], string> cellDic = new Dictionary<int[], string>();

                //基本信息
                cellDic.Add(new int[] { 2, 1 }, order.OrderNumber);
                cellDic.Add(new int[] { 2, 3 }, OrderBLL.ReadOrderStatus(order.OrderStatus,order.IsDelete));
                cellDic.Add(new int[] { 2, 5 }, order.PayName);
                cellDic.Add(new int[] { 2, 7 }, order.UserName);
                cellDic.Add(new int[] { 3, 1 }, order.AddDate.ToString());
                cellDic.Add(new int[] { 3, 3 }, order.PayDate.ToString());

                //邮寄信息
                cellDic.Add(new int[] { 5, 1 }, order.Consignee);
                cellDic.Add(new int[] { 5, 3 }, RegionBLL.RegionNameList(order.RegionId));
                cellDic.Add(new int[] { 5, 5 }, order.ZipCode);
                cellDic.Add(new int[] { 6, 1 }, order.Address);
                cellDic.Add(new int[] { 7, 1 }, order.Tel);
                cellDic.Add(new int[] { 7, 3 }, order.Email);
                cellDic.Add(new int[] { 7, 5 }, order.Mobile);
                cellDic.Add(new int[] { 8, 1 }, ShippingBLL.Read(order.ShippingId).Name);
                cellDic.Add(new int[] { 8, 3 }, order.ShippingDate.ToString("yyyy-MM-dd"));
                cellDic.Add(new int[] { 8, 5 }, order.ShippingNumber);

                //其他信息
                cellDic.Add(new int[] { 10, 1 }, order.UserMessage);
                cellDic.Add(new int[] { 11, 1 }, order.OrderNote);

                //汇总信息
                cellDic.Add(new int[] { 28, 0 }, "产品金额：" + order.ProductMoney + " 元" + " + 运费：" + order.ShippingMoney + " 元 + 其它费用：" + order.OtherMoney + "元 - 积分抵扣金额：" + order.PointMoney + " 元 - 余额：" + order.Balance + " 元");
                cellDic.Add(new int[] { 29, 0 }, "应付金额：" + OrderBLL.ReadNoPayMoney(order).ToString() + " 元");
                cellDic.Add(new int[] { 30, 0 }, "打印时间：" + RequestHelper.DateNow.ToString("yyyy-MM-dd") + "  操作者：" + Cookies.Admin.GetAdminName(false));

                orderExcel.CellParameters = cellDic;
                /*---------设置单元格值 end-------------------------------------*/
                orderExcel.Rows = 14;
                orderExcel.Left = 0;
                orderExcel.Top = 14;
                orderExcel.SheetPrefixName = order.OrderNumber;
                orderExcel.DataTableToExcel();

                string excelPath = "http://" + HttpContext.Current.Request.ServerVariables["Http_Host"] + outFile.Replace("\\", "/");
                ResponseHelper.Redirect(excelPath);
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, true);
            }
        }
    }
}