function iscroller(o){
	function loaded(o) {
		var myScroll = new iScroll(o,{ hScrollbar:false, vScrollbar:false});
	}
	document.addEventListener('DOMContentLoaded', loaded(o), false);
}

$(function () {
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
	
	
});
function layout(u){
	var $obj = $('<div class="dialog-layout"></div>');
	if(u == 0){
		$('.dialog-layout').remove();
	}else{
		if(!$('.dialog-layout').length){
			$obj.appendTo('body').show();
		}
	}
}