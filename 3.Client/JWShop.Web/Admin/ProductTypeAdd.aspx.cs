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
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace JWShop.Web.Admin
{
    public partial class ProductTypeAdd : JWShop.Page.AdminBasePage
    {
        protected ProductTypeInfo pageAttr = new ProductTypeInfo();
        /// <summary>
        /// 页面加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                int attributeClassID = RequestHelper.GetQueryString<int>("ID");

                pageAttr = ProductTypeBLL.Read(attributeClassID);

                if (attributeClassID != int.MinValue)
                {
                    CheckAdminPower("ReadProductType", PowerCheckType.Single);
                    ProductTypeInfo productType = ProductTypeBLL.Read(attributeClassID);
                    Name.Text = productType.Name;

                    Repeater1.DataSource = ProductTypeAttributeBLL.ReadList(attributeClassID);
                    Repeater1.DataBind();

                    Repeater2.DataSource = ProductTypeStandardBLL.ReadList(attributeClassID);
                    Repeater2.DataBind();
                   
                }
                if (RequestHelper.GetQueryString<string>("Action") == "GetBrandsByFirstLetter")
                {
                    GetBrandsByFirstLetter();
                }
            }
        }
        /// <summary>
        /// 提交按钮点击方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            #region
            //添加属性组，即产品类型
            int productTypeID = 0;
            ProductTypeInfo productType = new ProductTypeInfo();
            productType.Id = RequestHelper.GetQueryString<int>("ID");
            productType.Name = Name.Text;
            //productType.BrandIds = RequestHelper.GetForm<string>("proBrand").Replace(",",";");
            productType.BrandIds = RequestHelper.GetForm<string>("choosedBrnadIds").Replace(";;", ";");
            
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (productType.Id == int.MinValue)
            {
                CheckAdminPower("AddProductType", PowerCheckType.Single);
                productTypeID = ProductTypeBLL.Add(productType);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("ProductType"), productTypeID);

                #region
                //添加相关属性
                string[] namelist = RequestHelper.GetForm<string>("NameList").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] valuelist = RequestHelper.GetForm<string>("ValueList").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] typelist = RequestHelper.GetForm<string>("TypeList").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (namelist.Length > 0)
                {
                    for (int t = 0; t < namelist.Length; t++)
                    {
                        ProductTypeAttributeInfo attribute = new ProductTypeAttributeInfo();
                        attribute.Name = namelist[t];
                        var _attribute = ProductTypeAttributeBLL.Read(attribute.Name, productTypeID);
                        if (_attribute.Id > 0)
                        {
                            ScriptHelper.Alert("该属性已存在，请重新输入", RequestHelper.RawUrl);
                        }
                        else
                        {
                            attribute.ProductTypeId = productTypeID;
                            int typeNum = 0;
                            int.TryParse(typelist[t], out typeNum);

                            attribute.InputType = typeNum;
                            attribute.InputValue = valuelist[t].Replace('|', ';');
                            attribute.OrderId = 0;

                            ProductTypeAttributeBLL.Add(attribute);
                        }
                    }
                }
                #endregion
                #region
                //添加相关规格
                string[] snamelist = RequestHelper.GetForm<string>("SNameList").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] svaluelist = RequestHelper.GetForm<string>("SValueList").Split(',');
                string[] stypelist = RequestHelper.GetForm<string>("STypeList").Split(',');
                if (snamelist.Length > 0)
                {
                    for (int t = 0; t < snamelist.Length; t++)
                    {
                        ProductTypeStandardInfo standard = new ProductTypeStandardInfo();
                        standard.Name = snamelist[t];
                        var _standard = ProductTypeStandardBLL.Read(standard.Name, productTypeID);
                        if (_standard.Id > 0)
                        {
                            ScriptHelper.Alert("该规格已存在，请重新输入", RequestHelper.RawUrl);
                        }
                        else
                        {
                            standard.ProductTypeId = productTypeID;
                            int typeNum = 0;
                            int.TryParse(stypelist[t], out typeNum);

                            standard.ValueList = svaluelist[t].Replace('|', ';');

                            ProductTypeStandardBLL.Add(standard);
                        }
                    }
                }
                #endregion
            }
            else
            {
                CheckAdminPower("UpdateProductType", PowerCheckType.Single);
                ProductTypeBLL.Update(productType);
                AdminLogBLL.Add(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("ProductType"), productType.Id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");

                #region
                //删除原有属性
                //ProductTypeAttributeBLL.DeleteList(productType.Id);

                //添加相关属性
                string[] namelist = RequestHelper.GetForm<string>("NameList").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] valuelist = RequestHelper.GetForm<string>("ValueList").Split(',');
                string[] typelist = RequestHelper.GetForm<string>("TypeList").Split(',');
                string[] idList= RequestHelper.GetForm<string>("IdList").Split(',');

                if (namelist.Length > 0)
                {
                    List<ProductTypeAttributeInfo> productTypeAttrList = ProductTypeAttributeBLL.ReadList(productType.Id);
                    for (int t = 0; t < namelist.Length; t++)
                    {                       
                        if (idList[t]!="0")
                        {
                            ProductTypeAttributeInfo attribute = ProductTypeAttributeBLL.Read(Convert.ToInt32(idList[t]));
                            attribute.Name = namelist[t];
                            var _attribute = ProductTypeAttributeBLL.Read(attribute.Name, productType.Id);
                            if (_attribute.Id != attribute.Id && _attribute.Id>0)
                            {
                                ScriptHelper.Alert("该属性已存在，请重新输入", RequestHelper.RawUrl);
                            }
                            else
                            {
                                attribute.ProductTypeId = productType.Id;
                                int typeNum = 0;
                                int.TryParse(typelist[t], out typeNum);

                                attribute.InputType = typeNum;
                                attribute.InputValue = valuelist[t].Replace('|', ';');
                                attribute.OrderId = 0;

                                ProductTypeAttributeBLL.Update(attribute);
                            }
                        }
                        else
                        {
                            ProductTypeAttributeInfo attribute = new ProductTypeAttributeInfo();
                            attribute.Name = namelist[t];
                            var _attribute = ProductTypeAttributeBLL.Read(attribute.Name, productType.Id);
                            if (_attribute.Id >0)
                            {
                                ScriptHelper.Alert("该属性已存在，请重新输入", RequestHelper.RawUrl);
                            }
                            else
                            {
                                attribute.ProductTypeId = productType.Id;
                                int typeNum = 0;
                                int.TryParse(typelist[t], out typeNum);

                                attribute.InputType = typeNum;
                                attribute.InputValue = valuelist[t].Replace('|', ';');
                                attribute.OrderId = 0;

                                ProductTypeAttributeBLL.Add(attribute);
                            }
                        }
                    }
                }
                #endregion
                #region
                //删除原有规格
                //ProductTypeStandardBLL.DeleteList(productType.Id);
                //添加相关规格
                string[] snamelist = RequestHelper.GetForm<string>("SNameList").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] svaluelist = RequestHelper.GetForm<string>("SValueList").Split(',');
                string[] stypelist = RequestHelper.GetForm<string>("STypeList").Split(',');
                string[] sidList = RequestHelper.GetForm<string>("SIdList").Split(',');

                if (snamelist.Length > 0)
                {
                    List<ProductTypeStandardInfo> productTypeStandardList = ProductTypeStandardBLL.ReadList(productType.Id);
                    for (int t = 0; t < snamelist.Length; t++)
                    {
                        if (sidList[t] != "0")
                        {
                            ProductTypeStandardInfo standard = ProductTypeStandardBLL.Read(Convert.ToInt32(sidList[t]));
                             standard.Name = snamelist[t];
                            var _standard = ProductTypeStandardBLL.Read(standard.Name, productType.Id);
                            if (_standard.Id != standard.Id && _standard.Id>0)
                            {
                                ScriptHelper.Alert("该规格已存在，请重新输入", RequestHelper.RawUrl);
                            }
                            else
                            {                         
                                standard.ProductTypeId = productType.Id;
                                standard.ValueList = svaluelist[t].Replace('|', ';');

                                ProductTypeStandardBLL.Update(standard);
                            }
                        }
                        else
                        {
                            ProductTypeStandardInfo standard = new ProductTypeStandardInfo();
                            standard.Name = snamelist[t];
                            var _standard = ProductTypeStandardBLL.Read(standard.Name, productType.Id);
                            if (_standard.Id>0)
                            {
                                ScriptHelper.Alert("该规格已存在，请重新输入", RequestHelper.RawUrl);
                            }
                            else
                            {
                                standard.ProductTypeId = productType.Id;
                                standard.ValueList = svaluelist[t].Replace('|', ';');

                                ProductTypeStandardBLL.Add(standard);
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion


            ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);

        }
        /// <summary>
        /// 根据首字符筛选品牌
        /// </summary>
        protected void GetBrandsByFirstLetter()
        {
            string firstLetter = RequestHelper.GetQueryString<string>("FirstLetter");
            string _html = string.Empty;
            int attributeClassID = RequestHelper.GetQueryString<int>("ID");

            pageAttr = ProductTypeBLL.Read(attributeClassID);
            List<ProductBrandInfo> proBrandList = new List<ProductBrandInfo>();
            if (!string.IsNullOrEmpty(firstLetter))
            {
                if (firstLetter == "+")
                {//其他，不是以字母开头的
                    Regex regChar = new Regex("^[a-z]");
                    Regex regDChar = new Regex("^[A-Z]");
                    proBrandList = ProductBrandBLL.ReadList().Where(k => !regChar.IsMatch(k.Spelling)).Where(k => !regDChar.IsMatch(k.Spelling)).OrderBy(k => k.Spelling).ToList();
                }
                else
                {//以字母开头的

                   proBrandList = ProductBrandBLL.ReadList().Where(k => k.Spelling.ToLower().StartsWith(firstLetter.ToLower())).OrderBy(k => k.Spelling).ToList();
                   
                }
                foreach (ProductBrandInfo proBrand in proBrandList)
                {
                    if (!string.IsNullOrEmpty(pageAttr.BrandIds))
                    {
                        string brands = ";" + pageAttr.BrandIds + ";";

                        if (brands.IndexOf(";" + proBrand.Id.ToString() + ";") >= 0)
                        {
                            _html += "<label class=\"ig-checkbox checked\"><input type=\"checkbox\" checked=\"checked\" name=\"proBrand\" value=\"" + proBrand.Id + "\" onclick=\"chooseBrand(this,'" + proBrand.Name + "')\"/>" + proBrand.Name + "</label>";
                        }
                        else
                        {
                            _html += " <label class=\"ig-checkbox\" ><input type=\"checkbox\" name=\"proBrand\" value=\"" + proBrand.Id + "\" onclick=\"chooseBrand(this,'" + proBrand.Name + "')\"/>" + proBrand.Name + "</label>";
                        }
                    }
                    else
                    {
                        _html += "<label class=\"ig-checkbox\"><input type=\"checkbox\" name=\"proBrand\" value=\"" + proBrand.Id + "\" onclick=\"chooseBrand(this,'" + proBrand.Name + "')\"/>" + proBrand.Name + "</label>";
                    }
                }
                Response.Clear();
                ResponseHelper.Write(JsonConvert.SerializeObject(new { flag = true, content = _html }));
            }
            else
            {
                Response.Clear();
                ResponseHelper.Write(JsonConvert.SerializeObject(new {flag=false,content=_html }));
            }
            Response.End();
        }
        /// <summary>
        /// 根据首字符筛选品牌数量
        /// </summary>
        /// <param name="firstLetter"></param>
        /// <returns></returns>
        protected int GetBrandCountByFirstLetter(string firstLetter)
        {          
            List<ProductBrandInfo> proBrandList = new List<ProductBrandInfo>();           
                if (firstLetter == "+")
                {//其他，不是以字母开头的
                    Regex regChar = new Regex("^[a-z]");
                    Regex regDChar = new Regex("^[A-Z]");
                    proBrandList = ProductBrandBLL.ReadList().Where(k => !regChar.IsMatch(k.Spelling)).Where(k => !regDChar.IsMatch(k.Spelling)).OrderBy(k => k.Spelling).ToList();
                }
                else
                {//以字母开头的

                   proBrandList = ProductBrandBLL.ReadList().Where(k => k.Spelling.ToLower().StartsWith(firstLetter.ToLower())).OrderBy(k => k.Spelling).ToList();
                   
                }
                return proBrandList.Count;
        }
    }
}