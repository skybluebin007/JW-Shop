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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace JWShop.Web.Admin
{
    public partial class Vote : JWShop.Page.AdminBasePage
    {
        /// <summary>
        /// 投票一级分类
        /// </summary>
        protected List<VoteInfo> topVoteList = new List<VoteInfo>(); 
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadVote", PowerCheckType.Single);
              
                //BindControl(VoteBLL.ReadNamedList(), RecordList, MyPager);
            }
        }

        /// <summary>
        /// 删除按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteVote", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");           
            if (deleteID != string.Empty)
            {
                int[] ids = Array.ConvertAll<string, int>(deleteID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k =>Convert.ToInt32(k));
                VoteBLL.DeleteVote(ids);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Vote"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), "Vote.aspx");
            }
        }
        /// <summary>
        /// 投票记录导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnTOExcel_Click(object sender, EventArgs e)
        {
            int count = 0;
            List<VoteItemInfo> voteItemList = VoteItemBLL.ReadVoteItemList(1, 500, ref count, "VoteCount");
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("投票结果统计");
            //列宽

            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 20 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);

            // 第一行
            IRow row = sheet.CreateRow(0);
            row.Height = 400;//行高

            row.CreateCell(0).SetCellValue("姓名");

            row.CreateCell(1).SetCellValue("得票数");
            row.CreateCell(2).SetCellValue("类型");
            row.CreateCell(3).SetCellValue("奖项名称");
            row.CreateCell(4).SetCellValue("个人宣言");

            ICellStyle style1 = book.CreateCellStyle();
            //背景色
            style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            style1.FillPattern = FillPattern.SolidForeground;
            //水平对齐
            style1.Alignment = HorizontalAlignment.Center;
            //垂直对齐
            style1.VerticalAlignment = VerticalAlignment.Center;
            //字体加粗
            IFont f = book.CreateFont();
            f.Boldweight = (short)FontBoldWeight.Bold;
            style1.SetFont(f);
            for (int x = 0; x <= 4; x++) { row.Cells[x].CellStyle = style1; }


            //循环插入数据行
            ICellStyle style2 = book.CreateCellStyle();
            style2.Alignment = HorizontalAlignment.Center;
            style2.VerticalAlignment = VerticalAlignment.Center;



            int rowNo = 1;
            foreach (VoteItemInfo tmpItem in voteItemList)
            {

                IRow TmpRow = sheet.CreateRow(rowNo);
                TmpRow.Height = 300;//行高
                //名称
                TmpRow.CreateCell(0).SetCellType(CellType.String);
                TmpRow.CreateCell(0).SetCellValue(tmpItem.ItemName);
                TmpRow.Cells[0].CellStyle = style2;

                //票数

                TmpRow.CreateCell(1).SetCellType(CellType.Numeric);
                TmpRow.CreateCell(1).SetCellValue(tmpItem.VoteCount);
                TmpRow.Cells[1].CellStyle = style2;
                //类型
                TmpRow.CreateCell(2).SetCellValue(VoteBLL.ReadVote(VoteBLL.GetTopClassID(tmpItem.VoteID)).Title);
                TmpRow.Cells[2].CellStyle = style2;
                //奖项名称
                TmpRow.CreateCell(3).SetCellValue(tmpItem.Solution);
                TmpRow.Cells[3].CellStyle = style2;
                //个人宣言
                TmpRow.CreateCell(4).SetCellValue(tmpItem.Department);
                TmpRow.Cells[4].CellStyle = style2;

                rowNo++;
            }

            //锁定
            sheet.CreateFreezePane(0, 1, 1, 1);

            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }
    }
}