var redbag = {
    func: function () {
        var t,
			_interval = 50,
			_u = 0;

        function create() {
            var _left = Math.random() * 100,
				_scale = Math.random() * 1,
				_time = Math.random() * 3000 + 1000;

            _u++;

            if (_u < 50000000) {
                $('#redbag').append('<span style="left:' + _left + '%; transform:scale(' + _scale + ',' + _scale + '); -webkit-animation:redown ' + _time + 'ms linear both;"></span>');
            }
        }

        $('body').bind({
            'click': function () {
                clearInterval(t);
                setTimeout(function () {
                    $('#redbag').fadeOut();
                }, 2500);
            }
        })

        t = setInterval(function () {
            create();
        }, _interval);
    }
}

function application() {
	var _self = this;
	
	// 防拖拽
	$('body').bind({
        'touchmove': function(e){
			//e.stopPropagation();
			//e.preventDefault();
		}
    });
    // 下红包雨
	redbag.func();

	// 点击抽奖
	$('#start').bind('click', function(){
		_self.verification();
	});
	
	// 表单填写
	$('#form .submit').bind({
		'click': function(){
			var $form = $('#form'),
				$name = $form.find('[name="realName"]'),
				$company = $form.find('[name="company"]'),
				$phone = $form.find('[name="cellPhone"]');
				
			var reg = {
				name: /([\u4e00-\u9fa5]{2,4})/,
				phone: /((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/,
				email: /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/
			}
			
			if($name.val() == ''){
				alert($name.attr('null'));
				$name.focus();
				return false;
			}else if(!$name.val().match(reg.name)){
				alert($name.attr('error'));
				$name.focus();
				return false;
			}else if($company.val() == ''){
				alert($company.attr('null'));
				$company.focus();
				return false;
			}else if($phone.val() == ''){
				alert($phone.attr('null'));
				$phone.focus();
				return false;
			} else if (!Validate.isMobile($phone.val())) {
				alert($phone.attr('error'));
				$phone.focus();
				return false;
			}
			
			// 执行抽奖
			$form.css({'visibility': 'hidden'});
			_self.game();
		}
	})
	
	// 分享开关
	$("#share").bind({
		"touchstart":function(e){
			e.preventDefault();
		},
		"touchmove":function(e){
			e.preventDefault();
		},
		"touchend":function(){
			$(this).css({"visibility":"hidden"});
		}
	});
	
	// 音乐开关
	$("#audio").bind("click", function(){
		 var media = document.getElementById("media");
		if($(this).hasClass("play")){
			$(this).removeClass("play").addClass("pause");
			media.pause();
		}else{
			$(this).removeClass("pause").addClass("play");
			media.play();
		}
	});
	
	this.init();
	//this.loadWinning();
}
application.prototype = {
	init: function(){
		var a = $('body').width(),
			b = $('body').height(),
			c = a / 320,
			d = b / 486,
			e = 1;
		if(d > c){
			e = c;
		}else{
			e = d;
		}
		$('#MobileViewport').attr({'content': 'width=320, initial-scale=' + e + ', maximum-scale=' + e + ', user-scalable=no'});
		$('body').css({'visibility': 'visible'});
	},
	verification: function(){
		var _self = this;
	    if ($("#realName").val().length > 1) {
	        _self.game();
	    } else {
	        console.log("fill");
	        $('#form').css({ 'visibility': 'visible' }).children('.main').show();;
	    }
	},
	game: function () {
		var _self = this;
				
		var flag = false,     // 减速标记 true 为开始减速
			mode = 1,         // 转盘模式
			size = 8,         // 礼品数量
			t,                // 计时器
			gift = {},        // 奖品
			speed = 0,        // 转动速度
			index = 0,        // 当前数组指针  0.1.2.3.4.5 ....
			cycle = 0,        // 当前循环数组的全程次数
			quick = 0,        // 当前跑过的格子
			endindex = 0,     // 进入减速时的index标记
			endIndex = 0;     // 从后台取得的奖品等级标示
		
		var units = 360/size;
		
		function _begin() {
		    console.log("begin");
			//$("#start").unbind("click");
			flag = false;
			speed = 200;
			endindex = Math.floor(Math.random() * size);
			var realName = $("#realName").val();
			var company = $("#company").val();
			var cellPhone = $("#cellPhone").val();

			var reg = new RegExp("(^|&)id=([^&]*)(&|$)", "i");
			var r = window.location.search.substr(1).match(reg);
			if (r == null) return;
			$.ajax({
			    url: "/mobile/ajax.aspx?Action=GetPrize",
			    type: 'post',
			    dataType: 'json',
			    timeout: 10000,
			    data: { "activityid": r[2], realName: realName, company: company, cellPhone: cellPhone },
			    async: false,
				cache: false,
				success: function (data) {
				    var dataNum = parseInt(data.No);
				    //alert(dataNum);
				    if (dataNum <= 0) {
				        if (dataNum == 0) {
				            _self.game();
				        } else {
				            if (dataNum == -3) {
				                $("#dateline").css({ "visibility": "visible" }).children(".main").show();
				            }
				            else if (dataNum == -2) {
				                $("#havePrized").css({ "visibility": "visible" }).children(".main").show();
				            }
				            else if (dataNum == -4) {
				                $("#lotteryfinished").css({ "visibility": "visible" }).children(".main").show();
				            }
				            else {
				                //alert(dataNum);
				                $("#failure").css({ "visibility": "visible" }).children(".main").show();				                
				            }
				            $('#start').bind('click', function () {
				                _self.game();
				            })
				        }
				    } else {
				        /*各奖项在转盘中的对应位置*/
				        compareid = parseInt(data.No);
				        switch (compareid) {
				            case 1:
				                endIndex = 8;
				                break;
				            case 2:
				                endIndex = 1;
				                break;
				            case 3:
				                endIndex = 2;
				                break;
				            case 4:
				                endIndex = 3;
				                break;
				            case 5:
				                endIndex = 5;
				                break;
				            case 6:
				                endIndex = 4;
				                break;
				            case 7:
				                endIndex = 6;
				                break;
				            case 8:
				                endIndex = 7;
				                break;
				            default:
				                endIndex = 7;
				                break;
				        }
				        /*系统遗留问题，不通用 start*/
				        gift = data;
				        t = setInterval(_running, speed);
				        // $('#disc .wheel').css({'animation': 'rotate2 8s ease'})
				    }

				},
				error: function () {
				    console.log('警告：获取本轮奖品数据失败~');
				}
			});
		}
		
		function _running(){		
			// 开始起步
			if(quick < 6 && flag == false){
				clearInterval(t);
				speed = 200;
				t = setInterval(_running, speed);
			}
			// 开始加速
			if(quick == 6 && flag == false){
				clearInterval(t);
				speed = 100;
				t = setInterval(_running, speed);
			}
			// 开始减速
			if(cycle == 6 && index == endindex){
				clearInterval(t);
				speed = 200;
				t = setInterval(_running, speed);
				flag = true;
			}
			// 完全停止
			if(flag == true && index == endIndex-1){
				clearInterval(t);
				flag = false;
				speed = 0;
				cycle = 0; 
				quick = 0;
				
				_end();
			}
			// 正常循环
			if(mode == 1){
				// 大转盘逆时针转动
				$("#disc .wheel").css({"-webkit-transform": "rotate("+ (135-index*units) +"deg)", "-ms-transform": "rotate("+ (135-index*units) +"deg)", "-moz-transform": "rotate("+ (135-index*units) +"deg)", "transform": "rotate("+ (135-index*units) +"deg)", });
			}else if(mode == 2){
				// 指针转动
				$("#start span").css({"-webkit-transform": "rotate("+ (index*units) +"deg)", "-ms-transform": "rotate("+ (index*units) +"deg)", "-moz-transform": "rotate("+ (index*units) +"deg)", "transform": "rotate("+ (index*units) +"deg)", });
			}
			
			if(index < size - 1){
				index ++;
			}else{
				index = 0;
				cycle ++;
			}
			// 自叠加
			quick ++;
		}
		
		function _end(){
			var html = '';
			html += '<div class="gift">';
			html += '<img src="/Plugins/Template/XJLY/Mobile/static/lotteryactivity/goods/' + gift.No + '.png" />';
			html += '</div>';
			html += '<h2>' + gift.Prizelevel + '</h2>';
			html += '<h2>' + gift.PrizeTitle + '</h2>';
			if (gift.PrizeName != '') {
			    html += '<p style="text-align:center;">' + gift.PrizeName + '</p>';
			}
			html += '<br/>';			
			$("#success").html(html);
			
			setTimeout(function(){
				$("#layout").css({"visibility":"visible"}).children(".main").show();
			}, 1000);
			
			//$('#start').bind('click', function(){
			//	_self.game();
			//})
		}
		
		// 开始获取数据
		_begin();
	},
	loadWinning: function(){
		$.ajax({
			url: "./api/list.php",
			type: "POST",
			dataType: "json",
			success: function(data){
				var _html = '';
				if(data.type == 0){
					
				}else if(data.type == 1){
					$.each(data.list, function(i,e){
						_html += '<dd><span class="tel">'+ e.tel +'</span><span class="prize">'+ e.prize +'</span></dd>';
					})
					showList(_html);
				}
			},
			error: function(){
				console.log('警告：中奖名单获取失败~');
			}
		})
		
		function showList(html){
			$("#winning .list").html(html);
		}
	},
	showLayout: function(id){
		$('#' + id).css({'visibility': 'visible'}).children('.main').show();
	},
	hideLayout: function(id){
		$('#' + id).css({'visibility': 'hidden'}).children('.main').hide();
	}
}
var app = new application();