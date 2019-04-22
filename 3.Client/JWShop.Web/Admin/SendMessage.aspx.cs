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

namespace JWShop.Web.Admin
{
    public partial class SendMessage : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                switch (RequestHelper.GetQueryString<string>("Action"))
                {
                    case "delete":
                        CheckAdminPower("DeleteMessage", PowerCheckType.Single);
                        int id = RequestHelper.GetQueryString<int>("ID");
                        if (id > 0) {
                            ReceiveMessageBLL.Delete(id);
                            AdminLogBLL.Add(string.Format("删除已发送消息(ID:{0})",id));
                            ScriptHelper.Alert("删除成功", "SendMessage.aspx");
                        }
                        break;
                    case "search":
                    default:
                        CheckAdminPower("ReadMessage", PowerCheckType.Single);

                        Title.Text = RequestHelper.GetQueryString<string>("Title");
                        StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                        EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");

                        ReceiveMessageSearchInfo searchInfo = new ReceiveMessageSearchInfo();
                        searchInfo.Title = RequestHelper.GetQueryString<string>("Title");
                        searchInfo.StartDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                        searchInfo.EndDate = RequestHelper.GetQueryString<DateTime>("EndAddDate");
                        BindControl(ReceiveMessageBLL.SearchList(CurrentPage, PageSize, searchInfo, ref Count), RecordList, MyPager);
                        break;
                }
            }
        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteMessage", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            string[] ids = RequestHelper.GetIntsForm("SelectID").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                ReceiveMessageBLL.Delete(Array.ConvertAll<string, int>(ids, k => Convert.ToInt32(k)));
                AdminLogBLL.Add(string.Format("删除了ID为{0}的消息",ids));
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "SendMessage.aspx?Action=search&";
            URL += "Title=" + Title.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text;
            ResponseHelper.Redirect(URL);
        }
    }
}