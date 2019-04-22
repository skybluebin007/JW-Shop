<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductClone.aspx.cs" Inherits="JWShop.Web.Admin.ProductClone" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
<link rel="stylesheet" href="/Admin/static/css/plugin.css" />
<link href="/Admin/kindeditor/themes/default/default.css" rel="stylesheet" />
<link href="/Admin/kindeditor/plugins/code/prettify.css" rel="stylesheet" />
<script src="/Admin/kindeditor/kindeditor.js"></script>
<script src="/Admin/kindeditor/lang/zh_CN.js"></script>
<script src="/Admin/kindeditor/plugins/code/prettify.js"></script>
<script src="/Admin/layer/layer.js"></script>
<script src="/Admin/static/js/colpick.js"></script>
<script >
    var KE, KE2,KE3;
    $(document).ready(function () {
        KindEditor.ready(function (K) {
            KE = K.create('#ctl00_ContentPlaceHolder_Introduction', {
                cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                allowFileManager: true
            });
            KE2 = K.create('#ctl00_ContentPlaceHolder_Introduction_Mobile', {
                cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                allowFileManager: true
            });
            KE3 = K.create('#ctl00_ContentPlaceHolder_Remark', {
                cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                allowFileManager: true
            });
            prettyPrint();
        });
    });
</script>

