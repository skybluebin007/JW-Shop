$(function(){
	login();  // 登陆
	getPw();  // 找回密码
	register();  // 注册账号
});
// 登陆
function login(){
	if(!$("#loginForms").length){ return false; }
	var a = $("#loginForms"), user = $("#UserName"), password = $("#UserPassword"), mc = $("#mc");
	a.find(".submit").click(function(){
		var bool = true;
		if(user.val() == ""){
			bool = false;
			user.parents(".row").addClass("err");
		}
		if(password.val() == ""){
			bool = false;
			password.parents(".row").addClass("err");
		}
		return bool;
	});
	a.find(".txt").focus(function(){
		$(this).parents(".row").addClass("focus").addClass("blueLine").removeClass("err");
	}).blur(function(){
		if($(this).val() == ""){
			$(this).parents(".row").removeClass("focus").removeClass("blueLine");
		}else{
			$(this).parents(".row").removeClass("blueLine");
		}
	});
	// 随机图片
	var N = mc.children("img").length;
	if(N >= 2){
		var index = Math.ceil(Math.random()*N);
		mc.children("img").eq(index-1).animate({"opacity":1}, 1000).siblings("img").remove();
	}
}
// 找回密码

function getPw(){
    /*已整合至 findpassword.js
	if(!$("#getPw").length){ return false; }
	var a = $("#getPw"), user = $("#UserName"), email = $("#Email"), mobile = $("#Mobile"), code = $("#SafeCode"), mobileCode = $("#phoneVer");
	a.find(".submit").click(function(){
	    var bool = true;
	    var checkValue = $("input[name='checkType']:checked").val();
	  
	  
		if(user.val() == ""){
			bool = false;
			user.parents(".row").addClass("err");
		}
		if (checkValue == "0") {//手机验证
		    if (mobile.val() == "" || !/^1[3-9]\d{9}$/.test(mobile.val())) {
		        bool = false;
		        mobile.parents(".row").addClass("err");
		    }
		    if (mobileCode.val() == "") {
		        bool = false;
		        mobileCode.parents(".row").addClass("err");
		    }
		}
		else {//邮箱验证
		    if (email.val() == "" || !/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email.val())) {
		        bool = false;
		        email.parents(".row").addClass("err");
		    }
		}
		if(code.val() == ""){
			bool = false;
			code.parents(".row").addClass("err");
		}
		return bool;
	});
	a.find(".txt, .txt2").focus(function(){
		$(this).parents(".row").addClass("focus").addClass("blueLine").removeClass("err");
	}).blur(function(){
		if($(this).val() == ""){
			$(this).parents(".row").removeClass("focus").removeClass("blueLine");
		}else{
			$(this).parents(".row").removeClass("blueLine");
		}
	});
    */
}

// 注册账号
function register(){
	if(!$("#register").length){ return false; }
	var a = $("#register"), user = $("#UserName"), pw = $("#pw"), upw = $("#upw"), email = $("#email"), code = $("#code"), checkBox = $("#checkBox"), article = $("#article");
	a.find(".submit").click(function(){
		var bool = true;
		if(user.val() == ""){
			bool = false;
			user.parents(".row").addClass("err");
			user.parent().siblings(".ted").html("<font color='#cc0000'>请输入账号</font>");
		}else if(user.val().length < 3 || user.val().length > 15 || !/^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]+$/.test(user.val())){
			bool = false;
			user.parents(".row").addClass("err");
			user.parent().siblings(".ted").html("<font color='#cc0000'>3-15个字符组成（包括字母、数字、下划线、中文）</font>");
		}else{
			user.parent().siblings(".ted").html("<font color='#1d75c2'></font>");
		}
		if(pw.val() == ""){
			bool = false;
			pw.parents(".row").addClass("err");
			pw.parent().siblings(".ted").html("<font color='#cc0000'>请输入密码</font>");
		}else if(pw.val().length < 4 || pw.val().length > 10){
			bool = false;
			pw.parents(".row").addClass("err");
			pw.parent().siblings(".ted").html("<font color='#cc0000'>密码由4-10个字符组成</font>");
		}else{
			pw.parent().siblings(".ted").html("<font color='#1d75c2'></font>");
		}
		if(upw.val() == ""){
			bool = false;
			upw.parents(".row").addClass("err");
			upw.parent().siblings(".ted").html("<font color='#cc0000'>请输入密码</font>");
		}else if(pw.val() != upw.val()){
			bool = false;
			upw.parents(".row").addClass("err");
			upw.parent().siblings(".ted").html("<font color='#cc0000'>两次密码不一致</font>");
		}else{
			upw.parent().siblings(".ted").html("<font color='#1d75c2'></font>");
		}
		if(email.val() == ""){
			bool = false;
			email.parents(".row").addClass("err");
			email.parent().siblings(".ted").html("<font color='#cc0000'>请输入电子邮箱</font>");
		}else if(!/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email.val())){
			bool = false;
			email.parents(".row").addClass("err");
			email.parent().siblings(".ted").html("<font color='#cc0000'>请填写正确的邮箱地址 例如：abc@163.com</font>");
		}else{
			email.parent().siblings(".ted").html("<font color='#1d75c2'></font>");
		}
		if(code.val() == ""){
			bool = false;
			code.parents(".row").addClass("err");
			code.parent().siblings(".ted").html("<font color='#cc0000'>请输入验证码</font>");
		}else{
			code.parent().siblings(".ted").html("<font color='#1d75c2'></font>");
		}
		//alert(checkBox.attr("checked"));
		if(!checkBox.is(":checked")){
			bool = false;
			article.addClass("articleErr");
		}else{
			article.removeClass("articleErr");
		}
		return bool;
	});
	a.find(".txt, .txt2").focus(function(){
		$(this).parents(".row").addClass("focus").addClass("blueLine").removeClass("err");
		$(this).parent().siblings(".ted").html($(this).attr("data-rel"));
	}).blur(function(){
		if($(this).val() == ""){
			$(this).parents(".row").removeClass("focus").removeClass("blueLine");
		}else{
			$(this).parents(".row").removeClass("blueLine");
		}
	});
}




































