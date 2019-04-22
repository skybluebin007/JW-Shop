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
using System.Data;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Text;

namespace JWShop.Web.Admin
{
    public partial class ProductClone : JWShop.Page.AdminBasePage
    {
        protected bool IsMultiStandard = false;
        protected string color = string.Empty;
        protected int productID = 0;
        protected int lastProClassID = 0;
        protected int sendCount = 0;
        protected string promotDetail = string.Empty;
        protected List<UserGradeInfo> userGradeList = new List<UserGradeInfo>();
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();

        protected ProductInfo pageProduct = new ProductInfo();

        protected List<ProductTypeStandardRecordInfo> standardRecordList = new List<ProductTypeStandardRecordInfo>();
        //品牌列表
        protected List<ProductBrandInfo> productBrandList = new List<ProductBrandInfo>();
        //商品类型对应的属性列表
        protected List<ProductTypeAttributeInfo> attributeList = new List<ProductTypeAttributeInfo>();
        //商品类型对应的规格列表
        protected List<ProductTypeStandardInfo> standardList = new List<ProductTypeStandardInfo>();

        protected int LastClassID = 0;
        protected int proTypeID = 0;
        protected int isUpdate = 0;
        protected int _brandId = 0;
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindClassBrandAttributeClassStandardType();
                BrandID.Items.Insert(0, new ListItem("请选择", "0"));
                RelationBrandID.Items.Insert(0, new ListItem("请选择", "0"));
                AccessoryBrandID.Items.Insert(0, new ListItem("请选择", "0"));