<input type="hidden" name="RelationArticleID" id="RelationArticleID" />
<input type="hidden" name="RelationProductID" id="RelationProductID"/>
<input type="hidden" name="RelationAccessoryID" id="RelationAccessoryID"/>

    <div class="container ease confix" id="container" style="padding-top: 35px;">
    	<!--<div class="tab-title tabfix">
            <span class="cur">基本信息</span>
            <span>图片及描述</span>
            <span>商品属性</span>
            <span>商品规格</span>            
            <span>相关设置</span>
            <span>搜索优化</span>
            <span>关联商品</span>
            <span>商品配件</span>
            <span>关联文章</span>
        </div>-->
        <!--商品分类-->
        <div class="product-row" style="padding: 0 30px 0 15px; height: 35px;">
            <div class="product-main clearfix">
                <div class="form-row" style="padding-left: 65px; margin-bottom: 0;">
                    <div class="head" style="width: 72px;">商品分类：</div>
                    <%if (string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("classId")))
                      {%>
                    <%=ProductClassBLL.ProductClassNameListAdmin(ProductBLL.Read(productID).ClassId) %>
                    <%}
                      else
                      { %>
                    <%=ProductClassBLL.ProductClassNameListAdmin(RequestHelper.GetQueryString<string>("classId")) %>
                    <%} %>
                    <a style=" color: #c81623; display: inline-block; margin-left: 10px;" href="/admin/productaddinit.aspx?Action=clone&productId=<%=RequestHelper.GetQueryString<int>("ID") %>">切换分类</a>
                </div>
            </div>
        </div>
    	<div class="product-container product-container-border product-container-add">
            <!--商品基本信息-->
            <div class="product-row" style="padding-bottom: 10px;">
                <div class="product-head">1. 商品基本信息</div>
                <div class="product-main clearfix">

                    <div class="form-row">
                        <div class="head">宝贝标题<span class="red">*</span>：</div>
                        <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" MaxLength="40" placeholder="长度不能超过40个字"  Width="500px" onkeydown="gbcount(this,40,$('#ptitle'));" onkeyup="gbcount(this,40,$('#ptitle'));" onblur="checkProductName();gbcount(this,40,$('#ptitle'));"/>
                        <span id="NameMsg" style="color: red; display: inline;"></span>
                        还能输入<strong  id="ptitle">40</strong>字
                    </div>
                    <div class="form-row">
                        <div class="head">宝贝卖点：</div>
                        <SkyCES:TextBox ID="SellPoint" CssClass="text text-600" runat="server" MaxLength="200" TextMode="MultiLine" Width="500px" onkeydown="gbcount(this,200,$('#psell'));" onkeyup="gbcount(this,200,$('#psell'));" onblur="gbcount(this,200,$('#psell'));"/>还能输入<strong  id="psell">200</strong>字
                    </div>

                    <div class="form-row" style="display: none;">
                        <div class="head">字体颜色：</div>
                        <SkyCES:TextBox CssClass="txt" ID="ProductColor" MaxLength="7" runat="server"></SkyCES:TextBox>
                        <div class="form-colpick form-button2" id="colpick">拾色器</div>
                    </div>
                    <div class="form-row" style="display: none;">
                        <div class="head">字体样式：</div>
                        <asp:DropDownList ID="FontStyle" runat="server" CssClass="select">
                            <asp:ListItem Value="">正常</asp:ListItem>
                            <asp:ListItem Value="font-style:italic">斜体</asp:ListItem>
                            <asp:ListItem Value="text-decoration:underline">下划线</asp:ListItem>
                            <asp:ListItem Value="text-decoration:line-through">删除线</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-row" style="display: none;">
                        <div class="head">积分：</div>
                        <SkyCES:TextBox ID="SendPoint" CssClass="txt" runat="server" Text="0" RequiredFieldType="金额" />
                        <span>(克)</span>
                    </div>
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">所属品牌：</div>
                        <asp:DropDownList ID="BrandID" runat="server" class="select" />
                        <%--<select id="proBrand" name="proBrand" class="select">
                            <%foreach(ProductBrandInfo productBrand in productBrandList){ %>
                            <option value="<%=productBrand.Id%>"><%=productBrand.Name%></option>
                            <%} %>
                        </select>--%>
                    </div>
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">商品编号：</div>
                        <SkyCES:TextBox ID="ProductNumber" CssClass="txt" runat="server" MaxLength="15" placeholder="长度不能超过15个字" />
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <!--商品属性-->
            <div class="product-row" style="padding-bottom: 20px;<%if (attributeList.Count<=0){ %>display:none;<%} %>" >
            	<div class="clearfix addAttr">
	                <div class="product-head">商品属性：</div>
	                <div class="product-main">
	                	<div class="list" id="Attribute-Ajax">
		                	<div class="head"><%#Eval("Name")%></div>
		                	<div class="checkList">
			                    <asp:Repeater ID="Repeater1" runat="server">
			                        <ItemTemplate>
			                            <div class="form-checkbox" id='attr<%#Eval("Id")%>'>
			                                <label class="ig-checkbox"><input type="checkbox" name="chooseAll<%#Eval("Id")%>" _atid="<%#Eval("Id")%>" onclick="chooseAllAttr(this)" /><span class="red">全选</span></label>
			                             <%--   <%#ShowAttributeSelect((ProductTypeAttributeInfo)Container.DataItem)%>--%>
			                            </div>
			                        </ItemTemplate>
			                    </asp:Repeater>
		                    </div>
		                </div>
	                    <label for="" class="ig-checkbox"><input type="checkbox" class="checkall" name="chooseAllStandard" _divid="Attribute-Ajax" onclick="chooseAll(this)" /><span class="red">全选所有属性</span></label>
	                	<div class="clear"></div>
                	</div>
                </div>
            </div>
            <!--商品图片-->
            <div class="product-row" style="padding-bottom: 10px;">
                <div class="form-row" style="display: none;">
                    <!--商品主图移动至图集一行-->
                    <div class="head">产品主图:</div>
                    <div id="imgUpLoad">
                        <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" />
                        <div class="form-upload">
                            <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=ProductBLL.TableID%>&FilePath=ProductCoverPhoto/Original" width="300" height="40px" frameborder="0" allowtransparency="true" scrolling="no" id="uploadIFrame"></iframe>
                        </div>
                        <%-- <input class="form-cut" type="button" id="cutImage" value="裁剪图片" />--%>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="form-images">
                    <div class="head">商品图片<span class="red">*</span> ：</div>
                    <div class="ig-multigraph" id="ContentPhoto">
                        <ul class="search clearfix" id="ProductPhotoList">
                            <!--主图-->
                            <li class="productPhoto" id="firstPhotoLi" <%if (string.IsNullOrEmpty(ProductBLL.Read(productID).Photo))
                                                                         { %>style="display:none;" <%} %>>
                                <span class="ptitle">商品主图</span>
                                <img src="<%=ProductBLL.Read(productID).Photo.Replace("Original", "90-90")%>" style="width: 90px; height: 90px;" id="firstPhoto" />
                                <div class="opr">
                                    <span class="delete" onclick="deleteProductCoverPhoto()" title="删除">删除</span>
                                    <a class="cut" href="javascript:" title="裁剪" onclick="loadCoverPhotoCut()">裁剪</a>
                                </div>
                            </li>

                            <li class="add" id="firstPhotoUploadLi" <%if (!string.IsNullOrEmpty(ProductBLL.Read(productID).Photo))
                                                                         { %>style="display:none;" <%} %>><span>商品主图</span>
                             <iframe src="UploadProductCoverPhoto.aspx?Control=Photo&TableID=<%=ProductBLL.TableID%>&FilePath=ProductCoverPhoto/Original" width="90" height="90" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                            </li>

                            <!--主图End-->
                            <%foreach (ProductPhotoInfo productPhoto in productPhotoList)
                              { %>
                            <li class="productPhoto" id="ProductPhoto<%=productPhoto.Id%>">
                                <img src="<%=productPhoto.ImageUrl.Replace("Original", "75-75")%>" alt="" title="<%=productPhoto.Name%>" onload="photoLoad(this,90,90)" id="photo<%=productPhoto.Id %>" />                             
                                <div class="opr">
                                    <span class="down" onclick="MoveDownProductPhoto(<%=productPhoto.Id %>)" title="后移">后移</span>
                                    <span class="up" onclick="MoveUpProductPhoto(<%=productPhoto.Id %>)" title="前移">前移</span>
                                    <span class="delete" onclick="deleteProductPhoto(<%=productPhoto.Id %>)" title="删除">删除</span>
                                    <a  class="cut" href="javascript:loadCut('<%=productPhoto.ImageUrl.Replace("75-75", "Original")%>','ProductPhoto<%=productPhoto.Id %>',<%=productPhoto.Id %>)" title="裁剪">裁剪</a>
                                </div>
                            </li>
                            <%} %>
                            <li class="add" id="ProductPhotoListAdd"><span>商品图集</span>
                                    <iframe src="ProductPhotoAdd.aspx" width="90" height="90" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <div class="upload-tag"><span class="red">图片比例1:1，建议上传图片800×800为最佳视觉效果</span> </div>
                    </div>
                </div>
            </div>
            <!--商品规格-->
            <div class="product-row addAttr clearfix" <%if (standardList.Count<=0){%>style="display:none;"<%}%>>
                <div class="product-head">商品规格：</div>
                <div class="product-main" style="padding-top: 5px;">
                    <input type="hidden" id="proClassID" name="proClassID" value="<%=LastClassID %>" />
                    <div class="form-button" style="display:none;">
                        <%if (standardRecordList.Count > 0)
                          { %>
                        <a href="javascript:;" onclick="clearStandard(this,1);" class="ease trigerstandard" id="closeStandard">关闭规格</a>
                        <input type="hidden" name="isOpenStandard" id="isOpenStandard" value="1" />
                        <input type="hidden" name="StandardType" id="StandardType" value="<%=pageProduct.StandardType%>" />
                        <%}
                          else
                          { %>
                        <a href="javascript:;" onclick="clearStandard(this,2);" class="ease trigerstandard" id="openStandard">开启规格</a>
                        <input type="hidden" name="isOpenStandard" id="Hidden1" value="0" />
                        <input type="hidden" name="StandardType" id="Hidden2" value="0" />
                        <%} %>
                    </div>
                    <div id="standardAllBox" <%if (standardRecordList.Count == 0)
                                               { %>style="display:none;" <%} %>>
                        <div id="Standard-Ajax">
                            <table>
                            </table>
                        </div>
                        <div class="form-button" style="margin-top: 5px;">
                            <label class="ig-checkbox" style="margin-right: 10px;"><input type="checkbox" name="chooseAllStandard" _divid="Standard-Ajax" onclick="chooseAll(this)" /><span class="red">全选</span></label>
                            <a href="javascript:RecreateStandard(1,1);" style="background-color: #e64652; color: white;">生成单产品规格</a>
                            <a href="javascript:RecreateStandard(1,2);" style="background-color: #e64652; color: white;">生成产品组规格</a>
                        </div>
                        <table cellpadding="0" cellspacing="0" border="0" class="form-table add-table" width="100%" id="batchSet"  <%if (standardRecordList.Count > 0 && pageProduct.StandardType == (int)ProductStandardType.Single){ %> <%}else{ %> style="display:none;"<%} %>>                              
                            <thead><tr><td colspan="4">批量填充规格:</td></tr>
                                <tr><td > <span class="red">*</span>本站价</td><td ><span class="red">*</span>市场价</td><td><span class="red">*</span>库存</td><td >货号</td></tr></thead>
                            <tbody>                          
                            <tr>
                            <td><input type="text" id="bzj" value="" placeholder="*本站价" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" maxlength="8"></td>
                            <td><input type="text" id="scj" value="" placeholder="*市场价" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" maxlength="8"></td>
                            <td><input type="text" id="kc" value="" placeholder="*库存" onkeyup="value=value.replace(/[^\d]/g,'')" onafterpaste="value=value.replace(/[^\d]/g,'')" onblur="value=value.replace(/[^\d]/g,'')" maxlength="8"></td>
                            <td><input type="text" id="hh" value="" placeholder="货号"   maxlength="15"></td>
                             </tr>
                            <tr><td colspan="4" ><input type="button" id="plsc" value=" 确 定 " onclick="BatchSetStandard();" /></td></tr></tbody></table>
                        <br />
                        <table cellpadding="0" cellspacing="0" border="0" class="form-table add-table" width="100%" id="standardbox">
                            <%
                                int headCount = 1;
                                foreach (ProductTypeStandardRecordInfo standRecord in standardRecordList)
                                {
                                    if (headCount == 1) Response.Write(ShowStandardHead(standRecord, pageProduct.StandardType));
                                    Response.Write(ShowStandardSelect(standRecord, pageProduct.StandardType, headCount));
                                    headCount++;
                                } %>
                        </table>                    
                    </div>
                </div>
            </div>
            <!--商品一口价 库存 详情-->
            <div class="product-row" style="padding-bottom: 25px;">
                <div class="product-main">
                    <div class="form-row">
                        <div class="head">一口价及库存：</div>
                        <table width="500" border="0" cellpadding="0" cellspacing="1">
                            <tr>
                                <td><span class="red">*</span>本站价</td>
                                <td><span class="red">*</span>市场价</td>
                                <td><span class="red">*</span>排序号</td>
                                <td><span class="red">*</span>总库存</td>
                                <td><span class="red">*</span>库存下限</td>
                            </tr>
                            <tr>
                                <td>
                                    <SkyCES:TextBox ID="SalePrice" CssClass="txt" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this);checkSalePrice();" runat="server" MaxLength="10" CanBeNull="必填" RequiredFieldType="金额" /></td>
                                <td>
                                    <SkyCES:TextBox ID="MarketPrice" CssClass="txt" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this);checkMarketPrice();" runat="server" MaxLength="10" CanBeNull="必填" RequiredFieldType="金额" /></td>
                                <td>
                                    <SkyCES:TextBox ID="OrderID" onkeyup="value=value.replace(/[^\d]/g,'')" onafterpaste="value=value.replace(/[^\d]/g,'')" CssClass="txt" runat="server" MaxLength="10" Text="0" CanBeNull="必填" RequiredFieldType="金额" /></td>
                                <td>
                                    <SkyCES:TextBox ID="TotalStorageCount" CssClass="txt"  onkeyup="value=value.replace(/[^\d]/g,'')" onafterpaste="value=value.replace(/[^\d]/g,'')" runat="server" MaxLength="10" CanBeNull="必填" RequiredFieldType="数据校验" /></td>
                                <td>
                                    <SkyCES:TextBox ID="LowerCount"  onkeyup="value=value.replace(/[^\d]/g,'')" onafterpaste="value=value.replace(/[^\d]/g,'')" CssClass="txt" runat="server" MaxLength="10" Text="10" CanBeNull="必填" RequiredFieldType="数据校验" />
                                </td>
                            </tr>
                         <%--   <tr>
                                <td colspan="5" style="text-align: left;">当库存小于等于库存下限时商品统计会标红</td>
                            </tr>--%>
                        </table>
                        <input name="HidTotalStorageCount" type="hidden" id="HidTotalStorageCount" value="0" runat="server">
                    </div>
                    <div class="clear"></div>
                        <div class="form-row" style="display: none;">
                        <div class="head">计量单位：</div>
                        <SkyCES:TextBox ID="Units" CssClass="txt" runat="server" Text="" />
                    </div>
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">商品重量：</div>
                        <SkyCES:TextBox ID="Weight" CssClass="txt" runat="server" Text="0" RequiredFieldType="金额" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)"/>
                        <span>(克)</span>
                    </div>
                    <div class="clear"></div>
				</div>
				<div class="clear"></div>
			</div>
            <div class="form-row" style="float: none; padding-left: 108px; margin-bottom: 30px;">
                <div class="head" style="width: 100px;">商品描述：</div>
                <div class="content-box" id="ig-tab">
                    <ul class="tab clearfix">
                        <li class="cur">PC端</li>
                        <li>移动端</li>
                    </ul>
                    <div class="main" style="border: 0;">
                        <div class="row">
                            <textarea class="input" id="Introduction" name="Introduction" style="width: 700px; height: 400px" runat="server"></textarea>
                        </div>
                        <div class="row hidden">
                            <textarea class="input" id="Introduction_Mobile" name="Introduction" style="width: 700px; height: 400px" runat="server"></textarea></div>
                    </div>
                </div>
            </div>
            <!--商品售后服务-->
            <div class="product-row" style="padding-bottom: 30px;">
                <!--<div class="product-head">2. 售后服务：</div>-->
                <div class="product-main">
                    <div class="form-row">
                        <div class="head" style="font-size: 14px; font-weight: bold;">2. 售后服务</div>
                        <textarea class="input" id="Remark" name="Remark" style="width: 700px; height: 250px" runat="server"></textarea>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <!--商品其他信息-->
            <div class="product-row">
                <div class="product-head" style="text-indent: 14px;">3. 其他信息</div>
                <div class="product-main">
                	<div class="head" style="float: left; width: 92px; text-indent: 10px; line-height: 29px;">相关设置：</div>
                    <div class="form-checkbox form-nohead">
                        <label class="ig-checkbox <%if (pageProduct.IsSpecial == 1)
                                                    {%>checked<%} %>">
                            <asp:CheckBox ID="IsSpecial" runat="server" />特价

                        </label>
                        <label class="ig-checkbox <%if (pageProduct.IsNew == 1)
                                                    {%>checked<%} %>">
                            <asp:CheckBox ID="IsNew" runat="server" />新品

                        </label>
                        <label class="ig-checkbox <%if (pageProduct.IsHot == 1)
                                                    {%>checked<%} %>">
                            <asp:CheckBox ID="IsHot" runat="server" />热销

                        </label>
                        <label class="ig-checkbox <%if (pageProduct.IsSale == 1 || pageProduct.Id <= 0)
                                                    {%>checked<%} %>">
                            <asp:CheckBox ID="IsSale" Checked="true" runat="server" />上架

                        </label>
                        <label class="ig-checkbox <%if (pageProduct.IsTop == 1)
                                                    {%>checked<%} %>">
                            <asp:CheckBox ID="IsTop" runat="server" />推荐

                        </label>
                        <label class="ig-checkbox <%if (pageProduct.AllowComment == 1 || pageProduct.Id <= 0)
                                                    {%>checked<%} %>">
                            <asp:CheckBox ID="AllowComment" Checked="true" runat="server" />

                            允许评论</label>
                    </div>
                </div>
            
            <!--商品SEO-->
            <div class="product-row">
                <div class="product-main">
                    <div class="form-row">
                        <div class="head">搜索优化：</div>
                        <SkyCES:TextBox ID="Sub_Title" CssClass="txt txt-400" MaxLength="50" runat="server" placeholder="标题" Width="500px" />

                    </div>
                    <div class="form-row">
                        <div class="head"></div>
                        <SkyCES:TextBox ID="Keywords" CssClass="txt txt-400" MaxLength="100" runat="server" placeholder="关键词" Width="500px" />
                        多个关键字以空格分开                
                    </div>
                    <div class="form-row">
                        <div class="head"></div>
                        <SkyCES:TextBox ID="Summary" CssClass="text text-600" runat="server" MaxLength="200" TextMode="MultiLine" placeholder="描述" Width="500px" />
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
</div>
            <div class="product-row" style="display:none;">
            	<div class="product-tip">07</div>
                <div class="product-head">关联商品</div>
                <div class="product-main">
                	<div class="form-row form-nohead">
                        <asp:DropDownList ID="RelationClassID" runat="server" CssClass="select" Width="210" />    
                    </div>
                    <div class="form-row form-nohead">
                        <asp:DropDownList ID="RelationBrandID" runat="server" CssClass="select" Width="210" />    
                    </div>
                    <div class="clear"></div>
                    <SkyCES:TextBox ID="ProductName" CssClass="form-select-txt" runat="server" MaxLength="20" placeholder="标题关键词"/>
                    <input id="SearchRProduct" type="button"  class="form-select-submit ease" value=" 搜 索 " onclick="searchRelationProduct()" />
                    <div class="form-relink">
                        <div id="CandidateProductBox">
                        <asp:ListBox ID="CandidateProduct" runat="server" SelectionMode="Multiple" CssClass="all"></asp:ListBox>         
                        </div>           	
                        <div class="button">
                        	<a href="javascript:;" class="addall ease"  onclick="addAll('<%=IDPrefix%>CandidateProduct','<%=IDPrefix%>Product')">全部关联 

