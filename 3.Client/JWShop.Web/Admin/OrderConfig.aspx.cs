using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class OrderConfig : JWShop.Page.AdminBasePage
    {
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadConfig", PowerCheckType.Single);
                //积分抵现的比率
                PointToMoney.Text = ShopConfig.ReadConfigInfo().PointToMoney.ToString();
                //订单付款时限
                OrderPayTime.Text = ShopConfig.ReadConfigInfo().OrderPayTime.ToString();
                //订单单自动收货天数
                OrderRecieveShippingDays.Text = ShopConfig.ReadConfigInfo().OrderRecieveShippingDays.ToString();
                //订单支付模板Id 
                OrderPayTemplateId.Text = ShopConfig.ReadConfigInfo().OrderPayTemplateId;
                //订单自提模板Id
                SelfPickTemplateId.Text = ShopConfig.ReadConfigInfo().SelfPickTemplateId;
                //开团成功模板Id
                OpenGroupTemplateId.Text = ShopConfig.ReadConfigInfo().OpenGroupTemplateId;
                //参团成功模板Id
                GroupSignTemplateId.Text = ShopConfig.ReadConfigInfo().GroupSignTemplateId;
                //砍价成功模板Id
                BarGainTemplateId.Text = ShopConfig.ReadConfigInfo().BarGainTemplateId;
                //拼团成功模板Id
                GroupSuccessTemplateId.Text = ShopConfig.ReadConfigInfo().GroupSuccessTemplateId;
                //拼团失败模板Id
                GroupFailTemplateId.Text = ShopConfig.ReadConfigInfo().GroupFailTemplateId;
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("UpdateConfig", PowerCheckType.Single);
            ShopConfigInfo config = ShopConfig.ReadConfigInfo();
            //订单支付模板Id 
          config.OrderPayTemplateId = OrderPayTemplateId.Text.Trim();
            //订单自提模板Id
           config.SelfPickTemplateId = SelfPickTemplateId.Text.Trim();
            //开团成功模板Id
          config.OpenGroupTemplateId=  OpenGroupTemplateId.Text.Trim();
            //参团成功模板Id
           config.GroupSignTemplateId= GroupSignTemplateId.Text.Trim();
            //砍价成功模板Id
           config.BarGainTemplateId = BarGainTemplateId.Text.Trim();
            //拼团成功模板Id
            config.GroupSuccessTemplateId= GroupSuccessTemplateId.Text.Trim();
            //拼团失败模板Id
            config.GroupFailTemplateId = GroupFailTemplateId.Text.Trim();
            //积分抵现功能是否开启
            config.EnablePointPay = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$EnablePointPay");
            config.PointToMoney = Convert.ToInt32(PointToMoney.Text) < 0 ? 0 : Convert.ToInt32(PointToMoney.Text);
            if (config.EnablePointPay == 1 && (config.PointToMoney <= 0 || config.PointToMoney > 100))
            {
                ScriptHelper.Alert("积分抵现百分比必须大于0小于100");
            }
            
            if ((config.VoteStartDate - config.VoteEndDate).Days > 0) ScriptHelper.Alert("投票结束日期不得小于开始日期");
            //订单付款时限,不小于0
            config.OrderPayTime = Convert.ToInt32(OrderPayTime.Text) < 0 ? 0 : Convert.ToInt32(OrderPayTime.Text);
            //订单单自动收货天数
            config.OrderRecieveShippingDays = Convert.ToInt32(OrderRecieveShippingDays.Text) < 0 ? 0 : Convert.ToInt32(OrderRecieveShippingDays.Text);

            ShopConfig.UpdateConfigInfo(config);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateConfig"));
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}