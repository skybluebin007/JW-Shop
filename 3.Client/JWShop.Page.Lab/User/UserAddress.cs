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

namespace JWShop.Page.Lab
{
    public class UserAddress : UserBasePage
    {
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected UserAddressInfo userAddress = new UserAddressInfo();
        protected List<UserAddressInfo> userAddressList = new List<UserAddressInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();

            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Add":
                    Add();
                    break;
                case "Delete":
                    Delete();
                    break;
                case "Read":
                    Read();
                    break;
            }

            int id = RequestHelper.GetQueryString<int>("id");
            userAddress = UserAddressBLL.Read(id, base.UserId);

            singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            singleUnlimitClass.ClassID = userAddress.RegionId;

            userAddressList = UserAddressBLL.ReadList(base.UserId);
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
        private void Delete()
        {
            int id = RequestHelper.GetQueryString<int>("id");

            if (id < 1)
            {
                ResponseHelper.Write("error|无效的收货地址");
                ResponseHelper.End();
            }

            UserAddressBLL.Delete(id, base.UserId);
            ResponseHelper.Write("ok|");
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

    }
}