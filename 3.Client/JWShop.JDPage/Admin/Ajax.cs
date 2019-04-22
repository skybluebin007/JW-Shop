using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCES.EntLib;
using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using System.Web.Security;
using Newtonsoft.Json;

namespace JWShop.Page.Admin
{
  public  class Ajax: BasePage
    {
        /// <summary>
        /// 订单退款状态枚举
        /// </summary>
        protected List<EnumInfo> orderRefundStatus = EnumHelper.ReadEnumList<OrderRefundStatus>();
        /// <summary>
        /// 页面加载
        /// </summary>
        protected override void PageLoad()
        {
            string action = RequestHelper.GetQueryString<string>("action").ToLower();
            if(string.IsNullOrEmpty(action)) action=RequestHelper.GetForm<string>("action").ToLower();
            switch (action)
            {
                case "login":
                    Login();
                    break;
                case "changepassword":
                    ChangePassword();
                    break;
                case "findpwd":
                    FindPassword();
                    break;
                case "getorders":
                    GetOrders();
                    break;
                case "shopconfig":
                    ShopConfigSet();
                    break;
                case "operateorder":
                    OperateOrder();
                    break;
                case "getrefundlist":
                    GetRefundList();
                    break;
                case "getcoupons":
                    GetCoupons();
                    break;
                case "addcoupon":
                    AddCoupon();
                    break;
                case "getactivities":
                    GetActivities();
                    break;
                case "addfavorableactivity":
                    AddFavorableActivity();
                    break;
                case "deleteadimage":
                    DeleteAdImage();
                    break;
                case "addadimage":
                    AddAdImage();
                    break;
                case "writeoff":
                    WriteOff();
                    break;
                case "needrefund":
                    NeedRefund();
                    break;
                case "getgroupbuylist":
                    GetGroupBuyList();
                    break;
                case "groupbuyorderrefund":
                    GroupBuyOrderRefund();
                    break;
                    //case "orderrefund":
                    //    OrderRefund();
                    //break;
            }
        }
        /// <summary>
        /// 管理员登录
        /// </summary>
        protected void Login()
        {
            string loginName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("UserName"));
            string loginPass = StringHelper.SearchSafe(RequestHelper.GetForm<string>("UserPassword"));
            string redirecturl = RequestHelper.GetForm<string>("redirecturl");
            if (string.IsNullOrEmpty(loginName))
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false,msg= "请输入用户名" }));
                Response.End();
            }
            if (string.IsNullOrEmpty(loginName))
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请输入密码" }));
                Response.End();
            }
            var theLoginAdmin = AdminBLL.Read(loginName);
            //如果登录日期与上次登录日期不是同一天，更新登录记录，清空错误次数，解除锁定
            if (theLoginAdmin.Id > 0 && (DateTime.Now - theLoginAdmin.LastLoginDate).Days > 0)
            {
                AdminBLL.UpdateLogin(theLoginAdmin.Id, RequestHelper.DateNow, ClientHelper.IP);
            }
            //bool remember = Remember.Checked;
            loginPass = StringHelper.Password(loginPass, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            AdminInfo admin = AdminBLL.CheckLogin(loginName, loginPass);
            if (admin.Id > 0)
            {

                // 如果账户未锁定
                if (admin.Status == (int)BoolType.True)
                {                  

                        string randomNumber = Guid.NewGuid().ToString();
                        string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(admin.Id.ToString() + admin.Name + admin.GroupId.ToString() + randomNumber + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5");
                        string value = sign + "|" + admin.Id.ToString() + "|" + admin.Name + "|" + admin.GroupId.ToString() + "|" + randomNumber;
                        //if (remember)
                        //{
                        //    CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, value, 1, TimeType.Year);
                        //}
                        //else
                        //{
                            CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, value);
                        //}
                        string signvalue = FormsAuthentication.HashPasswordForStoringInConfigFile(admin.Id.ToString() + admin.Name + admin.GroupId.ToString() + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent + AdminBLL.Read(admin.Id).Password, "MD5");
                        CookiesHelper.AddCookie("AdminSign", signvalue);
                        AdminBLL.UpdateLogin(admin.Id, RequestHelper.DateNow, ClientHelper.IP);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("LoginSystem"));
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok=true,url=redirecturl}));
                    Response.End();                   

                }
                else
                {//如果账户已锁定
                    string errorMsg = "登录错误达到3次，已被锁定，可联系网站客服解锁，也可次日重新登录。";
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = errorMsg }));
                    Response.End();
                }
            }
            else
            {
                //登录失败，失败次数加1。如果失败超过3次，则锁定账户
                AdminBLL.UpdateLogin(loginName, RequestHelper.DateNow, ClientHelper.IP, 3);
                AdminLogBLL.Add("管理员：" + loginName + "在" + RequestHelper.DateNow + "登陆网站后台失败，登陆IP：" + ClientHelper.IP);
                if (theLoginAdmin.Id > 0 && theLoginAdmin.LoginErrorTimes >= 3)
                {
                    string errorMsg = "登录错误达到3次，已被锁定，可联系网站客服解锁，也可次日重新登录。";
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = errorMsg }));
                    Response.End();
                }
                else
                {
                    //ScriptHelper.AlertFront("登录失败", RequestHelper.RawUrl);
                    string errorMsg = "用户名或密码错误，登录失败。";              
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = errorMsg }));
                    Response.End();
                }
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        private void ChangePassword()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
            string oldPassword = RequestHelper.GetForm<string>("OldPassword");
            string newPassword = RequestHelper.GetForm<string>("NewPassword");
            string newPassword2 = RequestHelper.GetForm<string>("NewPassword2");
            if (string.IsNullOrEmpty(oldPassword))
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请输入原始密码" }));
                Response.End();
            }
            if (!string.Equals(newPassword,newPassword2))
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "两次输入新密码不一致" }));
                Response.End();
            }
            oldPassword = StringHelper.Password(oldPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            newPassword = StringHelper.Password(newPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            AdminInfo admin = AdminBLL.Read(Cookies.Admin.GetAdminID(false));
            if (admin.Password == oldPassword)
            {
                
                AdminBLL.ChangePassword(Cookies.Admin.GetAdminID(false), oldPassword, newPassword);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("ChangePassword"));
                //清除现有cookie
                CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().AdminCookies);
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = true }));
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "原始密码错误" }));
                Response.End();
            }
        }
        /// <summary>
        /// 管理员找回密码
        /// </summary>
        protected void FindPassword()
        {
            bool flag = true;
            string msg = string.Empty;        
            string adminName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("UserName"));
            string email = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Email"));
            var admin = AdminBLL.Read(adminName);
            //如果账号不存在
            if (admin.Id <= 0)
            {
                flag = false;
                msg = "账号不存在";
            }
            //如果账号不存在
            if (!string.Equals(admin.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                flag = false;
                msg = "账号、邮箱不匹配";
            }
            if (admin.Id > 0 && string.Equals(admin.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // 验证通过,发送邮件
                    string tempSafeCode = Guid.NewGuid().ToString();
                    AdminBLL.ChangeAdminSafeCode(admin.Id, tempSafeCode, RequestHelper.DateNow);
                    string url = "http://" + Request.ServerVariables["HTTP_HOST"] + "/MobileAdmin/ResetPassword.html?CheckCode=" + StringHelper.Encode(admin.Id + "|" + admin.Email + "|" + admin.Name + "|" + tempSafeCode, ShopConfig.ReadConfigInfo().SecureKey);
                    EmailContentInfo emailContent = EmailContentHelper.ReadSystemEmailContent("FindPassword");
                    EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                    emailSendRecord.Title = emailContent.EmailTitle;
                    emailSendRecord.Content = emailContent.EmailContent.Replace("$Url$", url);
                    emailSendRecord.IsSystem = (int)BoolType.True;
                    emailSendRecord.EmailList = admin.Email;
                    emailSendRecord.IsStatisticsOpendEmail = (int)BoolType.False;
                    emailSendRecord.SendStatus = (int)SendStatus.No;
                    emailSendRecord.AddDate = RequestHelper.DateNow;
                    emailSendRecord.SendDate = RequestHelper.DateNow;
                    emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                    EmailSendRecordBLL.SendEmail(emailSendRecord);
                    string emailResult = "您的申请已提交，请在15分钟内登录邮箱重设你的密码,！<a href=\"http://mail." + admin.Email.Substring(admin.Email.IndexOf("@") + 1) + "\"  target=\"_blank\">马上登录</a>";
                    flag = true;
                    msg = emailResult;
                }
                catch
                {
                    flag = true;
                    msg = "重置密码邮件发送失败，请检查配置";
                }

            }
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { ok = flag, msg = msg}));
            Response.End();
        }
        /// <summary>
        /// 基础配置
        /// </summary>
        protected void ShopConfigSet()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
            string param = RequestHelper.GetForm<string>("param").ToLower();
            string title = RequestHelper.GetForm<string>("shoptitle");
            string description = RequestHelper.GetForm<string>("shopdes");
            string tel = RequestHelper.GetForm<string>("shoptel");
            string address = RequestHelper.GetForm<string>("shopaddress");
            string hours = RequestHelper.GetForm<string>("shophour");
            ShopConfigInfo config = ShopConfig.ReadConfigInfo();
            switch (param)
            {
                case "title":
                    if (string.IsNullOrEmpty(title))
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请输入标题" }));
                        Response.End();
                    }
                    config.Title = title.Trim();
                    ShopConfig.UpdateConfigInfo(config);
                    break;
                case "description":
                    if (string.IsNullOrEmpty(description))
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请输入描述" }));
                        Response.End();
                    }
                    config.Description = description.Trim();
                    ShopConfig.UpdateConfigInfo(config);
                    break;
                case "tel":
                    if (string.IsNullOrEmpty(tel))
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请输入联系电话" }));
                        Response.End();
                    }
                    if (!ShopCommon.CheckMobile(tel) && !ShopCommon.CheckTel(tel))
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "联系电话格式错误" }));
                        Response.End();
                    }
                    config.Tel = tel;
                    ShopConfig.UpdateConfigInfo(config);
                    break;
                case "address":
                    if (string.IsNullOrEmpty(address))
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请输入地址" }));
                        Response.End();
                    }
                    config.Address = address;
                    ShopConfig.UpdateConfigInfo(config);
                    break;
                case "hour":
                    if (string.IsNullOrEmpty(hours))
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请输入营业时间" }));
                        Response.End();
                    }
                    config.BusinessHours = hours;
                    ShopConfig.UpdateConfigInfo(config);
                    break;
                case "orderpaydiscount":
                 
                    //满立减                 
                    config.PayDiscount = RequestHelper.GetForm<int>("PayDiscount");
                    config.OrderMoney = RequestHelper.GetForm<decimal>("OrderMoney");
                    config.OrderDisCount = RequestHelper.GetForm<decimal>("OrderDisCount");
                    if (config.PayDiscount == 1)
                    {
                        if (config.OrderMoney <= 0)
                        {
                            Response.Clear();
                            Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请填写有效的商品金额" }));                          
                            Response.End();
                        }
                        if (config.OrderDisCount <= 0)
                        {
                            Response.Clear();
                            Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请填写有效的优惠金额" }));
                            Response.End();
                        }
                        if (config.OrderMoney <= config.OrderDisCount)
                        {
                            Response.Clear();
                            Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "优惠金额必须小于订单金额" }));
                            Response.End();
                        }
                    }                 
                    ShopConfig.UpdateConfigInfo(config);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateConfig"));
                    break;
                case "orderset":                   
                    //订单付款时限
                    config.OrderPayTime = RequestHelper.GetForm<int>("OrderPayTime") < 0 ? 0 : RequestHelper.GetForm<int>("OrderPayTime");
                    //订单收货时限
                    config.OrderRecieveShippingDays = RequestHelper.GetForm<int>("OrderRecieveShippingDays") < 0 ? 0 : RequestHelper.GetForm<int>("OrderRecieveShippingDays");
                    //积分抵现                  
                    config.EnablePointPay = RequestHelper.GetForm<int>("EnablePointPay");
                    config.PointToMoney = RequestHelper.GetForm<int>("PointToMoney");
                    //模板消息Id
                    config.OrderPayTemplateId = RequestHelper.GetForm<string>("OrderPayTemplateId");
                    config.SelfPickTemplateId = RequestHelper.GetForm<string>("SelfPickTemplateId");
                    config.OpenGroupTemplateId = RequestHelper.GetForm<string>("OpenGroupTemplateId");
                    config.GroupSignTemplateId = RequestHelper.GetForm<string>("GroupSignTemplateId");
                    config.GroupSuccessTemplateId = RequestHelper.GetForm<string>("GroupSuccessTemplateId");
                    config.GroupFailTemplateId = RequestHelper.GetForm<string>("GroupFailTemplateId");
                    if (config.PointToMoney <=0 || config.PointToMoney>100)
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "积分抵现百分比必须大于0小于100" }));
                        Response.End();
                    }
                    ShopConfig.UpdateConfigInfo(config);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateConfig"));
                    break;
                case "otherset":
                    //评论显示状态
                    config.CommentDefaultStatus = RequestHelper.GetForm<int>("CommentDefaultStatus") != 2 ? 3 : RequestHelper.GetForm<int>("CommentDefaultStatus");
                    //整站图片压缩开关
                    config.AllImageIsNail = RequestHelper.GetForm<int>("AllImageIsNail") < 0 ? 0 : RequestHelper.GetForm<int>("AllImageIsNail");
                    config.AllImageWidth = RequestHelper.GetForm<int>("AllImageWidth");
                    if(config.AllImageIsNail==1 && config.AllImageWidth<600)
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "图片压缩宽度不小于600px" }));
                        Response.End();
                    }
                    config.SelfPick = RequestHelper.GetForm<int>("SelfPick");
                    config.GroupBuyDays = RequestHelper.GetForm<int>("GroupBuyDays") < 0 ? 2 : RequestHelper.GetForm<int>("GroupBuyDays");
                    config.PrintSN = RequestHelper.GetForm<string>("PrintSN");


                    ShopConfig.UpdateConfigInfo(config);
                    AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateConfig"));
                    break;
            }
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { ok = true }));
            Response.End();
        }
        /// <summary>
        /// 获取订单
        /// </summary>
        protected void GetOrders()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.OrderNumber = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("OrderNumber"));
           int orderStatus = RequestHelper.GetQueryString<int>("orderstatus");
            int orderperiod = RequestHelper.GetQueryString<int>("orderperiod");
            string searchkey =StringHelper.AddSafe(RequestHelper.GetQueryString<string>("searchkey"));
            DateTime orderDate = RequestHelper.GetQueryString<DateTime>("orderdate");
            int pageIndex = RequestHelper.GetQueryString<int>("pageindex");
            int pageSize = RequestHelper.GetQueryString<int>("pagesize");
            if (pageIndex > 0 && pageSize > 0)
            {
                orderSearch.OrderPeriod = orderperiod;
                orderSearch.SearchKey = searchkey;
                //orderSearch.OrderDate = orderDate;
                //如果查找已删除订单
                if (orderStatus == (int)Entity.OrderStatus.HasDelete)
                {
                    orderSearch.IsDelete = (int)BoolType.True;//已删除
                }
                else
                {
                    orderSearch.OrderStatus = orderStatus;
                    orderSearch.IsDelete = (int)BoolType.False;//未删除
                }
                orderSearch.Consignee = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Consignee"));
                orderSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                orderSearch.EndAddDate = RequestHelper.GetQueryString<DateTime>("EndAddDate");
                int count = 0;
                List<OrderInfo> orderList = OrderBLL.SearchList(pageIndex, pageSize, orderSearch, ref count);
                //订单状态文字暂存addcol1
                orderList.ForEach(k => k.AddCol1 = OrderBLL.ReadOrderStatus(k.OrderStatus, k.IsDelete));
                orderList.ForEach(k => k.OrderDetailList = OrderDetailBLL.ReadList(k.Id));
                orderList.ForEach(k => k.OrderDetailList.ForEach(k1 => k1.ProductPhoto = ProductBLL.Read(k1.ProductId).Photo));
                //是否有正在退款处理中
                orderList.ForEach(k => k.OrderRefundList = OrderRefundBLL.ReadListValid(k.Id));
                orderList.ForEach(k => k.IsRefunding = (k.OrderRefundList.Count(k1 => !OrderRefundBLL.HasReturn(k1.Status)) >0));
                orderList.ForEach(k => k.OrderRefundUrl = k.IsRefunding? "/mobileadmin/orderrefunddetail.html?id=" + k.OrderRefundList.Where(k1 => k1.Status != (int)OrderRefundStatus.HasReturn).FirstOrDefault().Id:"");

                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = true, count = orderList.Count, data = orderList }));
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new {ok=false,count=0 }));
                Response.End();
            }
        }

        /// <summary>
        /// 操作订单
        /// </summary>
        protected void OperateOrder()
        {
            bool flag = true;
            string msg = string.Empty;
            string param = RequestHelper.GetForm<string>("param").ToLower();
            int orderId = RequestHelper.GetForm<int>("orderid");
            if (!base.IsAdminLogin())
            {
                flag = false;
                msg = "登录已失效，请重新登录";              
            }
            if (orderId <= 0)
            {
                flag = false;
                msg = "请求参数错误";
            }
            OrderInfo order = OrderBLL.Read(orderId);
            int startOrderStatus = order.OrderStatus;
            var orderActionList = OrderActionBLL.ReadList(order.Id);
            switch (param)
            {
                case "check":
                    if (startOrderStatus == (int)(OrderStatus.WaitCheck))
                    {
                        #region  拼团单，未拼满或者拼团失败不能通过审核
                        if (order.IsActivity == (int)OrderKind.GroupBuy)
                        {
                            var groupBuy = GroupBuyBLL.Read(order.FavorableActivityId);
                            if (!(groupBuy.StartTime <= DateTime.Now && groupBuy.EndTime >= DateTime.Now && groupBuy.SignCount >= groupBuy.Quantity))
                            {
                                flag = false;
                                msg = "拼团未成功，暂不能通过审核";
                            }
                        }
                        #endregion
                        else
                        {
                            //自提：现场审核提货，完成
                            if (order.SelfPick == 1)
                            {
                                order.OrderStatus = (int)OrderStatus.ReceiveShipping;
                            }
                            else
                            {//配送：进入配货状态
                                order.OrderStatus = (int)OrderStatus.Shipping;
                            }                       
                            OrderBLL.AdminUpdateOrderAddAction(order, "", (int)OrderOperate.Check, startOrderStatus);
                        }
                    }
                    else
                    {
                        flag = false;
                        msg = "无效的请求";
                    }
                    break;
                case "cancel":                  
                    if (startOrderStatus == (int)(OrderStatus.WaitCheck) || startOrderStatus == (int)(OrderStatus.WaitPay))
                    {
                        order.OrderStatus = (int)OrderStatus.NoEffect;
                        #region 待付款状态，退还用户下单时抵现的积分
                        if (startOrderStatus == (int)OrderStatus.WaitPay && order.Point > 0)
                        {
                            var accountRecord = new UserAccountRecordInfo
                            {
                                RecordType = (int)AccountRecordType.Point,
                                Money = 0,
                                Point = order.Point,
                                Date = DateTime.Now,
                                IP = ClientHelper.IP,
                                Note = "取消订单：" + order.OrderNumber + "，退回用户积分",
                                UserId = order.UserId,
                                UserName = order.UserName,
                            };
                            UserAccountRecordBLL.Add(accountRecord);
                        }
                        #endregion
                        //更新商品库存数量
                        ProductBLL.ChangeOrderCountByOrder(orderId, ChangeAction.Minus);
                        OrderBLL.AdminUpdateOrderAddAction(order, "", (int)OrderOperate.Cancle, startOrderStatus);
                    }
                    else
                    {
                        flag = false;
                        msg = "无效的请求";
                    }
                    break;
                case "receive":
                    if (startOrderStatus == (int)(OrderStatus.HasShipping))
                    {
                        order.OrderStatus = (int)OrderStatus.ReceiveShipping;
                        #region 根据订单付款金额全额返还积分
                        int sendPoint = (int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
                        if (sendPoint > 0 && order.IsActivity == (int)BoolType.False)
                        {
                            var accountRecord = new UserAccountRecordInfo
                            {
                                RecordType = (int)AccountRecordType.Point,
                                Money = 0,
                                Point = sendPoint,
                                Date = DateTime.Now,
                                IP = ClientHelper.IP,
                                Note = ShopLanguage.ReadLanguage("OrderReceived").Replace("$OrderNumber", order.OrderNumber),
                                UserId = order.UserId,
                                UserName = order.UserName
                            };
                            UserAccountRecordBLL.Add(accountRecord);
                        }
                        #endregion  
                        OrderBLL.AdminUpdateOrderAddAction(order, "", (int)OrderOperate.Received, startOrderStatus);
                    }
                    else
                    {
                        flag = false;
                        msg = "无效的请求";
                    }
                    break;
                case "delete":
                    if (order.IsDelete == (int)BoolType.False)
                    {
                        order.IsDelete = (int)BoolType.True;
                        //order.OrderStatus = (int)OrderStatus.HasDelete;
                        OrderBLL.Update(order);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Order"), order.Id);
                    }
                    break;
                case "back":
                    if (orderActionList.Count > 0 && order.IsRefund != (int)BoolType.True && order.OrderStatus != (int)OrderStatus.WaitPay)
                    {
                        order.OrderStatus = OrderActionBLL.ReadLast(order.Id, order.OrderStatus).StartOrderStatus;
                        //减去用户积分,积分为负数
                        int sendPoint = 0;
                        if (startOrderStatus == (int)OrderStatus.ReceiveShipping && order.IsActivity == (int)BoolType.False)
                        {
                            //sendPoint = -OrderBLL.ReadOrderSendPoint(order.Id);
                            //根据订单付款金额全额返还积分
                            sendPoint = -(int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
                        }
                        if (startOrderStatus == (int)OrderStatus.HasShipping && order.OrderStatus == (int)OrderStatus.ReceiveShipping && order.IsActivity == (int)BoolType.False)//相当于收货确认
                        {
                            //sendPoint = OrderBLL.ReadOrderSendPoint(order.Id);
                            //根据订单付款金额全额返还积分
                            sendPoint = -(int)Math.Floor(OrderBLL.ReadNoPayMoney(order));
                        }
                        if (sendPoint != 0)
                        {
                            var accountRecord = new UserAccountRecordInfo
                            {
                                RecordType = (int)AccountRecordType.Point,
                                Money = 0,
                                Point = sendPoint,
                                Date = DateTime.Now,
                                IP = ClientHelper.IP,
                                Note = ShopLanguage.ReadLanguage("OrderBack").Replace("$OrderNumber", order.OrderNumber),
                                UserId = order.UserId,
                                UserName = order.UserName
                            };
                            UserAccountRecordBLL.Add(accountRecord);
                        }

                        //更新商品库存数量
                        switch (startOrderStatus)
                        {
                            case (int)OrderStatus.WaitPay:
                            case (int)OrderStatus.WaitCheck:
                                if (order.OrderStatus == (int)OrderStatus.NoEffect)//相当于取消操作
                                {
                                    ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Minus);
                                }
                                break;
                            case (int)OrderStatus.NoEffect:
                                ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Plus);
                                break;
                            case (int)OrderStatus.HasReturn:
                                ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Plus);
                                ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Plus);
                                break;
                            case (int)OrderStatus.Shipping:
                                if (order.OrderStatus == (int)OrderStatus.HasShipping)
                                {
                                    ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Plus);
                                }
                                break;
                            case (int)OrderStatus.HasShipping:
                                if (order.OrderStatus == (int)OrderStatus.Shipping)
                                {
                                    ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Minus);
                                }
                                if (order.OrderStatus == (int)OrderStatus.HasReturn)//相当于退货确认操作
                                {
                                    ProductBLL.ChangeOrderCountByOrder(order.Id, ChangeAction.Minus);
                                    ProductBLL.ChangeSendCountByOrder(order.Id, ChangeAction.Minus);
                                }
                                break;
                            default:
                                break;
                        }                        
                        OrderBLL.AdminUpdateOrderAddAction(order, "", (int)OrderOperate.Back, startOrderStatus);
                    }
                    else
                    {
                        flag = false;
                        msg = "无效的请求";
                    }
                    break;
            }
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { ok=flag,msg=msg}));
            Response.End();
        }
        /// <summary>
        /// 获取拼团列表
        /// </summary>
        private void GetGroupBuyList()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }

           int status = RequestHelper.GetQueryString<int>("status") < -2 ? 0 : RequestHelper.GetQueryString<int>("status");
            GroupBuySearchInfo searchInfo = new GroupBuySearchInfo();
            searchInfo.Status = status;

            int pageIndex = RequestHelper.GetQueryString<int>("pageindex");
            int pageSize = RequestHelper.GetQueryString<int>("pagesize");
            if (pageIndex > 0 && pageSize > 0)
            {              
                int count = 0;
                List<GroupBuyInfo> dataList = GroupBuyBLL.SearchList(pageIndex, pageSize, searchInfo, ref count);
                ////订单状态文字暂存addcol1
                //orderList.ForEach(k => k.AddCol1 = OrderBLL.ReadOrderStatus(k.OrderStatus, k.IsDelete));
                //orderList.ForEach(k => k.OrderDetailList = OrderDetailBLL.ReadList(k.Id));
                //orderList.ForEach(k => k.OrderDetailList.ForEach(k1 => k1.ProductPhoto = ProductBLL.Read(k1.ProductId).Photo));
                ////是否有正在退款处理中
                //orderList.ForEach(k => k.OrderRefundList = OrderRefundBLL.ReadListValid(k.Id));
                //orderList.ForEach(k => k.IsRefunding = (k.OrderRefundList.Count(k1 => !OrderRefundBLL.HasReturn(k1.Status)) > 0));
                //orderList.ForEach(k => k.OrderRefundUrl = k.IsRefunding ? "/mobileadmin/orderrefunddetail.html?id=" + k.OrderRefundList.Where(k1 => k1.Status != (int)OrderRefundStatus.HasReturn).FirstOrDefault().Id : "");

                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = true, count = dataList.Count, data = dataList }));
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, count = 0 }));
                Response.End();
            }
        }

        /// <summary>
        /// 获取退款记录
        /// </summary>
        protected void GetRefundList()
        {
            if (!base.IsAdminLogin())
            {             
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
            int pageIndex = RequestHelper.GetQueryString<int>("pageindex");
            int pageSize = RequestHelper.GetQueryString<int>("pagesize");
            if (pageIndex > 0 && pageSize > 0)
            {
                OrderRefundSearchInfo searchInfo = new OrderRefundSearchInfo();

                searchInfo.RefundNumber = RequestHelper.GetQueryString<string>("RefundNumber");
                searchInfo.OrderNumber = RequestHelper.GetQueryString<string>("OrderNumber");
                searchInfo.Status = RequestHelper.GetQueryString<int>("Status");
                searchInfo.StartTmCreate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                searchInfo.EndTmCreate = RequestHelper.GetQueryString<DateTime>("EndAddDate");
                int count = 0;
                var orderRefundList = OrderRefundBLL.SearchList(pageIndex, pageSize, searchInfo, ref count);
                //退款状态描述
                orderRefundList.ForEach(k => k.StatusDescription = orderRefundStatus.Where(k1 => k1.Value == k.Status).FirstOrDefault().ChineseName);
                //退款单对应订单信息
                orderRefundList.ForEach(k => k.Order = OrderBLL.Read(k.OrderId));
                //退款单对应订单详细信息
                orderRefundList.ForEach(k => k.Order.OrderDetailList = OrderDetailBLL.ReadList(k.OrderId));
                orderRefundList.ForEach(k => k.Order.OrderDetailList.ForEach(k1 => k1.ProductPhoto = ProductBLL.Read(k1.ProductId).Photo));
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = true, count = orderRefundList.Count, data = orderRefundList }));
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, count = 0 }));
                Response.End();
            }
        }
        /// <summary>
        /// 获取优惠券列表 
        /// </summary>
        protected void GetCoupons()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
            int pageIndex = RequestHelper.GetQueryString<int>("pageindex");
            int pageSize = RequestHelper.GetQueryString<int>("pagesize");
            int couponKind = RequestHelper.GetQueryString<int>("couponkind");
            //开始状态：未开始、进行中、已结束。默认: 进行中
            int timePeriod = RequestHelper.GetQueryString<int>("timeperiod") > 0 ? RequestHelper.GetQueryString<int>("timeperiod") : 2;

            CouponSearchInfo searchInfo = new CouponSearchInfo();
            if (couponKind > 0) searchInfo.Type = couponKind;
            if (timePeriod > 0) searchInfo.TimePeriod = timePeriod;
            int count = 0;
            var couponList = CouponBLL.SearchList(pageIndex, pageSize, searchInfo, ref count);
            //ajax开始日期
            couponList.ForEach(k => k.AjaxStartDate = string.Format("{0:yyyy-MM-dd}", k.UseStartDate));
            //ajax结束日期
            couponList.ForEach(k => k.AjaxEndDate = $"{k.UseEndDate:yyyy-MM-dd}");
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { ok = true, count = couponList.Count, data = couponList }));
            Response.End();
        }
        /// <summary>
        /// 添加/修改 优惠券
        /// </summary>
        protected void AddCoupon()
        {
            bool flag = true;
            string msg = string.Empty;
            try
            {
                if (!base.IsAdminLogin())
                {
                    flag = false;
                    msg = "登录已过期，请重新登录";
                }
                CouponInfo coupon = new CouponInfo();
                coupon.Id = RequestHelper.GetForm<int>("Id");
                coupon.Type = RequestHelper.GetForm<int>("Type") <= 0 ? (int)CouponKind.Common : RequestHelper.GetForm<int>("Type");
                coupon.Name = RequestHelper.GetForm<string>("Name");
                coupon.Money = RequestHelper.GetForm<decimal>("Money");
                coupon.UseMinAmount = RequestHelper.GetForm<decimal>("UseMinAmount");
                int days = RequestHelper.GetForm<int>("Days");
                if (coupon.Type == (int)CouponKind.Common)
                {//商家优惠券可以设置图片、总数
                    coupon.TotalCount = RequestHelper.GetForm<int>("TotalCount") < 0 ? 0 : RequestHelper.GetForm<int>("TotalCount");
                    coupon.Photo = RequestHelper.GetForm<string>("Photo");
                }
                if (string.IsNullOrEmpty(coupon.Name))
                {
                    flag = false;
                    msg = "请输入优惠券名称";
                }
                if (flag && coupon.Money <= 0)
                {
                    flag = false;
                    msg = "请输入有效的优惠券金额";
                }
                if (flag && coupon.UseMinAmount <=0)
                {
                    flag = false;
                    msg = "请输入有效的使用门槛";
                }
                if (flag && days <= 0)
                {
                    flag = false;
                    msg = "请输入正确的有效期";
                }
                if (flag)
                {
                    //新增
                    if (coupon.Id <= 0)
                    {
                        coupon.UseStartDate = DateTime.Now;
                    }
                    else
                    {
                        var theCoupon = CouponBLL.Read(coupon.Id);
                        coupon.UseStartDate = theCoupon.UseStartDate;
                        if (coupon.Type == (int)CouponKind.Common)
                        {//商家优惠券总数量不能小于已发放量                
                            if (coupon.TotalCount < theCoupon.UsedCount)
                            {
                                flag = false;
                                msg = "总数不得小于已发放数量";
                            }
                        }
                    }
                    coupon.UseEndDate = coupon.UseStartDate.AddDays(days);
                    if (coupon.Id <= 0)
                    {
                        coupon.Id = CouponBLL.Add(coupon);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Coupon"), coupon.Id);
                    }
                    else
                    {
                        
                        CouponBLL.Update(coupon);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Coupon"), coupon.Id);
                    }
                }
            }
            catch(Exception ex)
            {
                flag = false;
                msg = ex.Message;
            }
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { ok=flag,msg=msg}));
            Response.End();
        }
        /// <summary>
        /// 获取满立减活动列表
        /// </summary>
        private void GetActivities()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
            int pageIndex = RequestHelper.GetQueryString<int>("pageindex");
            int pageSize = RequestHelper.GetQueryString<int>("pagesize");
          
            //开始状态：未开始、进行中、已结束。默认: 进行中
            int timePeriod = RequestHelper.GetQueryString<int>("timeperiod") > 0 ? RequestHelper.GetQueryString<int>("timeperiod") : 2;          
            int count = 0;
            var activityList = FavorableActivityBLL.ReadList(pageIndex, pageSize, ref count,timePeriod);
            //ajax开始日期
            activityList.ForEach(k => k.AjaxStartDate = string.Format("{0:yyyy-MM-dd}", k.StartDate));
            //ajax结束日期
            activityList.ForEach(k => k.AjaxEndDate = $"{k.EndDate:yyyy-MM-dd}");
            //ajax 会员等级名称
            activityList.ForEach(k => k.UserGradeNames = GetUserGradeOfActivity(k.UserGrade));

            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { ok = true, count = activityList.Count, data = activityList }));
            Response.End();
        }
        /// <summary>
        /// 添加/修改  满立减活动
        /// </summary>
        private void AddFavorableActivity()
        {
            bool flag = true;
            string msg = string.Empty;
            try
            {
                if (!base.IsAdminLogin())
                {
                    flag = false;
                    msg = "登录已过期，请重新登录";
                }
                FavorableActivityInfo entity = new FavorableActivityInfo();
                entity.Id = RequestHelper.GetForm<int>("Id");             
                entity.Name = RequestHelper.GetForm<string>("Name");
                entity.UserGrade = RequestHelper.GetForm<string>("UserGrade");
                entity.ReduceMoney = RequestHelper.GetForm<decimal>("ReduceMoney");
                entity.OrderProductMoney = RequestHelper.GetForm<decimal>("OrderProductMoney");
                entity.StartDate = RequestHelper.GetForm<DateTime>("StartDate");
                entity.EndDate = RequestHelper.GetForm<DateTime>("EndDate");
                
                //默认整站订单优惠
                entity.Type = (int)FavorableType.AllOrders;
                //默认价格优惠
                entity.ReduceWay = 1;
                if (string.IsNullOrEmpty(entity.Name))
                {
                    flag = false;
                    msg = "请输入活动名称";
                }
                if (flag && string.IsNullOrEmpty(entity.UserGrade))
                {
                    flag = false;
                    msg = "请至少选择一个用户等级";
                }
                if (flag && entity.ReduceMoney<= 0)
                {
                    flag = false;
                    msg = "请输入有效的优惠金额";
                }
                if (flag && entity.OrderProductMoney <= 0)
                {
                    flag = false;
                    msg = "请输入有效的使用门槛";
                }
                if (flag && entity.EndDate<entity.StartDate)
                {
                    flag = false;
                    msg = "结束日期不得小于开始日期";
                }
                if (flag)
                {
                    if (entity.Id <= 0)
                    {
                        entity.Id = FavorableActivityBLL.Add(entity);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("FavorableActivity"), entity.Id);
                    }
                    else
                    {
                        FavorableActivityBLL.Update(entity);
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("FavorableActivity"), entity.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                msg = ex.Message;
            }
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { ok = flag, msg = msg }));
            Response.End();
        }
        /// <summary>
        /// 根据优惠活动选择的用户等级，获取对应的等级名称 
        /// </summary>
        /// <param name="userGrade"></param>
        /// <returns></returns>
        private string GetUserGradeOfActivity(string userGrade = "")
        {
            var userGrades = UserGradeBLL.ReadList();
            string result = string.Empty;
            List<string> gradeNames = new List<string>();
            if (!string.IsNullOrEmpty(userGrade))
            {
                string[] a = userGrade.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var b in a)
                {
                    UserGradeInfo gd = userGrades.Find(k => k.Id.ToString() == b) ?? new UserGradeInfo();
                    if (gd.Id > 0) gradeNames.Add(gd.Name);
                }
            }
            if (gradeNames.Count > 0)
            {
                result = string.Join(",", gradeNames);
            }
            return result;
        }
        /// <summary>
        /// 删除banner图片
        /// </summary>
        private void DeleteAdImage()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
            int id = RequestHelper.GetForm<int>("id");
            if (id <= 0)
            {
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的请求" }));
                Response.End();
            }
            else
            {
                AdImageBLL.Delete(id);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("FlashPhoto"), id);
                Response.Write(JsonConvert.SerializeObject(new { ok = true }));
                Response.End();
            }
        }
        /// <summary>
        /// 添加banner图片
        /// </summary>
        private void AddAdImage()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
           
            int id = RequestHelper.GetForm<int>("Id");
            AdImageInfo adImage = AdImageBLL.Read(id);
            adImage.AdType = RequestHelper.GetForm<int>("AdType");
            adImage.Title = RequestHelper.GetForm<string>("Title");
            adImage.ImageUrl = RequestHelper.GetForm<string>("ImageUrl");
            adImage.LinkUrl = RequestHelper.GetForm<string>("LinkUrl");
            adImage.OrderId = RequestHelper.GetForm<int>("OrderId")<0?0: RequestHelper.GetForm<int>("OrderId");
           
            if (string.IsNullOrEmpty(adImage.Title))
            {
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请输入标题" }));
                Response.End();
            }
            if (string.IsNullOrEmpty(adImage.ImageUrl))
            {
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "请上传图片" }));
                Response.End();
            }

            if (adImage.Id > 0)
            {
                //修改
                AdImageBLL.Update(adImage);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("AdImage"), adImage.Id);
            }
            else
            {
                //新增,必须先判断AdType
                if (adImage.AdType <= 0)
                {
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的请求" }));
                    Response.End();
                }
                adImage.Tm = DateTime.Now;
                id = AdImageBLL.Add(adImage);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("AdImage"), id);
            }
                Response.Write(JsonConvert.SerializeObject(new { ok = true }));
                Response.End();
           
        }

        /// <summary>
        /// 核销
        /// </summary>
        private void WriteOff()
        {
            if (!base.IsAdminLogin())
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "登录已失效，请重新登录" }));
                Response.End();
            }
            string pickCode = RequestHelper.GetQueryString<string>("pickUpCode");
            int checkCode = 0;
            OrderInfo order = PickUpCodeBLL.ReadByPickCode(pickCode, ref checkCode);
            if (checkCode == 1)
            {
                Response.Write(JsonConvert.SerializeObject(new { ok = true, msg = "", order = order }));
                Response.End();
            }
            else
            {
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的提货码" }));
                Response.End();
            }
        }
        #region 计算团购是否待退款、待审核
        private void NeedRefund()
        {
            bool needrefund = false, needcheck = false;
            int groupId = RequestHelper.GetQueryString<int>("id");
            int quantity = RequestHelper.GetQueryString<int>("quantity");
            int count = 0;
            var dataList = GroupSignBLL.SearchListByGroupId(Convert.ToInt32(groupId), 1, Convert.ToInt32(quantity), ref count);
            needrefund = dataList.Count(k => k.EndTime < DateTime.Now && k.Quantity > k.SignCount && k.GroupOrderStatus == 2 && k.IsRefund == 0) > 0;
            needcheck = dataList.Count(k => k.StartTime <= DateTime.Now && k.EndTime >= DateTime.Now && k.SignCount >= k.Quantity && k.GroupOrderStatus == 2) > 0;
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { needrefund = needrefund, needcheck = needcheck }));
            Response.End();
        }
        #endregion
        #region 团购订单后台退款
        /// <summary>
        /// 团购订单退款
        /// </summary>
        private void GroupBuyOrderRefund()
        {
            int orderId = RequestHelper.GetForm<int>("orderId");
            if (orderId <= 0)
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的操作" }));
                Response.End();
            }
            var order = OrderBLL.Read(orderId);
            #region 添加退款申请
            //如果是团购单，且拼团正在进行中，暂不能申请退款
            if (order.IsActivity == (int)OrderKind.GroupBuy && order.FavorableActivityId > 0)
            {
                var groupBuy = GroupBuyBLL.Read(order.FavorableActivityId);
                if (groupBuy.StartTime <= DateTime.Now && groupBuy.EndTime >= DateTime.Now && groupBuy.Quantity > groupBuy.SignCount)
                {
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "拼团正在进行，暂不能退款" }));
                    Response.End();
                }
            }

            //正在处理中的退款订单或商品
            var orderRefundList = OrderRefundBLL.ReadListValid(order.Id);
            //有正在处理中的退款订单或商品
            if (orderRefundList.Count(k => !OrderRefundBLL.HasReturn(k.Status) && k.UserId != 0 && k.RefundRemark != "拼团失败，系统自动退款") > 0)
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "该订单有正在处理中的退款" }));
                Response.End();
            }
            //自动匹配到上次系统自动退款的记录
            OrderRefundInfo orderRefund = orderRefundList.Find(k => !OrderRefundBLL.HasReturn(k.Status) && k.UserId == 0 && k.RefundRemark == "拼团失败，系统自动退款") ?? new OrderRefundInfo();
            //如果之前没有退款记录则新增退款申请记录
            if (orderRefund.Id <= 0)
            {
                orderRefund.RefundNumber = ShopCommon.CreateOrderRefundNumber();
                orderRefund.OrderId = orderId;
                orderRefund.Status = (int)OrderRefundStatus.Submit;
                orderRefund.TmCreate = DateTime.Now;
                orderRefund.RefundRemark = "拼团失败，系统自动退款";
                orderRefund.UserType = 1;
                orderRefund.UserId = 0;
                orderRefund.UserName = "";
              

                //默认退全部能退的额度
                var refundMsg = JWRefund.VerifySubmitOrderRefund(orderRefund, JWRefund.CanRefund(order).CanRefundMoney);
                if (refundMsg.CanRefund)
                {
                    int refundId = OrderRefundBLL.Add(orderRefund);
                    orderRefund.Id = refundId;
                    OrderRefundActionBLL.Add(new OrderRefundActionInfo
                    {
                        OrderRefundId = refundId,
                        Status = (int)BoolType.True,
                        Tm = DateTime.Now,
                        UserType = 1,
                        UserId = 0,
                        UserName = "",
                        Remark = "拼团失败，系统自动退款"
                    });
                }
                else
                {
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = refundMsg.ErrorCodeMsg }));
                    Response.End();
                }
            }
            #endregion
            #region 审核通过   
            if (orderRefund.Status == (int)OrderRefundStatus.Submit)
            {
                orderRefund.Status = (int)OrderRefundStatus.Approve;
                orderRefund.Remark = "系统审核通过，等待处理退款： ";
                OrderRefundBLL.Update(orderRefund);
                //退款操作记录
                AddOrderRefundAction(orderRefund, (int)BoolType.True);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("OrderRefund"), orderRefund.Id);
            }
            #endregion
            #region 退款处理
            //更改状态为退款中...
            if (orderRefund.Status == (int)OrderRefundStatus.Approve)
            {
                orderRefund.Status = (int)OrderRefundStatus.Returning;
                orderRefund.Remark = "正在处理退款";

                OrderRefundBLL.Update(orderRefund);

                //退款操作记录
                AddOrderRefundAction(orderRefund, (int)BoolType.True);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("OrderRefund"), orderRefund.Id);
            }

            //退款到账户余额及各支付渠道
            if (orderRefund.Status == (int)OrderRefundStatus.Returning)
            {

                string tradeNo = order.WxPayTradeNo;
                if (string.IsNullOrEmpty(tradeNo))
                {
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的微信支付交易号" }));
                    Response.End();
                }

                decimal totalMoney = order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney - order.PointMoney;
                if (orderRefund.RefundMoney > totalMoney)
                {
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "退款金额不能超过订单金额" }));
                    Response.End();
                }

                //商户退款单号
                //商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
                string batch_no = orderRefund.BatchNo;
                if (string.IsNullOrEmpty(batch_no))
                {
                    batch_no = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                    //记录退款批次号存入订单退款表
                    OrderRefundBLL.UpdateBatchNo(orderRefund.Id, batch_no);
                }

                /*******************请求参数验证 end*****************************************************************/


                //订单总金额
                string total_fee = Convert.ToInt32(totalMoney * 100).ToString();
                //退款金额
                string refund_fee = Convert.ToInt32(orderRefund.RefundMoney * 100).ToString();

                //申请退款
                /***
                * 申请退款完整业务流程逻辑
                * @param transaction_id 微信订单号（优先使用）
                * @param out_trade_no 商户订单号
                * @param out_refund_no 商户退款单号
                * @param total_fee 订单总金额
                * @param refund_fee 退款金额
                * @return 退款结果（xml格式）
                */

                try
                {
                    //小程序支付的退款业务逻辑
                    //bool isSuccess = JWShop.XcxApi.Pay.RefundBusiness.Run(tradeNo, "", batch_no, total_fee, refund_fee);
                    WxpayResult wxResult = JWShop.XcxApi.Pay.RefundBusiness.Run(tradeNo, "", batch_no, total_fee, refund_fee);
                    //bool isSuccess = true;
                    //if (isSuccess)
                    if(wxResult.result_code)
                    {
                        orderRefund.Status = (int)OrderRefundStatus.HasReturn;
                        orderRefund.TmRefund = DateTime.Now;
                        OrderRefundBLL.Update(orderRefund);

                        OrderRefundActionBLL.Add(new OrderRefundActionInfo
                        {
                            OrderRefundId = orderRefund.Id,
                            //Status = isSuccess ? 1 : 0,
                            Status = wxResult.result_code ? 1 : 0,
                            Tm = DateTime.Now,
                            UserType = 2,
                            UserId = 0,
                            UserName = "系统",
                            Remark = orderRefund.Remark
                        });

                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = true }));
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "微信退款失败,"+wxResult.err_code_des }));

                    }
                }
                catch (Exception ex)
                {
                    Response.Clear();
                    Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "微信退款出错, 请检查账户余额是否充足" }));
                    Response.End();
                }

            }
            else
            {
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(new { ok = false, msg = "无效的操作" }));
                Response.End();
            }

            #endregion
        }
        //增加退款操作记录
        private void AddOrderRefundAction(OrderRefundInfo entity, int status)
        {
            OrderRefundActionInfo submitOrderRefundAction = new OrderRefundActionInfo
            {
                OrderRefundId = entity.Id,
                Status = status,
                Remark = entity.Remark,
                Tm = DateTime.Now,
                UserType = 2,
                UserId = Cookies.Admin.GetAdminID(false),
                UserName = Cookies.Admin.GetAdminName(false)
            };

            OrderRefundActionBLL.Add(submitOrderRefundAction);
        }
        #endregion
        #region 订单退款操作，现已采用post，次方式暂未启用
        /*
        /// <summary>
        /// 订单退款操作
        /// </summary>
        protected void OrderRefund()
        {
            bool flag = true;
            string msg = string.Empty;
            string param = RequestHelper.GetForm<string>("param").ToLower();
            int id = RequestHelper.GetForm<int>("id");
            if (id <= 0)
            {
                flag = false;
                msg = "请求参数错误";              
            }
            OrderRefundInfo orderRefund = OrderRefundBLL.Read(id);
            switch (param)
            {
                case "approve":
                    if (orderRefund.Status == (int)OrderRefundStatus.Submit)
                    {
                        Approve((int)BoolType.True,orderRefund);
                    }
                    {
                        flag = false;
                        msg = "无效的操作";
                    }
                    break;
                case "reject":
                    if (orderRefund.Status == (int)OrderRefundStatus.Submit)
                    {
                        Approve((int)BoolType.False, orderRefund);
                    }
                    {
                        flag = false;
                        msg = "无效的操作";
                    }
                    break;
                case "finish":
                    //更改状态为退款中...
                    if (orderRefund.Status == (int)OrderRefundStatus.Approve)
                    {
                        orderRefund.Status = (int)OrderRefundStatus.Returning;
                        orderRefund.Remark = "正在处理退款";

                        OrderRefundBLL.Update(orderRefund);

                        //退款操作记录
                        AddOrderRefundAction(orderRefund, (int)BoolType.True);
                    }

                    //退款到账户余额及各支付渠道
                    if (orderRefund.Status == (int)OrderRefundStatus.Returning)
                    {
                        JWRefund.RefundToAnyPay(orderRefund);

                    }
                    else
                    {
                        flag = false;
                        msg = "无效的操作";                       
                    }
                    break;
                case "cancel":
                    if (orderRefund.Status == (int)OrderRefundStatus.Approve || orderRefund.Status == (int)OrderRefundStatus.Returning)
                    {
                        //更改状态为已取消...
                        orderRefund.Status = (int)OrderRefundStatus.Cancel;
                        orderRefund.Remark = "系统取消了退款";
                        OrderRefundBLL.Update(orderRefund);

                        //退款操作记录
                        AddOrderRefundAction(orderRefund, (int)BoolType.False);
                    }
                    else
                    {
                        flag = false;
                        msg = "无效的操作";
                    }
                    break;
            }
            Response.Clear();
            Response.Write(JsonConvert.SerializeObject(new { ok = flag,msg=msg }));
            Response.End();
        }
        /// <summary>
        /// 审核订单退款申请
        /// </summary>
        /// <param name="approveStatus">1：通过；0：拒绝</param>
        private void Approve(int approveStatus,OrderRefundInfo orderRefund)
        {        
            var submitOrderRefund = orderRefund;          
            switch (submitOrderRefund.Status)
            {
                case (int)OrderRefundStatus.Submit:
                    CheckAdminPower("OrderRefundApprove", PowerCheckType.Single);
                    if (approveStatus == (int)BoolType.True)
                    {
                        submitOrderRefund.Status = (int)OrderRefundStatus.Approve;
                        submitOrderRefund.Remark = "系统审核通过，等待处理退款： " ;
                    }
                    else
                    {
                        submitOrderRefund.Status = (int)OrderRefundStatus.Reject;
                        submitOrderRefund.Remark = "系统审核不通过： " ;
                    }
                    break;
             
            }

            OrderRefundBLL.Update(submitOrderRefund);

            //退款操作记录
            AddOrderRefundAction(submitOrderRefund, approveStatus);

            AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("OrderRefund"), submitOrderRefund.Id);
          
        }
        //增加退款操作记录
        private void AddOrderRefundAction(OrderRefundInfo entity, int status)
        {
            OrderRefundActionInfo submitOrderRefundAction = new OrderRefundActionInfo
            {
                OrderRefundId = entity.Id,
                Status = status,
                Remark = entity.Remark,
                Tm = DateTime.Now,
                UserType = 2,
                UserId = Cookies.Admin.GetAdminID(false),
                UserName = Cookies.Admin.GetAdminName(false)
            };

            OrderRefundActionBLL.Add(submitOrderRefundAction);
        }
        */
        #endregion
    }
}
