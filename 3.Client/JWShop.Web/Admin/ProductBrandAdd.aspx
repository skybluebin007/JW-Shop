<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductBrandAdd.aspx.cs" Inherits="JWShop.Web.Admin.ProductBrandAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>

<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
    <div class="container ease" id="container">
    	<div class="path-title"></div>
    	<div class="product-container product-container-border">
            <div class="product-row">      	
	            <div class="form-row">
		            <div class="head">名称：</div>
		            <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="400px" MaxLength="20"  CanBeNull="必填"/>
	            </div>
                <asp:PlaceHolder ID="Spell" runat="server" Visible="false">
                    <div class="form-row">
                        <div class="head">首字母拼音：</div>
                        <SkyCES:TextBox ID="Spelling" CssClass="txt" runat="server" Width="200px" MaxLength="20" CanBeNull="必填" /><span style="color:red;">如有纰漏请手动更正</span>
                    </div>
                </asp:PlaceHolder>
                <div class="form-row">
		            <div class="head">排序：</div>
		            <SkyCES:TextBox ID="OrderID" CssClass="txt" runat="server" Width="400px"  CanBeNull="必填"  RequiredFieldType="数据校验"/>
	            </div>
                <div class="form-row">
		            <div class="head">Logo：</div>
                  <a <%if (!string.IsNullOrEmpty(Logo.Text)){%>href="<%=Logo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Logo.Text)%>" class="icon"  height="50" id="nailimg"/></a>
		            <SkyCES:TextBox ID="Logo" CssClass="txt" runat="server" Width="400px" style="display:none;"/> 建议尺寸180*82
	            </div>
	          
		         <div class="form-row">
                    <div class="form-upload">
		            	<iframe src="UploadAdd.aspx?Control=Logo&TableID=<%=ProductBrandBLL.TableID %>&FilePath=BrandLogo/Original&NeedMark=0" width="400" height="30px" frameborder="0" allowTransparency="true" scrolling="no"></iframe>
                    </div>
	            </div>
	            <div class="form-row" style="display:none;">
		            <div class="head">链接地址：</div>
		            <SkyCES:TextBox ID="Url" CssClass="txt" runat="server" Width="400px" HintInfo="填写该项就表示品牌详细页直接链接到该地址，如果是外部地址，请在地址前带上Http://" />
	            </div>
	            <div class="form-row">
		            <div class="head">介绍：</div>
		            <SkyCES:TextBox ID="Description" CssClass="txt" runat="server" Width="400px" Height="100px" TextMode="MultiLine" />
	            </div>
                <div class="form-row">                	                      
                        <label class="ig-checkbox <%if(productBrand.IsTop==1){%>checked<%} %>"><asp:CheckBox ID="IsTop" runat="server" />是否推荐</label>
                </div>
              </div>
        </div>
        <div class="form-foot">		            
            <asp:Button CssClass="form-submit ease" style=" margin:0;" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
        </div>
    </div>
</asp:Content>
