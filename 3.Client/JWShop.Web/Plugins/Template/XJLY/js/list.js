$(function () {
    ueList();  // ueList
    //productAjax();  // ajax请求商品
    ueDetailBrand();  // 品牌详情页
    detail();  // 商品详情页
    cart();  // 购物车

    $(".menu-drop").hover(function () {
        $(this).addClass("z-menu-drop-open");
        $(this).find(".menu-drop-main").show();
    }, function () {
        $(this).removeClass("z-menu-drop-open");
        $(this).find(".menu-drop-main").hide();
    });
});
// ueList
function ueList() {
    var elem = $("#listContainer"), a = elem.find(".item"), b = elem.find(".g img"), c = elem.find(".ls"), d = $("#filterByPriceSection"), e = $("#filter"); defLength = 6, defBool = true;
    var $selector = $('#selector'), _selectorTxt = '';
    // 监控滚动
    $(window).scroll(function () {
        if (!e.length) { return false; }
        var scrollTop = $(document).scrollTop(),
			u = e.offset().top;
        if (scrollTop > u) {
            e.addClass("filterPosFixed");
        } else {
            e.removeClass("filterPosFixed");
        }
    });
    // 筛选商品 2016.3.24 Add
    $selector.find('.selector-mo span').bind('click', function () {
        if (_selectorTxt == '') {
            _selectorTxt = $(this).text();
            $(this).text('收起选项').parent().siblings().show();
        } else {
            $(this).text(_selectorTxt).parent().siblings(':not(".visible")').hide();
            _selectorTxt = '';
        }
    })
    $selector.find('.more').bind('click', function () {
        $(this).parent().hide().siblings().show();
    })
    // 热销商品
    $("#hotmall li").hover(
		function () {
		    $(this).find("img").stop().animate({ "left": 5 }, { easing: "jswing", duration: 200 });
		},
		function () {
		    $(this).find("img").stop().animate({ "left": 0 }, { easing: "jswing", duration: 200 });
		}
	);
    // 商品列表
    a.hover(
		function () {
		    $(this).addClass("hover");
		},
		function () {
		    $(this).removeClass("hover");
		}
	);
    b.hover(
		function () {
		    $(this).addClass("css3Scale");
		},
		function () {
		    $(this).removeClass("css3Scale");
		}
	).parent().attr({ "title": "全网正品 支持货到付款" });
    $("#listContainer .ls img").hover(
		function () {
		    $(this).parents("dd").addClass("cur").siblings("dd").removeClass("cur").end().parents(".item").find(".g img").attr({ "src": $(this).attr("bigimg") });
		},
		function () {
		    //
		}
	);
    // 智能判断小图模式
    c.each(function (index, element) {
        var me = $(this),
			N = me.find("dd").length,
			ci = Math.ceil(N / defLength),
			n = 0,
			w = me.find("dd").width() * defLength;
        me.find("dl").width(w * ci);
        if (N > defLength) {
            me.addClass("iTag");
            var func = function () {
                if (n < ci - 1) {
                    n++;
                    me.children(".l").removeClass("noDrop");
                    me.find("dl").animate({ "margin-left": (-w * n) }, { easing: "", duration: 300, complete: function () {
                        if (n == ci - 1) {
                            me.children(".r").addClass("noDrop");
                        } else {
                            me.children(".r").removeClass("noDrop");
                        }
                    }
                    });
                }
            },
			func2 = function () {
			    if (n > 0) {
			        n--;
			        me.children(".r").removeClass("noDrop");
			        me.find("dl").animate({ "margin-left": (-w * n) }, { easing: "", duration: 300, complete: function () {
			            if (n == 0) {
			                me.children(".l").addClass("noDrop");
			            } else {
			                me.children(".l").removeClass("noDrop");
			            }
			        }
			        });
			    }
			}
            me.children(".r").click(func);
            me.children(".l").click(func2);
        }
    });
    // 删选单价区间
    d.children(".txt").focus(function () {
        d.children(".btn").show();
        defBool = false;
        $("body").unbind("click").bind("click", function () {
            if (defBool) {
                d.children(".btn").hide();
            }
        })
    }).blur(function () {
        defBool = true;
    });
    d.find(".reset").click(function () {
        d.children(".txt").val("");
        return false;
    });
    // 切换模式
//    var jw_listModul = $.cookie("jw_listModul");
//    if (jw_listModul) {
//        e.find(".vc a").eq(jw_listModul).addClass("cur").siblings().removeClass("cur");
//        if (jw_listModul == 0) {
//            elem.attr({ "class": "listContainer listModul" });
//        } else if (jw_listModul == 1) {
//            elem.attr({ "class": "listContainer picModul" });
//        }
//    }
    e.find(".vc a").click(function () {
        $(this).addClass("cur").siblings("a").removeClass("cur");
        var index = $(this).index();
        if (index == 0) {
            elem.attr({ "class": "listContainer listModul" });
        } else if (index == 1) {
            elem.attr({ "class": "listContainer picModul" });
        }
        // 写缓存
        $.cookie("jw_listModul", index);
    });
}
// ajax请求数据
function productAjax() {
    var a = $("#filter"),
		listContainer = "#listContainer",
		orderBy = "orderBy=",  // 排序方式
		filter = "&isNew=0&isHot=0&isSpecial=0&isTop=0",  // 过滤方式字符串  isNew.isHot.isSpecial.isTop
		lowPrice = "",  // 过滤方式：低价位
		highPrice = "",  // 过滤方式：高价位
		page = 1,  // 翻页
		links = orderBy + filter + "&lowPrice=" + lowPrice + "&highPrice=" + highPrice;  // 最终搜索条件集合
    // 排序方式
    a.find(".d > a").click(function () {
        $(this).addClass("cur").siblings("a").removeClass("cur");
        orderBy = $(this).attr("value");
        if ($(this).hasClass("price")) {
            if (orderBy == "priceDesc") {
                $(this).attr({ "value": "priceAsc" }).html("价格<i class='asc'></i>");
            } else {
                $(this).attr({ "value": "priceDesc" }).html("价格<i class='desc'></i>");
            }
        } else {
            $(this).siblings("a").find("i").remove();
        }
        links = orderBy + filter + "&lowPrice=" + lowPrice + "&highPrice=" + highPrice;
        ajaxProData(links, listContainer, page);
    });
    // 多条件过滤结果
    a.find(".ug > a").click(function () {
        filter = ""; // 重置
        if ($(this).hasClass("cur")) {
            $(this).removeClass("cur");
        } else {
            $(this).addClass("cur");
        }
        a.find(".ug > a").each(function (index, element) {
            var name = $(this).attr("name"), values = 0;
            if ($(this).hasClass("cur")) {
                var values = 1;
            }
            filter += "&" + name + "=" + values;
        });
        links = orderBy + filter + "&lowPrice=" + lowPrice + "&highPrice=" + highPrice;
        ajaxProData(links, listContainer, page);
    });
    $("#filterByPriceSection .submit").click(function () {
        lowPrice = $("#filterByPriceSection .txt:eq(0)").val();
        highPrice = $("#filterByPriceSection .txt:eq(1)").val();
        links = orderBy + filter + "&lowPrice=" + lowPrice + "&highPrice=" + highPrice;
        ajaxProData(links, listContainer, page);
    });
    // 翻页
    a.find(".nextPage").click(function () {
        if (!$(this).hasClass("noPage")) {
            page++;
            //a.find(".currentPage").text(page);
            ajaxProData(links, listContainer, page);
            setMinPageStyle();
        }
    });
    a.find(".prevPage").click(function () {
        if (!$(this).hasClass("noPage")) {
            page--;
            //a.find(".currentPage").text(page);
            ajaxProData(links, listContainer, page);
            setMinPageStyle();
        }
    });
    ajaxProPager(links, listContainer, page);  // 直接运行
}
// ajax请求数据
function ajaxProData(links, container, page) {
    $.ajax({
        url: "/listAjax.aspx?" + links,
        data: {
            page: page
        },
        type: "GET",
        dataType: "html",
        beforeSend: function () {
            $(container).html("<li class='loading'>正在努力刷新数据...</div>");
        },
        success: function (data) {
            $(container).html(data);
            loadImgLazy();  // 惰性加载
            ueList();  // 列表交互
            ajaxProPager(links, container, page);  // 绑定翻页
            $("#filter .currentPage").text($(data).find(".currentPage").val());  // 更新筛选栏的页面数据
            $("#filter .totalPage").text($(data).find(".totalPage").val());  // 更新筛选栏的页面数据
            setMinPageStyle();  // 设置筛选栏翻页
        }
    })
}
function ajaxProPager(links, container, page) {
    $("#proPager a").click(function () {
        ajaxProData(links, container, $(this).attr("value"));
    });
}
function setMinPageStyle() {
    var a = $("#filter");
    if (a.find(".currentPage").text() == 1) {
        a.find(".prevPage").addClass("noPage");
    } else {
        a.find(".prevPage").removeClass("noPage");
    }
    if (parseInt(a.find(".totalPage").text()) == parseInt(a.find(".currentPage").text())) {
        a.find(".nextPage").addClass("noPage");
    } else {
        a.find(".nextPage").removeClass("noPage");
    }
}
// 品牌详情页
function ueDetailBrand() {
    var a = $("#detailBrand");
    // 初始化
    var N = a.find(".tod li").length / 2,
		wid = 174 + 25,  // 单体宽度
		g = 12,  //单屏个数
		j = Math.ceil(N * 2 / g),
		n = 0;  // 当前指针
    speed = 200,  // 速度
		tip = "";
    a.find(".tod ul").width(N * wid);
    // 循环
    for (var i = 0; i < j; i++) {
        if (i == 0) {
            tip += "<a href=\"javascript:;\" class=\"tip cur\"></a>";
        } else {
            tip += "<a href=\"javascript:;\" class=\"tip\"></a>";
        }
    }
    a.find(".tips").html(tip);
    // 执行
    var func = function () {
        if (n > j - 1) {
            n = j - 1;
        } else if (n < 0) {
            n = 0;
        }
        a.find(".tod ul").animate({ "margin-left": (-wid * 6 * n) }, { easing: "easeOutQuint", duration: 1000 });
        a.find(".miTip .tip").eq(n).addClass("cur").siblings().removeClass("cur");
    }
    a.find(".miTip .tip").click(function () {
        $(this).addClass("cur").siblings().removeClass("cur");
        n = $(this).index();
        func();
    });
    a.find(".miTip .l").click(function () {
        n--;
        func();
    });
    a.find(".miTip .r").click(function () {
        n++;
        func();
    });
}
// 商品详情页
function detail() {
    var a = $("#detail"), flaotProNav = $("#flaotProNav"), plans = $("#plans");
    // 监控滚动
    $(window).scroll(function () {
        if (!flaotProNav.length) { return false; }
        var scrollTop = $(document).scrollTop(),
			u = flaotProNav.offset().top;
        if (scrollTop > u) {
            flaotProNav.addClass("flaotProNavFix");
        } else {
            flaotProNav.removeClass("flaotProNavFix");
        }
    });
    // 切换（优惠套餐/关联商品）
    plans.find(".tab .item").click(function () {
        $(this).addClass("current").siblings(".item").removeClass("current");
        plans.find(".container .uis").eq($(this).index()).show().siblings(".uis").hide();
        if ($(this).index() == 0) {
            plans.find(".tab .miTip").show();
        } else {
            plans.find(".tab .miTip").hide();
        }
    });
    // 切换（商品介绍/商品评价/售后服务）
    flaotProNav.find(".main .item").click(function () {
        // alert($(this).index());
        $(this).addClass("cur").siblings(".item").removeClass("cur");
        flaotProNav.siblings(".itemTab").hide();
        if ($(this).index() == 1) {
            flaotProNav.siblings(".itemTab").show();
        } else {
            flaotProNav.siblings(".itemTab").eq($(this).index() - 1).show();
        }
        var scrollTop = flaotProNav.offset().top + 5;
        $("body, html").animate({ "scrollTop": scrollTop }, 200);
    });
    //== 实例化效果 ==//
    groupMall();  // 优惠套餐
    previewPro(); // 图片相关区域
    browsePro();  // 浏览过的产品
    buyPro();  // 购买and加入购物车
    magnifier();  // 放大镜效果
    // 
}
// 浏览过的产品
function browsePro() {
    var browsePro = $("#browsePro"),
		N = browsePro.find(".container dd").length,
		n = 0,
		hei = browsePro.find(".container dd").height();
    var func = function () {
        if (n < 0) {
            n = 0;
        } else if (n >= N - 1) {
            n = N - 2;
        }
        browsePro.find(".container dl").animate({ "margin-top": (-hei * n) }, { easing: "", duration: 500, complete: function () {
            arrow();
        }
        });
    },
	arrow = function () {
	    if (n == 0) {
	        browsePro.children(".up").animate({ "opacity": 0 }).addClass("deCursor");
	    } else {
	        browsePro.children(".up").animate({ "opacity": 1 }).removeClass("deCursor");
	    }
	    if (n == N - 2) {
	        browsePro.children(".down").animate({ "opacity": 0 }).addClass("deCursor");
	    } else {
	        browsePro.children(".down").animate({ "opacity": 1 }).removeClass("deCursor");
	    }
	}
    browsePro.children(".up").click(function () {
        n--;
        func();
    });
    browsePro.children(".down").click(function () {
        n++;
        func();
    });
    arrow();
}
// 图片相关区域
function previewPro() {
    var me = $("#previewPro"), bigImg = me.find(".bigImg"), bigImgElm = me.find(".bigImg img"),
		defLength = 5,
		N = me.find(".smallImg dd").length,
		ci = Math.ceil(N / defLength),
		n = 0,
		w = me.find("dd").width() * defLength;
    me.find(".smallImg dl").width(w * ci);
    var func = function () {
        if (n < ci - 1) {
            n++;
            me.find(".smallImg .l").removeClass("noDrop");
            me.find(".smallImg dl").animate({ "margin-left": (-w * n) }, { easing: "", duration: 300, complete: function () {
                if (n == ci - 1) {
                    me.find(".smallImg .r").addClass("noDrop");
                } else {
                    me.find(".smallImg .r").removeClass("noDrop");
                }
            }
            });
        }
    },
	func2 = function () {
	    if (n > 0) {
	        n--;
	        me.find(".smallImg .r").removeClass("noDrop");
	        me.find(".smallImg dl").animate({ "margin-left": (-w * n) }, { easing: "", duration: 300, complete: function () {
	            if (n == 0) {
	                me.find(".smallImg .l").addClass("noDrop");
	            } else {
	                me.find(".smallImg .l").removeClass("noDrop");
	            }
	        }
	        });
	    }
	}
    me.find(".smallImg .r").click(func);
    me.find(".smallImg .l").click(func2);
    // 触发效果
    me.find(".smallImg dd").hover(
		function () {
		    $(this).addClass("hover").siblings("dd").removeClass("hover");
		    bigImgElm.attr({ "src": $(this).children("img").attr("bigimg") });
		},
		function () {
		    // code
		}
	);
}
// 优惠套餐
function groupMall() {
    var a = $("#groupMall"), b = $("#plans");
    // 初始化
    var N = a.find(".other li").length,
		wid = 114 + 27,  // 单体宽度
		g = 7,  // 单屏个数
		j = Math.ceil(N / g),
		n = 0;  // 当前指针
    speed = 200,  // 速度
		tip = "";
    a.find(".other ul").width(N * wid);
    // 循环
    for (var i = 0; i < j; i++) {
        if (i == 0) {
            tip += "<a href=\"javascript:;\" class=\"tip cur\"></a>";
        } else {
            tip += "<a href=\"javascript:;\" class=\"tip\"></a>";
        }
    }
    b.find(".miTip li:eq(1)").html(tip);
    // 执行
    var func = function () {
        if (n > j - 1) {
            n = j - 1;
        } else if (n < 0) {
            n = 0;
        }
        a.find(".other ul").animate({ "margin-left": (-wid * 6 * n) }, { easing: "easeOutQuint", duration: 1000 });
        b.find(".miTip .tip").eq(n).addClass("cur").siblings().removeClass("cur");
    }
    b.find(".miTip .tip").click(function () {
        $(this).addClass("cur").siblings().removeClass("cur");
        n = $(this).index();
        func();
    });
    b.find(".miTip .l").click(function () {
        n--;
        func();
    });
    b.find(".miTip .r").click(function () {
        n++;
        func();
    });
    // 选择套餐
    a.find("li .u i").click(function () {
        if ($(this).parents("li").hasClass("cur")) {
            $(this).parents("li").removeClass("cur");
        } else {
            $(this).parents("li").addClass("cur");
        }
    });
    // 预留计算接口
}
// 购买and加入购物车
function buyPro() {
    var a = $("#buyPro"),
		colorSizeNum = ['', '', 1];  // 购买和加入购物车的参数    
    a.find(".choose_num .less").click(function () {  // 减
        if (parseInt(colorSizeNum[2]) <= 1) {
            $(this).addClass("noDrop");
        } else {
            colorSizeNum[2] = parseInt(colorSizeNum[2]) - 1;
            $(this).removeClass("noDrop");
        }
        $(this).siblings("#BuyCount").val(colorSizeNum[2]);
    });
    a.find(".choose_num .plus").click(function () {  // 增        
        if (CheckStore(parseInt(colorSizeNum[2]) + 1)) {
            colorSizeNum[2] = parseInt(colorSizeNum[2]) + 1;
            $(this).siblings(".less").removeClass("noDrop");
            $(this).siblings("#BuyCount").val(colorSizeNum[2]);
        } else {
            alert("库存不足");
        }
    });
    a.find(".choose_num .txt").keyup(function () {  // 直接输入数字
        if (/^[0-9]*[1-9][0-9]*$/.test($(this).val())) {
            if (!CheckStore(parseInt($(this).val()))) {
                $(this).val(1);
                colorSizeNum[2] = 1;
            } else {
                colorSizeNum[2] = parseInt($(this).val());
            }
        } else {
            $(this).val(1);
            colorSizeNum[2] = 1;
        }
    });
    //	a.find(".rowColor dd:eq(0), .rowSize dd:eq(0)").trigger("click");  // 默认选择第一个选项
}
// 放大镜效果
function magnifier() {
    if (!$("#imgZoom").length) { return false; }
    var a = $("#imgZoom"),  // 容器
		img_W = 700, // 图片宽度CSS有对应数据
		img_H = 700, // 图片高度CSS有对应数据
		zoom_w = 350, // 展示容器宽度
		zoom_h = 350, // 展示容器高度
		t1 = 1,  // 时间
		zoom_left = ($(window).width() - $("#wrapper").width()) / 2 + 20,
		zoom_top = a.offset().top,  // 上边距
		lens_border = 0,  // 放大镜区域边框
    // 放大镜的宽度
		lens_width = zoom_w / img_W * a.width();
    a.mouseenter(function (e) {
        a.css({ "position": "relative", "cursor": "move" });  // 设置基本样式
        var zoomLens_height = zoom_h / zoom_w * lens_width;
        var handX = e.pageX - zoom_left - 0 + lens_border;
        var handY = e.pageY - zoom_top - zoomLens_height / 2 + lens_border;
        var src = a.children("img").attr("src");
        $("<div class='zoomLens' id='zoomLens'></div>").css({ "opacity": 0.5, "width": lens_width, "height": (zoomLens_height), "left": handX, "top": handY }).appendTo(a);  // 插入鼠标焦点
        $("<div class='showZoom' id='showZoom'><img src='" + src + "' alt='' /></div>").css({ "top": -1, "left": 353, "z-index": 100 }).appendTo(a);  // 插入显示层
    }).mousemove(function (e) {
        var zoomLens_height = zoom_h / zoom_w * lens_width
        var handX = e.pageX - zoom_left - lens_width / 2;
        var handY = e.pageY - zoom_top - zoomLens_height / 2;
        handX <= 0 ? handX = 0 : handX;   // X最小极限值
        handX >= (a.width() - lens_width + lens_border) ? handX = a.width() - lens_width + lens_border : handX;   // X最大极限值
        handY <= 0 ? handY = 0 : handY;   // Y最小极限值
        handY >= (a.height() - zoomLens_height - lens_border) ? handY = a.height() - zoomLens_height - lens_border : handY;   // Y最大极限值
        $("#zoomLens").css({ "left": handX, "top": handY });  // 设置放大镜焦点当前位置
        // 设置放大镜的当前显示效果（大图显示位置）
        var perX = (handX) / a.width();
        var perY = (handY) / a.height();
        $("#showZoom img").css({ "left": -img_W * perX, "top": -img_H * perY });
    }).mouseleave(function () {
        $("#showZoom").fadeOut(t1);  // 逐渐隐藏
        $("#zoomLens").fadeOut(t1);  // 逐渐隐藏
        setTimeout(function () {
            $("#showZoom").remove();  // 删除HTML节点
            $("#zoomLens").remove();  // 删除HTML节点
        }, t1);
    });
}
// 购物车
function cart() {
    var cartContent = $("#cartContent"), cartList = $("#cartList");
    // 切换地址选择或者添加
    cartContent.find(".tabAddr .radio").change(function () {
        if (cartContent.find(".tabAddr .radio").eq(0).is(":checked")) {
            cartContent.find(".listAddr").slideDown(200).end().find(".addAddr").slideUp(200).end().find(".showAddr").slideUp(200);
        } else {
            cartContent.find(".addAddr").slideDown(200).end().find(".listAddr").slideUp(200).end().find(".showAddr").slideUp(200);
        }
    });
    // 选择地址
    cartContent.find(".listAddr .row").hover(
		function () {
		    $(this).addClass("hover");
		},
		function () {
		    $(this).removeClass("hover");
		}
	).click(function () {
	    cartContent.find(".showAddr").html($(this).html()).show().end().find(".listAddr").hide();
	});
    // 输入支付金额提示
    cartContent.find(".money").focus(function () {
        $(this).removeClass("red").removeClass("redBorder").siblings(".moneyTip").hide();
    }).blur(function () {
        if (!/^((0|([1-9][0-9]*))+(\.\d{2})?)$/.test($(this).val()) || $(this).val() == "0.00") {
            $(this).addClass("red").addClass("redBorder").siblings(".moneyTip").html("请输入合法的资金格式 例如：100 或者 0.01").show();
        } else if (parseInt($(this).val()) > parseInt($("#balance").text())) {
            $(this).addClass("red").addClass("redBorder").siblings(".moneyTip").html("账户余额不足以支付本次购物").show();
        }
    });
    // 展开发票和优惠券
    $("#paydo1, #paydo2").change(function () {
        if (this.checked) {
            $(this).parents("li").next().slideDown(100);
        } else {
            $(this).parents("li").next().slideUp(100);
        }
    });
}

