<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductGroupAdd.aspx.cs" Inherits="JWShop.Web.Admin.ProductGroupAdd" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script language="javascript" type="text/javascript" src="/Admin/js/ProductGroupAdd.js"></script>
    <div class="container ease" id="container" style="left:0; right:0; top:0; bottom:0; width:1000px;">
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">PC端图片：</div>                           
                <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" style="display:none;"/>                        
            </div>
            <div class="form-upload">                
                <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityPhoto/Original&NeedMark=0&NeedNail=0" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>  
                </div> 
            <div class="form-row">
                <div class="head">移动端图片：</div>
                <SkyCES:TextBox ID="PhotoMobile" CssClass="txt" runat="server" Width="400px" style="display:none;"/>                
            </div>
            <div class="form-upload"> 
                <iframe src="UploadAdd.aspx?Control=PhotoMobile&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityPhoto/Original&NeedMark=0&NeedNail=0" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
               </div>
            <div class="form-row">
                <div class="head">PC端更多地址：</div>
                <SkyCES:TextBox ID="Link" CssClass="txt" runat="server" Width="400px" /> 不可使用#号
            </div>
            <div class="form-row">
                <div class="head">移动端更多地址：</div>
                <SkyCES:TextBox ID="LinkMobile" CssClass="txt" runat="server" Width="400px" /> 不可使用#号
            </div>
            <div class="form-row">
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
    <input type="hidden" name="RelationProductID" id="RelationProductID" />

    </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" OnClientClick="return checkSubmit()" />
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var action='<%=RequestHelper.GetQueryString<string>("Action")%>';
        var id='<%=RequestHelper.GetQueryString<string>("ID") %>';
        var themeActivityID='<%=RequestHelper.GetQueryString<string>("ThemeActivityID") %>';
        if(action=="Update"){
            readProductGroup(id,themeActivityID);
        }
    </script>
</asp:Content>
