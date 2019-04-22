

/*新增收货地址，打开编辑框*/
function displayAddress() {
    $('#address-update-id').val(0);
    $('.js-address-edit').removeClass('hidden');
};
$(function () {
	$("#invoiceBtn").click(function(){
		$(".invoiceBox").toggle();
	});
	$(".invoiceBox .tt").click(function () {
	    $(this).addClass('selected').siblings().removeClass("selected");
		var txt=$(this).text();
		$("#InvoiceContent").val(txt);
	});
    //默认第一条支付方式
    if ($('#payways .tt').length > 0) {
        $('#payways .tt').eq(0).addClass('selected');
        var firstid = $('#payways .tt').eq(0).attr("id");
        $("#pay").val(firstid);
    }
    //选择支付方式
    $('#payways .tt').click(function () {
        $(this).addClass('selected').siblings().removeClass("selected");
        $("#pay").val($(this).attr("id"));
    })
    if ($('.check_box .review .spxx .fs').length > 0) {
        $('.check_box .review .spxx .fs').eq(0).addClass('cur');
    }
    $(".check_box .review .spxx .fs").click(function () {
        $(this).addClass("cur").siblings().removeClass("cur");
    });

    /*默认选择第一条收货地址*/
    if ($('.js-address-list').children().length > 0) {
        $('.js-address-list').find(".conitem").eq(0).children().addClass('selected');
        $(".moreAddress").show();
    }
    $(".moreAddress").click(function () {
        if ($(this).find("s").hasClass("hover")) {
        	var item=$('.js-address-list').find(".selected").parent();
        	$('.js-address-list').prepend(item);
            $('.js-address-list').find(".conitem:not(:first-child)").hide();
            $(this).html("更多地址<s class='icon'></s>");
        } else {
            $('.js-address-list').find(".conitem").show();
            $(this).html("收起地址<s class='icon hover'></s>");
        }
    });
    /*多条收货地址之间滑动*/
    $('.js-address-list').on({
        mouseenter: function () { $(this).find('a').removeClass('hidden'); },
        mouseleave: function () { $(this).find('a').addClass('hidden'); }
    }, 'div');
    /*选择收货地址*/
    $('.js-address-list').on('click', '.select-address', function () {
        $('.js-address-list').children().each(function () {
            $(this).find(".js-select-address-div").removeClass('selected');
        });
        $(this).parent().parent().addClass('selected');
        $('#address_id').val($(this).parent().parent().parent().attr('data-address'));
        //alert($(this).parent().parent().parent().attr('data-address'));
        //更新配送方式
        readShippingList();
    });

    $('.js-address-list').on('click', '.js-select-address-tt', function () {
        $('.js-address-list').children().each(function () {
            $(this).find(".js-select-address-div").removeClass('selected');
        });
        $(this).parent().addClass('selected');
        $('#address_id').val($(this).parent().parent().attr('data-address'));
        //alert($(this).parent().parent().attr('data-address'));
        //更新配送方式
        readShippingList();
    });
    /*编辑收货地址*/
    $('.js-address-list').on('click', '.edit-address', function () {
        var id = $(this).parents(".conitem").attr('data-address');
        $.ajax({
            url: '/User/UserAddress.aspx?Action=Read',
            type: 'GET',
            data: { 'id': id },
            dataType: 'JSON',
            async: false,
            success: function (json) {
                if (json.result == 'error') {
                    alertMessage(json.msg);
                }
                else {
                    $('#fmAddress input[name=consignee]').val(json.consignee);
                    $('#fmAddress input[name=address]').val(json.address);
                    $('#fmAddress input[name=mobile]').val(json.mobile);
                    $('#fmAddress input[name=tel]').val(json.tel);

                    unlimitClassData = null;
                    $('.unlimit').html(json.regionId);

                    $('.js-address-edit').removeClass('hidden');
                    $('#address-update-id').val(id);
                }
            }
        });
    });
    /*删除收货地址*/
    $('.js-address-list').on('click', '.del-address', function () {
        if (confirm('确认删除？')) {
            var parent = $(this).parents(".conitem");
            $.ajax({
                url: '/User/UserAddress.aspx?Action=Delete',
                type: 'GET',
                data: { 'id': $(parent).attr('data-address') },
                success: function (result) {
                    var arr = result.split('|');
                    if (arr[0] == 'error') {
                        alertMessage(arr[1]);
                    }
                    else {
                        var isSelected = $(parent).hasClass('selected');
                        $(parent).remove();

                        //如果删除的是选中项，则选择第一个收货地址
                        if (isSelected) {
                            if ($('.js-address-list').children().length > 0) {
                                var selAdd = $('.js-address-list').children().eq(0);
                                $(selAdd).addClass('selected');
                                $('#address_id').val($(selAdd).attr('data-address'));
                                //更新配送方式
                                readShippingList();
                            }
                        }

                        if ($('.js-address-list').children().length == 0) {
                            $('.js-address-edit').removeClass('hidden');
                            $('#address_id').val(0);
                        }
                        //如果删除项是当前编辑项，则设置编辑表单为新表单
                        if ($(parent).attr('data-address') == $('#address-update-id').val()) {
                            $('#address-update-id').val(0);
                        }
                    }
                }
            });
        }
    });
    /*重置表单*/
    $('#fmAddress input[type=button]').click(function () {
        resetForm($('#fmAddress'));
        $('.js-address-edit').addClass('hidden');
    });
});
/*保存收货地址*/
$('#fmAddress').validator({
    theme: 'jw_right',
    invalidClass: "form-input-invalid",
    msgWrapper: "div",
    fields: {
        consignee: { rule: "收货人 :required;" },
        address: { rule: "详细地址 :required" },
        mobile: { rule: "手机号码 :mobile" },
        tel: { rule: "固定电话 :tel" }
    },
    msgMaker: function (opt) {
        return '<label class="' + opt.type + '" style="line-height:30px;">' + opt.msg + '</label>';
    },
    //验证成功
    valid: function (form) {
        $.ajax({
            url: '/User/UserAddress.aspx?Action=Add',
            type: 'POST',
            data: $(form).serialize(),
            dataType: 'JSON',
            async: false,
            success: function (json) {
                if (json.result == 'error') {
                    alertMessage(json.msg);
                }
                else {
                    var updateId = parseInt($('#address-update-id').val());
                    if (updateId > 0) {
                        $('.js-address-list div[data-address=' + updateId + ']').remove();
                    }

                    $('.js-address-edit').addClass('hidden');
                    resetForm($(form));
//                  $("#genAddress").tmpl(json).appendTo('.js-address-list');
                    $('.js-address-list').prepend($("#genAddress").tmpl(json));
//                  $('.js-address-list').prepend(item);

                    /*选中该收货地址*/
                    $('.js-address-list').children().each(function () {
                        $(this).find(".js-select-address-div").removeClass('selected');
                    });
                    
                    $('.js-address-list').find(".conitem").show();
                    $(".moreAddress").find("s").addClass("hover");
                    var selAdd = $('.js-address-list div[data-address=' + json.id + ']');
                    $(selAdd).find(".js-select-address-div").addClass('selected');
                    if ($(this).find("s").hasClass("hover")) {
                    	$('.js-address-list').find(".conitem").show();
                    	$(".moreAddress").find("s").addClass("hover");
                    }
                    $('#address_id').val($(selAdd).attr('data-address'));
                    //更新配送方式
                    readShippingList();
                }
            }
        });
    }
});



