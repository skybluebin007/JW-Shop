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
using System.Data;
using System.IO;
using Ionic.Zip;

namespace JWShop.Web.Admin
{
    public partial class ImportProduct : System.Web.UI.Page
    {
        private string _dataPath = ServerHelper.MapPath("~/Upload/taobao");
        private string _photoPath = ServerHelper.MapPath("~/Upload/TaoBaoPhoto");
        private readonly DirectoryInfo _baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/Upload/taobao"));

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void ImportProducts(object sender, EventArgs e)
        {
            string categoryId = ProductClass.ClassID;
            int saleStatus = Convert.ToInt32(IsSale.SelectedValue);
            string selectedValue = dropFiles.SelectedValue;
            selectedValue = Path.Combine(_dataPath, selectedValue);

            if (!File.Exists(selectedValue))
            {
                ScriptHelper.Alert("选择的数据包文件有问题！");
            }
            else
            {
                PrepareDataFiles(new object[] { selectedValue });
                string path = Path.Combine(_dataPath, Path.GetFileNameWithoutExtension(selectedValue));
                DataTable productData = (DataTable)ProductBLL.ParseProductData(new object[] { path })[0];
                if ((productData != null) && (productData.Rows.Count > 0))
                {
                    foreach (DataRow row in productData.Rows)
                    {                        
                        ProductInfo product = new ProductInfo
                        {
                            ClassId = categoryId,
                            Name = (string)row["ProductName"],
                            ProductNumber = (string)row["SKU"],
                            BrandId = 0
                        };
                        if (row["Description"] != DBNull.Value)
                        {
                            product.Introduction1 = (string)row["Description"];
                        }
                        product.AddDate = DateTime.Now;
                        product.IsSale = saleStatus;
                        product.MarketPrice = (decimal)row["SalePrice"];
                        product.TotalStorageCount = (int)row["Stock"];

                        product.Spelling = ChineseCharacterHelper.GetFirstLetter((string)row["ProductName"]);
                        product.SendPoint = 0;
                        product.Weight = 0;
                        product.IsSpecial = 0;
                        product.IsNew = 0;
                        product.IsHot = 0;
                        if (row["Has_ShowCase"] != DBNull.Value)
                        {
                            product.IsTop = Convert.ToInt32(row["Has_ShowCase"]);
                        }
                        product.AllowComment = 0;
                        product.LowerCount = 0;
                        product.UpperCount = 0;
                        product.StandardType = 0;
                        product.AddDate = RequestHelper.DateNow;

                        product.OrderId = 0;
                        product.SalePrice = (decimal)row["SalePrice"];

                        if (row["ImageUrl1"] != DBNull.Value)
                        {
                            product.Photo = (string)row["ImageUrl1"];
                        }

                        int proID = ProductBLL.Add(product);
                        #region 生成缩略图和产品相册图
                        if (!(string.IsNullOrEmpty(product.Photo) || (product.Photo.Length <= 0)))
                        {
                            UploadImage(product.Photo,PhotoType.Product);
                        }                        
                        
                        if (row["ImageUrl2"] != DBNull.Value)
                        {
                            ProductPhotoInfo tempPhoto = new ProductPhotoInfo()
                            {
                                ProductId = proID,
                                Name = "ImageUrl2",
                                ImageUrl = (string)row["ImageUrl2"],
                                AddDate = DateTime.Now
                            };
                            ProductPhotoBLL.Add(tempPhoto);
                            UploadImage(tempPhoto.ImageUrl, PhotoType.ProductPhoto);
                        }
                        if (row["ImageUrl3"] != DBNull.Value)
                        {
                            ProductPhotoInfo tempPhoto = new ProductPhotoInfo()
                            {
                                ProductId = proID,
                                Name = "ImageUrl3",
                                ImageUrl = (string)row["ImageUrl3"],
                                AddDate = DateTime.Now
                            };
                            ProductPhotoBLL.Add(tempPhoto);
                            UploadImage(tempPhoto.ImageUrl, PhotoType.ProductPhoto);
                        }
                        if (row["ImageUrl4"] != DBNull.Value)
                        {
                            ProductPhotoInfo tempPhoto = new ProductPhotoInfo()
                            {
                                ProductId = proID,
                                Name = "ImageUrl4",
                                ImageUrl = (string)row["ImageUrl4"],
                                AddDate = DateTime.Now
                            };
                            ProductPhotoBLL.Add(tempPhoto);
                            UploadImage(tempPhoto.ImageUrl, PhotoType.ProductPhoto);
                        }
                        if (row["ImageUrl5"] != DBNull.Value)
                        {
                            ProductPhotoInfo tempPhoto = new ProductPhotoInfo()
                            {
                                ProductId = proID,
                                Name = "ImageUrl5",
                                ImageUrl = (string)row["ImageUrl5"],
                                AddDate = DateTime.Now
                            };
                            ProductPhotoBLL.Add(tempPhoto);
                            UploadImage(tempPhoto.ImageUrl, PhotoType.ProductPhoto);
                        }
                        #endregion                                             
                    }

                    File.Delete(selectedValue);
                    Directory.Delete(path, true);
                    BindFiles();
                    ScriptHelper.Alert("此次商品批量导入操作已成功！", RequestHelper.RawUrl);
                }
            }
        }

