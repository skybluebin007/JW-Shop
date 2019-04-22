using JWShop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JWShop.Business;
using JWShop.Common;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class WithdrawDetail : JWShop.Page.AdminBasePage
    {
        protected WithdrawInfo entity = new WithdrawInfo();
        protected List<WithdrawOperateInfo> actions = new List<WithdrawOperateInfo>();
        protected UserInfo distributor = new UserInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("ID");
                entity = WithdrawBLL.Read(id);
                //当前分销商
                distributor = UserBLL.Read(entity.Distributor_Id);
                if (entity.Id <= 0 || distributor.Id<=0)
                {
                    ScriptHelper.Alert("参数错误", RequestHelper.RawUrl);
                }
              
                entity.UserName =HttpUtility.UrlDecode(distributor.UserName,System.Text.Encoding.UTF8);
                entity.RealName = distributor.RealName;
                entity.Mobile = distributor.Mobile;

                //审核操作记录
                actions = Withdraw_OperateBLL.ReadListByWithdraw(entity.Id);
            }
        }
        /// <summary>
        /// 提现完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApproveButton_Click(object sender,EventArgs e)
        {
            this.WithdrawOperate(Withdraw_Operate.Pass);
        }
        /// <summary>
        /// 拒绝提现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RejectButton_Click(object sender, EventArgs e)
        {
            this.WithdrawOperate(Withdraw_Operate.Reject);
        }
        private void WithdrawOperate(Withdraw_Operate operate)
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            var _entity = WithdrawBLL.Read(id);
            if (_entity.Id <= 0)
            {
                ScriptHelper.Alert("记录不存在", RequestHelper.RawUrl);
            }
            if (_entity.Status != (int)Withdraw_Status.Apply)
            {
                ScriptHelper.Alert("当前状态不能执行此操作", RequestHelper.RawUrl);
            }
            #region 提现完成操作  判断分销商状态是否正常，剩余可提现额度是否满足本次提现
            if (operate==Withdraw_Operate.Pass)
            {
                distributor = UserBLL.Read(_entity.Distributor_Id);
                if (distributor.Distributor_Status != (int)Distributor_Status.Normal)
                {
                    ScriptHelper.Alert("当前分销商已冻结或待审核，不能执行此操作", RequestHelper.RawUrl);
                }
                if (distributor.Total_Commission - distributor.Total_Withdraw < 0)
                {
                    ScriptHelper.Alert("当前提现金额已超出分销商剩余提现额度，不能执行此操作", RequestHelper.RawUrl);
                }
            }
            #endregion
            if (WithdrawBLL.Operate(id, operate, Cookies.Admin.GetAdminID(false), Note.Text))
            {
                ScriptHelper.Alert("操作成功", RequestHelper.RawUrl);
            }
            else
            {
                ScriptHelper.Alert("操作失败", RequestHelper.RawUrl);
            }
        }
    }
}