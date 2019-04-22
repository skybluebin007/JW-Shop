using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using JWShop.Common;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace JWShop.Web.Admin
{
    public partial class t1 : System.Web.UI.Page
    {
        //砍价---底价
        protected decimal reservePrice = 0;
        //砍价商品
        protected ProductInfo barGainProduct = new ProductInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                reservePrice = RequestHelper.GetQueryString<decimal>("reservePrice") <=0?0: RequestHelper.GetQueryString<decimal>("reservePrice");
                int pid = RequestHelper.GetQueryString<int>("pid");
                if (pid > 0)
                {                  
                    barGainProduct = ProductBLL.Read(pid);
                }
                if (RequestHelper.GetQueryString<int>("action") == 1)
                {
                    SaveImg();
                }
            }
        }
        private void SaveImg()
        {
            string html_url = "http://" + Request.Url.Host + (Request.Url.Port > 0 ? ":" + Request.Url.Port : "") + "/Admin/bargainshare/t1.aspx?reservePrice=0&pid=1005";
            Bitmap m_Bitmap = WebSiteThumbnail.GetWebSiteThumbnail(html_url, 320, 256, 320, 256);
            //Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(html_url, 640, 380, 640, 380);
            string imgName = "xcx_bargainshare_" + Guid.NewGuid().ToString() + ".png";
            string path = "/upload/bargainshare/";
            if (!Directory.Exists(Server.MapPath(path)))
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }
            m_Bitmap.Save(Server.MapPath(path + imgName), System.Drawing.Imaging.ImageFormat.Png);//JPG、GIF、PNG等均可
            ScriptHelper.AlertFront("保存成功");
        }
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btn1_Click(object sender, EventArgs e)
        //{
        //    string html_url = "http://xcxshop_bargain.com/Admin/bargainshare/t1.aspx?reservePrice=0&pid=1005";
        //    Bitmap m_Bitmap = WebSiteThumbnail.GetWebSiteThumbnail(html_url, 640, 380, 640, 380);
        //    //Bitmap m_Bitmap = WebSnapshotsHelper.GetWebSiteThumbnail(html_url, 640, 380, 640, 380);
        //    string imgName = "xcx_bargainshare_" + Guid.NewGuid().ToString() + ".png";
        //    string path = "/upload/bargainshare/";
        //    if (!Directory.Exists(Server.MapPath(path)))
        //    {
        //        Directory.CreateDirectory(Server.MapPath(path));
        //    }
        //    m_Bitmap.Save(Server.MapPath(path + imgName), System.Drawing.Imaging.ImageFormat.Png);//JPG、GIF、PNG等均可
           
        //    ScriptHelper.AlertFront("保存成功");      
        //}

        public class WebSnapshotsHelper
        {
            Bitmap m_Bitmap;
            string m_Url;
            int m_BrowserWidth, m_BrowserHeight, m_ThumbnailWidth, m_ThumbnailHeight;
          
            public WebSnapshotsHelper(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
            {
                m_Url = Url;
                m_BrowserHeight = BrowserHeight;
                m_BrowserWidth = BrowserWidth;
                m_ThumbnailWidth = ThumbnailWidth;
                m_ThumbnailHeight = ThumbnailHeight;
            }
            public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
            {
                WebSnapshotsHelper thumbnailGenerator = new WebSnapshotsHelper(Url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight);
                return thumbnailGenerator.GenerateWebSiteThumbnailImage();
            }
            public Bitmap GenerateWebSiteThumbnailImage()
            {
                Thread m_thread = new Thread(new ThreadStart(_GenerateWebSiteThumbnailImage));
                m_thread.SetApartmentState(ApartmentState.STA);
                m_thread.Start();
                m_thread.Join();
                return m_Bitmap;
            }
            private void _GenerateWebSiteThumbnailImage()
            {
                WebBrowser m_WebBrowser = new WebBrowser();
                m_WebBrowser.ScrollBarsEnabled = false;
                m_WebBrowser.Navigate(m_Url);
                    m_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                while (m_WebBrowser.ReadyState != WebBrowserReadyState.Complete)

                {
                    System.Windows.Forms.Application.DoEvents();
                }
                m_WebBrowser.Dispose();
               
                
            }
            private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                WebBrowser m_WebBrowser = (WebBrowser)sender;

                m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
                m_WebBrowser.ScrollBarsEnabled = false;

                m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
                    m_WebBrowser.BringToFront();
                    m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
                    m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
                
            }

        }
    }
}