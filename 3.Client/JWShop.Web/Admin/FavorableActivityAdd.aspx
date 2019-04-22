<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="FavorableActivityAdd.aspx.cs" Inherits="JWShop.Web.Admin.FavorableActivityAdd" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <style type="text/css">
        .form-txt .head, .form-txt2 .head { left: -40px; width: 120px; }
   
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>

    <div class="container ease" id="container">
        <!--小程序优惠活动，做成满立减，有开始时间、结束时间，现金减免--!>
    	<!--<div class="path-title"></div>-->
        <div class="product-container product-container-border product-container-mt70">
            <div class="product-row">
                <!--<div class="product-tip">01</div>-->
                <div class="product-head">基本信息</div>
                <div class="product-main">
                	<div class="form-row">
                    	<div class="head">标题：</div>
                        <SkyCES:TextBox ID="Name" CssClass="txt" width="400" runat="server" MaxLength="60" CanBeNull="必填" />
                    </div>
                    <div class="clear"></div>
                    <div class="form-row" style="display:none;">
                    	<div class="head">封面图片：</div>
                    	<div id="imgUpLoad">
                            <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="236px" />
                            <div class="form-upload">
                                <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=FavorableActivityBLL.TableID%>&FilePath=GiftPackPhoto/Original&NeedMark=0" width="300" height="40px" frameborder="0" allowTransparency="true" scrolling="no" id="uploadIFrame"></iframe>
                            </div>
                            <input class="form-cut" type="button" id="cutImage" value="裁剪图片" />
                        </div>                      
                    </div>
                    <div class="clear"></div>
                    <div class="form-row" style="display:none;">
                        <div class="head">介绍：</div>
                        <SkyCES:TextBox ID="Content" CssClass="text" width="400" runat="server" MaxLength="200" TextMode="MultiLine" />
                    </div>
                    <div class="clear"></div>   
                </div>
            
                <!--<div class="product-tip">02</div>-->
                <div class="product-head">条件</div>
                <div class="product-main">
                    <div class="form-row" id="fType" style="display:none;">
                        <div class="head">活动类别：</div>
                        <input name="FavorableType" onclick="changeFavorType()" value="0" type="radio" id="type1" <%if (favorableType == 0){ %>checked="checked"<%} %> /><label for="type1"> 整站订单优惠</label>
                        &nbsp;&nbsp;&nbsp;
		                <input name="FavorableType" onclick="changeFavorType()" value="1" type="radio" id="type2" <%if (favorableType == 1){ %>checked="checked"<%} %>/><label for="type2"> 商品分类优惠</label>
                    </div>
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">活动时间：</div>
                        <SkyCES:TextBox ID="StartDate" CssClass="txt" runat="server" Width="140px"  RequiredFieldType="日期时间"  CanBeNull="必填" /> <span>到</span> <SkyCES:TextBox ID="EndDate" CssClass="txt" runat="server" Width="140px" RequiredFieldType="日期时间"  CanBeNull="必填" />
                    </div>
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">用户等级：</div>
                        <asp:CheckBoxList ID="UserGrade" runat="server" RepeatDirection="Horizontal" CssClass=""></asp:CheckBoxList>
                    </div>
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">最低金额：</div>
                        <SkyCES:TextBox ID="OrderProductMoney" CssClass="txt" runat="server" Width="60px" CanBeNull="必填" RequiredFieldType="金额" Text="0"/> 元
                    </div>
                    <div class="clear"></div>
                </div>
            
                <!--<div class="product-tip">03</div>-->
                <div class="product-head">优惠</div>
                <div class="product-main">
                    <!--运费优惠-->
                    <div class="form-row" id="ShippingFavor" style="display:none;">
                        <div class="head">运费优惠：</div>
                        <input name="ShippingWay" onclick="changeShippingWay()" value="0" type="radio" id="ShippingWay1" /> <label for="ShippingWay1"> 无运费优惠 </label>
                        &nbsp;&nbsp;&nbsp;
		                <input name="ShippingWay" onclick="changeShippingWay()" value="1" type="radio" id="ShippingWay2" /> <label for="ShippingWay2"> 免运费 </label>
                    </div>
                    <div class="clear"></div>
                    <div class="form-row" id="ShippingRegionDiv" style="display:none;">
                      <div class="head">运费优惠区域：</div>
                        <SkyCES:MultiUnlimitControl ID="RegionID" runat="server" />
                    </div>
                    <div class="clear"></div>
                    <!--运费优惠 End-->
                    <!--商品分类选择-->
                    <div class="form-row" id="ProductClassDiv" style="display:none;">
                     <div class="head">选择商品分类：</div>
                       <SkyCES:MultiUnlimitControl ID="ProductClass" Prefix="procls" runat="server" />
                    </div>
                    <div class="clear"></div>
                    <!--商品分类选择 End-->
                    <div class="form-row">
                        <div class="head">价格优惠：</div>
                        <label for="ReduceWay1" style="float:left; margin-right:10px;display:none;"><input name="ReduceWay" onclick="changeReduceWay()" value="0" type="radio" id="ReduceWay1" /> 无价格优惠 </label>
		                <label for="ReduceWay2" style="float:left; margin-right:10px;display:none;"><input name="ReduceWay" onclick="changeReduceWay()" value="1" type="radio" id="ReduceWay2" checked/> 现金减免 </label>
                        <span id="ReduceMoneyDiv"><SkyCES:TextBox CssClass="txt" Width="100px" ID="ReduceMoney" runat="server" CanBeNull="必填" RequiredFieldType="金额"  Text="0.00"/> 元 </span> 
                        <label for="ReduceWay3" style="float:left;display:none;"><input name="ReduceWay" onclick="changeReduceWay()" value="2" type="radio" id="ReduceWay3" /> 价格折扣 </label>
                        <span id="ReduceDiscountDiv" style="display:none;"><SkyCES:TextBox CssClass="txt" Width="30px" ID="ReduceDiscount" runat="server" CanBeNull="必填" RequiredFieldType="金额"  Text="0.00"/> 折(0-100间的数字，如打95折填写‘95’) </span>
                    </div>
                    <div class="clear"></div>
                    <div class="form-row" id="zslp" <%if (favorableType==(int)FavorableType.AllOrders){ %>style="display:none;"<%} %>>
                        <div class="head">赠送礼品：</div>
                        <input name="GiftWay" onclick="changeGiftWay()" value="0" type="radio" id="GiftWay1" /><label for="GiftWay1"> 无礼品赠送</label>
                        &nbsp;&nbsp;&nbsp;
		                <input name="GiftWay" onclick="changeGiftWay()" value="1" type="radio" id="GiftWay2" /><label for="GiftWay2"> 赠送以下任一种礼品 </label>
                    </div>
                    <div class="clear"></div>
                    <div id="GiftDiv" <%if (favorableType==(int)FavorableType.AllOrders){ %>style="display:none;"<%} %>>
                        <div class="form-row">
                            <div class="head">选择礼品：</div>
                            <input id="GiftName" class="txt txt-200" /> <input type="button" value="搜索" onclick="searchGift()" class="btn-button" />   
		                    <div id="SearchGiftList"></div>
                        </div>
                        <div class="clear"></div>
                        <div class="form-row" <%if (favorableType==(int)FavorableType.AllOrders){ %>style="display:none;"<%} %>>
                            <div class="head">优惠礼品：</div>
                            <div id="SelectGiftList">
		                        <%foreach (var gift in giftList){  %>
		                             <span id="Gift<%=gift.Id %>"><%=StringHelper.Substring(gift.Name,10)%><span onclick="deleteGift(<%=gift.Id %>)" style="cursor:pointer;display: inline-block;vertical-align: middle;line-height: 10px;"><img src="static/images/ico-delete.png" /></span><input name="GiftList" type="hidden" value="<%=gift.Id %>"/></span>
                                <%} %>
		                    </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" OnClientClick="return Page_ClientValidate()"/>
        </div>
    </div>

    <script type="text/javascript" src="/Admin/Js/FavorableActivityAdd.js"></script>
    <script type="text/javascript">
     var _type="<%=favorableType%>";
        function init() {
            var shippingWayObjs = os("name", "ShippingWay");
            setRadioValue(shippingWayObjs, "<%=favorableActivity.ShippingWay.ToString()%>");
            var reduceWayObjs = os("name", "ReduceWay");
            setRadioValue(reduceWayObjs, "<%=favorableActivity.ReduceWay.ToString()%>");
            var giftObjs = os("name", "GiftWay");
            var giftValue = "<%=favorableActivity.GiftId.ToString()%>";
            if (giftValue != "") {
                giftValue = "1";
            }
            else {
                giftValue = "0";
            }
            setRadioValue(giftObjs, giftValue);
            changeShippingWay();
            changeReduceWay();
            changeGiftWay();
            //if (parseInt(_type) <= 0) {//订单优惠
            //    $("#ShippingFavor").show();
            //    if ($('#ShippingFavor input[name="ShippingWay"]:checked ').val() == 1) $("#ShippingRegionDiv").show();
            //    else $("#ShippingRegionDiv").hide();
            //    $("#ProductClassDiv").hide();
            //    $("#GiftDiv").hide();
            //    $("#zslp").hide();
            //}
            //if (parseInt(_type) == 1) {//商品分类优惠
            //    $("#ShippingFavor").hide();
            //    $("#ShippingRegionDiv").hide();
            //    $("#ProductClassDiv").show();
            //    $("#zslp").show();
            //}
        }
        ////初始化判断显示、隐藏
        //init();

        //优惠类型切换
        //function changeFavorType() {
        //    var _typeId = $('#fType input[name="FavorableType"]:checked ').val();
           
        //    if (_typeId == 0) {//订单优惠
        //        $("#ShippingFavor").show();
        //        if ($('#ShippingFavor input[name="ShippingWay"]:checked ').val() == 1) $("#ShippingRegionDiv").show();
        //        else $("#ShippingRegionDiv").hide();
        //        $("#ProductClassDiv").hide();
        //        $("#GiftDiv").hide();
        //        $("#zslp").hide();
        //    }
        //    if (_typeId == 1) {//商品分类优惠
        //        $("#ShippingFavor").hide();
        //        $("#ShippingRegionDiv").hide();
        //        $("#ProductClassDiv").show();
        //        $("#zslp").show();
        //    }
           
        //}

        $(function () {
            //页面一打开就执行，放入ready是为了layer所需配件（css、扩展模块）加载完毕
            layer.ready(function () {
                $("#cutImage").click(function () {
                    var orgImage = $("#ctl00_ContentPlaceHolder_Photo").val();
                    if (orgImage.length > 0) {
                        layer.open({
                            type: 2,
                            //skin: 'layui-layer-lan',
                            title: '图片裁剪',
                            fix: false,
                            shadeClose: true,
                            maxmin: true,
                            area: ['900px', '500px'],
                            content: 'EditPhoto.aspx?Photo=' + orgImage + '&TableID=<%=FavorableActivityBLL.TableID%>&TargetID=ctl00_ContentPlaceHolder_Photo&MakeNail=1'
                        });
                    } else {
                        layer.alert("请先上传图片再裁剪");
                    }
                });
            });
        });

    </script>
</asp:Content>
