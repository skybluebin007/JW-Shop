using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.Vbe.Interop;
//using Microsoft.Office.Interop.Excel;
using SkyCES.EntLib;
using NPOI.HSSF.UserModel;

namespace JWShop.Common
{
    public class OrderExcelHelper : ExcelHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templetFilePath"></param>
        /// <param name="outputFilePath"></param>
        public OrderExcelHelper(string templetFilePath, string outputFilePath)
            : base(templetFilePath, outputFilePath)
        { }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="sheetCount"></param>
        //protected override void FillData(Microsoft.Office.Interop.Excel.Workbook workBook, int sheetCount)
        //{
        //    int rowCount = Dt.Rows.Count;
        //    int colCount = Dt.Columns.Count;
        //    for (int i = 1; i <= sheetCount; i++)
        //    {
        //        int startRow = (i - 1) * Rows;
        //        int endRow = i * Rows;
        //        if (i == sheetCount)
        //        {
        //            endRow = rowCount;
        //        }
        //        Worksheet sheet = (Worksheet)workBook.Worksheets.get_Item(i);
        //        sheet.Name = this.SheetPrefixName + "-" + i.ToString();
        //        for (int j = 0; j < endRow - startRow; j++)
        //        {
        //            for (int k = 0; k < colCount; k++)
        //            {
        //                if (k < 2)
        //                {
        //                    sheet.Cells[Top + j, Left + k] = Dt.Rows[startRow + j][k].ToString();
        //                }
        //                else
        //                {
        //                    sheet.Cells[Top + j, Left + k + 2] = Dt.Rows[startRow + j][k].ToString();
        //                }
        //            }
        //        }
        //        SetCellParameters(sheet);
        //    }
        //}

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="sheetCount"></param>
        protected override void FillData(NPOI.HSSF.UserModel.HSSFWorkbook workBook, int sheetCount)
        {
            int rowCount = Dt.Rows.Count;
            int colCount = Dt.Columns.Count;
            for (int i = 0; i < sheetCount; i++)
            {
                int startRow = i * Rows;
                int endRow = (i + 1) * Rows;
                if ((i + 1) == sheetCount)
                {
                    endRow = rowCount;
                }
                HSSFSheet sheet = (HSSFSheet)workBook.GetSheetAt(i);
                workBook.SetSheetName(i, this.SheetPrefixName + "-" + i.ToString());
                for (int j = 0; j < endRow - startRow; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        if (k < 2)
                        {
                            sheet.GetRow(Top + j).GetCell(Left + k).SetCellValue(Dt.Rows[startRow + j][k].ToString());
                        }
                        else
                        {
                            sheet.GetRow(Top + j).GetCell(Left + k + 2).SetCellValue(Dt.Rows[startRow + j][k].ToString());
                        }
                    }
                }
                SetCellParameters(sheet);
            }
        }
    }
}
