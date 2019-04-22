<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FavorableAjax.aspx.cs" Inherits="JWShop.Web.Admin.FavorableAjax" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<script type="text/javascript" src="/Admin/js/Common.js"></script>
<script type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>

                        <div class="head"><%if(typeID<=0){ %>运费优惠区域<%}else{ %>选择商品分类<%} %>：</div>
                        <SkyCES:MultiUnlimitControl ID="RegionID" Prefix="" runat="server" />

                