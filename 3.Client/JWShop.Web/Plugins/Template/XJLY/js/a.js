$(function(){
	uePub();  // uePub
	ueHome();  // ueHome
	//banner(); // banner
});
// banner()
//function banner(){
//	var me = $("#banner ul"),
//		t,
//		interval = 10000,
//		speed = 1500,
//		speed2 = 1000,
//		n = 0,
//		N = me.children("li").length,
//		htmlTip = "";
//	for(var i=0; i<N; i++){
//		if(i==0){
//			htmlTip += "<span class='cur'>"+ (i+1) +"</span>";
//		}else{
//			htmlTip += "<span>"+ (i+1) +"</span>";
//		}
//	}
//	me.siblings(".tip").html(htmlTip);
//	var func =  function(){
//		if(n >= N-1){
//			n = 0;
//		}else{
//			n ++;
//		}
//		me.children("li").eq(n).css({"z-index":3}).stop().fadeIn(speed).siblings("li").css({"z-index":2}).stop().fadeOut(speed2);
//		me.siblings(".tip").children("span").eq(n).addClass("cur").siblings("span").removeClass("cur");
//	}
//	me.siblings(".tip").children("span").hover(
//		function(){
//			clearInterval(t);
//			n = $(this).index()-1;
//			func();
//			t = setInterval(func, interval);
//		},
//		function(){
//			// code
//		}
//	);
//	t = setInterval(func, interval);
//}
// ueHome
function ueHome(){
	var $d = $("#wrapper"), $c = $("#banner");
	$c.find(".adv img").hover(
		function(){
			$(this).stop().animate({"margin-left":-5}, {easing:"jswing", duration:300});
		},
		function(){
			$(this).stop().animate({"margin-left":0}, {easing:"jswing", duration:300});
		}
	);
	$d.find(".hotPoint .link a").hover(
		function(){
			$(this).children("img").stop().animate({"margin-left":10}, {easing:"jswing", duration:300});
		},
		function(){
			$(this).children("img").stop().animate({"margin-left":0}, {easing:"jswing", duration:300});
		}
	);
	$d.find(".advGroup li").hover(
		function(){
			$(this).stop().animate({"padding-top":0}, {easing:"jswing", duration:300});
		},
		function(){
			$(this).stop().animate({"padding-top":10}, {easing:"jswing", duration:300});
		}
	);
	$(".homePro .item").hover(
		function(){
			$(this).addClass("hover");
		},
		function(){
			$(this).removeClass("hover");
		}
	);
	$(".homePro .view").each(function(){
        var me = $(this),
			t,
			interval = 10000,
			n = 0;
			N = me.find("dd").length,
			w = me.width(),
			htmlTip = "";
		me.children("dl").css({"width":w*(N+1)}).append(me.find("dd").eq(0).clone());
		for(var i=0; i<N; i++){
			if(i==0){
				htmlTip += "<span class='cur'></span>";
			}else{
				htmlTip += "<span></span>";
			}
		}
		me.find(".tip").html(htmlTip);
		var func = function(){
				if(n >= N-1){
					me.children("dl").stop().animate({"margin-left":-w*(n+1)}, {easing:"easeOutExpo",duration:1000, complete:function(){
						me.children("dl").css({"margin-left":0});
					}});
					me.find(".tip span").eq(0).addClass("cur").siblings("span").removeClass("cur");
					n = 0;
				}else{
					n ++;
					me.children("dl").stop().animate({"margin-left":-w*n}, {easing:"easeOutExpo",duration:1000});
					me.find(".tip span").eq(n).addClass("cur").siblings("span").removeClass("cur");
				}
				
			};
		me.children("dl").hover(
			function(){
				clearInterval(t);
			},
			function(){
				t = setInterval(func, interval);
			}
		);
		me.find(".tip span").hover(
			function(){
				clearInterval(t);
				n = $(this).index()-1;
				func();
				t = setInterval(func, interval);
			},
			function(){
				// code
			}
		);
		t = setInterval(func, interval);
    });
}
// uePub
function uePub(){
	$a = $("#topper"), $b = $("#nav"), $c = $("#header"), $d = $("#eTool");
	$a.find(".e").hover(
		function(){
			$(this).addClass("hover");
		},
		function(){
			$(this).removeClass("hover");
		}
	);
	$a.find(".car").hover(
		function(){
			$(this).addClass("carHover");
		},
		function(){
			$(this).removeClass("carHover");
		}
	);
	$a.find(".fav").hover(
		function(){
			$(this).addClass("favHover");
		},
		function(){
			$(this).removeClass("favHover");
		}
	);
	// resetNav 重设导航
	if($("#banner").length){
		$b.find("dt").addClass("hover moon").find('.drop li.item:gt(6)').hide();
	}else{
		$b.find("dt").hover(
			function(){
				$(this).addClass("hover");
			},
			function(){
				$(this).removeClass("hover");
			}
		);
	}
	$b.find(".item").hover(
		function(){
			$(this).addClass("hover");
		},
		function(){
			$(this).removeClass("hover");
		}
	);
	// search
//	var defaultValue = $("#searchTxt").val();
//	$("#searchTxt").focus(function(){
//		$(this).attr({"placeholder":defaultValue}).val("");
//	}).blur(function(){
//		if($(this).val() == "" || $(this).val() == defaultValue){
//			$(this).attr({"placeholder":""}).val(defaultValue);
//		}
//	});
	// goTop
	$("#goTop").click(function(){
		$("body, html").animate({"scrollTop":0}, {easing:"easeOutQuint", duration:1000});
	});
	// eTool关闭二维码
	
	// 惰性加载图片
	loadImgLazy();
	// 省市县三级联动
	worldCity();
}
// 惰性加载
function loadImgLazy(){
	/* 2016.3.29
	$("img.lazyImg").lazyload({
		placeholder : "/jwshop/images/web/blank.gif",
		effect : "fadeIn",
		effect_speed:500,
		threshold : 500
	});
	*/
}
// jAlert 警告框
function jAlert(){
	
}
// jConfirm 确认框
function jConfirm(title, info, callback){
	$("<div class='dialog' id='jConfirm'><div class='tid'><h2>"+ title +"</h2></div><div class='tod'><p class='red'>"+ info +"</p><p class='b'><a href='javascript:;' class='ok'>确定</a><a href='javascript:;' class='no'>取消</a></p></div></div><div class='overLay' id='overLay'></div>").appendTo("body");
	var me = $("#jConfirm"), overLay = $("#overLay");
	me.find(".no").click(function(){
		me.remove();
		overLay.remove();
		callback(false);
	});
	me.find(".ok").click(function(){
		me.remove();
		overLay.remove();
		callback(true);
	});
}
// 加载省市县三级联动
function worldCity(){
	var worldCity = $("#worldCity");
	var func = function(){
		worldCity.children("select").change(function(){
			$(this).nextAll("select").remove();
			if($(this).val() == 0){ return false; }
			var html = "<option value='0'>请选择</option>", num = 0;
			for(index in unlimitClassData){
				if(unlimitClassData[index][2] == $(this).val()){
					num ++;
					html += "<option value='"+ unlimitClassData[index][0] +"'>"+ unlimitClassData[index][1] +"</option>";
				}
			}
			if(num == 0){ return false; }
			$("<select>"+ html +"</select>").appendTo(worldCity);
			$(this).unbind("change");
			func();
		});
	}
	func();
}