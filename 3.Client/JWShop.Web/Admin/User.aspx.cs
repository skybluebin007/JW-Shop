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
    public partial class User : JWShop.Page.AdminBasePage
    {
        protected int status = 0;

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
                            UserBLL.Delete(id);
                            AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("User"), id);
                            break;
                        default:
                            break;
                    }
                }

                //会员类型
                usertype.DataSource = EnumHelper.ReadEnumList<UserType>();
                usertype.DataTextField = "ChineseName";
                usertype.DataValueField = "Value";
                usertype.DataBind();
                ListItem lt = new ListItem("--所有会员--","");
                usertype.Items.Insert(0,lt);

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.UserName =HttpUtility.UrlEncode( RequestHelper.GetQueryString<string>("UserName"), System.Text.Encoding.UTF8);
                userSearch.Mobile = RequestHelper.GetQueryString<string>("Mobile");
                userSearch.Status = RequestHelper.GetQueryString<int>("Status");
                userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
                userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
                userSearch.UserType= RequestHelper.GetQueryString<int>("Type");

                usertype.SelectedValue = userSearch.UserType.ToString();
                UserName.Text =HttpUtility.UrlDecode(userSearch.UserName,System.Text.Encoding.UTF8);
                Mobile.Text = userSearch.Mobile;
                StartRegisterDate.Text = RequestHelper.GetQueryString<string>("StartRegisterDate");
                EndRegisterDate.Text = RequestHelper.GetQueryString<string>("EndRegisterDate");

                status = userSearch.Status;
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                var userList = UserBLL.SearchListAndUserGrade(CurrentPage, PageSize, userSearch,ref Count);
                //Count = userList.Count;
               
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                userList.ForEach(k => k.UserName = System.Web.HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8));
                //userList = userList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                BindControl(userList, RecordList, MyPager);
            }
        }

        /// <summary>
        /// 激活按钮点击方法
        /// </summary>
        protected void ActiveButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("ActiveUser", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(',');
            if (ids.Length > 0)
            {
                foreach (int id in Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)))
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Status", (int)JWShop.Entity.UserStatus.Normal);
                    UserBLL.UpdatePart(UserInfo.TABLENAME, dict, id);
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("ActiveRecord"), ShopLanguage.ReadLanguage("User"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("ActiveOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 未激活按钮点击方法
        /// </summary>
        protected void UnActiveButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UnActiveUser", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(',');
            if (ids.Length > 0)
            {
                foreach (int id in Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)))
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Status", (int)JWShop.Entity.UserStatus.NoCheck);
                    UserBLL.UpdatePart(UserInfo.TABLENAME, dict, id);
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UnActiveRecord"), ShopLanguage.ReadLanguage("User"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UnActiveOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 冻结按钮点击方法
        /// </summary>
        protected void FreezeButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("FreezeUser", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(',');
            if (ids.Length > 0)
            {
                foreach (int id in Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)))
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Status", (int)JWShop.Entity.UserStatus.Frozen);
                    UserBLL.UpdatePart(UserInfo.TABLENAME, dict, id);
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("FreezeRecord"), ShopLanguage.ReadLanguage("User"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("FreezeOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 解除冻结按钮点击方法
        /// </summary>
        protected void UnFreezeButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UnFreezeUser", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(',');
            if (ids.Length > 0)
            {
                foreach (int id in Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)))
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("Status", (int)JWShop.Entity.UserStatus.Normal);
                    UserBLL.UpdatePart(UserInfo.TABLENAME, dict, id);
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UnFreezeRecord"), ShopLanguage.ReadLanguage("User"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UnFreezeOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "User.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "Mobile=" + Mobile.Text + "&";
            URL += "StartRegisterDate=" + StartRegisterDate.Text + "&";
            URL += "EndRegisterDate=" + EndRegisterDate.Text + "&";
            URL += "Type=" + usertype.Text + "&";
            URL += "Status=" + RequestHelper.GetQueryString<string>("Status");
            ResponseHelper.Redirect(URL);
        }
        protected void ExportButton_Click(object sender, EventArgs e)
        {
            //if (Cookies.Admin.GetAdminName(true).ToLower() != "admin") {
            //    //只有admin才有权限导出会员数据
            //    ScriptHelper.Alert("没有权限");
            //}
            //else
            //{
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
                userSearch.Mobile = RequestHelper.GetQueryString<string>("Mobile");
                userSearch.Status = RequestHelper.GetQueryString<int>("Status");
                userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
                userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
                UserName.Text = userSearch.UserName;
                Mobile.Text = userSearch.Mobile;
                StartRegisterDate.Text = RequestHelper.GetQueryString<string>("StartRegisterDate");
                EndRegisterDate.Text = RequestHelper.GetQueryString<string>("EndRegisterDate");

                status = userSearch.Status;

                var userList = UserBLL.SearchList(new UserSearchInfo
                {
                    UserName = userSearch.UserName,
                    CardNo = userSearch.CardNo,
                    Status = userSearch.Status
                });
                var data = userList.Take(1000).ToList();

                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
                sheet.DefaultColumnWidth = 18;
                sheet.CreateFreezePane(0, 1, 0, 1);

                NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
                row.Height = 20 * 20;
                row.CreateCell(0).SetCellValue("用户名");
                row.CreateCell(1).SetCellValue("性别");
                row.CreateCell(2).SetCellValue("手机");
                row.CreateCell(3).SetCellValue("邮箱");
                row.CreateCell(4).SetCellValue("固定电话");
                row.CreateCell(5).SetCellValue("QQ");
                row.CreateCell(6).SetCellValue("生日");
                row.CreateCell(7).SetCellValue("所在地");
                row.CreateCell(8).SetCellValue("注册时间");
                row.CreateCell(9).SetCellValue("登录次数");
                row.CreateCell(10).SetCellValue("最近登录");
                row.CreateCell(11).SetCellValue("会员状态");

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
                    dataRow.CreateCell(0).SetCellValue(entity.UserName);
                    dataRow.CreateCell(1).SetCellValue(entity.Sex == 1 ? "男" : entity.Sex == 2 ? "女" : "保密");
                    dataRow.CreateCell(2).SetCellValue(entity.Mobile);
                    dataRow.CreateCell(3).SetCellValue(entity.Mobile);
                    dataRow.CreateCell(4).SetCellValue(entity.Tel);
                    dataRow.CreateCell(5).SetCellValue(entity.QQ);
                    dataRow.CreateCell(6).SetCellValue(entity.Birthday);
                    dataRow.CreateCell(7).SetCellValue(RegionBLL.ReadCityName(entity.RegionId) + entity.Address);
                    dataRow.CreateCell(8).SetCellValue(entity.RegisterDate.ToString());
                    dataRow.CreateCell(9).SetCellValue(entity.LoginTimes);
                    dataRow.CreateCell(10).SetCellValue(entity.LastLoginDate.ToString());
                    dataRow.CreateCell(11).SetCellValue(entity.Status == 1 ? "未验证" : entity.Status == 2 ? "正常" : "冻结");
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
            //}
        }

        /// <summary>
        /// 每页显示条数控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["AdminPageSize"] = AdminPageSize.Text;
            string URL = "User.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "Mobile=" + Mobile.Text + "&";
            URL += "StartRegisterDate=" + StartRegisterDate.Text + "&";
            URL += "EndRegisterDate=" + EndRegisterDate.Text + "&";
            URL += "Status=" + RequestHelper.GetQueryString<string>("Status");
            ResponseHelper.Redirect(URL);
        }

        protected string getusername(string userid)
        {
            if (userid == "0")
                return "无上级";
            UserInfo model = new UserInfo();
            model = UserBLL.Read(int.Parse(userid));
            if (model.Id > 0)
            {
                return model.UserName;
            }

            return "无上级";
        }

    }
}