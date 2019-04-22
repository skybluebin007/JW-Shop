$(function () {
	miTouch();
	xcxht();
	pro_m();
	$("a").focus(function () {
        $(this).blur();
    });
    $(".g-top").bind("click",function() {
 		 $("body, html").stop().animate({"scrollTop":"0px"});
	});
	$(window).scroll(function(){
        var h=$(document).scrollTop();
		  if( h >= 250){ 
		   	  $(".g-top").show();
		  }else{
		  	  $(".g-top").hide();
		  }
	});
 	$(".goBack").click(function(){ 
		// history.back();
		history.go(-1);  
    });
    $(".tabbed .ztmore").click(function(e){
 		 $(this).find(".drop").toggleClass("hide");
 		 e.stopPropagation();
	});
	
	
})
// 系统交互：触屏效果
function miTouch(){
	$(".menulist li a,#navBar li a").bind({"touchstart":function(){
		$(this).addClass("hover");
	}, "touchmove":function(){
		$(this).removeClass("hover");
	}, "touchend":function(){
		$(this).removeClass("hover");
	}});
	
}
// 后台js效果
function xcxht(){
	// 切换密码模式
	$('.btn-password').bind('click', function(){
		if($(this).hasClass('btn-on')){
			$(this).removeClass('btn-on').addClass('btn-off').siblings('.txt').attr({'type': 'text'});
		}else{
			$(this).removeClass('btn-off').addClass('btn-on').siblings('.txt').attr({'type': 'password'});
		}
	});
	// 当发生 keyup 事件时，密码可删除
	$("#UserPassword").keyup(function(){
	    $(".btn-password_del").removeClass('hide');
	    $(this).parents(".logreg").find(".submit").addClass('hover');
	  });
	  $("#email").keyup(function(){
	    $(this).parents(".logreg").find(".submit").addClass('hover');
	  });
	// 密码删除 
	$('.btn-password_del').click(function(){
		$(this).addClass('hide').siblings('#UserPassword').val("").focus();// 清空并获得焦点
		$(this).parents(".logreg").find(".submit").removeClass('hover');
		
	});
	$('#NewPassword2').keyup(function(){
		$(this).parents(".logreg").find(".submit").addClass('hover');
		
	});
	
	// 停止营业
	$('#stopbtn').click(function(){
		$(this).parents(".tradepage").find('.closebox').removeClass('hide');
		$(this).parents(".open").addClass('hide');
	});
	// 恢复营业时间
	$('#openbtn').click(function(){
		$(this).parents(".tradepage").find('.open').removeClass('hide');
		$(this).parents(".closebox").addClass('hide');
	});
	// 打印设置
	$('.btn_print').bind('click', function(){
		if($(this).hasClass('btn_on')){
			$(this).removeClass('btn_on').addClass('btn_off');
		}else{
			$(this).removeClass('btn_off').addClass('btn_on');
			}
	});
	// 应用铃声
	$('.ringbox .item').click(function(){
		$(this).toggleClass("hover");
	});
	$(".sphead .dian").click(function(){
 		 $(this).find(".drop").toggleClass("hide");
	});
	$('.Setpage .bfpl .item a').click(function(){
		$(this).siblings().removeClass('hide');
		$(".mask").removeClass('hide');
	});
	$('.bfpl .popbox li').click(function(){
		$(this).addClass("hover").siblings().removeClass('hover');
	});
	$('.Setpage .bfpl .item .quer').click(function(){
		$(this).parents(".popbox").addClass('hide');
		$(".mask").addClass('hide');
		var pp = $(this).parents(".popbox").find(".hover").text();
		$(this).parents(".item").find(".txt b").text(pp);
	});
	$(".bannerlist .item .dian").click( function (e) {	
 		 $(this).parents(".item").find(".drop").toggleClass("hide");
 		 e.stopPropagation();
	});
	$("body").click(function () {
	    $(".bannerlist .item").find(".drop").addClass("hide");
	    $(".tabbed .ztmore").find(".drop").addClass("hide");
	});
	//待审核订单
	$('.orderlist2').on('click','.item .sp_info .sbtn', function(){
		if($(this).hasClass('open')){
			$(this).removeClass('open').addClass('close');
			$(this).html("收起<em class='icon'></em>")
			$(this).siblings('.sp_list').removeClass('hide');
		}else{
			$(this).removeClass('close').addClass('open');
			$(this).html("展开<em class='icon'></em>")
			$(this).siblings('.sp_list').addClass('hide');
			}
		});
		$('.orderlist2').on('click','.item .cuxiao .sj', function(){
			$('.Pop-ups.sj').removeClass('hide');
			$('.mask').removeClass('hide');
		});
		$('.orderlist2').on('click','.item .cuxiao .pt', function(){
			$('.Pop-ups.pt').removeClass('hide');
			$('.mask').removeClass('hide');
		});
		$(".mask").click(function(){
	 		 $(".Pop-ups").addClass("hide");
	 		 $(this).addClass('hide');
	 		 $('.Setpage .bfpl .popbox').addClass("hide");
		});
		$(".Pop-ups a").click(function(){
	 		 $(this).parents(".Pop-ups").addClass('hide');
	 		 $('.mask').addClass('hide');
		});
		
		$postform = $('#form1');
		$postform.find(".labbox label").click(function(){
	    var radioId = $(this).attr('name');
	    $(this).addClass('checked').siblings('label').removeClass('checked');
		$('input[type="radio"]:checked').attr('value')
	  });
	  //餐饮许可证
	    $postform = $('#honorform');
		$postform.find(".sexbox label").click(function(){
		    var radioId = $(this).attr('name');
		    $(this).addClass('checked').siblings('label').removeClass('checked');
			$('input[type="radio"]:checked').attr('value');
			$(this).parents(".itembox").siblings(".youxiaotime").toggleClass("hide");
	  	});
	  //图片上传
	  	$(".picbox .cypic").click(function(){
	 		 $(this).siblings(".slpicbox").removeClass("hide");
		});
		$(".inputFileUpload").click(function(){
	 		 $(this).parents(".slpicbox").addClass("hide");
		});
	  //文本聚焦失焦判断
	    $('.in_kong').each(function() {
	        inputPromt($(this));
	    });
	
	
}
function inputPromt(inputObj){
    var inputVal = inputObj.attr('place');
    inputObj.focus(function () {
        if(inputObj.val() == '' || inputObj.val() == inputVal){
            inputObj.val('');
        }
        inputObj.css('color', '#000');
    }).blur(function() {
        if (inputObj.val() == '' || inputObj.val() == inputVal) {
            inputObj.val(inputVal).css("color", "#e70012");
            $(this).siblings("p").css("display", "block");
        }else{
            inputObj.css('color', '#000');
            $(this).siblings("p").css("display", "none");
        }
    });
}
function pro_m(){
		if(!$('#pro_box').length){
			return false;
		}
		
		var $navSide = $('#navSide'),
			$navMain = $('#proMain'),
			navSide,
			navMain;
		
		$navSide.find('dd').bind('click', function(e){
			$(this).addClass('current').siblings().removeClass('current');
//			var index = $(this).index(),_height = $(this).height() + 1;
//			
//			$navMain.find('.list').eq(index).show().siblings().hide();
			$navMain.animate({scrollTop:0});
			// navSide.scrollTo(0, -_height*index, 300);
			// e.stopPropagation();
		})

		
		!function(){
			var _prevent = function(e){
				e.preventDefault();
			}
//			var scrollLoaded = function(){
//				navSide = new iScroll("navSide", { checkDOMChanges: true, vScrollbar: false });
//				navMain = new iScroll("proMain", { checkDOMChanges: true, vScrollbar: false });
//				
//			}
//			document.addEventListener("DOMContentLoaded", scrollLoaded, false);
		}()
}
//提示框
function Msg(title) {
    var _self = this,
        _html = '';

    _html += '<section class="dialog-msg">';
    _html += '<figure class="ico-failure"></figure>';
    _html += '<p>' + title + '</p>';
    _html += '</section>';

    var $obj = $(_html);
    if (!$('.dialog-msg').length) {
        $obj.appendTo('body').show();
        setTimeout(function () {
            $obj.remove();
        }, 1000);
    }
}

