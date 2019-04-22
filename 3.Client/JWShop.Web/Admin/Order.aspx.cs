using System;
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
    public partial class Order : JWShop.Page.AdminBasePage
    {
        protected int intOrderStatus = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {
                //检查待付款订单是否超时失效，超时则更新为失效状态
                OrderBLL.CheckOrderPayTime();
                //订单自动收货
                OrderBLL.CheckOrderRecieveTimeProg();

                CheckAdminPower("ReadOrder", PowerCheckType.Single);
               
                OrderNumber.Text = RequestHelper.GetQueryString<string>("OrderNumber");
                OrderStatus.Text = RequestHelper.GetQueryString<string>("OrderStatus");
                Consignee.Text = RequestHelper.GetQueryString<string>("Consignee");
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");
                SelfPick.Text = RequestHelper.GetQueryString<string>("SelfPick");
                OrderSearchInfo orderSearch = new OrderSearchInfo();
                orderSearch.OrderNumber = RequestHelper.GetQueryString<string>("OrderNumber");
                 intOrderStatus = RequestHelper.GetQueryString<int>("OrderStatus");
            
                //如果查找已删除订单
                if (intOrderStatus == (int)Entity.OrderStatus.HasDelete)
                {
                    orderSearch.IsDelete = (int)BoolType.True;//已删除
                }
                else
                {
                    orderSearch.OrderStatus = intOrderStatus;
                    orderSearch.IsDelete = (int)BoolType.False;//未删除
                }
                orderSearch.Consignee = RequestHelper.GetQueryString<string>("Consignee");
                orderSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                orderSearch.EndAddDate = RequestHelper.GetQueryString<DateTime>("EndAddDate");
                orderSearch.SelfPick = RequestHelper.GetQueryString<int>("SelfPick");
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                var orderList = OrderBLL.SearchList(CurrentPage, PageSize, orderSearch, ref Count);
                
                //取得订单最后一次操作的时间
                var orderActinList = OrderActionBLL.ReadListLastDate(orderList.Select(k => k.Id).ToArray());
                foreach (var order in orderList)
                {
                    var orderAction = orderActinList.FirstOrDefault(k => k.OrderId == order.Id) ?? new OrderActionInfo();
                    //此处使用InvoiceTitle字段来临时存储
                    order.InvoiceTitle = orderAction.OrderId > 0 ? orderAction.Date.ToString() : "";
                }
                BindControl(orderList, RecordList, MyPager);
                #region 删除订单
                if (RequestHelper.GetQueryString<string>("Action") == "delete")
                {
                    CheckAdminPower("DeleteOrder", PowerCheckType.Single);
                    int orderId = RequestHelper.GetQueryString<int>("orderId");
                    if (orderId > 0)
                    {
                        OrderInfo tmpOrder = OrderBLL.Read(orderId);
                        if (tmpOrder.OrderStatus == (int)JWShop.Entity.OrderStatus.WaitPay)
                        {//待付款直接删除退还积分库存
                            #region 退还积分
                            if (tmpOrder.Point > 0)
                            {
                                var accountRecord = new UserAccountRecordInfo
                                {
                                    RecordType = (int)AccountRecordType.Point,
                                    Money = 0,
                                    Point = tmpOrder.Point,
                                    Date = DateTime.Now,
                                    IP = ClientHelper.IP,
                                    Note = "取消订单：" + tmpOrder.OrderNumber + "，退回用户积分",
                                    UserId = tmpOrder.UserId,
                                    UserName = tmpOrder.UserName,
                                };
                                UserAccountRecordBLL.Add(accountRecord);
                            }
                            #endregion
                            //更新商品库存数量
                            ProductBLL.ChangeOrderCountByOrder(tmpOrder.Id, ChangeAction.Minus);
                            OrderBLL.Delete(orderId);
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecordCompletely"), ShopLanguage.ReadLanguage("Order"), orderId);
                        }
                        else
                        { //已付款逻辑删除可恢复
                            if (tmpOrder.IsDelete == (int)BoolType.False)
                            {
                                tmpOrder.IsDelete = (int)BoolType.True;
                                OrderBLL.Update(tmpOrder);
                                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Order"), orderId);
                            }
                        }
                    }

                    ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"),Request.UrlReferrer.ToString());
                }
                #endregion

               
              
                
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteOrder", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                OrderBLL.Delete(Array.ConvertAll<string, int>(deleteID.Split(','), k => Convert.ToInt32(k)), 0);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Order"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "Order.aspx?Action=search&";
            URL += "OrderNumber=" + OrderNumber.Text + "&";
            URL += "SelfPick=" + SelfPick.Text + "&";
            URL += "OrderStatus=" + OrderStatus.Text + "&";
            URL += "Consignee=" + Consignee.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.OrderNumber = ShopCommon.ConvertToT<string>(OrderNumber.Text);
            orderSearch.OrderStatus = ShopCommon.ConvertToT<int>(OrderStatus.Text);
            orderSearch.Consignee = ShopCommon.ConvertToT<string>(Consignee.Text);
            orderSearch.StartAddDate = ShopCommon.ConvertToT<DateTime>(StartAddDate.Text);
            orderSearch.EndAddDate = ShopCommon.SearchEndDate(ShopCommon.ConvertToT<DateTime>(EndAddDate.Text));
            var data = OrderBLL.SearchList(1, 1000, orderSearch, ref Count);

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
            sheet.DefaultColumnWidth = 18;
            sheet.CreateFreezePane(0, 1, 0, 1);

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.Height = 20 * 20;
            row.CreateCell(0).SetCellValue("订单号");
            row.CreateCell(1).SetCellValue("订单金额");
            row.CreateCell(2).SetCellValue("类型");
            row.CreateCell(3).SetCellValue("收货方式");
            row.CreateCell(4).SetCellValue("收货人");
            //row.CreateCell(3).SetCellValue("收货地址");
            row.CreateCell(5).SetCellValue("订单状态");
            row.CreateCell(6).SetCellValue("下单时间");
            row.CreateCell(7).SetCellValue("最近操作时间");

            //设置表头格式
            var headFont = book.CreateFont();
            headFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            headFont.FontHeightInPoints = 10;
            var headStyle = book.CreateCellStyle();
            headStyle.SetFont(headFont);
            headStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            foreach (var cell in row.Cells)
            {
                cell.CellStyle = headStyle;
            }

            //取得订单最后一次操作的时间
            var orderActinList = OrderActionBLL.ReadListLastDate(data.Select(k => k.Id).ToArray());

            foreach (var entity in data)
            {
                NPOI.SS.UserModel.IRow dataRow = sheet.CreateRow(data.IndexOf(entity) + 1);
                dataRow.CreateCell(0).SetCellValue(entity.OrderNumber);
                dataRow.CreateCell(1).SetCellValue((entity.ProductMoney + entity.ShippingMoney + entity.OtherMoney).ToString());
                dataRow.CreateCell(2).SetCellValue(EnumHelper.ReadEnumChineseName<OrderKind>(entity.IsActivity));
                dataRow.CreateCell(3).SetCellValue(entity.SelfPick == 1 ? "自提" : "配送");
                dataRow.CreateCell(4).SetCellValue(entity.Consignee);
                //dataRow.CreateCell(3).SetCellValue(entity.Address);
                dataRow.CreateCell(5).SetCellValue(OrderBLL.ReadOrderStatus(entity.OrderStatus,entity.IsDelete));
                dataRow.CreateCell(6).SetCellValue(entity.AddDate.ToString());

                var orderAction = orderActinList.FirstOrDefault(k => k.OrderId == entity.Id) ?? new OrderActionInfo();
                dataRow.CreateCell(7).SetCellValue(orderAction.OrderId > 0 ? orderAction.Date.ToString() : "");

                var style = book.CreateCellStyle();
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                foreach (var cell in dataRow.Cells)
                {
                    cell.CellStyle = style;
                }
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
            Response.End();
        }

        /// <summary>
        /// 每页显示条数控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["AdminPageSize"] = AdminPageSize.Text;
            string URL = "Order.aspx?Action=search&";
            URL += "OrderNumber=" + OrderNumber.Text + "&";
            URL += "OrderStatus=" + OrderStatus.Text + "&";
            URL += "Consignee=" + Consignee.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }

        protected int GetOrderCount(int orderStatus) {
            int result = 0;
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            if (orderStatus > 0)
            {
                //如果查找已删除订单
                if (orderStatus == (int)Entity.OrderStatus.HasDelete)
                {
                    orderSearch.IsDelete = (int)BoolType.True;//已删除
                }
                else
                {
                    orderSearch.OrderStatus = orderStatus;
                    orderSearch.IsDelete = (int)BoolType.False;//未删除
                }
            }
            var orderList = OrderBLL.SearchList(CurrentPage, PageSize, orderSearch, ref result);
            return result;
        }
    }
}