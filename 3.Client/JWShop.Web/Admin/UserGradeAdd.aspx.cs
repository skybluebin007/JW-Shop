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
    public partial class UserGradeAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int userGradeID = RequestHelper.GetQueryString<int>("ID");
                if (userGradeID != int.MinValue)
                {
                    CheckAdminPower("ReadUserGrade", PowerCheckType.Single);
                    UserGradeInfo userGrade = UserGradeBLL.Read(userGradeID);
                    Name.Text = userGrade.Name;
                    MinMoney.Text = userGrade.MinMoney.ToString();
                    MaxMoney.Text = userGrade.MaxMoney.ToString();
                    Discount.Text = userGrade.Discount.ToString();
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            UserGradeInfo userGrade = new UserGradeInfo();
            userGrade.Id = RequestHelper.GetQueryString<int>("ID");
            userGrade.Name = Name.Text;
            userGrade.MinMoney = Convert.ToDecimal(MinMoney.Text);
            userGrade.MaxMoney = Convert.ToDecimal(MaxMoney.Text);
            userGrade.Discount = Convert.ToDecimal(Discount.Text);
            if (userGrade.MinMoney >= userGrade.MaxMoney)
            {
                ScriptHelper.Alert("等级最高金额必须大于最低金额");
            }
            #region 判断等级范围是否重叠
            var gradeList = UserGradeBLL.ReadList().Where(k => k.Id != userGrade.Id).ToList();
            if(gradeList.Any(k=>(k.MinMoney>=userGrade.MinMoney && k.MaxMoney<=userGrade.MaxMoney) || (k.MinMoney<=userGrade.MinMoney && k.MaxMoney >= userGrade.MaxMoney) ||(k.MinMoney<userGrade.MinMoney && k.MaxMoney>userGrade.MinMoney && k.MaxMoney<userGrade.MaxMoney) || (k.MinMoney>userGrade.MinMoney && k.MinMoney<userGrade.MaxMoney && k.MaxMoney>userGrade.MaxMoney)))
            {
                ScriptHelper.Alert("等级范围重叠，请重新输入");
            }
            #endregion
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (userGrade.Id == int.MinValue)
            {
                CheckAdminPower("AddUserGrade", PowerCheckType.Single);
                int id = UserGradeBLL.Add(userGrade);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("UserGrade"), id);
            }
            else
            {
                CheckAdminPower("UpdateUserGrade", PowerCheckType.Single);
                UserGradeBLL.Update(userGrade);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("UserGrade"), userGrade.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}