//收藏产品
function collectProduct(productID) {
    var url = "/Ajax.html?Action=Collect&ProductID=" + productID; ;
    Ajax.requestURL(url, dealCollectProduct);
}
function dealCollectProduct(content) {
    if (content != "") {
        alertMessage(content);
    }
}
//添加到购物车
function addToCart(productID, productName, productStandardType, CurrentMemberPrice, obj) {

    var check = true;
    var attributeName = "";
    if (productStandardType == "1") {
        var standardValue = getTextValue(os("name", "StandardValue"));
        for (var i = 0; i < standardValue.length; i++) {
            attributeName += standardValue[i] + ",";
            if (standardValue[i] == "") {
                check = false;
                break;
            }
        }
        if (attributeName == "") {
            var oname = $(obj).attr("oname");
            attributeName = oname;
        }
    }

    if (check) {
        var buyCount = 1;
        if (Validate.isInt(buyCount) && parseInt(buyCount) > 0) {
            if (attributeName != "") {
                productName = productName + "(" + attributeName + ")";
            }
            var currentMemberPrice = CurrentMemberPrice;
            var url = "/Ajax.html?Action=AddToCart&ProductID=" + productID + "&ProductName=" + encodeURIComponent(productName) + "&BuyCount=" + buyCount + "&CurrentMemberPrice=" + currentMemberPrice;
            Ajax.requestURL(url, dealAddToCart);
        }
        else {
            alertMessage("数量填写有错误");
        }
    }
    else {
        alertMessage("请选择规格");
    }
}
function dealAddToCart(content) {
    if (content == "ok") {

        alertMessage("添加成功");
        var buyCount = 1;
        //o("ProductTotalPrice").innerHTML = parseFloat(o("ProductTotalPrice").innerHTML) + parseInt(buyCount) * parseFloat(currentMemberPrice);
        if ($("#ProductBuyCount").length > 0) $("#ProductBuyCount").text(parseInt($("#ProductBuyCount").text()) + parseInt(buyCount));
        if ($("#rightProductBuyCount").length > 0) $("#rightProductBuyCount").text($("#ProductBuyCount").text());
        //显示购物车下拉框（ajax加载最新加入的4件商品）
        $(".carbox").addClass("carhover");
        LoadNewCartProducts();
    }
    else {
        redirect = false;
        alertMessage(content);
    }
}



