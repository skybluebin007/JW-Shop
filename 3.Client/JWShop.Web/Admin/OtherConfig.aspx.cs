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
    public partial class OtherConfig :JWShop.Page.AdminBasePage
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
                AllImageWidth.Text = ShopConfig.ReadConfigInfo().AllImageWidth.ToString();
                GroupBuyDays.Text = ShopConfig.ReadConfigInfo().GroupBuyDays.ToString();
                PrintSN.Text = ShopConfig.ReadConfigInfo().PrintSN;
                FirstLevelDistributorRebatePercent.Text = ShopConfig.ReadConfigInfo().FirstLevelDistributorRebatePercent.ToString();
                SecondLevelDistributorRebatePercent.Text = ShopConfig.ReadConfigInfo().SecondLevelDistributorRebatePercent.ToString();
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

            config.CommentDefaultStatus = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$CommentDefaultStatus");
            int allImageWidth = 0;
            if (!int.TryParse(AllImageWidth.Text.Trim(), out allImageWidth))
            {
                ScriptHelper.Alert("图片宽度必须是数字");
            }
            config.AllImageWidth = allImageWidth < 0 ? 0 : allImageWidth;
            //config.AllImageWidth = Convert.ToInt32(AllImageWidth.Text) < 0 ? 0 : Convert.ToInt32(AllImageWidth.Text);
            config.AllImageIsNail = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$AllImageIsNail");
            #region 启用整站压缩 图片宽度不得小于600px
            if (config.AllImageIsNail == 1 && config.AllImageWidth < 600) ScriptHelper.Alert("整站图片压缩宽度不得小于600PX");
            #endregion
            config.SelfPick = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$SelfPick");
            config.GroupBuyDays = Convert.ToInt32(GroupBuyDays.Text) < 0 ? 2 : Convert.ToInt32(GroupBuyDays.Text);
            config.PrintSN = PrintSN.Text;
            //分销商是否需要审核
            config.CheckToBeDistributor = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$CheckToBeDistributor");
            decimal _d1 = 0,_d2=0;
            //1级分销返佣
            if(decimal.TryParse(FirstLevelDistributorRebatePercent.Text.Trim(),out _d1))
            {
                if (_d1 > 0 && _d1 <= 40)
                {
                    config.FirstLevelDistributorRebatePercent = _d1;
                }
                else
                {                   
                    ScriptHelper.Alert("1级返佣比例必须是0~40之间的数字");
                }
            }
            else
            {
               ScriptHelper.Alert("1级返佣比例必须大于0且不超过40");
            }

            //2级分销返佣
            if (decimal.TryParse(SecondLevelDistributorRebatePercent.Text.Trim(), out _d2))
            {
                if (_d2 > 0 && _d2 <= _d1)
                {
                    config.SecondLevelDistributorRebatePercent = _d2;
                }
                else
                {              
                    ScriptHelper.Alert("2级返佣比例必须大于0且不超过1级返佣比例");
                }
            }
            else
            {                
                ScriptHelper.Alert("2级返佣比例必须大于0且不超过1级返佣比例");
            }

            ShopConfig.UpdateConfigInfo(config);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateConfig"));
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}