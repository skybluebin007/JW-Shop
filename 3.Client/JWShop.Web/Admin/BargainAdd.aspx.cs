using JWShop.Business;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JWShop.Web.Admin
{
    public partial class BargainAdd : JWShop.Page.AdminBasePage
    {

        /// <summary>
        /// 活动商品详情
        /// </summary>
        protected List<BargainDetailsInfo> DetailList = new List<BargainDetailsInfo>();
        //活动状态,默认开启
        protected int Status = (int)Bargain_Status.OnGoing;
        protected bool enableUpdate = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //默认开始时间是当前  结束时间2天后
                StartDate.Text =DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss");
                EndDate.Text = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd hh:mm:ss");

                var id = RequestHelper.GetQueryString<int>("ID");

                string Action = RequestHelper.GetQueryString<string>("Action");
                if (Action == "Delete")
                {
                    Delete();
                }
                if (id >0)
                {
                    CheckAdminPower("ReadBargain", PowerCheckType.Single);
                    var bargain = BargainBLL.ReadBargain(id);
                    Name.Text = bargain.Name;
                    StartDate.Text = bargain.StartDate.ToString("yyyy-MM-dd hh:mm:ss");
                    EndDate.Text = bargain.EndDate.ToString("yyyy-MM-dd hh:mm:ss");
                    //if (bargain.LimitCount == int.MaxValue)
                    //{
                    //    //Unlimited.Checked = true;
                    //}
                    //else
                    //{
                    //    LimitCount.Text = bargain.LimitCount.ToString();
                    //}
                    LimitCount.Text = bargain.LimitCount.ToString();
                    NumberPeople.Text = bargain.NumberPeople.ToString();
                    SuccessRate.Text = bargain.SuccessRate.ToString();
                    Status = bargain.Status;
                    DetailList = BargainDetailsBLL.ReadByBargainId(bargain.Id);

                    if (bargain.Id > 0 && bargain.EndDate < DateTime.Now)
                    {
                        SubmitButton.Visible = false;
                        enableUpdate = false;
                    }
                    if (bargain.Id > 0 && bargain.EndDate >= DateTime.Now && bargain.StartDate <= DateTime.Now)
                    {
                        SubmitButton.Visible = false;
                        enableUpdate = false;
                    }
                }


            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            var ID = RequestHelper.GetQueryString<int>("ID");
            var bargain = BargainBLL.ReadBargain(ID);
            if(bargain.Id>0 && bargain.EndDate < DateTime.Now)
            {
                ScriptHelper.Alert("活动已结束，不能编辑");
            }
            if (bargain.Id > 0 && bargain.EndDate >= DateTime.Now && bargain.StartDate<=DateTime.Now)
            {
                ScriptHelper.Alert("活动进行中，不能编辑");
            }
            bargain.Name = Name.Text;
            bargain.StartDate = DateTime.Parse(StartDate.Text);
            bargain.EndDate = DateTime.Parse(EndDate.Text);
            if (bargain.StartDate >= bargain.EndDate)
            {
                ScriptHelper.Alert("结束时间不能小于开始时间");
            }

            //if (!Unlimited.Checked && string.IsNullOrEmpty(LimitCount.Text))
            //{
            //    ScriptHelper.Alert("请填写助力限制次数");
            //}
            //bargain.LimitCount = Unlimited.Checked ? int.MaxValue : int.Parse(LimitCount.Text);
            if (string.IsNullOrEmpty(LimitCount.Text))
            {
                ScriptHelper.Alert("请填写助力限制次数");
            }
            bargain.LimitCount =int.Parse(LimitCount.Text);
            bargain.NumberPeople = int.Parse(NumberPeople.Text);
            //bargain.SuccessRate = int.Parse(SuccessRate.Text);
            //砍价成功率默认100%
            bargain.SuccessRate = 100;
            //记录改变前的状态
            int original_Status = bargain.Status;
            bargain.Status = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$Status") <= 0 ? 0 : 1;          
            string alertMessage = string.Empty;
            try
            {
                #region 参加活动的商品
                //原始
                List<BargainDetailsInfo> original_b_details = BargainDetailsBLL.ReadByBargainId(bargain.Id);
                //待新增、修改
                List<BargainDetailsInfo> B_Details =new List<BargainDetailsInfo>();

                var Ids = Array.ConvertAll(RequestHelper.GetForm<string>("Id").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return int.Parse(s); });
                var Product_Id = Array.ConvertAll(RequestHelper.GetForm<string>("Product_Id").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return int.Parse(s); });
                var BargainId = Array.ConvertAll(RequestHelper.GetForm<string>("BargainId").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return int.Parse(s); });
                var Product_Name = RequestHelper.GetForm<string>("Product_Name").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //var Product_OriginalPrice = RequestHelper.GetForm<string>("Product_OriginalPrice");

                var product_OriginalPrice = Array.ConvertAll(RequestHelper.GetForm<string>("product_OriginalPrice").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), k => decimal.Parse(k));

                var Product_ReservePrice = Array.ConvertAll(RequestHelper.GetForm<string>("Product_ReservePrice").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return decimal.Parse(s); });
                var Stock = Array.ConvertAll(RequestHelper.GetForm<string>("Stock").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return int.Parse(s); });
                var Sort = Array.ConvertAll(RequestHelper.GetForm<string>("Sort").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return int.Parse(s); });
                //是否在售
                var Product_IsSale = string.IsNullOrWhiteSpace(RequestHelper.GetForm<string>("Product_IsSale")) ? null : Array.ConvertAll(RequestHelper.GetForm<string>("Product_IsSale").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return int.Parse(s); });
                //是否删除
                var Product_IsDelete = string.IsNullOrWhiteSpace(RequestHelper.GetForm<string>("Product_IsDelete")) ? null : Array.ConvertAll(RequestHelper.GetForm<string>("Product_IsDelete").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return int.Parse(s); });
                //商品表编号
                var Product_Real_Id = string.IsNullOrWhiteSpace(RequestHelper.GetForm<string>("Product_Real_Id")) ? null : Array.ConvertAll(RequestHelper.GetForm<string>("Product_Real_Id").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), delegate (string s) { return int.Parse(s); });

                //正在参加砍价的商品
                var _bargainProducts = BargainDetailsBLL.ReadBargainProducts();
                for (int i = 0; i < Product_Id.Length; i++)
                {
                    //添加：Product_Id[i]>0
                    //修改：Product_Id[i] > 0  && Product_Real_Id[i]==Product_Id[i] 商品表必须存在此商品
                    if ((Ids[i] <= 0 && Product_Id[i] > 0) || (Ids[i] > 0 && Product_Id[i] > 0 && Product_Real_Id != null && Product_Real_Id[i] == Product_Id[i]))
                    {

                        #region 是否下架或删除
                        if (Product_IsSale != null && Product_IsSale.Length > i)
                        {
                            if (Product_IsSale[i] != 1) alertMessage = "商品：【" + Product_Name[i] + "】已下架，不能添加";
                        }
                        if (Product_IsDelete != null && Product_IsDelete.Length > i)
                        {
                            if (Product_IsDelete[i] != 0) alertMessage = "商品：【" + Product_Name[i] + "】已删除，不能添加";
                        }
                        #endregion
                        #region 同一商品同时只能参加1个砍价活动
                        if (_bargainProducts.Any(k => k.ProductID == Product_Id[i]) && Ids[i] <= 0)
                        {
                            alertMessage = "商品：【" + Product_Name[i] + "】正在参与砍价活动，不得重复添加";
                        }
                        #endregion
                        if (string.IsNullOrEmpty(alertMessage))
                        {
                            //判断商品底价不得超过原价
                            if (product_OriginalPrice[i] <= Product_ReservePrice[i])
                            {
                                alertMessage = "商品底价必须小于原价";
                                //ScriptHelper.Alert("商品：【"+Product_Name[i]+"】底价必须大于【"+(product_OriginalPrice[i] - product_OriginalPrice[i] * bargain.SuccessRate/100) +"】");
                                //return;
                            }
                            B_Details.Add(new BargainDetailsInfo()
                            {
                                Id = Ids[i],
                                ProductID = Product_Id[i],
                                ReservePrice = Product_ReservePrice[i],
                                Sort = Sort[i],
                                Stock = Stock[i],
                                BargainId = BargainId[i],
                                ProductName = Product_Name[i],
                                ShareImage1= (original_b_details.FirstOrDefault(k=>k.Id==Ids[i])??new BargainDetailsInfo()).ShareImage1
                            });
                        }
                    }
                }

                #endregion


                if (string.IsNullOrEmpty(alertMessage))
                {
                    alertMessage = ShopLanguage.ReadLanguage("AddOK");
                    if (ID <=0)
                    {
                        CheckAdminPower("AddBargain", PowerCheckType.Single);
                        int id = BargainBLL.AddBargain(bargain);
                        B_Details.ForEach(item => { item.BargainId = id;item.ShareImage1 = CreateBargainShareImage(item.ReservePrice, item.ProductID); BargainDetailsBLL.AddBargainDetails(item); });

                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Bargain"), id);
                    }
                    else
                    {
                        CheckAdminPower("UpdateBargain", PowerCheckType.Single);
                        BargainBLL.UpdateBargain(bargain);
                        #region 修改时关闭活动，则将未支付成功的砍价全部置为“砍价失败”，原“活动已取消，砍价失败”
                        if (original_Status == (int)Bargain_Status.OnGoing && bargain.Status == (int)Bargain_Status.ShutDown)
                        {
                            //异步 关闭
                            ShutDownBargain(bargain.Id);
                        }
                        #endregion

                        B_Details.ForEach(item =>
                        {
                            item.BargainId = bargain.Id;
                            item.ShareImage1 = CreateBargainShareImage(item.ReservePrice, item.ProductID,item.ShareImage1);
                            if (item.Id > 0)
                            {
                                BargainDetailsBLL.UpdateBargainDetails(item);
                            }
                            else
                            {
                                BargainDetailsBLL.AddBargainDetails(item);
                            }
                        });
                        AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Bargain"), bargain.Id);
                        alertMessage = ShopLanguage.ReadLanguage("UpdateOK");

                    }
                }
            }
            catch (Exception ex)
            {
                new TxtLog(Server.MapPath("/apilog/")).Write("-----bargainadd error:" + ex);
                ScriptHelper.Alert("商品底价、库存、排序号 填写不规范，请检查");
            }
            if (alertMessage == ShopLanguage.ReadLanguage("AddOK") || alertMessage == ShopLanguage.ReadLanguage("UpdateOK"))
            {
                ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
            }
            else
            {
                ScriptHelper.Alert(alertMessage);
            }
        }
        /// <summary>
        /// 关闭“砍价活动”
        /// </summary>
        /// <param name="id"></param>
        private async void ShutDownBargain(int id)
        {
            await Task.Run(() => {
                BargainBLL.ChangeBargainStatus(id, (int)Bargain_Status.ShutDown);
            });
        }
        private void Delete()
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            if (id > 0)
            {
                var bargain = BargainBLL.ReadBargain(id);
                if (bargain.Id > 0 && bargain.EndDate >= DateTime.Now && bargain.StartDate <= DateTime.Now)
                {
                    ScriptHelper.Alert("活动进行中，不能删除");
                }
                else
                {
                    BargainDetailsBLL.Delete(id);
                }
            }
        }
        /// <summary>
        /// 生成砍价分享图片,返回图片名称
        /// </summary>
        /// <param name="reservePrice">砍价---底价</param>
        /// <param name="pid">砍价---商品</param>
        private string CreateBargainShareImage(decimal reservePrice,int pid,string shareImage1="")
        {
            string result = string.Empty;
            try
            {
                string path = "/upload/bargainshare/";
                #region 删掉原图
                if (!string.IsNullOrWhiteSpace(shareImage1))
                {
                    if(File.Exists(Server.MapPath(path+shareImage1)))
                    {
                        File.Delete(Server.MapPath(path + shareImage1));
                    }
                }
                #endregion
                //string html_url = "http://"+ Request.Url.Host+(Request.Url.Port>0?":"+Request.Url.Port:"") +"/Admin/bargainshare/t1.aspx?reservePrice=" + reservePrice + "&pid=" + pid;      
                string html_url = "http://" + Request.Url.Host + "/Admin/bargainshare/t1.aspx?reservePrice=" + reservePrice + "&pid=" + pid;
                Bitmap m_Bitmap = WebSiteThumbnail.GetWebSiteThumbnail(html_url, 320, 256, 320, 256);                
                string imgName = "xcx_bargainshare_" + Guid.NewGuid().ToString() + ".png";             
                if (!Directory.Exists(Server.MapPath(path)))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }
                m_Bitmap.Save(Server.MapPath(path + imgName), System.Drawing.Imaging.ImageFormat.Png);//JPG、GIF、PNG等均可              
                result = path+imgName;
            }
            catch(Exception ex)
            {
                new TxtLog(Server.MapPath("/apilog/")).Write("-----生成砍价分享图片错误:" + ex + "----");
            }            
            return result;
           
        }

    }
}