<font>&gt;&gt;</font></a>
                            <a href="javascript:;" class="addone ease" onclick="addSingle('<%=IDPrefix%>CandidateProduct','<%=IDPrefix%>Product')">关联 <font>&gt;</font></a>
                            <a href="javascript:;" class="delone ease" onclick="dropSingle('<%=IDPrefix%>Product')"><font>&lt;</font> 取消关联</a>
                            <a href="javascript:;" class="delall ease" onclick="dropAll('<%=IDPrefix%>Product')"><font>&lt;&lt;</font> 全部取消

关联</a>
                        </div>
                        <asp:ListBox ID="Product" runat="server" SelectionMode="Multiple" CssClass="select"></asp:ListBox>                         
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="product-row" style="display:none;">
            	<div class="product-tip">08</div>
                <div class="product-head">商品配件</div>
                <div class="product-main">
                	<div class="form-row form-nohead">
                        <asp:DropDownList ID="AccessoryClassID" runat="server" CssClass="select" Width="210" />                      
                    </div>
                    <div class="form-row form-nohead">
                        <asp:DropDownList ID="AccessoryBrandID" runat="server" CssClass="select" Width="210" /> 
                    </div>
                    <div class="clear"></div>
                    <SkyCES:TextBox ID="AccessoryProductName" CssClass="form-select-txt" runat="server"/>  
                    <input id="SearchPAccessory" type="button" value=" 搜 索 " class="form-select-submit ease" onclick="searchProductAccessory()" />
                    <div class="form-relink">
                        <div id="CandidateAccessoryBox">
                        <asp:ListBox ID="CandidateAccessory" runat="server" CssClass="all" SelectionMode="Multiple"></asp:ListBox>
                    	</div>
                        <div class="button">
                        	<a href="javascript:;" class="addall ease" onclick="addProductAccessoryAll('<%=IDPrefix%>CandidateAccessory','<%=IDPrefix%>Accessory')">全部关联 

