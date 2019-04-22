using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class SaleStop : JWShop.Page.AdminBasePage
    {
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("StatisticsSale", PowerCheckType.Single);

                foreach (ProductClassInfo productClass in ProductClassBLL.ReadNamedList())
                {
                    ClassID.Items.Add(new ListItem(productClass.Name, "|" + productClass.Id + "|"));
                }
                ClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

                BrandID.DataSource = ProductBrandBLL.ReadList();
                BrandID.DataTextField = "Name";
                BrandID.DataValueField = "Id";
                BrandID.DataBind();
                BrandID.Items.Insert(0, new ListItem("所有品牌", string.Empty));

                ClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                BrandID.Text = RequestHelper.GetQueryString<string>("BrandID");
                Name.Text = RequestHelper.GetQueryString<string>("Name");

                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.IsSale = (int)BoolType.True;
                productSearch.Name = RequestHelper.GetQueryString<string>("Name");
                productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
                productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");

                productList = ProductBLL.SearchList(CurrentPage, PageSize, productSearch, ref Count);
                dt = StatisticsSaleStop(productList);
                BindControl(MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "SaleStop.aspx?Action=search&";
            URL += "Name=" + Name.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text;
            ResponseHelper.Redirect(URL);
        }
        /// <summary>
        /// 读取滞销信息
        /// </summary>
        protected DataTable StatisticsSaleStop(List<ProductInfo> productList)
        {
            string productIDList = string.Empty;
            foreach (ProductInfo product in productList)
            {
                if (productIDList == string.Empty)
                {
                    productIDList = product.Id.ToString();
                }
                else
                {
                    productIDList += "," + product.Id.ToString();
                }
            }
            return OrderBLL.StatisticsSaleStop(productIDList);
        }
        /// <summary>
        /// 读取一条滞销信息
        /// </summary>
        protected DataRow ReadSaleStop(int productID, DataTable dt)
        {
            DataRow dr = null;
            foreach (DataRow temp in dt.Rows)
            {
                if (temp["ProductID"].ToString() == productID.ToString())
                {
                    dr = temp;
                    break;
                }
            }
            return dr;
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsSale = (int)BoolType.True;
            productSearch.Name = ShopCommon.ConvertToT<string>(Name.Text);
            productSearch.ClassId = ShopCommon.ConvertToT<string>(ClassID.Text);
            productSearch.BrandId = ShopCommon.ConvertToT<int>(BrandID.Text);

            var dataProduct = ProductBLL.SearchList(1, 1000, productSearch, ref Count);
            var dataStatis = StatisticsSaleStop(dataProduct);

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
            sheet.DefaultColumnWidth = 18;
            sheet.CreateFreezePane(0, 2, 0, 2);

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.Height = 20 * 20;
            row.CreateCell(0).SetCellValue("商品名称");
            row.CreateCell(1).SetCellValue("最近一次销售");
            row.CreateCell(2).SetCellValue("");
            row.CreateCell(3).SetCellValue("");
            row.CreateCell(4).SetCellValue("滞销天数");

            NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(1);
            row2.Height = 20 * 20;
            row2.CreateCell(0).SetCellValue("");
            row2.CreateCell(1).SetCellValue("订单号");
            row2.CreateCell(2).SetCellValue("销售数量");
            row2.CreateCell(3).SetCellValue("日期");
            row2.CreateCell(4).SetCellValue("");

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 4, 4));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 3));

            //设置表头格式
            var headFont = book.CreateFont();
            headFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            headFont.FontHeightInPoints = 10;
            var headStyle = book.CreateCellStyle();
            headStyle.SetFont(headFont);
            headStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            foreach (var cell in row.Cells)
            {
                cell.CellStyle = headStyle;
            }
            foreach (var cell in row2.Cells)
            {
                cell.CellStyle = headStyle;
            }

            foreach (var product in dataProduct)
            {
                DataRow dr = ReadSaleStop(product.Id, dataStatis);

                NPOI.SS.UserModel.IRow dataRow = sheet.CreateRow(dataProduct.IndexOf(product) + 2);
                dataRow.CreateCell(0).SetCellValue(product.Name);
                if (dr != null)
                {
                    dataRow.CreateCell(1).SetCellValue(Convert.ToString(dr["OrderNumber"]));
                    dataRow.CreateCell(2).SetCellValue(Convert.ToString(dr["BuyCount"]));
                    dataRow.CreateCell(3).SetCellValue(Convert.ToString(dr["AddDate"]));
                    dataRow.CreateCell(4).SetCellValue(Convert.ToString((DateTime.Now.Date - Convert.ToDateTime(dr["AddDate"]).Date).Days));
                }
                else
                {
                    dataRow.CreateCell(1).SetCellValue("");
                    dataRow.CreateCell(2).SetCellValue("");
                    dataRow.CreateCell(3).SetCellValue("");
                    dataRow.CreateCell(4).SetCellValue("");
                }

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
    }
}