function resetForm(form) {
    $(form)[0].reset();
    if ($('#UnlimitClass1')) $('#UnlimitClass1').val('0');
    if ($('#UnlimitClass2')) $('#UnlimitClass2').remove();
    if ($('#UnlimitClass3')) $('#UnlimitClass3').remove();
    if ($('#UnlimitClass4')) $('#UnlimitClass4').remove();
    if ($('#UnlimitClass5')) $('#UnlimitClass5').remove();
}

/*配送方式*/
$(function () {
    //计算商品优惠
    selectProductFavor();
    //读取配送方式及计算运费
    readShippingList();  
    //读取商品优惠礼品列表
    selectGifts();

    /*使用积分*/
    $(".summary").on("click", '#M_UsePoint', function () {
        $("#usePoint .M-point-hide").toggleClass("hidden");
        $("#usePoint .M-piont-msg").addClass("hidden");
        $("#M_pointTotalDischarge").html("0.00");
        $("#M_pointToUse").val("0");
        $("#M_pointToUse").attr('_old', 0);
        $("#point").val('0');
        $("#pointMoney").val('0');
        showMoneyDetail();
    });

    $(".summary").on("keyup", '#M_pointToUse', function () {
        var _cost = $(this).val();
        if (_cost == "") {
            $("#usePoint .M-piont-msg").addClass("hidden");
            $("#M_pointTotalDischarge").html("0.00");
            $("#point").val('0');
            $(this).attr('_old', 0);
            showMoneyDetail();

            return;
        }
        if (Validate.isInt(_cost)) {
            _cost = parseInt(_cost);
            var _maxUse = $("#pointLeft").val();
            var _rate = $("#pointRate").val();
            if (_cost > _maxUse) {
                $("#u-point-msg").html("您本次最多可使用" + _maxUse + "积分");
                $("#usePoint .M-piont-msg").removeClass("hidden");
                $(this).val($(this).attr('_old'));
            } else {
                $("#usePoint .M-piont-msg").addClass("hidden");
                var _pointMoney = Math.Round(_cost * _rate / 100, 2, true);
                $("#M_pointTotalDischarge").html("-" + _pointMoney);
                $("#pointMoney").val(_pointMoney);
                $("#point").val(_cost);
                $(this).val(_cost);
                $(this).attr('_old', _cost);
                showMoneyDetail();
            }
        } else {
            $("#u-point-msg").html("积分数必须为大于等于0的整数");
            $("#usePoint .M-piont-msg").removeClass("hidden");
            $(this).val($(this).attr('_old'));
        }
    });
});
/*读取配送方式*/
function readShippingList() {
    loading("ShippingListAjax", "配送方式");

    var id = parseInt($('#address_id').val());
    if (id > 0) {
        $.ajax({
            url: '/CheckOutShippingAjax.aspx?id=' + id,
            type: 'GET',
            success: function (content) {
                var arr = content.split('||');
                $('#ShippingListAjax').html(arr[0]);
                //alert(content);
                $('#butaddress').html(arr[1]);
                //优惠活动列表展示
                $('#FavorList').html(arr[2]);
                ////获取选择的优惠活动id
                //var _favorid = $("input[name='FavorableActivity']:checked").val();
                //$('#favorid').val(_favorid);
                //计算邮费价格
                selectShipping();
            }
        });
    }
    else {
        $('#ShippingListAjax').html("");
    }
}
/*计算邮费价格,订单优惠金额*/
function selectShipping() {
    var shippingId = $("input[name='ShippingId']:checked").val();
    var addressId = $('#address_id').val();
    var favorId = $("input[name='FavorableActivity']:checked").val(); 
    if (addressId > 0) {
        $.ajax({
            url: '/CheckOutShippingAjax.aspx?Action=SelectShipping',
            type: 'GET',
            data: { shippingId: shippingId, addressId: addressId,favorId:favorId },
            success: function (result) {
                var arr = result.split('|');
                if (arr[0] != 'ok') {
                    alertMessage(arr[1]);
                }
                else {
                    $('#check_data_shippingmoney').parent().removeClass('hidden');
                    $('#check_data_shippingmoney').text('￥' + arr[1]);

                    $('#shipping_money').val(arr[1]);
                    //计算优惠金额总额
                    $('#orderfavorableMoney').val(arr[2]);                   
                    var _newFavorMoney = parseFloat($('#orderfavorableMoney').val()) + parseFloat($('#productfavorableMoney').val());
                    $("#favorableMoney").val(_newFavorMoney);
                    $("#check_data_favorablemoney").text('￥' + "-" + _newFavorMoney.toFixed(2))
                    //$('#check_data_totalmoney').text((parseFloat($('#product_money').val()) + parseFloat($('#shipping_money').val())).toFixed(2));
                    showMoneyDetail();

                }
            }
        });
    }
}
/*计算商品优惠金额*/
function selectProductFavor() {
    var _prodcutfavorid = $("input[name='ProductFavorableActivity']:checked").val();
    $.ajax({
        url: '/CheckOut.aspx?Action=SelectProductFavor',
        type: 'GET',
        data: {favorId: _prodcutfavorid },
        success: function (result) {
            var arr = result.split('|');
            if (arr[0] == 'ok') {              
                //计算优惠金额总额
                $('#productfavorableMoney').val(arr[1]);
                var _newFavorMoney = parseFloat($('#orderfavorableMoney').val()) + parseFloat($('#productfavorableMoney').val());
              
                $("#favorableMoney").val(_newFavorMoney);
                $("#check_data_favorablemoney").text('￥' + "-" + _newFavorMoney.toFixed(2))
            
               showMoneyDetail();
               selectGifts();
            }
        }
    });
}
//读取商品优惠礼品列表
function selectGifts() {
    var _prodcutfavorid = $("input[name='ProductFavorableActivity']:checked").val();
    $.ajax({
        url: '/CheckOut.aspx?Action=ReadingGifts',
        type: 'GET',
        dataType:'Json',
        data: { favorId: _prodcutfavorid },
        success: function (result) { 
            if (result.count <= 0) {
                $("#giftlist .giftList").html("");
                $("#giftlist").hide();
            }
            else {
                var _html="";
                for (var i in result.dataList) {
                    var gift = result.dataList[i];
                    if (i == 0) { _html += "<ul class='giftInfo'><li class='photo'><img src='" + gift.Photo + "' alt='" + gift.Name + "'/></li><li class='name'>" + gift.Name + "</li><li><input type='radio' name='GiftID' value='" + gift.Id + "' checked='checked'/></li></ul>"; }
                    else {
                        _html += "<ul class='giftInfo'><li class='photo'><img src='" + gift.Photo + "' alt='" + gift.Name + "'/></li><li class='name'>" + gift.Name + "</li><li><input type='radio' name='GiftID' value='" + gift.Id + "'/></li></ul>";
                    }
                }
                $("#giftlist .giftList").html(_html);
                $("#giftlist").show();
            }
        },
        error: function () {
           
        }
    });
}
/*提交订单*/
$('#fmCheckOut').validator({
    theme: 'alertMessage',
    stopOnError: true,
    fields: {
        address_id: { rule: "配送地址 :required" },
        ShippingId: { rule: "配送方式 :checked;" },
        pay: { rule: "支付方式 :required" }
    },
    //验证成功
    valid: function (form) {
        var me = this;
        me.holdSubmit();

        $.ajax({
            url: '/Checkout.aspx?Action=Submit',
            type: 'POST',
            data: $(form).serialize(),
            success: function (result) {
                me.holdSubmit(false);

                var arr = result.split('|');
                if (arr[0] == 'error') {
                    if (arr[2] == '') {
                        alertMessage(arr[1]);
                    }
                    else {
                        alertMessage(arr[1]);
                        window.location.href = arr[2];
                    }
                }
                else {
                    window.location.href = arr[2];
                }
            },
            error: function () {
                me.holdSubmit(false);
            }
        });
    }
});