<font>&gt;&gt;</font></a>
                            <a href="javascript:;" class="addone ease" onclick="addProductAccessorySingle('<%=IDPrefix%>CandidateAccessory','<%=IDPrefix%>Accessory')">关联 <font>&gt;</font></a>
                            <a href="javascript:;" class="delone ease" onclick="dropSingle('<%=IDPrefix%>Accessory')"><font>&lt;</font> 取消关联</a>
                            <a href="javascript:;" class="delall ease" onclick="dropAll('<%=IDPrefix%>Accessory')"><font>&lt;&lt;</font> 全部取消

关联</a>
                        </div>
                        <asp:ListBox ID="Accessory" runat="server" CssClass="select" SelectionMode="Multiple"></asp:ListBox>
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="product-row" style="display:none;">
            	<div class="product-tip">09</div>
                <div class="product-head">关联文章</div>
                <div class="product-main">
                	<div class="form-row form-nohead">
                        <asp:DropDownList ID="ArticleClassID" runat="server" CssClass="select" Width="210" />                         
                    </div>
                    <div class="clear"></div>
                    <SkyCES:TextBox ID="ArticleName" CssClass="form-select-txt" runat="server" placeholder="标题关键词" value="" MaxLength="20" /> <input id="SearchAN" type="button" class="form-select-submit ease" value=" 搜 索 " onclick="searchRelationArticle()" />
                    <div class="form-relink">
                        <div id="CandidateArticleBox">
                        <asp:ListBox ID="CandidateArticle" CssClass="all" runat="server" SelectionMode="Multiple"></asp:ListBox>   
                        </div>                 	
                        <div class="button">
                        	<a href="javascript:;" class="addall ease" onclick="addAll('<%=IDPrefix%>CandidateArticle','<%=IDPrefix%>Article')">全部关联 

