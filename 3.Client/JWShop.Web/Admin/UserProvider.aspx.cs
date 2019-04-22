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
    public partial class UserProvider : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadUser", PowerCheckType.Single);
                string action = RequestHelper.GetQueryString<string>("Action");
                int id = RequestHelper.GetQueryString<int>("Id");
                if (id > 0)
                {
                    switch (action)
                    {
                        case "Delete":
                            CheckAdminPower("DeleteUser", PowerCheckType.Single);
                            if (ProductBLL.CountByShop(id) > 0)
                            {
                                ScriptHelper.Alert("该供应商存在相关产品，不能删除。");
                            }
                            else
                            {
                                UserBLL.Delete(id);
                                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("User"), id);
                            }
                            break;
                        default:
                            break;
                    }
                }

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
                userSearch.ProviderNo = RequestHelper.GetQueryString<string>("ProviderNo");
                UserName.Text = userSearch.UserName;
                ProviderNo.Text = userSearch.ProviderNo;

                var userList = UserBLL.SearchList(new UserSearchInfo { UserName = userSearch.UserName, ProviderNo = userSearch.ProviderNo, UserType = (int)UserType.Provider });
                Count = userList.Count;
                userList = userList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                BindControl(userList, RecordList, MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "UserProvider.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "ProviderNo=" + ProviderNo.Text;
            ResponseHelper.Redirect(URL);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var userList = UserBLL.SearchList(new UserSearchInfo { UserName = UserName.Text, ProviderNo = ProviderNo.Text, UserType = (int)UserType.Provider });

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
            sheet.DefaultColumnWidth = 18;
            sheet.CreateFreezePane(0, 1, 0, 1);

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.Height = 20 * 20;
            row.CreateCell(0).SetCellValue("供应商编号");
            row.CreateCell(1).SetCellValue("用户名");
            row.CreateCell(2).SetCellValue("电子邮箱");
            row.CreateCell(3).SetCellValue("公司名称");
            row.CreateCell(4).SetCellValue("对公银行开户行");
            row.CreateCell(5).SetCellValue("税务号");
            row.CreateCell(6).SetCellValue("法人代表");
            row.CreateCell(7).SetCellValue("联系人");
            row.CreateCell(8).SetCellValue("联系电话");
            row.CreateCell(9).SetCellValue("传真");
            row.CreateCell(10).SetCellValue("经营品牌");
            row.CreateCell(11).SetCellValue("经营品类");
            row.CreateCell(12).SetCellValue("结算方式");
            row.CreateCell(13).SetCellValue("结算周期");
            row.CreateCell(14).SetCellValue("物流配送");
            row.CreateCell(15).SetCellValue("售后服务");
            row.CreateCell(16).SetCellValue("退换货保障");
            row.CreateCell(17).SetCellValue("详细地址");

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

            foreach (var user in userList)
            {
                NPOI.SS.UserModel.IRow dataRow = sheet.CreateRow(userList.IndexOf(user) + 1);
                dataRow.CreateCell(0).SetCellValue(user.ProviderNo);
                dataRow.CreateCell(1).SetCellValue(user.UserName);
                dataRow.CreateCell(2).SetCellValue(user.Email);
                dataRow.CreateCell(3).SetCellValue(user.ProviderName);
                dataRow.CreateCell(4).SetCellValue(user.ProviderBankNo);
                dataRow.CreateCell(5).SetCellValue(user.ProviderTaxRegistration);
                dataRow.CreateCell(6).SetCellValue(user.ProviderCorporate);
                dataRow.CreateCell(7).SetCellValue(user.ProviderLinker);
                dataRow.CreateCell(8).SetCellValue(user.ProviderLinkerTel);
                dataRow.CreateCell(9).SetCellValue(user.ProviderFax);
                dataRow.CreateCell(10).SetCellValue(user.ProviderOperateBrand);
                dataRow.CreateCell(11).SetCellValue(user.ProviderOperateClass);
                dataRow.CreateCell(12).SetCellValue(user.ProviderAccount);
                dataRow.CreateCell(13).SetCellValue(user.ProviderAccountCycle);
                dataRow.CreateCell(14).SetCellValue(user.ProviderShipping);
                dataRow.CreateCell(15).SetCellValue(user.ProviderService);
                dataRow.CreateCell(16).SetCellValue(user.ProviderEnsure);
                dataRow.CreateCell(17).SetCellValue(RegionBLL.RegionNameList(user.RegionId) + ", " + user.ProviderAddress);
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