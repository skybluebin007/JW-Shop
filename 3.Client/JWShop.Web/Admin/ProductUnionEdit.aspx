<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductUnionEdit.aspx.cs" Inherits="JWShop.Web.Admin.ProductUnionEdit" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">	
<link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">    
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartAddDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndAddDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <script language="javascript" src="/Admin/js/ProductBatchEdit.js" type="text/javascript"></script>
    <style>
        .listBlock {width:100%; height:30px; margin:10px 0;
        }
        .listBlock a { padding:10px 15px; border:1px solid #ccc;
        }
    </style>
<div class="container ease" id="container">
    	<div class="product-container product-container-border">
            <dl class="product-filter product-filter-pro clearfix" style="clear: both;">
                <dd>
                    <div class="head">商品名称：</div>
                    <SkyCES:TextBox CssClass="txt" ID="Name" runat="server" Width="100px" /></dd>
                <dd>
                    <div class="head">商品分类：</div>
                    <asp:DropDownList ID="ClassID" runat="server" CssClass="select" />
                </dd>
                <dd>
                    <div class="head">品牌：</div>
                    <asp:DropDownList ID="BrandID" runat="server" CssClass="select" />
                </dd>
                <dd>
                    <div class="head">发布时间：</div>
                    <SkyCES:TextBox CssClass="txt" ID="StartAddDate" runat="server" RequiredFieldType="日期时间" />
                    <span class="tp">到</span>
                    <SkyCES:TextBox CssClass="txt" ID="EndAddDate" runat="server" RequiredFieldType="日期时间" /></dd>
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" /></dt>
            </dl>
<div style="clear:both"></div>
<div class="listBlock" style=" clear:both; display:block;">
        <a href="ProductSingleEdit.aspx">逐个编辑</a>
        <a href="ProductUnionEdit.aspx">统一编辑</a>
</div>
<div class="pageMark"></div>
<asp:Repeater ID="RecordList" runat="server">
	<ItemTemplate>
		<div class="nameBlock" title="<%# Eval("Name")%>" id="Product<%# Eval("ID")%>">
		<%# StringHelper.Substring(Eval("Name").ToString(),13)%><img src="Style/Images/delete.gif" title="删除" onclick="deleteProduct(<%# Eval("ID")%>)" />
		<span style="display:none"><input name="ProductID" value="<%# Eval("ID")%>" type="checkbox" checked="checked" /></span>
		</div>
	</ItemTemplate>
</asp:Repeater>
            <div style="clear: both; width: 100%; height: 2px;"></div>
            <div class="form-row">
                <span style="color: red">友情提示：文本框不写表示不修改此项</span>
            </div>
            <div class="form-row">
                <div class="head">市场价：</div>
                <input type="text" class="txt" id="MarketPrice" />
                元 
            </div>
            <div class="form-row">
                <div class="head">本站价：</div>
                <input type="text" class="txt" id="SalePrice" />
                元 
            </div>
            <div class="form-row">
                <div class="head">重量：</div>
                <input type="text" class="txt" id="Weight" />
                克 
            </div>

            <div class="form-row" style="display: none;">
                <div class="head">赠送积分：</div>
                <input type="text" class="txt" id="SendPoint" />
            </div>
            <div <%if (ShopConfig.ReadConfigInfo().ProductStorageType == (int)ProductStorageType.SelfStorageSystem)
                   { %>style="display:none" <%} %>>
                <div class="head">总库存数量：</div>
                <input type="text" class="txt" id="TotalStorageCount" />
            </div>
            <div class="form-row">
                <div class="head">库存下限：</div>
                <input type="text" class="txt" id="LowerCount" />
            </div>
            <div class="form-row" style="display: none;">
                <div class="head">库存上限：</div>
                <input type="text" class="txt" id="UpperCount" />
            </div>
        </div>
    <div class="form-foot">
        <input type="button"  value=" 确 定 " class="form-submit ease" Style="margin: 0; position: static;"  onclick="saveUnionEdit()"/> 
    </div>
    </div>
</asp:Content>
