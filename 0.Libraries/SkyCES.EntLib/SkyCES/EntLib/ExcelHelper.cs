namespace SkyCES.EntLib
{
    using Microsoft.Office.Interop.Excel;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using NPOI.HSSF.UserModel;


    public class ExcelHelper
    {
        private Dictionary<int[], string> cellParameters = new Dictionary<int[], string>();
        private System.Data.DataTable dt = new System.Data.DataTable();
        private int left = 0;
        private object missing = System.Reflection.Missing.Value;
        private string outputFile = string.Empty;
        private int rows = 10;
        private string sheetPrefixName = "Sheet";
        private string templetFile = string.Empty;
        private int top = 0;
        private Dictionary<string, string> variableParameters = new Dictionary<string, string>();

        public ExcelHelper(string templetFilePath, string outputFilePath)
        {
            if (templetFilePath == string.Empty)
            {
                throw new Exception("Excel模板文件路径不能为空！");
            }
            if (outputFilePath == string.Empty)
            {
                throw new Exception("输出Excel文件路径不能为空！");
            }
            string path = outputFilePath.Substring(0, outputFilePath.LastIndexOf(@"\"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(templetFilePath))
            {
                throw new Exception("指定路径的Excel模板文件不存在！");
            }
            this.templetFile = templetFilePath;
            this.outputFile = outputFilePath;
        }

        public void DataTableToExcel()
        {
            int count = this.dt.Rows.Count;
            int sheetCount = this.GetSheetCount(count);
            DateTime now = DateTime.Now;

            FileStream file = new FileStream(this.templetFile, FileMode.Open, FileAccess.Read);//读入excel模板
            HSSFWorkbook workbook = new HSSFWorkbook(file);
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheet("Sheet1");

            for (int i = 1; i < sheetCount; i++)
            {
                workbook.CloneSheet(0);
            }
            this.FillData(workbook, sheetCount);

            try
            {
                FileStream fs = File.Create(this.outputFile);
                workbook.Write(fs);
                fs.Close();

            }
            catch (Exception exception)
            {
                throw exception;
            }



            //Application o = new ApplicationClass
            //{
            //    Visible = false
            //};
            //DateTime time2 = DateTime.Now;
            //Workbook workBook = o.Workbooks.Open(this.templetFile, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing);
            //Worksheet worksheet = (Worksheet)workBook.Sheets.get_Item(1);
            //for (int i = 1; i < sheetCount; i++)
            //{
            //    worksheet.Copy(this.missing, workBook.Worksheets[i]);
            //}
            //this.FillData(workBook, sheetCount);
            //worksheet.Activate();
            //try
            //{
            //    workBook.SaveAs(this.outputFile, this.missing, this.missing, this.missing, this.missing, this.missing, XlSaveAsAccessMode.xlExclusive, this.missing, this.missing, this.missing, this.missing, this.missing);
            //    workBook.Close(null, null, null);
            //    o.Workbooks.Close();
            //    o.Application.Quit();
            //    o.Quit();
            //    Marshal.ReleaseComObject(worksheet);
            //    Marshal.ReleaseComObject(workBook);
            //    Marshal.ReleaseComObject(o);
            //    worksheet = null;
            //    workBook = null;
            //    o = null;
            //    GC.Collect();
            //}
            //catch (Exception exception)
            //{
            //    throw exception;
            //}
            //finally
            //{
            //    Process[] processesByName = Process.GetProcessesByName("Excel");
            //    foreach (Process process in processesByName)
            //    {
            //        DateTime startTime = process.StartTime;
            //        if ((startTime > now) && (startTime < time2))
            //        {
            //            process.Kill();
            //        }
            //    }
            //}
        }

        protected virtual void FillData(HSSFWorkbook workBook, int sheetCount) { }
        //protected virtual void FillData(Workbook workBook, int sheetCount) { }
        private int GetSheetCount(int rowCount)
        {
            int num = rowCount % this.rows;
            if (num == 0)
            {
                return (rowCount / this.rows);
            }
            return (Convert.ToInt32((int)(rowCount / this.rows)) + 1);
        }

        //protected void SetCellParameters(Worksheet sheet)
        //{
        //    foreach (KeyValuePair<int[], string> pair in this.cellParameters)
        //    {
        //        try
        //        {
        //            sheet.Cells[(int) pair.Key[0], (int) pair.Key[1]] = pair.Value;
        //        }
        //        catch
        //        {
        //            throw new Exception(string.Concat(new object[] { "单元格（", (int) pair.Key[0], ",", (int) pair.Key[1], "）不存在" }));
        //        }
        //    }
        //}
        protected void SetCellParameters(HSSFSheet sheet)
        {
            foreach (KeyValuePair<int[], string> pair in this.cellParameters)
            {
                try
                {
                    int row = (int)pair.Key[0];
                    int cell = (int)pair.Key[1];
                    if (sheet.GetRow(row) == null)
                    {
                        sheet.CreateRow(row);
                    }
                    if (sheet.GetRow(row).GetCell(cell) == null)
                    {
                        sheet.GetRow(row).CreateCell(cell);
                    }
                    sheet.GetRow(row).GetCell(cell).SetCellValue(pair.Value);
                }
                catch
                {
                    throw new Exception(string.Concat(new object[] { "单元格（", (int)pair.Key[0], ",", (int)pair.Key[1], "）不存在" }));

                }
            }
        }

        //protected void SetVariableParameters(Worksheet sheet)
        //{
        //    foreach (KeyValuePair<string, string> pair in this.variableParameters)
        //    {
        //        try
        //        {
        //            ((TextBox) sheet.TextBoxes(pair.Key)).Text = pair.Value;
        //        }
        //        catch
        //        {
        //            throw new Exception("名称为" + pair.Key + "的单元格不存在");
        //        }
        //    }
        //}

        public Dictionary<int[], string> CellParameters
        {
            get
            {
                return this.cellParameters;
            }
            set
            {
                this.cellParameters = value;
            }
        }

        public System.Data.DataTable Dt
        {
            get
            {
                return this.dt;
            }
            set
            {
                this.dt = value;
            }
        }

        public int Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        public int Rows
        {
            get
            {
                return this.rows;
            }
            set
            {
                this.rows = value;
            }
        }

        public string SheetPrefixName
        {
            get
            {
                return this.sheetPrefixName;
            }
            set
            {
                this.sheetPrefixName = value;
            }
        }

        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }

        public Dictionary<string, string> VariableParameters
        {
            get
            {
                return this.variableParameters;
            }
            set
            {
                this.variableParameters = value;
            }
        }
    }
}

