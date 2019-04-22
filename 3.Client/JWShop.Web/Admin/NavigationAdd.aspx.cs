using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class NavigationAdd : JWShop.Page.AdminBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            int navigationType = RequestHelper.GetQueryString<int>("navigationType");
            int id = RequestHelper.GetQueryString<int>("Id");
            NavigationInfo navigation = new NavigationInfo();
            if (id > 0)
            {
                navigation = NavigationBLL.Read(id);
                navigationType = navigation.NavigationType;
            }

            //导航类别
            ddlNavigationType.DataSource = EnumHelper.ReadEnumList<NavigationType>();
            ddlNavigationType.DataTextField = "ChineseName";
            ddlNavigationType.DataValueField = "Value";
            ddlNavigationType.DataBind();
            ddlNavigationType.Text = navigationType.ToString();

            //父类导航
            int curNavigation = int.Parse(ddlNavigationType.Text);
            ddlParent.DataSource = NavigationBLL.ReadFatherList(curNavigation);
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "Id";
            ddlParent.DataBind();
            ddlParent.Items.Insert(0, new ListItem("父级导航", "0"));

            //内容ID
            ClassId.DataSource = ClassRelation.Read();
            ClassId.DataTextField = "Name";
            ClassId.DataValueField = "VirtualId";
            ClassId.DataBind();
            ClassId.Items.Insert(0, new ListItem("--请选择--", "0"));

            //显示方式
            radioNavigationShowType.DataSource = EnumHelper.ReadEnumList<NavigationShowType>();
            radioNavigationShowType.DataTextField = "ChineseName";
            radioNavigationShowType.DataValueField = "Value";
            radioNavigationShowType.DataBind();

            LinkTypeForCustom.Visible = false;
            IsVisible.Text = "1";

            if (id > 0)
            {
                ddlParent.Text = navigation.ParentId.ToString();
                radioNavigationShowType.Text = navigation.ClassType.ToString();
                LinkType.Text = navigation.ClassType.ToString();
                LinkTypeForURL.Visible = navigation.ClassType == (int)NavigationClassType.Url;
                LinkTypeForCustom.Visible = navigation.ClassType != (int)NavigationClassType.Url;
                ShowTypeForCustom.Visible = !navigation.IsSingle;
                radioIsSingle.Text = navigation.IsSingle.ToString().ToLower();
                radioNavigationShowType.Text = navigation.ShowType.ToString();

                Name.Text = navigation.Name;
                Remark.Text = navigation.Remark;
                OrderId.Text = navigation.OrderId.ToString();
                URL.Text = navigation.Url;
                ClassId.Text = navigation.ClassId.ToString();
                IsVisible.Text = navigation.IsVisible.ToString().ToLower();
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("id");
            NavigationInfo entity = NavigationBLL.Read(id);

            entity.NavigationType = int.Parse(ddlNavigationType.Text);
            entity.ParentId = int.Parse(ddlParent.Text);
            entity.Name = Name.Text;
            entity.Remark = Remark.Text;
            entity.OrderId = int.Parse(OrderId.Text);
            entity.ClassType = int.Parse(LinkType.Text);
            entity.ClassId = int.Parse(ClassId.Text);
            entity.IsVisible = bool.Parse(IsVisible.Text);
            //选择URL形式，只保存url数据
            if (entity.ClassType == (int)NavigationClassType.Url)
            {
                entity.Url = URL.Text;
                entity.IsSingle = false;
                entity.ShowType = 0;
            }
            else
            {
                entity.Url = string.Empty;
                entity.IsSingle = bool.Parse(radioIsSingle.Text);
                entity.ShowType = entity.IsSingle ? 0 : int.Parse(radioNavigationShowType.Text);
            }
            
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (entity.Id > 0)
            {
                NavigationBLL.Update(entity);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            else
            {
                NavigationBLL.Add(entity);
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }

        protected void LinkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value = Convert.ToInt32(((RadioButtonList)sender).Text);
            LinkTypeForURL.Visible = value == (int)NavigationClassType.Url;
            LinkTypeForCustom.Visible = value != (int)NavigationClassType.Url;
        }

        protected void radioIsSingle_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool value = Convert.ToBoolean(((RadioButtonList)sender).Text);
            ShowTypeForCustom.Visible = !value;
        }
    }
}