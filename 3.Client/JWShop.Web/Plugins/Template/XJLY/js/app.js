$(function () {
	if(!$("#nav_main .listshow").is(":visible")){ 
		var h=$("#nav_main .listshow").height();
		$("#nav_main .navlist_content").height(h);
		$("#nav_main .wholeSort").hover(function(){
			$(this).find("span").css("cursor","pointer").siblings(".listshow").stop().slideDown(500);
		},function(){
			$(this).find(".listshow").stop().slideUp(300);
		});
	}
	$("#nav_main .wholeSort .item").each(function(){
		var n=$(this).find(".navlist_content").find("a").length;
		if(!n){
			$(this).find(".navlist_content").remove();
		}
	});
	$(".toolbar_tab").hover(function(){
		$(this).find("em").animate({"left":"-70px"},300)
	},function(){
		$(this).find("em").animate({"left":"35px"},300)
	});
	
	$(".checkout .tt .qmark-tip").hover(function(){
		$(this).siblings(".tip").show();
	},function(){
		$(this).siblings(".tip").hide();
	});
    //顶部购物车最新加入商品显示
    $(".carbox").hover(function(){
        $(this).addClass("carhover");
    },function(){
        $(this).removeClass("carhover");
    });

	
	banner();
	index();
	showproduct(); // 详细页图片
	hotlist();    //热销产品图片切换
	showTit(); //详细页底部热卖
	tools();
	psort();  //列表分类检索
	elevator();
    //顶部购物车下拉框加载（最新4件）
	LoadNewCartProducts();
});

