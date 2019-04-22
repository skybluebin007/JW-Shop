using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Business;
using JWShop.Entity;
using JWShop.Common;
using SkyCES.EntLib;
using System.Threading.Tasks;
using System.Threading;

namespace JWShop.Web.Admin
{
    public partial class Distributor : JWShop.Page.AdminBasePage
    {
        //经销商状态，默认1--正常
        protected int distributor_Status = 1;
        //0 客户 1 试压   2 水工  3 经销商
        protected int usertype = 3;
        //所有用户
        private List<UserInfo> all_users = null;
        //分销商列表
        protected List<UserInfo> userList=null;
        //public Distributor()
        //{
        //    if (all_users == null)
        //    {
        //        all_users = UserBLL.ReadList();
        //    }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadDistributor", PowerCheckType.Single);
                TaskFactory tFactory = Task.Factory;
                List<Task> taskList = new List<Task>();
                taskList.Add(tFactory.StartNew(() =>
                {
                    if (all_users == null)
                    {
                        all_users = UserBLL.ReadList();
                    }
                }));
                //Task t1 =new Task(()=> {
                //    if (all_users == null)
                //    {
                //        all_users = UserBLL.ReadList();
                //    }
                //}) ;
                //t1.Start();
               
                string action = RequestHelper.GetQueryString<string>("Action");
                int id = RequestHelper.GetQueryString<int>("Id");
                if (id > 0)
                {
                    switch (action)
                    {                       
                        default:
                            break;
                    }
                }

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.UserName = HttpUtility.UrlEncode(RequestHelper.GetQueryString<string>("UserName"), System.Text.Encoding.UTF8);
                userSearch.Mobile = RequestHelper.GetQueryString<string>("Mobile");

                userSearch.UserType= RequestHelper.GetQueryString<int>("usertype") == int.MinValue ? 3 : RequestHelper.GetQueryString<int>("usertype");
                //分销商状态，默认1--正常
                userSearch.Distributor_Status = RequestHelper.GetQueryString<int>("distributor_Status")==int.MinValue?1: RequestHelper.GetQueryString<int>("distributor_Status");
                userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
                userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
                UserName.Text = HttpUtility.UrlDecode(userSearch.UserName, System.Text.Encoding.UTF8);
                Mobile.Text = userSearch.Mobile;
                StartRegisterDate.Text = RequestHelper.GetQueryString<string>("StartRegisterDate");
                EndRegisterDate.Text = RequestHelper.GetQueryString<string>("EndRegisterDate");

                usertype = userSearch.UserType;
                distributor_Status = userSearch.Distributor_Status;
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                userList = UserBLL.SearchList(CurrentPage, PageSize,userSearch,ref Count);


                //while (all_users == null)
                //{
                //    Thread.Sleep(100);
                //    continue;
                //}
                //等待任务完成
                Task.WaitAll(taskList.ToArray());

