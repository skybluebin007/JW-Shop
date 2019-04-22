<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductAddInit.aspx.cs" Inherits="JWShop.Web.Admin.ProductAddInit" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="container ease confix" id="container">
	<div class="product-container product-container-border">
		<div>
			<div class="threeSelect clearfix">
				<div class="list" id="firstdiv">
					<div class="thSearch"><div class="con"><s class="proIcon"></s><input type="text" name="" id="topsearch" value="" placeholder="输入名称" /></div></div>
					<ul>
	                <%foreach(var topClass in ProductClassBLL.ReadRootList()){ %>
						<li _topcid="<%=topClass.Id %>" _topname="<%=topClass.Name %>"><s class="proIcon"></s><%=topClass.Name%></li>
	                <%} %>
					
					</ul>
				</div>
				<div class="list hidden" id="seconddiv">
					<div class="thSearch"><div class="con"><s class="proIcon"></s><input type="text" name="" id="secondsearch" value="" placeholder="输入名称" /></div></div>
					<ul>
						<li><s class="proIcon"></s>手机数码</li>
						
					</ul>
				</div>
				<div class="list hidden" id="thirddiv">
					<div class="thSearch"><div class="con"><s class="proIcon"></s><input type="text" name="" id="thirdsearch" value="" placeholder="输入名称" /></div></div>
					<ul>
						<li><s class="proIcon"></s>手机数码</li>
					
					</ul>
				</div>
	            <div class="list hidden" id="branddiv">
					<div class="thSearch"><div class="con"><s class="proIcon"></s><input type="text" name="" id="brandsearch" value="" placeholder="输入名称" /></div></div>
					<ul>
						<li><s class="proIcon"></s>手机数码</li>
					
					</ul>
				</div>
			</div>
		</div>
		<div class="threeSelectFinsh">
			<span class="proIcon"></span>
			您当前选择的是：无
		</div>
		<input class="threeBtn disabled" readonly="readonly" type="" name="" disabled id="stepnext" value="我已选好商品分类，现在添加商品" />
        <input type="hidden" id="pid" value="<%=RequestHelper.GetQueryString<int>("productId") %>" />
        <input type="hidden" id="action" value="<%=RequestHelper.GetQueryString<string>("Action") %>" />
	</div>