<font>&gt;&gt;</font></a>
                            <a href="javascript:;" class="addone ease" onclick="addSingle('<%=IDPrefix%>CandidateArticle','<%=IDPrefix%>Article')">关联 <font>&gt;</font></a>
                            <a href="javascript:;" class="delone ease" onclick="dropSingle('<%=IDPrefix%>Article')"><font>&lt;</font> 取消关联</a>
                            <a href="javascript:;" class="delall ease" onclick="dropAll('<%=IDPrefix%>Article')"><font>&lt;&lt;</font> 全部取消

关联</a>
                        </div>
                        <asp:ListBox ID="Article" CssClass="select" runat="server" SelectionMode="Multiple"></asp:ListBox>   
                        <div class="clear"></div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            
        </div>
        <div class="form-foot btnfix" style="float: none; width: 100%; padding: 20px 0 5px !important;text-align: center;">
            <asp:Button CssClass="form-submit ease" style="margin: 0 25px 0 0 !important; float: none;display: inline-block;" ID="SubmitButton" Text="发布" runat="server"  OnClick="SubmitButton_Click" OnClientClick="return checkProduct()"/>
         
               <!--添加商品 可以保存草稿-->          
             <asp:Button CssClass="form-submit ease" style="float: none; display: inline-block;" ID="DraftButton" Text="保存草稿" runat="server"  OnClick="DraftButton_Click" OnClientClick="return checkProduct()" />
            
        </div>
    </div>

    <script language="javascript" type="text/javascript">
        $("#ptitle").text(40-$("#<%=Name.ClientID%>").val().length);
        $("#psell").text(200-$("#<%=SellPoint.ClientID%>").val().length);
        var productID=<%=productID%>;
        var isUpdate=<%=isUpdate%>;
        var standardRecordsCount=<%=standardRecordList.Count%>;
        //console.log(isUpdate);
    </script>	 
    <script src="Js/ProductAdd.js"></script>
    <!--如果商品类型有规格，则自动开启-->
    <%if(standardList.Count>0){%><script>$("#openStandard").click();</script><%} %>
    <script>
        $(function(){
            $(".product-container").on("change",".ig-checkbox input",function(){
                if (this.checked) {
                    $(this).parent().addClass("checked");
                    if ($(this).attr("ig-bind") && $("input[ig-bind='" + $(this).attr("ig-bind") + "']").length == $("input[ig-bind='" + $(this).attr("ig-bind") + "']:checked").length) {
                        $(".ig-checkbox .checkall[bind='" + $(this).attr("ig-bind") + "']").attr({ "checked": true }).parent().addClass("checked");
                    }
                } else {
                    $(this).parent().removeClass("checked");
                    if ($(this).attr("ig-bind") && $("input[ig-bind='" + $(this).attr("ig-bind") + "']").length != $("input[ig-bind='" + $(this).attr("ig-bind") + "']:checked").length) {
                        $(".ig-checkbox .checkall[bind='" + $(this).attr("ig-bind") + "']").attr({ "checked": false }).parent().removeClass("checked");
                    }
                }
            });
        });
        function loadProducts(tag){
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: '选择产品',
                fix: false,
                shadeClose: true,
                maxmin: true,
                area: ['1200px', '600px'],
                content: 'SelectProduct.aspx?Tag='+tag+''
            });
        }
        //后移商品图片
        function MoveDownProductPhoto(photoid) {
            if (confirm("您确定将此图片往后移一位？")) {
                $.ajax({
                    type: 'get',
                    url: "Ajax.aspx?Action=MoveDownProductPhoto&productPhotoId=" + photoid,
                    //data: {},
                    cache: false,
                    //dataType: 'json',
                    success: function (content) {
                        if (content == "ok") {
                            window.location.reload();
                            //刷新页面且定位在“图片及描述”栏目
                            //window.location.href="?eq=3&ID=<%=RequestHelper.GetQueryString<int>("ID")%>";  
                        }
                        else {
                            alert("系统忙，请稍后重试");
                        }
                    },
                    error: function () {

                        alert("系统忙，请稍后重试");
                    }
                });
            }
        }
        //前移商品图片
        function MoveUpProductPhoto(photoid) {
            if (confirm("您确定将此图片往前移一位？")) {
                $.ajax({
                    type: 'get',
                    url: "Ajax.aspx?Action=MoveUpProductPhoto&productPhotoId=" + photoid,
                    //data: {},
                    cache: false,
                    //dataType: 'json',
                    success: function (content) {
                        if (content == "ok") {
                            window.location.reload();
                            //刷新页面且定位在“图片及描述”栏目
                            //window.location.href="?eq=3&ID=<%=RequestHelper.GetQueryString<int>("ID")%>";                          
                        }
                        else {
                            alert("系统忙，请稍后重试");
                        }
                    },
                    error: function () {

                        alert("系统忙，请稍后重试");
                    }
                });
            }
        }
        $(function(){
            var _eq="<%= RequestHelper.GetQueryString<string>("eq")%>";
            
            if(_eq!=null && _eq>0){
                //console.log(_eq);
                //$(".tab-title span").eq(_eq).click();
                $(".tab-title span").eq(_eq).addClass('cur').siblings().removeClass('cur');
                $('.product-container .product-row').eq(_eq).show().siblings().hide();
            }
        })
    </script>
    <script>
        ; !function () {


            //页面一打开就执行，放入ready是为了layer所需配件（css、扩展模块）加载完毕
            $("#cutImage").click(function () {
                var orgImage = $("#ctl00_ContentPlaceHolder_Photo").val();
                if (orgImage.length>0) {
                    layer.open({
                        type: 2,
                        //skin: 'layui-layer-lan',
                        title: '图片裁剪',
                        fix: false,
                        shadeClose: true,
                        maxmin: true,
                        area: ['900px', '500px'],
                        content: 'EditPhoto.aspx?Photo=' + orgImage + '&TableID=<%=ProductBLL.TableID%>&TargetID=ctl00_ContentPlaceHolder_Photo&MakeNail=1'
                        });
                    } else {
                        alert("请先上传图片再裁剪");
                    }
                });
            //官网欢迎页              

        }();
            //裁剪主图
            function loadCoverPhotoCut(){
                var orgImage = $("#ctl00_ContentPlaceHolder_Photo").val();
                if (orgImage.length>0) {
                    layer.open({
                        type: 2,
                        //skin: 'layui-layer-lan',
                        title: '图片裁剪',
                        fix: false,
                        shadeClose: true,
                        maxmin: true,
                        area: ['900px', '500px'],
                        content: 'EditPhoto.aspx?Photo=' + orgImage + '&TableID=<%=ProductBLL.TableID%>&TargetID=ctl00_ContentPlaceHolder_Photo&MakeNail=1'
                });           
            } else {
                alert("请先上传图片再裁剪");
            }
        }
        //裁剪图集
        function loadCut(orgImage,targetImg,productPhotoId){
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: '图片裁剪',
                fix: false,
                shadeClose: true,
                maxmin: true,
                area: ['1100px', '600px'],
                content: 'EditPhoto.aspx?Photo=' + orgImage + '&TableID=<%=ProductPhotoBLL.TableID%>&TargetID='+targetImg+'&MakeNail=1&TargetType=1&ProductID=<%=productID%>&ProStyle=0&ProductPhotoID='+productPhotoId
            });
        }
        //规格全选--自动生成单规格
        function chooseAll(obj) {
            var flag=false;
            if (standardRecordsCount > 0)
            {
                if (!confirm("此操作将清除该商品已有规格记录，确认继续吗?")) {
                    flag=false;
                    event.preventDefault();
                }
                else
                {
                    flag=true;
                }
            }
            else
            {
                flag=true;
            }
            if(flag==true){
                var divId = $(obj).attr("_divId");
                $("#"+divId+" input[type='checkbox']").each(function () {
                    $(this).prop("checked", $(obj).prop("checked"));          
                });
                $("#"+divId+" label").each(function () {
                    if($(obj).is(":checked"))   $(this).addClass("checked");
                    else $(this).removeClass("checked");
                });
                //如果选中则自动生成单产品规格
                if($(obj).is(":checked")){  setStandardList(1,1);}
                    //取消则关闭规格
                else { 
                    $("#standardbox").html("");
                    $("#standardbox").hide();
                    $("#batchSet").hide();       
                }
            }
        }
        //属性全选
        function chooseAllAttr(obj) {
            var atId = $(obj).attr("_atid");
            $("#attr"+atId+" input[type='checkbox']").each(function () {
                $(this).prop("checked", $(obj).prop("checked"));
            });
            $("#attr"+atId+" label").each(function () {
                if($(obj).is(":checked"))   $(this).addClass("checked");
                else $(this).removeClass("checked");
            });
        }
        //规格单项选择--自动生成单规格
        function RecreateStandard(sType, pType)
        {
            var flag=false;
            if (standardRecordsCount > 0)
            {
                if (!confirm("此操作将清除该商品已有规格记录，确认继续吗?")) {
                    flag=false;
                    event.preventDefault();
                 
                }
                else
                {
                    flag=true;
                }
            }
            else
            {
                flag=true;
            }
            if(flag==true){
                setStandardList(sType, pType);
            }
        }
</script>
</asp:Content>
