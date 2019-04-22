using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;
using Ionic.Zip;
using Ionic.Zlib;
using System.Data;
using System.IO;
using System.Text;
using System.Globalization;
namespace JWShop.Web.Admin
{
    public partial class ExportTaobo :  JWShop.Page.AdminBasePage
    {
        private readonly DirectoryInfo _baseDir= new DirectoryInfo(HttpContext.Current.Request.MapPath("~/Exportdata/"));
        private readonly Encoding _encoding = Encoding.UTF8;
      
        private readonly string _flag = DateTime.Now.ToString("yyyyMMddHHmmss");
        private readonly bool _includeCostPrice;
        private readonly bool _includeImages=true;
        private readonly bool _includeStock;
        private DirectoryInfo _productImagesDir;

        private  string _url;
        private DirectoryInfo _workDir;
        private readonly string _zipFilename = string.Format("taobao.{0}.{1}.zip", "5.0", DateTime.Now.ToString("yyyyMMddHHmmss"));
        private const string ExportVersion = "5.0";
        private const string ProductFilename = "products.csv";
        private string _province="湖南";
        private string _city="长沙";
        private List<ProductInfo> ExportList = new List<ProductInfo>();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckAdminPower("ReadProduct", PowerCheckType.Single);
                RegionID.DataSource = RegionBLL.ReadRegionUnlimitClass();
                RegionID.ClassID = "|1|27|607|";
                foreach (ProductClassInfo productClass in ProductClassBLL.ReadNamedList())
                {
                    ClassID.Items.Add(new ListItem(productClass.Name, "|" + productClass.Id + "|"));
                }
                ClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

                BrandID.DataSource = ProductBrandBLL.ReadList();
                BrandID.DataTextField = "Name";
                BrandID.DataValueField = "ID";
                BrandID.DataBind();
                BrandID.Items.Insert(0, new ListItem("所有品牌", string.Empty));

                ClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                BrandID.Text = RequestHelper.GetQueryString<string>("BrandID");
                Key.Text = RequestHelper.GetQueryString<string>("Key");
                StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");
                IsSpecial.Text = RequestHelper.GetQueryString<string>("IsSpecial");
                IsNew.Text = RequestHelper.GetQueryString<string>("IsNew");
                IsHot.Text = RequestHelper.GetQueryString<string>("IsHot");
                IsTop.Text = RequestHelper.GetQueryString<string>("IsTop");

                List<ProductInfo> productList = new List<ProductInfo>();
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.Key = RequestHelper.GetQueryString<string>("Key");
                productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
                productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
                productSearch.IsSpecial = RequestHelper.GetQueryString<int>("IsSpecial");
                productSearch.IsNew = RequestHelper.GetQueryString<int>("IsNew");
                productSearch.IsHot = RequestHelper.GetQueryString<int>("IsHot");
                productSearch.IsSale = (int)BoolType.True;
                productSearch.IsTop = RequestHelper.GetQueryString<int>("IsTop");
                productSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                productSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                productSearch.IsDelete = 0;//没有逻辑删除的商品
                PageSize = Session["AdminPageSize"] == null ? 20 : Convert.ToInt32(Session["AdminPageSize"]);
                AdminPageSize.Text = Session["AdminPageSize"] == null ? "20" : Session["AdminPageSize"].ToString();
                productList = ProductBLL.SearchList(CurrentPage, PageSize, productSearch, ref Count);
              
