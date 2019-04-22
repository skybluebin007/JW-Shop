using NPOI.SS.UserModel;
using SkyCES.EntLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace JWShop.Common
{
   public class ExcelToDataTable
    {
        public static DataTable Convert(string filePath, out bool isSuccess, out string msg)
        {
            DataTable data = new DataTable();
            isSuccess = false;
            msg = "";

            if (string.IsNullOrEmpty(filePath))
            {
                msg = "请先上传Excel文件";
                return data;
            }
            if (!filePath.EndsWith(".xls"))
            {
                FileHelper.DeleteFile(new List<string> { filePath });

                msg = "请选择模版文件进行上传";
                return data;
            }

            data = Convert(filePath);
            FileHelper.DeleteFile(new List<string> { filePath });

            if (data == null)
            {
                msg = "转换失败";
                return data;
            }
            if (data.Rows.Count < 1)
            {
                msg = "上传文件中没有数据";
                return data;
            }

            isSuccess = true;
            return data;
        }

        private static DataTable Convert(string filePath)
        {
            using (FileStream stream = new FileStream(ServerHelper.MapPath(filePath), FileMode.Open))
            {
                string sheetName = "Sheet1";
                DataTable data = new DataTable();
                ISheet sheet = null;
                int startRow = 0;
                try
                {
                    IWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(stream);

                    if (sheetName != null)
                    {
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                        {
                            sheet = workbook.GetSheetAt(0);
                        }
                    }
                    else
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                    if (sheet != null)
                    {
                        IRow firstRow = sheet.GetRow(0);
                        int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;

                        //最后一列的标号
                        int rowCount = sheet.LastRowNum;
                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue; //没有数据的行默认是null　　　　　　　

                            DataRow dataRow = data.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            data.Rows.Add(dataRow);
                        }
                    }
                    return data;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