// banner焦点图
function banner() {
		if (!$("#banner_main").length || $("#banner_main li").length <= 1) {	return false; }
		$("#banner_main").find("li:gt(0)").hide();
	var _this = $("#banner_main"),
		me = $("#banner_main ul"),
		tip = $("#banner_main .tip"),
		t, interval = 10000,
		speed = 1000,
		speed2 = 700,
		n = 0,
		N = me.children("li").length;
	if ($("#banner_main .tip").length) {
		var htmlTip = "";
		for (var i = 0; i < N; i++) {
			if (i == 0) {
				htmlTip += "<span class='cur'>"+(i+1)+"</span>";
			} else {
				htmlTip += "<span>"+(i+1)+"</span>";
			}
		}
		tip.html(htmlTip);
	}
	var func = function() {
		if (n >= N - 1) {
			n = 0;
		} else {
			n++;
		}
		me.children("li").eq(n).css({
			"z-index": 2
		}).stop().fadeIn(speed).siblings("li").css({
			"z-index": 1
		}).stop().fadeOut(speed2);
		if ($("#banner_main .tip").length) {
			tip.children("span").eq(n).addClass("cur").siblings("span").removeClass("cur");
		}
	}
	tip.children("span").click(function() {
		clearInterval(t);
		n = $(this).index() - 1;
		func();
		t = setInterval(func, interval);
	})
	t = setInterval(func, interval);
}
function index(){
	var myDate = new Date();
	var hours=myDate.getHours();  //获取当前小时数(0-23)
	var min=myDate.getMinutes();     //获取当前分钟数(0-59)
	hours=parseInt(hours);
	min=parseInt(min/10);
	var curdeg=(360/12)*hours;
	var curmin=(30/6)*min;
	$(function(){
		$("#jdClockHours").css("transform", "rotate("+(curdeg+curmin)+"deg)");
	});
	$(".indexTit .tab li:first-child").each(function(){
		var txt=$(this).find("a").text().replace(/\s/g,"");
		txt=txt.length;
		if(txt==2){
			$(this).css("width","58px");
		}
		if(txt==3){
			$(this).css("width","70px");
		}
		if(txt==4){
			$(this).css("width","82px");
		}
		if(txt==5){
			$(this).css("width","94px");
		}
		$(this).addClass("tab-selected");
	});
	$(".indexTit .tab li").hover(function(){
		var txt=$(this).find("a").text().replace(/\s/g,"");
		txt=txt.length;
		if(txt==2){
			$(this).css("width","58px");
		}
		if(txt==3){
			$(this).css("width","70px");
		}
		if(txt==4){
			$(this).css("width","82px");
		}
		if(txt==5){
			$(this).css("width","94px");
		}
		if(txt==6){
			$(this).css("width","106px");
		}
		$(this).addClass("tab-selected").siblings().removeClass(" tab-selected");
	},function(){})
	
	$(".index .indexImglist").each(function(){
		var _this=$(this);
		indexImglist(_this);
	});
}
function indexImglist(_this){
	if ( _this.find("li").length <= 1) {	return false; }
	var a = _this, b = a.children(".list"), c = b.children("li"), d = a.children(".tip"), length = c.length, width = a.width(), i = 0, interval = 6000, speed = 600, t;
	c.width(width);
	b.width(width*(length+1)).append(c.eq(0).clone()).css({"left": 0});
	for(var j=0; j<length; j++){
		if(j == 0){
			d.append("<span class='cur'></span>");
		}else{
			d.append("<span></span>");
		}
	}
	function fun(){
		i ++;
		b.stop().animate({"left": -i*width}, speed, function(){
			if(i >= length){
				i = 0;
				b.css({"left": 0});
			}
			tip();
		});
	}
	function tip(){
		d.children("span").eq(i).addClass("cur").siblings().removeClass("cur");
	}
	d.children("span").bind("click", function(){
		i = $(this).index() - 1;
		fun();
	});
	a.hover(
		function(){ clearInterval(t); },
		function(){ t = setInterval(fun, interval); }
	);
	t = setInterval(fun, interval);
}
function elevator(){
	if($("#elevator").length){ 
		var $nava=$("#elevator .fixul li");
		var arr = new Array();
		$(".index .indexWrap").each(function(){
			var t=$(this).offset().top;
			arr.push(t);
		});
		$(window).scroll(function() {
			var a = $(document).scrollTop();
			for(var i=1 in arr) {
				if(a >= arr[0] - 200) {
					$(".elevator").show();
				}else {
					$(".elevator").hide();
					$nava.eq(0).removeClass("current");
					$(".indexWrap").eq(0).removeClass("current");
				}
				if( a >= arr[i-1]-200 && a < arr[i]-200){
					$nava.eq(i-1).addClass("current").siblings().removeClass("current");
					$(".indexWrap").eq(i-1).addClass("current").siblings(".indexWrap").removeClass("current");
				}else if(  a >= arr[arr.length-1]-200 ){
					$nava.eq(arr.length-1).addClass("current").siblings().removeClass("current");
					$(".indexWrap").eq(arr.length-1).addClass("current").siblings(".indexWrap").removeClass("current");
				}
			}
		});
		$nava.click(function(){
			var index=$(this).index();
			$("body, html").stop().animate({"scrollTop":arr[index]},600);
			
		});
	}
}
function showTit(){
	var a=$("#pageProduct .detail");
	if(!a.length){ return false;}
	var tab1=0,tab2=0,tab3=0;
	var arr = new Array();
	var tab=a.offset().top;
	a.find(".detaila1con .a1").click(function(){
		tab1=a.find("#detail_infor_1").offset().top;
        tab2=a.find("#detail_infor_2").offset().top;
        tab3=a.find("#detail_infor_3").offset().top;
        arr=[tab1,tab2,tab3];
		var index=$(this).index();
	    $("body, html").stop().animate({"scrollTop":arr[index]},1);
	    $(this).addClass("cur").siblings().removeClass("cur");
	});
	$(window).scroll(function(){
        var h=$(document).scrollTop();
        tab1=a.find("#detail_infor_1").offset().top;
        tab2=a.find("#detail_infor_2").offset().top;
        tab3=a.find("#detail_infor_3").offset().top;
        arr=[tab1,tab2,tab3];
        //console.log(arr[1])
		  if( h >= tab){ 
		   	  a.find(".tabcon").addClass("hover");
		  }else{
		  	  a.find(".tabcon").removeClass("hover");
		  }
		  if( h>=arr[0] && h<arr[1] ){   
		   	  a.find(".tabcon .a1").eq(0).addClass("cur").siblings(".a1").removeClass("cur");
		  }
		  if( h>=arr[1] && h<arr[2]){
		   	   a.find(".tabcon .a1").eq(1).addClass("cur").siblings(".a1").removeClass("cur");
		  }
		  if (h >= arr[2]) {
		      a.find(".tabcon .a1").eq(2).addClass("cur").siblings(".a1").removeClass("cur");
		  }
	});
	var b=$("#pageProduct #detail_infor_2");
	b.find(".eval_tit li").click(function(){
		var index=$(this).index();
		$(this).addClass("cur").siblings().removeClass("cur");
	});
}
function psort(){
	if(!$("#pageProduct .proSort").length){ return false;}
	var $sort=$("#pageProduct .proSort");
	$sort.find(".s_brand .sl_list li:gt(13)").hide();
	$sort.find(".pSelect").each(function(){
		if($(this).find("li").length<8){
		    $(this).find(".sl_ext .sl_extMore").hide();
		}
	});
	//多选
	$sort.find(".sl_extMultiple").click(function(){
		$(this).parents(".pSelect").addClass("multiple");
		$(this).parents(".pSelect").find(".sl_list li:gt(13)").show();
		$("#pageProduct .proSort .pSelect .sureClose").show();
	});
	$("#pageProduct .proSort .pSelect .sureClose a.close").click(function(){
		$(this).parents(".pSelect").removeClass("multiple");
		$(this).parents(".pSelect").find(".sl_list li:gt(13)").hide();
		$(this).parents(".pSelect").find(".sl_list li").removeClass("selected");
		$(this).parents(".pSelect").find(".sl_selected ul").empty();
		$("#pageProduct .proSort .pSelect .sureClose").hide();
		$(this).parents(".pSelect").find(".sl_selected").hide();
		$("#pageProduct .proSort .pSelect .sureClose a.sure").hide();
	});
	
	$sort.find(".sl_list li").click(function (e) {
	    $("#pageProduct .proSort .pSelect .sureClose a.sure").css('display', 'inline-block');
	  var _seted=$(this).parents(".sl_list").siblings(".sl_selected");
		if($(this).parents(".pSelect").hasClass("multiple")){
			if($(this).hasClass("selected")){
				$(this).removeClass("selected");
				var txt=$(this).text();
				_seted.find("li:contains("+txt+")").remove();
				if(_seted.find("li").length<1){_seted.hide(); }
				_seted.find("ul").append(clone);
				//selected();
				e.preventDefault();
				
			}else{
				if(_seted.find("li").length>4){  alert("已选条件不能大于5个"); return false;}
				$(this).addClass("selected");
				var clone=$(this).clone();
				$(this).parents(".pSelect").find(".sl_selected").show();
				$(this).parents(".pSelect").find(".sl_selected ul").append(clone);
				//selected();
				//return false;
				e.preventDefault();
			}
			selected();
		}
	});
	//更多
	$sort.find(".sl_extMore").click(function(){
		if($(this).parents(".pSelect").find("li").length>8){
			if($(this).hasClass("addclick")){
				$(this).parents(".pSelect").removeClass("addList");
				$(this).parents(".pSelect").find(".sl_list li:gt(13)").hide();
				$(this).html("更多<s class='icon'></s>");
				$(this).removeClass("addclick");
			}else{
				$(this).addClass("addclick");
				$(this).parents(".pSelect").addClass("addList");
				$(this).parents(".pSelect").find(".sl_list li:gt(13)").show();
				$(this).html("收起<s class='icon'></s>");
			}
		}
		
	});
}
function selected(){
	var _ul=$("#pageProduct .proSort .sl_selected ul");
	_ul.find("li").bind("click",function(e){
		var txt=$(this).text();
		$(this).parents(".sl_selected").siblings(".sl_list").find("li:contains("+ txt +")").removeClass("selected");
		$(this).remove(); 
		if (_ul.find("li").length < 1) {
		    $("#pageProduct .proSort .sl_selected").hide();
		    $("#pageProduct .proSort .pSelect .sureClose a.sure").hide();
        }
		e.preventDefault();
	});
}
function hotlist(){
	if(!$("#hotlist").length){ return false;}
	var time=5000, tim=800,speed = 1000,speed2 = 1100,n = 0;
    var $simg=$("#hotlist ul");
  	var len = $("#hotlist ul").find("li").length;
  	if($("#hotlist").parent().hasClass("myBLH_2_left_2")){
  		len=Math.ceil(len/4);
  	}else{
  		len=Math.ceil(len/6);
  	}
    $("#hotlist .num").find(".z").text(len);
    var xwid=$("#hotlist .conlist").width();
	var func = function(){
		if(n < len-1){
			n++;
		}else{
			n = 0;
		}
        $simg.stop().animate({"margin-left":-n*xwid}, speed2);
        $("#hotlist .num").find(".dq").text(n+1);
	}
	var func2 = function(){
		if(n > 0){
			n--;
           $simg.stop().animate({"margin-left":-n*xwid}, speed2);
           $("#hotlist .num").find(".dq").text(n+1);
		}else{
            $simg.stop().animate({"margin-left":-(len-1)*xwid}, speed2);
			n = len-1;
			$("#hotlist .num").find(".dq").text(n+1);
           
		}
	}
	// 绑定按钮事件
	$(".hotlist .btnleft").click(function(){
		func2(); 
	});
	$(".hotlist .btnright").click(function(){
		func();
	});
}
function showproduct(){
	if(!$("#pageProduct .photoimg").length){ return false;}
	$("#pageProduct .photoimg .listcon").find("a").eq(0).addClass("hover");
	var time=5000, tim=800,speed = 1000,speed2 = 700,n = 0;
    var $simg=$("#pageProduct .photoimg .listcon");
  	var len = $("#pageProduct .photoimg .listcon").find("a").length;
    var len=Math.ceil(len/5);
    var xwid=$("#pageProduct .photoimg .list").width()+7;
	var func = function(index){
		if(n < len-1){
			n++;
		}else{
			n = 0;
		}
        $simg.stop().animate({"margin-left":-n*xwid}, speed2);
	}
	var func2 = function(){
		if(n > 0){
			n--;
           $simg.stop().animate({"margin-left":-n*xwid}, speed2);
		}else{
            $simg.stop().animate({"margin-left":-(len-1)*xwid}, speed2);
			n = len-1;
           
		}
	}
    var func3 = function(index){
        $simg.stop().animate({"margin-left":-index*xwid}, speed2);
        n=index;
	}
	// 绑定按钮事件
	$("#pageProduct .btnleft").click(function(){
		func2();
	});
	$("#pageProduct .btnright").click(function(){
		func();
	});
	$simg.find("img").on("mouseover",function(){
		$(this).parent().addClass("hover").siblings().removeClass("hover");
		$("#bigImg").attr("src", $(this).attr("bigimg"));
		$("#bigImg").attr("jqimg",$(this).attr("originalimg"));
	})  
	$(".jqzoom").jqueryzoom({
        xzoom: 350, //放大图的宽度(默认是 200)
        yzoom: 348, //放大图的高度(默认是 200)
        offset: 10, //离原图的距离(默认是 10)
        position: "right", //放大图的定位(默认是 "right")
        preload: 1
    });
}
function tools() {
	$("#addFavo").click(function() {
		var fm = $("title").html();
		AddFavorite(fm, location.href, '');
	});
	$("a").focus(function() {
		$(this).blur();
	});
	$("#helpList > li").each(function(){
		if(!$(this).find("ul").children("li").length){
			$(this).find("ul").remove();
			$(this).find("s").hide();
		}
		if($(this).find("ul").is(":visible")){
			$(this).find("s").addClass("hasadd");
		}else{
		}
	});
	$("#helpList > li").click(function(){
		if($(this).find("ul").is(":visible")){
			$(this).children("ul").hide();
			$(this).find("s").removeClass("hasadd");
		}else{
			$(this).children("ul").show();
			$(this).find("s").addClass("hasadd");
		}
	});
	$("#myOrder .myBLH_2_left_1 .item:lt(2)").show();
	
	$("#goTop").click(function(){
		$("body, html").stop().animate({"scrollTop":0});
    });

    $(".input_check_box label span").click(function () {
        $(".agreement,.layout").show();

//        $(".agreement").css("show", "hidden");
    });
	$(".layout,.agreement .close,.agreement .agreen_btn").click(function(){
		$(".pictureListBig,.agreement,.layout").hide();
	});
	$("#goTop,.tab_top").click(function(){
		$("body, html").stop().animate({"scrollTop":0});
    });
	$(".returnTop").click(function() {
		$("body, html").stop().animate({
			"scrollTop": 0
		});
	});
}
function checkForm(){
    if($("#formPost").length){ 
       $("#formPost tr.code input").val("");
    }
	$("#formPost input[type='submit']").click(function(){
		
		var bool = true;
		$("#formPost .required").each(function(){
			var a, b, c;  
			$(this).find(".tip").html("");  
			if($(this).find("input").length){
				a = $(this).find("input").val().replace(/\s+/gm,' ');
				b = $(this).find("input").attr("model");
				c = $(this).find("label").text();
				c = c.substr(0, c.length-1);
			}else{
				a = $(this).find("textarea").val().replace(/\s+/gm,' ');
				b = "text"; 
				c = $(this).find("label").text();
				c = c.substr(0, c.length-1);
			}
			if(a == "" || a == " "){
			    $(this).find(".tip").html("<span class='err'>"+ c +" 不能为空!</span>");
				bool = false;
			}else{
				
				switch (b){
					case "name": 
					if(!a.match(/^[A-Za-z]+$/) && !a.match(/^[\u4e00-\u9fa5]{0,}$/)){
						$(this).find(".tip").html("<span class='err'>只能是汉字或英文</span>");
						bool = false;
					}else{
						$(this).find(".tip").html("");
					}
					break;
					case "phone": 
					if(!a.match(/^[0-9]*$/)){
						$(this).find(".tip").html("<span class='err'>只能是纯数字</span>");
						bool = false;
					}else{
						$(this).find(".tip").html("");
					}
					break;
					case "email":  
					if(!a.match(/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/)){
						$(this).find(".tip").html("<span class='err'>请输入正确的邮箱格式</span>");
						bool = false;
					}else{
						$(this).find(".tip").html("");
					}
					break;
				}
			}
        });
		return bool;
	});
}
function AddFavorite(title, url) {
	try {
		window.external.addFavorite(url, title);
	} catch (e) {
		try {
			window.sidebar.addPanel(title, url, "");
		} catch (e) {
			alert("抱歉，您所使用的浏览器无法完成此操作。\n\n加入收藏失败，请使用Ctrl+D进行添加");
		}
	}
}
//顶部banner关闭
$("#topbanner-close").click(function () {
    $("#top-banner").animate({"opacity":0},200,function(){
        $("#top-banner").css({"display":"none"});
    });
})

