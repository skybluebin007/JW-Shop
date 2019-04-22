/**
Core script to handle the entire theme and core functions
**/
var Layout = function () {

    var _goUrl = function(url, index, i) {
        if (index <= 0) {
            index = layer.load(2, { time: 10 * 1000 });
        }

        var pageContentBody = $('.wrapper .page-content');
        $.ajax({
            type: "GET",
            cache: false,
            url: url,
            dataType: "html",
            success: function (res) {
                _handleGoUrl(index, i);
                pageContentBody.html(res);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                _handleGoUrl(index, i);
                pageContentBody.html('<h4>Could not load the requested content.</h4>');
            }
        });
    };

    var _handleGoUrl = function(index, i) {
        layer.close(index);
        if (i >= 0) {
            $('.menu dd').removeClass('current');
            if (i > 0) {
                $('.menu dd[data-id=' + i + ']').addClass('current');
            }
            else {
                $('.menu dd').eq(0).addClass('current');
            }
        }
    };

    // Handle sidebar menu
    var handleSidebarMenu = function () {
        //handle ajax links within navbar menu
        $('#navbar .ease a').click(function () {
            var that = $(this).parent();
            var id = $(this).attr('data-id');

            //设定最长等待10秒
            var index = layer.load(2, { time: 10 * 1000 });
            $.ajax({
                url: '/Seller/Console/GetMenuList?id=' + id,
                type: 'GET',
                dataType: "JSON",
                success: function (result) {
                    $('#menubar').removeClass('hidden');
                    $('#menu_main').empty();
                    $("#tmplMenu").tmpl(result).appendTo('#menu_main');

                    $(that).addClass('current').siblings().removeClass('current');

                    //默认跳转到第一个网址
                    var isRedirect = false;
                    for (var i in result) {
                        if (isRedirect) break;
                        for (var k in result[i].list) {
                            var url = result[i].list[k].url;
                            if ($.trim(url) != '') {
                                _goUrl(url, index, 0);
                                isRedirect = true;
                                break;
                            }
                        }
                    }
                }
            });
        });

        // handle ajax links within sidebar menu
        $('#menu_main').on('click', 'a', function (e) {
            e.preventDefault();

            var url = $(this).attr("href");
            var i = $(this).parent().attr('data-id');
            _goUrl(url, 0, i);
        });

        // handle ajax link within main content
        $('.page-content').on('click', '.ajaxify', function(e) {
            e.preventDefault();

            var url = $(this).attr("href");
            var i = $('#menu_main .ease.current').attr('data-id');
            _goUrl(url, 0, i);
        });
    };

    var loadPage = function (url) {
        _goUrl(url, 0, -1);
    };

    return {

        init: function () {
            handleSidebarMenu();
        },

        loadPage: function (url) {
            loadPage(url);
        }
    };

}();