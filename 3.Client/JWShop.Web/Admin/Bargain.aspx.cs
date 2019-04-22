using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JWShop.Web.Admin
{
    public partial class Bargain : JWShop.Page.AdminBasePage
    {
        protected int status = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadBargain", PowerCheckType.Single);
                string Action = RequestHelper.GetQueryString<string>("Action");
                if (Action == "Delete")
                {
                    Delete();
                }
                if(Action== "ShutDown")
                {
                    ShutDown();
                }
                List<BargainInfo> dataList = BargainBLL.SearchBargainList(CurrentPage,PageSize,new BargainSearch(),ref Count);
                foreach (var bargain in dataList)
                {
                    int total_bargain = 0;
                    //到期，将状态置为“已结束”
                    if (DateTime.Now > bargain.EndDate && bargain.Status!=(int)Bargain_Status.End)
                    {
                        //异步 改变“砍价活动状态”
                        TimeExpire(bargain.Id);
                    }
                    var bargainDetails = BargainDetailsBLL.ReadByBargainId(bargain.Id);
                    foreach (var bdt in bargainDetails)
                    {
                        var bargain_orders = BargainOrderBLL.SearchBargainOrderList(new BargainOrderSearch { BargainDetailsId = bdt.Id });
                        foreach (var bo in bargain_orders)
                        {
                            total_bargain += RecordingBLL.SearchRecordingList(new RecordingSearch { BOrderId = bo.Id }).Where(k=>k.UserId>0).Count();
                        }

                    }
                    //本次砍价总砍次数
                    bargain.Bargain_Records_Total = total_bargain;
                }
                BindControl(dataList, RecordList, MyPager);

            }
        }
        /// <summary>
        /// 到期：状态置为 “已结束”
        /// </summary>
        /// <param name="id"></param>
        private async void TimeExpire(int id)
        {

            await Task.Run(() => {
                #region 结束 事务：将未支付成功的砍价全部置为“砍价失败”，原“活动已结束，砍价失败”
                BargainBLL.ChangeBargainStatus(id, (int)Bargain_Status.End);
                #endregion
            });
        }

     
        /// <summary>
        /// 关闭
        /// </summary>
        private void ShutDown()
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            if (id > 0)
            {
                var bargain = BargainBLL.ReadBargain(id);
                if (bargain.Id > 0 && bargain.Status!=0)
                {
                    #region 关闭 事务：将未支付成功的砍价全部置为“砍价失败”，原“活动已取消，砍价失败”
                    BargainBLL.ChangeBargainStatus(id,(int)Bargain_Status.ShutDown);
                    #endregion
                    //Dictionary<string, object> dict = new Dictionary<string, object>();
                    //dict.Add("[Status]", 0);
                    //BargainBLL.UpdatePart("[Bargain]", dict, id);
                }
                else
                {
                    ScriptHelper.Alert("操作不合法");
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        private void Delete()
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            if (id > 0)
            {
                var bargain = BargainBLL.ReadBargain(id);
                if (bargain.Id > 0 && bargain.EndDate >= DateTime.Now && bargain.StartDate <= DateTime.Now)
                {
                    ScriptHelper.Alert("活动进行中，不能删除");
                }
                else
                {
                    BargainBLL.Delete(id);
                }
            }
        }
        protected string ShowHref(object id)
        {
            string result = string.Empty;
            int _id = 0;
            int.TryParse(id.ToString(), out _id);
            if (_id > 0)
            {
                var bargain = BargainBLL.ReadBargain(_id);
                if (bargain.Id > 0 && bargain.StartDate > DateTime.Now)
                {//未开始                   
                    result = "<a href = \"BargainAdd.aspx?ID=" + id + "\"  class=\"ig-colink\">修改</a> ";
                    result += "<a href = \"Bargain.aspx?ID=" + id + "&Action=Delete\"  class=\"ig-colink\" onclick=\"javascript: return(window.confirm('确定要删除吗'))\">删除</a> ";
                }
                if (bargain.Id > 0 && bargain.StartDate <= DateTime.Now && bargain.EndDate >= DateTime.Now)
                {//进行中
                    result = "<a href = \"BargainAdd.aspx?ID=" + id + "\"  class=\"ig-colink\">查看</a> ";
                    result += " <a onclick = \"SelectRecording(" + id + ");\" class=\"ig-colink\">砍价记录</a> ";
                    //有效的活动可以关闭
                    if (bargain.Status != 0)
                    {
                        result += "<a href = \"Bargain.aspx?ID=" + id + "&Action=ShutDown\"  class=\"ig-colink\" onclick=\"javascript:return(window.confirm('确定要关闭吗，关闭后不可恢复'))\">关闭</a> ";
                    }
                }
                if (bargain.Id > 0 && bargain.EndDate < DateTime.Now)
                {// 已结束
                    result = "<a href = \"BargainAdd.aspx?ID=" + id + "\"  class=\"ig-colink\">查看</a> ";
                    result += " <a onclick = \"SelectRecording(" + id + ");\" class=\"ig-colink\">砍价记录</a> ";
                    result += "<a href = \"Bargain.aspx?ID=" + id + "&Action=Delete\"  class=\"ig-colink\" onclick=\"javascript:return(window.confirm('确定要删除吗'))\">删除</a> ";
                }
                //
                //                  
            }
            return result;
        }
    }
}