//顶部购物车下拉框加载（最新4件）
function LoadNewCartProducts() {
    $("#header_main .list").html("<img style='margin:0 auto; width:24px; display:block;' src='/Admin/Images/loadcart.gif' />");
    $.ajax({
        url: "/ajax.html?Action=ReadCart",
        type: "GET",
        dataType: "JSON",
        success: function (result) {
            if (result.flag == "ok") {
                if (result.msg != "")
                {
                    $("#header_main .list").html(result.msg);
                    //给删除按钮绑定事件--顶部购物车最新商品“删除”
                    $("#header_main .delet").click(function () {
                        var _cartId = $(this).attr("_cid");
                        var _curPrice = $(this).attr("_cprice")
                        var _deleteCount = $(this).attr("_dcount")
                        if (_deleteCount == "") _deleteCount = 0;
                        $($(this).parent()).fadeOut(500, function () {
                            $(this).remove();
                        });
                        $.ajax({
                            url: "/CartAjax.html?Action=DeleteCart&StrCartID=" + _cartId + "&OldCount=" + _deleteCount,
                            type: "GET",
                            //dataType: "JSON",
                            success: function (result) {
                                if (result == "ok") {
                                    LoadNewCartProducts();
                                    if ($("#ProductBuyCount").length > 0 && parseInt($("#ProductBuyCount").text()) >= parseInt(_deleteCount)) {
                                        $("#ProductBuyCount").text(parseInt($("#ProductBuyCount").text()) - parseInt(_deleteCount));
                                        if ($("#rightProductBuyCount").length > 0) $("#rightProductBuyCount").text($("#ProductBuyCount").text());
                                    }
                                  
                                }
                                else {
                                    alertMessage("系统忙，请稍后重试");
                                }
                            }
                               , error: function () {
                                   alertMessage("系统忙，请稍后重试");
                               }
                        })
                    })
                }
                else
                { $("#header_main .list").html("购物车空空,赶紧选购商品吧"); }              
            }
            else {
                //$("#header_main .list").html(result.msg);
                $("#header_main .list").html("系统忙，请稍后重试");
            }
        }
                   , error: function () {
                       $("#header_main .list").html("系统忙，请稍后重试");
                   }
    })
}