                BindControl(productList, RecordList, MyPager);
            }
        }
        /// <summary>
        /// 搜索按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string URL = "ExportTaobo.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text + "&";
            URL += "IsSpecial=" + IsSpecial.Text + "&";
            URL += "IsNew=" + IsNew.Text + "&";
            URL += "IsHot=" + IsHot.Text + "&";
            URL += "IsTop=" + IsTop.Text;
            ResponseHelper.Redirect(URL);
        }
        /// <summary>
        /// 每页显示条数控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["AdminPageSize"] = AdminPageSize.Text;
            string URL = "ExportTaobo.aspx?Action=search&";
            URL += "Key=" + Key.Text + "&"; ;
            URL += "ClassID=" + ClassID.Text + "&";
            URL += "BrandID=" + BrandID.Text + "&";
            URL += "StartAddDate=" + StartAddDate.Text + "&";
            URL += "EndAddDate=" + EndAddDate.Text + "&";
            URL += "IsSpecial=" + IsSpecial.Text + "&";
            URL += "IsNew=" + IsNew.Text + "&";
            URL += "IsHot=" + IsHot.Text + "&";
            URL += "IsTop=" + IsTop.Text;
            ResponseHelper.Redirect(URL);
        }
        /// <summary>
        /// 导出按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExportButton_Click(object sender, EventArgs e)
        {
            _url = "http://" + HttpContext.Current.Request.Url.Host + ((HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + HttpContext.Current.Request.Url.Port)) + this.ApplicationPath;
            //获取省市
            string[] strRegions=RegionID.ClassID.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
            if(strRegions.Length>=2) _province=RegionBLL.ReadRegionCache(Convert.ToInt32(strRegions[1])).RegionName.Replace("省","");
            if (strRegions.Length >= 3) _city = RegionBLL.ReadRegionCache(Convert.ToInt32(strRegions[2])).RegionName.Replace("市", ""); ; 

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Key = RequestHelper.GetQueryString<string>("Key");
            productSearch.ClassId = RequestHelper.GetQueryString<string>("ClassID");
            productSearch.BrandId = RequestHelper.GetQueryString<int>("BrandID");
            productSearch.IsSpecial = RequestHelper.GetQueryString<int>("IsSpecial");
            productSearch.IsNew = RequestHelper.GetQueryString<int>("IsNew");
            productSearch.IsHot = RequestHelper.GetQueryString<int>("IsHot");
            productSearch.IsSale = (int)BoolType.True;
            productSearch.IsTop = RequestHelper.GetQueryString<int>("IsTop");
            productSearch.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
            productSearch.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
            //待导出商品列表
            ExportList = ProductBLL.SearchList(productSearch);
            if (ExportList.Count <= 0) {
                ScriptHelper.Alert("请至少选择一个商品");
            }
            else {
                DoExport();
            }
        }


        private string CopyImage(string imageUrl, int index)
        {
            string str = string.Empty;
            if (imageUrl.StartsWith("http://"))
            {
                return str;
            }
            imageUrl = this.Trim(imageUrl);
            string path = HttpContext.Current.Request.MapPath("~" + imageUrl);
            if (!File.Exists(path))
            {
                return str;
            }
            FileInfo info = new FileInfo(path);
            string str3 = info.Name.ToLower();
            if ((!str3.EndsWith(".jpg") && !str3.EndsWith(".gif")) && ((!str3.EndsWith(".jpeg") && !str3.EndsWith(".png")) && !str3.EndsWith(".bmp")))
            {
                return str;
            }
            str3 = str3.Replace(info.Extension.ToLower(), ".tbi");
            info.CopyTo(Path.Combine(this._productImagesDir.FullName, str3), true);
            return (str + str3.Replace(".tbi", string.Format(":1:{0}:|;", index - 1)));
        }

        public  void DoExport()
        {
            this._workDir = this._baseDir.CreateSubdirectory(this._flag);
            this._productImagesDir = this._workDir.CreateSubdirectory("products");
            string path = Path.Combine(this._workDir.FullName, "products.csv");
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                string productCSV = this.GetProductCSV();
                UnicodeEncoding encoding = new UnicodeEncoding();
                int byteCount = encoding.GetByteCount(productCSV);
                byte[] preamble = encoding.GetPreamble();
                byte[] dst = new byte[preamble.Length + byteCount];
               System.Buffer.BlockCopy(preamble, 0, dst, 0, preamble.Length);
                encoding.GetBytes(productCSV.ToCharArray(), 0, productCSV.Length, dst, preamble.Length);
                stream.Write(dst, 0, dst.Length);
            }
            using (ZipFile file = new ZipFile())
            {
                file.CompressionLevel = CompressionLevel.Default;
                file.AddFile(path, "");
                file.AddDirectory(this._productImagesDir.FullName, this._productImagesDir.Name);
                HttpResponse response = HttpContext.Current.Response;
                response.ContentType = "application/x-zip-compressed";
                response.ContentEncoding = this._encoding;
                response.AddHeader("Content-Disposition", "attachment; filename=" + this._zipFilename);
                response.Clear();
                file.Save(response.OutputStream);
                this._workDir.Delete(true);
                response.Flush();
                response.Close();
            }
        }

        private string GetProductCSV()
        {
            StringBuilder builder = new StringBuilder();
            string format = "\"{0}\"\t{1}\t\"{2}\"\t{3}\t\"{4}\"\t\"{5}\"\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t\"{19}\"\t\"{20}\"\t\"{21}\"\t{22}\t{23}\t\"{24}\"\t{25}\t\"{26}\"\t{27}\t\"{28}\"\t\"{29}\"\t\"{30}\"\t\"{31}\"\t\"{32}\"\t\"{33}\"\t\"{34}\"\t{35}\t{36}\t{37}\t{38}\t\"{39}\"\t{40}\t{41}\t\"{42}\"\r\n";
            builder.Append("version 1.00\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n");
            builder.Append("title\tcid\tseller_cids\tstuff_status\tlocation_state\tlocation_city\titem_type\tprice\tauction_increment\tnum\tvalid_thru\tfreight_payer\tpost_fee\tems_fee");
            builder.Append("\texpress_fee\thas_invoice\thas_warranty\tapprove_status\thas_showcase\tlist_time\tdescription\tcateProps\tpostage_id\thas_discount\tmodified\tupload_fail_msg");
            builder.Append("\tpicture_status\tauction_point\tpicture\tvideo\tskuProps\tinputPids\tinputValues\touter_id\tpropAlias\tauto_fill\tnum_id\tlocal_cid\tnavigation_type\tuser_name\tsyncStatus\tsubtitle\twireless_desc\r\n");
            builder.Append("宝贝名称\t宝贝类目\t店铺类目\t新旧程度\t省\t城市\t出售方式\t宝贝价格\t加价幅度\t宝贝数量\t有效期\t运费承担\t平邮\tEMS\t快递\t发票\t保修\t放入仓库\t橱窗推荐\t开始时间\t宝贝描述");
            builder.Append("\t宝贝属性\t邮费模版ID\t会员打折\t修改时间\t上传状态\t图片状态\t返点比例\t新图片\t视频\t销售属性组合\t用户输入ID串\t用户输入名-值对\t商家编码\t销售属性别名\t代充类型\t数字ID\t本地ID");
            builder.Append("\t宝贝分类\t账户名称\t宝贝状态\t宝贝卖点(摘要)\t无线详情(手机详情描述)\r\n");
            foreach (var item in ExportList)
            {
                string str2 = string.Empty; ;//pc端详情描述,最少5字符，最多25000字符
               
         if(item.Introduction1.Length>=5)  str2 = this.Trim(StringHelper.Substring(item.Introduction1,24900,false)).Replace(string.Format("src=\"{0}/upload/attached/image", this.ApplicationPath), string.Format("src=\"{0}/upload/attached/image", this._url));
                         
              
               //手机端详情描述----不能直接调用本站的
                //string mobileContent = string.IsNullOrEmpty(item.Introduction1_Mobile) ? item.Introduction1 : item.Introduction1_Mobile;
                string mobileContent = string.Empty;
                mobileContent = this.Trim(mobileContent).Replace(string.Format("src=\"{0}/upload/attached/image", this.ApplicationPath), string.Format("src=\"{0}/upload/attached/image", this._url));
                
                string str3 = string.Empty; //摘要
                if (!string.IsNullOrEmpty(item.Summary))
                {
                    str3 = this.Trim(Convert.ToString(item.Summary).Trim());
                    //if (!string.IsNullOrEmpty(str3) && (str3.Length > 0))
                    //{
                    //    str2 = str3 + "<br/>" + str2;
                    //}
                }
                str2 = str2.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\"", "\"\"");
                mobileContent = mobileContent.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\"", "\"\"");
                string str4 = string.Empty;
                if (!string.IsNullOrEmpty(item.Photo))
                {
                    str4 = str4 + this.CopyImage(item.Photo, 1);
                }
                int phi = 2;
                foreach (var productPhoto in ProductPhotoBLL.ReadList(item.Id, 0).Take(4))
                {
                   
                    if (!string.IsNullOrEmpty(productPhoto.ImageUrl))
                    {
                        str4 = str4 + this.CopyImage(productPhoto.ImageUrl.Replace("75-75","Original"), phi);
                        phi++;
                    }
                }
                //if (row["ImageUrl2"] != DBNull.Value)
                //{
                //    str4 = str4 + this.CopyImage((string)row["ImageUrl2"], 2);
                //}
                //if (row["ImageUrl3"] != DBNull.Value)
                //{
                //    str4 = str4 + this.CopyImage((string)row["ImageUrl3"], 3);
                //}
                //if (row["ImageUrl4"] != DBNull.Value)
                //{
                //    str4 = str4 + this.CopyImage((string)row["ImageUrl4"], 4);
                //}
                //if (row["ImageUrl5"] != DBNull.Value)
                //{
                //    str4 = str4 + this.CopyImage((string)row["ImageUrl5"], 5);
                //}
                //DataRow[] rowArray = this._exportData.Tables["skus"].Select("ProductId=" + row["ProductId"].ToString(), "SalePrice desc");
                string str5 = "0";
                int num = 0;
                string str6 = "1";
                string str7 = "0";
                string str8 = "0";
                string str9 = "0";
                string str10 = "";
                string str11 = "";//分类ID
                string str12 = "";
                string str13 =item.ProductCode;
                string str14 = "";
                string str15 = "";
                string str16 = "";
                string str17 = "";
                string str18 = "";
                string str19 = "";
                string str20 = "";
                string str21 = "";
                string str22 = "";
                string str23 = "";
                string str24 = "";
                string str25 = "";
                //DataRow[] rowArray2 = this._exportData.Tables["TaobaoSku"].Select("ProductId=" + row["Productid"].ToString());
                //if (rowArray2.Length > 0)
                //{
                //    if (this._includeStock)
                //    {
                //        if ((rowArray2[0]["SkuQuantities"] != null) && (rowArray2[0]["SkuQuantities"].ToString() != ""))
                //        {
                //            string[] strArray = null;
                //            if (rowArray2[0]["SkuQuantities"].ToString().Contains(","))
                //            {
                //                strArray = rowArray2[0]["SkuQuantities"].ToString().Split(new char[] { ',' });
                //            }
                //            else
                //            {
                //                strArray = new string[] { rowArray2[0]["SkuQuantities"].ToString() };
                //            }
                //            foreach (string str26 in strArray)
                //            {
                //                num += Convert.ToInt32(str26);
                //            }
                //        }
                //        else
                //        {
                //            num += Convert.ToInt32(rowArray2[0]["Num"]);
                //        }
                //    }
                    //str20 = Convert.ToString(rowArray2[0]["LocationState"]);
                    //str21 = Convert.ToString(rowArray2[0]["LocationCity"]);
                    //str22 = (Convert.ToString(rowArray2[0]["HasInvoice"]).ToLower() == "true") ? "1" : "0";
                    //str23 = (Convert.ToString(rowArray2[0]["HasWarranty"]).ToLower() == "true") ? "1" : "0";
                    //str24 = (Convert.ToString(rowArray2[0]["HasDiscount"]).ToLower() == "true") ? "1" : "0";
                    //str25 = (rowArray2[0]["StuffStatus"].ToString() == "new") ? "1" : "0";
                str20 = _province;
                str21 = _city;
                str22 = "0";
                str23 = "0";
                str24 = "0";
                str25 = "1" ;
                    //if (Convert.ToString(rowArray2[0]["FreightPayer"]) == "buyer")
                    //{
                    //    str6 = "2";
                    //    str7 = Convert.ToString(rowArray2[0]["PostFee"]);
                    //    str8 = Convert.ToString(rowArray2[0]["ExpressFee"]);
                    //    str9 = Convert.ToString(rowArray2[0]["EMSFee"]);
                    //}
                    //str11 = Convert.ToString(rowArray2[0]["Cid"]);
                    //str14 = Convert.ToString(rowArray2[0]["PropertyAlias"]);
                    //str10 = Convert.ToString(rowArray2[0]["inputpids"]);
                    //str12 = Convert.ToString(rowArray2[0]["inputstr"]);
                    //str17 = Convert.ToString(rowArray2[0]["SkuQuantities"]);
                    //str18 = Convert.ToString(rowArray2[0]["skuPrices"]);
                    //str16 = Convert.ToString(rowArray2[0]["SkuProperties"]);
                    //str19 = Convert.ToString(rowArray2[0]["SkuOuterIds"]);
                    //if (!string.IsNullOrEmpty(str17))
                    //{
                    //    string[] strArray2 = str17.Split(new char[] { ',' });
                    //    string[] strArray3 = str18.Split(new char[] { ',' });
                    //    string[] strArray4 = str19.Split(new char[] { ',' });
                    //    string[] strArray5 = str16.Split(new char[] { ',' });
                    //    for (int i = 0; i < strArray2.Length; i++)
                    //    {
                    //        string str27 = str15;
                    //        str15 = str27 + strArray3[i] + ":" + strArray2[i] + ":" + strArray4[i] + ":" + strArray5[i] + ";";
                    //    }
                    //}
                //}
                //else if (this._includeStock && (rowArray.Length > 0))
                //{
                //    foreach (DataRow row2 in rowArray)
                //    {
                //        num += (int)row2["Stock"];
                //    }
                //}
                //计算剩余库存量
                int leftStorageCount = 0;
                if (item.StandardType != 1)
                {
                    if (ShopConfig.ReadConfigInfo().ProductStorageType == (int)ProductStorageType.SelfStorageSystem)
                    {
                        leftStorageCount = item.TotalStorageCount - item.OrderCount;
                    }
                    else
                    {
                        leftStorageCount = item.ImportVirtualStorageCount;
                    }
                }
                else {
                    List<ProductTypeStandardRecordInfo> standRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(item.Id, 1);
                    if (standRecordList.Count > 0) leftStorageCount = standRecordList[0].Storage;//取第一种规格的库存
                }
                str5 = leftStorageCount.ToString();//库存数量
                string spostage_id = "0";//邮费模板ID
                //if (rowArray.Length > 0)
                //{
                    builder.AppendFormat(format, new object[] { 
                        this.Trim(item.Name), str11, "", str25, str20, str21, "1", item.SalePrice, "", str5, "14", str6, str7, str9, str8, str22, 
                        str23, "0", "0", "", str2, str14, spostage_id, str24, DateTime.Now, "100", "", "0", str4, string.Empty, str15, str10, 
                        str12, str13, string.Empty, "0", "0", "0", "1", "", "1",str3,mobileContent
                     });
                //}
            }
            return builder.Remove(builder.Length - 2, 2).ToString();
        }

        private string Trim(string str)
        {
            while (str.StartsWith("\""))
            {
                str = str.Substring(1);
            }
            while (str.EndsWith("\""))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        private  string ApplicationPath
        {
            get
            {
                string applicationPath = "/";
                if (HttpContext.Current != null)
                {
                    try
                    {
                        applicationPath = HttpContext.Current.Request.ApplicationPath;
                    }
                    catch
                    {
                        applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                    }
                }
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                return applicationPath.ToLower(CultureInfo.InvariantCulture);
            }
        }
    }
}