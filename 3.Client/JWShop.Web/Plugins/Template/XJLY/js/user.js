$(function(){
	ueUser();  // ueUser
//	orderAjax();  // ajax请求订单
});
// ueUser
function ueUser(){
	var b = $("#sideBar");
	// 侧栏
	b.find("dt").click(function(){
		if($(this).hasClass("cur")){
			$(this).removeClass("cur").next("dd").slideDown(200);
		}else{
			$(this).addClass("cur").next("dd").slideUp(200);
		}
	});
	// 通用切换选项卡
	$("#userTab .tab a.item").click(function(){
		$(this).addClass("cur").siblings("a.item").removeClass("cur");
		$("#userTab .container .conIndex").eq($(this).index()).show().siblings(".conIndex").hide();
	});
	// 充值
	var rechargeForm = $("#rechargeForm");
	rechargeForm.find(".money").focus(function(){
		$(this).removeClass("red").removeClass("redBorder").siblings(".rechargeTip").hide();
	}).blur(function(){
		if(!/^((0|([1-9][0-9]*))+(\.\d{2})?)$/.test($(this).val()) || $(this).val() == "0.00"){
			$(this).addClass("red").addClass("redBorder").siblings(".rechargeTip").show();
		}
	});
	// 提交充值订单
	rechargeForm.find(".submit").click(function(){
		if(rechargeForm.find(".money").val() == ""){
			rechargeForm.find(".money").addClass("redBorder");
			return false;
		}else if(rechargeForm.find(".rechargeTip").is(":visible")){
			rechargeForm.find(".money").addClass("redBorder");
			return false;
		}
	});
	
	// sms
	var smsCon = $("#smsCon");
	smsCon.find(".allCheckBox").change(function(){
		$(this).parents(".conIndex").find(".checkBox").prop({"checked":this.checked});
	});
	smsCon.find(".checkBox").change(function(){
		if($(this).parents(".conIndex").find(".checkBox").length == $(this).parents(".conIndex").find(".checkBox:checked").length){
			$(this).parents(".conIndex").find(".allCheckBox").prop({"checked":true});
		}else{
			$(this).parents(".conIndex").find(".allCheckBox").prop({"checked":false});
		}
		
	});
	smsCon.find(".allCheck").click(function(){
		$(this).parents(".conIndex").find(".checkBox, .allCheckBox").prop({"checked":true});
	});
	smsCon.find(".nullCheck").click(function(){
		$(this).parents(".conIndex").find(".checkBox, .allCheckBox").prop({"checked":false});
	});
	// 发送信息、好友查找
	var sendSms = $("#sendSms"), findFre = $("#findFre");
	sendSms.find(".submit").click(function(){
		var bool = true;
		if(sendSms.find(".smsName").val() == "" || sendSms.find(".smsName").val() == "请输入收件人"){
			sendSms.find(".smsName").val("请输入收件人").addClass("redBorder");
			bool = false;
		}
		if(sendSms.find(".smsTitle").val() == "" || sendSms.find(".smsTitle").val() == "请输入标题"){
			sendSms.find(".smsTitle").val("请输入标题").addClass("redBorder");
			bool = false;
		}
		return bool;
	});
	sendSms.find(".txt_300, .text_300").focus(function(){
		$(this).removeClass("redBorder");
	});
	findFre.find(".txt").keyup(function(){
		var key = $(this).val();
		$.ajax({
			url: "ajax.xml",
			data:{
				key: key
			},
			type:"GET",
			dataType:"xml",
			success: function(data){
				if($(data).find("err").attr("type") == 1){
					var html = "";
					$(data).find("row").each(function(index, element){
                        var name = $(this).attr("name");
						html += "<li><a href=\"javascript:;\">"+ name +"</a></li>"
                    });
					findFre.children(".ls").html(html);
					findFre.find(".ls a").click(function(){
						sendSms.find(".smsName").val($(this).text()).removeClass("redBorder");
					});
				}else{
					findFre.children(".ls").html("<li><span>查询出现错误，联系管理员.</span></li>");
				}
			}
		})
	});
	// 添加优惠券
	var createCoupon = $("#createCoupon");
	createCoupon.find(".submit").click(function(){
		var bool = true;
		if(createCoupon.find(".card").val() == "" || createCoupon.find(".card").val() == "请输入卡号"){
			createCoupon.find(".card").val("请输入卡号").addClass("redBorder");
			bool = false;
		}
		if(createCoupon.find(".password").val() == "" || createCoupon.find(".password").val() == "请输入密码"){
			createCoupon.find(".password").val("请输入密码").addClass("redBorder");
			bool = false;
		}
		return bool;
	});
	createCoupon.find(".txt_300").focus(function(){
		$(this).removeClass("redBorder");
	});
	// 留言
	var sendMsg = $("#sendMsg");
	sendMsg.find(".submit").click(function(){
		var bool = true;
		if(sendMsg.find(".msgTitle").val() == "" || sendMsg.find(".msgTitle").val() == "请输入标题"){
			sendMsg.find(".msgTitle").val("请输入标题").addClass("redBorder");
			bool = false;
		}
		if(sendMsg.find(".msgInfo").val() == "" || sendMsg.find(".msgInfo").val() == "请输入内容"){
			sendMsg.find(".msgInfo").val("请输入内容").addClass("redBorder");
			bool = false;
		}
		return bool;
	});
	sendMsg.find(".txt_300, .text_300").focus(function(){
		$(this).removeClass("redBorder");
	});
	// 地址薄
	var createAddr = $("#createAddr");
	createAddr.find(".submit").click(function(){
		var bool = true;
		if(createAddr.find(".addrName").val() == "" || createAddr.find(".addrName").val() == "请输入收货人"){
			createAddr.find(".addrName").val("请输入收货人").addClass("redBorder");
			bool = false;
		}
		if(!/^\d{11}$/.test(createAddr.find(".addrPhone").val())){
			createAddr.find(".addrPhone").val("手机号不合法").addClass("redBorder");
			bool = false;
		}
		return bool;
	});
	createAddr.find(".txt_300").focus(function(){
		$(this).removeClass("redBorder");
	});
	// 修改密码
	var changePs = $("#changePs");
	changePs.find(".submit").click(function(){
		var bool = true;
		if(changePs.find(".oldPassword").val().length < 4 || changePs.find(".oldPassword").val().length > 10){
			changePs.find(".oldPassword").addClass("redBorder");
			bool = false;
		}
		if(changePs.find(".newPassword").val().length < 4 || changePs.find(".newPassword").val().length > 10){
			changePs.find(".newPassword").addClass("redBorder");
			bool = false;
		}
		if(changePs.find(".newPassword").val() != changePs.find(".uPassword").val()){
			changePs.find(".uPassword").addClass("redBorder");
			bool = false;
		}
		return bool;
	});
	changePs.find(".txt_300").focus(function(){
		$(this).removeClass("redBorder");
	});
}
//// 订单AJAX
//function orderAjax(){
//	var filter = "status=",
//		key = "&key=",
//		page = 1,
//		links = filter + key;
//	// 订单列表搜索
//	var orderDefaultVal = "搜索订单编号", a = $("#orderSearch"), b = $("#sideBar"), c = $("#orderList"), container = c.find(".tbody");
//	a.find(".txt").focus(function(){
//		$(this).val("")
//		a.removeClass("redBorder");
//		a.animate({"width":250}, {easing:"", duration:400});
//	}).blur(function(){
//		var _this = $(this);
//		if(_this.val() == "" || _this.val() == orderDefaultVal){
//			a.animate({"width":215}, {easing:"", duration:400, complete:function(){
//				_this.val(orderDefaultVal);
//			}});
//		}
//	});
//	a.find(".submit").click(function(){
//		if(a.find(".txt").val() == "" || a.find(".txt").val() == orderDefaultVal){
//			a.addClass("redBorder").find(".txt").val(orderDefaultVal);;
//			return false;
//		}else{
//			key = "&key=" + a.find(".txt").val();
//			links = filter + key;
//			ajaxOrderData(links, container, page);
//		}
//	});
//	// 选择不同当前状态
//	c.find(".tab .item").click(function(){
//		$(this).addClass("cur").siblings(".item").removeClass("cur");
//		filter = "status=" + $(this).attr("value");
//		links = filter + key;
//		ajaxOrderData(links, container, page);
//	});
//	ajaxOrderPager(links, container, page);  // 绑定翻页
//}
//function ajaxOrderData(links, container, page){
//	$.ajax({
//		url: "orderAjax.asp?" + links,
//		data: {
//			page: page
//		},
//		type:"GET",
//		dataType:"html",
//		beforeSend: function(){
//			container.html("<div class='loading'>请稍等，正在加载数据...</div>");
//			ajaxOrderPager(links, container, page);  // 绑定翻页
//		},
//		success: function(data){
//			container.html(data);
//		}
//	})
//}
//function ajaxOrderPager(links, container, page){
//	$("#orderPager a").click(function(){
//		ajaxOrderData(links, container, $(this).attr("value"));
//	});
//}