                //推荐人用户名
                userList.ForEach(k => k.Recommend_UserName = (all_users.FirstOrDefault(a => a.Id == k.Recommend_UserId) ?? new UserInfo()).UserName);
                userList.ForEach(k => k.Recommend_UserName = HttpUtility.UrlDecode(k.Recommend_UserName, System.Text.Encoding.UTF8));
                userList.ForEach(k => k.Recommend_UserName = !string.IsNullOrWhiteSpace(k.Recommend_UserName) ? k.Recommend_UserName : "无");

                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                userList.ForEach(k => k.UserName = HttpUtility.UrlDecode(k.UserName, System.Text.Encoding.UTF8));
                var distributorGradeList = DistributorGradeBLL.ReadList();
                userList.ForEach(k => k.Distributor_Grade_Title = (distributorGradeList.Where(d => k.Total_Commission >= d.Min_Amount && k.Total_Commission < d.Max_Amount).FirstOrDefault() ?? new DistributorGradeInfo()).Title);
                BindControl(userList, RecordList, MyPager);
            }
        }

        /// <summary>
        /// 激活按钮点击方法
        /// </summary>
        protected void ActiveButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("ActiveDistributor", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(',');
            if (ids.Length > 0)
            {
                foreach (int id in Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)))
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("[Distributor_Status]", 1);
                    UserBLL.UpdatePart(UserInfo.TABLENAME, dict, id);
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("ActiveRecord"), ShopLanguage.ReadLanguage("Distributor"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("ActiveOK"), RequestHelper.RawUrl);
            }
        }
       
        /// <summary>
        /// 冻结按钮点击方法
        /// </summary>
        protected void FreezeButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("FreezeDistributor", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(',');
            if (ids.Length > 0)
            {
                foreach (int id in Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)))
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("[Distributor_Status]", -1);
                    UserBLL.UpdatePart(UserInfo.TABLENAME, dict, id);
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("FreezeRecord"), ShopLanguage.ReadLanguage("Distributor"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("FreezeOK"), RequestHelper.RawUrl);
            }
        }
        /// <summary>
        /// 解除冻结按钮点击方法
        /// </summary>
        protected void UnFreezeButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UnFreezeDistributor", PowerCheckType.Single);
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(',');
            if (ids.Length > 0)
            {
                foreach (int id in Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)))
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("[Distributor_Status]", 1);
                    UserBLL.UpdatePart(UserInfo.TABLENAME, dict, id);
                }
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UnFreezeRecord"), ShopLanguage.ReadLanguage("Distributor"), string.Join(",", ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UnFreezeOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "Distributor.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "Mobile=" + Mobile.Text + "&";
            URL += "StartRegisterDate=" + StartRegisterDate.Text + "&";
            URL += "EndRegisterDate=" + EndRegisterDate.Text + "&";
            URL += "distributor_Status=" + RequestHelper.GetQueryString<string>("distributor_Status");
            ResponseHelper.Redirect(URL);
        }
        //protected void ExportButton_Click(object sender, EventArgs e)
        //{
        //    //if (Cookies.Admin.GetAdminName(true).ToLower() != "admin") {
        //    //    //只有admin才有权限导出会员数据
        //    //    ScriptHelper.Alert("没有权限");
        //    //}
        //    //else
        //    //{
        //    UserSearchInfo userSearch = new UserSearchInfo();
        //    userSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
        //    userSearch.Mobile = RequestHelper.GetQueryString<string>("Mobile");
        //    userSearch.Status = RequestHelper.GetQueryString<int>("Status");
        //    userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
        //    userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
        //    UserName.Text = userSearch.UserName;
        //    Mobile.Text = userSearch.Mobile;
        //    StartRegisterDate.Text = RequestHelper.GetQueryString<string>("StartRegisterDate");
        //    EndRegisterDate.Text = RequestHelper.GetQueryString<string>("EndRegisterDate");

        //    status = userSearch.Status;

        //    var userList = UserBLL.SearchList(new UserSearchInfo
        //    {
        //        UserName = userSearch.UserName,
        //        CardNo = userSearch.CardNo,
        //        Status = userSearch.Status
        //    });
        //    var data = userList.Take(1000).ToList();

        //    NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
        //    NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");
        //    sheet.DefaultColumnWidth = 18;
        //    sheet.CreateFreezePane(0, 1, 0, 1);

        //    NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
        //    row.Height = 20 * 20;
        //    row.CreateCell(0).SetCellValue("用户名");
        //    row.CreateCell(1).SetCellValue("性别");
        //    row.CreateCell(2).SetCellValue("手机");
        //    row.CreateCell(3).SetCellValue("邮箱");
        //    row.CreateCell(4).SetCellValue("固定电话");
        //    row.CreateCell(5).SetCellValue("QQ");
        //    row.CreateCell(6).SetCellValue("生日");
        //    row.CreateCell(7).SetCellValue("所在地");
        //    row.CreateCell(8).SetCellValue("注册时间");
        //    row.CreateCell(9).SetCellValue("登录次数");
        //    row.CreateCell(10).SetCellValue("最近登录");
        //    row.CreateCell(11).SetCellValue("会员状态");

        //    //设置表头格式
        //    var headFont = book.CreateFont();
        //    headFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
        //    headFont.FontHeightInPoints = 10;
        //    var headStyle = book.CreateCellStyle();
        //    headStyle.SetFont(headFont);
        //    headStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
        //    headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        //    headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        //    headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        //    headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        //    foreach (var cell in row.Cells)
        //    {
        //        cell.CellStyle = headStyle;
        //    }
        //    foreach (var entity in data)
        //    {
        //        NPOI.SS.UserModel.IRow dataRow = sheet.CreateRow(data.IndexOf(entity) + 1);
        //        dataRow.CreateCell(0).SetCellValue(entity.UserName);
        //        dataRow.CreateCell(1).SetCellValue(entity.Sex == 1 ? "男" : entity.Sex == 2 ? "女" : "保密");
        //        dataRow.CreateCell(2).SetCellValue(entity.Mobile);
        //        dataRow.CreateCell(3).SetCellValue(entity.Mobile);
        //        dataRow.CreateCell(4).SetCellValue(entity.Tel);
        //        dataRow.CreateCell(5).SetCellValue(entity.QQ);
        //        dataRow.CreateCell(6).SetCellValue(entity.Birthday);
        //        dataRow.CreateCell(7).SetCellValue(RegionBLL.ReadCityName(entity.RegionId) + entity.Address);
        //        dataRow.CreateCell(8).SetCellValue(entity.RegisterDate.ToString());
        //        dataRow.CreateCell(9).SetCellValue(entity.LoginTimes);
        //        dataRow.CreateCell(10).SetCellValue(entity.LastLoginDate.ToString());
        //        dataRow.CreateCell(11).SetCellValue(entity.Status == 1 ? "未验证" : entity.Status == 2 ? "正常" : "冻结");
        //        var style = book.CreateCellStyle();
        //        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
        //        foreach (var cell in dataRow.Cells)
        //        {
        //            cell.CellStyle = style;
        //        }
        //    }

        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    book.Write(ms);
        //    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
        //    Response.BinaryWrite(ms.ToArray());
        //    book = null;
        //    ms.Close();
        //    ms.Dispose();
        //    Response.End();
        //    //}
        //}

        /// <summary>
        /// 每页显示条数控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["AdminPageSize"] = AdminPageSize.Text;
            string URL = "Distributor.aspx?Action=search&";
            URL += "UserName=" + UserName.Text + "&";
            URL += "Mobile=" + Mobile.Text + "&";
            URL += "StartRegisterDate=" + StartRegisterDate.Text + "&";
            URL += "EndRegisterDate=" + EndRegisterDate.Text + "&";
            URL += "distributor_Status=" + RequestHelper.GetQueryString<string>("distributor_Status");
            ResponseHelper.Redirect(URL);
        }
    }
}