/*---商品筛选区域--start---*/
$(function () {
    $(".J_extMultiple").click(function () {
        resetMultiple();
        $(this).parent().parent().addClass("multiple");
    });
    $('.J_btnsCancel').click(function () {
        resetMultiple();
    });
    $('.J_valueList li a').unbind().click(function (e) {
        if ($(".sl-wrap").hasClass("multiple")) {
            toggleSelect($(this).parent());
            e.preventDefault();
        }
    });
    $('.J_btnsConfirm').click(function () {
        var qsBrand = [], qsEx = [];
        var at;
        var isBrand;
        $('.J_valueList li.selected a').each(function () {
            var href = $(this).attr("href");
            isBrand = $(this).parent().parent().hasClass('v-fixed') ? true : false;
            if (isBrand) {
                qsBrand.push(getQueryStringProduct(href, 'brand'));
            }
            else {
                if (!at) {
                    at = getQueryStringProduct(href, 'at');
                    var ats = getQueryStringProduct(href, 'at').split(';');
                    at = ats[ats.length - 1];
                }
                var exs = getQueryStringProduct(href, 'ex').split(';');
                qsEx.push(exs[exs.length - 1]);
            }
        });
        var url = document.location.search;
        if (isBrand) {
            var orgBrand = getQueryStringProduct(url, 'brand');
            url = url + '&brand=' + qsBrand.join(',');
        }
        else {
            var orgAt = getQueryStringProduct(url, 'at');
            if (orgAt == "") {
                url += '&at=' + at;
            }
            else {
                url = url.replace('&at=' + orgAt, '&at=' + orgAt + ';' + at);
            }
            var orgEx = getQueryStringProduct(url, 'ex');
            if (orgEx == "") {
                url += '&ex=' + qsEx.join(',');
            }
            else {
                url = url.replace('&ex=' + orgEx, '&ex=' + orgEx + ';' + qsEx.join(','));
            }
        }
        window.location.href = '/list.html' + url;
    });
    /*红色版多选确定*/
    $('.sure').click(function () {
        var qsBrand = [], qsEx = [];
        var at;
        var isBrand;
        $('.sl_list li.selected a').each(function () {
            var href = $(this).attr("href");        
            isBrand = $(this).parents(".pSelect").hasClass("multiple") ? true : false;
            if (isBrand) {
                qsBrand.push(getQueryStringProduct(href, 'brand'));
            }
            else {
                if (!at) {
                    at = getQueryStringProduct(href, 'at');
                    var ats = getQueryStringProduct(href, 'at').split(';');
                    at = ats[ats.length - 1];
                }
                var exs = getQueryStringProduct(href, 'ex').split(';');
                qsEx.push(exs[exs.length - 1]);
            }
        });
        var url = document.location.search;
        if (isBrand) {
            var orgBrand = getQueryStringProduct(url, 'brand');
            url = url + '&brand=' + qsBrand.join(',');
        }
        else {
            var orgAt = getQueryStringProduct(url, 'at');
            if (orgAt == "") {
                url += '&at=' + at;
            }
            else {
                url = url.replace('&at=' + orgAt, '&at=' + orgAt + ';' + at);
            }
            var orgEx = getQueryStringProduct(url, 'ex');
            if (orgEx == "") {
                url += '&ex=' + qsEx.join(',');
            }
            else {
                url = url.replace('&ex=' + orgEx, '&ex=' + orgEx + ';' + qsEx.join(','));
            }
        }
        window.location.href = '/list.html' + url;
    });
    /*红色版多选确定*/
    $('#filterByPriceSection .btn .submit').click(function () {
        var baseUrl = $(this).attr('url');
        var _min = $('#min_price').val();
        var _max = $('#max_price').val();
        if (_min >= 0) baseUrl += "&min=" + _min;
        if (_max >= 0) baseUrl += "&max=" + _max;

        window.location.href = baseUrl;
    });
});

function resetMultiple() {
    $(".J_brandSelected").hide();
    $(".brand-selected").html("");
    $("div.multiple").removeClass("multiple");
    $("li.selected").removeClass("selected");
    $(".J_btnsConfirm").addClass("disabled");
}
function toggleSelect(that) {
    $(that).toggleClass("selected");
    var id = $(that).attr("id");
    if ($(that).hasClass("selected")) {
        $(".J_btnsConfirm").removeClass("disabled");

        if ($(that).parent().hasClass('v-fixed')) {
            $('.brand-selected').append($(that).prop("outerHTML").replace("id=", "data-brand-id="));
            $(".J_brandSelected").show();
        }
    }
    else {
        $("li[data-brand-id=" + id + "]").remove();
        if ($('.J_valueList li.selected a').length == 0) {
            $(".J_brandSelected").hide();
            $(".J_btnsConfirm").addClass("disabled");
        }
    }
}