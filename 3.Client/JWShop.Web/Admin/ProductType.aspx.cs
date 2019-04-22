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
    public partial class ProductType : JWShop.Page.AdminBasePage
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
                CheckAdminPower("ReadProductType", PowerCheckType.Single);
                BindControl(ProductTypeBLL.ReadList(), RecordList);
            }
        }

        /// <summary>
        /// 删除按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            CheckAdminPower("DeleteProductType", PowerCheckType.Single);
            string deleteID = RequestHelper.GetIntsForm("SelectID");
            if (deleteID != string.Empty)
            {
                ProductTypeBLL.DeleteList(deleteID);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("AttributeClass"), deleteID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }



        protected string GetBrandList(string ids)
        {
            string brandStr = string.Empty;
            string[] idArr = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string id in idArr)
            {
                ProductBrandInfo brand = ProductBrandBLL.Read(Convert.ToInt32(id));
                brandStr += brand.Name + ";";
            }
            return brandStr;
        }
    }
}