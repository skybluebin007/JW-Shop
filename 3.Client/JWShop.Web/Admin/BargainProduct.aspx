<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BargainProduct.aspx.cs" Inherits="JWShop.Web.Admin.BargainProduct" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<div class="cont">
    <input type="hidden" name="Id" value="<%=int.MinValue %>""/>
    <input type="hidden" name="Product_Id" value="<%=product.Id %>"/>
    <input type="hidden" name="BargainId" value="<%=int.MinValue %>"/>
    <div class="form-row">
		<div class="head"><s>*</s>商品名称：</div>
		<input type="text"  class="txt" name="Product_Name" value="<%=product.Name %>"/>
	</div>
	<div class="form-row">
		<div class="head"><s>*</s>商品原价：</div>
		<input type="text"  class="txt txt2" name="Product_OriginalPrice" readonly="readonly" value="<%=product.MarketPrice %>"/><i class="tps">支持两位小数</i>
	</div>
	<div class="form-row">
		<div class="head"><s>*</s>商品底价：</div>
		<input type="text"  class="txt txt2" onkeyup="value=value.replace(/[^(\-)?\d+(\.\d{1,2})?$]/g,'')" name="Product_ReservePrice"/><i class="tps">不高于原价，如果设置为0元，用户将不进行支付直接领取</i>
	</div>
	<div class="form-row">
		<div class="head"><s>*</s>商品库存：</div>
		<input type="text"  class="txt txt2" onkeyup="value=value.replace(/[^\d+$ ]/g,'')" name="Stock"/>
	</div>
	<div class="form-row">
		<div class="head">排序号：</div>
		<input type="text"  class="txt txt2" onkeyup="value=value.replace(/[^\d+$]/g,'')" name="Sort"/><i class="tps">用于控制商品排序，数字越大排序越靠前</i>
	</div>
</div>
