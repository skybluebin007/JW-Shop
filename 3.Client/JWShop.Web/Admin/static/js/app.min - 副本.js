var overall = function(){
	var $header = $("#header"),
		$wrapper = $("#wrapper"),
		$container = $("#container"),
		$navbar = $("#navbar"),
		$menubar = $("#menubar"),
		$menuPointer = $("#menu-pointer"),
		$navPointer = $("#nav-pointer"),
		$account = $("#account");
	$navPointer.bind("click", function(){
		if($navbar.hasClass("navbar-mini")){
			$navbar.removeClass("navbar-mini");
			$wrapper.removeClass("wrapper-max");
		}else{
			$navbar.addClass("navbar-mini");
			$wrapper.addClass("wrapper-max");
		}
	});
	$menuPointer.bind("click", function(){
		if($menubar.hasClass("menubar-mini")){
			$menubar.removeClass("menubar-mini");
			$container.removeClass("container-max");
		}else{
			$menubar.addClass("menubar-mini");
			$container.addClass("container-max");
		}
	});
	$account.bind("click", function(){
		if($(this).hasClass("current")){
			$(this).removeClass("current");
		}else{
			$(this).addClass("current");
		}
	})
	this.ini();
}
overall.prototype = {
	ini: function(){
		// 超级链接虚线选择
		$("a").focus(function(){ $(this).blur(); });
		// 多子栏目展示
		$(".classify-column .sub").bind("click", function(){
			if($(this).hasClass("sub-show")){
				$(this).attr({"class":"sub sub-hide"});
				var $bind = $(this).parents("tr").attr("bind");
				$("tr[ig-bind='"+ $bind +"']").removeAttr("style");
			}else if($(this).hasClass("sub-hide")){
				$(this).attr({"class":"sub sub-show"});
				var $bind = $(this).parents("tr").attr("bind");
				$("tr[ig-bind='"+ $bind +"']").attr({"style":"display:none;"});
			}
		});
		$("#sub-showall").bind("click", function(){
			$("tr[ig-bind").removeAttr("style");
			$(".sub-show").attr({"class":"sub sub-hide"});
		})
		$("#sub-hideall").bind("click", function(){
			$("tr[ig-bind").attr({"style":"display:none;"});
			$(".sub-hide").attr({"class":"sub sub-show"});
		})
		// 金额滚动加载
		if ($("#moneyscroller").length) {		    
			var $moneyscroller = $("#moneyscroller");
			function moneypush(money){
				var _html = "", _html2 = "", h = 68,
					arr = money.toString().split("").reverse(), arr2;
				for(var i=0; i<arr.length; i++){
					_html += arr[i];
					if((i+1)%3 == 0 && (i+1) != arr.length){
						_html += ",";
					}
				}
				arr2 = _html.split("").reverse();
				for(index in arr2){
					if(arr2[index] == ","){
						_html2 += "<em></em>";
					}else{
						_html2 += "<div class='row' sp='"+ h*arr2[index] +"'><div class='con'><dl style='top:0px'><dd>0</dd><dd>1</dd><dd>2</dd><dd>3</dd><dd>4</dd><dd>5</dd><dd>6</dd><dd>7</dd><dd>8</dd><dd>9</dd></dl></div></div>";
					}
				}
				$moneyscroller.html(_html2);
				setTimeout(function(){
					moneyscroller(money.toString().length);
				}, 1000)
			}
			function moneyscroller(p){
				$moneyscroller.children(".row").each(function(index, element){
					$(this).addClass("d").find("dl").css({"top": -$(this).attr("sp"), "transition":"all "+ (2000-index*100) +"ms ease", "":"", "":"", "":""});
				});
			}
			//$.get("/Admin/Ajax.aspx", { action: "GetPayCount", orderType: 1}, function (data) {
			//    moneypush(data);
		    //})
            /*******后台首页统计支付金额******/
			$.ajax({
			    url: "/Admin/Ajax.aspx",
			    dataType: "JSON",
			    data: { action: "GetPayCount", orderType: 5 },
			    type: "GET",
			    cache: false,
			    success: function (data) {
			        moneypush(data.money);
			    }
			})
			$("#filter-date span").bind("click", function () {
			    $(this).addClass("cur").siblings().removeClass("cur");
			    var dataType = parseInt($(this).attr("did"));
				$.ajax({
				    url: "/Admin/Ajax.aspx",
				    dataType: "JSON",
				    data: { action: "GetPayCount", orderType: dataType},
				    type: "GET",
				    cache: false,
				    success: function (data) {
				        moneypush(data.money);
				    }
				})
			})
		    /*******后台首页统计支付金额******/
		}
		this.plugin();
		this.moniud();
		this.relink();
	},
	plugin: function(){
		// 颜色色谱
		if($("#colpick").length){
			var $colpick = $("#colpick"), $colpickVal = $("#colpick input");
			$colpick.colpick({
				layout: "hex",
				submit: 0,
				onChange: function(hsb,hex,rgb,el,bySetColor){
					$(el).css("background-color","#" + hex);
					if(!bySetColor){
						$colpickVal.val("#" + hex);
					}
				}
			});
		}
		// 图表主题
		Highcharts.createElement('link', {
		    href: '/Admin/static/css/googleapis.css',
			rel: 'stylesheet',
			type: 'text/css'
		}, null, document.getElementsByTagName('head')[0]);
		Highcharts.theme = {
			colors: ["#7cb5ec", "#f7a35c", "#90ee7e", "#7798BF", "#aaeeee", "#ff0066", "#eeaaee",
				"#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
			chart: {
				backgroundColor: null,
				style: {
					fontFamily: "Dosis, sans-serif"
				}
			},
			title: {
				style: {
					fontSize: '16px',
					fontWeight: 'bold',
					textTransform: 'uppercase'
				}
			},
			tooltip: {
				borderWidth: 0,
				backgroundColor: 'rgba(219,219,216,0.8)',
				shadow: false
			},
			legend: {
				itemStyle: {
					fontWeight: 'bold',
					fontSize: '13px'
				}
			},
			xAxis: {
				gridLineWidth: 1,
				labels: {
					style: {
						fontSize: '12px'
					}
				}
			},
			yAxis: {
				minorTickInterval: 'auto',
				title: {
					style: {
						textTransform: 'uppercase'
					}
				},
				labels: {
					style: {
						fontSize: '12px'
					}
				}
			},
			plotOptions: {
				candlestick: {
					lineColor: '#404048'
				}
			},
			background2: '#F0F0EA'
		};
		Highcharts.setOptions(Highcharts.theme);
		// 图标数据
		if($("#chart-order").length){
			$(".index-chart .tab dd").click(function(){
				$(this).addClass("cur").siblings().removeClass("cur");
				var sid = $(this).parent().next().attr("id");
				var tabid = parseInt($(this).attr("did"));
				var dataType = "GetChartData";
				if (tabid > 4) dataType = "GetUserData";
				$.ajax({
				    url: "/Admin/Ajax.aspx",
					data: {
					    action: dataType, dataType: tabid
					},
					dataType: "JSON",
					type: "GET",
					success: function(data){
					    drawChart(data.arr, sid, tabid);
					}
				})
			});
			function drawChart(data, sid, showtype) {
			    var categorylist;
			    if (showtype == 1 || showtype == 5) {
			        categorylist = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23'];
			    } else if (showtype == 2 || showtype == 6) {
			        categorylist = ['1', '2', '3', '4', '5', '6', '7'];
			    } else if (showtype == 3 || showtype == 7) {
			            categorylist= ['1', '', '3', '', '5', '', '7', '', '9', '', '11', '', '13', '', '15', '', '17', '', '19', '', '21', '', '23', '', '25', '', '27', '', '29', ''];
			    } else if (showtype == 4 || showtype == 8) {
			        categorylist = ['1', '2', '3'];
			    }
				$("#" + sid).highcharts({
					title: {
						text: "",
						x: -20 //center
					},
					xAxis: {
						lineWidth: 2,
						lineColor: "#f95348",
						categories: categorylist,
						tickLength: 1
					},
					yAxis: {
						title: "",
						min: 0
					},
					tooltip: {
						valueSuffix: ''
					},
					legend: {
						enabled: 0
					},
					credits: {
						enabled:0
					},
					series: [{
						name: '订单量',
						data: data
					}]
				});
			}

			$.ajax({
			    url: "/Admin/Ajax.aspx",
			    data: {
			        action: "GetChartData", dataType: 4
			    },
			    dataType: "JSON",
			    type: "GET",
			    success: function (data) {
			        drawChart(data.arr, "chart-order", 4);
			    }
			})
			$.ajax({
			    url: "/Admin/Ajax.aspx",
			    data: {
			        action: "GetUserData", dataType: 8
			    },
			    dataType: "JSON",
			    type: "GET",
			    success: function (data) {
			        drawChart(data.arr, "chart-user", 8);
			    }
			})
			// 模拟第一次数据加载
			//drawChart([70, 69, 95, 145, 182, 115, 252, 265, 233, 183, 319, 916, 916, 296, 196, 916, 710, 69, 95, 145, 182, 215, 252, 215, 233, 113, 139, 96, 96, 96, 96], "chart-order",3);
			//drawChart([0, 69, 95, 15, 12, 15, 22, 26, 23, 13, 11, 96, 96, 26, 16, 96, 70, 6, 95, 15, 12, 15, 22, 25, 23, 13, 19, 9, 9, 6, 9], "chart-user",3);
		}
		// other
	},
	moniud: function(){
		// 多选模拟
		$(".ig-checkbox input").change(function(){
			if(this.checked){
				$(this).parent().addClass("checked");
				if($(this).attr("ig-bind") && $("input[ig-bind='"+ $(this).attr("ig-bind") +"']").length == $("input[ig-bind='"+ $(this).attr("ig-bind") +"']:checked").length){
					$(".ig-checkbox .checkall[bind='"+ $(this).attr("ig-bind") +"']").attr({"checked": true}).parent().addClass("checked");
				}
			}else{
				$(this).parent().removeClass("checked");
				if($(this).attr("ig-bind") && $("input[ig-bind='"+ $(this).attr("ig-bind") +"']").length != $("input[ig-bind='"+ $(this).attr("ig-bind") +"']:checked").length){
					$(".ig-checkbox .checkall[bind='"+ $(this).attr("ig-bind") +"']").attr({"checked": false}).parent().removeClass("checked");
				}
			}
		});
		// 全选模拟
		$(".ig-checkbox .checkall").change(function(){
			if(this.checked){
				$(this).parent().addClass("checked");
				$(".ig-checkbox input[ig-bind='"+ $(this).attr("bind") +"']").attr({"checked": true}).parent().addClass("checked");
			}else{
				$(this).parent().removeClass("checked");
				$(".ig-checkbox input[ig-bind='"+ $(this).attr("bind") +"']").attr({"checked": false}).parent().removeClass("checked");
			}
		});
		
		// tab切换
		$("#ig-tab .tab li").bind("click", function(){
			$(this).addClass("cur").siblings().removeClass("cur");
			$("#ig-tab > .main > .row").eq($(this).index()).show().siblings().hide();
		});
	},
	relink: function(){  // 相关数据匹配
		$(".form-relink .addall").bind("click", function(){
			var _all = $(this).parents(".form-relink").children(".all"),
				_select = $(this).parents(".form-relink").children(".select");
			_select.append(_all.html());
			_all.html("");
		})
		$(".form-relink .delall").bind("click", function(){
			var _all = $(this).parents(".form-relink").children(".all"),
				_select = $(this).parents(".form-relink").children(".select");
			_all.append(_select.html());
			_select.html("");
		})
		$(".form-relink .addone").bind("click", function(){
			var _all = $(this).parents(".form-relink").children(".all"),
				_select = $(this).parents(".form-relink").children(".select"),
				_html = "";
			_all.children("option:selected").each(function(index, element){
				_html += "<option>"+ $(this).html() +"</option>";
			});
			_select.append(_html);
			_all.children("option:selected").remove();
		})
		$(".form-relink .delone").bind("click", function(){
			var _all = $(this).parents(".form-relink").children(".all"),
				_select = $(this).parents(".form-relink").children(".select"),
				_html = "";
			_select.children("option:selected").each(function(index, element){
				_html += "<option>"+ $(this).html() +"</option>";
			});
			_all.append(_html);
			_select.children("option:selected").remove();
		})
	}
}
var overaller = new overall();



