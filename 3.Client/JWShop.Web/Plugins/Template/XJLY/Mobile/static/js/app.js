function application(){
	var _self = this;
	
	this.rgexp = {
		'username': /^[\u4e00-\u9fa5a-z0-9_]{3,15}$/,
		'mobile': /^1[3|4|5|8]\d{9}$/,
		'telphone': /((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/,
		'email': /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/,
		'password': /^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{4,10}$/,
		'number': /^[0-9]*[1-9][0-9]*$/,
		'qq': /^\d[1-9]{5,10}$/
		
	}
	this.number = {
		'scrollTop': 0
	}
    
	// 弹出快捷菜单
	$('#showQuiteMenu').bind('click', function(){
		if($('#quiteMenu').is(':hidden')){
			$('#quiteMenu').show();
			$('#nav').css({'top': '1.7rem'});
			$('#listPro').css({'padding-top': '2.3rem'});
			$('#wrapper').css({'padding-top': '1.7rem'});
		}else{
			$('#quiteMenu').hide();
			$('#nav').css({'top': '.8rem'});
			$('#listPro').css({'padding-top': '1.4rem'});
			$('#wrapper').css({'padding-top': '.8rem'});
		}
	})
	
	// 弹出搜索面板
	$('#showSearcher').bind('focus', function(){
		$('#searcher').show();
		$('#keyWord').focus();
	})
	// 关闭搜索面板
	$('#closeSearcher').bind('click', function(){
		$('#searcher').hide();
	})
	// 清除搜索历史
	$('#clearHistory').bind('click', function(){
	    $('#searcherHistory').empty();
	    $.ajax({
	        url: '/Ajax.html?Action=DeleteHistorySearch',
	        type: 'GET',
	        //data: $("form").serialize(),
	        dataType: "JSON",
	        success: function (result) {

	            if (result.flag == "ok") {
	                //alertMessage("设置成功", 500);

	            }
	            else {
	                alertMessage("系统忙，请稍后重试", 500);
	            }
	        }
	    });
	})
	
}
application.prototype = {
    constructor: app,
    ini: function(){
		// 公用
		$('[ig-back]').click(function(){ history.go(-1); })
		$('[ig-top]').click(function(){ $('html, body').animate({'scrollTop': 0}, 300); })
		$('[ig-number] label:first-child').bind('click', function(){
			$txt = $(this).siblings('.txt');
			var num = parseInt($txt.val());
			if(num > 1){
				num --;
			}
			$txt.val(num);
		})
		$('[ig-number] label:last-child').bind('click', function(){
			$txt = $(this).siblings('.txt');
			var num = parseInt($txt.val());
			if(num < 999){
				num ++;
			}
			$txt.val(num);
		})
		$('[ig-number] .txt').bind('keyup', function(){
			if(!$(this).val().match(app.rgexp.number)){
				$(this).val(1);
			}
		})
		$('[ig-radio]').children('*').bind('click', function(){
			$(this).addClass('checked').siblings().removeClass('checked');
		})
		$('[ig-checkbox]').children('*').bind('click', function(){
			if($(this).hasClass('checked')){
				$(this).removeClass('checked');
			}else{
				$(this).addClass('checked');
			}
		});
		$('.gongcheckbox').bind('click', function(){
			if($(this).hasClass('checked')){
				$(this).removeClass('checked');
			}else{
				$(this).addClass('checked');
			}
		})
		$('[ig-addfavo]').bind('click', function(){
			if($(this).hasClass('checked')){
			    $(this).removeClass('checked');			    
				//app.jMsg('已经从收藏列表中删除');
			}else{
				$(this).addClass('checked');
				if ($("#ProductID").length > 0) {
				    collectProduct($("#ProductID").val());
				}
				//app.jMsg('恭喜，已经加入到收藏');
			}
		})
		$('[ig-addcar]').bind('click', function () {
		    var productid = $(this).attr("productid");
		    var productname = $(this).attr("productname");
		    var productstandard = $(this).attr("productstandard");
		    addToCart(productid, productname, productstandard, $(this));			
		})
		$('[ig-addcarnow]').bind('click', function () {
		    var productid = $(this).attr("productid");
		    var productname = $(this).attr("productname");
		    var productstandard = $(this).attr("productstandard");

		    buyNow(productid, productname, productstandard);
		})
		
		$('[ig-star] li').bind('click', function(){
			$(this).siblings().removeClass('checked');
			$(this).addClass('checked').prevAll().addClass('checked');
			var _sid = $(this).attr('sid'),
				_val = $(this).index()+1;
			$(this).parent().siblings('input').val(_val).attr({'name': _sid});
		})
		
		$('body').on('change', '.ug-checkbox input[type="checkbox"]', function () {
			if($(this).prop('checked')){
				$(this).parent().addClass('checked');
			}else{
				$(this).parent().removeClass('checked');
			}
		})
		$('body').on('change', '.ug-radio input[type="radio"]', function () {
			if($(this).prop('checked')){
				$(this).parent().addClass('checked').parent().siblings().children('.ug-radio').removeClass('checked');
			}
		})
		
		$('#checkDate').bind({
			'focus': function(){
				$(this).attr({'type': 'date'});
			},
			'blur': function(){
				$(this).attr({'type': 'text'});
			}
		})
		
		
		// 独立函数
        this.scroller();
		this.former();
		this.nav();
		this.product();
		this.winScroll();
		this.productDetail();
		this.car();
		this.checkouts();
		this.checkCity();
		this.user();
    },
	checkouts: function(){
		if(!$('#checkoutPage').length){
			return false;
		}
		var logisticsRadio, // 配送方式
			paytypeRadio,   // 付款方式
			addressRadio,   // 送货地址
			invoiceRadio,   // 发票信息
		    couponRadio;    //优惠券
		
		// 展开填写字段(发票信息)
		$('#showInvoiceRow').unbind().bind('click', function(){
			if($(this).hasClass('checked')){
				$(this).removeClass('checked').parent().siblings().hide();
			}else{
				$(this).addClass('checked').parent().siblings().show();
			}
		})
		// 确认选择项(发票信息)
		$('.order-invoice .panel-filter .ok').bind('click', function(e){
			$(this).parents('.panel-filter').hide();
			if($('#showInvoiceRow').hasClass('checked')){
				$(this).parents('.panel-filter').siblings('span').text($('[name="InvoiceTitle"]').val() + '-' + $('[name="InvoiceContent"]:checked').val());
			}else{
				$(this).parents('.panel-filter').siblings('span').text('')
				$('[name="InvoiceTitle"]').val('');
				$('[name="InvoiceContent"]:checked').prop({'checked': false}).parent().removeClass('checked');
			}
			e.stopPropagation();
		})
		// 展开选择项(收货地址)
		$('.order-address').bind('click', function(){
			$(this).children('.panel-filter').show();
		})
		// 关闭选择项(收货地址)
		$('.order-address .panel-filter .cancel').bind('click', function(e){
			$(this).parents('.panel-filter').hide();
			e.stopPropagation();
		})
		// 确认选择项(收货地址)
		$('.order-address .panel-filter .ok').bind('click', function(e){
			var name = $(this).parents('.panel-filter').find('.checked span[name]').text(),
				phone = $(this).parents('.panel-filter').find('.checked span[phone]').text(),
				address = $(this).parents('.panel-filter').find('.checked span[address]').text();
			$(this).parents('.panel-filter').hide()
				.siblings('.name').text(name)
				.siblings('.phone').text(phone)
				.siblings('.address').text(address);
			readUserAddress();//重新读取地址
			e.stopPropagation();
		})
		// 展开选择项(配送方式)
		$('.order-logistics > span, .order-paytype > span, .order-invoice > span ,.order-coupon > span').bind('click', function () {
			$(this).siblings('.panel-filter').show();
		})
		// 关闭选择项(配送方式)
		$('.order-logistics .panel-filter .cancel, .order-paytype .panel-filter .cancel,.order-coupon .panel-filter .cancel, .order-invoice .panel-filter .cancel').bind('click', function (e) {
			$(this).parents('.panel-filter').hide();
			e.stopPropagation();
		})
		// 确认选择项(配送方式)
		$('.order-logistics .panel-filter .ok, .order-paytype .panel-filter .ok,.order-coupon .panel-filter .ok').bind('click', function (e) {
			var value = $(this).parents('.panel-filter').find('.checked').text();
			$(this).parents('.panel-filter').hide().siblings('span').text(value);
			e.stopPropagation();
		})
		// 选择项(付款方式、优惠券、收货地址)
		$('#paytypeRadio .radio dd,#couponRadio .radio dd,#addressRadio .radio dt').bind('click', function () {
			if($(this).find('input').prop('checked')){
				$(this).addClass('checked').siblings().removeClass('checked');
			}
		})
	  
		$('body').on('click', '#logisticsRadio .radio dd', function () {
		    if ($(this).find('input').prop('checked')) {
		        $(this).addClass('checked').siblings().removeClass('checked');
		    }
		});
		!function(){
			var _prevent = function(e){
				e.preventDefault();
			}
			var scrollLoaded = function(){
				logisticsRadio = new iScroll("logisticsRadio", { checkDOMChanges:true, vScrollbar:false });
				paytypeRadio = new iScroll("paytypeRadio", { checkDOMChanges:true, vScrollbar:false });
				addressRadio = new iScroll("addressRadio", { checkDOMChanges:true, vScrollbar:false });
				// invoiceRadio = new iScroll("invoiceRadio", { checkDOMChanges:true, vScrollbar:false });
			}
			document.addEventListener("DOMContentLoaded", scrollLoaded, false);
		}()
	},
	user: function(){
		$('#addAddAddress').bind('click', function(){
			$(this).hide();
			$('#userAddress').hide();
			$('#userAddressForm').show();
		})
	},
	checkCity: function(){
		// ChineseDistricts / province / city / district
		var _self = this,
			$input = $('#checkCity'),
			$obj,
			_html = '',
			_normal = '<li sid="" class="current">请选择</li>',
			result,
			lister;
		
		// 创建选择结构
		function create(sid, tit, callback){
			if(ChineseDistricts[sid] == undefined){
				$('#dialogCheckcityResult .list li.current').text(tit).attr({'sid': sid});
				if(callback){
					callback(false);
				}
			}else{
				var _province = '';
				$.each(ChineseDistricts[sid], function(index, element){
					_province += '<dd sid="'+ index +'">'+ element +'</dd>';
				})
				$('#dialogCheckcityResult .list li.current').nextAll().remove();
				$(_normal).appendTo('#dialogCheckcityResult .list').prev('li').removeClass('current').text(tit).attr({'sid': sid});
				$('#dialogCheckcityLister .list').html(_province);
				if(callback){
					callback(true);
				}
			}
		}
		
		// 修改选择结构
		function edit(sid, sid2, tit){
			var _province = '';
			$.each(ChineseDistricts[sid], function(index, element){
				_province += '<dd sid="'+ index +'">'+ element +'</dd>';
			})
			$('#dialogCheckcityLister .list').html(_province).children('dd[sid="'+ sid2 +'"]').addClass('checked');
		}
				
		$input.bind('click', function(){
			if($('#dialogCheckcity').length){
				$('#dialogCheckcity').show();
			}else{
				_html += '<section class="dialog-checkcity" id="dialogCheckcity">';
				_html += '<div class="head">';
				_html += '<h2>选择地区</h2>';
				_html += '<a href="javascript:;" class="close">关闭</a>';
				_html += '</div>';
				_html += '<div class="result clearfix" id="dialogCheckcityResult">';
				_html += '<ul class="list"></ul>';
				_html += '</div>';
				_html += '<div class="lister" id="dialogCheckcityLister">';
				_html += '<dl class="list"></dl>';
				_html += '</div>';
				_html += '</section>';
				
				$obj = $(_html);
				$obj.appendTo('body');
				create(86);  // 初始化中国省份
				scroller();  // 载入滑动效果
			}
			app.layout(1);
		})
		
		// 修改当前层级
		$('body').on('click', '#dialogCheckcityResult .list li', function(){
			var sid = 86,
				sid2 = '',
				tit = '';
			$(this).addClass('current').siblings().removeClass('current');
			if($(this).prev().length){
				sid = $(this).prev().attr('sid'),
				tit = $(this).prev().text();
			}
			sid2 = $(this).attr('sid')
			edit(sid, sid2, tit);
		})
		
		// 选择当前层级
		$('body').on('click', '#dialogCheckcity .lister dd', function(){
			var sid = $(this).attr('sid'),
				tit = $(this).text();
			$(this).addClass('checked').siblings().removeClass('checked');
			setTimeout(function(){
				create(sid, tit,function(res){
					if(!res){
						$input.val($('#dialogCheckcityResult .list').text());
						$obj.hide();
						app.layout(0);
					}else{
						lister.scrollTo(0, 0, 200);
					}
				});
			}, 300);
		})
		
		// 关闭多级联动选项卡
		$('body').on('click', '#dialogCheckcity .head', function(){
			$obj.hide();
			app.layout(0);
		})
		
		// 载入滑动插件
		function scroller(){
			result = new iScroll("dialogCheckcityResult", { checkDOMChanges:true, vScroll:false, vScrollbar:false });
			lister = new iScroll("dialogCheckcityLister", { checkDOMChanges:true, vScrollbar:false });
		}
	},
	winScroll: function(){
		var _self = this;
		
		$(window).bind({
			'touchstart': function(e){
				
			},
			'scroll': function (e) {
				
				// 赋值
				_self.number.scrollTop = $(this).scrollTop();
				// 滚动变色
				if( _self.number.scrollTop <= 90){
					var _per = Math.abs($(this).scrollTop()/100);
					//$('#header .wrap').css({'background': 'rgba(220,0,0,'+ _per +')'});
				}else{
					var _per = .9;
					//$('#header .wrap').css({'background': 'rgba(220,0,0,'+ _per +')'});
				}
			},
			'touchend': function(){
				
			}
		})
	},
	car: function(){
		var _self = this;
		
		$('body').on('click', '#listCar .del', function () {
		    var obj = $(this);
			_self.jConfirm('确认要删除此商品吗？', function(res){
			    if (res) {
				    // 执行AJAX数据交互				    
				    var strCartID = obj.attr("cartid");
				    var price = obj.attr("cartprice");
                    var oldCount = $("#BuyCount" + strCartID).val();
                    $(obj.parent().parent()).remove();

                    $.get("/CartAjax.aspx?Action=DeleteCart&StrCartID=" + strCartID + "&OldCount=" + oldCount + "&Price=" + price + "", function () {
                        dealDeleteCart();
                    });				    
				}
			});
		})
		
	    $('body').on('click', '#listCar .btn .edit', function () {
			$(this).hide().siblings('.comp').show();
			$(this).parents('.item').addClass('item-edit');
		})
		
	    $('body').on('click', '#listCar .btn .comp', function () {
	        // $(".js-number").text($(this).parents().find(".BuyCount").val());
	        $(this).parents(".item").find(".normal .js-number").text($(this).parents(".item").find(".BuyCount").val());
			$(this).hide().siblings('.edit').show();
			$(this).parents('.item').removeClass('item-edit');
		})
		
	},
	product: function(){
		if(!$('#listPro').length){
			return false;
		}
		
		var filterPanelMain,  // 滚动区域
			md = 'list',      // 筛选层级模式
			ud = 0;           // 点击的第几个属性
		
		// 展开筛选面板
		$('#showFilter').bind('click', function(){
			$('#filterPanel').show();
		})
		
		// 排序方式
		$('#showOrder').bind('click', function(){
			if($('#orderPanel').is(':hidden')){
				$('#orderPanel').show();
			}else{
				$('#orderPanel').hide();
			}
		})
		$('#orderPanel').bind('click', function(){
			$(this).hide();
		})
		
		// 多选快捷筛选
		$('#filterPanelMain .ug li').bind('click', function(){
			if($(this).hasClass('checked')){
				$(this).removeClass('checked');
			}else{
				$(this).addClass('checked');
			}
		})
		
		// 详细分类选择
		$('#filterPanelMain').on('click', '.detail dd', function(){
			$(this).addClass('checked').siblings().removeClass('checked');
		})
		
		// 展开详细分类
		$('#filterSelector dd').bind('click', function(){
			if($(this).attr('data-list') == ''){
				return false;
			}
			md = 'detail';
			ud = $(this).index();
			$('#filterSelector, #filterUg').hide();
			var list = $(this).attr('data-list').split(','),
				_html = '<dd>不限</dd>';
			var valList;
			if (typeof ($(this).attr('data-vallist'))!="undefined") valList = $(this).attr('data-vallist').split(',')
			for (index in list) {
			    if (getStringLength(list[index]) > 1) {
			        if (typeof ($(this).attr('data-vallist')) != "undefined")
			            _html += '<dd valid=' + valList[index] + '>' + list[index] + '</dd>';
                    else
					    _html += '<dd>'+ list[index] +'</dd>';
				}
			}
			$('#filterDetail').html(_html).show();
		})
		
		// 确认&取消按钮
		$('#filterCancel').bind('click', function(){
			if(md == 'list'){
				$('#filterPanel').hide();
			}else if(md == 'detail'){
				$('#filterDetail').empty().hide();
				$('#filterSelector, #filterUg').show();
			}
		})
		$('#filterOk').bind('click', function(){
			if(md == 'list'){
			    //属性选择
			    var hrefs = window.location.href;
			    if (hrefs.indexOf("?") < 0) { hrefs += "?"; }
			    var selectEx = selectAt = "";
			    $('#filterSelector dd[data-selected!="0"][data-selected!="不限"]').each(function () {
			        selectEx += encodeURI($.trim($(this).children('span').text())) + ";";
			            selectAt += $(this).attr("data-atid") + ";";
			    });

			    selectAt = selectAt.substr(0, selectAt.length - 1);
			    selectEx = selectEx.substr(0, selectEx.length - 1);
			        if (hrefs.indexOf("&at=") >= 0) {
			            var paraArr = window.location.search.split("&");
			            for (i = 0; i < paraArr.length; i++) {
			                var atvalueArr = paraArr[i].split("=");
			                if (selectEx != "") {
			                    if (atvalueArr[0].toLowerCase() == "at") hrefs = hrefs.replace(paraArr[i], "at=" + selectAt);
			                    if (atvalueArr[0].toLowerCase() == "ex") hrefs = hrefs.replace(paraArr[i], "ex=" + selectEx);
			                } else {
			                    if (atvalueArr[0].toLowerCase() == "at") hrefs = hrefs.replace("&" + paraArr[i], "");
			                    if (atvalueArr[0].toLowerCase() == "ex") hrefs = hrefs.replace("&" + paraArr[i], "");
			                }
			            }
			        } else {
			            if (selectEx != "") 
			                hrefs += "&at=" + selectAt + "&ex=" + selectEx;
			        }
			    //品牌选择
			    var brandid = "0";
			    $('#filterSelector dd[data-brandid]').each(function () {
			        brandid = $(this).attr("data-brandid");
			    });

			        if (hrefs.indexOf("&brand=") >= 0) {
			            var paraArr = window.location.search.split("&");
			            for (i = 0; i < paraArr.length; i++) {
			                var atvalueArr = paraArr[i].split("=");
			                if (atvalueArr[0].toLowerCase() == "brand") {
			                    if (brandid != "0") {
			                        hrefs = hrefs.replace(paraArr[i], "brand=" + brandid);
			                    } else {
			                        hrefs = hrefs.replace("&"+paraArr[i], "");
			                    }
			                }
			            }
			        } else {
			            if (brandid != "0") 
                            hrefs += "&brand=" + brandid;                        
			        }

			    window.location.href = hrefs;
			}
			else if (md == 'detail')
			{
				md = 'list';
				var value = $('#filterDetail dd.checked').text();
				$('#filterSelector dd').eq(ud).children('span').text(value);
				if (typeof ($('#filterSelector dd').eq(ud).attr("data-atid")) != "undefined")
				    $('#filterSelector dd').eq(ud).attr("data-selected", value);
                //获取品牌选值
				if (typeof ($('#filterSelector dd').eq(ud).attr("data-brandid")) != "undefined")
				{
				    var brandid = $('#filterDetail dd.checked').attr("valid");
				    if (typeof (brandid)!="undefined")
				        $('#filterSelector dd').eq(ud).attr("data-brandid", brandid);
				    else
				        $('#filterSelector dd').eq(ud).attr("data-brandid", "0");
				}
				$('#filterDetail').empty().hide();
				$('#filterSelector, #filterUg').show();
			}
		})
		
		!function(){
			var _prevent = function(e){
				e.preventDefault();
			}
			var scrollLoaded = function(){
				filterPanelMain = new iScroll("filterPanelMain", { checkDOMChanges: true, vScrollbar: false })
				
			}
			document.addEventListener("DOMContentLoaded", scrollLoaded, false);
		}()
	},
	productDetail: function(){
		// 切换TAB
		$('#detailTab li').bind('click', function(){
			var index = $(this).index();
			$(this).addClass('current').siblings().removeClass('current');
			$('#detailPro').children('*').eq(index).show().siblings().hide();
			$('body').animate({'scrollTop': 0}, 300);
		})
		
		// 标题切换
		$('#detailPro h1').bind('click', function(){
			$('#detailTab li').eq(1).trigger('click');
			$('body').animate({'scrollTop': 0}, 300);
		})
		
		// 评论切换
		$('#detailPro .detail-comment .head').bind('click', function(){
			$('#detailTab li').eq(2).trigger('click');
			$('body').animate({'scrollTop': 0}, 300);
		})
		
	},
	nav: function(){
		if(!$('#nav').length){
			return false;
		}
		
		var $navSide = $('#navSide'),
			$navMain = $('#navMain'),
			navSide,
			navMain;
		
		$navSide.find('dd').bind('click', function(e){
			$(this).addClass('current').siblings().removeClass('current');
			var index = $(this).index(),_height = $(this).height() + 1;
			
			$navMain.find('.list').eq(index).show().siblings().hide();
			// navSide.scrollTo(0, -_height*index, 300);
			// e.stopPropagation();
		})
		
		!function(){
			var _prevent = function(e){
				e.preventDefault();
			}
			var scrollLoaded = function(){
				navSide = new iScroll("navSide", { checkDOMChanges: true, vScrollbar: false });
				navMain = new iScroll("navMain", { checkDOMChanges: true, vScrollbar: false });
				
			}
			document.addEventListener("DOMContentLoaded", scrollLoaded, false);
		}()
	},
	former: function(){
		var $loginForm = $('#loginForm'),
			$regForm = $('#registerForm'),
			$findpwForm = $('#findpwForm'),
			$editpwForm = $('#editpwForm'),
			$feedbookForm = $('#feedbookForm'),
			$addressForm = $('#addressForm'),
			$infoForm = $('#infoForm'),
			$warm = $('#logregWram');
		
		// 切换密码模式
		$('.btn-password').bind('click', function(){
			if($(this).hasClass('btn-on')){
				$(this).removeClass('btn-on').addClass('btn-off').siblings('.txt').attr({'type': 'text'});
			}else{
				$(this).removeClass('btn-off').addClass('btn-on').siblings('.txt').attr({'type': 'password'});
			}
		})
		
		// 登录
		$loginForm.find('[type="submit"]').bind('click', function(e){
			if($loginForm.find('[name="UserName"]').val() == ''){
				$warm.text($loginForm.find('[name="UserName"]').attr('null'));
				return false;
			}
			if($loginForm.find('[name="UserPassword"]').val() == ''){
				$warm.text($loginForm.find('[name="UserPassword"]').attr('null'));
				return false;
			}
		})
		
		// 注册
		$regForm.find('[type="submit"]').bind('click', function(e){
			if($regForm.find('[name="UserName"]').val() == ''){
				$warm.text($regForm.find('[name="UserName"]').attr('null'));
				return false;
			}
			if(!$regForm.find('[name="UserName"]').val().match(app.rgexp.username)){
				$warm.text($regForm.find('[name="UserName"]').attr('error'));
				return false;
			}
			if($regForm.find('[name="UserPassword"]').val() == ''){
				$warm.text($regForm.find('[name="UserPassword"]').attr('null'));
				return false;
			}
			if(!$regForm.find('[name="UserPassword"]').val().match(app.rgexp.password)){
				$warm.text($regForm.find('[name="UserPassword"]').attr('error'));
				return false;
			}
			if($regForm.find('[name="UserPassword2"]').val() == ''){
				$warm.text($regForm.find('[name="UserPassword2"]').attr('null'));
				return false;
			}
			if($regForm.find('[name="UserPassword2"]').val() != $regForm.find('[name="UserPassword"]').val()){
				$warm.text($regForm.find('[name="UserPassword2"]').attr('error'));
				return false;
			}
			if($regForm.find('[name="Email"]').val() == ''){
				$warm.text($regForm.find('[name="Email"]').attr('null'));
				return false;
			}
			if(!$regForm.find('[name="Email"]').val().match(app.rgexp.email)){
				$warm.text($regForm.find('[name="Email"]').attr('error'));
				return false;
			}
			if($regForm.find('[name="SafeCode"]').val() == ''){
				$warm.text($regForm.find('[name="SafeCode"]').attr('null'));
				return false;
			}
		})
		
		// 找回密码
		$findpwForm.find('[type="submit"]').bind('click', function(e){
			if($findpwForm.find('[name="UserName"]').val() == ''){
				$warm.text($findpwForm.find('[name="UserName"]').attr('null'));
				return false;
			}
			if(!$findpwForm.find('[name="UserName"]').val().match(app.rgexp.username)){
				$warm.text($findpwForm.find('[name="UserName"]').attr('error'));
				return false;
			}
			if($findpwForm.find('[name="Email"]').val() == ''){
				$warm.text($findpwForm.find('[name="Email"]').attr('null'));
				return false;
			}
			if(!$findpwForm.find('[name="Email"]').val().match(app.rgexp.email)){
				$warm.text($findpwForm.find('[name="Email"]').attr('error'));
				return false;
			}
			if($findpwForm.find('[name="SafeCode"]').val() == ''){
				$warm.text($findpwForm.find('[name="SafeCode"]').attr('null'));
				return false;
			}
		})
		
		
		// 修改密码
		$editpwForm.find('[type="submit"]').bind('click', function(e){
			if($editpwForm.find('[name="OldPassword"]').val() == ''){
				$warm.text($editpwForm.find('[name="OldPassword"]').attr('null'));
				return false;
			}
			if(!$editpwForm.find('[name="OldPassword"]').val().match(app.rgexp.password)){
				$warm.text($editpwForm.find('[name="OldPassword"]').attr('error'));
				return false;
			}
			if($editpwForm.find('[name="UserPassword1"]').val() == ''){
				$warm.text($editpwForm.find('[name="UserPassword1"]').attr('null'));
				return false;
			}
			if(!$editpwForm.find('[name="UserPassword1"]').val().match(app.rgexp.password)){
				$warm.text($editpwForm.find('[name="UserPassword1"]').attr('error'));
				return false;
			}
			if($editpwForm.find('[name="UserPassword2"]').val() == ''){
				$warm.text($editpwForm.find('[name="UserPassword2"]').attr('null'));
				return false;
			}
			if($editpwForm.find('[name="UserPassword2"]').val() != $editpwForm.find('[name="UserPassword1"]').val()){
				$warm.text($editpwForm.find('[name="UserPassword2"]').attr('error'));
				return false;
			}
		})
		
		// 留言
		$feedbookForm.find('[type="submit"]').bind('click', function(e){
			if($feedbookForm.find('[name="title"]').val() == ''){
				$warm.text($feedbookForm.find('[name="title"]').attr('null'));
				return false;
			}
			if($feedbookForm.find('[name="content"]').val() == ''){
				$warm.text($feedbookForm.find('[name="content"]').attr('null'));
				return false;
			}
			if($feedbookForm.find('[name="content"]').val().length < 10){
				$warm.text($feedbookForm.find('[name="content"]').attr('error'));
				return false;
			}
		})
		
		// 添加收货地址
		$addressForm.find('[type="submit"]').bind('click', function(e){
			if($addressForm.find('[name="Consignee"]').val() == ''){
				$warm.text($addressForm.find('[name="Consignee"]').attr('null'));
				return false;
			}
			if($addressForm.find('[name="Mobile"]').val() == ''){
				$warm.text($addressForm.find('[name="Mobile"]').attr('null'));
				return false;
			}
			if(!$addressForm.find('[name="Mobile"]').val().match(app.rgexp.mobile)){
				$warm.text($addressForm.find('[name="Mobile"]').attr('error'));
				return false;
			}
			if($addressForm.find('[name="Tel"]').val() != '' && !$addressForm.find('[name="Tel"]').val().match(app.rgexp.telphone)){
				$warm.text($addressForm.find('[name="Tel"]').attr('error'));
				return false;
			}
			if ($addressForm.find('[name="ZipCode"]').val() != '' && !$addressForm.find('[name="ZipCode"]').val().match(app.rgexp.number)) {
			    $warm.text($addressForm.find('[name="ZipCode"]').attr('error'));
			    return false;
			}
			if ($("#UnlimitClass3").length <= 0) {
			    $warm.text("请选择完整的地区信息");
			    return false;
			}
			else {
			    if ($("#UnlimitClass3").find("option:selected").val() == "0") {
			        $("#UnlimitClass3").focus();
			        $warm.text("请选择完整的地区信息");
			        return false;
			    }

			}
			if ($addressForm.find('[name="Address"]').val() == '') {
			    $warm.text($addressForm.find('[name="Address"]').attr('error'));
			    return false;
			}
		})
		
		// 修改基本资料
		$infoForm.find('[type="submit"]').bind('click', function(e){
			if($infoForm.find('[name="Email"]').val() != '' && !$infoForm.find('[name="Email"]').val().match(app.rgexp.email)){
				$warm.text($infoForm.find('[name="Email"]').attr('error'));
				return false;
			}
			if($infoForm.find('[name="QQ"]').val() != '' && !$infoForm.find('[name="QQ"]').val().match(app.rgexp.qq)){
				$warm.text($infoForm.find('[name="QQ"]').attr('error'));
				return false;
			}
			if($infoForm.find('[name="Mobile"]').val() != '' && !$infoForm.find('[name="Mobile"]').val().match(app.rgexp.mobile)){
				$warm.text($infoForm.find('[name="Mobile"]').attr('error'));
				return false;
			}
			if($infoForm.find('[name="Tel"]').val() != '' && !$infoForm.find('[name="Tel"]').val().match(app.rgexp.telphone)){
				$warm.text($infoForm.find('[name="Tel"]').attr('error'));
				return false;
			}
		})
		
	},
    scroller: function(){
		!function(){
			if($('#banner li').length > 1){
				var $a = $('#banner'), length = $a.find('li').length, vi = 0, wid, t, autoPlayTime = 8000, autoAnimateTime = 500, loop = true;
				var clone = $a.find('li').eq(0).clone().addClass('clone'), tipHtml = '';;
				$a.children('.list').append(clone);
				if(length > 1){
					for(var i=0; i<length; i++){
						i == 0 ? tipHtml += '<span class="cur"></span>' : tipHtml += '<span></span>';
					}
					$a.children('.tip').show();
					$a.children('.num').text('1/' + length);
				}
				$a.children('.tip').html(tipHtml);
				var _init = function(){
					wid = $a.width();
					$a.children('.list').width(wid*(length+1));
					$a.find('li').width(wid);
					// $a.find('img').css({'width':wid});
					$a.css({'opacity':1});
				}
				var _func = function(){
					if(vi >= length){
						vi = 0;
						_func();
					}else{
						vi++;
						$a.children('.list').css({'-webkit-transform':'translate3d(-' + wid*vi + 'px, 0px, 0px)', '-webkit-transition':'-webkit-transform ' + autoAnimateTime + 'ms linear'});
						if(vi == length){
							$a.children('.tip').children('span').eq(0).addClass('cur').siblings().removeClass('cur');
							$a.children('.num').html('1/' + length);
							setTimeout(function(){
								$a.children('.list').css({'-webkit-transform':'translate3d(0px, 0px, 0px)', '-webkit-transition':'-webkit-transform 0ms linear'});
							}, autoAnimateTime);
						}else{
							$a.children('.tip').children('span').eq(vi).addClass('cur').siblings().removeClass('cur');
							$a.children('.num').html((vi + 1) + '/' + length);
						}
					}
				}
				var _touch = function(){
					var o_pagex = 0, o_pagey = 0,   // 接触记录值
						e_pagex = 0, e_pagey = 0;   // 离开记录值
					$a.bind({
						'touchstart':function(e){
							clearInterval(t);
							o_pagex = e.originalEvent.targetTouches[0].pageX;
							o_pagey = e.originalEvent.targetTouches[0].pageY;
						},
						'touchmove':function(e){
							e_pagex = e.originalEvent.changedTouches[0].pageX;
							e_pagey = e.originalEvent.changedTouches[0].pageY;
							var xpage = e_pagex - o_pagex;   //::负数-向左边滑动::正数-向右边滑动
							var ypage = e_pagey - o_pagey;
							if(Math.abs(xpage) > Math.abs(ypage)){
								if(xpage >= 0){
									if(vi <= 0){
										$a.children('.list').css({'-webkit-transform':'translate3d(-' + (wid*length - xpage) + 'px, 0px, 0px)', '-webkit-transition':'-webkit-transform 0ms linear'});
										vi = length;
									}
								}else{
									if(vi >= length){
										$a.children('.list').css({'-webkit-transform':'translate3d(0px, 0px, 0px)', '-webkit-transition':'-webkit-transform 0ms linear'});
										vi = 0;
									}
								}
								$a.children('.list').css({'-webkit-transform':'translate3d(-' + (wid*vi - xpage) + 'px, 0px, 0px)', '-webkit-transition':'-webkit-transform 0ms linear'});
								e.preventDefault();
							}
						},
						'touchend':function(e){
							$a.children('.list').css({'-webkit-transition':'-webkit-transform ' + autoAnimateTime + 'ms linear'});
							e_pagex = e.originalEvent.changedTouches[0].pageX
							e_pagey = e.originalEvent.changedTouches[0].pageY
							if(Math.abs(e_pagey - o_pagey) > 0 && Math.abs(e_pagex - o_pagex) < 50){
								vi -=1;
								_func();
							}else{
								if(e_pagex - o_pagex > 0){  // 手指向右边滑动
									vi-=2;
									_func();
								}else if(e_pagex - o_pagex < 0){  // 手指向左边滑动
									_func();
								}
							}
							t = setInterval(_func, autoPlayTime);
						}
					});
				}
				_touch();
				_init();
				t = setInterval(_func, autoPlayTime);
				$(window).resize(_init);
				window.onorientationchange = function() {
					_init();
				};
			}
		}()
    },
	jMsg: function(title){
		var _self = this,
			_html = '';
		
		_html += '<section class="dialog-msg">';
		_html += '<figure class="ico-success"></figure>';
		_html += '<p>'+ title +'</p>';
		_html += '</section>';
		
		var $obj = $(_html);
		if(!$('.dialog-msg').length){
			$obj.appendTo('body').show();
			setTimeout(function(){
				$obj.remove();
			}, 1000);
		}
    },
	jAlert: function(info, callback){
		var _self = this,
			_html = '';
		
		_self.layout(1);
		
		_html += '<div class="dialog-alert">';
		_html += '<div class="main">'+ info +'</div>';
		_html += '<div class="foot">';
		_html += '<a href="javascript:;" class="ok">我知道了</a>';
		_html += '</div>';
		_html += '</div>';		
				
		var $obj = $(_html);
		$obj.appendTo('body').show();
		$obj.find('.close')
			.bind('click', function(){
				_self.layout(0);
				$obj.hide().remove();
				if(callback){
					callback(false);
				}
			});
		$obj.find('.ok')
			.bind('click', function(){
				_self.layout(0);
				$obj.hide().remove();
				if(callback){
					callback(true);
				}
			})
	},
	jConfirm: function(info, callback){
		var _self = this,
			_html = '';
		
		_self.layout(1);		
		
		_html += '<div class="dialog-confirm">';
		_html += '<div class="main">'+ info +'</div>';
		_html += '<div class="foot">';
		_html += '<a href="javascript:;" class="cancel">取消</a>';
		_html += '<a href="javascript:;" class="ok">确定</a>';
		_html += '</div>';
		_html += '</div>';
				
		var $obj = $(_html);
		$obj.appendTo('body').show();
		$obj.find('.close')
			.bind('click', function(){
				_self.layout(0);
				$obj.hide().remove();
				if(callback){
					callback(false);
				}
			});
		$obj.find('.ok')
			.bind('click', function(){
				_self.layout(0);
				$obj.hide().remove();
				if(callback){
					callback(true);
				}
			})
		$obj.find('.cancel')
			.bind('click', function(){
				_self.layout(0);
				$obj.hide().remove();
				if(callback){
					callback(false);
				}
			})
	},
	layout: function(u){
		var $obj = $('<div class="dialog-layout"></div>');
		
		if(u == 0){
			$('.dialog-layout').remove();
		}else{
			if(!$('.dialog-layout').length){
				$obj.appendTo('body').show();
			}
		}
	}
}
var app = new application();
app.ini();