</div>
<script type="text/javascript">
    var _chooseclassid = "", _topcid = 0, _secondcid = 0, _thirdcid = 0, _topname = "", _secondname = "",_brandid=0,_brandclassid=0,_brandname="";    
    var _secondsearchname = $("#secondsearch").val();
    var _thirdsearchname = $("#thirdsearch").val();

    //搜索1级分类
    $("#topsearch").change(function () {
        $("#secondsearch").val("");
        $("#thirdsearch").val("");
        $("#seconddiv").hide();
        $("#thirddiv").hide();
        var _topsearchname = $("#topsearch").val();
        $.ajax({
            type: 'get',
            url: "?Action=GetTopClass",
            data: {classname: _topsearchname },
            cache: false,
            dataType: 'json',
            success: function (data) {
                if (data.count > 0) {
                    var _html = "";
                    for (var i in data.dataList) {
                        var item = data.dataList[i];
                        _html += "<li _topcid='" + item.Id + "' _topname='" + item.Name + "'><s class='proIcon'></s>" + item.Name + "</li>";
                    }
                    $("#firstdiv ul").html(_html);
                    $("#firstdiv li").click(function () {
                        _topcid = $(this).attr("_topcid");
                        _topname = $(this).attr("_topname");
                        $(this).addClass("red").siblings().removeClass("red");
                        _topsearchname = $("#secondsearch").val();
                        GetSecondClassList();
                    })
                    $("#seconddiv").hide();
                    $("#thirddiv").hide();
                }
                else {
                    $("#firstdiv ul").html("没有符合条件的分类");
                }

            },
            error: function () { }
        });
    })
    //选择1级分类获取2级分类
    $("#firstdiv li").click(function () {
        _topcid = $(this).attr("_topcid");
        _topname = $(this).attr("_topname");     
        $(this).addClass("red").siblings().removeClass("red");
        _secondsearchname = $("#secondsearch").val();
        var _choosehtml = "<span class='proIcon'></span>您当前选择的是：";
        $(".threeSelectFinsh").html(_choosehtml + "<span>" + _topname + "</span>");
        _chooseclassid = "|" + _topcid + "|";
        //console.log(_chooseclassid);
        $("#secondsearch").val("");
        $("#thirdsearch").val("");
        //获取2级分类
        GetSecondClassList();
        //获取品牌列表
        _brandclassid = _topcid;
        _brandname = "";
        $("#brandsearch").val("");
        GetBrandList();
    })
    //搜索2级分类
    $("#secondsearch").change(function () {
        _secondsearchname = $("#secondsearch").val();
        GetSecondClassList();
        $("#thirdsearch").val("");
        $("#thirddiv").hide();
    })
    function GetSecondClassList() {
        //获取二级分类，先清除原有3及分类的数据并隐藏
        $("#thirddiv").hide();
        $("#thirddiv ul").html("");
        
        _secondsearchname = $("#secondsearch").val();
        $.ajax({
            type: 'get',
            url: "?Action=GetSecondClass",
            data: { topClassId: _topcid, classname: _secondsearchname },
            cache: false,
            dataType: 'json',
            success: function (data) {
                if (data.count > 0) {
                    var _html = "";
                    for (var i in data.dataList) {
                        var item = data.dataList[i];
                        _html += "<li _secondcid='" + item.Id + "' _secondname='" + item.Name + "' onclick='GetThirdClass(this);'><s class='proIcon'></s>" + item.Name + "</li>";
                    }
                    $("#seconddiv ul").html(_html);
                    $("#seconddiv").show();
                    $("#stepnext").addClass("disabled");
                    $("#stepnext").attr("disabled", "disabled");
                }
                else {
                    $("#stepnext").removeClass("disabled");
                    $("#stepnext").removeAttr("disabled");
                    if (_secondsearchname == "") {
                        $("#seconddiv ul").html("");
                        $("#seconddiv").hide();
                    }
                    else {
                        $("#seconddiv ul").html("没有符合条件的分类");
                    }                  
                }

            },
            error: function () { }
        });
    }   
    //选择2级分类获取3级分类
    function GetThirdClass(obj) {
        _secondcid = $(obj).attr("_secondcid");
        _secondname = $(obj).attr("_secondname");
        
        $(obj).addClass("red").siblings().removeClass("red");
        var _choosehtml = "<span class='proIcon'></span>您当前选择的是：";
        $(".threeSelectFinsh").html(_choosehtml + "<span>" + _topname + " > <span>" + _secondname + "</span>");
        _chooseclassid = "|" + _topcid + "|" + _secondcid + "|";
        console.log(_chooseclassid);
        $("#secondsearch").val("");
        $("#thirdsearch").val("");
        //获取3级分类
        GetThirdClassList();
        //获取品牌列表
        _brandclassid = _secondcid;
        _brandname = "";
        $("#brandsearch").val("");
        GetBrandList();
    }
    //搜索3级分类
    $("#thirdsearch").change(function () {
        _thirdsearchname = $("#thirdsearch").val();       
        GetThirdClassList();      
    })
    function GetThirdClassList() {
        _thirdsearchname = $("#thirdsearch").val();
        $.ajax({
            type: 'get',
            url: "?Action=GetThirdClass",
            data: { secondClassId: _secondcid, classname: _thirdsearchname },
            cache: false,
            dataType: 'json',
            success: function (data) {
                if (data.count > 0) {
                    var _html = "";
                    for (var i in data.dataList) {
                        var item = data.dataList[i];
                        _html += "<li _thirdcid='" + item.Id + "' _thirdname='" + item.Name + "' onclick='GetFourthClass(this);'><s class='proIcon'></s>" + item.Name + "</li>";
                    }
                    $("#thirddiv ul").html(_html);
                    $("#thirddiv").show();
                    $("#stepnext").addClass("disabled");
                    $("#stepnext").attr("disabled", "disabled");
                }
                else {
                    $("#stepnext").removeClass("disabled");
                    $("#stepnext").removeAttr("disabled");
                    if (_thirdsearchname == "") {
                        $("#thirddiv ul").html("");
                        $("#thirddiv").hide();
                    }
                    else {
                        $("#thirddiv ul").html("没有符合条件的分类");
                    }
                }
            },
            error: function () { }
        });
    }
    //选择3级分类
    function GetFourthClass(obj) {
        _thirdcid = $(obj).attr("_thirdcid");
        var _thirdname = $(obj).attr("_thirdname");
        $(obj).addClass("red").siblings().removeClass("red");
        var _choosehtml = "<span class='proIcon'></span>您当前选择的是：";
        $(".threeSelectFinsh").html(_choosehtml + "<span>" + _topname + " > <span>" + _secondname + " > <span>" + _thirdname + "</span>");    
        _chooseclassid = "|" + _topcid + "|" + _secondcid + "|"+ _thirdcid + "|";
        console.log(_chooseclassid);
        $("#stepnext").removeClass("disabled");
        $("#stepnext").removeAttr("disabled");
        //获取品牌列表
        _brandclassid = _thirdcid;
        _brandname = "";
        $("#brandsearch").val("");
        GetBrandList();
    }
    //选好分类下一步添加商品
    $("#stepnext").click(function () {
        //window.location.href = "productadd.aspx?classId=" + _chooseclassid + "&ID=" + $("#pid").val();
        if ($("#action").val() == "clone") {
            window.location.href = "productclone.aspx?classId=" + _chooseclassid + "&ID=" + $("#pid").val() + "&BrandId=" + _brandid;
        }
        else
        {
            window.location.href = "productadd.aspx?classId=" + _chooseclassid + "&ID=" + $("#pid").val() + "&BrandId=" + _brandid;
        }
    })
    //获取品牌列表
    function GetBrandList() {
            $.ajax({
            type: 'get',
            url: "?Action=GetBrandList",
            data: { classId: _brandclassid ,brandname:_brandname},
            cache: false,
            dataType: 'json',
            success: function (data) {
                if (data.count > 0) {
                    var _html = "";
                    for (var i in data.dataList) {
                        var item = data.dataList[i];
                        _html += "<li _id='" + item.Id + "' _name='" + item.Name + "' onclick='GetBrand(this);'>" + item.Name + "</li>";
                    }
                    $("#branddiv ul").html(_html);
                    $("#branddiv").show();                 
                }
                else { 
                    if ($("#brandsearch").val() == "") {
                        $("#branddiv ul").html("");
                        $("#branddiv").hide();
                    }
                    else {
                        $("#branddiv ul").html("没有符合条件的品牌");
                    }
                }

            },
            error: function () { }
        });
    }
    //搜索品牌
    $("#brandsearch").change(function () {
        _brandname = $("#brandsearch").val();
        GetBrandList();
    })
    //选择品牌
    function GetBrand(obj) {
        _id = $(obj).attr("_id");
        var _name = $(obj).attr("_name");
        $(obj).addClass("red").siblings().removeClass("red");
        //var _choosehtml = "<span class='proIcon'></span>您当前选择的是：";
        //$(".threeSelectFinsh").html(_choosehtml + "<span>" + _topname + " > <span>" + _secondname + " > <span>" + _thirdname + "</span>");
        //_chooseclassid = "|" + _topcid + "|" + _secondcid + "|" + _thirdcid + "|";
       
        _brandid = _id;
    }
    $(function () {
          if($(".threeSelect .list").length > 3 ){
          	$(".threeSelect").css({"width":"1500px"});
          }else{
          	$(".threeSelect").css({"width":"100%"});
          }
    })
</script>
</asp:Content>
