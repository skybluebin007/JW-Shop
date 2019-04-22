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
    public partial class ProductStorage : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("StatisticsProduct", PowerCheckType.Single);

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
                StorageAnalyse.Text = RequestHelper.GetQueryString<string>("StorageAnalyse");

                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.IsSale = (int)BoolType.True;
                productSearch.Name = RequestHelper.GetQueryString<string>("Name");
                productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
                productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
                productSearch.StorageAnalyse = RequestHelper.GetQueryString<int>("StorageAnalyse");
                List<ProductInfo> productList = ProductBLL.SearchList(CurrentPage, PageSize, productSearch, ref Count);
                BindControl(productList, RecordList, MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "ProductStorage.aspx?Action=search&";
            URL += "Name=" + Name.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StorageAnalyse=" + StorageAnalyse.Text;
            ResponseHelper.Redirect(URL);
        }

        /// <summary>
        /// 显示颜色
        /// </summary>
        protected string ShowColor(int lowerCount, int storageCount, int importActualStorageCount, int upperCount)
        {
            int tempStorageCount = storageCount;
            if (ShopConfig.ReadConfigInfo().ProductStorageType == (int)ProductStorageType.ImportStorageSystem)
            {
                tempStorageCount = importActualStorageCount;
            }
            string content = string.Empty;
            if (tempStorageCount <= lowerCount)
            {
                content = "#FF0000";
            }
            else
            {
                content = "#33dd33";
            }
            return content;
        }
        /// <summary>
        /// 显示库存数量
        /// </summary>
        protected string ShowStorageCount(int storageCount, int importActualStorageCount)
        {
            int tempStorageCount = storageCount;
            if (ShopConfig.ReadConfigInfo().ProductStorageType == (int)ProductStorageType.ImportStorageSystem)
            {
                tempStorageCount = importActualStorageCount;
            }
            return tempStorageCount.ToString();
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsSale = (int)BoolType.True;
            productSearch.Name = ShopCommon.ConvertToT<string>(Name.Text);
            productSearch.ClassId = ShopCommon.ConvertToT<string>(ClassID.Text);
            productSearch.BrandId = ShopCommon.ConvertToT<int>(BrandID.Text);
            productSearch.StorageAnalyse = ShopCommon.ConvertToT<int>(StorageAnalyse.Text);
            var data = ProductBLL.SearchList(1, 1000, productSearch, ref Count);

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
            sheet.DefaultColumnWidth = 18;
            sheet.CreateFreezePane(0, 1, 0, 1);

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.Height = 20 * 20;
            row.CreateCell(0).SetCellValue("商品名称");
            row.CreateCell(1).SetCellValue("剩余库存量");
            row.CreateCell(2).SetCellValue("库存下限");
            row.CreateCell(3).SetCellValue("库存上限");

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

            foreach (var entity in data)
            {
                NPOI.SS.UserModel.IRow dataRow = sheet.CreateRow(data.IndexOf(entity) + 1);
                dataRow.CreateCell(0).SetCellValue(entity.Name);
                dataRow.CreateCell(1).SetCellValue(ShowStorageCount(entity.TotalStorageCount - entity.SendCount, entity.ImportActualStorageCount));
                dataRow.CreateCell(2).SetCellValue(Convert.ToString(entity.LowerCount));
                dataRow.CreateCell(3).SetCellValue(Convert.ToString(entity.UpperCount));

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
