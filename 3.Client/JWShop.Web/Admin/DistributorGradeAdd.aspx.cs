using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JWShop.Web.Admin
{
    public partial class DistributorGradeAdd : JWShop.Page.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("ID");
                if (id>0)
                {
                    CheckAdminPower("ReadDistributorGrade", PowerCheckType.Single);
                    DistributorGradeInfo dis_grade = DistributorGradeBLL.Read(id);
                    Title.Text = dis_grade.Title;
                    Min_Amount.Text = dis_grade.Min_Amount.ToString();
                    Max_Amount.Text = dis_grade.Max_Amount.ToString();
                    Discount.Text = dis_grade.Discount.ToString();
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            DistributorGradeInfo dis_grade = new DistributorGradeInfo();
            dis_grade.Id = RequestHelper.GetQueryString<int>("ID");
            dis_grade.Title = Title.Text.Trim();
            dis_grade.Min_Amount = Convert.ToDecimal(Min_Amount.Text);
            dis_grade.Max_Amount = Convert.ToDecimal(Max_Amount.Text);
            dis_grade.Discount = Convert.ToDecimal(Discount.Text);
            if (dis_grade.Min_Amount>=dis_grade.Max_Amount)
            {
                ScriptHelper.Alert("等级上限必须大于下限");
            }
            #region 判断等级范围是否重叠
            var gradeList = DistributorGradeBLL.ReadList().Where(k => k.Id != dis_grade.Id).ToList();
            if (gradeList.Any(k => (k.Min_Amount >= dis_grade.Min_Amount && k.Max_Amount<= dis_grade.Max_Amount) || (k.Min_Amount<=dis_grade.Min_Amount  && k.Max_Amount>= dis_grade.Max_Amount) || (k.Min_Amount< dis_grade.Min_Amount && k.Max_Amount> dis_grade.Min_Amount && k.Max_Amount< dis_grade.Max_Amount) || (k.Min_Amount> dis_grade.Min_Amount && k.Min_Amount< dis_grade.Max_Amount && k.Max_Amount > dis_grade.Max_Amount)))
            {
                ScriptHelper.Alert("等级范围重叠，请重新输入");
            }
            #endregion
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (dis_grade.Id<=0)
            {
                CheckAdminPower("AddDistributorGrade", PowerCheckType.Single);
                int id = DistributorGradeBLL.Add(dis_grade);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("DistributorGrade"), id);
            }
            else
            {
                CheckAdminPower("UpdateDistributorGrade", PowerCheckType.Single);
                DistributorGradeBLL.Update(dis_grade);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("DistributorGrade"), dis_grade.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, "DistributorGrade.aspx");
        }
    }
}