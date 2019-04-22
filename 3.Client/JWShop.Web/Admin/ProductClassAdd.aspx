<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductClassAdd.aspx.cs" Inherits="JWShop.Web.Admin.ProductClassAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/static/css/plugin.css" />
<link href="/Admin/kindeditor/themes/default/default.css" rel="stylesheet" />
<link href="/Admin/kindeditor/plugins/code/prettify.css" rel="stylesheet" />
<script src="/Admin/kindeditor/kindeditor.js"></script>
<script src="/Admin/kindeditor/lang/zh_CN.js"></script>
<script src="/Admin/kindeditor/plugins/code/prettify.js"></script>
<script src="/Admin/layer/layer.js"></script>
<script src="/Admin/static/js/colpick.js"></script>    
<script >
    var KE;
    $(document).ready(function () {
        KindEditor.ready(function (K) {
            KE = K.create('#ctl00_ContentPlaceHolder_Description', {
                cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                allowFileManager: true
            });   
            prettyPrint();
        });
    });
</script>
    <div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
    	<div class="product-row">
            <div class="form-row">
                <div class="head">上级分类：</div>
		        <asp:DropDownList ID="FatherID" runat="server" CssClass="select" />
	        </div>                        
	         <div class="form-row">
                <div class="head">分类名称：</div>
                <SkyCES:TextBox ID="ClassName" CssClass="txt" runat="server" MaxLength="60" CanBeNull="必填"  placeholder="长度限制1-60个字符之间"/>
            </div> 
            <div class="form-row">
                <div class="head">英文名称：</div>
                <SkyCES:TextBox ID="EnClassName" CssClass="txt" Width="400px" runat="server" MaxLength="50" placeholder="长度限制1-60个字符之间"/>
            </div>
	         <div class="form-row">
                <div class="head">产品类型：</div>
                <asp:DropDownList ID="ProductType" runat="server" CssClass="select" />
            </div>
	        <div class="form-row">
		        <div class="head">排序ID：</div>
		        <SkyCES:TextBox ID="OrderID" CssClass="txt" runat="server" Width="300px" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"  />
	        </div>

            <div class="form-row">
		        <div class="head">分类图片：</div>
                <a id="imgurl_Photo" <%if (!string.IsNullOrEmpty(Photo.Text)){%>href="<%=Photo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Photo.Text)%>" class="icon"  height="50" id="img_Photo"/></a>
		        <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
	        </div>
	        <div class="form-row">		       
                <div class="form-upload">
		        	<iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=ProductBLL.TableID%>&FilePath=ProductCoverPhoto/Original&NeedMark=0" width="400" height="30px" frameborder="0" allowTransparency="true" scrolling="no"></iframe>
                </div>
	        </div>
            <div class="form-row" style="display:none;">
		        <div class="head">手机图片：</div>
            <a id="imgurl_Keywords" <%if (!string.IsNullOrEmpty(Keywords.Text))
                 {%>href="<%=Keywords.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Keywords.Text)%>" class="icon"  height="50" id="imgurl_Keywords"/></a>
		        <SkyCES:TextBox ID="Keywords" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
	        </div>
            <div class="form-row"  style="display:none;">
		        <div class="form-upload">
		        	<iframe src="UploadAdd.aspx?Control=Keywords&TableID=<%=ProductBLL.TableID%>&FilePath=ProductCoverPhoto/Original" width="400" height="30px" frameborder="0" allowTransparency="true" scrolling="no"></iframe>
                </div>
	        </div>
	        <div class="form-row" style="display:none;">
		        <div class="head">售后服务：</div>
		      <%--  <SkyCES:TextBox ID="Description" CssClass="txt" runat="server" Width="400px" Height="100px" TextMode="MultiLine" />--%>
                 <textarea class="input" id="Description" name="Description" style="width:100%;height:300px" runat="server" ></textarea>
	        </div>
            <div class="form-row"  style="display:none;">
                <div class="head">页面标题：</div>
                <SkyCES:TextBox ID="PageTitle" CssClass="txt" MaxLength="40" runat="server" Width="400px" ig-max-controller onkeydown="gbcount(this,40,$('#seoTitle'));" onkeyup="gbcount(this,40,$('#seoTitle'));" onblur="gbcount(this,40,$('#seoTitle'));"/>
                        还能输入<strong  id="seoTitle">40</strong>字
            </div>
            <div class="form-row" style="display:none;">
                <div class="head">页面关键字：</div>
                <SkyCES:TextBox ID="PageKeyWord" CssClass="txt" MaxLength="50" runat="server" Width="400px" ig-max-controller onkeydown="gbcount(this,50,$('#seoKeyword'));" onkeyup="gbcount(this,50,$('#seoKeyword'));" onblur="gbcount(this,50,$('#seoKeyword'));"/>
                        还能输入<strong  id="seoKeyword">50</strong>字,多个关键字以空格分开
            </div>
            <div class="form-row" style="display:none;">
                <div class="head">页面描述：</div>
                <SkyCES:TextBox ID="PageSummary" CssClass="text" MaxLength="100" runat="server" Width="400px" Height="100px" TextMode="MultiLine" ig-max-controller onkeydown="gbcount(this,100,$('#seoSummary'));" onkeyup="gbcount(this,100,$('#seoSummary'));" onblur="gbcount(this,100,$('#seoSummary'));"/>
                        还能输入<strong  id="seoSummary">100</strong>字
            </div>
        </div>
    </div>
    <div class="form-foot">
        <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
    </div>
</div>
     <script language="javascript" type="text/javascript">        
         $('#seoTitle').text(40-$("#<%=PageTitle.ClientID%>").val().length);
         $('#seoKeyword').text(50-$("#<%=PageKeyWord.ClientID%>").val().length);
         $('#seoSummary').text(100-$("#<%=PageSummary.ClientID%>").val().length);
         </script>
</asp:Content>	