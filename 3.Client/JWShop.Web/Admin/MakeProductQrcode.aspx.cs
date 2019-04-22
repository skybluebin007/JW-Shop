using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using ThoughtWorks.QRCode.Codec;
using System.IO;

namespace JWShop.Web.Admin
{
    public partial class MakeProductQrcode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            List<ProductInfo> productList = ProductBLL.ReadList();
            foreach (ProductInfo pro in productList)
            {
                #region 删除原有的二维码
                if(!string.IsNullOrEmpty(pro.Qrcode)){
                    string codeName = pro.Qrcode;
                    if (File.Exists(Server.MapPath(codeName)))
                    {
                        File.Delete(Server.MapPath(codeName));
                    }
                }
                #endregion
                #region 生成新的二维码
                string ewmName = string.Empty;//二维码路径
                CreateQRcode("http://" + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "") + "/mobile/ProductDetail-i" + pro.Id + ".html", "pro_" + pro.Id.ToString(), ref ewmName);             
                pro.Qrcode = ewmName;
                ProductBLL.Update(pro);
                #endregion
            }
            ScriptHelper.Alert("操作完成", RequestHelper.RawUrl);
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="str"></param>
        protected void CreateQRcode(string url, string str, ref string imageName)
        {
            Bitmap bt;
            string enCodeString = url;
            //生成设置编码实例
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置二维码的规模，默认4
            qrCodeEncoder.QRCodeScale = 10;
            //设置二维码的版本，默认7
            qrCodeEncoder.QRCodeVersion = 8;
            //设置错误校验级别，默认中等
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            //生成二维码图片
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            //二维码图片的名称
            string filename = str + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            imageName = "/Upload/QRcode/" + filename + ".jpg";
            //保存二维码图片在photos路径下
            bt.Save(Server.MapPath("~/Upload/QRcode/") + filename + ".jpg");

        }
    }
}