//添加优惠券
function addUserCoupon() {
      var number = $("#Number").val();
    var password = $("#Password").val();
    if (number != "" && password != "") {
        var url = "/Ajax.html?Action=CheckUserCoupon&Number=" + number + "&Password=" + password + "&totalProductMoney=" + $("#product_money").val();
        Ajax.requestURL(url, dealAddUserCoupon);
    }
    else {
        alertMessage("优惠券编号和密码不能为空", 500);
    }
}
function dealAddUserCoupon(data) {
    if (data.indexOf("|") == -1) {
        alertMessage(data, 500);
    }
    else {
        var dataArray = data.split("|");
        var number = $("#Number").val();
        var _value = dataArray[0] + "|" + dataArray[1];
      
       //o("UserCoupon").options.add(new Option("编号：" + number + "（" + dataArray[1] + " 元）", dataArray[0] + "|" + dataArray[1]));
        //o("UserCoupon").value = dataArray[0] + "|" + dataArray[1];
        $("#UserCoupon").append("<option value='"+_value+"'>编号：" + number + "（" + dataArray[1] + " 元）</option>");
        $("#UserCoupon").val(_value);
      
        selectUserCoupon();
      
    }
}
//选择优惠券
function selectUserCoupon() {
    //var userCoupon = $("#UserCoupon").val();
    var userCoupon = $("#UserCoupon option:selected").val();   
    var arr = userCoupon.split("|");   
    $('#CouponMoney').parent().removeClass('hidden');
    $('#CouponMoney').text('￥' +"-"+ arr[1]);
    //$('#check_data_totalmoney').text((parseFloat($('#product_money').val()) + parseFloat($('#shipping_money').val()) - parseFloat(arr[1])).toFixed(2));
   showMoneyDetail();
}



//计算应付款金额
function showMoneyDetail() {
    
    var productMoney = parseFloat($('#product_money').val());
  
    var shippingMoney = parseFloat($('#shipping_money').val());
    if (isNaN(shippingMoney)) {
        shippingMoney = 0;
    } 
   
    var pointMoney = parseFloat($("#pointMoney").val());
  
    var userCoupon = $("#UserCoupon option:selected").val();
    var arr = userCoupon.split("|");
    var couponMoney = parseFloat(arr[1]);
 
    var favorableMoney = parseFloat($("#favorableMoney").val());
  
    if (productMoney + shippingMoney - pointMoney - couponMoney - favorableMoney >= 0) {
        $("#check_data_totalmoney").text((productMoney + shippingMoney - pointMoney - couponMoney - favorableMoney).toFixed(2));
    }
    else {
        alertMessage("金额有错误", 500);
    }
}

