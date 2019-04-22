//右侧菜单
;(function($){
	$(function(e){
		nav_init();
	});
	
	function nav_init(){
		
		if(!$("#navbar").get(0)) return false;
		
		$("#navbar > .nav > dd > a").on("click",function(e){
			var _parent = $(this).parent();
			var _sub = _parent.children(".sub-nav");
			
			if(_sub.get(0)){
				e.preventDefault();
				if(_sub.is(":hidden")){
					$("#navbar").find(".sub-nav:visible").hide().siblings("a").children("i").removeClass("triangle_open");
					
					_sub.show();
					$(this).children("i").addClass("triangle_open");
				}else{
					$(this).children("i").removeClass("triangle_open");
					_sub.hide();
				}
			}
		//	console.log(_parent);
		});
	}
	$(".tab-title span").click(function() {
		var $li = $(".tab-title span");
		var $ul = $('.container .product-main');
		var $this = $(this);
		var $t = $this.index();
		$li.removeClass("cur");
		$this.addClass('cur');
		$ul.eq($t).show().siblings(".product-main").hide();
	});
	$(".tab-title span").click(function() {
		$(this).addClass('cur').siblings().removeClass('cur');
		$('.product-container .product-row').eq($(this).index()).show().siblings().hide();
		
	});
})(jQuery)
