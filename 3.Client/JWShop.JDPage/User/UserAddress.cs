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

namespace JWShop.Page
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
            singleUnlimitClass.ClassID = (!string.IsNullOrEmpty(userAddress.RegionId)) ? userAddress.RegionId : "|1|";
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
                case "Add":
                    Add();
                    break;
                case "Read":
                    Read();
                    break;
                case "SetDefaultAddress":
                    int addressId = RequestHelper.GetQueryString<int>("addressId");
                    SetDefaultAddress(addressId);
                    break;
                case "ReadAddressList":
                    ReadAddressList();
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
              int newId=  UserAddressBLL.Add(userAddress);
                //如果选择了默认，则将此项设为默认地址，其他不默认
              if (userAddress.IsDefault > 0) UserAddressBLL.SetDefault(newId, base.UserId);
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


        private void Add()
        {
            var entity = new UserAddressInfo();

            int updateId = RequestHelper.GetForm<int>("updateId");
            entity.Consignee = StringHelper.AddSafe(RequestHelper.GetForm<string>("consignee"));
            entity.Address = StringHelper.AddSafe(RequestHelper.GetForm<string>("address"));
            entity.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("mobile"));
            entity.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("tel"));
            entity.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("email"));
            entity.IsDefault = RequestHelper.GetForm<int>("isdefault");

            SingleUnlimitClass unlimitClass = new SingleUnlimitClass();
            entity.RegionId = unlimitClass.ClassID;

            dynamic dymic = new System.Dynamic.ExpandoObject();
            dymic.result = "error";

            if (string.IsNullOrEmpty(entity.Consignee))
            {
                dymic.msg = "请填写收货人";
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dymic));
                ResponseHelper.End();
            }
            if (entity.RegionId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length < 3)
            {
                dymic.msg = "请填写完整的地区信息";
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dymic));
                ResponseHelper.End();
            }
            if (string.IsNullOrEmpty(entity.Address))
            {
                dymic.msg = "请填写详细地址";
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dymic));
                ResponseHelper.End();
            }
            if (string.IsNullOrEmpty(entity.Mobile) && string.IsNullOrEmpty(entity.Tel))
            {
                dymic.msg = "手机号码或固定电话至少填写一个";
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dymic));
                ResponseHelper.End();
            }

            entity.UserId = base.UserId;
            entity.UserName = base.UserName;

            if (updateId > 0)
            {
                entity.Id = updateId;
                UserAddressBLL.Update(entity);
            }
            else updateId = UserAddressBLL.Add(entity);

            dymic.result = "ok";
            dymic.id = updateId;
            dymic.consignee = entity.Consignee;
            dymic.address = RegionBLL.RegionNameList(entity.RegionId) + " " + entity.Address;
            dymic.mobile = entity.Mobile;
            dymic.tel = entity.Tel;
            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dymic));
            ResponseHelper.End();
        }

        private void Read()
        {
            int id = RequestHelper.GetQueryString<int>("id");

            if (id < 1)
            {
                ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result = "error", msg = "无效的收货地址" }));
                ResponseHelper.End();
            }

            var entity = UserAddressBLL.Read(id, base.UserId);
            SingleUnlimitClass unlimitClass = new SingleUnlimitClass();
            unlimitClass.ClassID = entity.RegionId;
            unlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();

            ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result = "ok", consignee = entity.Consignee, address = entity.Address, mobile = entity.Mobile, tel = entity.Tel, regionId = unlimitClass.ShowContent() }));
            ResponseHelper.End();
        }
        /// <summary>
        /// 设为默认
        /// </summary>
       protected void  SetDefaultAddress(int addressId){
           if (addressId < 1)
           {
               Response.Clear();
               ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "error", msg = "无效的收货地址" }));
               ResponseHelper.End();
           }
           if (base.UserId <= 0)
           {
               Response.Clear();
               ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "error", msg = "无效的操作,请重新登录" }));
               ResponseHelper.End();
           }
           UserAddressBLL.SetDefault(addressId, base.UserId);

           Response.Clear();
           ResponseHelper.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "ok", msg = "操作成功" }));
           ResponseHelper.End();
        }
        /// <summary>
        /// 获取地址列表
        /// </summary>
       protected void ReadAddressList() {
           try
           {
               List<UserAddressInfo> tmpuserAddressList = UserAddressBLL.ReadList(base.UserId);
               List<UserAddressInfo> resultList = new List<UserAddressInfo>();
               foreach (var item in tmpuserAddressList)
               {
                   item.RegionId = RegionBLL.RegionNameList(item.RegionId);
                   resultList.Add(item);
               }
               Response.Clear();
               Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "ok", dataList = resultList, msg = "" }));
           }
           catch (Exception ex)
           {
               Response.Clear();
               Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { flag = "error", dataList = "", msg = "系统忙，请稍后重试" }));
           }
           finally {
               Response.End();
           }
       }
    }
}