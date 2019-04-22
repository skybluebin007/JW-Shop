using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using Newtonsoft.Json;

namespace JWShop.Page.Mobile
{
    public class UserAddress : UserBasePage
    {
        /// <summary>
        /// 单一无限极分类
        /// </summary>
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        /// <summary>
        /// 用户地址列表
        /// </summary>
        protected List<UserAddressInfo> userAddressList = new List<UserAddressInfo>();
        /// <summary>
        /// 用户地址
        /// </summary>
        protected UserAddressInfo userAddress = new UserAddressInfo();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            base.PageLoad();
            userAddressList = UserAddressBLL.ReadList(base.UserId);
            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Update":
                    int id = RequestHelper.GetQueryString<int>("ID");
                    userAddress = UserAddressBLL.Read(id, base.UserId);
                    singleUnlimitClass.ClassID = userAddress.RegionId;
                    break;
                case "Delete":
                    string deleteID = RequestHelper.GetQueryString<string>("ID");
                    UserAddressBLL.Delete(deleteID, base.UserId);
                    ResponseHelper.Write("ok");
                    ResponseHelper.End();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        protected override void PostBack()
        {
            UserAddressInfo userAddress = new UserAddressInfo();
            userAddress.Id = RequestHelper.GetForm<int>("ID");
            userAddress.Consignee = StringHelper.AddSafe(RequestHelper.GetForm<string>("Consignee"));
            userAddress.RegionId = singleUnlimitClass.ClassID;
            userAddress.Address = StringHelper.AddSafe(RequestHelper.GetForm<string>("Address"));
            userAddress.ZipCode = StringHelper.AddSafe(RequestHelper.GetForm<string>("ZipCode"));
            userAddress.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("Tel"));
            userAddress.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("Mobile"));
            userAddress.IsDefault = RequestHelper.GetForm<int>("IsDefault");
            userAddress.UserId = base.UserId;
            userAddress.UserName = base.UserName;
            string alertMessage = "添加成功";
            if (userAddress.Id <= 0)
            {
                UserAddressBLL.Add(userAddress);
            }
            else
            {
                UserAddressBLL.Update(userAddress);
                //如果选择了默认，则将此项设为默认地址，其他不默认
                if (userAddress.IsDefault > 0) UserAddressBLL.SetDefault(userAddress.Id, base.UserId);
                alertMessage = "修改成功";
            }
            ScriptHelper.AlertFront(alertMessage, RequestHelper.RawUrl);
        }
    }
}