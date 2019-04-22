
//导入
function importdata(file) {
    $.ajax({
        url: '/Admin/UserBatchAdd.aspx?Action=FileUpload',
        type: 'POST',
        data: { file: file },
        dataType: "JSON",
        success: function (json) {
            if (json.flag) {
                $("#tmplRow").tmpl(json.list).appendTo('#eachRow');
            }
            else {
                layer.msg(json.msg);
            }
        }
    });
}


function addRow() {
    var data = new Object();
    data.row = document.getElementById("eachRow").rows.length;
    data.RealName = '';
    data.UserName = '';
    data.Mobile = '';
    data.Birthday = '';
    //data.Email= '';
    //data.Tel= '';
    //data.QQ = '';
    $("#tmplRow").tmpl(data).appendTo('#eachRow');
}

function delRow(obj) {
    $(obj).parent().parent().remove();

    $("#eachRow tr").each(function (i) {
        $(this).find("input[name=realname]").attr("id", "realname" + i);
        $(this).find("input[name=username]").attr("id", "username" + i);
        $(this).find("input[name=mobile]").attr("id", "mobile" + i);
        $(this).find("input[name=birthday]").attr("id", "birthday" + i);
        //$(this).find("input[name=email]").attr("id", "email" + i);
        //$(this).find("input[name=tel]").attr("id", "tel" + i);
        //$(this).find("input[name=qq]").attr("id", "qq" + i);
    });
}

$('form').validator({
    theme: 'jw_alert_set_border',
    ignoreBlank: true,
    stopOnError: true,
    rules: { 
        nameRemote: function (element) {
            return $.get("/Admin/UserBatchAdd.aspx?Action=CheckLoginName", { loginName: element.value,loginType:1 }, function (data) { });
        },
        mobileRemote: function (element) {
            return $.get("/Admin/UserBatchAdd.aspx?Action=CheckLoginName", { loginName: element.value,loginType:2 }, function (data) { });
        },
        //emailRemote: function (element) {
        //    return $.get("/Admin/UserBatchAdd.aspx?Action=CheckLoginName", { loginName: element.value, loginType: 3 }, function (data) { });
        //},
    },
    fields: {
        realname:{rule:'姓名:required;username'},
        username: { rule: '用户名:required;username;' },
        mobile: { rule: '手机:required;mobile;' },
        birthday: { rule: '生日:date' }
        //email: { rule: '邮箱:required;email;' },
        //tel: { rule: "固定电话:tel;" },
        //qq: { rule: "QQ:qq;" },
    },
    valid: function (form) {
        var len = document.getElementById("eachRow").rows.length;
        if (len < 1) {
            layer.msg('请添加至少一条记录');
            return;
        }

        /*检查用户名是否重复 start--*/
        var _name_arr = [];
        $("#eachRow input[name=username]").each(function () {
            _name_arr.push($(this).val());
        });

        //列表中是否有重复
        var isRepeat = false;
        var hash = {};
        for (var i in _name_arr) {
            if (hash[_name_arr[i]]) {
                isRepeat = true;
                break;
            }
            hash[_name_arr[i]] = true;
        }
        if (isRepeat) {
            layer.msg('列表中有重复的用户名/微信昵称');
            return;
        }
        /*检查用户名是否重复 End--*/
        /*检查手机号是否重复 start--*/
        var _mobile_arr = [];
        $("#eachRow input[name=mobile]").each(function () {
            _mobile_arr.push($(this).val());
        });

        //列表中是否有重复
        var isRepeat = false;
        var hash = {};
        for (var i in _mobile_arr) {
            if (hash[_mobile_arr[i]]) {
                isRepeat = true;
                break;
            }
            hash[_mobile_arr[i]] = true;
        }
        if (isRepeat) {
            layer.msg('列表中有重复的手机号');
            return;
        }
        /*检查手机号是否重复 End--*/
        /*检查Email是否重复 start--*/
        /*
        var _email_arr = [];
        $("#eachRow input[name=email]").each(function () {
            _email_arr.push($(this).val());
        });

        //列表中是否有重复
        var isRepeat = false;
        var hash = {};
        for (var i in _email_arr) {
            if (hash[_email_arr[i]]) {
                isRepeat = true;
                break;
            }
            hash[_email_arr[i]] = true;
        }
        if (isRepeat) {
            layer.msg('列表中有重复的邮箱');
            return;
        }

        */
        /*检查Email是否重复 End--*/
        var me = this;

        //后台匹配
        $.ajax({
            url: '/Admin/UserBatchAdd.aspx?Action=CheckLoginNames',
            type: 'GET',
            data: { userNames: _name_arr.join('|'), mobiles: _mobile_arr.join('|') },
            //data: { userNames: _name_arr.join('|'), mobiles: _mobile_arr.join('|'), emails: _email_arr.join('|') },
            dataType: 'JSON',
            success: function (json) {
                if (json.flag) {
                    me.holdSubmit();

                    $.ajax({
                        url: '/Admin/UserBatchAdd.aspx?Action=Submit',
                        type: 'POST',
                        data: $(form).serialize(),
                        success: function (result) {
                             me.holdSubmit(false);                         
                             if (result == "ok") { alert('添加成功'); window.location.href = "/Admin/User.aspx"; }
                             else if (result == "error") { layer.msg('添加失败，请稍后重试'); }
                             else { layer.msg(result); }
                        },
                        error: function (xhr) {
                            me.holdSubmit(false);

                            layer.alert(xhr.responseText, function (index) {
                                layer.close(index);
                            });
                        }
                    });
                }
                else {
                    layer.alert(json.msg);
                    return;
                }
            },
            error: function () {
                me.holdSubmit(false);
            }
        });
    }
});

//上传主图片后，触发字段执行验证
function triggerValidate(id) {
    $('#' + id).trigger('validate');
}

//对图片的验证进行特殊处理
$('#eachRow').on(
    {
        'valid.field': function () {
            $(this).parent().removeClass('jw-alert-set-border-error');
        },
        'invalid.field': function () {
            $(this).parent().addClass('jw-alert-set-border-error');
        }
    },
    'input[name=img]'
);