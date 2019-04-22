using System;
using System.IO;
using System.Web;
using System.Data;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class AdminGroupAdd : JWShop.Page.AdminBasePage
    {
        protected string power = string.Empty;
        protected List<PowerInfo> channelPowerList = new List<PowerInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int groupID = RequestHelper.GetQueryString<int>("ID");
                if (groupID != int.MinValue)
                {
                    CheckAdminPower("ReadAdminGroup", PowerCheckType.Single);
                    AdminGroupInfo adminGroup = AdminGroupBLL.Read(groupID);
                    Name.Text = adminGroup.Name;
                    Note.Text = adminGroup.Note;
                    power = adminGroup.Power;
                }
                if (groupID == 1)
                {
                    //如果是超级管理员组则不能修改权限
                    //SubmitButton.Visible = false;
                }
            }

            //绑定权限列表
            XmlHelper xh = new XmlHelper(ServerHelper.MapPath("~/Config/AdminPower.Config"));
            XmlNode xn = xh.ReadNode("Config");
            foreach (XmlNode temp in xn.ChildNodes)
            {
                PowerInfo power = new PowerInfo();
                power.Text = temp.Attributes["Text"].Value;
                power.Key = temp.Attributes["Key"].Value;
                power.XML = temp.InnerXml;
                channelPowerList.Add(power);
            }

           

        }
        protected List<PowerInfo> ReadPowerBlock(string xml)
        {
            string childNode = "<root>" + xml + "</root>";
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(childNode);
            List<PowerInfo> blockPowerList = new List<PowerInfo>();
            foreach (XmlNode xn in xd.SelectNodes("root/Block"))
            {
                PowerInfo power = new PowerInfo();
                power.Text = xn.Attributes["Text"].Value;
                power.XML = xn.InnerXml;
                blockPowerList.Add(power);
            }
            return blockPowerList;
        }
        protected List<PowerInfo> ReadPowerItem(string xml)
        {
            string childNode = "<root>" + xml + "</root>";
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(childNode);
            List<PowerInfo> itemPowerList = new List<PowerInfo>();
            foreach (XmlNode xn in xd.SelectNodes("root/Item"))
            {
                PowerInfo power = new PowerInfo();
                power.Text = xn.Attributes["Text"].Value;
                power.Value = xn.Attributes["Value"].Value;
                itemPowerList.Add(power);
            }
            return itemPowerList;
        }

        protected void SubmitButton_Click(object sender, EventArgs E)
        {
            AdminGroupInfo adminGroup = new AdminGroupInfo();
            adminGroup.Id = RequestHelper.GetQueryString<int>("ID");
            adminGroup.Name = Name.Text;
            adminGroup.Power = RequestHelper.GetForm<string>("Rights").Replace(",", "|");
            adminGroup.Note=Note.Text;
            if (adminGroup.Power != string.Empty)
            {
                adminGroup.Power = "|" + adminGroup.Power + "|";
            }
            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            if (adminGroup.Id == int.MinValue)
            {
                CheckAdminPower("AddAdminGroup", PowerCheckType.Single);
                int id = AdminGroupBLL.Add(adminGroup);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("AdminGroup"), id);
            }
            else
            {
                CheckAdminPower("UpdateAdminGroup", PowerCheckType.Single);
                AdminGroupInfo tmpAdminGroup = AdminGroupBLL.Read(adminGroup.Id);
                adminGroup.AdminCount = tmpAdminGroup.AdminCount;
                adminGroup.AddDate = tmpAdminGroup.AddDate;
                adminGroup.IP = tmpAdminGroup.IP;
             
                AdminGroupBLL.Update(adminGroup);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("AdminGroup"), adminGroup.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}