                string classId = RequestHelper.GetQueryString<string>("classId");
                productID = RequestHelper.GetQueryString<int>("ID");
                _brandId = RequestHelper.GetQueryString<int>("BrandId");
                //ProductClass.DataSource = ProductClassBLL.ReadUnlimitClassList();
                if (productID <= 0)//添加商品
                {
                    DraftButton.Visible = true;//添加商品可保存草稿
                    if (string.IsNullOrEmpty(classId))
                    {
                        Response.Redirect("/admin/productaddinit.aspx?Action=clone");
                    }                  
                }
                else//修改商品
                {
                    CheckAdminPower("ReadProduct", PowerCheckType.Single);
                    DraftButton.Visible = false;//修改商品不可保存草稿
                    ProductInfo product = ProductBLL.Read(productID);
                    pageProduct = product;
                    //如果修改了分类则标识isupdate
                    if (!string.IsNullOrEmpty(classId) && product.ClassId != classId) isUpdate = 1;

                    Name.Text = product.Name;
                    SellPoint.Text = product.SellPoint;
                    Name.Attributes.Add("style", "color:" + product.Color);
                    color = product.Color;
                    FontStyle.Text = product.FontStyle;
                    ProductNumber.Text = product.ProductNumber;
                    //ProductClass.ClassID = product.ClassId;
                    Keywords.Text = product.Keywords;
                    MarketPrice.Text = product.MarketPrice.ToString();
                    SendPoint.Text = product.SendPoint.ToString();
                    Photo.Text = product.Photo;
                    Summary.Text = product.Summary;
                    Introduction.Value = product.Introduction1;
                    Weight.Text = product.Weight.ToString();

                    if (Convert.ToBoolean(product.IsSpecial)) IsSpecial.Checked = true;
                    if (Convert.ToBoolean(product.IsNew)) IsNew.Checked = true;
                    if (Convert.ToBoolean(product.IsHot)) IsHot.Checked = true;
                    if (Convert.ToBoolean(product.IsSale))
                        IsSale.Checked = true;
                    else
                        IsSale.Checked = false;
                    if (Convert.ToBoolean(product.IsTop)) IsTop.Checked = true;
                    if (Convert.ToBoolean(product.AllowComment))
                        AllowComment.Checked = true;
                    else
                        AllowComment.Checked = false;
                    TotalStorageCount.Text = product.TotalStorageCount.ToString();

                    LastClassID = ProductClassBLL.GetLastClassID(product.ClassId);
                    this.proTypeID = ProductClassBLL.GetProductClassType(LastClassID);
                    //商品分类对应的属性列表
                    //attributeList = ProductTypeAttributeBLL.JoinAttribute(ProductClassBLL.Read(ProductClassBLL.GetLastClassID(product.ClassId)).ProductTypeId, productID);
                    attributeList = ProductTypeAttributeBLL.JoinAttribute(this.proTypeID, productID);
                    Repeater1.DataSource = attributeList;
                    Repeater1.DataBind();
                    //商品类型对应的规格列表
                    standardList = ProductTypeStandardBLL.ReadList(this.proTypeID);
                    standardRecordList = ProductTypeStandardRecordBLL.ReadListByProduct(product.Id, product.StandardType);
                    if (product.StandardType == 1)
                    {
                        if (standardRecordList.Count > 0) TotalStorageCount.ReadOnly = true;

                    }
                    if (string.IsNullOrEmpty(classId)) { LastClassID = ProductClassBLL.GetLastClassID(product.ClassId); }
                    else { LastClassID = ProductClassBLL.GetLastClassID(classId); }
                    int proTypeID = ProductClassBLL.GetProductClassType(LastClassID);
                    ProductTypeInfo aci = ProductTypeBLL.Read(proTypeID);
                    if (aci.Id > 0)
                    {
                        string[] strArray = aci.BrandIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        int[] intArray;

                        intArray = Array.ConvertAll<string, int>(strArray, s => int.Parse(s));
                        productBrandList = ProductBrandBLL.ReadList(intArray);
                    }

                    if (productBrandList.Count > 0)
                    {
                        BrandID.DataSource = productBrandList;
                        BrandID.DataTextField = "Name";
                        BrandID.DataValueField = "ID";
                        BrandID.DataBind();
                        BrandID.Items.Insert(0, new ListItem("请选择", "0"));

                        RelationBrandID.DataSource = productBrandList;
                        RelationBrandID.DataTextField = "Name";
                        RelationBrandID.DataValueField = "ID";
                        RelationBrandID.DataBind();
                        RelationBrandID.Items.Insert(0, new ListItem("请选择", "0"));

                        AccessoryBrandID.DataSource = productBrandList;
                        AccessoryBrandID.DataTextField = "Name";
                        AccessoryBrandID.DataValueField = "ID";
                        AccessoryBrandID.DataBind();
                        AccessoryBrandID.Items.Insert(0, new ListItem("请选择", "0"));
                    }
                    if (_brandId > 0)
                        BrandID.Text = _brandId.ToString();
                    else
                        BrandID.Text = product.BrandId.ToString();

                    sendCount = product.SendCount;
                    OrderID.Text = product.OrderId.ToString();

                    SalePrice.Text = product.SalePrice.ToString();
                    Units.Text = product.Unit;
                    Introduction_Mobile.Value = product.Introduction1_Mobile;
                    Sub_Title.Text = product.SubTitle;
                    LowerCount.Text = product.LowerCount.ToString();

                    Remark.Value = product.Remark;

                    BindRelation(product);
                    productPhotoList = ProductPhotoBLL.ReadList(productID, 0);
                }
                //userGradeList = UserGradeBLL.JoinUserGrade(productID);
            }
           
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
           int id = RequestHelper.GetQueryString<int>("ID");
            ProductInfo product = new ProductInfo();
            //if (id > 0) product = ProductBLL.Read(id);
            product.Name = Name.Text;
            product.SellPoint = SellPoint.Text;
            product.Spelling = ChineseCharacterHelper.GetFirstLetter(Name.Text);
            product.Color = RequestHelper.GetForm<string>("ProductColor");
            product.FontStyle = FontStyle.Text;
            product.ProductNumber = ProductNumber.Text;
            //product.ClassId = ProductClass.ClassID;
            product.ClassId = string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("classId")) ? ProductBLL.Read(id).ClassId : RequestHelper.GetQueryString<string>("classId");
            product.Keywords = Keywords.Text;
            product.BrandId = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$BrandID");
            product.MarketPrice = Convert.ToDecimal(MarketPrice.Text);
            product.SendPoint = Convert.ToInt32(SendPoint.Text);
            product.Photo = Photo.Text;
            product.Summary = Summary.Text;
            product.Introduction1 = Introduction.Value;
            product.Weight = Convert.ToDecimal(Weight.Text);
            product.IsSpecial = Convert.ToInt32(IsSpecial.Checked);
            product.IsNew = Convert.ToInt32(IsNew.Checked);
            product.IsHot = Convert.ToInt32(IsHot.Checked);
            product.IsSale = Convert.ToInt32(IsSale.Checked);
            product.IsTop = Convert.ToInt32(IsTop.Checked);
            //保存时如果售后服务为空则自动获取所属分类的售后服务并保存
            product.Remark = string.IsNullOrEmpty(StringHelper.KillHTML(Remark.Value)) ? GetProductClassRemark(product.ClassId) : Remark.Value;
            product.Accessory = RequestHelper.GetForm<string>("RelationAccessoryID");
            product.RelationProduct = RequestHelper.GetForm<string>("RelationProductID");
            product.RelationArticle = RequestHelper.GetForm<string>("RelationArticleID");
            product.AllowComment = Convert.ToInt32(AllowComment.Checked);
            product.TotalStorageCount = Convert.ToInt32(TotalStorageCount.Text);
            if (TotalStorageCount.ReadOnly) product.TotalStorageCount = Convert.ToInt32(HidTotalStorageCount.Value);
            product.LowerCount = 0;
            product.UpperCount = 0;
            product.StandardType = Convert.ToInt32(RequestHelper.GetForm<string>("StandardType"));
            product.AddDate = RequestHelper.DateNow;


            product.OrderId = Convert.ToInt32(OrderID.Text.Trim());

            product.SalePrice = Convert.ToDecimal(SalePrice.Text);
            product.Unit = Units.Text;
            product.Introduction1_Mobile = Introduction_Mobile.Value;
            product.SubTitle = Sub_Title.Text;

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            CheckAdminPower("AddProduct", PowerCheckType.Single);
            int pid = ProductBLL.Add(product);
            AddProductPhoto(pid);
            #region 添加时生成二维码
            string ewmName = string.Empty;//二维码路径
            CreateQRcode("http://" + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port > 0 ? ":" + HttpContext.Current.Request.Url.Port : "") + "/mobile/ProductDetail-i" + pid + ".html", "pro_" + pid.ToString(), ref ewmName);
            ProductInfo tmpProduct = ProductBLL.Read(pid);
            tmpProduct.Qrcode = ewmName;
            ProductBLL.Update(tmpProduct);
            #endregion
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Product"), pid);

            HanderAttribute(product);
            HanderProductStandard(product);
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
        /// <summary>
        /// 保存草稿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DraftButton_Click(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            ProductInfo product = new ProductInfo();
            if (id > 0) product = ProductBLL.Read(id);
            product.Name = Name.Text;
            product.Spelling = ChineseCharacterHelper.GetFirstLetter(Name.Text);
            product.Color = RequestHelper.GetForm<string>("ProductColor");
            product.FontStyle = FontStyle.Text;
            product.ProductNumber = ProductNumber.Text;
            //product.ClassId = ProductClass.ClassID;
            if (!string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("classId"))) product.ClassId = RequestHelper.GetQueryString<string>("classId");
            product.Keywords = Keywords.Text;
            product.BrandId = RequestHelper.GetForm<int>("ctl00$ContentPlaceHolder$BrandID");
            product.MarketPrice = Convert.ToDecimal(MarketPrice.Text);
            product.SendPoint = Convert.ToInt32(SendPoint.Text);
            product.Photo = Photo.Text;
            product.Summary = Summary.Text;
            product.Introduction1 = Introduction.Value;
            product.Weight = Convert.ToDecimal(Weight.Text);
            product.IsSpecial = Convert.ToInt32(IsSpecial.Checked);
            product.IsNew = Convert.ToInt32(IsNew.Checked);
            product.IsHot = Convert.ToInt32(IsHot.Checked);
            product.IsSale = 2;//草稿状态
            product.IsTop = Convert.ToInt32(IsTop.Checked);
            product.Remark = Remark.Value;
            product.Accessory = RequestHelper.GetForm<string>("RelationAccessoryID");
            product.RelationProduct = RequestHelper.GetForm<string>("RelationProductID");
            product.RelationArticle = RequestHelper.GetForm<string>("RelationArticleID");
            product.AllowComment = Convert.ToInt32(AllowComment.Checked);
            product.TotalStorageCount = Convert.ToInt32(TotalStorageCount.Text);
            if (TotalStorageCount.ReadOnly) product.TotalStorageCount = Convert.ToInt32(HidTotalStorageCount.Value);
            product.LowerCount = Convert.ToInt32(LowerCount.Text.Trim());
            product.UpperCount = 0;
            product.StandardType = Convert.ToInt32(RequestHelper.GetForm<string>("StandardType"));
            product.AddDate = RequestHelper.DateNow;


            product.OrderId = Convert.ToInt32(OrderID.Text.Trim());

            product.SalePrice = Convert.ToDecimal(SalePrice.Text);
            product.Unit = Units.Text;
            product.Introduction1_Mobile = Introduction_Mobile.Value;
            product.SubTitle = Sub_Title.Text;

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");

            CheckAdminPower("AddProduct", PowerCheckType.Single);
            int pid = ProductBLL.Add(product);
            AddProductPhoto(pid);
            AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Product"), pid);

            HanderAttribute(product);
            //HanderMemberPrice(product.ID);
            HanderProductStandard(product);
            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
        }
        /// <summary>
        /// 属性处理
        /// </summary>
        /// <param name="productID"></param>
        protected void HanderAttribute(ProductInfo product)
        {
            if (product.Id > 0)
            {
                ProductTypeAttributeRecordBLL.Delete(product.Id);
            }
            int lastClassID = ProductClassBLL.GetLastClassID(product.ClassId);
            int productTypeID = ProductClassBLL.GetProductClassType(lastClassID);
            List<ProductTypeAttributeInfo> attributeList = ProductTypeAttributeBLL.ReadList(productTypeID);
            foreach (ProductTypeAttributeInfo attribute in attributeList)
            {
                ProductTypeAttributeRecordInfo attributeRecord = new ProductTypeAttributeRecordInfo();
                attributeRecord.AttributeId = attribute.Id;
                attributeRecord.ProductId = product.Id;
                attributeRecord.Value = RequestHelper.GetForm<string>(attribute.Id.ToString() + "Value").Replace(',', ';');
                ProductTypeAttributeRecordBLL.Add(attributeRecord);
            }
        }
        
        /// <summary>
        /// 处理商品规格
        /// </summary>
        /// <param name="product"></param>
        protected void HanderProductStandard(ProductInfo product)
        {
            string productIDList = string.Empty;
            if (product.StandardType == (int)ProductStandardType.Group)
            {
                productIDList = "," + RequestHelper.GetForm<string>("Product") + ",";
                productIDList = productIDList.Replace(",0,", "," + product.Id.ToString() + ",");
                productIDList = productIDList.Substring(1, productIDList.Length - 2);
            }
            ProductBLL.UpdateProductStandardType(productIDList, product.StandardType, product.Id);

            if (product.Id > 0)
            {
                ProductTypeStandardRecordBLL.DeleteByProductID(product.Id.ToString());
            }
            if (RequestHelper.GetForm<string>("isOpenStandard") == "1")
            {

                string standardIDList = RequestHelper.GetForm<string>("StandardIDList");
                if (standardIDList != string.Empty)
                {
                    string[] productIDArray = productIDList.Split(',');

                    string[] valueArr = RequestHelper.GetForm<string>("sValueList").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] marketPriceArr = RequestHelper.GetForm<string>("sMarketPrice").Split(',');
                    string[] salePriceArr = RequestHelper.GetForm<string>("sSalePrice").Split(',');
                    string[] storageArr = RequestHelper.GetForm<string>("sStorage").Split(',');
                    string[] productNumberArr = RequestHelper.GetForm<string>("sProductNumber").Split(',');

                    for (int i = 0; i < valueArr.Length; i++)
                    {
                        ProductTypeStandardRecordInfo standardRecord = new ProductTypeStandardRecordInfo();
                        if (product.StandardType == (int)ProductStandardType.Group)
                        {
                            standardRecord.GroupTag = productIDList;
                            standardRecord.ProductId = Convert.ToInt32(productIDArray[i]);
                            standardRecord.SalePrice = 0;
                            standardRecord.SalePrice = 0;
                            standardRecord.Storage = 0;
                            standardRecord.ProductCode = string.Empty;
                        }
                        else
                        {
                            standardRecord.ProductId = product.Id;
                            standardRecord.MarketPrice = Convert.ToDecimal(marketPriceArr[i]);
                            standardRecord.SalePrice = Convert.ToDecimal(salePriceArr[i]);
                            standardRecord.Storage = Convert.ToInt32(storageArr[i]);
                            standardRecord.ProductCode = productNumberArr[i];
                        }
                        standardRecord.StandardIdList = standardIDList.Substring(0, standardIDList.Length - 1);
                        standardRecord.ValueList = valueArr[i].Replace("|", ";");


                        ProductTypeStandardRecordBLL.Add(standardRecord);
                    }
                }
            }
        }
        /// <summary>
        /// 绑定关联产品，配件，文章
        /// </summary>
        /// <param name="product"></param>
        protected void BindRelation(ProductInfo product)
        {
            if (product.RelationArticle != string.Empty)
            {
                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.InArticleId = product.RelationArticle;
                Article.DataSource = ArticleBLL.SearchList(articleSearch);
                Article.DataTextField = "Title";
                Article.DataValueField = "ID";
                Article.DataBind();
            }
            if (product.RelationProduct != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductId = product.RelationProduct;
                Product.DataSource = ProductBLL.SearchList(productSearch);
                Product.DataTextField = "Name";
                Product.DataValueField = "ID";
                Product.DataBind();
            }
            if (product.Accessory != string.Empty)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductId = product.Accessory;
                Accessory.DataSource = ProductBLL.SearchList(productSearch);
                Accessory.DataTextField = "Name";
                Accessory.DataValueField = "ID";
                Accessory.DataBind();
            }
        }
        /// <summary>
        /// 绑定商品分类，品牌，商品类型，规格
        /// </summary>
        protected void BindClassBrandAttributeClassStandardType()
        {

            List<ProductClassInfo> productClassList = ProductClassBLL.ReadNamedList();
            foreach (ProductClassInfo productClass in productClassList)
            {
                RelationClassID.Items.Add(new ListItem(productClass.Name, "|" + productClass.Id + "|"));
            }
            RelationClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

            foreach (ProductClassInfo productClass in productClassList)
            {
                AccessoryClassID.Items.Add(new ListItem(productClass.Name, "|" + productClass.Id + "|"));
            }
            AccessoryClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));


            foreach (ArticleClassInfo articleClass in ArticleClassBLL.ReadNamedList())
            {
                ArticleClassID.Items.Add(new ListItem(articleClass.Name, "|" + articleClass.Id + "|"));
            }
            ArticleClassID.Items.Insert(0, new ListItem("请选择", string.Empty));


        }
        /// <summary>
        /// 添加产品图片
        /// </summary>
        /// <param name="productID"></param>
        protected void AddProductPhoto(int productID)
        {
            string productPhotoList = RequestHelper.GetForm<string>("ProductPhoto");
            if (productPhotoList != string.Empty)
            {
                foreach (string temp in productPhotoList.Split(','))
                {
                    ProductPhotoInfo productPhoto = new ProductPhotoInfo();
                    productPhoto.ProductId = productID;
                    productPhoto.Name = temp.Split('|')[0];
                    productPhoto.ImageUrl = temp.Split('|')[1];
                    ProductPhotoBLL.Add(productPhoto);
                }
            }
        }

        protected string ShowAttributeSelect(ProductTypeAttributeInfo attribute)
        {

            string selectStr = string.Empty;


            if (attribute.InputType == 5)
            {
                selectStr = "<select name=\"" + attribute.Id + "Value\" style=\"width: 200px;\" class=\"select\">";
                if (attribute.InputValue != "")
                {
                    foreach (string inputValue in attribute.InputValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        selectStr += "<option value=\"" + inputValue + "\"";
                        if (attribute.AttributeRecord.Value == inputValue) selectStr += " selected=\"selected\"";
                        selectStr += ">" + inputValue + "</option>";
                    }
                }
                selectStr += "</select>";
            }
            else
            {
                foreach (string inputValue in attribute.InputValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if ((";" + attribute.AttributeRecord.Value + ";").IndexOf(";" + inputValue + ";") > -1)
                    {
                        selectStr += "<label class=\"ig-checkbox checked\">";
                    }
                    else
                    {
                        selectStr += "<label class=\"ig-checkbox\">";
                    }
                    selectStr += "<input name=\"" + attribute.Id + "Value\" type=\"checkbox\"";
                    if ((";" + attribute.AttributeRecord.Value + ";").IndexOf(";" + inputValue + ";") > -1)
                    {
                        selectStr += "checked=\"checked\"";
                    }
                    selectStr += " value=\"" + inputValue + "\" />";
                    selectStr += inputValue + "</label> &nbsp;&nbsp;";
                }
            }

            return selectStr;
        }

        protected string ShowStandardHead(ProductTypeStandardRecordInfo standardInfo, int pType)
        {
            string[] valArr = standardInfo.StandardIdList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string headStr = "<thead><tr>";
            foreach (string valstr in valArr)
            {
                if (!string.IsNullOrEmpty(valstr))
                    headStr += "<td>" + ProductTypeStandardBLL.Read(Convert.ToInt32(valstr)).Name + "</td>";
            }
            if (pType == 1)
            {
                headStr += "<td width=\"15%\"><input type=\"hidden\" name=\"StandardIDList\" id=\"StandardIDList\" value=\"" + standardInfo.StandardIdList + ";\"> <span class=\"red\">*</span>本站价</td><td width=\"15%\"><span class=\"red\">*</span>市场价</td><td width=\"15%\"><span class=\"red\">*</span>库存</td><td width=\"15%\" height=\"40\">货号</td><td>图片</td></tr></thead>";
            }
            else
            {
                headStr += "<td width=\"15%\"><input type=\"hidden\" name=\"StandardIDList\" id=\"StandardIDList\" value=\"" + standardInfo.StandardIdList + ";\"> 关联产品</td></tr></thead>";
            }
            return headStr;
        }
        protected string ShowStandardSelect(ProductTypeStandardRecordInfo standardInfo, int pType, int recordCount)
        {
            string bodyStr = "<tr>";
            string[] valArr = standardInfo.ValueList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int hidCount = 1;
            foreach (string valstr in valArr)
            {
                bodyStr += "<td>" + valstr + "";
                if (hidCount == 1) bodyStr += "<input type=\"hidden\" name=\"sValueList\" id=\"sValueList\" value=\"" + standardInfo.ValueList.Replace(',', '|') + "\">";
                bodyStr += "</td>";
                hidCount++;
            }
            if (pType == 1)
            {
                bodyStr += "<td ><input type=\"text\" name=\"sSalePrice\" value=\"" + standardInfo.SalePrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsSalePrice()\" maxlength=\"8\"></td><td ><input type=\"text\" name=\"sMarketPrice\" value=\"" + standardInfo.MarketPrice + "\" onkeyup=\"clearNoNum(this)\" onafterpaste=\"clearNoNum(this)\" onblur=\"clearNoNum(this);setsMarketPrice()\"  maxlength=\"8\"></td><td ><input type=\"text\" name=\"sStorage\" onkeyup=\"value=value.replace(/[^\\d]/g,'')\" onafterpaste=\"value=value.replace(/[^\\d]/g,'')\" onblur=\"setTotalStorage(this)\" value=\"" + standardInfo.Storage + "\" maxlength=\"8\"></td><td  height=\"30\" ><input type=\"text\" name=\"sProductNumber\" value=\"" + standardInfo.ProductCode + "\" ></td><td><img id=\"img_sPhoto" + recordCount + "\" src=\"" + ShopCommon.ShowImage(standardInfo.Photo) + "\" width=\"20\" height=\"20\"><input type=\"hidden\" name=\"sPhoto\" id=\"ctl00_ContentPlaceHolder_sPhoto" + recordCount + "\" value=\"" + standardInfo.Photo + "\" ><iframe src=\"UploadStandardPhoto.aspx?Control=sPhoto" + recordCount + "&TableID=1&FilePath=ProductCoverPhoto/Original\" width=\"50px\" height=\"50px\" frameborder=\"0\" allowTransparency=\"true\" scrolling=\"no\" id=\"uploadIFrame\"></iframe></td></tr>";
            }
            else
            {
                ProductInfo tempPro = ProductBLL.Read(standardInfo.ProductId);
                bodyStr += "<td ><input type=\"hidden\" id=\"Product" + recordCount + "\" name=\"Product\" value=\"" + tempPro.Id + "\" /><span id=\"ProductName" + recordCount + "\">" + (tempPro.Id > 0 ? tempPro.Name : "暂无") + "</span> | <a href=\"javascript:loadProducts(" + recordCount + ");\">修改</a> | <a href=\"javascript:void(0)\" onclick=\"deleteStandard(this)\">删除</a></td>";
            }
            return bodyStr;
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
        /// <summary>
        /// 根据商品分类逐级获取分类的Remark（售后服务）
        /// </summary>
        /// <param name="classId">商品分类</param>
        /// <returns></returns>
        protected string GetProductClassRemark(string classId) {
            string result = string.Empty;
            if (classId.IndexOf("|") >= 0) {
                foreach (string _cid in classId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)) {
                    int cid = 0;
                    if (int.TryParse(_cid, out cid))
                    {
                        if (cid > 0)
                        {
                            ProductClassInfo proClass = ProductClassBLL.Read(cid);
                            if (!string.IsNullOrEmpty(proClass.Remark)) result = proClass.Remark;
                        }
                    }
                }            
            }
            else
            {
                int cid = 0;
                if (int.TryParse(classId, out cid)) {
                    if (cid > 0) {
                        ProductClassInfo proClass = ProductClassBLL.Read(cid);
                        if (!string.IsNullOrEmpty(proClass.Remark)) result = proClass.Remark;
                    }
                }
            }
            return result;
        }
    }
}