        protected void UploadImage(string fileName,PhotoType photoType)
        {
            //取得传递值
            string fileType = ShopConfig.ReadConfigInfo().UploadImage;

            try
            {
                //上传文件
                UploadHelper upload = new UploadHelper();
                upload.Path = _photoPath + "/Original/";
                upload.FileType = fileType;
                upload.FileNameType = FileNameType.Guid;
                upload.MaxWidth = ShopConfig.ReadConfigInfo().AllImageWidth;//整站图片压缩开启后的压缩宽度
                upload.AllImageIsNail = ShopConfig.ReadConfigInfo().AllImageIsNail;//整站图片压缩开关
                int needNail = RequestHelper.GetQueryString<int>("NeedNail");
                if (needNail == 0) upload.AllImageIsNail = 0;//如果页面有传值且值为0则不压缩图片，以页面传值为准;
                int curMaxWidth = RequestHelper.GetQueryString<int>("CurMaxWidth");//页面传值最大宽度
                if (curMaxWidth > 0)
                {
                    upload.AllImageIsNail = 1;
                    upload.MaxWidth = curMaxWidth;//如果有页面传值设置图片最大宽度，以页面传值为准
                }
                FileInfo file = null;
                int waterType = ShopConfig.ReadConfigInfo().WaterType;

                if (waterType == 2 || waterType == 3)
                {
                    string needMark = RequestHelper.GetQueryString<string>("NeedMark");
                    if (needMark == string.Empty || needMark == "1")
                    {
                        int waterPossition = ShopConfig.ReadConfigInfo().WaterPossition;
                        string text = ShopConfig.ReadConfigInfo().Text;
                        string textFont = ShopConfig.ReadConfigInfo().TextFont;
                        int textSize = ShopConfig.ReadConfigInfo().TextSize;
                        string textColor = ShopConfig.ReadConfigInfo().TextColor;
                        string waterPhoto = Server.MapPath(ShopConfig.ReadConfigInfo().WaterPhoto);

                        file = upload.SaveFromTaobao(waterType, waterPossition, text, textFont, textSize, textColor, waterPhoto, fileName);
                    }
                    else if (needMark == "0")
                    {
                        file = upload.SaveFromTaobao(fileName);
                    }
                }
                else
                {
                    file = upload.SaveFromTaobao(fileName);
                }
                //生成处理
                string originalFile = fileName;
                string otherFile = string.Empty;
                string makeFile = string.Empty;
                Dictionary<int, int> dic = new Dictionary<int, int>();

                foreach (var phototype in PhotoSizeBLL.SearchList((int)photoType))
                {
                    dic.Add(phototype.Width, phototype.Height);
                }
                if (!dic.ContainsKey(90)) dic.Add(90, 90);//后台商品列表默认使用尺寸(如果不存在则手动添加)
                if (!dic.ContainsKey(75)) dic.Add(75, 75);//后台商品图集默认使用尺寸(如果不存在则手动添加)


                if (dic.Count > 0)
                {
                    foreach (KeyValuePair<int, int> kv in dic)
                    {
                        makeFile = originalFile.Replace("Original", kv.Key.ToString() + "-" + kv.Value.ToString());
                        otherFile += makeFile + "|";
                        ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(originalFile), ServerHelper.MapPath(makeFile), kv.Key, kv.Value, ThumbnailType.InBox);
                    }
                    otherFile = otherFile.Substring(0, otherFile.Length - 1);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.ProcessException(ex, false);
            }

        }

        protected  string PrepareDataFiles(params object[] initParams)
        {
            string path = (string)initParams[0];
            DirectoryInfo _workDir = _baseDir.CreateSubdirectory(Path.GetFileNameWithoutExtension(path));
            using (ZipFile file = ZipFile.Read(Path.Combine(_baseDir.FullName, path)))
            {
                foreach (ZipEntry entry in file)
                {
                    entry.Extract(_workDir.FullName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            return _workDir.FullName;
        }

        private void BindFiles()
        {
            dropFiles.Items.Clear();
            dropFiles.Items.Add(new ListItem("-请选择-", ""));
            DirectoryInfo info = new DirectoryInfo(_dataPath);
            foreach (FileInfo info2 in info.GetFiles("*.zip", SearchOption.TopDirectoryOnly))
            {
                string name = info2.Name;
                dropFiles.Items.Add(new ListItem(name, name));
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!this.fileUploader.HasFile)
            {
                ScriptHelper.Alert("请先选择一个数据包文件");
            }
            else if ((this.fileUploader.PostedFile.ContentLength == 0) || (((this.fileUploader.PostedFile.ContentType != "application/x-zip-compressed") && (this.fileUploader.PostedFile.ContentType != "application/zip")) && (this.fileUploader.PostedFile.ContentType != "application/octet-stream")))
            {
                ScriptHelper.Alert("请上传正确的数据包文件");
            }
            else
            {
                string fileName = Path.GetFileName(this.fileUploader.PostedFile.FileName);
                fileUploader.PostedFile.SaveAs(Path.Combine(this._dataPath, fileName));
                BindFiles();
                dropFiles.SelectedValue = fileName;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);            
            if (!Page.IsPostBack)
            {               
                BindFiles();

                ProductClass.DataSource = ProductClassBLL.ReadUnlimitClassList();
                ProductClass.DataBind();
            }
        }
    }
}